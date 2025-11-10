using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameTypes;
using GrimmLib;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000002 RID: 2
	public sealed class World : IPreloadable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020EC File Offset: 0x000002EC
		public World(RelayTwo pRelay)
		{
			this.Init(pRelay);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020FC File Offset: 0x000002FC
		public World(string pFilepath)
		{
			this.Init(new RelayTwo(pFilepath));
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002110 File Offset: 0x00000310
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002118 File Offset: 0x00000318
		public bool isReadyToPlay { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002124 File Offset: 0x00000324
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000212C File Offset: 0x0000032C
		public bool paused { get; set; }

		// Token: 0x06000007 RID: 7 RVA: 0x00002138 File Offset: 0x00000338
		private void Init(RelayTwo pRelay)
		{
			this.paused = false;
			this.isReadyToPlay = false;
			this.relay = pRelay;
			this.dialogueRunner = new DialogueRunner(this.relay, Language.SWEDISH);
			this.roomRunner = new RoomRunner(this.relay);
			this.programRunner = new ProgramRunner(this.relay);
			this.sourceCodeDispenser = new SourceCodeDispenser(this.relay);
			this.timetableRunner = new TimetableRunner(this.relay);
			this.settings = new WorldSettings(this.relay);
			this.tingRunner = new MimanTingRunner(this.relay, this.dialogueRunner, this.programRunner, this.sourceCodeDispenser, this.roomRunner, this.timetableRunner, this.settings);
			this.grimmApiDefinitions = new MimanGrimmApiDefinitions(this);
			this.grimmApiDefinitions.RegisterFunctions();
			this.grimmApiDefinitions.RegisterExpressions();
			this.translator = new Translator(Translator.Language.SWEDISH);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002224 File Offset: 0x00000424
		public void Save(string pFilepath)
		{
			this.relay.SaveAll(pFilepath);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002234 File Offset: 0x00000434
		public void Update(float dt)
		{
			if (!this.isReadyToPlay)
			{
				throw new Exception("Must preload before update!");
			}
			if (this.paused)
			{
				return;
			}
			this.settings.tickNr++;
			this.settings.totalWorldTime += dt;
			this.settings.gameTimeSeconds += dt * this.settings.gameTimeSpeed;
			this.settings.UpdateRain(dt);
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			this.programRunner.Update(dt);
			stopwatch.Stop();
			double totalSeconds = stopwatch.Elapsed.TotalSeconds;
			stopwatch.Reset();
			stopwatch.Start();
			this.dialogueRunner.Update(dt);
			double totalSeconds2 = stopwatch.Elapsed.TotalSeconds;
			stopwatch.Reset();
			stopwatch.Start();
			this.tingRunner.Update(dt, this.settings.gameTimeClock, this.settings.totalWorldTime);
			double totalSeconds3 = stopwatch.Elapsed.TotalSeconds;
			double num = totalSeconds + totalSeconds2 + totalSeconds3;
			if (num > 0.1599999964237213)
			{
				D.Log(string.Concat(new object[] { "TIMING DATA SLOW FRAME (", num, " s.) | programs: ", totalSeconds, ", dialogue: ", totalSeconds2, ", tings: ", totalSeconds3 }));
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000023B8 File Offset: 0x000005B8
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000023C0 File Offset: 0x000005C0
		public MimanTingRunner tingRunner { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000023CC File Offset: 0x000005CC
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000023D4 File Offset: 0x000005D4
		public DialogueRunner dialogueRunner { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000023E0 File Offset: 0x000005E0
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000023E8 File Offset: 0x000005E8
		public RoomRunner roomRunner { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000023F4 File Offset: 0x000005F4
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000023FC File Offset: 0x000005FC
		public ProgramRunner programRunner { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002408 File Offset: 0x00000608
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002410 File Offset: 0x00000610
		public SourceCodeDispenser sourceCodeDispenser { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000014 RID: 20 RVA: 0x0000241C File Offset: 0x0000061C
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002424 File Offset: 0x00000624
		public RelayTwo relay { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002430 File Offset: 0x00000630
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002438 File Offset: 0x00000638
		public WorldSettings settings { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002444 File Offset: 0x00000644
		// (set) Token: 0x06000019 RID: 25 RVA: 0x0000244C File Offset: 0x0000064C
		public TimetableRunner timetableRunner { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002458 File Offset: 0x00000658
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002460 File Offset: 0x00000660
		public MimanGrimmApiDefinitions grimmApiDefinitions { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001C RID: 28 RVA: 0x0000246C File Offset: 0x0000066C
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002474 File Offset: 0x00000674
		public Translator translator { get; private set; }

		// Token: 0x0600001E RID: 30 RVA: 0x00002480 File Offset: 0x00000680
		public IEnumerable<string> Preload()
		{
			yield return "Preparing rooms";
			Stopwatch roomPreloadTimer = new Stopwatch();
			roomPreloadTimer.Start();
			foreach (string s in this.roomRunner.Preload())
			{
				yield return s;
			}
			roomPreloadTimer.Stop();
			yield return "Preparing programs";
			foreach (Ting ting in this.tingRunner.GetTings())
			{
				MimanTing t = (MimanTing)ting;
				t.PrepareForBeingHacked();
				t.MaybeFixGroupIfOutsideIslandOfTiles();
				t.StartMasterProgramIfItIsOn();
			}
			this.RefreshTranslationLanguage();
			MimanPathfinder2.ClearRoomNetwork();
			this.isReadyToPlay = true;
			yield break;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024A4 File Offset: 0x000006A4
		public void RefreshTranslationLanguage()
		{
			Dictionary<string, Translator.Language> dictionary = new Dictionary<string, Translator.Language>
			{
				{
					"swe",
					Translator.Language.SWEDISH
				},
				{
					"eng",
					Translator.Language.ENGLISH
				},
				{
					"lat",
					Translator.Language.LATIN
				}
			};
			this.translator.SetLanguage(dictionary[this.settings.translationLanguage]);
		}
	}
}
