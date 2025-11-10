using System;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class Seagull_Fly : SeagullState
{
	// Token: 0x0600041F RID: 1055 RVA: 0x0001D790 File Offset: 0x0001B990
	public Seagull_Fly(Seagull pSeagull)
		: base(pSeagull)
	{
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x0001D79C File Offset: 0x0001B99C
	public override void OnEnter()
	{
		SeagullZone.ZoneType[] array = new SeagullZone.ZoneType[0];
		if (this._seagull.currentZone.zoneType == SeagullZone.ZoneType.GROUND)
		{
			array = new SeagullZone.ZoneType[1];
		}
		this._seagull.SetANewRandomCurrentZone(array);
		this._goalPosition = this._seagull.currentZone.transform.position;
		this._totalDistance = Vector3.Distance(this._seagull.transform.position, this._goalPosition);
		this._timer = 10f;
		base.PlayAnimation(new string[] { "Fly1", "Fly2" });
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0001D83C File Offset: 0x0001BA3C
	public override SeagullState Update()
	{
		this._timer -= Time.deltaTime;
		if (this._timer <= 0f)
		{
			this._seagull.currentZone.taken = false;
			return new Seagull_Fly(this._seagull);
		}
		if (float.IsNaN(this._seagull.speed))
		{
			Debug.Log("Nan!!!!!");
		}
		float num = this._seagull.speed * 0.8f;
		Quaternion quaternion = Quaternion.LookRotation(this._goalPosition - this._seagull.transform.position);
		Quaternion quaternion2 = Quaternion.Slerp(this._seagull.transform.rotation, quaternion, Time.deltaTime * num);
		this._seagull.transform.rotation = quaternion2;
		this._seagull.transform.Translate(Vector3.forward * Time.deltaTime * this._seagull.speed);
		float num2 = Vector3.Distance(this._seagull.transform.position, this._goalPosition);
		float num3 = 1f - num2 / Mathf.Clamp(this._totalDistance, 0.01f, float.PositiveInfinity);
		float num4 = 3f + Mathf.Sin(num3 * 3.1415927f) * 8f;
		float num5 = num4 - this._seagull.speed;
		this._seagull.speed += num5 * Time.deltaTime;
		if (num2 > 0.5f)
		{
			return null;
		}
		return SeagullState.GetStateFromZoneType(this._seagull.currentZone, this._seagull);
	}

	// Token: 0x04000325 RID: 805
	private Vector3 _goalPosition;

	// Token: 0x04000326 RID: 806
	private float _totalDistance;

	// Token: 0x04000327 RID: 807
	private Vector3 _bounce;

	// Token: 0x04000328 RID: 808
	private float _timer;
}
