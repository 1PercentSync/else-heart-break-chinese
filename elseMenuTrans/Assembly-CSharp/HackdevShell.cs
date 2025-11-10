using System;
using GameWorld2;

// Token: 0x020000BB RID: 187
public class HackdevShell : Shell
{
	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600056A RID: 1386 RVA: 0x00026208 File Offset: 0x00024408
	public Hackdev hackdev
	{
		get
		{
			return base.ting as Hackdev;
		}
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x00026218 File Offset: 0x00024418
	public override void CreateTing()
	{
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x0002621C File Offset: 0x0002441C
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
