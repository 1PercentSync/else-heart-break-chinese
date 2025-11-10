using System;
using System.Collections.Generic;
using RelayLib;

namespace GameWorld2
{
	// Token: 0x0200005E RID: 94
	public class Memory : MimanTing
	{
		// Token: 0x06000590 RID: 1424 RVA: 0x0001B274 File Offset: 0x00019474
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_data = base.EnsureCell<Dictionary<string, object>>("data", new Dictionary<string, object>());
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0001B294 File Offset: 0x00019494
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x0001B298 File Offset: 0x00019498
		public override string tooltipName
		{
			get
			{
				return base.name;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x0001B2A0 File Offset: 0x000194A0
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x0001B2B0 File Offset: 0x000194B0
		public Dictionary<string, object> data
		{
			get
			{
				return this.CELL_data.data;
			}
			set
			{
				this.CELL_data.data = value;
			}
		}

		// Token: 0x17000131 RID: 305
		public object this[string pKey]
		{
			get
			{
				return this.CELL_data.data[pKey];
			}
			set
			{
				this.CELL_data.data[pKey] = value;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x0001B2E8 File Offset: 0x000194E8
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001B2EC File Offset: 0x000194EC
		public override bool DoesMasterProgramExist()
		{
			return false;
		}

		// Token: 0x04000179 RID: 377
		private ValueEntry<Dictionary<string, object>> CELL_data;
	}
}
