using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000027 RID: 39
	public class MusicBox : MimanTing, TingWithButton
	{
		// Token: 0x06000376 RID: 886 RVA: 0x00012E60 File Offset: 0x00011060
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_onButtonPressedProgramName = base.EnsureCell<string>("onButtonPressedProgramName", "MusicBoxer");
			this.CELL_onUpdateProgramName = base.EnsureCell<string>("onUpdateProgramName", "BlankSlate");
			this.CELL_cutoffFrequency = base.EnsureCell<float>("cutoff", 5000f);
			this.CELL_small = base.EnsureCell<bool>("small", false);
			this.CELL_mixer = base.EnsureCell<bool>("mixer", false);
			this.CELL_loop = base.EnsureCell<bool>("loop", true);
			this.CELL_resonance = base.EnsureCell<float>("resonance", 1f);
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00012F04 File Offset: 0x00011104
		public override IntPoint[] interactionPoints
		{
			get
			{
				if (this.mixer)
				{
					return new IntPoint[] { base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 1 };
				}
				if (!this.small)
				{
					return new IntPoint[] { base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 2 };
				}
				return base.interactionPoints;
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00012F8C File Offset: 0x0001118C
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			if (!this.small)
			{
				base.FixGroupIfOutsideIslandOfTiles();
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00012FA0 File Offset: 0x000111A0
		public override bool DoesMasterProgramExist()
		{
			return this._onButtonPressedProgram != null;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00012FB0 File Offset: 0x000111B0
		public override void FixBeforeSaving()
		{
			if (base.name.ToLower().Contains("mixer"))
			{
				this.mixer = true;
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00012FE0 File Offset: 0x000111E0
		[SprakAPI(new string[] { "Print", "text" })]
		public void API_Print(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00012FF0 File Offset: 0x000111F0
		[SprakAPI(new string[] { "Set the cutoff frequency for the sound that is playing" })]
		public void API_SetCutoff(float pCutoff)
		{
			this.cutoffFrequency = pCutoff;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00012FFC File Offset: 0x000111FC
		[SprakAPI(new string[] { "Set the filter resonance for the sound that is playing" })]
		public void API_SetResonance(float pResonance)
		{
			this.resonance = pResonance;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00013008 File Offset: 0x00011208
		[SprakAPI(new string[] { "Set the pitch for the sound that is playing" })]
		public void API_SetPitch(float pPitch)
		{
			base.pitch = pPitch;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00013014 File Offset: 0x00011214
		[SprakAPI(new string[] { "Returns the time since day 0, whatever that means (in seconds)" })]
		public float API_Time()
		{
			return base.gameClock.totalSeconds;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00013030 File Offset: 0x00011230
		[SprakAPI(new string[] { "The sinus function", "x" })]
		public float API_Sin(float x)
		{
			return (float)Math.Sin((double)x);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0001303C File Offset: 0x0001123C
		[SprakAPI(new string[] { "The cosinus function", "x" })]
		public float API_Cos(float x)
		{
			return (float)Math.Cos((double)x);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00013048 File Offset: 0x00011248
		[SprakAPI(new string[] { "Set the sound to play in a loop" })]
		public void API_PlayLoop(string pSoundName)
		{
			base.soundName = pSoundName;
			base.isPlaying = true;
			base.audioTime = 0f;
			base.audioLoop = false;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00013078 File Offset: 0x00011278
		[SprakAPI(new string[] { "Set the sound to play" })]
		public void API_PlaySound(string pSoundName)
		{
			base.soundName = pSoundName;
			base.isPlaying = true;
			base.audioTime = 0f;
			base.audioLoop = false;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x000130A8 File Offset: 0x000112A8
		[SprakAPI(new string[] { "Start playing the current sound" })]
		public void API_Play()
		{
			base.isPlaying = true;
			base.audioTime = 0f;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x000130BC File Offset: 0x000112BC
		[SprakAPI(new string[] { "Stop the current sound" })]
		public void API_Stop()
		{
			base.isPlaying = false;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x000130C8 File Offset: 0x000112C8
		[SprakAPI(new string[] { "Pause or unpause the sound" })]
		public void API_TogglePause()
		{
			base.isPlaying = !base.isPlaying;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000130DC File Offset: 0x000112DC
		[SprakAPI(new string[] { "Pause the master program", "number of seconds to pause for" })]
		public void API_Sleep(float seconds)
		{
			this.onButtonProgram.sleepTimer = seconds;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000130EC File Offset: 0x000112EC
		[SprakAPI(new string[] { "Is a sound playing?" })]
		public bool API_IsPlaying()
		{
			return base.isPlaying;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000130F4 File Offset: 0x000112F4
		[SprakAPI(new string[] { "Get a random value between 0 and 1", "" })]
		public float API_Random()
		{
			return (float)MusicBox.s_random.NextDouble();
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00013110 File Offset: 0x00011310
		public void PushButton(Ting pUser)
		{
			base.PlaySound("Button");
			this.onButtonProgram.Start();
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00013128 File Offset: 0x00011328
		public override void Update(float dt)
		{
			if (base.isPlaying)
			{
				base.audioTime += dt;
				if (this.loop && base.audioTime > base.audioTotalLength)
				{
					base.audioTime = 0f;
					base.PlaySound(base.soundName);
				}
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00013184 File Offset: 0x00011384
		public override bool autoUnregisterFromUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00013188 File Offset: 0x00011388
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Locker;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00013194 File Offset: 0x00011394
		public override bool canBePickedUp
		{
			get
			{
				return !this.mixer && this.small;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600038F RID: 911 RVA: 0x000131AC File Offset: 0x000113AC
		public override string verbDescription
		{
			get
			{
				if (this.mixer)
				{
					return "use";
				}
				if (this.small)
				{
					return "press button";
				}
				return "press button on";
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000131D8 File Offset: 0x000113D8
		public override string tooltipName
		{
			get
			{
				if (this.mixer)
				{
					return "mixer";
				}
				if (this.small)
				{
					return "music box";
				}
				if (base.prefab.ToLower().Contains("theremin"))
				{
					return "theremin";
				}
				if (base.prefab.ToLower().Contains("gramophone"))
				{
					return "record player";
				}
				return "jukebox";
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0001324C File Offset: 0x0001144C
		// (set) Token: 0x06000392 RID: 914 RVA: 0x0001325C File Offset: 0x0001145C
		[EditableInEditor]
		public string onButtonPressedProgramName
		{
			get
			{
				return this.CELL_onButtonPressedProgramName.data;
			}
			set
			{
				this.CELL_onButtonPressedProgramName.data = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0001326C File Offset: 0x0001146C
		// (set) Token: 0x06000394 RID: 916 RVA: 0x0001327C File Offset: 0x0001147C
		[EditableInEditor]
		public string onUpdateProgramName
		{
			get
			{
				return this.CELL_onUpdateProgramName.data;
			}
			set
			{
				this.CELL_onUpdateProgramName.data = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0001328C File Offset: 0x0001148C
		public Program onButtonProgram
		{
			get
			{
				if (this._onButtonPressedProgram == null)
				{
					this._onButtonPressedProgram = base.EnsureProgram("OnButtonProgram", this.onButtonPressedProgramName);
					this._onButtonPressedProgram.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(MusicBox)));
				}
				return this._onButtonPressedProgram;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000396 RID: 918 RVA: 0x000132E4 File Offset: 0x000114E4
		public Program onUpdateProgram
		{
			get
			{
				if (this._onUpdateProgram == null)
				{
					this._onUpdateProgram = base.EnsureProgram("OnUpdateProgram", this.onUpdateProgramName);
					this._onUpdateProgram.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(MusicBox)));
				}
				return this._onUpdateProgram;
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0001333C File Offset: 0x0001153C
		public override void PrepareForBeingHacked()
		{
			if (this.onButtonProgram == null)
			{
				this.logger.Log("There was a problem generating the button program");
			}
			if (this.onUpdateProgram == null)
			{
				this.logger.Log("There was a problem generating the update program");
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000398 RID: 920 RVA: 0x00013380 File Offset: 0x00011580
		// (set) Token: 0x06000399 RID: 921 RVA: 0x00013390 File Offset: 0x00011590
		public float cutoffFrequency
		{
			get
			{
				return this.CELL_cutoffFrequency.data;
			}
			set
			{
				this.CELL_cutoffFrequency.data = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600039A RID: 922 RVA: 0x000133A0 File Offset: 0x000115A0
		// (set) Token: 0x0600039B RID: 923 RVA: 0x000133B0 File Offset: 0x000115B0
		public float resonance
		{
			get
			{
				return this.CELL_resonance.data;
			}
			set
			{
				this.CELL_resonance.data = value;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600039C RID: 924 RVA: 0x000133C0 File Offset: 0x000115C0
		// (set) Token: 0x0600039D RID: 925 RVA: 0x000133D0 File Offset: 0x000115D0
		[EditableInEditor]
		public bool small
		{
			get
			{
				return this.CELL_small.data;
			}
			set
			{
				this.CELL_small.data = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600039E RID: 926 RVA: 0x000133E0 File Offset: 0x000115E0
		// (set) Token: 0x0600039F RID: 927 RVA: 0x000133F0 File Offset: 0x000115F0
		[EditableInEditor]
		public bool mixer
		{
			get
			{
				return this.CELL_mixer.data;
			}
			set
			{
				this.CELL_mixer.data = value;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00013400 File Offset: 0x00011600
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x00013410 File Offset: 0x00011610
		[EditableInEditor]
		public bool loop
		{
			get
			{
				return this.CELL_loop.data;
			}
			set
			{
				this.CELL_loop.data = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00013420 File Offset: 0x00011620
		public bool isJukebox
		{
			get
			{
				return !this.small && !this.mixer;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0001343C File Offset: 0x0001163C
		public override Program masterProgram
		{
			get
			{
				return this.onButtonProgram;
			}
		}

		// Token: 0x040000DA RID: 218
		public new static string TABLE_NAME = "Ting_MusicBoxes";

		// Token: 0x040000DB RID: 219
		private ValueEntry<string> CELL_onButtonPressedProgramName;

		// Token: 0x040000DC RID: 220
		private ValueEntry<string> CELL_onUpdateProgramName;

		// Token: 0x040000DD RID: 221
		private ValueEntry<float> CELL_cutoffFrequency;

		// Token: 0x040000DE RID: 222
		private ValueEntry<float> CELL_resonance;

		// Token: 0x040000DF RID: 223
		private ValueEntry<bool> CELL_small;

		// Token: 0x040000E0 RID: 224
		private ValueEntry<bool> CELL_mixer;

		// Token: 0x040000E1 RID: 225
		private ValueEntry<bool> CELL_loop;

		// Token: 0x040000E2 RID: 226
		private Program _onButtonPressedProgram;

		// Token: 0x040000E3 RID: 227
		private Program _onUpdateProgram;

		// Token: 0x040000E4 RID: 228
		private static Random s_random = new Random(DateTime.Today.Millisecond * DateTime.Today.Second * DateTime.Today.Minute * DateTime.Today.Hour);
	}
}
