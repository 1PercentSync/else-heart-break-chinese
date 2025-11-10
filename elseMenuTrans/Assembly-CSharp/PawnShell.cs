using System;
using GameTypes;
using GameWorld2;
using RelayLib;
using UnityEngine;

// Token: 0x020000C6 RID: 198
public class PawnShell : Shell
{
	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00026F24 File Offset: 0x00025124
	public Pawn pawn
	{
		get
		{
			return base.ting as Pawn;
		}
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x00026F34 File Offset: 0x00025134
	public override void CreateTing()
	{
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x00026F38 File Offset: 0x00025138
	protected override void Setup()
	{
		base.Setup();
		this._animator = base.GetComponentInChildren<Animator>();
		this._light = base.GetComponentInChildren<Light>();
		if (this._light == null)
		{
			D.Log(base.name + " can't find light");
		}
		SoundDictionary.LoadSingleSound("FishAttack", "Fish game attack sound 1");
		SoundDictionary.LoadSingleSound("FishDamage", "Fish game damage sound 1");
		this.RefreshLook();
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x00026FB0 File Offset: 0x000251B0
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		this.pawn.AddDataListener<bool>("dead", new ValueEntry<bool>.DataChangeHandler(this.OnDeadChange));
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x00026FE0 File Offset: 0x000251E0
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		this.pawn.RemoveDataListener<bool>("dead", new ValueEntry<bool>.DataChangeHandler(this.OnDeadChange));
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x00027010 File Offset: 0x00025210
	private void OnDeadChange(bool pPrevDead, bool pNewDead)
	{
		this.RefreshLook();
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x00027018 File Offset: 0x00025218
	protected override void RefreshSmoke()
	{
		if (Time.frameCount % this._moduluTickToCheckForSmoke == 0)
		{
			base.ting.emitsSmoke = base.ting.containsBrokenPrograms || this.pawn.dead;
		}
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x00027060 File Offset: 0x00025260
	private void RefreshLook()
	{
		Color color = ((!this.pawn.dead) ? Color.white : Color.black);
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
		{
			renderer.material.color = color;
		}
		if (this._light != null)
		{
			this._light.color = color;
		}
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x000270D8 File Offset: 0x000252D8
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x000270DC File Offset: 0x000252DC
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
		if (this._animator != null && !this.pawn.isBeingHeld)
		{
			Vector3 vector = MimanHelper.TilePositionToVector3(this.pawn.localPoint);
			Vector3 vector2 = vector + new Vector3(0f, Shell.GetSurfaceHeight(vector), 0f);
			Vector3 vector3 = vector2 - base.transform.position;
			Vector3 vector4 = this._pid.Update(vector3, Time.deltaTime);
			base.transform.position += vector4;
			base.SnapShellToTingDirection();
			bool flag = vector4.magnitude > 0f;
			this._animator.SetBool(PawnShell.MOVING, flag);
		}
	}

	// Token: 0x040003EB RID: 1003
	private Light _light;

	// Token: 0x040003EC RID: 1004
	private PidVector3 _pid = new PidVector3();

	// Token: 0x040003ED RID: 1005
	private Animator _animator;

	// Token: 0x040003EE RID: 1006
	private static readonly int MOVING = Animator.StringToHash("Moving");
}
