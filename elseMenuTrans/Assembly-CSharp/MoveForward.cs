using System;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class MoveForward : MonoBehaviour
{
	// Token: 0x0600039C RID: 924 RVA: 0x0001A44C File Offset: 0x0001864C
	private void Update()
	{
		base.transform.Translate(this.movement * Time.deltaTime);
	}

	// Token: 0x040002B6 RID: 694
	public Vector3 movement;
}
