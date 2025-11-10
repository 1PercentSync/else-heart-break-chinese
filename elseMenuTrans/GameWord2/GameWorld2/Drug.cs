using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200002C RID: 44
	public class Drug : MimanTing
	{
		// Token: 0x060003E2 RID: 994
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "Citnap");
			this.CELL_drugType = base.EnsureCell<string>("drugType", "drug药物");
			this.CELL_charges = base.EnsureCell<int>("charges", 1);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00013DB4 File Offset: 0x00011FB4
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x060003E4 RID: 996
		public override void FixBeforeSaving()
		{
			if (base.name.ToLower().Contains("bun"))
			{
				this.masterProgramName = "Bun";
				this.drugType = "Bun面包";
				return;
			}
			if (base.name.ToLower().Contains("baguette"))
			{
				this.masterProgramName = "Baguette";
				this.drugType = "baguette法棍";
				return;
			}
			if (base.name.ToLower().Contains("cig"))
			{
				this.masterProgramName = "Cigarette";
				this.drugType = "cigarette香烟";
				this.charges = 4;
				return;
			}
			if (base.name.ToLower().Contains("snus"))
			{
				this.masterProgramName = "Snus";
				this.drugType = "snus鼻烟";
				this.charges = Randomizer.GetIntValue(6, 10);
				return;
			}
			if (base.name.ToLower().Contains("slip"))
			{
				this.masterProgramName = "Citnap";
				this.drugType = "drug药物";
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00013EEC File Offset: 0x000120EC
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00013EF0 File Offset: 0x000120F0
		public override string tooltipName
		{
			get
			{
				return this.drugType;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003E7 RID: 999
		public override string verbDescription
		{
			get
			{
				return "eat食用";
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00013F00 File Offset: 0x00012100
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x00013F10 File Offset: 0x00012110
		[EditableInEditor]
		public string drugType
		{
			get
			{
				return this.CELL_drugType.data;
			}
			set
			{
				this.CELL_drugType.data = value;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00013F20 File Offset: 0x00012120
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x00013F30 File Offset: 0x00012130
		[EditableInEditor]
		public int charges
		{
			get
			{
				return this.CELL_charges.data;
			}
			set
			{
				this.CELL_charges.data = value;
			}
		}

		// Token: 0x060003EC RID: 1004
		[SprakAPI(new string[] { "Trippy迷幻" })]
		public void API_Trippy()
		{
			if (this._user == null)
			{
				return;
			}
			this._user.StopAction();
			this._user.StartAction(Drug.trippyAnims[Randomizer.GetIntValue(0, Drug.trippyAnims.Length)], null, 9999f, 3f);
		}

		// Token: 0x060003ED RID: 1005
		[SprakAPI(new string[] { "Turn left左转" })]
		public void API_TurnLeft()
		{
			if (this._user == null)
			{
				return;
			}
			this._user.TurnLeft();
		}

		// Token: 0x060003EE RID: 1006
		[SprakAPI(new string[] { "Turn right右转" })]
		public void API_TurnRight()
		{
			if (this._user == null)
			{
				return;
			}
			this._user.TurnRight();
		}

		// Token: 0x060003EF RID: 1007
		[SprakAPI(new string[] { "Make time appear to go faster让时间流逝得更快" })]
		public void API_FastForward()
		{
			if (this._user == null)
			{
				return;
			}
			this._dialogueRunner.EventHappened(this._user.name + "_TookFastForwardDrug");
		}

		// Token: 0x060003F0 RID: 1008
		[SprakAPI(new string[] { "Move forward one step向前移动一步" })]
		public void API_Move()
		{
			WorldCoordinate worldCoordinate = new WorldCoordinate(base.room.name, base.localPoint + IntPoint.DirectionToIntPoint(base.direction));
			base.position = worldCoordinate;
		}

		// Token: 0x060003F1 RID: 1009
		[SprakAPI(new string[] { "Pause the master program暂停主程序", "number of seconds to pause for暂停的秒数" })]
		public void API_Sleep(float seconds)
		{
			this.masterProgram.sleepTimer = seconds;
		}

		// Token: 0x060003F2 RID: 1010
		[SprakAPI(new string[] { "Get a quick energy boost快速获取能量" })]
		public void API_QuickBoost()
		{
			this._user.sleepiness -= 10f;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001406C File Offset: 0x0001226C
		public void Take(Character pUser)
		{
			this._user = pUser;
			this.charges--;
			this.masterProgram.maxExecutionTime = 7f;
			this.masterProgram.Start();
			if (this.onDrugUse != null)
			{
				this.onDrugUse();
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x000140C0 File Offset: 0x000122C0
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x000140D0 File Offset: 0x000122D0
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

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x000140E0 File Offset: 0x000122E0
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Drug)));
				}
				return this._program;
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00014138 File Offset: 0x00012338
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
			this._user = null;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00014168 File Offset: 0x00012368
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Locker || pTingToInteractWith is TrashCan || pTingToInteractWith is SendPipe || pTingToInteractWith is Stove;
		}

		// Token: 0x040000EF RID: 239
		public new static string TABLE_NAME = "Ting_Drugs";

		// Token: 0x040000F0 RID: 240
		private ValueEntry<string> CELL_programName;

		// Token: 0x040000F1 RID: 241
		private ValueEntry<string> CELL_drugType;

		// Token: 0x040000F2 RID: 242
		private ValueEntry<int> CELL_charges;

		// Token: 0x040000F3 RID: 243
		private Program _program;

		// Token: 0x040000F4 RID: 244
		private Character _user;

		// Token: 0x040000F5 RID: 245
		public Drug.OnDrugUse onDrugUse;

		// Token: 0x040000F6 RID: 246
		private static string[] trippyAnims = new string[] { "Shrug", "Walk", "TalkingInTelephone" };

		// Token: 0x0200002D RID: 45
		// (Invoke) Token: 0x060003FA RID: 1018
		public delegate void OnDrugUse();
	}
}
