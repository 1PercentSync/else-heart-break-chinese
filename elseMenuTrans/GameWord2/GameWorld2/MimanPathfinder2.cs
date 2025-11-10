using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameTypes;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200008B RID: 139
	public class MimanPathfinder2
	{
		// Token: 0x060007AC RID: 1964 RVA: 0x0002197C File Offset: 0x0001FB7C
		public MimanPathfinder2(TingRunner pTingRunner, RoomRunner pRoomRunner)
		{
			this._tingRunner = pTingRunner;
			this._roomRunner = pRoomRunner;
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x000219B4 File Offset: 0x0001FBB4
		public static RoomNetwork roomNetwork
		{
			get
			{
				return MimanPathfinder2._roomNetwork;
			}
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x000219BC File Offset: 0x0001FBBC
		public static void ClearRoomNetwork()
		{
			MimanPathfinder2._roomNetwork = null;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x000219C4 File Offset: 0x0001FBC4
		public RoomNetwork RecreateRoomNetwork()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			MimanPathfinder2._roomNetwork = new RoomNetwork();
			foreach (Room room in this._roomRunner.rooms)
			{
				foreach (Ting ting in this.GetExits(room))
				{
					HashSet<Ting> linkedExitsInOtherRooms = this.GetLinkedExitsInOtherRooms(ting);
					int group = ting.tile.group;
					RoomGroup roomGroup = new RoomGroup(room, group);
					Dictionary<RoomGroup, Ting> dictionary = null;
					if (!MimanPathfinder2._roomNetwork.linkedRoomGroups.TryGetValue(roomGroup, out dictionary))
					{
						dictionary = new Dictionary<RoomGroup, Ting>();
						MimanPathfinder2._roomNetwork.linkedRoomGroups.Add(roomGroup, dictionary);
					}
					D.isNull(dictionary, "maybeRooms is null");
					foreach (Ting ting2 in linkedExitsInOtherRooms)
					{
						D.isNull(ting2, "linking ting is null");
						dictionary[RoomGroup.FromTing(ting2)] = ting;
					}
				}
			}
			stopwatch.Stop();
			if (stopwatch.Elapsed.TotalSeconds > 0.0)
			{
				D.Log("Recreating Room Network took " + stopwatch.Elapsed.TotalSeconds + " s.");
			}
			return MimanPathfinder2._roomNetwork;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00021B80 File Offset: 0x0001FD80
		public void EnsureRoomNetwork()
		{
			if (MimanPathfinder2._roomNetwork == null)
			{
				this.RecreateRoomNetwork();
			}
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00021B94 File Offset: 0x0001FD94
		public MimanPath Search(Ting pStart, Ting pGoal)
		{
			this.EnsureRoomNetwork();
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			MimanPath mimanPath = this.AStar(pStart, pGoal);
			stopwatch.Stop();
			if (stopwatch.Elapsed.TotalSeconds > 0.009999999776482582)
			{
				D.Log(string.Concat(new object[]
				{
					"SLOW MimanPathfinder2 search from ",
					pStart.name,
					" at ",
					pStart.position,
					" to ",
					pGoal.name,
					" at ",
					pGoal.position,
					" with result: ",
					mimanPath.status,
					" took ",
					stopwatch.Elapsed.TotalSeconds,
					" s. Iterations: ",
					mimanPath.iterations
				}));
			}
			return mimanPath;
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00021C90 File Offset: 0x0001FE90
		private MimanPath AStar(Ting pStart, Ting pGoal)
		{
			if (pStart.room == pGoal.room)
			{
				return new MimanPath
				{
					status = MimanPathStatus.IN_THE_SAME_ROOM_ALREADY
				};
			}
			HashSet<RoomGroup> hashSet = new HashSet<RoomGroup>();
			RoomGroup roomGroup = RoomGroup.FromTing(pStart);
			RoomGroup roomGroup2 = RoomGroup.FromTing(pGoal);
			RoomGroupNode roomGroupNode = new RoomGroupNode
			{
				roomGroup = roomGroup,
				gscore = 0f,
				fscore = this.CostEstimate(roomGroup, roomGroup2),
				hasBeenTouched = true,
				depth = 0
			};
			List<RoomGroupNode> list = new List<RoomGroupNode> { roomGroupNode };
			int num = 0;
			while (list.Count > 0)
			{
				num++;
				if (num > 1000)
				{
					D.Log(string.Concat(new object[] { "Hit maximum iterations when doing MimanPathfinder2 search from ", pStart.position, " to ", pGoal.position }));
					return new MimanPath
					{
						status = MimanPathStatus.NO_PATH_FOUND,
						iterations = num,
						tings = new Ting[0]
					};
				}
				RoomGroupNode cheapest = this.GetCheapest(list);
				if (cheapest.prev != null)
				{
					cheapest.depth = cheapest.prev.depth + 1;
				}
				if (cheapest.roomGroup.Equals(roomGroup2))
				{
					return new MimanPath
					{
						status = MimanPathStatus.FOUND_GOAL,
						tings = this.GetListOfTingsLeadingThroughRoomGroups(cheapest),
						iterations = num
					};
				}
				list.Remove(cheapest);
				hashSet.Add(cheapest.roomGroup);
				Dictionary<RoomGroup, Ting> dictionary = null;
				if (!MimanPathfinder2._roomNetwork.linkedRoomGroups.TryGetValue(cheapest.roomGroup, out dictionary))
				{
					dictionary = new Dictionary<RoomGroup, Ting>();
				}
				List<RoomGroupNode> list2 = new List<RoomGroupNode>();
				foreach (RoomGroup roomGroup3 in dictionary.Keys)
				{
					list2.Add(new RoomGroupNode
					{
						roomGroup = roomGroup3
					});
				}
				foreach (RoomGroupNode roomGroupNode2 in list2)
				{
					float num2 = cheapest.gscore + this.ActualCost(cheapest.roomGroup, roomGroupNode2.roomGroup);
					bool flag = hashSet.Contains(roomGroupNode2.roomGroup);
					if (!flag || num2 <= roomGroupNode2.gscore)
					{
						if (!roomGroupNode2.hasBeenTouched || num2 < roomGroupNode2.gscore)
						{
							roomGroupNode2.hasBeenTouched = true;
							roomGroupNode2.prev = cheapest;
							roomGroupNode2.gscore = num2;
							roomGroupNode2.fscore = roomGroupNode2.gscore + this.CostEstimate(roomGroupNode2.roomGroup, roomGroup2);
							if (!list.Contains(roomGroupNode2))
							{
								list.Add(roomGroupNode2);
							}
						}
					}
				}
			}
			return new MimanPath
			{
				status = MimanPathStatus.NO_PATH_FOUND,
				iterations = num,
				tings = new Ting[0]
			};
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00021FEC File Offset: 0x000201EC
		private Ting[] GetListOfTingsLeadingThroughRoomGroups(RoomGroupNode current)
		{
			List<RoomGroup> list = new List<RoomGroup>();
			while (current.prev != null)
			{
				list.Add(current.roomGroup);
				current = current.prev;
			}
			list.Add(current.roomGroup);
			list.Reverse();
			string[] array = list.Select((RoomGroup r) => r.ToString()).ToArray<string>();
			List<Ting> list2 = new List<Ting>();
			for (int i = 0; i < list.Count - 1; i++)
			{
				RoomGroup roomGroup = list[i];
				RoomGroup roomGroup2 = list[i + 1];
				Dictionary<RoomGroup, Ting> dictionary = MimanPathfinder2._roomNetwork.linkedRoomGroups[roomGroup];
				Ting ting = dictionary[roomGroup2];
				list2.Add(ting);
			}
			return list2.ToArray();
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x000220C0 File Offset: 0x000202C0
		private RoomGroupNode GetCheapest(List<RoomGroupNode> pNodes)
		{
			if (pNodes.Count == 0)
			{
				throw new Exception("Can't find cheapest node in pNodes since it is empty");
			}
			RoomGroupNode roomGroupNode = pNodes[0];
			foreach (RoomGroupNode roomGroupNode2 in pNodes)
			{
				if (roomGroupNode2.gscore < roomGroupNode.gscore)
				{
					roomGroupNode = roomGroupNode2;
				}
			}
			return roomGroupNode;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00022150 File Offset: 0x00020350
		private float CostEstimate(RoomGroup pStart, RoomGroup pGoal)
		{
			if (pStart.Equals(pGoal))
			{
				return 1f;
			}
			int num = pStart.room.worldPosition.x - pGoal.room.worldPosition.x;
			int num2 = pStart.room.worldPosition.y - pGoal.room.worldPosition.y;
			return (float)(Math.Abs(num) + Math.Abs(num2));
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x000221E4 File Offset: 0x000203E4
		private float ActualCost(RoomGroup pStart, RoomGroup pGoal)
		{
			if (pStart.Equals(pGoal))
			{
				return 50f;
			}
			return 100f;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002220C File Offset: 0x0002040C
		private HashSet<Ting> GetLinkedExitsInSameRoom(Ting pTing)
		{
			D.isNull(pTing, "pTing is null in GetLinkedExitNodes");
			HashSet<Ting> hashSet = new HashSet<Ting>();
			foreach (Ting ting in this.GetExits(pTing.room))
			{
				if (this.InSameRoomAndLinked(pTing, ting))
				{
					hashSet.Add(ting);
				}
			}
			return hashSet;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00022268 File Offset: 0x00020468
		private HashSet<Ting> GetLinkedExitsInOtherRooms(Ting pTing)
		{
			HashSet<Ting> hashSet = new HashSet<Ting>();
			if (pTing is IExit)
			{
				Ting linkTarget = (pTing as IExit).GetLinkTarget();
				if (linkTarget != null)
				{
					hashSet.Add(linkTarget);
				}
				Door door = pTing as Door;
				if (door != null && door.elevatorAlternatives.Length > 0)
				{
					foreach (string text in door.elevatorAlternatives)
					{
						Ting ting = this._tingRunner.GetTing(text);
						hashSet.Add(ting);
					}
				}
			}
			return hashSet;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x000222FC File Offset: 0x000204FC
		private bool InSameRoomAndLinked(Ting a, Ting b)
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

		// Token: 0x060007BA RID: 1978 RVA: 0x00022384 File Offset: 0x00020584
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

		// Token: 0x060007BB RID: 1979 RVA: 0x000223B4 File Offset: 0x000205B4
		private Ting[] BuildExitsCacheForRoom(string pRoomName)
		{
			HashSet<Ting> hashSet = new HashSet<Ting>();
			Ting[] tingsInRoom = this._tingRunner.GetTingsInRoom(pRoomName);
			foreach (Ting ting in tingsInRoom)
			{
				if (ting is IExit)
				{
					hashSet.Add(ting);
				}
			}
			Ting[] array2 = hashSet.ToArray<Ting>();
			this._exitForRoomsCache[pRoomName] = array2;
			return array2;
		}

		// Token: 0x0400020F RID: 527
		private TingRunner _tingRunner;

		// Token: 0x04000210 RID: 528
		private RoomRunner _roomRunner;

		// Token: 0x04000211 RID: 529
		private static RoomNetwork _roomNetwork;

		// Token: 0x04000212 RID: 530
		private Dictionary<RoomGroup, Dictionary<RoomGroup, float>> _costCache = new Dictionary<RoomGroup, Dictionary<RoomGroup, float>>();

		// Token: 0x04000213 RID: 531
		private Dictionary<string, Ting[]> _exitForRoomsCache = new Dictionary<string, Ting[]>();
	}
}
