using System;
using GameWorld2;

// Token: 0x020000BD RID: 189
public class KeyShell : Shell
{
	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000571 RID: 1393 RVA: 0x00026244 File Offset: 0x00024444
	public Key key
	{
		get
		{
			return base.ting as Key;
		}
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x00026254 File Offset: 0x00024454
	public override void CreateTing()
	{
	}
}
