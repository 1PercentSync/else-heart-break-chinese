using System;
using GameWorld2;

// Token: 0x020000BC RID: 188
public class JewelleryShell : Shell
{
	// Token: 0x17000088 RID: 136
	// (get) Token: 0x0600056E RID: 1390 RVA: 0x00026228 File Offset: 0x00024428
	public Jewellery jewellery
	{
		get
		{
			return base.ting as Jewellery;
		}
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00026238 File Offset: 0x00024438
	public override void CreateTing()
	{
	}
}
