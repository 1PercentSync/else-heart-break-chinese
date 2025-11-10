using System;
using GameWorld2;

// Token: 0x020000AB RID: 171
public class ButtonShell : Shell
{
	// Token: 0x17000071 RID: 113
	// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00020910 File Offset: 0x0001EB10
	public Button button
	{
		get
		{
			return base.ting as Button;
		}
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00020920 File Offset: 0x0001EB20
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00020924 File Offset: 0x0001EB24
	public override void CreateTing()
	{
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x00020928 File Offset: 0x0001EB28
	protected override void Setup()
	{
		base.Setup();
		SoundDictionary.LoadSingleSound("Bird", "Seagull sound 8");
		SoundDictionary.LoadSingleSound("Boop", "Blip 0");
		SoundDictionary.LoadSingleSound("FiveBlips", "Blip 1");
	}
}
