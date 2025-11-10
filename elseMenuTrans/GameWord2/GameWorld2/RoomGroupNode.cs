using System;

namespace GameWorld2
{
	// Token: 0x0200008A RID: 138
	public class RoomGroupNode
	{
		// Token: 0x060007AA RID: 1962 RVA: 0x00021954 File Offset: 0x0001FB54
		public override string ToString()
		{
			return this.roomGroup.ToString();
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00021968 File Offset: 0x0001FB68
		public override int GetHashCode()
		{
			return this.roomGroup.GetHashCode();
		}

		// Token: 0x04000209 RID: 521
		public RoomGroup roomGroup;

		// Token: 0x0400020A RID: 522
		public float gscore;

		// Token: 0x0400020B RID: 523
		public float fscore;

		// Token: 0x0400020C RID: 524
		public RoomGroupNode prev;

		// Token: 0x0400020D RID: 525
		public bool hasBeenTouched = false;

		// Token: 0x0400020E RID: 526
		public int depth;
	}
}
