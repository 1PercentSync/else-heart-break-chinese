using System;
using System.Collections.Generic;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000078 RID: 120
	public class Screwdriver : MimanTing
	{
		// Token: 0x060006D1 RID: 1745 RVA: 0x0001F764 File Offset: 0x0001D964
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "Screwdriver");
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001F784 File Offset: 0x0001D984
		public override void FixBeforeSaving()
		{
			base.FixBeforeSaving();
			if (this.masterProgramName == "BlankSlate")
			{
				this.masterProgramName = "Screwdriver";
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0001F7B8 File Offset: 0x0001D9B8
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001F7BC File Offset: 0x0001D9BC
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0001F7CC File Offset: 0x0001D9CC
		public override string tooltipName
		{
			get
			{
				return "screwdriver";
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0001F7D4 File Offset: 0x0001D9D4
		public override string verbDescription
		{
			get
			{
				return "use";
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001F7DC File Offset: 0x0001D9DC
		public void UseOnComputer(Computer pComputer)
		{
			this._computerTarget = pComputer;
			this.masterProgram.Start();
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001F7F0 File Offset: 0x0001D9F0
		[SprakAPI(new string[] { "Set the speed of the computer you use the screwdriver on", "Mhz (max 500)" })]
		public void API_SetMhz(float mhz)
		{
			if (this._computerTarget != null && this._computerTarget.masterProgram != null)
			{
				if (mhz < 0f)
				{
					mhz = 0f;
				}
				if (mhz > 500f)
				{
					mhz = 500f;
				}
				this._computerTarget.mhz = (int)mhz;
				return;
			}
			throw new Error("No computer found.");
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001F85C File Offset: 0x0001DA5C
		[SprakAPI(new string[] { "Set the maximum execution time for the computer", "-2 = infinite, -1 = default, 0+ = time in seconds" })]
		public void API_SetMaxTime(float maxTime)
		{
			if (this._computerTarget != null && this._computerTarget.masterProgram != null)
			{
				this._computerTarget.maxExecutionTime = (float)((int)maxTime);
				return;
			}
			throw new Error("No computer found.");
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001F898 File Offset: 0x0001DA98
		[SprakAPI(new string[] { "Enable an API", "The name of the API" })]
		public void API_EnableAPI(string name)
		{
			if (this._computerTarget != null && this._computerTarget.masterProgram != null)
			{
				string text = name.ToLower();
				if (text != null)
				{
					if (Screwdriver.<>f__switch$map3 == null)
					{
						Screwdriver.<>f__switch$map3 = new Dictionary<string, int>(5)
						{
							{ "internet", 0 },
							{ "arcade", 1 },
							{ "floppy", 2 },
							{ "memory", 3 },
							{ "door", 4 }
						};
					}
					int num;
					if (Screwdriver.<>f__switch$map3.TryGetValue(text, out num))
					{
						switch (num)
						{
						case 0:
							this._computerTarget.hasInternetAPI = true;
							break;
						case 1:
							this._computerTarget.hasArcadeMachineAPI = true;
							break;
						case 2:
							this._computerTarget.hasFloppyAPI = true;
							break;
						case 3:
							this._computerTarget.hasMemoryAPI = true;
							break;
						case 4:
							this._computerTarget.hasDoorAPI = true;
							break;
						default:
							goto IL_0101;
						}
						this._computerTarget.RemovePrograms();
						return;
					}
				}
				IL_0101:
				throw new Error("No API with name '" + name + "' found.");
			}
			throw new Error("No computer found.");
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0001F9D8 File Offset: 0x0001DBD8
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x0001F9E8 File Offset: 0x0001DBE8
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

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0001F9F8 File Offset: 0x0001DBF8
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Screwdriver)));
				}
				return this._program;
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001FA50 File Offset: 0x0001DC50
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001FA70 File Offset: 0x0001DC70
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Locker || pTingToInteractWith is TrashCan || pTingToInteractWith is SendPipe || pTingToInteractWith is Stove || pTingToInteractWith is Computer;
		}

		// Token: 0x040001D0 RID: 464
		public new static string TABLE_NAME = "Ting_Screwdriver";

		// Token: 0x040001D1 RID: 465
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001D2 RID: 466
		private Program _program;

		// Token: 0x040001D3 RID: 467
		private Computer _computerTarget;
	}
}
