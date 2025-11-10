using System;
using GameWorld2;

// Token: 0x020000AF RID: 175
public class CreditCardShell : Shell
{
	// Token: 0x1700007A RID: 122
	// (get) Token: 0x06000526 RID: 1318 RVA: 0x00025AB4 File Offset: 0x00023CB4
	public CreditCard creditCard
	{
		get
		{
			return base.ting as CreditCard;
		}
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00025AC4 File Offset: 0x00023CC4
	public override void CreateTing()
	{
	}
}
