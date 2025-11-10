using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class Seagull_Walk : SeagullState
{
	// Token: 0x0600041C RID: 1052 RVA: 0x0001D65C File Offset: 0x0001B85C
	public Seagull_Walk(Seagull pSeagull)
		: base(pSeagull)
	{
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0001D668 File Offset: 0x0001B868
	public override void OnEnter()
	{
		this._seagull.SetANewRandomCurrentZone(new SeagullZone.ZoneType[]
		{
			SeagullZone.ZoneType.SKY_CIRCLE,
			SeagullZone.ZoneType.VANTAGE_POINT
		});
		this._seagull.agent.enabled = true;
		float num = this._seagull.currentZone.transform.localScale.x / 2f;
		float num2 = this._seagull.currentZone.transform.localScale.z / 2f;
		this._seagull.agent.SetDestination(this._seagull.currentZone.transform.position + new Vector3(global::UnityEngine.Random.Range(-num, num), 0f, global::UnityEngine.Random.Range(-num2, num2)));
		base.PlayAnimation(new string[] { "Walk" });
		base.RemoveVerticalRotation();
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0001D748 File Offset: 0x0001B948
	public override SeagullState Update()
	{
		if (this._seagull.agent.remainingDistance < 1f)
		{
			this._seagull.agent.enabled = false;
			return new Seagull_Fly(this._seagull);
		}
		return null;
	}
}
