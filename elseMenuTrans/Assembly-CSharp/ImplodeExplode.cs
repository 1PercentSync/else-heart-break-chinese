using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
public class ImplodeExplode : MonoBehaviour
{
	// Token: 0x06000368 RID: 872 RVA: 0x00019498 File Offset: 0x00017698
	private void Start()
	{
		float quot = this.outerRadius / this.innerRadius;
		this.innerPos = new Vector3[this.nrOfObjects].Mutate(() => global::UnityEngine.Random.onUnitSphere * this.innerRadius);
		this.outerPos = new Vector3[this.nrOfObjects].SetWithIndex((int i) => this.innerPos[i] * quot);
		this.innerRot = new Quaternion[this.nrOfObjects].Mutate(() => global::UnityEngine.Random.rotation);
		this.outerRot = new Quaternion[this.nrOfObjects].Mutate(() => global::UnityEngine.Random.rotation);
		this.transforms = new Transform[this.nrOfObjects];
		this.transforms.SetWithIndex(delegate(int i)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate(this.prefabs.RandNth<GameObject>(), this.innerPos[i], this.innerRot[i]) as GameObject;
			Collider component = gameObject.GetComponent<Collider>();
			if (component != null)
			{
				component.enabled = false;
			}
			return gameObject.transform;
		});
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00019598 File Offset: 0x00017798
	private void Update()
	{
		this.blending = Mathf.Abs(Mathf.Sin(Time.time * this.speed));
		this.SetBlendedPositions(this.blending);
		this.SetBlendedRotations(this.blending);
	}

	// Token: 0x0600036A RID: 874 RVA: 0x000195DC File Offset: 0x000177DC
	private void SetBlendedPositions(float fraction)
	{
		this.transforms.ForEachWithIndex(delegate(int i, Transform t)
		{
			t.position = this.transform.position + this.BlendVectors(this.innerPos[i], this.outerPos[i], fraction);
		});
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00019618 File Offset: 0x00017818
	private void SetBlendedRotations(float fraction)
	{
		this.transforms.ForEachWithIndex(delegate(int i, Transform t)
		{
			t.rotation = this.BlendQuaternions(this.innerRot[i], this.outerRot[i], fraction);
		});
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00019654 File Offset: 0x00017854
	private Vector3 BlendVectors(Vector3 v1, Vector3 v2, float fraction)
	{
		Vector3 vector = v2 - v1;
		return v1 + vector * fraction;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00019678 File Offset: 0x00017878
	private Quaternion BlendQuaternions(Quaternion q1, Quaternion q2, float fraction)
	{
		return Quaternion.Lerp(q1, q2, fraction);
	}

	// Token: 0x04000281 RID: 641
	public int nrOfObjects = 5;

	// Token: 0x04000282 RID: 642
	[Range(0f, 100f)]
	public float innerRadius = 4f;

	// Token: 0x04000283 RID: 643
	[Range(1f, 100f)]
	public float outerRadius = 8f;

	// Token: 0x04000284 RID: 644
	[Range(0f, 2f)]
	public float speed = 1f;

	// Token: 0x04000285 RID: 645
	public GameObject[] prefabs = new GameObject[1];

	// Token: 0x04000286 RID: 646
	private Transform[] transforms;

	// Token: 0x04000287 RID: 647
	private Vector3[] innerPos;

	// Token: 0x04000288 RID: 648
	private Vector3[] outerPos;

	// Token: 0x04000289 RID: 649
	private Quaternion[] innerRot;

	// Token: 0x0400028A RID: 650
	private Quaternion[] outerRot;

	// Token: 0x0400028B RID: 651
	public float blending;
}
