using System;
using UnityEngine;

// Token: 0x02000061 RID: 97
[Serializable]
public class OrbitNewCameraState : NewCameraState
{
	// Token: 0x06000325 RID: 805 RVA: 0x00018118 File Offset: 0x00016318
	public void BeginMove()
	{
		this.isMoving = true;
		this.dragTime = 0f;
	}

	// Token: 0x06000326 RID: 806 RVA: 0x0001812C File Offset: 0x0001632C
	public void Move(float pRightLeftAngle, float pUpDown)
	{
		if (!this.isMoving)
		{
			return;
		}
		this.lastLeftRightAngleSignal = pRightLeftAngle * GreatCamera.invertSignum;
		this.currentAngle += pRightLeftAngle * GreatCamera.invertSignum;
		this.currentAngle = Mathf.Repeat(this.currentAngle, 360f);
		this.tilt -= pUpDown;
		this.tilt = Mathf.Clamp(this.tilt, this.minTilt, this.maxTilt);
		this.targetTilt = this.tilt;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x000181B4 File Offset: 0x000163B4
	public void Zoom(float pNearFar)
	{
		this.radius -= pNearFar;
		this.radius = Mathf.Clamp(this.radius, this.zoomLevels[0], this.zoomLevels[this.zoomLevels.Length - 1]);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x000181FC File Offset: 0x000163FC
	public void ZoomDiscrete(int dir)
	{
		this.zoomLevel -= (float)dir;
		this.zoomLevel = Mathf.Clamp(this.zoomLevel, 0f, (float)(this.zoomLevels.Length - 1));
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0001823C File Offset: 0x0001643C
	public void EndMove()
	{
		if (!this.isMoving)
		{
			return;
		}
		this.isMoving = false;
		this.SnapTargetAngle();
		float num = float.MaxValue;
		for (int i = 0; i < this.zoomLevels.Length; i++)
		{
			float num2 = Mathf.Abs(this.zoomLevels[i] - this.radius);
			if (num2 < num)
			{
				num = num2;
				this.zoomLevel = (float)i;
			}
		}
	}

	// Token: 0x0600032A RID: 810 RVA: 0x000182A8 File Offset: 0x000164A8
	private void SnapTargetAngle()
	{
		float num = this.currentAngle;
		float num2 = this.stepSize / 2f;
		if (this.dragTime < 0.2f)
		{
			if (this.lastLeftRightAngleSignal < 0f)
			{
				num -= num2;
			}
			else if (this.lastLeftRightAngleSignal > 0f)
			{
				num += num2;
			}
		}
		int num3 = (int)((num + num2) / this.stepSize);
		this.targetAngle = (float)num3 * this.stepSize;
		this.targetAngle = Mathf.Repeat(this.targetAngle, 360f);
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x0600032B RID: 811 RVA: 0x00018338 File Offset: 0x00016538
	private float stepSize
	{
		get
		{
			return 360f / this.angleSteps;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600032C RID: 812 RVA: 0x00018348 File Offset: 0x00016548
	private float targetRadius
	{
		get
		{
			int num = (int)this.zoomLevel;
			if (num >= 0 && num < this.zoomLevels.Length)
			{
				return this.zoomLevels[num];
			}
			return 10f;
		}
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00018380 File Offset: 0x00016580
	public void Update()
	{
		if (this.isMoving)
		{
			this.dragTime += Time.deltaTime;
		}
		else if (!Mathf.Approximately(this.autoRotateSpeed, 0f))
		{
			this.currentAngle += this.autoRotateSpeed * Time.deltaTime;
			this.SnapTargetAngle();
		}
		else
		{
			this.angleSpeed = this.anglePid.Update(Mathf.DeltaAngle(this.currentAngle, this.targetAngle), Time.deltaTime);
			this.angleSpeed = Mathf.Clamp(this.angleSpeed, -Time.deltaTime * this.maxAngleSpeedChange, Time.deltaTime * this.maxAngleSpeedChange);
			this.currentAngle += this.angleSpeed;
			this.currentAngle = Mathf.Repeat(this.currentAngle, 360f);
			float num = this.tiltPid.Update(Mathf.DeltaAngle(this.tilt, this.targetTilt), Time.deltaTime);
			num = Mathf.Clamp(num, -Time.deltaTime * this.maxAngleSpeedChange, Time.deltaTime * this.maxAngleSpeedChange);
			this.tilt += num;
			this.zoomSpeed = this.zoomPid.Update(this.radius - this.targetRadius, Time.deltaTime);
			this.zoomSpeed = Mathf.Clamp(this.zoomSpeed, -Time.deltaTime * 100f, Time.deltaTime * 100f);
			this.radius -= this.zoomSpeed;
		}
		Vector3 vector = base.lookTarget + new Vector3(Mathf.Cos(0.017453292f * this.currentAngle), Mathf.Cos(0.017453292f * this.tilt), Mathf.Sin(0.017453292f * this.currentAngle)) * this.radius;
		if (NewCameraState.ValidVector3(vector))
		{
			base.position = vector;
		}
	}

	// Token: 0x04000243 RID: 579
	public float[] zoomLevels = new float[] { 30f, 40f, 60f };

	// Token: 0x04000244 RID: 580
	public PID anglePid = new PID();

	// Token: 0x04000245 RID: 581
	public PID zoomPid = new PID();

	// Token: 0x04000246 RID: 582
	public PID tiltPid = new PID();

	// Token: 0x04000247 RID: 583
	private bool isMoving;

	// Token: 0x04000248 RID: 584
	private float dragTime;

	// Token: 0x04000249 RID: 585
	public float angleSteps = 8f;

	// Token: 0x0400024A RID: 586
	public float targetAngle;

	// Token: 0x0400024B RID: 587
	public float currentAngle;

	// Token: 0x0400024C RID: 588
	public float angleSpeed;

	// Token: 0x0400024D RID: 589
	private float maxAngleSpeedChange = 120f;

	// Token: 0x0400024E RID: 590
	private float lastLeftRightAngleSignal;

	// Token: 0x0400024F RID: 591
	public float minTilt = 10f;

	// Token: 0x04000250 RID: 592
	public float maxTilt = 80f;

	// Token: 0x04000251 RID: 593
	public float tilt = 45f;

	// Token: 0x04000252 RID: 594
	public float targetTilt;

	// Token: 0x04000253 RID: 595
	public float zoomLevel = 1f;

	// Token: 0x04000254 RID: 596
	public float zoomSpeed;

	// Token: 0x04000255 RID: 597
	public float radius = 10f;

	// Token: 0x04000256 RID: 598
	public float autoRotateSpeed;
}
