using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000077 RID: 119
	public class FryingPan : MimanTing
	{
		// Token: 0x060006C4 RID: 1732 RVA: 0x0001F63C File Offset: 0x0001D83C
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "BlankSlate");
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0001F65C File Offset: 0x0001D85C
		public override IntPoint[] interactionPoints
		{
			get
			{
				return base.interactionPoints;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x0001F664 File Offset: 0x0001D864
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060006C7 RID: 1735
		public override string tooltipName
		{
			get
			{
				return "frying pan煎锅";
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001F670 File Offset: 0x0001D870
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060006C9 RID: 1737
		public override string verbDescription
		{
			get
			{
				return "use使用";
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x0001F688 File Offset: 0x0001D888
		// (set) Token: 0x060006CB RID: 1739 RVA: 0x0001F698 File Offset: 0x0001D898
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

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x0001F6A8 File Offset: 0x0001D8A8
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

		// Token: 0x060006CD RID: 1741 RVA: 0x0001F700 File Offset: 0x0001D900
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001F720 File Offset: 0x0001D920
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Locker || pTingToInteractWith is TrashCan || pTingToInteractWith is SendPipe || pTingToInteractWith is Stove;
		}

		// Token: 0x040001CD RID: 461
		public new static string TABLE_NAME = "Ting_FryingPan";

		// Token: 0x040001CE RID: 462
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001CF RID: 463
		private Program _program;
	}
}
