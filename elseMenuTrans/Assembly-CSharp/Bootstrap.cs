using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class Bootstrap : MonoBehaviour
{
	// Token: 0x060002F0 RID: 752 RVA: 0x00013D94 File Offset: 0x00011F94
	private void Start()
	{
		if (WorldOwner.instance.world != null)
		{
			Debug.Log("Bootstrapped world!");
		}
		else
		{
			Debug.Log("Failed to bootstrap world!");
		}
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00013DCC File Offset: 0x00011FCC
	private void Update()
	{
	}
}
