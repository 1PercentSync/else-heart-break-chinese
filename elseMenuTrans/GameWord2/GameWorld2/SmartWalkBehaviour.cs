using System;
using System.Collections.Generic;
using GameTypes;
using Pathfinding;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200006F RID: 111
	public class SmartWalkBehaviour
	{
		// Token: 0x06000646 RID: 1606 RVA: 0x0001D784 File Offset: 0x0001B984
		public SmartWalkBehaviour(Character pCharacter, RoomRunner pRoomRunner, TingRunner pTingRunner, WorldSettings pWorldSettings)
		{
			this._character = pCharacter;
			this._roomRunner = pRoomRunner;
			this._tingRunner = pTingRunner;
			this._worldSettings = pWorldSettings;
			this._mimanPathFinder = new MimanPathfinder2(this._tingRunner, this._roomRunner);
			this.CalculateFinalTargetPosition();
			bool flag = this.RefreshPaths();
			if (flag)
			{
				this._character.StartAction("Walking", null, 99999f, 99999f);
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001D8A0 File Offset: 0x0001BAA0
		private void CalculateFinalTargetPosition()
		{
			if (this._character.walkMode == Character.WalkMode.WALK_TO_TING_AND_HACK || this._character.walkMode == Character.WalkMode.WALK_TO_TING_AND_INTERACT || this._character.walkMode == Character.WalkMode.WALK_TO_TING_AND_USE_HAND_ITEM)
			{
				this._character.finalTargetPosition = this._character.finalTargetTing.position;
			}
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001D900 File Offset: 0x0001BB00
		public void Update(float dt)
		{
			if (this._tilePath.status == PathStatus.DESTINATION_UNREACHABLE)
			{
				return;
			}
			if (this._mimanPath.status == MimanPathStatus.NO_PATH_FOUND)
			{
				return;
			}
			if (this._character.actionName == "Walking")
			{
				this.Move(dt);
			}
			else if (this._character.actionName == "")
			{
				this.StartWalkingAgain();
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001D978 File Offset: 0x0001BB78
		public void StartWalkingAgain()
		{
			bool flag = this.RefreshPaths();
			if (flag)
			{
				this._character.StartAction("Walking", this._character.actionOtherObject, 99999f, 99999f);
				this._character.walkTimer = 0f;
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001D9CC File Offset: 0x0001BBCC
		private bool FastForwardWalkIteratorToCharacterPosition()
		{
			while (this._character.walkIterator <= this._tilePath.nodes.Length - 1)
			{
				PointTileNode pointTileNode = this._tilePath.nodes[this._character.walkIterator];
				if (pointTileNode.position == this._character.position)
				{
					return true;
				}
				this._character.walkIterator++;
			}
			return false;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001DA48 File Offset: 0x0001BC48
		private void Move(float dt)
		{
			if (this._tilePath.status == PathStatus.DESTINATION_UNREACHABLE)
			{
				return;
			}
			this._character.walkTimer += this._character.calculateFinalWalkSpeed() * dt;
			if (this._character.walkTimer >= 1f)
			{
				this._character.walkTimer = 0f;
				this._character.walkIterator++;
				if (this._tilePath.nodes == null)
				{
					this._character.CancelWalking();
					return;
				}
				if (this._character.walkIterator > this._tilePath.nodes.Length - 1)
				{
					this._character.CancelWalking();
					return;
				}
				PointTileNode pointTileNode = this._tilePath.nodes[this._character.walkIterator];
				if (this._character.walkIterator > this._tilePath.nodes.Length - 7)
				{
					PointTileNode pointTileNode2 = this._tilePath.nodes[this._tilePath.nodes.Length - 1];
					PointTileNode tile = this._character.room.GetTile(pointTileNode2.position.localPosition);
					if (tile.HasOccupants<Character>(this._character))
					{
						this._character.CancelWalking();
						this._character.timetableTimer = 1f;
						return;
					}
				}
				this._character.direction = (pointTileNode.localPoint - this._character.localPoint).ToDirection();
				this._character.position = pointTileNode.position;
				this.AnalyzeNewTile();
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001DBE8 File Offset: 0x0001BDE8
		public void AnalyzeNewTile()
		{
			if (this._character.position == this._character.finalTargetPosition)
			{
				this.ReachedFinalPosition();
			}
			else if (this._character.position == this._character.targetPositionInRoom)
			{
				if (this._mimanPath.tings.Length != 0)
				{
					this._character.walkIterator = 0;
					this._character.targetPositionInRoom = WorldCoordinate.NONE;
					Ting ting = this._mimanPath.tings[0];
					if (ting is Door && (ting as Door).isLocked)
					{
						this.UseKeyToGetThroughDoor(ting as Door);
					}
					else if (this._character.handItem is Key)
					{
						this._character.PutHandItemIntoInventory();
					}
					else
					{
						this._character.InteractWith(ting);
					}
				}
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001DCE4 File Offset: 0x0001BEE4
		public void UseKeyToGetThroughDoor(Door pDoor)
		{
			Key key = null;
			List<Key> list = new List<Key>();
			foreach (Ting ting in this._character.inventoryItems)
			{
				if (ting is Key)
				{
					list.Add(ting as Key);
				}
			}
			if (this._character.handItem is Key)
			{
				list.Add(this._character.handItem as Key);
			}
			if (list.Count > 0)
			{
				int intValue = Randomizer.GetIntValue(0, list.Count);
				key = list[intValue];
			}
			if (key == null)
			{
				D.Log(string.Concat(new object[]
				{
					"No key found for character ",
					this._character.name,
					" at door ",
					pDoor
				}));
				this._character.CancelWalking();
				return;
			}
			if (this._character.handItem != key)
			{
				this._character.MoveHandItemToInventory();
				this._character.handItem = key;
			}
			this._character.UseHandItemToInteractWith(pDoor);
			pDoor.autoLockTimer = 4f;
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001DE0C File Offset: 0x0001C00C
		private bool RefreshPaths()
		{
			if (this._character.finalTargetPosition == WorldCoordinate.NONE)
			{
				this._worldSettings.Notify(this._character.name, "Can't walk there");
				return false;
			}
			if (this._character.room.name == this._character.finalTargetPosition.roomName)
			{
				return this.TilePathfindToTargetPositionInRoom();
			}
			bool flag = this.MimanPathfindToTargetRoom();
			return flag && this.TilePathfindToTargetPositionInRoom();
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001DE9C File Offset: 0x0001C09C
		private bool TilePathfindToTargetPositionInRoom()
		{
			this._tilePath = Path.EMPTY;
			this._startTileNode = this._character.tile;
			if (this._startTileNode == null)
			{
				this.TryToFindAlternativeStartingTile();
				if (this._startTileNode != null)
				{
					this._character.StartAction("", null, 0f, 0f);
					this._character.position = this._startTileNode.position;
				}
			}
			Room room = this._roomRunner.GetRoom(this._character.finalTargetPosition.roomName);
			if (this._character.room == room)
			{
				if (this._character.finalTargetTing != null)
				{
					bool flag = this._character.finalTargetTing is Door || this._character.finalTargetTing is Portal;
					IntPoint intPoint;
					if (this._character.finalTargetTing.interactionPointsTryTheseFirst != null && SmartWalkBehaviour.GetClosestInteractionPoint(this._roomRunner, room, this._character.tile, this._character.finalTargetTing.interactionPointsTryTheseFirst, out intPoint, this._character, flag))
					{
						this._character.finalTargetPosition = new WorldCoordinate(this._character.finalTargetPosition.roomName, intPoint);
					}
					else
					{
						if (!SmartWalkBehaviour.GetClosestInteractionPoint(this._roomRunner, room, this._character.tile, this._character.finalTargetTing.interactionPoints, out intPoint, this._character, flag))
						{
							this._character.CancelWalking();
							this._character.timetableTimer = Randomizer.GetValue(5f, 10f);
							return false;
						}
						this._character.finalTargetPosition = new WorldCoordinate(this._character.finalTargetPosition.roomName, intPoint);
					}
				}
				this._character.targetPositionInRoom = this._character.finalTargetPosition;
			}
			this._goalTileNode = this._character.room.GetTile(this._character.targetPositionInRoom.localPosition);
			if (this._goalTileNode == null)
			{
				this.TryToFindAlternativeGoalTile();
			}
			if (this._goalTileNode == null)
			{
				return false;
			}
			if (this._startTileNode.group > -1 && this._goalTileNode.group > -1 && this._startTileNode.group != this._goalTileNode.group)
			{
				this._character.CancelWalking();
				this._character.timetableTimer = Randomizer.GetValue(5f, 10f);
				return false;
			}
			this._character.room.Reset();
			this._tilePath = SmartWalkBehaviour._tilePathSolver.FindPath(this._startTileNode, this._goalTileNode, this._roomRunner, false);
			foreach (PointTileNode pointTileNode in this._tilePath.nodes)
			{
				Fence occupantOfType = pointTileNode.GetOccupantOfType<Fence>();
				if (occupantOfType != null)
				{
					this._character.CancelWalking();
					this._character.WalkToTingAndInteract(occupantOfType);
					return false;
				}
			}
			if (this._tilePath.status == PathStatus.DESTINATION_UNREACHABLE)
			{
				return false;
			}
			if (this._tilePath.status == PathStatus.ALREADY_THERE)
			{
				this.AnalyzeNewTile();
				return false;
			}
			return this._tilePath.status == PathStatus.FOUND_GOAL;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001E210 File Offset: 0x0001C410
		private void TryToFindAlternativeStartingTile()
		{
			this._character.logger.Log(this._character.name + " is not on a tile, will look for an alternative starting tile...");
			PointTileNode pointTileNode = this._character.room.FindClosestTile(this._character.localPoint);
			if (pointTileNode != null)
			{
				this._startTileNode = pointTileNode;
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001E270 File Offset: 0x0001C470
		private void TryToFindAlternativeGoalTile()
		{
			PointTileNode pointTileNode = this._character.room.FindClosestFreeTile(this._character.targetPositionInRoom.localPosition, this._character.tileGroup);
			if (pointTileNode != null)
			{
				this._goalTileNode = pointTileNode;
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001E2C0 File Offset: 0x0001C4C0
		private bool MimanPathfindToTargetRoom()
		{
			Ting character = this._character;
			Ting ting;
			if (this._character.finalTargetTing != null)
			{
				ting = this._character.finalTargetTing;
			}
			else
			{
				string roomName = this._character.finalTargetPosition.roomName;
				if (roomName == "undefined_room")
				{
					return false;
				}
				Ting[] tingsInRoom = this._tingRunner.GetTingsInRoom(roomName);
				if (tingsInRoom.Length == 0)
				{
					D.Log(string.Concat(new object[] { this._character, ": No tings in final room ", roomName, ", can't do room pathfinding to there!" }));
					return false;
				}
				Ting ting2 = tingsInRoom[0];
				ting = ting2;
			}
			if (character == null)
			{
				return false;
			}
			if (ting == null)
			{
				return false;
			}
			this._mimanPath = this._mimanPathFinder.Search(character, ting);
			if (this._mimanPath.status == MimanPathStatus.FOUND_GOAL)
			{
				if (this._mimanPath.tings == null)
				{
					throw new Exception("tings == null in _mimanPath!");
				}
				if (this._mimanPath.tings.Length == 0)
				{
					throw new Exception("No tings in _mimanPath!");
				}
				Ting ting3 = this._mimanPath.tings[0];
				D.isNull(ting3, "firstTingToInteractWith is null");
				this._character.targetPositionInRoom = new WorldCoordinate(ting3.room.name, ting3.interactionPoints[0]);
				return true;
			}
			else
			{
				if (this._mimanPath.status == MimanPathStatus.IN_THE_SAME_ROOM_ALREADY)
				{
					return true;
				}
				if (this._mimanPath.status == MimanPathStatus.NO_PATH_FOUND)
				{
					this._character.CancelWalking();
					return false;
				}
				throw new Exception("Failed to find matching case");
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001E464 File Offset: 0x0001C664
		private void ReachedFinalPosition()
		{
			if (this._character.walkMode == Character.WalkMode.WALK_TO_POINT)
			{
				this._character.StopAction();
			}
			else if (this._character.walkMode == Character.WalkMode.WALK_TO_TING_AND_INTERACT)
			{
				if (this._character.finalTargetTing != null)
				{
					if (!this.CharacterIsAtInteractionPointOfFinalTargetTing())
					{
						if (this._character.timetable != null)
						{
							D.Log(this._character + " is at interaction point of his/her finalTargetTing, will reset the timetable timer to zero, has timetable: " + this._character.timetable.name);
							this._character.CancelWalking();
							this._character.timetableTimer = 0f;
						}
						else
						{
							this._character.WalkToTingAndInteract(this._character.finalTargetTing);
						}
						return;
					}
					if (this._character.finalTargetTing.canBePickedUp)
					{
						this._character.PickUp(this._character.finalTargetTing);
					}
					else if (this._character.CanInteractWith(this._character.finalTargetTing))
					{
						this._character.InteractWith(this._character.finalTargetTing);
					}
					else
					{
						SmartWalkBehaviour.s_logger.Log(this._character.name + " can't interact with " + this._character.finalTargetTing.name);
					}
				}
			}
			else if (this._character.walkMode == Character.WalkMode.WALK_TO_TING_AND_HACK)
			{
				if (this._character.finalTargetTing != null)
				{
					if (!this.CharacterIsAtInteractionPointOfFinalTargetTing())
					{
						this._character.WalkToTingAndHack(this._character.finalTargetTing as MimanTing);
					}
					else
					{
						MimanTing mimanTing = this._character.finalTargetTing as MimanTing;
						D.isNull(mimanTing);
						this._character.Hack(mimanTing);
					}
				}
			}
			else if (this._character.walkMode == Character.WalkMode.WALK_TO_TING_AND_USE_HAND_ITEM)
			{
				MimanTing mimanTing2 = this._character.finalTargetTing as MimanTing;
				D.isNull(mimanTing2);
				this._character.UseHandItemToInteractWith(mimanTing2);
			}
			this._character.ClearWalkingData();
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001E684 File Offset: 0x0001C884
		private bool CharacterIsAtInteractionPointOfFinalTargetTing()
		{
			if (this._character.finalTargetTing.room != this._character.room)
			{
				return false;
			}
			foreach (IntPoint intPoint in this._character.finalTargetTing.interactionPoints)
			{
				if (intPoint == this._character.localPoint)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001E700 File Offset: 0x0001C900
		private static bool GetClosestInteractionPoint(RoomRunner pRoomRunner, Room pRoom, PointTileNode pStartTile, IntPoint[] pPossiblePoints, out IntPoint closestPoint, Character pCharacter, bool pIgnoreCharacters)
		{
			D.isNull(pRoom, "pRoom is null");
			D.isNull(pPossiblePoints, "possiblePoints is null");
			if (pRoom != pCharacter.room)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Error for ",
					pCharacter.name,
					"! Can only pathfind to closest interaction point in the same room: ",
					pCharacter.room.name,
					", tried to do it in: ",
					pRoom.name
				}));
			}
			closestPoint = IntPoint.Zero;
			float num = float.MaxValue;
			bool flag = false;
			foreach (IntPoint intPoint in pPossiblePoints)
			{
				PointTileNode tile = pRoom.GetTile(intPoint);
				if (tile != null)
				{
					Type[] array = SmartWalkBehaviour.notTrueObstacles;
					if (pIgnoreCharacters)
					{
						array = SmartWalkBehaviour.notTrueObstaclesIncludingCharacters;
					}
					if (!tile.HasOccupantsButIgnoreSomeTypes(array))
					{
						pRoom.Reset();
						Path path = SmartWalkBehaviour._tilePathSolver.FindPath(pStartTile, tile, pRoomRunner, false);
						D.isNull(path, "path is null");
						if ((path.status == PathStatus.FOUND_GOAL || path.status == PathStatus.ALREADY_THERE) && path.pathLength < num)
						{
							closestPoint = intPoint;
							num = path.pathLength;
							flag = true;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x040001AD RID: 429
		public static Logger s_logger = new Logger();

		// Token: 0x040001AE RID: 430
		private static PathSolver _tilePathSolver = new PathSolver();

		// Token: 0x040001AF RID: 431
		private MimanPathfinder2 _mimanPathFinder;

		// Token: 0x040001B0 RID: 432
		private Character _character;

		// Token: 0x040001B1 RID: 433
		private RoomRunner _roomRunner;

		// Token: 0x040001B2 RID: 434
		private TingRunner _tingRunner;

		// Token: 0x040001B3 RID: 435
		private WorldSettings _worldSettings;

		// Token: 0x040001B4 RID: 436
		private Path _tilePath;

		// Token: 0x040001B5 RID: 437
		private PointTileNode _startTileNode;

		// Token: 0x040001B6 RID: 438
		private PointTileNode _goalTileNode;

		// Token: 0x040001B7 RID: 439
		private MimanPath _mimanPath = new MimanPath();

		// Token: 0x040001B8 RID: 440
		private static Type[] notTrueObstacles = new Type[]
		{
			typeof(Door),
			typeof(Portal),
			typeof(Point)
		};

		// Token: 0x040001B9 RID: 441
		private static Type[] notTrueObstaclesIncludingCharacters = new Type[]
		{
			typeof(Door),
			typeof(Portal),
			typeof(Point),
			typeof(Character)
		};
	}
}
