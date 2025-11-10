using System;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class HoverExitArrow : MonoBehaviour
{
	// Token: 0x06000364 RID: 868 RVA: 0x000193E0 File Offset: 0x000175E0
	private void Start()
	{
		if (HoverExitArrow._normal == null)
		{
			HoverExitArrow._normal = Resources.Load("ExitArrowMaterial") as Material;
			HoverExitArrow._hover = Resources.Load("ExitArrowSelectedMaterial") as Material;
		}
	}

	// Token: 0x06000365 RID: 869 RVA: 0x00019428 File Offset: 0x00017628
	public void ShowHoverMaterial()
	{
		base.renderer.material = HoverExitArrow._hover;
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0001943C File Offset: 0x0001763C
	private void Update()
	{
		base.renderer.material = HoverExitArrow._normal;
	}

	// Token: 0x0400027F RID: 639
	private static Material _normal;

	// Token: 0x04000280 RID: 640
	private static Material _hover;
}
