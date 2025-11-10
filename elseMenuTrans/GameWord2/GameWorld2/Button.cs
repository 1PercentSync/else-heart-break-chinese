using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000061 RID: 97
	public class Button : MimanTing
	{
		// Token: 0x060005D4 RID: 1492 RVA: 0x0001BAA8 File Offset: 0x00019CA8
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "Button");
			this.CELL_user = base.EnsureCell<string>("user", "");
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001BAE8 File Offset: 0x00019CE8
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001BAF0 File Offset: 0x00019CF0
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001BB38 File Offset: 0x00019D38
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0001BB00 File Offset: 0x00019D00
		public Character user
		{
			get
			{
				return this._tingRunner.GetTingUnsafe(this.CELL_user.data) as Character;
			}
			set
			{
				if (value != null)
				{
					this.CELL_user.data = value.name;
				}
				else
				{
					this.CELL_user.data = null;
				}
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001BB58 File Offset: 0x00019D58
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001BB5C File Offset: 0x00019D5C
		public override string tooltipName
		{
			get
			{
				return "button";
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x0001BB64 File Offset: 0x00019D64
		public override string verbDescription
		{
			get
			{
				return "push";
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001BB6C File Offset: 0x00019D6C
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[] { base.localPoint - IntPoint.DirectionToIntPoint(base.direction) * 1 };
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001BBA8 File Offset: 0x00019DA8
		public void Push(Character pUser)
		{
			this.user = pUser;
			this.masterProgram.Start();
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001BBBC File Offset: 0x00019DBC
		[SprakAPI(new string[] { "Print", "text" })]
		public void API_Print(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001BBCC File Offset: 0x00019DCC
		[SprakAPI(new string[] { "Play a sound", "name" })]
		public void API_PlaySound(string name)
		{
			base.PlaySound(name);
			base.audioLoop = false;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001BBDC File Offset: 0x00019DDC
		[SprakAPI(new string[] { "Get the name of the person using the button" })]
		public string API_GetUser()
		{
			if (this.user != null)
			{
				return this.user.name;
			}
			return "";
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001BBFC File Offset: 0x00019DFC
		[SprakAPI(new string[] { "Pause the master program", "number of seconds to pause for" })]
		public void API_Sleep(float seconds)
		{
			this.masterProgram.sleepTimer = seconds;
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0001BC0C File Offset: 0x00019E0C
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x0001BC1C File Offset: 0x00019E1C
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

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0001BC2C File Offset: 0x00019E2C
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Button)));
					list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ConnectionAPI(this, this._tingRunner, this.masterProgram), typeof(ConnectionAPI)));
					this._program.FunctionDefinitions = list;
				}
				return this._program;
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001BCAC File Offset: 0x00019EAC
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x04000185 RID: 389
		public new static string TABLE_NAME = "Ting_Buttons";

		// Token: 0x04000186 RID: 390
		private ValueEntry<string> CELL_programName;

		// Token: 0x04000187 RID: 391
		private ValueEntry<string> CELL_user;

		// Token: 0x04000188 RID: 392
		private Program _program;
	}
}
