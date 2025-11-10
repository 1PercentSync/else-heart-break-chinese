using System;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class NewCameraState
{
	// Token: 0x17000048 RID: 72
	// (get) Token: 0x0600031A RID: 794 RVA: 0x00017F20 File Offset: 0x00016120
	// (set) Token: 0x0600031B RID: 795 RVA: 0x00017F28 File Offset: 0x00016128
	public Vector3 position { get; set; }

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x0600031C RID: 796 RVA: 0x00017F34 File Offset: 0x00016134
	// (set) Token: 0x0600031D RID: 797 RVA: 0x00017F3C File Offset: 0x0001613C
	public Vector3 lookTarget { get; set; }

	// Token: 0x0600031E RID: 798 RVA: 0x00017F48 File Offset: 0x00016148
	public static NewCameraState Lerp(NewCameraState a, NewCameraState b, float c)
	{
		return new NewCameraState
		{
			position = Vector3.Lerp(a.position, b.position, c),
			lookTarget = Vector3.Lerp(a.lookTarget, b.lookTarget, c)
		};
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00017F8C File Offset: 0x0001618C
	public static bool ValidVector3(Vector3 pVector)
	{
		return !float.IsNaN(pVector.x) && !float.IsNaN(pVector.y) && !float.IsNaN(pVector.z);
	}
}
