using System;
using GameTypes;
using GameWorld2;
using RelayLib;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class FountainShell : Shell
{
	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000551 RID: 1361 RVA: 0x00025FA0 File Offset: 0x000241A0
	public Fountain fountain
	{
		get
		{
			return base.ting as Fountain;
		}
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00025FB0 File Offset: 0x000241B0
	public override void CreateTing()
	{
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x00025FB4 File Offset: 0x000241B4
	protected override void Setup()
	{
		base.Setup();
		this._particles = base.transform.Find("FountainWater").GetComponentsInChildren<ParticleSystem>();
		if (this._particles.Length == 0)
		{
			D.LogError("Can't find any particle systems");
		}
		this.fountain.masterProgram.Start();
		this.RefreshParticles();
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00026010 File Offset: 0x00024210
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		this.fountain.AddDataListener<bool>("on", new ValueEntry<bool>.DataChangeHandler(this.OnOnChange));
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x00026040 File Offset: 0x00024240
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		this.fountain.RemoveDataListener<bool>("on", new ValueEntry<bool>.DataChangeHandler(this.OnOnChange));
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00026070 File Offset: 0x00024270
	private void OnOnChange(bool pPrevios, bool pNew)
	{
		this.RefreshParticles();
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x00026078 File Offset: 0x00024278
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
		this.RefreshParticles();
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x00026088 File Offset: 0x00024288
	private void RefreshParticles()
	{
		foreach (ParticleSystem particleSystem in this._particles)
		{
			particleSystem.enableEmission = this.fountain.on && !this.fountain.containsBrokenPrograms;
		}
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x000260DC File Offset: 0x000242DC
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x040003DF RID: 991
	private ParticleSystem[] _particles;
}
