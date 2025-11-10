using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000076 RID: 118
	public class Stove : MimanTing
	{
		// Token: 0x060006AE RID: 1710 RVA: 0x0001F2F8 File Offset: 0x0001D4F8
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "Stove");
			this.CELL_on = base.EnsureCell<bool>("on", false);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0001F334 File Offset: 0x0001D534
		public override void FixBeforeSaving()
		{
			if (this.masterProgramName == "BlankSlate")
			{
				this.masterProgramName = "Stove";
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0001F364 File Offset: 0x0001D564
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001F36C File Offset: 0x0001D56C
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[] { base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 2 };
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0001F3A8 File Offset: 0x0001D5A8
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0001F3B8 File Offset: 0x0001D5B8
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0001F3BC File Offset: 0x0001D5BC
		public override string tooltipName
		{
			get
			{
				return "stove";
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0001F3C4 File Offset: 0x0001D5C4
		public override string verbDescription
		{
			get
			{
				return "turn " + ((!this.on) ? "on" : "off");
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001F3F8 File Offset: 0x0001D5F8
		public void Fry(Character pUser, MimanTing pTing)
		{
			if (this.on)
			{
				this._objectOnStove = pTing;
				this.masterProgram.Start();
			}
			else
			{
				this._objectOnStove = null;
				this._worldSettings.Notify(pUser.name, "Stove is not on");
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001F444 File Offset: 0x0001D644
		[SprakAPI(new string[] { "Remove all source code in the object on the stove" })]
		public void API_ClearCode()
		{
			if (this._objectOnStove != null && this._objectOnStove.masterProgram != null)
			{
				this._objectOnStove.masterProgram.sourceCodeContent = "";
				this._objectOnStove.masterProgram.Compile();
				return;
			}
			throw new Error("No object on stove");
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001F4A4 File Offset: 0x0001D6A4
		[SprakAPI(new string[] { "Get the source code in the object on the stove" })]
		public string API_GetCode()
		{
			if (this._objectOnStove != null && this._objectOnStove.masterProgram != null)
			{
				return this._objectOnStove.masterProgram.sourceCodeContent;
			}
			throw new Error("No object on stove");
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001F4E8 File Offset: 0x0001D6E8
		[SprakAPI(new string[] { "Add code to the end of the objects program", "The extra code to add" })]
		public void API_AppendCode(string code)
		{
			if (this._objectOnStove != null && this._objectOnStove.masterProgram != null)
			{
				Program masterProgram = this._objectOnStove.masterProgram;
				masterProgram.sourceCodeContent = masterProgram.sourceCodeContent + "\n" + code + "\n";
				this._objectOnStove.masterProgram.Compile();
				return;
			}
			throw new Error("No object on stove");
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001F558 File Offset: 0x0001D758
		[SprakAPI(new string[] { "Get a random value between 0 and 1" })]
		public float API_Random()
		{
			return Randomizer.GetValue(0f, 1f);
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0001F56C File Offset: 0x0001D76C
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x0001F57C File Offset: 0x0001D77C
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

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x0001F58C File Offset: 0x0001D78C
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x0001F59C File Offset: 0x0001D79C
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

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x0001F5AC File Offset: 0x0001D7AC
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Stove)));
				}
				return this._program;
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001F604 File Offset: 0x0001D804
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001F624 File Offset: 0x0001D824
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return false;
		}

		// Token: 0x040001C8 RID: 456
		public new static string TABLE_NAME = "Ting_Stove";

		// Token: 0x040001C9 RID: 457
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001CA RID: 458
		private ValueEntry<bool> CELL_on;

		// Token: 0x040001CB RID: 459
		private Program _program;

		// Token: 0x040001CC RID: 460
		private MimanTing _objectOnStove;
	}
}
