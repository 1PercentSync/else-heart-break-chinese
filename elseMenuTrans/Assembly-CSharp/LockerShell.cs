using System;
using GameWorld2;

// Token: 0x020000BF RID: 191
public class LockerShell : Shell
{
	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600057F RID: 1407 RVA: 0x000263EC File Offset: 0x000245EC
	public Locker locker
	{
		get
		{
			return base.ting as Locker;
		}
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x000263FC File Offset: 0x000245FC
	public override void CreateTing()
	{
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00026400 File Offset: 0x00024600
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
