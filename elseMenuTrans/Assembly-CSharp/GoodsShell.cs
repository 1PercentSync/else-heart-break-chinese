using System;
using GameWorld2;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class GoodsShell : Shell
{
	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000563 RID: 1379 RVA: 0x00026128 File Offset: 0x00024328
	public Goods goods
	{
		get
		{
			return base.ting as Goods;
		}
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x00026138 File Offset: 0x00024338
	protected override void Setup()
	{
		base.Setup();
		this._renderer = base.GetComponentInChildren<Renderer>();
		this._light = base.GetComponentInChildren<Light>();
		this.RefreshColor();
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0002616C File Offset: 0x0002436C
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x00026170 File Offset: 0x00024370
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
		this.RefreshColor();
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00026180 File Offset: 0x00024380
	private void RefreshColor()
	{
		this._renderer.material.color = new Color(this.goods.GetPureness(), 0.5f + this.goods.GetPureness(), 0.5f + this.goods.GetPureness());
		if (this._light)
		{
			this._light.intensity = 0.5f + this.goods.GetPureness();
		}
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x000261FC File Offset: 0x000243FC
	public override void CreateTing()
	{
	}

	// Token: 0x040003E0 RID: 992
	private Renderer _renderer;

	// Token: 0x040003E1 RID: 993
	private Light _light;
}
