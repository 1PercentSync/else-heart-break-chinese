using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000071 RID: 113
	public class Sink : MimanTing
	{
		// Token: 0x0600066F RID: 1647 RVA: 0x0001EB68 File Offset: 0x0001CD68
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "SimpleSink");
			this.CELL_on = base.EnsureCell<bool>("on", false);
			this.CELL_drinkName = base.EnsureCell<string>("drinkName", "");
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001EBBC File Offset: 0x0001CDBC
		public override void FixBeforeSaving()
		{
			base.FixBeforeSaving();
			if (this.masterProgramName == "" || this.masterProgramName == "BlankSlate")
			{
				this.masterProgramName = "SimpleSink";
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001EC04 File Offset: 0x0001CE04
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001EC0C File Offset: 0x0001CE0C
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001EC10 File Offset: 0x0001CE10
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x0001EC20 File Offset: 0x0001CE20
		public override string verbDescription
		{
			get
			{
				return (!this.on) ? "turn on water in" : "turn off water in";
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x0001EC3C File Offset: 0x0001CE3C
		public override string tooltipName
		{
			get
			{
				return "sink";
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0001EC44 File Offset: 0x0001CE44
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[] { base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 2 };
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x0001EC80 File Offset: 0x0001CE80
		// (set) Token: 0x06000678 RID: 1656 RVA: 0x0001EC90 File Offset: 0x0001CE90
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

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0001ECA0 File Offset: 0x0001CEA0
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x0001ECB0 File Offset: 0x0001CEB0
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

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0001ECC0 File Offset: 0x0001CEC0
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Sink)));
				}
				return this._program;
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001ED18 File Offset: 0x0001CF18
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001ED38 File Offset: 0x0001CF38
		public void Toggle()
		{
			this.on = !this.on;
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001ED4C File Offset: 0x0001CF4C
		// (set) Token: 0x0600067F RID: 1663 RVA: 0x0001ED8C File Offset: 0x0001CF8C
		public Drink currentDrink
		{
			get
			{
				if (this.CELL_drinkName.data == "")
				{
					return null;
				}
				return this._tingRunner.GetTing<Drink>(this.CELL_drinkName.data);
			}
			set
			{
				if (value == null)
				{
					this.CELL_drinkName.data = "";
				}
				else
				{
					this.CELL_drinkName.data = value.name;
				}
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001EDC8 File Offset: 0x0001CFC8
		public void UseDrinkOnSink(Drink pDrink)
		{
			this.currentDrink = pDrink;
			this.masterProgram.Start();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001EDDC File Offset: 0x0001CFDC
		[SprakAPI(new string[] { "Set the liquid level of the drink", "The value" })]
		public void API_SetLiquidAmount(float amount)
		{
			if (this.currentDrink != null)
			{
				this.currentDrink.amount = amount;
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001EDF8 File Offset: 0x0001CFF8
		[SprakAPI(new string[] { "Returns the name of the sink" })]
		public string API_GetName()
		{
			return base.name;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001EE00 File Offset: 0x0001D000
		[SprakAPI(new string[] { "Remove all code in the drink" })]
		public void API_ClearCode()
		{
			if (this.currentDrink != null)
			{
				this.currentDrink.masterProgram.sourceCodeContent = "";
				this.currentDrink.masterProgram.Compile();
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001EE40 File Offset: 0x0001D040
		[SprakAPI(new string[] { "Add code to the end of the drink program", "The extra code to add" })]
		public void API_AppendCode(string code)
		{
			if (this.currentDrink != null)
			{
				Program masterProgram = this.currentDrink.masterProgram;
				masterProgram.sourceCodeContent = masterProgram.sourceCodeContent + "\n" + code + "\n";
				this.currentDrink.masterProgram.Compile();
			}
		}

		// Token: 0x040001BE RID: 446
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001BF RID: 447
		private ValueEntry<bool> CELL_on;

		// Token: 0x040001C0 RID: 448
		private ValueEntry<string> CELL_drinkName;

		// Token: 0x040001C1 RID: 449
		private Program _program;
	}
}
