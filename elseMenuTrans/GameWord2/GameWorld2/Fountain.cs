using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000073 RID: 115
	public class Fountain : MimanTing
	{
		// Token: 0x06000687 RID: 1671 RVA: 0x0001EEA0 File Offset: 0x0001D0A0
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "BlankSlate");
			this.CELL_on = base.EnsureCell<bool>("on", true);
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x0001EEDC File Offset: 0x0001D0DC
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[]
				{
					base.localPoint + IntPoint.Up * 3,
					base.localPoint + IntPoint.Right * 3,
					base.localPoint + IntPoint.Left * 3,
					base.localPoint + IntPoint.Down * 3
				};
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001EF78 File Offset: 0x0001D178
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001EF80 File Offset: 0x0001D180
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001EF90 File Offset: 0x0001D190
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600068C RID: 1676
		public override string verbDescription
		{
			get
			{
				return "admire欣赏";
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600068D RID: 1677
		public override string tooltipName
		{
			get
			{
				return "fountain喷泉";
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x0001EFA4 File Offset: 0x0001D1A4
		// (set) Token: 0x0600068F RID: 1679 RVA: 0x0001EFB4 File Offset: 0x0001D1B4
		[EditableInEditor]
		public bool on
		{
			get
			{
				return this.CELL_on.data;
			}
			set
			{
				this.CELL_on.data = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x0001EFC4 File Offset: 0x0001D1C4
		// (set) Token: 0x06000691 RID: 1681 RVA: 0x0001EFD4 File Offset: 0x0001D1D4
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

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x0001EFE4 File Offset: 0x0001D1E4
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Lamp)));
				}
				return this._program;
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001F03C File Offset: 0x0001D23C
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x040001C2 RID: 450
		private ValueEntry<bool> CELL_on;

		// Token: 0x040001C3 RID: 451
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001C4 RID: 452
		private Program _program;
	}
}
