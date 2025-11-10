using System;
using System.Collections.Generic;
using GameTypes;
using GrimmLib;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000005 RID: 5
	public abstract class MimanTing : Ting
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002838 File Offset: 0x00000A38
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programObjectIds = base.EnsureCell<int[]>("programs", new int[0]);
			this.CELL_connectedTings = base.EnsureCell<string[]>("connections", new string[0]);
			this.CELL_emitsSmoke = base.EnsureCell<bool>("emitsSmoke", false);
			this.CELL_isPlaying = base.EnsureCell<bool>("isPlaying", false);
			this.CELL_pitch = base.EnsureCell<float>("pitch", 1f);
			this.CELL_soundName = base.EnsureCell<string>("soundName", "");
			this.CELL_audioTime = base.EnsureCell<float>("audioTime", 0f);
			this.CELL_audioTotalLength = base.EnsureCell<float>("audioTotalLength", 60f);
			this.CELL_audioLoop = base.EnsureCell<bool>("audioLoop", false);
			this.CELL_messageTimer = base.EnsureCell<float>("messageTimer", 0f);
			this.CELL_userDefinedLabel = base.EnsureCell<string>("userDefinedLabel", "");
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002934 File Offset: 0x00000B34
		internal void SetMimanRunners(ProgramRunner pProgramRunner, SourceCodeDispenser pSourceCodeDispenser, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			this._programRunner = pProgramRunner;
			this._sourceCodeDispenser = pSourceCodeDispenser;
			this._dialogueRunner = pDialogueRunner;
			this._worldSettings = pWorldSettings;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002954 File Offset: 0x00000B54
		public virtual void MaybeFixGroupIfOutsideIslandOfTiles()
		{
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002958 File Offset: 0x00000B58
		protected void FixGroupIfOutsideIslandOfTiles()
		{
			if (base.tile == null)
			{
				PointTileNode pointTileNode = new PointTileNode(base.localPoint, base.room);
				base.room.AddTile(pointTileNode);
				base.SetCachedTile();
			}
			for (int i = 0; i < this.interactionPoints.Length; i++)
			{
				PointTileNode tile = base.room.GetTile(this.interactionPoints[i]);
				if (tile != null)
				{
					if (base.tile.group == -1 && tile.group == -1)
					{
						throw new Exception("Both tile at position and tile at interaction point belong to group -1 for " + base.name);
					}
					if (base.tile.group != tile.group)
					{
						base.tile.group = tile.group;
						break;
					}
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002A34 File Offset: 0x00000C34
		public override string UseTingOnTingDescription(Ting pOtherTing)
		{
			if (pOtherTing is Locker && this.CanInteractWith(pOtherTing))
			{
				return "put " + this.tooltipName + " into locker";
			}
			if (pOtherTing is SendPipe && this.CanInteractWith(pOtherTing))
			{
				return "put " + this.tooltipName + " into pipe";
			}
			if (pOtherTing is TrashCan && this.CanInteractWith(pOtherTing))
			{
				return "throw " + this.tooltipName + " into trash can";
			}
			return base.UseTingOnTingDescription(pOtherTing);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002AD0 File Offset: 0x00000CD0
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002B44 File Offset: 0x00000D44
		public MimanTing[] connectedTings
		{
			get
			{
				string[] data = this.CELL_connectedTings.data;
				int num = data.Length;
				MimanTing[] array = new MimanTing[num];
				int num2 = 0;
				foreach (string text in data)
				{
					MimanTing mimanTing = this._tingRunner.GetTing(text) as MimanTing;
					D.isNull(mimanTing, "Miman ting is null (the cast failed)");
					array[num2++] = mimanTing;
				}
				return array;
			}
			set
			{
				string[] array = new string[value.Length];
				int num = 0;
				for (int i = 0; i < value.Length; i++)
				{
					MimanTing mimanTing = value[i];
					array[num++] = mimanTing.name;
				}
				this.CELL_connectedTings.data = array;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002B94 File Offset: 0x00000D94
		public float AddConnectionToTing(Ting pTing)
		{
			if (pTing == this)
			{
				return -1f;
			}
			D.isNull(pTing, "Can't connect to null");
			string name = pTing.name;
			string[] data = this.CELL_connectedTings.data;
			int num = 0;
			foreach (string text in data)
			{
				if (name == text)
				{
					return (float)num;
				}
				num++;
			}
			string[] array2 = new string[data.Length + 1];
			int num2 = 0;
			foreach (string text2 in data)
			{
				array2[num2++] = text2;
			}
			array2[num2] = name;
			this.CELL_connectedTings.data = array2;
			return (float)num2;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002C58 File Offset: 0x00000E58
		public virtual void Say(string pLine, string pConversation)
		{
			this.lastConversation = pConversation;
			if (base.dialogueLine != "")
			{
				base.dialogueLine = "";
			}
			this.messageTimer = this._worldSettings.totalWorldTime;
			base.dialogueLine = pLine;
			this._tingRunner.Register(this);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002CB0 File Offset: 0x00000EB0
		protected void UpdateBubbleTimer()
		{
			if (!this._dialogueLineIsEmpty_Cache && this._worldSettings.totalWorldTime - this.messageTimer > 3f)
			{
				this.Say("", "");
				if (this.autoUnregisterFromUpdate)
				{
					this._tingRunner.Unregister(this);
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002D0C File Offset: 0x00000F0C
		public virtual bool autoUnregisterFromUpdate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002D10 File Offset: 0x00000F10
		[ShowInEditor]
		public bool containsBrokenPrograms
		{
			get
			{
				foreach (Program program in this.programs)
				{
					if (program.ContainsErrors())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002D4C File Offset: 0x00000F4C
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002E10 File Offset: 0x00001010
		public Program[] programs
		{
			get
			{
				D.isNull(this._programRunner, "ProgramRunner must be set");
				D.isNull(this.CELL_programObjectIds, "CELL_programObjectIds is null");
				int[] data = this.CELL_programObjectIds.data;
				if (this.CELL_programObjectIds.data == null)
				{
					D.Log("CELL_programObjectIds.data is null");
					return new Program[0];
				}
				int num = data.Length;
				List<Program> list = new List<Program>();
				foreach (int num2 in data)
				{
					Program programUnsafe = this._programRunner.GetProgramUnsafe(num2);
					if (programUnsafe == null)
					{
						throw new Exception("Can't get program with id " + num2);
					}
					list.Add(programUnsafe);
				}
				return list.ToArray();
			}
			set
			{
				int[] array = new int[value.Length];
				int num = 0;
				for (int i = 0; i < value.Length; i++)
				{
					Program program = value[i];
					array[num++] = program.objectId;
				}
				this.CELL_programObjectIds.data = array;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002E60 File Offset: 0x00001060
		[ShowInEditor]
		public string programNames
		{
			get
			{
				Program[] programs = this.programs;
				int num = programs.Length;
				if (num == 0)
				{
					return "[] or not generated yet";
				}
				string[] array = new string[num];
				int num2 = 0;
				foreach (Program program in programs)
				{
					array[num2] = programs[num2].name;
					num2++;
				}
				return "[" + string.Join(",", array) + "]";
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002EDC File Offset: 0x000010DC
		public void AddProgramToProgramsArray(Program pNewProgram)
		{
			foreach (Program program in this.programs)
			{
				if (program == pNewProgram)
				{
					throw new Exception("Adding a program (" + pNewProgram.name + ") to programs array that is already in there");
				}
			}
			Program[] programs2 = this.programs;
			Program[] array = new Program[programs2.Length + 1];
			int num = 0;
			foreach (Program program2 in programs2)
			{
				array[num++] = program2;
			}
			array[num] = pNewProgram;
			this.programs = array;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002F80 File Offset: 0x00001180
		public Error[] ChangeAndRecompileProgram(string pProgramName, string pNewSourceCodeContent)
		{
			Program program = this.GetProgram(pProgramName);
			if (program == null)
			{
				throw new Exception("Can't find program '" + pProgramName + "' to change and recompile");
			}
			program.sourceCodeContent = pNewSourceCodeContent;
			return program.Compile();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002FC0 File Offset: 0x000011C0
		public Program GetProgram(string pProgramName)
		{
			foreach (Program program in this.programs)
			{
				if (program.name == pProgramName)
				{
					return program;
				}
			}
			return null;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003000 File Offset: 0x00001200
		public Program EnsureProgram(string pProgramName, string pNameOfSourceCodeToUseIfProgramDoesNotExist)
		{
			this.logger.Log("Ensuring program with name '" + pProgramName + "'...");
			Program program = this.GetProgram(pProgramName);
			if (program != null)
			{
				this.logger.Log("Program already existed");
				return program;
			}
			SourceCode sourceCode = this._sourceCodeDispenser.GetSourceCode(pNameOfSourceCodeToUseIfProgramDoesNotExist);
			Program program2 = this._programRunner.CreateProgram(pProgramName, sourceCode.content, pNameOfSourceCodeToUseIfProgramDoesNotExist);
			this.logger.Log(string.Concat(new object[] { "Created a new program with id ", program2.objectId, " and name ", program2.name, " from source ", sourceCode.name }));
			this.AddProgramToProgramsArray(program2);
			return program2;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000043 RID: 67
		public abstract Program masterProgram { get; }

		// Token: 0x06000044 RID: 68 RVA: 0x000030C0 File Offset: 0x000012C0
		public virtual void PrepareForBeingHacked()
		{
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000030C4 File Offset: 0x000012C4
		public virtual void Init()
		{
			base.ConnectToCurrentTile();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000030CC File Offset: 0x000012CC
		public void StartMasterProgramIfItIsOn()
		{
			if (this.masterProgram != null && this.masterProgram.isOn)
			{
				D.Log("(RE)STARTING ALREADY RUNNING PROGRAM IN " + base.name);
				this.PrepareForBeingHacked();
				this.masterProgram.Start();
			}
		}

		// Token: 0x06000047 RID: 71
		public abstract bool DoesMasterProgramExist();

		// Token: 0x06000048 RID: 72 RVA: 0x0000311C File Offset: 0x0000131C
		public void TurnDegrees(int pDegrees)
		{
			int num = (int)IntPoint.DirectionToIntPoint(base.direction).Degrees();
			num -= pDegrees;
			base.direction = GridMath.DegreesToDirection(num);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003150 File Offset: 0x00001350
		public void PlaySound(string pKey)
		{
			if (this.onPlaySound != null)
			{
				this.onPlaySound(pKey);
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003170 File Offset: 0x00001370
		public virtual void OnPutDown()
		{
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00003174 File Offset: 0x00001374
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00003184 File Offset: 0x00001384
		[EditableInEditor]
		public bool emitsSmoke
		{
			get
			{
				return this.CELL_emitsSmoke.data;
			}
			set
			{
				this.CELL_emitsSmoke.data = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003194 File Offset: 0x00001394
		public virtual int securityLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00003198 File Offset: 0x00001398
		// (set) Token: 0x0600004F RID: 79 RVA: 0x000031A8 File Offset: 0x000013A8
		[EditableInEditor]
		public float audioTime
		{
			get
			{
				return this.CELL_audioTime.data;
			}
			set
			{
				this.CELL_audioTime.data = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000031B8 File Offset: 0x000013B8
		// (set) Token: 0x06000051 RID: 81 RVA: 0x000031C8 File Offset: 0x000013C8
		[EditableInEditor]
		public string soundName
		{
			get
			{
				return this.CELL_soundName.data;
			}
			set
			{
				this.CELL_soundName.data = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000031D8 File Offset: 0x000013D8
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000031E8 File Offset: 0x000013E8
		[EditableInEditor]
		public bool isPlaying
		{
			get
			{
				return this.CELL_isPlaying.data;
			}
			set
			{
				this.CELL_isPlaying.data = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000031F8 File Offset: 0x000013F8
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00003208 File Offset: 0x00001408
		[EditableInEditor]
		public float audioTotalLength
		{
			get
			{
				return this.CELL_audioTotalLength.data;
			}
			set
			{
				this.CELL_audioTotalLength.data = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003218 File Offset: 0x00001418
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00003228 File Offset: 0x00001428
		[EditableInEditor]
		public bool audioLoop
		{
			get
			{
				return this.CELL_audioLoop.data;
			}
			set
			{
				this.CELL_audioLoop.data = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003238 File Offset: 0x00001438
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00003248 File Offset: 0x00001448
		public float pitch
		{
			get
			{
				return this.CELL_pitch.data;
			}
			set
			{
				this.CELL_pitch.data = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003258 File Offset: 0x00001458
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00003268 File Offset: 0x00001468
		[EditableInEditor]
		public float messageTimer
		{
			get
			{
				return this.CELL_messageTimer.data;
			}
			set
			{
				this.CELL_messageTimer.data = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00003278 File Offset: 0x00001478
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00003288 File Offset: 0x00001488
		public string userDefinedLabel
		{
			get
			{
				return this.CELL_userDefinedLabel.data;
			}
			set
			{
				this.CELL_userDefinedLabel.data = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003298 File Offset: 0x00001498
		public SourceCodeDispenser sourceCodeDispenser
		{
			get
			{
				return this._sourceCodeDispenser;
			}
		}

		// Token: 0x04000012 RID: 18
		protected ProgramRunner _programRunner;

		// Token: 0x04000013 RID: 19
		protected SourceCodeDispenser _sourceCodeDispenser;

		// Token: 0x04000014 RID: 20
		protected DialogueRunner _dialogueRunner;

		// Token: 0x04000015 RID: 21
		protected WorldSettings _worldSettings;

		// Token: 0x04000016 RID: 22
		private ValueEntry<int[]> CELL_programObjectIds;

		// Token: 0x04000017 RID: 23
		private ValueEntry<string[]> CELL_connectedTings;

		// Token: 0x04000018 RID: 24
		private ValueEntry<bool> CELL_emitsSmoke;

		// Token: 0x04000019 RID: 25
		private ValueEntry<bool> CELL_isPlaying;

		// Token: 0x0400001A RID: 26
		private ValueEntry<string> CELL_soundName;

		// Token: 0x0400001B RID: 27
		private ValueEntry<float> CELL_pitch;

		// Token: 0x0400001C RID: 28
		private ValueEntry<float> CELL_audioTime;

		// Token: 0x0400001D RID: 29
		private ValueEntry<float> CELL_audioTotalLength;

		// Token: 0x0400001E RID: 30
		private ValueEntry<bool> CELL_audioLoop;

		// Token: 0x0400001F RID: 31
		private ValueEntry<float> CELL_messageTimer;

		// Token: 0x04000020 RID: 32
		private ValueEntry<string> CELL_userDefinedLabel;

		// Token: 0x04000021 RID: 33
		public MimanTing.OnPlaySound onPlaySound;

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x06000060 RID: 96
		public delegate void OnPlaySound(string pKey);
	}
}
