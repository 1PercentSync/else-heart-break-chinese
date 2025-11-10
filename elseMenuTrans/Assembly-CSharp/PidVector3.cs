using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class PidVector3
{
	// Token: 0x06000231 RID: 561 RVA: 0x00010DD0 File Offset: 0x0000EFD0
	public Vector3 Update(Vector3 currentError, float timeFrame)
	{
		this.integral += currentError * timeFrame;
		this.derivate = (currentError - this.lastError) / timeFrame;
		this.lastError = currentError;
		return currentError * this.pFactor + this.integral * this.iFactor + this.derivate * this.dFactor;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x00010E4C File Offset: 0x0000F04C
	public void Reset()
	{
		this.integral = (this.derivate = (this.lastError = Vector3.zero));
	}

	// Token: 0x04000162 RID: 354
	public float pFactor = 0.03f;

	// Token: 0x04000163 RID: 355
	public float iFactor;

	// Token: 0x04000164 RID: 356
	public float dFactor;

	// Token: 0x04000165 RID: 357
	[SerializeField]
	[HideInInspector]
	private Vector3 integral;

	// Token: 0x04000166 RID: 358
	[HideInInspector]
	[SerializeField]
	private Vector3 derivate;

	// Token: 0x04000167 RID: 359
	[SerializeField]
	[HideInInspector]
	private Vector3 lastError;
}
