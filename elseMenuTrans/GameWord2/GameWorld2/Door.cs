using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200000E RID: 14
	public class Door : MimanTing, IExit
	{
		// Token: 0x0600013E RID: 318 RVA: 0x000086A0 File Offset: 0x000068A0
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_isLocked = base.EnsureCell<bool>("isLocked", false);
			this.CELL_targetDoorName = base.EnsureCell<string>("targetDoorName", "");
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "OnDoorUsed");
			this.CELL_elevatorAlternatives = base.EnsureCell<string[]>("elevatorAlternatives", new string[0]);
			this.CELL_elevatorFloor = base.EnsureCell<int>("elevatorFloor", 0);
			this.CELL_isElevatorEntrance = base.EnsureCell<bool>("isElevatorEntrance", false);
			this.CELL_code = base.EnsureCell<int>("code", 0);
			this.CELL_autoLockTimer = base.EnsureCell<float>("autoLockTimer", 0f);
			this.CELL_isMoveable = base.EnsureCell<bool>("isMoveable", false);
			this.CELL_hasDoorKnob = base.EnsureCell<bool>("hasDoorKnob", true);
			this.CELL_useForRoomPathfinding = base.EnsureCell<bool>("useForRoomPathfinding", true);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000878C File Offset: 0x0000698C
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008794 File Offset: 0x00006994
		public void ResetAttempts()
		{
			this._attempts = 0;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000087A0 File Offset: 0x000069A0
		public void IncreaseAttempts()
		{
			this._attempts++;
			if (this._attempts > 25)
			{
				D.Log(string.Concat(new object[] { "Failures to get through door ", base.name, " reached ", 25, ", will reset code" }));
				this._program.sourceCodeContent = this._sourceCodeDispenser.GetSourceCode(this._program.sourceCodeName).content;
				this._program.Compile();
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000883C File Offset: 0x00006A3C
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000884C File Offset: 0x00006A4C
		public override void FixBeforeSaving()
		{
			if (base.prefab.ToLower().Contains("elevator"))
			{
				this.hasDoorKnob = false;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000887C File Offset: 0x00006A7C
		public override void Update(float dt)
		{
			if (this.autoLockTimer > 0f)
			{
				this.autoLockTimer -= dt;
				if (this.autoLockTimer < 0f)
				{
					this.isLocked = true;
				}
			}
			base.UpdateBubbleTimer();
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000088C4 File Offset: 0x00006AC4
		public override bool autoUnregisterFromUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000088C8 File Offset: 0x00006AC8
		private void WriteTargetToTile()
		{
			D.isNull(base.room, "room is null");
			D.isNull(this._roomRunner, "room runner is null!");
			PointTileNode tile = base.room.GetTile(this.interactionPoints[0]);
			if (tile == null)
			{
				return;
			}
			tile.AddOccupant(this);
			if (this.targetPosition == WorldCoordinate.NONE)
			{
				tile.teleportTarget = null;
			}
			else
			{
				tile.teleportTarget = this._roomRunner.GetRoom(this.targetPosition.roomName).GetTile(this.targetPosition.localPosition);
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00008974 File Offset: 0x00006B74
		public void Open()
		{
			base.StartAction("Opening", null, 2f, 2f);
		}

		// Token: 0x06000148 RID: 328
		[SprakAPI(new string[] { "Lock the door, returns success/fail锁门，返回成功/失败", "The security code安全代码" })]
		public bool API_Lock(float code)
		{
			return this.Lock(code);
		}

		// Token: 0x06000149 RID: 329
		[SprakAPI(new string[] { "Unlock the door, returns success/fail解锁，返回成功/失败", "The security code安全代码" })]
		public bool API_Unlock(float code)
		{
			return this.Unlock(code);
		}

		// Token: 0x0600014A RID: 330
		public bool Lock(float pCode)
		{
			if ((int)pCode == this.code)
			{
				this.isLocked = true;
				this.SyncLockOnTargetDoor();
				D.Log(base.name + " was locked with code " + pCode.ToString());
				this.Say("Locked!已上锁！", "");
				base.PlaySound("DoorLock");
				base.audioLoop = false;
				return true;
			}
			D.Log(base.name + " FAILED to be locked with code " + pCode.ToString());
			this.Say("Key is not working, invalid code:钥匙无效，错误代码: " + pCode.ToString(), "");
			return false;
		}

		// Token: 0x0600014B RID: 331
		public bool Unlock(float pCode)
		{
			if ((int)pCode == this.code || this.code == 0)
			{
				this.isLocked = false;
				this.SyncLockOnTargetDoor();
				this.Say("Unlocked!已解锁！", "");
				base.PlaySound("DoorUnlock");
				base.audioLoop = false;
				return true;
			}
			D.Log(base.name + " FAILED to be unlocked with code " + pCode.ToString());
			this.Say("Key is not working, invalid code:钥匙无效，错误代码: " + pCode.ToString(), "");
			return false;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00008AD4 File Offset: 0x00006CD4
		private void SyncLockOnTargetDoor()
		{
			if (this.targetDoor != null)
			{
				this.targetDoor.isLocked = this.isLocked;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00008B00 File Offset: 0x00006D00
		public void WalkThrough(Character pUser)
		{
			if (pUser == null)
			{
				D.Log("Can't call WalkThrough with pUser == null !");
				return;
			}
			if (this.masterProgram.ContainsErrors())
			{
				this.masterProgram.ClearErrors();
				this.masterProgram.Compile();
			}
			this._user = pUser;
			this.IncreaseAttempts();
			this.masterProgram.Start();
		}

		// Token: 0x0600014E RID: 334
		[SprakAPI(new string[] { "Walk out through another door穿过并从另一扇门出去" })]
		public void API_Goto(string doorName)
		{
			if (this._user == null)
			{
				D.Log("User for door " + base.name + " is null");
				return;
			}
			Door door = this._tingRunner.GetTingUnsafe(doorName) as Door;
			if (door != null)
			{
				Room room = door.room;
				IntPoint intPoint = door.interactionPoints[0];
				int x = intPoint.x;
				int y = intPoint.y;
				this.PushAwayBlockers(room, x, y, IntPoint.DirectionToIntPoint(door.direction));
				WorldCoordinate worldCoordinate = new WorldCoordinate(room.name, x, y);
				this._user.targetPositionInRoom = door.position;
				this._user.position = worldCoordinate;
				this._user.direction = door.direction;
				this._user.StopAction();
				this._user.StartAction("WalkingThroughDoorPhase2", null, 1.35f, 1.35f);
				this.ResetAttempts();
				this._dialogueRunner.EventHappened(this._user.name + "_open_" + base.name);
				if (this.isElevatorEntrance)
				{
					door.elevatorFloor = this.elevatorFloor;
				}
				return;
			}
			throw new Error("Can't find door with name不能找到名称为 " + doorName + "的门");
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00008CA8 File Offset: 0x00006EA8
		private void PushAwayBlockers(Room targetRoom, int x, int y, IntPoint pushDir)
		{
			PointTileNode tile = targetRoom.GetTile(x, y);
			if (tile == null)
			{
				return;
			}
			Ting[] occupants = tile.GetOccupants();
			foreach (Ting ting in occupants)
			{
				if (ting is Character)
				{
					WorldCoordinate worldCoordinate = new WorldCoordinate(ting.position.roomName, ting.position.localPosition + pushDir * 2);
					if (targetRoom.GetTile(worldCoordinate.localPosition) != null)
					{
						ting.position = worldCoordinate;
					}
					else
					{
						D.Log(string.Concat(new object[] { "Can't push ", ting.name, " to ", worldCoordinate, " because there is no tile there" }));
					}
				}
			}
		}

		// Token: 0x06000150 RID: 336
		[SprakAPI(new string[] { "Get the name of the person using the door获取正在使用门的人的名称" })]
		public string API_GetUser()
		{
			if (this._user != null)
			{
				return this._user.name;
			}
			return "";
		}

		// Token: 0x06000151 RID: 337
		[SprakAPI(new string[] { "Stop the person to walk through the door阻止人穿过这扇门" })]
		public void API_StopUser()
		{
			if (this._user != null)
			{
				this._user.StopAction();
				this._user.ClearState();
			}
		}

		// Token: 0x06000152 RID: 338
		[SprakAPI(new string[] { "Say something说点什么" })]
		public void API_Say(string message)
		{
			this.Say(message, "");
		}

		// Token: 0x06000153 RID: 339
		[SprakAPI(new string[] { "Pause the master program暂停主程序", "number of seconds to pause for暂停的秒数" })]
		public void API_Sleep(float seconds)
		{
			this.masterProgram.sleepTimer = seconds;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00008DE8 File Offset: 0x00006FE8
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00008DF8 File Offset: 0x00006FF8
		[EditableInEditor]
		public bool isLocked
		{
			get
			{
				return this.CELL_isLocked.data;
			}
			set
			{
				this.CELL_isLocked.data = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00008E08 File Offset: 0x00007008
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00008E18 File Offset: 0x00007018
		[EditableInEditor]
		public bool isMoveable
		{
			get
			{
				return this.CELL_isMoveable.data;
			}
			set
			{
				this.CELL_isMoveable.data = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00008E28 File Offset: 0x00007028
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00008E38 File Offset: 0x00007038
		[EditableInEditor]
		public bool isElevatorEntrance
		{
			get
			{
				return this.CELL_isElevatorEntrance.data;
			}
			set
			{
				this.CELL_isElevatorEntrance.data = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00008E48 File Offset: 0x00007048
		[ShowInEditor]
		public WorldCoordinate targetPosition
		{
			get
			{
				Door targetDoor = this.targetDoor;
				if (targetDoor != null)
				{
					return new WorldCoordinate(targetDoor.room.name, targetDoor.interactionPoints[0]);
				}
				return WorldCoordinate.NONE;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00008E8C File Offset: 0x0000708C
		public Door targetDoor
		{
			get
			{
				return this._tingRunner.GetTingUnsafe(this.targetDoorName) as Door;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00008EA4 File Offset: 0x000070A4
		[ShowInEditor]
		public string targetDoorReferenceStatus
		{
			get
			{
				if (this.targetDoor == null)
				{
					return "null";
				}
				return "OK, " + this.targetDoor.name;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00008ED8 File Offset: 0x000070D8
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00008EE8 File Offset: 0x000070E8
		[EditableInEditor]
		public string targetDoorName
		{
			get
			{
				return this.CELL_targetDoorName.data;
			}
			set
			{
				this.CELL_targetDoorName.data = value;
				this.WriteTargetToTile();
				MimanPathfinder2.ClearRoomNetwork();
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00008F04 File Offset: 0x00007104
		[ShowInEditor]
		public string elevatorAlternativesString
		{
			get
			{
				return string.Join(", ", this.CELL_elevatorAlternatives.data);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00008F1C File Offset: 0x0000711C
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00008F2C File Offset: 0x0000712C
		public string[] elevatorAlternatives
		{
			get
			{
				return this.CELL_elevatorAlternatives.data;
			}
			set
			{
				this.CELL_elevatorAlternatives.data = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00008F3C File Offset: 0x0000713C
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00008F4C File Offset: 0x0000714C
		[EditableInEditor]
		public int elevatorFloor
		{
			get
			{
				return this.CELL_elevatorFloor.data;
			}
			set
			{
				if (this.CELL_elevatorFloor.data != value)
				{
					base.StartAction("Moving", null, 4f, 4f);
				}
				this.CELL_elevatorFloor.data = value;
				if (this.elevatorFloor >= 0 && this.elevatorFloor < this.elevatorAlternatives.Length)
				{
					this.CELL_targetDoorName.data = this.elevatorAlternatives[this.elevatorFloor];
					this.WriteTargetToTile();
					this.SetSourceCodeFromDoorTarget();
				}
				else
				{
					D.Log("Can't go to floor " + this.elevatorFloor);
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00008FF0 File Offset: 0x000071F0
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00009000 File Offset: 0x00007200
		[EditableInEditor]
		public int code
		{
			get
			{
				return this.CELL_code.data;
			}
			set
			{
				this.CELL_code.data = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00009010 File Offset: 0x00007210
		public override string tooltipName
		{
			get
			{
				return "door门";
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00009018 File Offset: 0x00007218
		public override string verbDescription
		{
			get
			{
				return "open开";
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00009020 File Offset: 0x00007220
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[] { base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 3 };
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000905C File Offset: 0x0000725C
		public IntPoint waitingPoint
		{
			get
			{
				return base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 4;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00009088 File Offset: 0x00007288
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00009098 File Offset: 0x00007298
		[EditableInEditor]
		public string masterProgramName
		{
			get
			{
				return this.CELL_programName.data;
			}
			set
			{
				this.CELL_programName.data = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600016C RID: 364 RVA: 0x000090A8 File Offset: 0x000072A8
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					if (this.masterProgramName == "OnDoorUsed" || this.masterProgramName == "MinistryDoor")
					{
						string text = "SpecialProgram_" + base.name;
						this.masterProgramName = text;
						bool flag = this.masterProgramName == "OnDoorUsed";
						SourceCode sourceCode = this._sourceCodeDispenser.CreateSourceCodeFromString(text, (!flag) ? this.ministrySrc : this.src);
						this._programRunner.CreateProgram(text, sourceCode.content, text);
						this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					}
					else
					{
						this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					}
					List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Door)));
					list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ConnectionAPI(this, this._tingRunner, this.masterProgram), typeof(ConnectionAPI)));
					this._program.FunctionDefinitions = list;
				}
				return this._program;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600016D RID: 365
		private string src
		{
			get
			{
				return string.Format("\nGoto(\"{0}\")\n", this.targetDoorName);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600016E RID: 366
		private string ministrySrc
		{
			get
			{
				return string.Format("\nGoto(\"{0}\")\n", this.targetDoorName);
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000091F8 File Offset: 0x000073F8
		public void SetSourceCodeFromDoorTarget()
		{
			this.masterProgram.StopAndReset();
			this.masterProgram.sourceCodeContent = this.src;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00009224 File Offset: 0x00007424
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00009244 File Offset: 0x00007444
		public override int securityLevel
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00009248 File Offset: 0x00007448
		public Ting GetLinkTarget()
		{
			if (this.useForRoomPathfinding)
			{
				return this.targetDoor;
			}
			return null;
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00009270 File Offset: 0x00007470
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00009260 File Offset: 0x00007460
		[EditableInEditor]
		public bool useForRoomPathfinding
		{
			get
			{
				return this.CELL_useForRoomPathfinding.data;
			}
			set
			{
				this.CELL_useForRoomPathfinding.data = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00009290 File Offset: 0x00007490
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00009280 File Offset: 0x00007480
		[ShowInEditor]
		public float autoLockTimer
		{
			get
			{
				return this.CELL_autoLockTimer.data;
			}
			set
			{
				this.CELL_autoLockTimer.data = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000178 RID: 376 RVA: 0x000092B0 File Offset: 0x000074B0
		// (set) Token: 0x06000177 RID: 375 RVA: 0x000092A0 File Offset: 0x000074A0
		[EditableInEditor]
		public bool hasDoorKnob
		{
			get
			{
				return this.CELL_hasDoorKnob.data;
			}
			set
			{
				this.CELL_hasDoorKnob.data = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000092C0 File Offset: 0x000074C0
		public bool isBusy
		{
			get
			{
				return base.actionName == "Opening";
			}
		}

		// Token: 0x04000064 RID: 100
		private const int FAILURE_THRESHOLD = 25;

		// Token: 0x04000065 RID: 101
		public new static string TABLE_NAME = "Ting_Doors";

		// Token: 0x04000066 RID: 102
		private ValueEntry<bool> CELL_isLocked;

		// Token: 0x04000067 RID: 103
		private ValueEntry<string> CELL_targetDoorName;

		// Token: 0x04000068 RID: 104
		private ValueEntry<string> CELL_programName;

		// Token: 0x04000069 RID: 105
		private ValueEntry<string[]> CELL_elevatorAlternatives;

		// Token: 0x0400006A RID: 106
		private ValueEntry<int> CELL_elevatorFloor;

		// Token: 0x0400006B RID: 107
		private ValueEntry<bool> CELL_isElevatorEntrance;

		// Token: 0x0400006C RID: 108
		private ValueEntry<int> CELL_code;

		// Token: 0x0400006D RID: 109
		private ValueEntry<float> CELL_autoLockTimer;

		// Token: 0x0400006E RID: 110
		private ValueEntry<bool> CELL_isMoveable;

		// Token: 0x0400006F RID: 111
		private ValueEntry<bool> CELL_hasDoorKnob;

		// Token: 0x04000070 RID: 112
		private ValueEntry<bool> CELL_useForRoomPathfinding;

		// Token: 0x04000071 RID: 113
		private Program _program;

		// Token: 0x04000072 RID: 114
		private Character _user;

		// Token: 0x04000073 RID: 115
		private int _attempts = 0;
	}
}
