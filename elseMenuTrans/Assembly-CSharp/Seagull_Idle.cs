using System;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class Seagull_Idle : SeagullState
{
	// Token: 0x06000416 RID: 1046 RVA: 0x0001D458 File Offset: 0x0001B658
	public Seagull_Idle(Seagull pSeagull)
		: base(pSeagull)
	{
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x0001D464 File Offset: 0x0001B664
	public override void OnEnter()
	{
		base.RemoveVerticalRotation();
		this._timer = global::UnityEngine.Random.Range(5f, 30f);
		base.PlayAnimation(new string[] { "Idle1" });
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x0001D498 File Offset: 0x0001B698
	public override SeagullState Update()
	{
		this._timer -= Time.deltaTime;
		if (this._timer <= 0f)
		{
			this._seagull.currentZone.taken = false;
			return new Seagull_Fly(this._seagull);
		}
		return null;
	}

	// Token: 0x04000321 RID: 801
	private float _timer;
}
