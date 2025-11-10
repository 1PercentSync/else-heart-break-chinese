using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class HackEffectFlyer : MonoBehaviour
{
	// Token: 0x06000023 RID: 35 RVA: 0x00002DC0 File Offset: 0x00000FC0
	private void Start()
	{
		this.turnSpeed *= global::UnityEngine.Random.Range(0.9f, 1.1f);
		this._individualTimeMod = global::UnityEngine.Random.Range(0f, 1f);
		this._randomVec = global::UnityEngine.Random.insideUnitSphere.normalized;
		this._randomRotVec = global::UnityEngine.Random.insideUnitSphere.normalized * 90f;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002E30 File Offset: 0x00001030
	public void SetGoal(Transform pGoal)
	{
		if (pGoal == null)
		{
			return;
		}
		this._goal = pGoal;
		this._totalDistance = Vector3.Distance(base.transform.position, this._goal.position);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002E74 File Offset: 0x00001074
	private void Update()
	{
		if (this._goal == null)
		{
			return;
		}
		float num = Time.time + this._individualTimeMod;
		Vector3 vector = this._randomVec * Mathf.Sin(num) * this.danceAmount;
		this._randomVec = Quaternion.Euler(this._randomRotVec.x * Time.deltaTime, this._randomRotVec.y * Time.deltaTime, this._randomRotVec.z * Time.deltaTime) * this._randomVec;
		Vector3 vector2 = vector + this._goal.position;
		float num2 = this._speed * this.turnSpeed;
		Quaternion quaternion = Quaternion.LookRotation(vector2 - base.transform.position);
		Quaternion quaternion2 = Quaternion.Slerp(base.transform.rotation, quaternion, Time.deltaTime * num2);
		base.transform.rotation = quaternion2;
		base.transform.Translate(Vector3.forward * Time.deltaTime * this._speed);
		float num3 = Vector3.Distance(base.transform.position, vector2);
		float num4 = 1f - num3 / Mathf.Clamp(this._totalDistance, 0.01f, 10000f);
		float num5 = this.baseSpeed + Mathf.Sin(num4 * 3.1415927f) * this.speedBoost;
		float num6 = num5 - this._speed;
		this._speed += num6 * Time.deltaTime;
		if (num3 > 0.5f)
		{
		}
		if (!base.particleSystem.IsAlive())
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400001D RID: 29
	public float baseSpeed = 5f;

	// Token: 0x0400001E RID: 30
	public float speedBoost = 8f;

	// Token: 0x0400001F RID: 31
	public float danceRate = 1f;

	// Token: 0x04000020 RID: 32
	public float danceAmount = 2f;

	// Token: 0x04000021 RID: 33
	public float turnSpeed = 0.5f;

	// Token: 0x04000022 RID: 34
	private float _speed = 1f;

	// Token: 0x04000023 RID: 35
	private float _totalDistance;

	// Token: 0x04000024 RID: 36
	private Transform _goal;

	// Token: 0x04000025 RID: 37
	private float _individualTimeMod;

	// Token: 0x04000026 RID: 38
	private Vector3 _randomVec;

	// Token: 0x04000027 RID: 39
	private Vector3 _randomRotVec;
}
