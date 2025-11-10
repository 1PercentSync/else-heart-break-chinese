using System;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class LightRayAnimator : MonoBehaviour
{
	// Token: 0x0600038B RID: 907 RVA: 0x00019EA4 File Offset: 0x000180A4
	private void Start()
	{
		this._baseScale = base.transform.localScale;
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00019EB8 File Offset: 0x000180B8
	private void Update()
	{
		base.transform.localScale = new Vector3(this._baseScale.x + Mathf.Sin(Time.time * this.speed) * this.amount, this._baseScale.y, this._baseScale.z);
		float num = base.renderer.material.mainTextureOffset.y + Time.deltaTime * this.scrollTextureSpeed;
		if (num > 1f)
		{
			num = 0f;
		}
		base.renderer.material.mainTextureOffset = new Vector2(base.renderer.material.mainTextureOffset.x, num);
	}

	// Token: 0x0400029D RID: 669
	public float speed = 0.2f;

	// Token: 0x0400029E RID: 670
	public float amount = 0.2f;

	// Token: 0x0400029F RID: 671
	public float scrollTextureSpeed = 0.5f;

	// Token: 0x040002A0 RID: 672
	private Vector3 _baseScale;
}
