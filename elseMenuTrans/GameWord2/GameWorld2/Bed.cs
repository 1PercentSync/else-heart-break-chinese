using System;
using GameTypes;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200000F RID: 15
	public class Bed : MimanTing
	{
		// Token: 0x0600017B RID: 379 RVA: 0x000092DC File Offset: 0x000074DC
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_exitPoint = base.EnsureCell<int>("exitPoint", 0);
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000092F8 File Offset: 0x000074F8
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[]
				{
					base.localPoint + IntPoint.DirectionToIntPoint(IntPoint.Turn(base.direction, 0)) * 2,
					base.localPoint + IntPoint.DirectionToIntPoint(IntPoint.Turn(base.direction, 180)) * 2
				};
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000936C File Offset: 0x0000756C
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00009374 File Offset: 0x00007574
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

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600017F RID: 383 RVA: 0x000093D8 File Offset: 0x000075D8
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000093DC File Offset: 0x000075DC
		public override bool DoesMasterProgramExist()
		{
			return false;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000093E0 File Offset: 0x000075E0
		public IntPoint GetCurrentExitPoint()
		{
			return this.interactionPoints[this.CELL_exitPoint.data];
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00009400 File Offset: 0x00007600
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00009410 File Offset: 0x00007610
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00009420 File Offset: 0x00007620
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00009424 File Offset: 0x00007624
		public override string verbDescription
		{
			get
			{
				return "sleep in睡在";
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000942C File Offset: 0x0000762C
		public override string tooltipName
		{
			get
			{
				return "bed床";
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00009434 File Offset: 0x00007634
		public override bool isBeingUsed
		{
			get
			{
				return base.AnotherTingSharesTheTile();
			}
		}

		// Token: 0x04000074 RID: 116
		private ValueEntry<int> CELL_exitPoint;
	}
}
