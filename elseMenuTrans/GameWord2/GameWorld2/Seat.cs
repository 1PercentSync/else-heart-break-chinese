using System;
using GameTypes;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000013 RID: 19
	public class Seat : MimanTing
	{
		// Token: 0x060001AF RID: 431 RVA: 0x000098C4 File Offset: 0x00007AC4
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_exitPoint = base.EnsureCell<int>("exitPoint", 0);
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x000098E0 File Offset: 0x00007AE0
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[]
				{
					base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 1,
					base.localPoint + IntPoint.DirectionToIntPoint(base.direction).RotatedWithDegrees(-90f),
					base.localPoint + IntPoint.DirectionToIntPoint(base.direction).RotatedWithDegrees(90f)
				};
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000997C File Offset: 0x00007B7C
		public override bool DoesMasterProgramExist()
		{
			return false;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00009980 File Offset: 0x00007B80
		public IntPoint computerPoint
		{
			get
			{
				return base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 2;
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000099AC File Offset: 0x00007BAC
		public void CalculateNewExitPoint()
		{
			IntPoint[] interactionPoints = this.interactionPoints;
			Room room = base.room;
			for (int i = 0; i < interactionPoints.Length; i++)
			{
				IntPoint intPoint = interactionPoints[i];
				PointTileNode tile = room.GetTile(intPoint);
				if (tile != null && !tile.HasOccupants())
				{
					this.exitPoint = i;
					break;
				}
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00009A10 File Offset: 0x00007C10
		public IntPoint GetCurrentExitPoint()
		{
			return this.interactionPoints[this.CELL_exitPoint.data];
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00009A30 File Offset: 0x00007C30
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00009A40 File Offset: 0x00007C40
		[EditableInEditor]
		public int exitPoint
		{
			get
			{
				return this.CELL_exitPoint.data;
			}
			set
			{
				this.CELL_exitPoint.data = value;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00009A50 File Offset: 0x00007C50
		public override string tooltipName
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001B8 RID: 440
		public override string verbDescription
		{
			get
			{
				return "sit down坐下";
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00009A60 File Offset: 0x00007C60
		[ShowInEditor]
		public override bool isBeingUsed
		{
			get
			{
				return base.AnotherTingSharesTheTile();
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00009A68 File Offset: 0x00007C68
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0400007D RID: 125
		private ValueEntry<int> CELL_exitPoint;
	}
}
