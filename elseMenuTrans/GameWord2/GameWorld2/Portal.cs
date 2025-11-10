using System;
using GameTypes;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200002B RID: 43
	public class Portal : MimanTing, IExit
	{
		// Token: 0x060003D0 RID: 976 RVA: 0x00013A68 File Offset: 0x00011C68
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_targetPortalName = base.EnsureCell<string>("targetPortalName", "");
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00013A88 File Offset: 0x00011C88
		public override void Init()
		{
			base.Init();
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00013A90 File Offset: 0x00011C90
		public override bool DoesMasterProgramExist()
		{
			return false;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00013A94 File Offset: 0x00011C94
		private void WriteTargetToTile()
		{
			D.isNull(base.room, "room is null");
			D.isNull(this._roomRunner, "room runner is null!");
			PointTileNode tile = base.room.GetTile(this.interactionPoints[0]);
			if (tile == null)
			{
				return;
			}
			tile.AddOccupant(this);
			if (this.targetPosition == WorldCoordinate.NONE)
			{
				tile.teleportTarget = null;
			}
			else
			{
				tile.teleportTarget = this._roomRunner.GetRoom(this.targetPosition.roomName).GetTile(this.targetPosition.localPosition);
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00013B40 File Offset: 0x00011D40
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00013B48 File Offset: 0x00011D48
		public void WalkThrough(Character pCharacter)
		{
			if (this.targetPortal == null)
			{
				this._worldSettings.Notify(pCharacter.name, "This arrow is leading nowhere!");
				return;
			}
			WorldCoordinate worldCoordinate = new WorldCoordinate(this.targetPortal.room.name, this.targetPortal.interactionPoints[0]);
			this.logger.Log(string.Concat(new object[] { base.name, " used the portal ", base.name, " and will now teleport to ", worldCoordinate }));
			pCharacter.position = worldCoordinate;
			pCharacter.direction = this.targetPortal.direction;
			pCharacter.StopAction();
			pCharacter.StartAction("WalkingThroughPortalPhase2", null, 2.2f, 2.2f);
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00013C1C File Offset: 0x00011E1C
		[ShowInEditor]
		public WorldCoordinate targetPosition
		{
			get
			{
				Portal targetPortal = this.targetPortal;
				if (targetPortal != null)
				{
					return new WorldCoordinate(targetPortal.room.name, targetPortal.interactionPoints[0]);
				}
				return WorldCoordinate.NONE;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00013C60 File Offset: 0x00011E60
		public Portal targetPortal
		{
			get
			{
				return this._tingRunner.GetTingUnsafe(this.targetPortalName) as Portal;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00013C78 File Offset: 0x00011E78
		[ShowInEditor]
		public string targetPortalReferenceStatus
		{
			get
			{
				if (this.targetPortal == null)
				{
					return "null";
				}
				return "OK, " + this.targetPortal.name;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00013CAC File Offset: 0x00011EAC
		// (set) Token: 0x060003DA RID: 986 RVA: 0x00013CBC File Offset: 0x00011EBC
		[EditableInEditor]
		public string targetPortalName
		{
			get
			{
				return this.CELL_targetPortalName.data;
			}
			set
			{
				this.CELL_targetPortalName.data = value;
				this.WriteTargetToTile();
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003DB RID: 987 RVA: 0x00013CD0 File Offset: 0x00011ED0
		public override string tooltipName
		{
			get
			{
				return "another area";
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003DC RID: 988 RVA: 0x00013CD8 File Offset: 0x00011ED8
		public override string verbDescription
		{
			get
			{
				return "walk over to";
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003DD RID: 989 RVA: 0x00013CE0 File Offset: 0x00011EE0
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[] { base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 2 };
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00013D1C File Offset: 0x00011F1C
		public Ting GetLinkTarget()
		{
			return this.targetPortal;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00013D24 File Offset: 0x00011F24
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040000ED RID: 237
		public new static string TABLE_NAME = "Ting_Portals";

		// Token: 0x040000EE RID: 238
		private ValueEntry<string> CELL_targetPortalName;
	}
}
