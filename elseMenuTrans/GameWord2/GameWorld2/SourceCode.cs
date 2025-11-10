using System;
using RelayLib;

namespace GameWorld2
{
	// Token: 0x0200001A RID: 26
	public class SourceCode : RelayObjectTwo
	{
		// Token: 0x06000208 RID: 520 RVA: 0x0000AECC File Offset: 0x000090CC
		protected override void SetupCells()
		{
			this.CELL_name = base.EnsureCell<string>("name", "undefined");
			this.CELL_content = base.EnsureCell<string>("content", "");
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000AF08 File Offset: 0x00009108
		// (set) Token: 0x0600020A RID: 522 RVA: 0x0000AF18 File Offset: 0x00009118
		public string name
		{
			get
			{
				return this.CELL_name.data;
			}
			set
			{
				this.CELL_name.data = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000AF28 File Offset: 0x00009128
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0000AF38 File Offset: 0x00009138
		public string content
		{
			get
			{
				return this.CELL_content.data;
			}
			set
			{
				this.CELL_content.data = value;
			}
		}

		// Token: 0x0400009D RID: 157
		public const string TABLE_NAME = "SourceCodes";

		// Token: 0x0400009E RID: 158
		private ValueEntry<string> CELL_name;

		// Token: 0x0400009F RID: 159
		private ValueEntry<string> CELL_content;
	}
}
