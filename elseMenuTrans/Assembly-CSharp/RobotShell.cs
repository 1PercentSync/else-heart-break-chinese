using System;
using GameWorld2;

// Token: 0x020000CA RID: 202
public class RobotShell : Shell
{
	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0002772C File Offset: 0x0002592C
	public Robot robot
	{
		get
		{
			return base.ting as Robot;
		}
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0002773C File Offset: 0x0002593C
	public override void CreateTing()
	{
	}
}
