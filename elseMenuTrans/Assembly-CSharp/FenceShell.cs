using System;
using GameWorld2;

// Token: 0x020000B4 RID: 180
public class FenceShell : Shell
{
	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000546 RID: 1350 RVA: 0x00025F44 File Offset: 0x00024144
	public Fence fence
	{
		get
		{
			return base.ting as Fence;
		}
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00025F54 File Offset: 0x00024154
	public override void CreateTing()
	{
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00025F58 File Offset: 0x00024158
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
