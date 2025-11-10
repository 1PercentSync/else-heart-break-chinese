using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000010 RID: 16
	public class Floppy : MimanTing
	{
		// Token: 0x06000189 RID: 393
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "BlankSlate空白软盘");
		}

		// Token: 0x0600018A RID: 394
		public override void FixBeforeSaving()
		{
			base.FixBeforeSaving();
			if (this.masterProgramName == "BlankSlate")
			{
				if (base.room.name.Contains("Ministry"))
				{
					this.masterProgramName = "MinistryData部委数据" + Randomizer.GetIntValue(0, 10).ToString();
					return;
				}
				this.masterProgramName = "DigitalTrash数字垃圾" + Randomizer.GetIntValue(0, 30).ToString();
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000094E8 File Offset: 0x000076E8
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Locker || pTingToInteractWith is TrashCan || pTingToInteractWith is SendPipe || pTingToInteractWith is Stove;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00009518 File Offset: 0x00007718
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00009528 File Offset: 0x00007728
		public override string tooltipName
		{
			get
			{
				return this.masterProgramName;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600018E RID: 398
		public override string verbDescription
		{
			get
			{
				return "inspect读取";
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00009538 File Offset: 0x00007738
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000190 RID: 400 RVA: 0x0000953C File Offset: 0x0000773C
		// (set) Token: 0x06000191 RID: 401 RVA: 0x0000954C File Offset: 0x0000774C
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

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0000955C File Offset: 0x0000775C
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Floppy)));
					this._program.compilationTurnedOn = false;
				}
				return this._program;
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000095C0 File Offset: 0x000077C0
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x04000075 RID: 117
		private ValueEntry<string> CELL_programName;

		// Token: 0x04000076 RID: 118
		private Program _program;
	}
}
