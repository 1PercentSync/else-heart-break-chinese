using System;
using GameWorld2;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class MachineShell : Shell
{
	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000583 RID: 1411 RVA: 0x0002640C File Offset: 0x0002460C
	public Machine machine
	{
		get
		{
			return base.ting as Machine;
		}
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0002641C File Offset: 0x0002461C
	protected override void Setup()
	{
		base.Setup();
		this._particles = base.GetComponentInChildren<ParticleSystem>();
		this._goodsAirPoint = base.transform.Find("GoodsAirPoint");
		this.RefreshParticleSystem();
		Transform transform = null;
		if (base.name.Contains("MachineA"))
		{
			transform = base.transform.Find("RefinerCube").Find("RefinerBlock");
		}
		else if (base.name.Contains("MachineB"))
		{
			transform = base.transform.Find("Middle").Find("FactoryBuilderGyro1");
		}
		else
		{
			Debug.LogError("Unknown machine type");
		}
		if (transform != null)
		{
			this._animator = transform.GetComponent<Animator>();
			this._animatedBlockAudio = transform.audio;
		}
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x000264F4 File Offset: 0x000246F4
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		Machine machine = this.machine;
		machine.onRunBlock = (Action)Delegate.Combine(machine.onRunBlock, new Action(this.OnRunBlock));
		Machine machine2 = this.machine;
		machine2.onGoodsConverted = (Action)Delegate.Combine(machine2.onGoodsConverted, new Action(this.OnGoodsConverted));
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00026558 File Offset: 0x00024758
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		Machine machine = this.machine;
		machine.onRunBlock = (Action)Delegate.Remove(machine.onRunBlock, new Action(this.OnRunBlock));
		Machine machine2 = this.machine;
		machine2.onGoodsConverted = (Action)Delegate.Combine(machine2.onGoodsConverted, new Action(this.OnGoodsConverted));
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x000265BC File Offset: 0x000247BC
	private void OnRunBlock()
	{
		if (this._animatedBlockAudio != null && !this._animatedBlockAudio.isPlaying)
		{
			this._animatedBlockAudio.Play();
		}
		this._animator.SetTrigger("Run");
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00026608 File Offset: 0x00024808
	private void OnGoodsConverted()
	{
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0002660C File Offset: 0x0002480C
	private void RefreshParticleSystem()
	{
		if (this._particles)
		{
			this._particles.enableEmission = this.machine.masterProgram.isOn;
		}
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x00026644 File Offset: 0x00024844
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
		this.RefreshParticleSystem();
		if (this.goodsShell != null)
		{
			this.goodsShell.transform.position = this._goodsAirPoint.position;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x0600058B RID: 1419 RVA: 0x00026690 File Offset: 0x00024890
	private Shell goodsShell
	{
		get
		{
			if (this.machine.currentGoods == null)
			{
				return null;
			}
			return ShellManager.GetShellWithName(this.machine.currentGoods.name);
		}
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x000266C4 File Offset: 0x000248C4
	public override void CreateTing()
	{
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x000266C8 File Offset: 0x000248C8
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x040003E3 RID: 995
	private ParticleSystem _particles;

	// Token: 0x040003E4 RID: 996
	private Transform _goodsAirPoint;

	// Token: 0x040003E5 RID: 997
	private Animator _animator;

	// Token: 0x040003E6 RID: 998
	private AudioSource _animatedBlockAudio;
}
