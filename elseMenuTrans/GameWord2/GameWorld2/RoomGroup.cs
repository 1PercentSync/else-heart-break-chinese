using System;
using System.Collections.Generic;
using GameTypes;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200008C RID: 140
	public struct RoomGroup
	{
		// Token: 0x060007BD RID: 1981 RVA: 0x0002242C File Offset: 0x0002062C
		public RoomGroup(Room pRoom, int pGroup)
		{
			this.room = pRoom;
			this.group = pGroup;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00022448 File Offset: 0x00020648
		private static bool FindCloseAlternativeTile(Ting pTing, out PointTileNode tile)
		{
			Room room = pTing.room;
			int x = pTing.localPoint.x;
			int y = pTing.localPoint.y;
			for (int i = 1; i < 20; i++)
			{
				PointTileNode tile2 = room.GetTile(new IntPoint(x, y + 1));
				PointTileNode tile3 = room.GetTile(new IntPoint(x + 1, y));
				PointTileNode tile4 = room.GetTile(new IntPoint(x, y - 1));
				PointTileNode tile5 = room.GetTile(new IntPoint(x - 1, y));
				List<PointTileNode> list = new List<PointTileNode> { tile2, tile3, tile4, tile5 };
				RoomGroup.Shuffle<PointTileNode>(list);
				foreach (PointTileNode pointTileNode in list)
				{
					if (pointTileNode != null && pointTileNode.group > -1)
					{
						tile = pointTileNode;
						return true;
					}
				}
			}
			tile = new PointTileNode(IntPoint.Zero, pTing.room);
			return false;
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00022594 File Offset: 0x00020794
		public static void Shuffle<T>(IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int num = RoomGroup.rng.Next(i + 1);
				T t = list[num];
				list[num] = list[i];
				list[i] = t;
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x000225E4 File Offset: 0x000207E4
		public static RoomGroup FromTing(Ting pTing)
		{
			D.isNull(pTing, "pTing is null in RoomGroup.FromTing()");
			if (pTing.tile == null)
			{
				(pTing as MimanTing).MaybeFixGroupIfOutsideIslandOfTiles();
			}
			if (pTing.tile == null)
			{
				D.Log(pTing.name + " is on null tile, will find a close alternative tile");
				PointTileNode pointTileNode;
				if (RoomGroup.FindCloseAlternativeTile(pTing, out pointTileNode))
				{
					D.Log("Will use tile " + pointTileNode + " instead");
					return new RoomGroup(pTing.room, pointTileNode.group);
				}
			}
			if (pTing.tile == null)
			{
				D.Log(pTing.name + " is still on null tile, will fail.");
				return new RoomGroup(pTing.room, -1);
			}
			return new RoomGroup(pTing.room, pTing.tile.group);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x000226AC File Offset: 0x000208AC
		public override bool Equals(object obj)
		{
			RoomGroup roomGroup = (RoomGroup)obj;
			return this.room == roomGroup.room && this.group == roomGroup.group;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x000226E4 File Offset: 0x000208E4
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000226F8 File Offset: 0x000208F8
		public override string ToString()
		{
			return string.Format("({0} #{1})", this.room.name, this.group);
		}

		// Token: 0x04000215 RID: 533
		public Room room;

		// Token: 0x04000216 RID: 534
		public int group;

		// Token: 0x04000217 RID: 535
		private static Random rng = new Random();
	}
}
