using System;
using GameWorld2;

// Token: 0x020000D3 RID: 211
public class TaserShell : Shell
{
	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000607 RID: 1543 RVA: 0x00027A9C File Offset: 0x00025C9C
	public Taser taser
	{
		get
		{
			return base.ting as Taser;
		}
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00027AAC File Offset: 0x00025CAC
	public override void CreateTing()
	{
	}
}
