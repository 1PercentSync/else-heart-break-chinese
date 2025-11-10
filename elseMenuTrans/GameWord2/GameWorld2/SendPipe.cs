using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000080 RID: 128
	public class SendPipe : MimanTing
	{
		// Token: 0x06000739 RID: 1849 RVA: 0x0002067C File Offset: 0x0001E87C
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "DefaultPipe");
			this.CELL_stuffName = base.EnsureCell<string>("stuffName", "");
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x000206BC File Offset: 0x0001E8BC
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[] { base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 2 };
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000206F8 File Offset: 0x0001E8F8
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00020700 File Offset: 0x0001E900
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x00020710 File Offset: 0x0001E910
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600073E RID: 1854
		public override string verbDescription
		{
			get
			{
				return "inspect检查";
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0002071C File Offset: 0x0001E91C
		public override string tooltipName
		{
			get
			{
				return "send pipe";
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x00020724 File Offset: 0x0001E924
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x00020734 File Offset: 0x0001E934
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

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x00020744 File Offset: 0x0001E944
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(SendPipe)));
					list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ConnectionAPI(this, this._tingRunner, this.masterProgram), typeof(ConnectionAPI)));
					this._program.FunctionDefinitions = list;
				}
				return this._program;
			}
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x000207C4 File Offset: 0x0001E9C4
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000207E4 File Offset: 0x0001E9E4
		public void PutStuffIntoIt(Ting pStuff)
		{
			pStuff.isBeingHeld = false;
			pStuff.position = base.position;
			this.stuff = pStuff;
			this.masterProgram.Start();
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00020818 File Offset: 0x0001EA18
		// (set) Token: 0x06000746 RID: 1862 RVA: 0x00020828 File Offset: 0x0001EA28
		public string stuffName
		{
			get
			{
				return this.CELL_stuffName.data;
			}
			set
			{
				this.CELL_stuffName.data = value;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00020838 File Offset: 0x0001EA38
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x00020850 File Offset: 0x0001EA50
		public Ting stuff
		{
			get
			{
				return this._tingRunner.GetTingUnsafe(this.CELL_stuffName.data);
			}
			set
			{
				if (value == null)
				{
					this.CELL_stuffName.data = "";
				}
				else
				{
					this.CELL_stuffName.data = value.name;
				}
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0002088C File Offset: 0x0001EA8C
		[SprakAPI(new string[] { "Send something to another (connected) thing" })]
		public void API_Send(float connection)
		{
			if (this.stuff == null)
			{
				throw new Error("Nothing to send");
			}
			if (connection < 0f)
			{
				throw new Error("Connection id too low: " + connection);
			}
			if (connection >= (float)base.connectedTings.Length)
			{
				throw new Error("Connection id too high: " + connection);
			}
			MimanTing mimanTing = base.connectedTings[(int)connection];
			if (mimanTing == null)
			{
				throw new Error("Invalid target");
			}
			if (mimanTing is SendPipe)
			{
				SendPipe sendPipe = mimanTing as SendPipe;
				sendPipe.stuff = this.stuff;
			}
			WorldCoordinate position = mimanTing.position;
			this.stuff.position = position;
			this.stuff = null;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00020948 File Offset: 0x0001EB48
		[SprakAPI(new string[] { "Get a random number between 0.0 and 1.0" })]
		public float API_Random()
		{
			return Randomizer.GetValue(0f, 1f);
		}

		// Token: 0x040001EF RID: 495
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001F0 RID: 496
		private ValueEntry<string> CELL_stuffName;

		// Token: 0x040001F1 RID: 497
		private Program _program;
	}
}
