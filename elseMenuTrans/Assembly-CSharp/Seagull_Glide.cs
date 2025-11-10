using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class Seagull_Glide : SeagullState
{
	// Token: 0x06000419 RID: 1049 RVA: 0x0001D4E8 File Offset: 0x0001B6E8
	public Seagull_Glide(Seagull pSeagull)
		: base(pSeagull)
	{
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0001D4F4 File Offset: 0x0001B6F4
	public override void OnEnter()
	{
		this._timer = global::UnityEngine.Random.Range(3f, 15f);
		this._rotationSpeed = global::UnityEngine.Random.Range(0.3f, 0.7f);
		base.PlayAnimation(new string[] { "Glide" });
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0001D540 File Offset: 0x0001B740
	public override SeagullState Update()
	{
		this._timer -= Time.deltaTime;
		if (this._timer <= 0f)
		{
			this._seagull.currentZone.taken = false;
			return new Seagull_Fly(this._seagull);
		}
		if (!this._hasPooed && this._timer < 2f)
		{
			if (global::UnityEngine.Random.value < 0.2f)
			{
				this._seagull.Poo();
			}
			this._hasPooed = true;
		}
		Quaternion quaternion = Quaternion.LookRotation(this._seagull.currentZone.transform.position - this._seagull.transform.position);
		this._seagull.transform.rotation = Quaternion.Slerp(this._seagull.transform.rotation, quaternion, Time.deltaTime * this._rotationSpeed);
		this._seagull.transform.Translate(Vector3.forward * Time.deltaTime * this._seagull.speed);
		return null;
	}

	// Token: 0x04000322 RID: 802
	private float _timer;

	// Token: 0x04000323 RID: 803
	private float _rotationSpeed;

	// Token: 0x04000324 RID: 804
	private bool _hasPooed;
}
