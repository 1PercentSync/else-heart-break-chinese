using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class Stroboscope : MonoBehaviour
{
	// Token: 0x06000446 RID: 1094 RVA: 0x0001E550 File Offset: 0x0001C750
	private void Start()
	{
		this._light = base.GetComponent<Light>();
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0001E560 File Offset: 0x0001C760
	private void Update()
	{
		this._light.enabled = 0f < Mathf.Sin(Time.time * this.rate);
	}

	// Token: 0x0400034E RID: 846
	public float rate = 10f;

	// Token: 0x0400034F RID: 847
	private Light _light;
}
