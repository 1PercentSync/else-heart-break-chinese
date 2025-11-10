using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class PitchRandomizer : MonoBehaviour
{
	// Token: 0x060003B2 RID: 946 RVA: 0x0001AAD4 File Offset: 0x00018CD4
	private void Start()
	{
		if (base.audio != null)
		{
			base.audio.pitch = global::UnityEngine.Random.Range(this.MinPitch, this.MaxPitch);
		}
		else
		{
			Debug.Log("Warning, PitchRandomzier script on something without an audio component.");
		}
	}

	// Token: 0x040002CC RID: 716
	public float MinPitch = 0.8f;

	// Token: 0x040002CD RID: 717
	public float MaxPitch = 1.2f;
}
