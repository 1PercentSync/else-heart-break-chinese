using System;
using System.Collections.Generic;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000064 RID: 100
	public class Taser : MimanTing, TingWithButton
	{
		// Token: 0x060005F2 RID: 1522 RVA: 0x0001C2A4 File Offset: 0x0001A4A4
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "Taser");
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001C2C4 File Offset: 0x0001A4C4
		public void PushButton(Ting pUser)
		{
			base.dialogueLine = "";
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001C2D4 File Offset: 0x0001A4D4
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001C2E4 File Offset: 0x0001A4E4
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Character || pTingToInteractWith is Locker;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0001C300 File Offset: 0x0001A500
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0001C304 File Offset: 0x0001A504
		public override string verbDescription
		{
			get
			{
				return "use";
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0001C30C File Offset: 0x0001A50C
		public override string tooltipName
		{
			get
			{
				return "taser";
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x0001C314 File Offset: 0x0001A514
		// (set) Token: 0x060005FA RID: 1530 RVA: 0x0001C324 File Offset: 0x0001A524
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

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x0001C334 File Offset: 0x0001A534
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Extractor)));
				}
				return this._program;
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001C38C File Offset: 0x0001A58C
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x0400018F RID: 399
		public new static string TABLE_NAME = "Ting_Tasers";

		// Token: 0x04000190 RID: 400
		private ValueEntry<string> CELL_programName;

		// Token: 0x04000191 RID: 401
		private Program _program;
	}
}
