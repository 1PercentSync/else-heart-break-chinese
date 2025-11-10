using System;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class SmartLamps : MonoBehaviour
{
	// Token: 0x06000436 RID: 1078 RVA: 0x0001DF70 File Offset: 0x0001C170
	private void Start()
	{
		this._index = global::UnityEngine.Random.Range(0, this.repeatHorizontal * this.repeatVertical);
		this._mesh = base.GetComponent<MeshFilter>().mesh;
		this.SetUVCoordinates();
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x0001DFB0 File Offset: 0x0001C1B0
	private void SetUVCoordinates()
	{
		float offsetW = 1f / (float)this.repeatHorizontal;
		float offsetH = 1f / (float)this.repeatVertical;
		float topX = (float)(this._index % this.repeatHorizontal) / (float)this.repeatHorizontal;
		float topY = (float)(this._index / this.repeatHorizontal) / (float)this.repeatVertical;
		this._mesh.uv = this._mesh.uv.Map((Vector2 coord) => new Vector2(coord.x * offsetW + topX, coord.y * offsetH + topY));
	}

	// Token: 0x0400033A RID: 826
	public Texture2D _correctTexture;

	// Token: 0x0400033B RID: 827
	public int repeatHorizontal = 5;

	// Token: 0x0400033C RID: 828
	public int repeatVertical = 3;

	// Token: 0x0400033D RID: 829
	private Mesh _mesh;

	// Token: 0x0400033E RID: 830
	public int _index;
}
