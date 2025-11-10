using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class HideWhenPlaying : MonoBehaviour
{
	// Token: 0x06000362 RID: 866 RVA: 0x000193C8 File Offset: 0x000175C8
	private void Start()
	{
		base.renderer.enabled = false;
	}
}
