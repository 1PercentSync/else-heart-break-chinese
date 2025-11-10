using System;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class Wiggle : MonoBehaviour
{
	// Token: 0x06000464 RID: 1124 RVA: 0x0001EC20 File Offset: 0x0001CE20
	private void Update()
	{
		base.transform.localPosition = new Vector3(0f, Mathf.Sin(Time.time * 3f) * 0.02f, 0f);
	}
}
