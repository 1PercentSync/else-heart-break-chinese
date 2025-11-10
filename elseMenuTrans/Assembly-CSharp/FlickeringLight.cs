using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class FlickeringLight : MonoBehaviour
{
	// Token: 0x06000314 RID: 788 RVA: 0x00017DD4 File Offset: 0x00015FD4
	private void Start()
	{
		base.light.intensity = this.maxIntensity;
		base.StartCoroutine(this.Flicker());
	}

	// Token: 0x06000315 RID: 789 RVA: 0x00017E00 File Offset: 0x00016000
	private IEnumerator Flicker()
	{
		for (;;)
		{
			if (base.audio != null)
			{
				base.audio.Play();
			}
			base.light.intensity = global::UnityEngine.Random.Range(this.minIntensity, this.maxIntensity);
			float randNoise = (float)global::UnityEngine.Random.Range(-1, 1) * global::UnityEngine.Random.Range(-this.noise, this.noise);
			yield return new WaitForSeconds(this.speed + randNoise);
		}
		yield break;
	}

	// Token: 0x04000233 RID: 563
	public float speed = 1f;

	// Token: 0x04000234 RID: 564
	public float noise = 0.2f;

	// Token: 0x04000235 RID: 565
	public float minIntensity = 0.2f;

	// Token: 0x04000236 RID: 566
	public float maxIntensity = 1f;
}
