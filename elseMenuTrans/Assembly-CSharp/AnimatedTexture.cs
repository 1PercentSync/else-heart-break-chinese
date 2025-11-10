using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
internal class AnimatedTexture : MonoBehaviour
{
	// Token: 0x060002DE RID: 734 RVA: 0x00013884 File Offset: 0x00011A84
	private void Start()
	{
		base.renderer.sharedMaterial.SetTextureScale("_MainTex", this.size);
	}

	// Token: 0x060002DF RID: 735 RVA: 0x000138A4 File Offset: 0x00011AA4
	private void Update()
	{
		this._timer += Time.deltaTime;
		if (this._timer > 1f / this.framesPerSecond)
		{
			this.NextFrame();
			this._timer = 0f;
		}
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x000138EC File Offset: 0x00011AEC
	private void NextFrame()
	{
		this.index++;
		if (this.index >= this.rows * this.columns)
		{
			this.index = 0;
		}
		Vector2 vector = new Vector2((float)this.index / (float)this.columns - (float)(this.index / this.columns), (float)(this.index / this.columns) / (float)this.rows);
		base.renderer.sharedMaterial.SetTextureOffset("_MainTex", vector);
	}

	// Token: 0x040001C5 RID: 453
	public int columns = 2;

	// Token: 0x040001C6 RID: 454
	public int rows = 2;

	// Token: 0x040001C7 RID: 455
	public float framesPerSecond = 10f;

	// Token: 0x040001C8 RID: 456
	public Vector2 size = new Vector2(0.5f, 0.5f);

	// Token: 0x040001C9 RID: 457
	private int index;

	// Token: 0x040001CA RID: 458
	private float _timer;
}
