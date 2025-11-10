using System;

namespace GameWorld2
{
	// Token: 0x02000083 RID: 131
	public class Cigarette : Drug
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x00020E38 File Offset: 0x0001F038
		public override string tooltipName
		{
			get
			{
				return "cigarette香烟" + ((base.charges > 0) ? "" : " (used已使用)");
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00020E60 File Offset: 0x0001F060
		public override string verbDescription
		{
			get
			{
				return "smoke抽烟";
			}
		}
	}
}
