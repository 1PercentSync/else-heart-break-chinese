using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000081 RID: 129
	public class Fence : MimanTing
	{
		// Token: 0x0600074C RID: 1868 RVA: 0x00020964 File Offset: 0x0001EB64
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "DefaultFence");
			this.CELL_userName = base.EnsureCell<string>("userName", "");
			this.CELL_goalPointIndex = base.EnsureCell<int>("goalPointIndex", 0);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x000209B8 File Offset: 0x0001EBB8
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x000209C0 File Offset: 0x0001EBC0
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x000209D0 File Offset: 0x0001EBD0
		public override void Update(float dt)
		{
			base.UpdateBubbleTimer();
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x000209D8 File Offset: 0x0001EBD8
		public override void Init()
		{
			base.Init();
			PointTileNode tile = base.room.GetTile(this.interactionPoints[0]);
			PointTileNode tile2 = base.room.GetTile(this.interactionPoints[1]);
			tile.teleportTarget = base.tile;
			tile2.teleportTarget = base.tile;
			base.tile.RemoveAllLinks();
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x00020A48 File Offset: 0x0001EC48
		public override IntPoint[] interactionPointsTryTheseFirst
		{
			get
			{
				return new IntPoint[]
				{
					base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 2,
					base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * -2
				};
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x00020AAC File Offset: 0x0001ECAC
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[]
				{
					base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 2,
					base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * -2
				};
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x00020B10 File Offset: 0x0001ED10
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000754 RID: 1876
		public override string verbDescription
		{
			get
			{
				return "try to walk through尝试穿过";
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000755 RID: 1877
		public override string tooltipName
		{
			get
			{
				return "fence栅栏";
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x00020B24 File Offset: 0x0001ED24
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x00020B34 File Offset: 0x0001ED34
		[EditableInEditor]
		public int goalPointIndex
		{
			get
			{
				return this.CELL_goalPointIndex.data;
			}
			set
			{
				this.CELL_goalPointIndex.data = value;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x00020B44 File Offset: 0x0001ED44
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x00020B54 File Offset: 0x0001ED54
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

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x00020B64 File Offset: 0x0001ED64
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x00020B74 File Offset: 0x0001ED74
		[EditableInEditor]
		public string userName
		{
			get
			{
				return this.CELL_userName.data;
			}
			set
			{
				this.CELL_userName.data = value;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x00020B84 File Offset: 0x0001ED84
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x00020BC0 File Offset: 0x0001EDC0
		public Character user
		{
			get
			{
				if (this.userName == "")
				{
					return null;
				}
				return this._tingRunner.GetTingUnsafe(this.userName) as Character;
			}
			set
			{
				if (value == null)
				{
					this.userName = "";
				}
				else
				{
					this.userName = value.name;
				}
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x00020BF0 File Offset: 0x0001EDF0
		public WorldCoordinate goalPosition
		{
			get
			{
				return new WorldCoordinate(base.room.name, this.interactionPoints[this.goalPointIndex]);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x00020C24 File Offset: 0x0001EE24
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Fence)));
					list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ConnectionAPI(this, this._tingRunner, this.masterProgram), typeof(ConnectionAPI)));
					this._program.FunctionDefinitions = list;
				}
				return this._program;
			}
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00020CA4 File Offset: 0x0001EEA4
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00020CC4 File Offset: 0x0001EEC4
		public void StartedWalkingThrough(Character pCharacter)
		{
			if (pCharacter == null)
			{
				D.Log("pCharacter argument to StartedWalkingThrough() for fence " + base.name + " is null!");
				return;
			}
			this.user = pCharacter;
			if (this.user.localPoint == this.interactionPoints[0])
			{
				this.goalPointIndex = 1;
			}
			else
			{
				this.goalPointIndex = 0;
			}
			this.masterProgram.Start();
			if (this.masterProgram.ContainsErrors())
			{
				this.API_Grill();
			}
		}

		// Token: 0x06000762 RID: 1890
		[SprakAPI(new string[] { "Stops the person to walk through the fence阻止角色穿过栅栏" })]
		public void API_Grill()
		{
			D.Log("Grill!");
			if (this.user != null)
			{
				this._dialogueRunner.EventHappened(base.name + "_grill_" + this.user.name);
				this.user.StopAction();
			}
		}

		// Token: 0x06000763 RID: 1891
		[SprakAPI(new string[] { "Get the name of the person walking through fence获取正在穿过栅栏的人的名称" })]
		public string API_GetUser()
		{
			if (this.user == null)
			{
				return "";
			}
			return this.user.name;
		}

		// Token: 0x06000764 RID: 1892
		[SprakAPI(new string[] { "Is the user carrying a modifier?使用者是否携带了修改器？" })]
		public bool API_UserHasModifier()
		{
			return this.user != null && this.user.hasHackdev;
		}

		// Token: 0x06000765 RID: 1893
		[SprakAPI(new string[] { "Say something说点什么" })]
		public void API_Say(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00020DF4 File Offset: 0x0001EFF4
		public override int securityLevel
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x040001F2 RID: 498
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001F3 RID: 499
		private ValueEntry<string> CELL_userName;

		// Token: 0x040001F4 RID: 500
		private ValueEntry<int> CELL_goalPointIndex;

		// Token: 0x040001F5 RID: 501
		private Program _program;
	}
}
