using System;
using GameWorld2;

// Token: 0x020000CD RID: 205
public class SeatShell : Shell
{
	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00027770 File Offset: 0x00025970
	public Seat seat
	{
		get
		{
			return base.ting as Seat;
		}
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x00027780 File Offset: 0x00025980
	public override void CreateTing()
	{
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x00027784 File Offset: 0x00025984
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
