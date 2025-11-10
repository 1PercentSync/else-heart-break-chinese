using System;
using GameWorld2;

// Token: 0x0200001B RID: 27
public class GameViewControls
{
	// Token: 0x060000CE RID: 206 RVA: 0x00005F80 File Offset: 0x00004180
	public GameViewControls(Fade pFade, RoomChanger pRoomChanger, GreatCamera pCamera, CommandLine pCommandLine, BubbleCanvasController pBubbleCanvasController, DepthOfFieldScatter pDepthOfField)
	{
		this.fade = pFade;
		this.roomChanger = pRoomChanger;
		this.camera = pCamera;
		this.commandLine = pCommandLine;
		this.bubbleManager = pBubbleCanvasController;
		this.depthOfField = pDepthOfField;
		if (this.camera == null || this.roomChanger == null || this.fade == null || this.world == null || this.commandLine == null || this.bubbleManager == null || this.depthOfField == null)
		{
			throw new Exception("None of these can be null!");
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x060000CF RID: 207 RVA: 0x0000603C File Offset: 0x0000423C
	// (set) Token: 0x060000D0 RID: 208 RVA: 0x00006044 File Offset: 0x00004244
	public CommandLine commandLine { get; private set; }

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x060000D1 RID: 209 RVA: 0x00006050 File Offset: 0x00004250
	// (set) Token: 0x060000D2 RID: 210 RVA: 0x00006058 File Offset: 0x00004258
	public GreatCamera camera { get; private set; }

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060000D3 RID: 211 RVA: 0x00006064 File Offset: 0x00004264
	// (set) Token: 0x060000D4 RID: 212 RVA: 0x00006070 File Offset: 0x00004270
	public World world
	{
		get
		{
			return WorldOwner.instance.world;
		}
		set
		{
			WorldOwner.instance.world = value;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060000D5 RID: 213 RVA: 0x00006080 File Offset: 0x00004280
	// (set) Token: 0x060000D6 RID: 214 RVA: 0x00006088 File Offset: 0x00004288
	public Fade fade { get; private set; }

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x060000D7 RID: 215 RVA: 0x00006094 File Offset: 0x00004294
	// (set) Token: 0x060000D8 RID: 216 RVA: 0x0000609C File Offset: 0x0000429C
	public RoomChanger roomChanger { get; private set; }

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060000D9 RID: 217 RVA: 0x000060A8 File Offset: 0x000042A8
	// (set) Token: 0x060000DA RID: 218 RVA: 0x000060B0 File Offset: 0x000042B0
	public BubbleCanvasController bubbleManager { get; private set; }

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060000DB RID: 219 RVA: 0x000060BC File Offset: 0x000042BC
	// (set) Token: 0x060000DC RID: 220 RVA: 0x000060C4 File Offset: 0x000042C4
	public DepthOfFieldScatter depthOfField { get; private set; }

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060000DD RID: 221 RVA: 0x000060D0 File Offset: 0x000042D0
	public MimanTing avatar
	{
		get
		{
			if (WorldOwner.instance.worldIsLoaded)
			{
				string avatarName = this.world.settings.avatarName;
				return this.world.tingRunner.GetTingUnsafe(avatarName) as MimanTing;
			}
			return null;
		}
	}

	// Token: 0x04000081 RID: 129
	public float deltaTimeMultiplier = 1f;

	// Token: 0x04000082 RID: 130
	public bool pauseWorld;

	// Token: 0x04000083 RID: 131
	public int updatesPerFrame = 1;
}
