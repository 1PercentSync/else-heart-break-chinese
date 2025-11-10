using System;
using GameWorld2;

// Token: 0x020000C7 RID: 199
public class PointShell : Shell
{
	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060005C2 RID: 1474 RVA: 0x000271A8 File Offset: 0x000253A8
	public Point point
	{
		get
		{
			return base.ting as Point;
		}
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x000271B8 File Offset: 0x000253B8
	public override void CreateTing()
	{
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x000271BC File Offset: 0x000253BC
	protected override void Setup()
	{
		base.Setup();
		base.renderer.enabled = false;
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x000271D0 File Offset: 0x000253D0
	protected override void UpdateTooltip()
	{
		this._showTooltip = false;
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x000271DC File Offset: 0x000253DC
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
