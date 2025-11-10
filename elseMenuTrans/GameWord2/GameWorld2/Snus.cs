using System;

namespace GameWorld2
{
	// Token: 0x02000082 RID: 130
	public class Snus : Drug
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00020E00 File Offset: 0x0001F000
		public override string tooltipName
		{
			get
			{
				return "snus" + ((base.charges > 0) ? "" : " (empty)");
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x00020E28 File Offset: 0x0001F028
		public override string verbDescription
		{
			get
			{
				return "take one";
			}
		}
	}
}
