using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
internal struct SplineNode
{
	// Token: 0x060002A0 RID: 672 RVA: 0x0001282C File Offset: 0x00010A2C
	internal SplineNode(Vector3 pPoint, float pTime)
	{
		this.point = pPoint;
		this.timeToHere = pTime;
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0001283C File Offset: 0x00010A3C
	public override string ToString()
	{
		return string.Concat(new object[] { "[pos: ", this.point, ", timeHere: ", this.timeToHere, "]" });
	}

	// Token: 0x0400019E RID: 414
	internal Vector3 point;

	// Token: 0x0400019F RID: 415
	internal float timeToHere;
}
