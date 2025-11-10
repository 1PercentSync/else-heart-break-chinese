using System;
using GameWorld2;

// Token: 0x020000CC RID: 204
public class ScrewdriverShell : Shell
{
	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060005E5 RID: 1509 RVA: 0x00027750 File Offset: 0x00025950
	public Screwdriver screwdriver
	{
		get
		{
			return base.ting as Screwdriver;
		}
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x00027760 File Offset: 0x00025960
	public override void CreateTing()
	{
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x00027764 File Offset: 0x00025964
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
