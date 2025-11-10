using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class SecretTurner : MonoBehaviour
{
	// Token: 0x06000429 RID: 1065 RVA: 0x0001DC18 File Offset: 0x0001BE18
	private void Update()
	{
		if (Input.GetKey(KeyCode.Period))
		{
			base.transform.Rotate(Vector3.up, 3f * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.Comma))
		{
			base.transform.Rotate(Vector3.up, -3f * Time.deltaTime);
		}
	}
}
