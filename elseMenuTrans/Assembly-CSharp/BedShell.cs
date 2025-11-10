using System;
using GameWorld2;

// Token: 0x020000AA RID: 170
public class BedShell : Shell
{
	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060004C4 RID: 1220 RVA: 0x000208F0 File Offset: 0x0001EAF0
	public Bed bed
	{
		get
		{
			return base.ting as Bed;
		}
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x00020900 File Offset: 0x0001EB00
	public override void CreateTing()
	{
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00020904 File Offset: 0x0001EB04
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
