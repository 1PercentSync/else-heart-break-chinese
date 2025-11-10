using System;
using GameTypes;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000074 RID: 116
	public class Locker : MimanTing
	{
		// Token: 0x06000695 RID: 1685 RVA: 0x0001F064 File Offset: 0x0001D264
		public override void Init()
		{
			base.Init();
			if (!this._roomRunner.HasRoom(this.inventoryRoomName))
			{
				SimpleRoomBuilder simpleRoomBuilder = new SimpleRoomBuilder(this._roomRunner);
				simpleRoomBuilder.CreateRoomWithSize(this.inventoryRoomName, 5, 4);
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x0001F0A8 File Offset: 0x0001D2A8
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[] { base.localPoint + IntPoint.Up * 2 };
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001F0E0 File Offset: 0x0001D2E0
		public override bool DoesMasterProgramExist()
		{
			return false;
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0001F0E4 File Offset: 0x0001D2E4
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000699 RID: 1689
		public override string verbDescription
		{
			get
			{
				return "open打开";
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600069A RID: 1690
		public override string tooltipName
		{
			get
			{
				return "locker储物柜";
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0001F0F8 File Offset: 0x0001D2F8
		public string inventoryRoomName
		{
			get
			{
				return base.name + "_locker";
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001F10C File Offset: 0x0001D30C
		public Ting[] GetItems()
		{
			return this._roomRunner.GetRoom(this.inventoryRoomName).GetTings().ToArray();
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0001F134 File Offset: 0x0001D334
		public bool isFull
		{
			get
			{
				return this.GetItems().Length >= 20;
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001F148 File Offset: 0x0001D348
		public bool PutTingIntoRandomFreeSpot(MimanTing pTing)
		{
			Room room = this._roomRunner.GetRoom(this.inventoryRoomName);
			foreach (IntPoint intPoint in room.points)
			{
				if (!room.GetTile(intPoint).HasOccupants())
				{
					pTing.position = new WorldCoordinate(this.inventoryRoomName, intPoint);
					return true;
				}
			}
			D.Log("No free spot in the locker");
			return false;
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0001F1C0 File Offset: 0x0001D3C0
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}
	}
}
