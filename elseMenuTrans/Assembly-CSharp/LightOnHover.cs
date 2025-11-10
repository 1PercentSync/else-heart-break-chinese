using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class LightOnHover : MonoBehaviour
{
	// Token: 0x06000388 RID: 904 RVA: 0x00019E58 File Offset: 0x00018058
	private void OnMouseEnter()
	{
		base.light.enabled = true;
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00019E68 File Offset: 0x00018068
	private void OnMouseExit()
	{
		base.light.enabled = false;
	}
}
