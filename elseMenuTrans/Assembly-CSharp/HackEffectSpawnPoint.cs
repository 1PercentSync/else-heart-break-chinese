using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class HackEffectSpawnPoint : MonoBehaviour
{
	// Token: 0x06000027 RID: 39 RVA: 0x00003048 File Offset: 0x00001248
	private void Start()
	{
		base.StartCoroutine("Spawn");
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00003058 File Offset: 0x00001258
	private IEnumerator Spawn()
	{
		for (;;)
		{
			GameObject flyerGameObject = global::UnityEngine.Object.Instantiate(this.flyerPrefab, base.transform.position, global::UnityEngine.Random.rotation) as GameObject;
			HackEffectFlyer flyer = flyerGameObject.GetComponent<HackEffectFlyer>();
			flyer.SetGoal(this.goalPoint);
			yield return new WaitForSeconds(global::UnityEngine.Random.Range(this.minTimeSpacing, this.maxTimeSpacing));
		}
		yield break;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00003074 File Offset: 0x00001274
	private void Update()
	{
	}

	// Token: 0x04000028 RID: 40
	public Transform goalPoint;

	// Token: 0x04000029 RID: 41
	public GameObject flyerPrefab;

	// Token: 0x0400002A RID: 42
	public float minTimeSpacing = 1f;

	// Token: 0x0400002B RID: 43
	public float maxTimeSpacing = 3f;
}
