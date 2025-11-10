using System;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000086 RID: 134
	public class Map : MimanTing
	{
		// Token: 0x06000784 RID: 1924 RVA: 0x00021090 File Offset: 0x0001F290
		protected override void SetupCells()
		{
			base.SetupCells();
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x00021098 File Offset: 0x0001F298
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000786 RID: 1926
		public override string tooltipName
		{
			get
			{
				return "map地图";
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000787 RID: 1927
		public override string verbDescription
		{
			get
			{
				return "look at查看 [m]";
			}
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x000210AC File Offset: 0x0001F2AC
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Locker || pTingToInteractWith is TrashCan || pTingToInteractWith is SendPipe || pTingToInteractWith is Stove;
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x000210DC File Offset: 0x0001F2DC
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x000210E0 File Offset: 0x0001F2E0
		public override bool DoesMasterProgramExist()
		{
			return false;
		}

		// Token: 0x040001FA RID: 506
		public new static string TABLE_NAME = "Ting_Maps";
	}
}
