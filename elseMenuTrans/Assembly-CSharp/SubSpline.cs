using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class SubSpline
{
	// Token: 0x06000299 RID: 665 RVA: 0x00012460 File Offset: 0x00010660
	public SubSpline(Vector3[] pControlPoints, float pTension)
	{
		if (pControlPoints.Length < 2)
		{
			Debug.LogError("Path must have at least two points!");
		}
		this.tension = pTension;
		this.nodes = new SplineNode[pControlPoints.Length + 2];
		this.nodes[0] = new SplineNode(this.GetExtrapolatedStartPoint(pControlPoints[0], pControlPoints[1]), 0f);
		this.nodes[this.nodes.Length - 1] = new SplineNode(this.GetExtraolatedEndPoint(pControlPoints[pControlPoints.Length - 2], pControlPoints[pControlPoints.Length - 1]), 0f);
		for (int i = 0; i < pControlPoints.Length; i++)
		{
			int num = i + 1;
			float num2 = 0f;
			if (i > 0)
			{
				float timeToHere = this.nodes[num - 1].timeToHere;
				float magnitude = (pControlPoints[i] - this.nodes[num - 1].point).magnitude;
				num2 = timeToHere + magnitude;
			}
			SplineNode splineNode = new SplineNode(pControlPoints[i], num2);
			this.nodes[num] = splineNode;
		}
	}

	// Token: 0x0600029A RID: 666 RVA: 0x000125B4 File Offset: 0x000107B4
	private Vector3 GetExtrapolatedStartPoint(Vector3 pFirst, Vector3 pSecond)
	{
		return pFirst + pFirst - pSecond;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x000125C4 File Offset: 0x000107C4
	private Vector3 GetExtraolatedEndPoint(Vector3 pNextToLast, Vector3 pLast)
	{
		return pLast + pLast - pNextToLast;
	}

	// Token: 0x0600029C RID: 668 RVA: 0x000125D4 File Offset: 0x000107D4
	private Vector3 GetHermiteInternal(int idxFirstPoint, float t)
	{
		float num = t * t;
		float num2 = num * t;
		Vector3 point = this.nodes[idxFirstPoint - 1].point;
		Vector3 point2 = this.nodes[idxFirstPoint].point;
		Vector3 point3 = this.nodes[idxFirstPoint + 1].point;
		Vector3 point4 = this.nodes[idxFirstPoint + 2].point;
		Vector3 vector = this.tension * (point3 - point);
		Vector3 vector2 = this.tension * (point4 - point2);
		float num3 = 2f * num2 - 3f * num + 1f;
		float num4 = -2f * num2 + 3f * num;
		float num5 = num2 - 2f * num + t;
		float num6 = num2 - num;
		return num3 * point2 + num4 * point3 + num5 * vector + num6 * vector2;
	}

	// Token: 0x0600029D RID: 669 RVA: 0x000126D0 File Offset: 0x000108D0
	private Vector3 GetHermiteAtTime(float timeParam)
	{
		int i;
		for (i = 1; i < this.nodes.Length - 2; i++)
		{
			if (this.nodes[i].timeToHere > timeParam)
			{
				break;
			}
		}
		int num = i - 1;
		float num2 = (timeParam - this.nodes[num].timeToHere) / (this.nodes[num + 1].timeToHere - this.nodes[num].timeToHere);
		return this.GetHermiteInternal(num, num2);
	}

	// Token: 0x0600029E RID: 670 RVA: 0x0001275C File Offset: 0x0001095C
	private float TotalTime()
	{
		return this.nodes[this.nodes.Length - 2].timeToHere;
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00012778 File Offset: 0x00010978
	public Curve GetCurve(int pSteps)
	{
		Curve curve = default(Curve);
		curve.points = new SplineNode[pSteps];
		float num = this.TotalTime() / (float)pSteps;
		for (int i = 0; i < pSteps; i++)
		{
			float num2 = num * (float)i;
			Vector3 hermiteAtTime = this.GetHermiteAtTime(num2);
			float num3 = 0f;
			if (i > 0)
			{
				Vector3 vector = hermiteAtTime - curve.points[i - 1].point;
				num3 = curve.points[i - 1].timeToHere + vector.magnitude;
			}
			curve.points[i] = new SplineNode(hermiteAtTime, num3);
		}
		return curve;
	}

	// Token: 0x0400019C RID: 412
	private float tension;

	// Token: 0x0400019D RID: 413
	private SplineNode[] nodes;
}
