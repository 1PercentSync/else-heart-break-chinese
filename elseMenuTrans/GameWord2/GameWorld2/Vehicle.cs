using System;
using GameTypes;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200007D RID: 125
	public class Vehicle : MimanTing
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x0001FDC4 File Offset: 0x0001DFC4
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_movingDoorName = base.EnsureCell<string>("movingDoorName", "");
			this.CELL_currentNavNodeName = base.EnsureCell<string>("currentNavNodeName", "");
			this.CELL_nextNavNodeName = base.EnsureCell<string>("nextNavNodeName", "");
			this.CELL_turning = base.EnsureCell<VehicleTurningDirection>("turning", VehicleTurningDirection.FORWARD);
			this.CELL_speed = base.EnsureCell<float>("speed", 0f);
			this.CELL_distance = base.EnsureCell<float>("distance", 0f);
			base.AddDataListener<WorldCoordinate>("position", new ValueEntry<WorldCoordinate>.DataChangeHandler(this.OnPositionChanged));
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001FE70 File Offset: 0x0001E070
		public override bool DoesMasterProgramExist()
		{
			return false;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001FE74 File Offset: 0x0001E074
		~Vehicle()
		{
			base.RemoveDataListener<WorldCoordinate>("position", new ValueEntry<WorldCoordinate>.DataChangeHandler(this.OnPositionChanged));
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001FEC0 File Offset: 0x0001E0C0
		public override void Update(float dt)
		{
			this.UpdateMovingDoorPosition();
			if (this.currentNavNode == null || this.nextNavNode == null)
			{
				return;
			}
			if (this.safetySwitchOn)
			{
				if (this.movingDoor.actionName == "")
				{
					this.safetySwitchOn = false;
				}
				return;
			}
			this.distance += this.speed * dt;
			if (this.distanceFraction >= 0.99f)
			{
				NavNode currentNavNode = this.currentNavNode;
				this.distance = 0f;
				this.currentNavNode = this.nextNavNode;
				this.nextNavNode = this._tingRunner.GetTingUnsafe(this.currentNavNode.mainTrackName) as NavNode;
				if (this.nextNavNode == null)
				{
					throw new Exception(string.Concat(new object[]
					{
						"nextNavNode is null for vehicle ",
						base.name,
						" at position ",
						base.position,
						" and current nav node ",
						this.currentNavNode.name
					}));
				}
				base.position = this.currentNavNode.position;
				if (this.onNewNavNode != null)
				{
					this.onNewNavNode(currentNavNode, this.currentNavNode);
				}
			}
			if (this.currentNavNode.room != this.nextNavNode.room)
			{
				base.position = new WorldCoordinate(base.room.name, 10000, 10000);
			}
			else
			{
				IntPoint intPoint = this.nextNavNode.localPoint - this.currentNavNode.localPoint;
				IntPoint intPoint2 = this.currentNavNode.localPoint + intPoint.scale(this.distanceFraction);
				base.position = new WorldCoordinate(base.room.name, intPoint2);
				base.direction = intPoint.Clamped().ToDirection();
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x000200A8 File Offset: 0x0001E2A8
		public override bool autoUnregisterFromUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x000200AC File Offset: 0x0001E2AC
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x000200BC File Offset: 0x0001E2BC
		[EditableInEditor]
		public string movingDoorName
		{
			get
			{
				return this.CELL_movingDoorName.data;
			}
			set
			{
				this.CELL_movingDoorName.data = value;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x000200CC File Offset: 0x0001E2CC
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x000200DC File Offset: 0x0001E2DC
		[EditableInEditor]
		public float distance
		{
			get
			{
				return this.CELL_distance.data;
			}
			set
			{
				this.CELL_distance.data = value;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x000200EC File Offset: 0x0001E2EC
		[ShowInEditor]
		public float distanceFraction
		{
			get
			{
				if (this.distanceBetweenNodes == 0f)
				{
					return 1f;
				}
				return this.distance / this.distanceBetweenNodes;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0002011C File Offset: 0x0001E31C
		[ShowInEditor]
		public float distanceBetweenNodes
		{
			get
			{
				if (this.currentNavNode == null)
				{
					return 0f;
				}
				if (this.nextNavNode == null)
				{
					return 0f;
				}
				if (this.currentNavNode.room != this.nextNavNode.room)
				{
					return 0f;
				}
				IntPoint localPoint = this.currentNavNode.localPoint;
				IntPoint localPoint2 = this.nextNavNode.localPoint;
				return (float)(Math.Abs(localPoint.x - localPoint2.x) + Math.Abs(localPoint.y - localPoint2.y));
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x000201B0 File Offset: 0x0001E3B0
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x000201C0 File Offset: 0x0001E3C0
		[EditableInEditor]
		public VehicleTurningDirection turning
		{
			get
			{
				return this.CELL_turning.data;
			}
			set
			{
				this.CELL_turning.data = value;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x000201D0 File Offset: 0x0001E3D0
		public virtual float maxSpeed
		{
			get
			{
				return 40f;
			}
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x000201D8 File Offset: 0x0001E3D8
		private float Clamp(float value, float min, float max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x000201F0 File Offset: 0x0001E3F0
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x00020200 File Offset: 0x0001E400
		[EditableInEditor]
		public float speed
		{
			get
			{
				return this.CELL_speed.data;
			}
			set
			{
				this.CELL_speed.data = this.Clamp(value, 0f, this.maxSpeed);
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00020220 File Offset: 0x0001E420
		public Door movingDoor
		{
			get
			{
				if (this._movingDoorCache == null)
				{
					this._movingDoorCache = this._tingRunner.GetTingUnsafe(this.CELL_movingDoorName.data) as Door;
				}
				return this._movingDoorCache;
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00020260 File Offset: 0x0001E460
		private void OnPositionChanged(WorldCoordinate pPrevPos, WorldCoordinate pNewPos)
		{
			this.UpdateMovingDoorPosition();
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x00020268 File Offset: 0x0001E468
		public virtual IntPoint movingDoorPositionOffset
		{
			get
			{
				return base.localPoint + IntPoint.DirectionToIntPoint(IntPoint.Turn(base.direction, 90)) * 4;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00020298 File Offset: 0x0001E498
		public virtual int movingDoorRotationOffset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0002029C File Offset: 0x0001E49C
		private void UpdateMovingDoorPosition()
		{
			if (this.movingDoor != null)
			{
				this.movingDoor.position = new WorldCoordinate(base.position.roomName, this.movingDoorPositionOffset);
				this.movingDoor.direction = IntPoint.Turn(base.direction, this.movingDoorRotationOffset);
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x000202F4 File Offset: 0x0001E4F4
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x00020304 File Offset: 0x0001E504
		[EditableInEditor]
		public string nextNavNodeName
		{
			get
			{
				return this.CELL_nextNavNodeName.data;
			}
			set
			{
				this.CELL_nextNavNodeName.data = value;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x00020314 File Offset: 0x0001E514
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x00020334 File Offset: 0x0001E534
		public NavNode nextNavNode
		{
			get
			{
				return this._tingRunner.GetTingUnsafe(this.CELL_nextNavNodeName.data) as NavNode;
			}
			set
			{
				if (value == null)
				{
					this.CELL_nextNavNodeName.data = "";
				}
				else
				{
					this.CELL_nextNavNodeName.data = value.name;
				}
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x00020370 File Offset: 0x0001E570
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x00020380 File Offset: 0x0001E580
		[EditableInEditor]
		public string currentNavNodeName
		{
			get
			{
				return this.CELL_currentNavNodeName.data;
			}
			set
			{
				this.CELL_currentNavNodeName.data = value;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x00020390 File Offset: 0x0001E590
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x000203B0 File Offset: 0x0001E5B0
		public NavNode currentNavNode
		{
			get
			{
				return this._tingRunner.GetTingUnsafe(this.CELL_currentNavNodeName.data) as NavNode;
			}
			set
			{
				if (value == null)
				{
					this.CELL_currentNavNodeName.data = "";
				}
				else
				{
					this.CELL_currentNavNodeName.data = value.name;
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x000203EC File Offset: 0x0001E5EC
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x000203F0 File Offset: 0x0001E5F0
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040001DE RID: 478
		public new static string TABLE_NAME = "Tings_Vehicles";

		// Token: 0x040001DF RID: 479
		private ValueEntry<string> CELL_movingDoorName;

		// Token: 0x040001E0 RID: 480
		private ValueEntry<string> CELL_currentNavNodeName;

		// Token: 0x040001E1 RID: 481
		private ValueEntry<string> CELL_nextNavNodeName;

		// Token: 0x040001E2 RID: 482
		private ValueEntry<VehicleTurningDirection> CELL_turning;

		// Token: 0x040001E3 RID: 483
		private ValueEntry<float> CELL_speed;

		// Token: 0x040001E4 RID: 484
		private ValueEntry<float> CELL_distance;

		// Token: 0x040001E5 RID: 485
		private Door _movingDoorCache;

		// Token: 0x040001E6 RID: 486
		public bool safetySwitchOn = false;

		// Token: 0x040001E7 RID: 487
		public Vehicle.OnNewNavNode onNewNavNode;

		// Token: 0x0200007E RID: 126
		// (Invoke) Token: 0x0600071E RID: 1822
		public delegate void OnNewNavNode(NavNode pOldNavNode, NavNode pNewNavNode);
	}
}
