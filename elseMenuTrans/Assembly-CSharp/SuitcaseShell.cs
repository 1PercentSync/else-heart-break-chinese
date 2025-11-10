using System;
using GameWorld2;

// Token: 0x020000D2 RID: 210
public class SuitcaseShell : Shell
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06000603 RID: 1539 RVA: 0x00027A7C File Offset: 0x00025C7C
	public Suitcase suitcase
	{
		get
		{
			return base.ting as Suitcase;
		}
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x00027A8C File Offset: 0x00025C8C
	public override void CreateTing()
	{
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x00027A90 File Offset: 0x00025C90
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
