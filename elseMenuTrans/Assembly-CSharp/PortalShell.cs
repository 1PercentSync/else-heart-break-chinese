using System;
using GameWorld2;

// Token: 0x020000C8 RID: 200
public class PortalShell : Shell
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060005C8 RID: 1480 RVA: 0x000271E8 File Offset: 0x000253E8
	public Portal portal
	{
		get
		{
			return base.ting as Portal;
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x000271F8 File Offset: 0x000253F8
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x000271FC File Offset: 0x000253FC
	public override void CreateTing()
	{
	}
}
