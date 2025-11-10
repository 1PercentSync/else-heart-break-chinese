using System;
using GameWorld2;

// Token: 0x020000D5 RID: 213
public class TeleporterShell : Shell
{
	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06000613 RID: 1555 RVA: 0x00027BC0 File Offset: 0x00025DC0
	public Teleporter teleporter
	{
		get
		{
			return base.ting as Teleporter;
		}
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x00027BD0 File Offset: 0x00025DD0
	public override void CreateTing()
	{
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x00027BD4 File Offset: 0x00025DD4
	protected override void Setup()
	{
		base.Setup();
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00027BDC File Offset: 0x00025DDC
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x00027BE4 File Offset: 0x00025DE4
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
	}
}
