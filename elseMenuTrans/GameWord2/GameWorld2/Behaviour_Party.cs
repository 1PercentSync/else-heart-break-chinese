using System;
using GameTypes;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000039 RID: 57
	public class Behaviour_Party : TimetableBehaviour
	{
		// Token: 0x0600044A RID: 1098 RVA: 0x0001589C File Offset: 0x00013A9C
		public Behaviour_Party(string pRoomName)
		{
			this._roomName = pRoomName;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000158AC File Offset: 0x00013AAC
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			bool flag = pCharacter.room.name == this._roomName;
			if (pCharacter.busy || pCharacter.talking)
			{
				return 1f;
			}
			bool flag2 = pCharacter.handItem is Drink;
			if (!flag)
			{
				this._bartender = null;
				this._dancePoint = null;
				InteractionHelper.GoToRoom(pCharacter, pRoomRunner, this._roomName, pTingRunner);
				return 15f;
			}
			if (!flag2)
			{
				if (this._bartender == null)
				{
					this._bartender = Behaviour_Party.GetBartender(pCharacter, pTingRunner);
				}
				if (this._bartender == null)
				{
					pCharacter.handItem = Behaviour_Sell.CreateTingToSell(pCharacter, pTingRunner, "Beer", pWorldSettings);
					return 2f;
				}
				if (this._bartender.IsIdle() && (this._bartender.conversationTarget == null || !MimanGrimmApiDefinitions.AreTingsWithinDistance(pCharacter, this._bartender, 7)))
				{
					pCharacter.WalkToTingAndInteract(this._bartender);
					return 0.5f;
				}
				pCharacter.CancelWalking();
				return 0.5f;
			}
			else
			{
				this._bartender = null;
				if (this._dancePoint != null)
				{
					Ting[] occupants = this._dancePoint.tile.GetOccupants();
					foreach (Ting ting in occupants)
					{
						if (ting is Character && ting != pCharacter)
						{
							return this.GoSit(pCharacter, pTingRunner, pRoomRunner);
						}
					}
					if (pCharacter.tile != null)
					{
						Ting[] occupants2 = pCharacter.tile.GetOccupants();
						foreach (Ting ting2 in occupants2)
						{
							if (ting2 is Character && ting2 != pCharacter)
							{
								return this.GoSit(pCharacter, pTingRunner, pRoomRunner);
							}
						}
					}
					if (!MimanGrimmApiDefinitions.AreTingsWithinDistance(pCharacter, this._dancePoint, 8))
					{
						return this.GoSit(pCharacter, pTingRunner, pRoomRunner);
					}
					if (Randomizer.OneIn(2) || pCharacter.handItem == null)
					{
						pCharacter.StartAction("Dancing", null, 99999f, 99999f);
						return (float)Randomizer.GetIntValue(5, 30);
					}
					if (pCharacter.handItem is Drink && (pCharacter.handItem as Drink).amount > 30f)
					{
						pCharacter.CancelWalking();
						pCharacter.InteractWith(pCharacter.handItem);
						return 3f;
					}
					pCharacter.PutHandItemIntoInventory();
					this._dancePoint = null;
					return 3f;
				}
				else
				{
					if (!pCharacter.sitting)
					{
						return this.GoSit(pCharacter, pTingRunner, pRoomRunner);
					}
					if (Randomizer.OneIn(3))
					{
						return this.GoDancing(pCharacter, pRoomRunner);
					}
					return 1f;
				}
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00015B4C File Offset: 0x00013D4C
		private float GoSit(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner)
		{
			Seat randomTing = InteractionHelper.GetRandomTing<Seat>(pTingRunner, pCharacter.room.name, pCharacter.tileGroup);
			if (randomTing != null && !randomTing.isBeingUsed && Randomizer.OneIn(2))
			{
				pCharacter.WalkToTingAndInteract(randomTing);
				return (float)Randomizer.GetIntValue(5, 10);
			}
			return this.GoDancing(pCharacter, pRoomRunner);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00015BA8 File Offset: 0x00013DA8
		private float GoDancing(Character pCharacter, RoomRunner pRoomRunner)
		{
			this._dancePoint = InteractionHelper.GetRandomTingWhere<Point>(pRoomRunner, pCharacter.room.name, (Point t) => t.name.ToLower().Contains("dance"));
			if (this._dancePoint != null)
			{
				pCharacter.WalkTo(this._dancePoint.position);
				return 3f;
			}
			return 3f;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00015C10 File Offset: 0x00013E10
		public static Character GetBartender(Character pCharacter, MimanTingRunner pTingRunner)
		{
			foreach (Character character in pTingRunner.GetTingsOfTypeInRoom<Character>(pCharacter.room.name))
			{
				if (character.IsAtTimetableTaskOfType(typeof(Behaviour_Sell)))
				{
					return character;
				}
			}
			return null;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00015C60 File Offset: 0x00013E60
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return pCharacter.room.name == this._roomName;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00015C78 File Offset: 0x00013E78
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
			if (pCharacter.handItem is Drink)
			{
				pCharacter.MoveHandItemToInventory();
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00015C94 File Offset: 0x00013E94
		public override string ToString()
		{
			return string.Format("[Party] {0}", this._roomName);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00015CA8 File Offset: 0x00013EA8
		public void Reset()
		{
		}

		// Token: 0x0400010B RID: 267
		private string _roomName;

		// Token: 0x0400010C RID: 268
		private Character _bartender;

		// Token: 0x0400010D RID: 269
		private Point _dancePoint;
	}
}
