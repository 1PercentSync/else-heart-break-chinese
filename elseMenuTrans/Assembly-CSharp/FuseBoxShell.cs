using System;
using GameWorld2;

// Token: 0x020000B9 RID: 185
public class FuseBoxShell : Shell
{
	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600055F RID: 1375 RVA: 0x00026108 File Offset: 0x00024308
	public FuseBox fuseBox
	{
		get
		{
			return base.ting as FuseBox;
		}
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x00026118 File Offset: 0x00024318
	public override void CreateTing()
	{
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0002611C File Offset: 0x0002431C
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
