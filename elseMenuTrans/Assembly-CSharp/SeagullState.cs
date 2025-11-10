using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
public abstract class SeagullState
{
	// Token: 0x06000410 RID: 1040 RVA: 0x0001D358 File Offset: 0x0001B558
	public SeagullState(Seagull pSeagull)
	{
		this._seagull = pSeagull;
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0001D368 File Offset: 0x0001B568
	public virtual void OnEnter()
	{
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0001D36C File Offset: 0x0001B56C
	public virtual SeagullState Update()
	{
		return null;
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x0001D370 File Offset: 0x0001B570
	protected void PlayAnimation(string[] pAnimationNames)
	{
		string text = pAnimationNames[global::UnityEngine.Random.Range(0, pAnimationNames.Length)];
		this._seagull.animationComponent.CrossFadeQueued(text, 0.5f, QueueMode.PlayNow);
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0001D3A4 File Offset: 0x0001B5A4
	public static SeagullState GetStateFromZoneType(SeagullZone pTargetZone, Seagull pSeagull)
	{
		switch (pTargetZone.zoneType)
		{
		case SeagullZone.ZoneType.GROUND:
			if (global::UnityEngine.Random.Range(0, 100) < 50)
			{
				return new Seagull_Walk(pSeagull);
			}
			return new Seagull_Idle(pSeagull);
		case SeagullZone.ZoneType.SKY_CIRCLE:
			return new Seagull_Glide(pSeagull);
		case SeagullZone.ZoneType.VANTAGE_POINT:
			return new Seagull_Idle(pSeagull);
		default:
			throw new UnityException("Did not find a state to return");
		}
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x0001D404 File Offset: 0x0001B604
	protected void RemoveVerticalRotation()
	{
		Vector3 eulerAngles = this._seagull.transform.rotation.eulerAngles;
		this._seagull.transform.rotation = Quaternion.Euler(new Vector3(0f, eulerAngles.y, 0f));
	}

	// Token: 0x04000320 RID: 800
	protected Seagull _seagull;
}
