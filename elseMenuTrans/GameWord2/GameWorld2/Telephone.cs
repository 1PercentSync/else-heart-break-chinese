using System;
using System.Collections.Generic;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000085 RID: 133
	public class Telephone : MimanTing
	{
		// Token: 0x06000774 RID: 1908 RVA: 0x00020F34 File Offset: 0x0001F134
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "BlankSlate");
			this.CELL_ringing = base.EnsureCell<bool>("ringing", false);
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00020F70 File Offset: 0x0001F170
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00020F80 File Offset: 0x0001F180
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Character || pTingToInteractWith is Locker;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x00020F9C File Offset: 0x0001F19C
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x00020FA0 File Offset: 0x0001F1A0
		public override string verbDescription
		{
			get
			{
				return "answer";
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x00020FA8 File Offset: 0x0001F1A8
		public override string tooltipName
		{
			get
			{
				return "telephone";
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00020FB0 File Offset: 0x0001F1B0
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x00020FC0 File Offset: 0x0001F1C0
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

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x00020FD0 File Offset: 0x0001F1D0
		// (set) Token: 0x0600077D RID: 1917 RVA: 0x00020FE0 File Offset: 0x0001F1E0
		[EditableInEditor]
		public bool ringing
		{
			get
			{
				return this.CELL_ringing.data;
			}
			set
			{
				this.CELL_ringing.data = value;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x00020FF0 File Offset: 0x0001F1F0
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Telephone)));
				}
				return this._program;
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00021048 File Offset: 0x0001F248
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00021068 File Offset: 0x0001F268
		public void Use()
		{
			this.ringing = false;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00021074 File Offset: 0x0001F274
		public override void Update(float dt)
		{
			base.UpdateBubbleTimer();
		}

		// Token: 0x040001F6 RID: 502
		public new static string TABLE_NAME = "Ting_Telephones";

		// Token: 0x040001F7 RID: 503
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001F8 RID: 504
		private ValueEntry<bool> CELL_ringing;

		// Token: 0x040001F9 RID: 505
		private Program _program;
	}
}
