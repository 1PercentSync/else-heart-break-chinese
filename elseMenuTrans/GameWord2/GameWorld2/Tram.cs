using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200007A RID: 122
	public class Tram : Vehicle
	{
		// Token: 0x060006E2 RID: 1762 RVA: 0x0001FAC8 File Offset: 0x0001DCC8
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "DefaultTram");
			this.CELL_doorForwardOffset = base.EnsureCell<int>("doorForwardOffset", 4);
			this.CELL_doorSideOffset = base.EnsureCell<int>("doorSideOffset", 2);
			this.CELL_doorRotationOffset = base.EnsureCell<int>("doorRotationOffset", 90);
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0001FB28 File Offset: 0x0001DD28
		[ShowInEditor]
		public override IntPoint movingDoorPositionOffset
		{
			get
			{
				return base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * this.doorForwardOffset + IntPoint.DirectionToIntPoint(IntPoint.Turn(base.direction, 90)) * this.doorSideOffset;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001FB7C File Offset: 0x0001DD7C
		public override int movingDoorRotationOffset
		{
			get
			{
				return this.doorRotationOffset;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0001FB84 File Offset: 0x0001DD84
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001FB88 File Offset: 0x0001DD88
		public override string verbDescription
		{
			get
			{
				return "enter";
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0001FB90 File Offset: 0x0001DD90
		public override string tooltipName
		{
			get
			{
				return "tram";
			}
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001FB98 File Offset: 0x0001DD98
		[SprakAPI(new string[] { "Set the speed of the tram" })]
		public void API_SetSpeed(float speed)
		{
			this.logger.Log(string.Concat(new object[] { "Speed of ", base.name, " was set to ", speed }));
			base.speed = speed;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001FBE4 File Offset: 0x0001DDE4
		[SprakAPI(new string[] { "Get the next node in the tram track system" })]
		public string API_GetNextNavNode()
		{
			return base.nextNavNodeName;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0001FBEC File Offset: 0x0001DDEC
		[SprakAPI(new string[] { "Is the tram at a station?" })]
		public bool API_IsAtStation()
		{
			if (base.currentNavNode == null)
			{
				D.Log("Warning, current nav node for " + base.name + " is null!");
				return false;
			}
			return base.currentNavNode.isStation;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001FC2C File Offset: 0x0001DE2C
		[SprakAPI(new string[] { "Turn left" })]
		public void API_TurnLeft()
		{
			base.turning = VehicleTurningDirection.LEFT;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001FC38 File Offset: 0x0001DE38
		[SprakAPI(new string[] { "Turn right" })]
		public void API_TurnRight()
		{
			base.turning = VehicleTurningDirection.RIGHT;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001FC44 File Offset: 0x0001DE44
		[SprakAPI(new string[] { "Go straight forward" })]
		public void API_DoNotTurn()
		{
			base.turning = VehicleTurningDirection.FORWARD;
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0001FC50 File Offset: 0x0001DE50
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x0001FC60 File Offset: 0x0001DE60
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

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0001FC70 File Offset: 0x0001DE70
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Tram)));
				}
				return this._program;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0001FCC8 File Offset: 0x0001DEC8
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0001FCD8 File Offset: 0x0001DED8
		[EditableInEditor]
		public int doorForwardOffset
		{
			get
			{
				return this.CELL_doorForwardOffset.data;
			}
			set
			{
				this.CELL_doorForwardOffset.data = value;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0001FCE8 File Offset: 0x0001DEE8
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x0001FCF8 File Offset: 0x0001DEF8
		[EditableInEditor]
		public int doorSideOffset
		{
			get
			{
				return this.CELL_doorSideOffset.data;
			}
			set
			{
				this.CELL_doorSideOffset.data = value;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0001FD08 File Offset: 0x0001DF08
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0001FD18 File Offset: 0x0001DF18
		[EditableInEditor]
		public int doorRotationOffset
		{
			get
			{
				return this.CELL_doorRotationOffset.data;
			}
			set
			{
				this.CELL_doorRotationOffset.data = value;
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001FD28 File Offset: 0x0001DF28
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
			else
			{
				this.masterProgram.nameOfOwner = base.name;
			}
		}

		// Token: 0x040001D5 RID: 469
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001D6 RID: 470
		private ValueEntry<int> CELL_doorForwardOffset;

		// Token: 0x040001D7 RID: 471
		private ValueEntry<int> CELL_doorSideOffset;

		// Token: 0x040001D8 RID: 472
		private ValueEntry<int> CELL_doorRotationOffset;

		// Token: 0x040001D9 RID: 473
		private Program _program;
	}
}
