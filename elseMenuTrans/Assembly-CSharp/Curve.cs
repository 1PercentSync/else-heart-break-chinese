using System;
using System.Text;
using UnityEngine;

// Token: 0x02000042 RID: 66
public struct Curve
{
	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000295 RID: 661 RVA: 0x000122F0 File Offset: 0x000104F0
	public float length
	{
		get
		{
			return this.points[this.points.Length - 1].timeToHere;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000296 RID: 662 RVA: 0x0001230C File Offset: 0x0001050C
	public int pointCount
	{
		get
		{
			return this.points.Length;
		}
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00012318 File Offset: 0x00010518
	public Vector3 GetInterpolatedPositionAtLength(float pLength)
	{
		for (int i = 0; i < this.points.Length; i++)
		{
			if (this.points[i].timeToHere > pLength)
			{
				float num = pLength - this.points[i - 1].timeToHere;
				Vector3 vector = this.points[i].point - this.points[i - 1].point;
				float num2 = this.points[i].timeToHere - this.points[i - 1].timeToHere;
				return this.points[i - 1].point + vector * (num / num2);
			}
		}
		return this.points[this.points.Length - 1].point;
	}

	// Token: 0x06000298 RID: 664 RVA: 0x000123F8 File Offset: 0x000105F8
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (SplineNode splineNode in this.points)
		{
			stringBuilder.Append("point ");
			stringBuilder.Append(splineNode.ToString());
			stringBuilder.Append("\n");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0400019B RID: 411
	internal SplineNode[] points;
}
