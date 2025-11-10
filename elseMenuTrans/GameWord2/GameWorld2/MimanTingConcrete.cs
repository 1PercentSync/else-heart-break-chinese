using System;

namespace GameWorld2
{
	// Token: 0x02000004 RID: 4
	public class MimanTingConcrete : MimanTing
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002828 File Offset: 0x00000A28
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000282C File Offset: 0x00000A2C
		public override bool DoesMasterProgramExist()
		{
			return false;
		}
	}
}
