using System;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class InternetSentry : MonoBehaviour
{
	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000371 RID: 881 RVA: 0x000196A8 File Offset: 0x000178A8
	public Transform guardTarget
	{
		get
		{
			return this._guardTarget;
		}
	}

	// Token: 0x06000372 RID: 882 RVA: 0x000196B0 File Offset: 0x000178B0
	private void Start()
	{
		this._rotationSpeed = global::UnityEngine.Random.Range(0.6f, 0.9f);
		this._attackEffect = base.transform.Find("InternetATTACKfx");
		if (this._attackEffect == null)
		{
			Debug.Log("No AttackEffect found on " + base.name);
		}
	}

	// Token: 0x06000373 RID: 883 RVA: 0x00019710 File Offset: 0x00017910
	public void SetAttackTarget(Transform pTarget)
	{
		if (this._attackEffect != null)
		{
			this._attackEffect.GetComponent<ParticleSystem>().enableEmission = true;
		}
		this._attackTarget = pTarget;
	}

	// Token: 0x06000374 RID: 884 RVA: 0x0001973C File Offset: 0x0001793C
	public void SetNoAttackTarget()
	{
		if (this._attackEffect != null)
		{
			this._attackEffect.GetComponent<ParticleSystem>().enableEmission = false;
		}
		this._attackTarget = null;
	}

	// Token: 0x06000375 RID: 885 RVA: 0x00019768 File Offset: 0x00017968
	private void Update()
	{
		if (this._guardTarget == null)
		{
			GameObject gameObject = GameObject.Find("NetNode_" + this.guardTargetName);
			if (!(gameObject != null))
			{
				return;
			}
			this._guardTarget = gameObject.transform;
		}
		Transform transform = this._guardTarget;
		float num = 1f;
		float num2 = 1f;
		if (this._attackTarget != null)
		{
			transform = this._attackTarget;
			num = 2f;
			num2 = 4f;
			if (this._attackEffect != null)
			{
				this._attackEffect.LookAt(transform.position);
			}
		}
		Quaternion quaternion = Quaternion.LookRotation(transform.transform.position - base.transform.position);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, quaternion, Time.deltaTime * this._rotationSpeed * num2);
		base.transform.Translate(Vector3.forward * Time.deltaTime * this._speed * num);
	}

	// Token: 0x0400028E RID: 654
	public string guardTargetName;

	// Token: 0x0400028F RID: 655
	private Transform _guardTarget;

	// Token: 0x04000290 RID: 656
	private Transform _attackTarget;

	// Token: 0x04000291 RID: 657
	private float _speed = 70f;

	// Token: 0x04000292 RID: 658
	private float _rotationSpeed;

	// Token: 0x04000293 RID: 659
	private Transform _attackEffect;
}
