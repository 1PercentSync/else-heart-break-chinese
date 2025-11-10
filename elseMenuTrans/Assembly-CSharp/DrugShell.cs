using System;
using GameWorld2;

// Token: 0x020000B2 RID: 178
public class DrugShell : Shell
{
	// Token: 0x1700007D RID: 125
	// (get) Token: 0x06000538 RID: 1336 RVA: 0x00025ED0 File Offset: 0x000240D0
	public Drug drug
	{
		get
		{
			return base.ting as Drug;
		}
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x00025EE0 File Offset: 0x000240E0
	public override void CreateTing()
	{
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00025EE4 File Offset: 0x000240E4
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
