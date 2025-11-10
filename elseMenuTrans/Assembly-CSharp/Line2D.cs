using System;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class Line2D
{
	// Token: 0x06000282 RID: 642 RVA: 0x00011CB8 File Offset: 0x0000FEB8
	public Line2D(Vector2 pPointOnLine, Vector2 pNormal)
	{
		this._position = pPointOnLine;
		this._biNormal.x = pNormal.y;
		this._biNormal.y = -pNormal.x;
		this._normal = pNormal;
		this._distance = this.DotProduct(this._normal, pPointOnLine);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00011D14 File Offset: 0x0000FF14
	public float DistanceTo(Vector2 pPosition)
	{
		return this.DotProduct(pPosition, this._normal) - this._distance;
	}

	// Token: 0x06000284 RID: 644 RVA: 0x00011D2C File Offset: 0x0000FF2C
	public Vector2 ClosestPointOnLine(Vector2 pPosition)
	{
		float num = this.DotProduct(this._normal, pPosition - this._position);
		return new Vector3(pPosition.x - num * this._normal.x, pPosition.y - num * this._normal.y);
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00011D88 File Offset: 0x0000FF88
	public float DotProduct(Vector2 a, Vector2 b)
	{
		return a.x * b.x + a.y * b.y;
	}

	// Token: 0x06000286 RID: 646 RVA: 0x00011DAC File Offset: 0x0000FFAC
	public Vector2 GetIntersectionPoint(Vector2 startPos, Vector2 rayForward)
	{
		float num = -this.DistanceTo(startPos) / this.DotProduct(rayForward, this._normal);
		return startPos + rayForward * num;
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000287 RID: 647 RVA: 0x00011DE0 File Offset: 0x0000FFE0
	public Vector2 BiNormal
	{
		get
		{
			return this._biNormal;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000288 RID: 648 RVA: 0x00011DE8 File Offset: 0x0000FFE8
	public Vector2 Normal
	{
		get
		{
			return this._normal;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000289 RID: 649 RVA: 0x00011DF0 File Offset: 0x0000FFF0
	public Vector2 Point
	{
		get
		{
			return this._position;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x0600028A RID: 650 RVA: 0x00011DF8 File Offset: 0x0000FFF8
	// (set) Token: 0x0600028B RID: 651 RVA: 0x00011E00 File Offset: 0x00010000
	public float distance
	{
		get
		{
			return this._distance;
		}
		set
		{
			this._distance = value;
		}
	}

	// Token: 0x04000197 RID: 407
	private Vector2 _biNormal;

	// Token: 0x04000198 RID: 408
	private Vector2 _normal;

	// Token: 0x04000199 RID: 409
	private Vector2 _position;

	// Token: 0x0400019A RID: 410
	private float _distance;
}
