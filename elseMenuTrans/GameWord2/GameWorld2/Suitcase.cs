using System;
using GameTypes;

namespace GameWorld2
{
	// Token: 0x02000084 RID: 132
	public class Suitcase : Locker
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x00020E70 File Offset: 0x0001F070
		public override string tooltipName
		{
			get
			{
				return "suitcase";
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x00020E78 File Offset: 0x0001F078
		public override string verbDescription
		{
			get
			{
				return "look inside";
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x00020E80 File Offset: 0x0001F080
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x00020E84 File Offset: 0x0001F084
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[]
				{
					base.localPoint + IntPoint.Up * 2,
					base.localPoint + IntPoint.Left * 2,
					base.localPoint + IntPoint.Right * 2,
					base.localPoint + IntPoint.Down * 2
				};
			}
		}
	}
}
