using System;
using GameWorld2;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class CigaretteShell : Shell
{
	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000503 RID: 1283 RVA: 0x00024AD8 File Offset: 0x00022CD8
	public Cigarette cigarette
	{
		get
		{
			return base.ting as Cigarette;
		}
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00024AE8 File Offset: 0x00022CE8
	public override void CreateTing()
	{
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00024AEC File Offset: 0x00022CEC
	protected override void Setup()
	{
		base.Setup();
		this._smokeEffect = base.GetComponentInChildren<ParticleSystem>();
		Cigarette cigarette = this.cigarette;
		cigarette.onDrugUse = (Drug.OnDrugUse)Delegate.Combine(cigarette.onDrugUse, new Drug.OnDrugUse(this.OnSmoke));
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00024B34 File Offset: 0x00022D34
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		Cigarette cigarette = this.cigarette;
		cigarette.onDrugUse = (Drug.OnDrugUse)Delegate.Remove(cigarette.onDrugUse, new Drug.OnDrugUse(this.OnSmoke));
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00024B70 File Offset: 0x00022D70
	private void OnSmoke()
	{
		this._smokeEffect.Play();
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00024B80 File Offset: 0x00022D80
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x040003D0 RID: 976
	private ParticleSystem _smokeEffect;
}
