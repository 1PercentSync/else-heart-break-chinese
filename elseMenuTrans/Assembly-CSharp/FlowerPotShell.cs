using System;
using GameWorld2;

// Token: 0x020000B6 RID: 182
public class FlowerPotShell : Shell
{
	// Token: 0x17000082 RID: 130
	// (get) Token: 0x0600054E RID: 1358 RVA: 0x00025F84 File Offset: 0x00024184
	public FlowerPot flowerPot
	{
		get
		{
			return base.ting as FlowerPot;
		}
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00025F94 File Offset: 0x00024194
	public override void CreateTing()
	{
	}
}
