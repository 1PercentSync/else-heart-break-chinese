using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class SlowRotate : MonoBehaviour
{
	// Token: 0x06000076 RID: 118 RVA: 0x000047E4 File Offset: 0x000029E4
	private void Start()
	{
	}

	// Token: 0x06000077 RID: 119 RVA: 0x000047E8 File Offset: 0x000029E8
	private void Update()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		eulerAngles.x += this.xRotationSpeed * Time.deltaTime;
		eulerAngles.y += this.yRotationSpeed * Time.deltaTime;
		eulerAngles.z += this.zRotationSpeed * Time.deltaTime;
		Mathf.Clamp(eulerAngles.x, 0f, 360f);
		Mathf.Clamp(eulerAngles.y, 0f, 360f);
		Mathf.Clamp(eulerAngles.z, 0f, 360f);
		base.transform.eulerAngles = eulerAngles;
	}

	// Token: 0x0400005C RID: 92
	public float xRotationSpeed;

	// Token: 0x0400005D RID: 93
	public float yRotationSpeed;

	// Token: 0x0400005E RID: 94
	public float zRotationSpeed;
}
