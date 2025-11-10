using System;
using GameTypes;
using GameWorld2;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class SendPipeShell : Shell
{
	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060005ED RID: 1517 RVA: 0x00027790 File Offset: 0x00025990
	public SendPipe sendPipe
	{
		get
		{
			return base.ting as SendPipe;
		}
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x000277A0 File Offset: 0x000259A0
	public override void CreateTing()
	{
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x000277A4 File Offset: 0x000259A4
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x000277A8 File Offset: 0x000259A8
	protected override void Setup()
	{
		base.Setup();
		this._target = base.transform.Find("SendPipeTarget");
		if (this._target == null)
		{
			D.LogError("SendPipeTarget is null");
		}
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x000277EC File Offset: 0x000259EC
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
		if (this.stuffShell != null)
		{
			Debug.Log("send pipe '" + base.name + "' has stuff: " + this.stuffShell.name);
			if (this.stuffShell.ting.isBeingHeld)
			{
				Debug.Log("Let go of " + this.stuffShell.name);
				this.sendPipe.stuff = null;
				return;
			}
			this.stuffShell.transform.position = this._target.position;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0002788C File Offset: 0x00025A8C
	private Shell stuffShell
	{
		get
		{
			if (this.sendPipe.stuff == null)
			{
				return null;
			}
			return ShellManager.GetShellWithName(this.sendPipe.stuffName);
		}
	}

	// Token: 0x040003F0 RID: 1008
	private Transform _target;
}
