using System;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000069 RID: 105
	public class Node
	{
		// Token: 0x0600063F RID: 1599 RVA: 0x0001D5F0 File Offset: 0x0001B7F0
		public override string ToString()
		{
			return string.Format("[Node] " + this.ting.name, new object[0]);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001D620 File Offset: 0x0001B820
		public override int GetHashCode()
		{
			return this.ting.worldPoint.GetHashCode();
		}

		// Token: 0x0400019D RID: 413
		public Ting ting;

		// Token: 0x0400019E RID: 414
		public float gscore;

		// Token: 0x0400019F RID: 415
		public float fscore;

		// Token: 0x040001A0 RID: 416
		public Node prev;

		// Token: 0x040001A1 RID: 417
		public bool hasBeenTouched = false;

		// Token: 0x040001A2 RID: 418
		public int depth;
	}
}
