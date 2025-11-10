using System;
using GameWorld2;

// Token: 0x020000D1 RID: 209
public class StoveShell : Shell
{
	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000600 RID: 1536 RVA: 0x00027A60 File Offset: 0x00025C60
	public Stove stove
	{
		get
		{
			return base.ting as Stove;
		}
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x00027A70 File Offset: 0x00025C70
	public override void CreateTing()
	{
	}
}
