using System;
using GameTypes;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000022 RID: 34
	public class Point : MimanTing
	{
		// Token: 0x06000310 RID: 784 RVA: 0x000121DC File Offset: 0x000103DC
		public override bool DoesMasterProgramExist()
		{
			return false;
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000311 RID: 785 RVA: 0x000121E0 File Offset: 0x000103E0
		[ShowInEditor]
		public override bool isBeingUsed
		{
			get
			{
				return base.AnotherTingSharesTheTile();
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000312 RID: 786 RVA: 0x000121E8 File Offset: 0x000103E8
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[] { base.localPoint };
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00012204 File Offset: 0x00010404
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}
	}
}
