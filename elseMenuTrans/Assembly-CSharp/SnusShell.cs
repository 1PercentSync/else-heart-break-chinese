using System;
using GameWorld2;

// Token: 0x020000D0 RID: 208
public class SnusShell : Shell
{
	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060005FC RID: 1532 RVA: 0x00027A40 File Offset: 0x00025C40
	public Snus snus
	{
		get
		{
			return base.ting as Snus;
		}
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x00027A50 File Offset: 0x00025C50
	public override void CreateTing()
	{
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x00027A54 File Offset: 0x00025C54
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
