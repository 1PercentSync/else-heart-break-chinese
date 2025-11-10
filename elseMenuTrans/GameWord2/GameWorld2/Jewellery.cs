using System;
using System.Collections.Generic;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000075 RID: 117
	public class Jewellery : MimanTing
	{
		// Token: 0x060006A2 RID: 1698 RVA: 0x0001F1D8 File Offset: 0x0001D3D8
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "BlankSlate");
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0001F1F8 File Offset: 0x0001D3F8
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060006A4 RID: 1700
		public override string tooltipName
		{
			get
			{
				return "jewellery珠宝";
			}
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001F204 File Offset: 0x0001D404
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060006A6 RID: 1702
		public override string verbDescription
		{
			get
			{
				return "admire欣赏";
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0001F21C File Offset: 0x0001D41C
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x0001F22C File Offset: 0x0001D42C
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

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0001F23C File Offset: 0x0001D43C
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

		// Token: 0x060006AA RID: 1706 RVA: 0x0001F294 File Offset: 0x0001D494
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001F2B4 File Offset: 0x0001D4B4
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Locker || pTingToInteractWith is TrashCan || pTingToInteractWith is SendPipe;
		}

		// Token: 0x040001C5 RID: 453
		public new static string TABLE_NAME = "Ting_Jewellery";

		// Token: 0x040001C6 RID: 454
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001C7 RID: 455
		private Program _program;
	}
}
