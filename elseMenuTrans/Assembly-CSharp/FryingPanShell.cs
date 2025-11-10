using System;
using GameWorld2;

// Token: 0x020000B8 RID: 184
public class FryingPanShell : Shell
{
	// Token: 0x17000084 RID: 132
	// (get) Token: 0x0600055B RID: 1371 RVA: 0x000260E8 File Offset: 0x000242E8
	public FryingPan fryingPan
	{
		get
		{
			return base.ting as FryingPan;
		}
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x000260F8 File Offset: 0x000242F8
	public override void CreateTing()
	{
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x000260FC File Offset: 0x000242FC
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
