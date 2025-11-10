using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class LerumBlinker : MonoBehaviour
{
	// Token: 0x06000386 RID: 902 RVA: 0x00019E14 File Offset: 0x00018014
	private void Update()
	{
		base.renderer.material.mainTextureOffset = LerumBlinker.offsets[(int)(Time.time * this.scrollSpeed) % 4];
	}

	// Token: 0x0400029B RID: 667
	public float scrollSpeed = 5f;

	// Token: 0x0400029C RID: 668
	private static Vector2[] offsets = new Vector2[]
	{
		new Vector2(0f, 0f),
		new Vector2(0.5f, 0f),
		new Vector2(0f, 0.5f),
		new Vector2(0.5f, 0.5f)
	};
}
