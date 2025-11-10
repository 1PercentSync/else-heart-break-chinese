using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200002A RID: 42
	public class TrashCan : MimanTing
	{
		// Token: 0x060003B9 RID: 953 RVA: 0x000136A0 File Offset: 0x000118A0
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "TrashCan");
			this.CELL_currentTrashName = base.EnsureCell<string>("currentTrash", "");
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000136E0 File Offset: 0x000118E0
		public override void FixBeforeSaving()
		{
			base.FixBeforeSaving();
			if (this.masterProgramName == "" || this.masterProgramName == "BlankSlate")
			{
				this.masterProgramName = "TrashCan";
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00013728 File Offset: 0x00011928
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00013730 File Offset: 0x00011930
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00013740 File Offset: 0x00011940
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003BE RID: 958
		public override string verbDescription
		{
			get
			{
				return "inspect检查";
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0001374C File Offset: 0x0001194C
		public override string tooltipName
		{
			get
			{
				return "trash can";
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00013754 File Offset: 0x00011954
		public void Throw(MimanTing pTing)
		{
			this.currentTrash = pTing;
			this.masterProgram.Start();
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00013768 File Offset: 0x00011968
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x000137A8 File Offset: 0x000119A8
		public Ting currentTrash
		{
			get
			{
				if (this.CELL_currentTrashName.data == "")
				{
					return null;
				}
				return this._tingRunner.GetTingUnsafe(this.CELL_currentTrashName.data);
			}
			set
			{
				if (value == null)
				{
					this.CELL_currentTrashName.data = "";
				}
				else
				{
					this.CELL_currentTrashName.data = value.name;
				}
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000137E4 File Offset: 0x000119E4
		public override void Update(float dt)
		{
			base.UpdateBubbleTimer();
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000137EC File Offset: 0x000119EC
		[SprakAPI(new string[] { "Move the current trash to a room" })]
		public void API_MoveToRoom(string roomName)
		{
			if (this.currentTrash == null)
			{
				this.Say("No current trash", "");
				return;
			}
			this.currentTrash.isBeingHeld = false;
			Room roomUnsafe = this._roomRunner.GetRoomUnsafe(roomName);
			if (roomUnsafe == null)
			{
				throw new Error("Can't find a room called " + roomName);
			}
			IntPoint[] points = roomUnsafe.points;
			PointTileNode pointTileNode = null;
			foreach (IntPoint intPoint in points)
			{
				PointTileNode tile = roomUnsafe.GetTile(intPoint);
				if (!tile.HasOccupants())
				{
					pointTileNode = tile;
					break;
				}
			}
			if (pointTileNode != null)
			{
				this.currentTrash.position = pointTileNode.position;
			}
			else
			{
				this.Say("Can't throw away thing, " + roomName + " is full!", "");
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000138CC File Offset: 0x00011ACC
		[SprakAPI(new string[] { "Delete the current trash (irreversibly)" })]
		public void API_Delete()
		{
			if (this.currentTrash == null)
			{
				this.Say("No current trash", "");
				return;
			}
			this._tingRunner.RemoveTing(this.currentTrash.name);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001390C File Offset: 0x00011B0C
		[SprakAPI(new string[] { "Check if the current trash is a specific type" })]
		public bool API_IsType(string type)
		{
			return this.API_GetType().ToLower() == type.ToLower();
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00013930 File Offset: 0x00011B30
		[SprakAPI(new string[] { "Get the type of the current trash" })]
		public string API_GetType()
		{
			if (this.currentTrash == null)
			{
				this.Say("No current trash", "");
				return "";
			}
			string text = this.currentTrash.GetType().Name.ToLower();
			if (text == "hackdev")
			{
				text = "modifier";
			}
			return text;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001398C File Offset: 0x00011B8C
		[SprakAPI(new string[] { "Say something" })]
		public void API_Say(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001399C File Offset: 0x00011B9C
		[SprakAPI(new string[] { "Sleep", "seconds to sleep" })]
		public void API_Sleep(float time)
		{
			this.masterProgram.sleepTimer = time;
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003CA RID: 970 RVA: 0x000139AC File Offset: 0x00011BAC
		// (set) Token: 0x060003CB RID: 971 RVA: 0x000139BC File Offset: 0x00011BBC
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

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003CC RID: 972 RVA: 0x000139CC File Offset: 0x00011BCC
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(TrashCan)));
				}
				return this._program;
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00013A24 File Offset: 0x00011C24
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
			this.currentTrash = null;
		}

		// Token: 0x040000E9 RID: 233
		public new static string TABLE_NAME = "Tings_TrashCans";

		// Token: 0x040000EA RID: 234
		private ValueEntry<string> CELL_programName;

		// Token: 0x040000EB RID: 235
		private ValueEntry<string> CELL_currentTrashName;

		// Token: 0x040000EC RID: 236
		private Program _program;
	}
}
