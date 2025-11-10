using System;
using GameWorld2;

// Token: 0x020000C1 RID: 193
public class MapShell : Shell
{
	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600058F RID: 1423 RVA: 0x000266D4 File Offset: 0x000248D4
	public Map map
	{
		get
		{
			return base.ting as Map;
		}
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x000266E4 File Offset: 0x000248E4
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x000266E8 File Offset: 0x000248E8
	public override void CreateTing()
	{
	}
}
