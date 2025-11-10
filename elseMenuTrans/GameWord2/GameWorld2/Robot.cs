using System;
using GameTypes;

namespace GameWorld2
{
	// Token: 0x02000067 RID: 103
	public class Robot : MimanTing
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0001CDD8 File Offset: 0x0001AFD8
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[]
				{
					base.localPoint + IntPoint.Up * 1,
					base.localPoint + IntPoint.Right * 1,
					base.localPoint + IntPoint.Left * 1,
					base.localPoint + IntPoint.Down * 1
				};
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x0001CE74 File Offset: 0x0001B074
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600062E RID: 1582
		public override string verbDescription
		{
			get
			{
				return "inspect检查";
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x0001CE80 File Offset: 0x0001B080
		public override string tooltipName
		{
			get
			{
				return "robot";
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0001CE88 File Offset: 0x0001B088
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0001CE8C File Offset: 0x0001B08C
		public override bool DoesMasterProgramExist()
		{
			return false;
		}
	}
}
