using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class ShrinkUvs : MonoBehaviour
{
	// Token: 0x0600042D RID: 1069 RVA: 0x0001DD18 File Offset: 0x0001BF18
	private void Start()
	{
		this._index = global::UnityEngine.Random.Range(0, this.repeatHorizontal * this.repeatVertical);
		this._mesh = base.GetComponent<MeshFilter>().mesh;
		this.SetUVCoordinates();
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x0001DD58 File Offset: 0x0001BF58
	private void SetUVCoordinates()
	{
		float offsetW = 1f / (float)this.repeatHorizontal;
		float offsetH = 1f / (float)this.repeatVertical;
		float topX = (float)(this._index % this.repeatHorizontal) / (float)this.repeatHorizontal;
		float topY = (float)(this._index / this.repeatHorizontal) / (float)this.repeatVertical;
		this._mesh.uv = this._mesh.uv.Map((Vector2 coord) => new Vector2(coord.x * offsetW + topX, coord.y * offsetH + topY));
	}

	// Token: 0x04000332 RID: 818
	public int repeatHorizontal = 5;

	// Token: 0x04000333 RID: 819
	public int repeatVertical = 3;

	// Token: 0x04000334 RID: 820
	private Mesh _mesh;

	// Token: 0x04000335 RID: 821
	public int _index;
}
