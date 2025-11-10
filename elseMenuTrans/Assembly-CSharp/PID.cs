using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
[Serializable]
public class PID
{
	// Token: 0x06000322 RID: 802 RVA: 0x00017FEC File Offset: 0x000161EC
	public float Update(float currentError, float timeFrame)
	{
		this.integral += currentError * timeFrame;
		this.derivate = (currentError - this.lastError) / timeFrame;
		this.lastError = currentError;
		return currentError * this.pFactor + this.integral * this.iFactor + this.derivate * this.dFactor;
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00018044 File Offset: 0x00016244
	public void Reset()
	{
		this.integral = (this.derivate = (this.lastError = 0f));
	}

	// Token: 0x0400023D RID: 573
	public float pFactor = 0.1f;

	// Token: 0x0400023E RID: 574
	public float iFactor;

	// Token: 0x0400023F RID: 575
	public float dFactor;

	// Token: 0x04000240 RID: 576
	[SerializeField]
	[HideInInspector]
	private float integral;

	// Token: 0x04000241 RID: 577
	[SerializeField]
	[HideInInspector]
	private float derivate;

	// Token: 0x04000242 RID: 578
	[HideInInspector]
	[SerializeField]
	private float lastError;
}
