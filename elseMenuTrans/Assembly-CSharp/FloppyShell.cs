using System;
using GameWorld2;

// Token: 0x020000B5 RID: 181
public class FloppyShell : Shell
{
	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600054A RID: 1354 RVA: 0x00025F64 File Offset: 0x00024164
	public Floppy floppy
	{
		get
		{
			return base.ting as Floppy;
		}
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x00025F74 File Offset: 0x00024174
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x00025F78 File Offset: 0x00024178
	public override void CreateTing()
	{
	}
}
