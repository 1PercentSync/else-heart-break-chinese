using System;
using GameWorld2;

// Token: 0x020000C2 RID: 194
public class MemoryShell : Shell
{
	// Token: 0x1700008F RID: 143
	// (get) Token: 0x06000593 RID: 1427 RVA: 0x000266F4 File Offset: 0x000248F4
	public Memory memory
	{
		get
		{
			return base.ting as Memory;
		}
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x00026704 File Offset: 0x00024904
	public override void CreateTing()
	{
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x00026708 File Offset: 0x00024908
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
