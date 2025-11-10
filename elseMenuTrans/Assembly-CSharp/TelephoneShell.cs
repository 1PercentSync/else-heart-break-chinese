using System;
using GameWorld2;
using RelayLib;
using UnityEngine;

// Token: 0x020000D4 RID: 212
public class TelephoneShell : Shell
{
	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x0600060A RID: 1546 RVA: 0x00027AB8 File Offset: 0x00025CB8
	public Telephone telephone
	{
		get
		{
			return base.ting as Telephone;
		}
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x00027AC8 File Offset: 0x00025CC8
	public override void CreateTing()
	{
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x00027ACC File Offset: 0x00025CCC
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00027AD0 File Offset: 0x00025CD0
	protected override void Setup()
	{
		base.Setup();
		this._animator = base.GetComponent<Animator>();
		this.RefreshPlaySound();
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00027AEC File Offset: 0x00025CEC
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		this.telephone.AddDataListener<bool>("ringing", new ValueEntry<bool>.DataChangeHandler(this.OnRingingChange));
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x00027B1C File Offset: 0x00025D1C
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		this.telephone.RemoveDataListener<bool>("ringing", new ValueEntry<bool>.DataChangeHandler(this.OnRingingChange));
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x00027B4C File Offset: 0x00025D4C
	private void OnRingingChange(bool pOld, bool pNew)
	{
		this.RefreshPlaySound();
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x00027B54 File Offset: 0x00025D54
	private void RefreshPlaySound()
	{
		this._animator.SetBool("Ring", this.telephone.ringing);
		if (this.telephone.ringing && !this._audioSource.isPlaying)
		{
			this._audioSource.Play();
		}
		else
		{
			this._audioSource.Stop();
		}
	}

	// Token: 0x040003F2 RID: 1010
	private Animator _animator;
}
