using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
internal class UVScrambler : MonoBehaviour
{
	// Token: 0x0600030F RID: 783 RVA: 0x00017BEC File Offset: 0x00015DEC
	public void Start()
	{
		this._mesh = base.GetComponent<MeshFilter>().mesh;
		this._defaultUvs = (Vector2[])this._mesh.uv.Clone();
		this._newUvs = (Vector2[])this._defaultUvs.Clone();
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00017C3C File Offset: 0x00015E3C
	public void OnDisable()
	{
		switch (this.uvset)
		{
		case UVScrambler.UVSet.ZERO:
			this._mesh.uv = this._defaultUvs;
			break;
		case UVScrambler.UVSet.ONE:
			this._mesh.uv1 = this._defaultUvs;
			break;
		case UVScrambler.UVSet.TWO:
			this._mesh.uv2 = this._defaultUvs;
			break;
		}
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00017CB0 File Offset: 0x00015EB0
	public void Update()
	{
		for (int i = 0; i < this._defaultUvs.Length; i++)
		{
			this._newUvs[i] = this._defaultUvs[i] + new Vector2(global::UnityEngine.Random.Range(0f, 1f), global::UnityEngine.Random.Range(0f, 1f)) * this.randomAmmount;
		}
		switch (this.uvset)
		{
		case UVScrambler.UVSet.ZERO:
			this._mesh.uv = this._newUvs;
			break;
		case UVScrambler.UVSet.ONE:
			this._mesh.uv1 = this._newUvs;
			break;
		case UVScrambler.UVSet.TWO:
			this._mesh.uv2 = this._newUvs;
			break;
		}
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00017D90 File Offset: 0x00015F90
	public void GetUVRow(int pIndex)
	{
	}

	// Token: 0x0400022A RID: 554
	public UVScrambler.UVSet uvset = UVScrambler.UVSet.ONE;

	// Token: 0x0400022B RID: 555
	private Vector2[] _defaultUvs;

	// Token: 0x0400022C RID: 556
	private Vector2[] _newUvs;

	// Token: 0x0400022D RID: 557
	private Mesh _mesh;

	// Token: 0x0400022E RID: 558
	public float randomAmmount = 0.5f;

	// Token: 0x0200005B RID: 91
	public enum UVSet
	{
		// Token: 0x04000230 RID: 560
		ZERO,
		// Token: 0x04000231 RID: 561
		ONE,
		// Token: 0x04000232 RID: 562
		TWO
	}
}
