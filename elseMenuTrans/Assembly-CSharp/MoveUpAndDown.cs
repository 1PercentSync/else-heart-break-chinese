using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class MoveUpAndDown : MonoBehaviour
{
	// Token: 0x0600039E RID: 926 RVA: 0x0001A48C File Offset: 0x0001868C
	private void Start()
	{
		this.startPos = base.transform.position;
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0001A4A0 File Offset: 0x000186A0
	private void Update()
	{
		base.transform.position = this.startPos + new Vector3(0f, this.height * Mathf.Sin(Time.time * this.rate), 0f);
	}

	// Token: 0x040002B7 RID: 695
	private Vector3 startPos;

	// Token: 0x040002B8 RID: 696
	public float height = 30f;

	// Token: 0x040002B9 RID: 697
	public float rate = 1f;
}
