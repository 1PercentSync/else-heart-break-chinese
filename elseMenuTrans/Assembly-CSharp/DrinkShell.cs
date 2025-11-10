using System;
using GameWorld2;

// Token: 0x020000B1 RID: 177
public class DrinkShell : Shell
{
	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06000534 RID: 1332 RVA: 0x00025EB0 File Offset: 0x000240B0
	public Drink drink
	{
		get
		{
			return base.ting as Drink;
		}
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00025EC0 File Offset: 0x000240C0
	public override void CreateTing()
	{
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x00025EC4 File Offset: 0x000240C4
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
