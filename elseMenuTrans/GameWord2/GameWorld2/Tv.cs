using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000070 RID: 112
	public class Tv : MimanTing
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x0001E864 File Offset: 0x0001CA64
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "TvProgram");
			this.CELL_channelName = base.EnsureCell<string>("channelName", "Bergman");
			this.CELL_on = base.EnsureCell<bool>("on", true);
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001E8B8 File Offset: 0x0001CAB8
		public override void FixBeforeSaving()
		{
			base.FixBeforeSaving();
			if (this.masterProgramName == "BlankSlate")
			{
				this.masterProgramName = "TvProgram";
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001E8EC File Offset: 0x0001CAEC
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0001E8F4 File Offset: 0x0001CAF4
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001E8F8 File Offset: 0x0001CAF8
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001E908 File Offset: 0x0001CB08
		public override string verbDescription
		{
			get
			{
				return "turn " + ((!this.on) ? "on" : "off");
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0001E93C File Offset: 0x0001CB3C
		public override string tooltipName
		{
			get
			{
				return "tv";
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001E944 File Offset: 0x0001CB44
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[]
				{
					base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 1,
					base.localPoint + IntPoint.Up * 2,
					base.localPoint + IntPoint.Left * 2,
					base.localPoint + IntPoint.Right * 2,
					base.localPoint + IntPoint.Down * 2
				};
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001EA08 File Offset: 0x0001CC08
		public void Flip()
		{
			this.on = !this.on;
			if (this.on)
			{
				this.masterProgram.Start();
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001EA3C File Offset: 0x0001CC3C
		public override void Update(float dt)
		{
			base.UpdateBubbleTimer();
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001EA44 File Offset: 0x0001CC44
		[SprakAPI(new string[] { "Print", "text" })]
		public void API_Print(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001EA54 File Offset: 0x0001CC54
		[SprakAPI(new string[] { "Get the name of the current channel" })]
		public string API_GetChannel()
		{
			return this.channelName;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001EA5C File Offset: 0x0001CC5C
		[SprakAPI(new string[] { "Set the channel" })]
		public void API_SetChannel(string newChannelName)
		{
			this.channelName = newChannelName;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001EA68 File Offset: 0x0001CC68
		[SprakAPI(new string[] { "Get the current hour" })]
		public int API_GetHour()
		{
			return this._worldSettings.gameTimeClock.hours;
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0001EA88 File Offset: 0x0001CC88
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x0001EA98 File Offset: 0x0001CC98
		[EditableInEditor]
		public string channelName
		{
			get
			{
				return this.CELL_channelName.data;
			}
			set
			{
				this.CELL_channelName.data = value;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x0001EAA8 File Offset: 0x0001CCA8
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x0001EAB8 File Offset: 0x0001CCB8
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

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x0001EAC8 File Offset: 0x0001CCC8
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Tv)));
				}
				return this._program;
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0001EB20 File Offset: 0x0001CD20
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x0001EB40 File Offset: 0x0001CD40
		// (set) Token: 0x0600066D RID: 1645 RVA: 0x0001EB50 File Offset: 0x0001CD50
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

		// Token: 0x040001BA RID: 442
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001BB RID: 443
		private ValueEntry<string> CELL_channelName;

		// Token: 0x040001BC RID: 444
		private ValueEntry<bool> CELL_on;

		// Token: 0x040001BD RID: 445
		private Program _program;
	}
}
