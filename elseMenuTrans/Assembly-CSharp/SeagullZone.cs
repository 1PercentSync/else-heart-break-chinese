using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class SeagullZone : MonoBehaviour
{
	// Token: 0x06000426 RID: 1062 RVA: 0x0001DB88 File Offset: 0x0001BD88
	private void Start()
	{
		base.renderer.enabled = false;
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0001DB98 File Offset: 0x0001BD98
	private void OnDrawGizmos()
	{
		Gizmos.color = ((!this.taken) ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f));
		Gizmos.DrawCube(base.transform.position, new Vector3(1.2f, 1.2f, 1.2f));
	}

	// Token: 0x0400032B RID: 811
	public SeagullZone.ZoneType zoneType;

	// Token: 0x0400032C RID: 812
	public bool taken;

	// Token: 0x0200008F RID: 143
	public enum ZoneType
	{
		// Token: 0x0400032E RID: 814
		GROUND,
		// Token: 0x0400032F RID: 815
		SKY_CIRCLE,
		// Token: 0x04000330 RID: 816
		VANTAGE_POINT
	}
}
