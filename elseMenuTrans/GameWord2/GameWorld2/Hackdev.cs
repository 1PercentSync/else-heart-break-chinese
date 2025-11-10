using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200005B RID: 91
	public class Hackdev : MimanTing
	{
		// Token: 0x0600057E RID: 1406
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "BasicHackdev骇客设备");
			this.CELL_level = base.EnsureCell<int>("level", 0);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001B130 File Offset: 0x00019330
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0001B140 File Offset: 0x00019340
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000581 RID: 1409
		public override string verbDescription
		{
			get
			{
				return "use使用";
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0001B14C File Offset: 0x0001934C
		public override string tooltipName
		{
			get
			{
				return base.name;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0001B154 File Offset: 0x00019354
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x0001B164 File Offset: 0x00019364
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

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0001B174 File Offset: 0x00019374
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Hackdev)));
					list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ConnectionAPI(this, this._tingRunner, this.masterProgram), typeof(ConnectionAPI)));
					this._program.FunctionDefinitions = list;
				}
				return this._program;
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001B1F4 File Offset: 0x000193F4
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is SendPipe;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001B200 File Offset: 0x00019400
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0001B220 File Offset: 0x00019420
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x0001B230 File Offset: 0x00019430
		[EditableInEditor]
		public int level
		{
			get
			{
				return this.CELL_level.data;
			}
			set
			{
				this.CELL_level.data = value;
			}
		}

		// Token: 0x0600058A RID: 1418
		[SprakAPI(new string[] { "Log日志" })]
		public void API_Log(string text)
		{
			D.Log("LOG日志: " + text);
		}

		// Token: 0x04000172 RID: 370
		public new static string TABLE_NAME = "Tings_Hackdevs";

		// Token: 0x04000173 RID: 371
		private ValueEntry<string> CELL_programName;

		// Token: 0x04000174 RID: 372
		private ValueEntry<int> CELL_level;

		// Token: 0x04000175 RID: 373
		private Program _program;
	}
}
