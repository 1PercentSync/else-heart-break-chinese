using System;
using GameWorld2;

// Token: 0x020000B3 RID: 179
public class ExtractorShell : Shell
{
	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600053C RID: 1340 RVA: 0x00025EF0 File Offset: 0x000240F0
	// (set) Token: 0x0600053D RID: 1341 RVA: 0x00025EF8 File Offset: 0x000240F8
	public string clipboard
	{
		get
		{
			return ClipboardHelper.clipboard;
		}
		set
		{
			ClipboardHelper.clipboard = value;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x0600053E RID: 1342 RVA: 0x00025F00 File Offset: 0x00024100
	public Extractor extractor
	{
		get
		{
			return base.ting as Extractor;
		}
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x00025F10 File Offset: 0x00024110
	public override void CreateTing()
	{
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00025F14 File Offset: 0x00024114
	protected override void Setup()
	{
		base.Setup();
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x00025F1C File Offset: 0x0002411C
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00025F24 File Offset: 0x00024124
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00025F2C File Offset: 0x0002412C
	private void CopyToClipboard(string text)
	{
		this.clipboard = text;
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x00025F38 File Offset: 0x00024138
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}
}
