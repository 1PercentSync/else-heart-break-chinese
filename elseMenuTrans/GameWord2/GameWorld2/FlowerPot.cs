using System;

namespace GameWorld2
{
	// Token: 0x0200005D RID: 93
	public class FlowerPot : MimanTing
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0001B264 File Offset: 0x00019464
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001B268 File Offset: 0x00019468
		public override bool DoesMasterProgramExist()
		{
			return false;
		}
	}
}
