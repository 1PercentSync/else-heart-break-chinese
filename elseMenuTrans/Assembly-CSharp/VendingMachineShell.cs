using System;
using GameTypes;
using GameWorld2;
using UnityEngine;

// Token: 0x020000DA RID: 218
public class VendingMachineShell : Shell
{
	// Token: 0x06000635 RID: 1589 RVA: 0x00028890 File Offset: 0x00026A90
	protected override void Setup()
	{
		base.Setup();
		this._cokePoint = base.transform.Find("CokePoint");
		D.isNull(this._cokePoint);
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000636 RID: 1590 RVA: 0x000288BC File Offset: 0x00026ABC
	public VendingMachine vendingMachine
	{
		get
		{
			return base.ting as VendingMachine;
		}
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x000288CC File Offset: 0x00026ACC
	public override void CreateTing()
	{
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x000288D0 File Offset: 0x00026AD0
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
		if (this.vendingMachine.dispensedCoke != null)
		{
			Shell shellWithName = ShellManager.GetShellWithName(this.vendingMachine.dispensedCoke.name);
			shellWithName.transform.position = this._cokePoint.transform.position;
		}
	}

	// Token: 0x04000407 RID: 1031
	private Transform _cokePoint;
}
