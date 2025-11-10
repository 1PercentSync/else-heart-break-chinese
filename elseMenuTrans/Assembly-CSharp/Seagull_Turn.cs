using System;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class Seagull_Turn : SeagullState
{
	// Token: 0x06000422 RID: 1058 RVA: 0x0001D9DC File Offset: 0x0001BBDC
	public Seagull_Turn(Seagull pSeagull)
		: base(pSeagull)
	{
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0001D9E8 File Offset: 0x0001BBE8
	public override void OnEnter()
	{
		this._goalPosition = this._seagull.transform.position - this._seagull.transform.forward * 5f + new Vector3(0f, 1f, 0f);
		this._timer = global::UnityEngine.Random.Range(1f, 4f);
		base.PlayAnimation(new string[] { "Fly1", "Fly2" });
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0001DA74 File Offset: 0x0001BC74
	public override SeagullState Update()
	{
		this._timer -= Time.deltaTime;
		if (this._timer <= 0f)
		{
			this._seagull.currentZone.taken = false;
			return new Seagull_Fly(this._seagull);
		}
		float num = 3f;
		float num2 = num - this._seagull.speed;
		this._seagull.speed += num2 * Time.deltaTime;
		float num3 = this._seagull.speed * 0.4f;
		Quaternion quaternion = Quaternion.LookRotation(this._goalPosition - this._seagull.transform.position);
		this._seagull.transform.rotation = Quaternion.Slerp(this._seagull.transform.rotation, quaternion, Time.deltaTime * num3);
		this._seagull.transform.Translate(Vector3.forward * Time.deltaTime * this._seagull.speed);
		return null;
	}

	// Token: 0x04000329 RID: 809
	private Vector3 _goalPosition;

	// Token: 0x0400032A RID: 810
	private float _timer;
}
