using System;
using System.Collections.Generic;
using GameTypes;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000068 RID: 104
	public class MimanPathfinder_DEPRECATED
	{
		// Token: 0x06000632 RID: 1586 RVA: 0x0001CE90 File Offset: 0x0001B090
		public MimanPathfinder_DEPRECATED(TingRunner pTingRunner)
		{
			this._tingRunner = pTingRunner;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001CEB8 File Offset: 0x0001B0B8
		private Ting[] BuildExitsCacheForRoom(string pRoomName)
		{
			List<Ting> list = new List<Ting>();
			Ting[] tingsInRoom = this._tingRunner.GetTingsInRoom(pRoomName);
			foreach (Ting ting in tingsInRoom)
			{
				if (ting is IExit)
				{
					list.Add(ting);
				}
			}
			Ting[] array2 = list.ToArray();
			this._exitForRoomsCache[pRoomName] = array2;
			return array2;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001CF20 File Offset: 0x0001B120
		private Ting[] GetExits(Room pRoom)
		{
			string name = pRoom.name;
			Ting[] array;
			if (this._exitForRoomsCache.TryGetValue(name, out array))
			{
				return array;
			}
			return this.BuildExitsCacheForRoom(name);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001CF50 File Offset: 0x0001B150
		public MimanPath Search(Ting pStart, Ting pGoal)
		{
			return this.AStar(pStart, pGoal);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001CF68 File Offset: 0x0001B168
		private MimanPath AStar(Ting pStart, Ting pGoal)
		{
			if (pStart.room == pGoal.room)
			{
				return new MimanPath
				{
					status = MimanPathStatus.IN_THE_SAME_ROOM_ALREADY
				};
			}
			HashSet<Ting> hashSet = new HashSet<Ting>();
			Node node = new Node
			{
				ting = pStart,
				gscore = 0f,
				fscore = this.CostEstimate(pStart, pGoal),
				hasBeenTouched = true,
				depth = 0
			};
			List<Node> list = new List<Node> { node };
			int num = 0;
			while (list.Count > 0)
			{
				num++;
				Node cheapest = this.GetCheapest(list);
				if (cheapest.prev != null)
				{
					cheapest.depth = cheapest.prev.depth + 1;
				}
				if (cheapest.ting.room == pGoal.room)
				{
					return new MimanPath
					{
						status = MimanPathStatus.FOUND_GOAL,
						tings = this.SearchBackwardsForTingsOnTheWay(cheapest),
						iterations = num
					};
				}
				list.Remove(cheapest);
				hashSet.Add(cheapest.ting);
				HashSet<Node> linkedExitNodes = this.GetLinkedExitNodes(cheapest.ting);
				foreach (Node node2 in linkedExitNodes)
				{
					float num2 = cheapest.gscore + this.ActualCost(cheapest.ting, node2.ting);
					bool flag = hashSet.Contains(node2.ting);
					if (!flag || num2 <= node2.gscore)
					{
						if (!node2.hasBeenTouched || num2 < node2.gscore)
						{
							node2.hasBeenTouched = true;
							node2.prev = cheapest;
							node2.gscore = num2;
							node2.fscore = node2.gscore + this.CostEstimate(node2.ting, pGoal);
							if (!list.Contains(node2))
							{
								list.Add(node2);
							}
						}
					}
				}
			}
			return new MimanPath
			{
				status = MimanPathStatus.NO_PATH_FOUND
			};
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001D19C File Offset: 0x0001B39C
		private Ting[] SearchBackwardsForTingsOnTheWay(Node pFinalNode)
		{
			List<Ting> list = new List<Ting>();
			Node node = pFinalNode;
			while (node.prev != null)
			{
				list.Add(node.ting);
				node = node.prev;
			}
			list.Reverse();
			List<Ting> list2 = new List<Ting>();
			Ting ting = null;
			foreach (Ting ting2 in list)
			{
				if (ting == null || (ting as IExit).GetLinkTarget() != ting2)
				{
					list2.Add(ting2);
				}
				ting = ting2;
			}
			return list2.ToArray();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001D264 File Offset: 0x0001B464
		private bool LinkAllowed(Ting a, Ting b)
		{
			if (a.tile == null || b.tile == null)
			{
				return false;
			}
			int group = a.tile.group;
			int group2 = b.tile.group;
			if (group == -1)
			{
				D.Log("Found tile " + a.tile + " with group -1");
			}
			else if (group2 == -1)
			{
				D.Log("Found tile " + b.tile + " with group -1");
			}
			return group == group2;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001D2EC File Offset: 0x0001B4EC
		private HashSet<Node> GetLinkedExitNodes(Ting pTing)
		{
			HashSet<Node> hashSet = new HashSet<Node>();
			foreach (Ting ting in this.GetExits(pTing.room))
			{
				if (this.LinkAllowed(pTing, ting))
				{
					hashSet.Add(new Node
					{
						ting = ting
					});
				}
			}
			if (pTing is IExit)
			{
				Ting linkTarget = (pTing as IExit).GetLinkTarget();
				if (linkTarget != null)
				{
					if (!this.IsTargetDoorInABusyElevator(linkTarget as Door))
					{
						hashSet.Add(new Node
						{
							ting = linkTarget
						});
					}
				}
				Door door = pTing as Door;
				if (door != null && door.elevatorAlternatives.Length > 0)
				{
					foreach (string text in door.elevatorAlternatives)
					{
						Ting ting2 = this._tingRunner.GetTing(text);
						hashSet.Add(new Node
						{
							ting = ting2
						});
					}
				}
			}
			return hashSet;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001D408 File Offset: 0x0001B608
		private bool IsTargetDoorInABusyElevator(Door pTargetDoor)
		{
			return pTargetDoor != null && pTargetDoor.elevatorAlternatives.Length > 0 && pTargetDoor.room.GetTingsOfType<Character>().Count > 0;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001D440 File Offset: 0x0001B640
		private Node GetCheapest(List<Node> pNodes)
		{
			if (pNodes.Count == 0)
			{
				throw new Exception("Can't find cheapest node in pNodes since it is empty");
			}
			Node node = pNodes[0];
			foreach (Node node2 in pNodes)
			{
				if (node2.gscore < node.gscore)
				{
					node = node2;
				}
			}
			return node;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001D4D0 File Offset: 0x0001B6D0
		private float CostEstimate(Ting pStart, Ting pGoal)
		{
			IExit exit = pStart as IExit;
			if (exit != null && exit.GetLinkTarget() == pGoal)
			{
				return 0f;
			}
			int num = pStart.worldPoint.x - pGoal.worldPoint.x;
			int num2 = pStart.worldPoint.y - pGoal.worldPoint.y;
			return (float)(Math.Abs(num) + Math.Abs(num2));
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001D54C File Offset: 0x0001B74C
		private float ActualCost(Ting pStart, Ting pGoal)
		{
			IExit exit = pStart as IExit;
			if (exit != null && exit.GetLinkTarget() == pGoal)
			{
				return 0f;
			}
			Dictionary<Ting, float> dictionary;
			float num;
			if (this._costCache.TryGetValue(pStart, out dictionary) && dictionary.TryGetValue(pGoal, out num))
			{
				return num;
			}
			float num2 = ((pStart.room != pGoal.room) ? 1f : 0.1f);
			if (dictionary == null)
			{
				dictionary = new Dictionary<Ting, float>();
				this._costCache.Add(pStart, dictionary);
			}
			dictionary[pGoal] = num2;
			return num2;
		}

		// Token: 0x0400019A RID: 410
		private TingRunner _tingRunner;

		// Token: 0x0400019B RID: 411
		private Dictionary<string, Ting[]> _exitForRoomsCache = new Dictionary<string, Ting[]>();

		// Token: 0x0400019C RID: 412
		private Dictionary<Ting, Dictionary<Ting, float>> _costCache = new Dictionary<Ting, Dictionary<Ting, float>>();
	}
}
