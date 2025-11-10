using System;
using System.Collections.Generic;
using System.Text;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200008D RID: 141
	public class RoomNetwork
	{
		// Token: 0x060007C6 RID: 1990 RVA: 0x0002273C File Offset: 0x0002093C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (RoomGroup roomGroup in this.linkedRoomGroups.Keys)
			{
				stringBuilder.Append(roomGroup + " => ");
				Dictionary<RoomGroup, Ting> dictionary = this.linkedRoomGroups[roomGroup];
				foreach (RoomGroup roomGroup2 in dictionary.Keys)
				{
					stringBuilder.Append(string.Concat(new object[]
					{
						roomGroup2,
						" (via ",
						dictionary[roomGroup2],
						"), "
					}));
				}
				stringBuilder.Append("\n");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000218 RID: 536
		public Dictionary<RoomGroup, Dictionary<RoomGroup, Ting>> linkedRoomGroups = new Dictionary<RoomGroup, Dictionary<RoomGroup, Ting>>();
	}
}
