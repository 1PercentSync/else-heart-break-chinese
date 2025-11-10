using System;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200007F RID: 127
	public class NavNode : MimanTing
	{
		// Token: 0x06000723 RID: 1827 RVA: 0x00020408 File Offset: 0x0001E608
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_isSwitch = base.EnsureCell<bool>("isSwitch", false);
			this.CELL_mainTrack = base.EnsureCell<string>("mainTrack", "");
			this.CELL_leftTrack = base.EnsureCell<string>("leftTrack", "");
			this.CELL_rightTrack = base.EnsureCell<string>("rightTrack", "");
			this.CELL_isStation = base.EnsureCell<bool>("isStation", false);
			this.CELL_stationName = base.EnsureCell<string>("stationName", "");
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00020498 File Offset: 0x0001E698
		public override bool DoesMasterProgramExist()
		{
			return false;
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0002049C File Offset: 0x0001E69C
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x000204AC File Offset: 0x0001E6AC
		[EditableInEditor]
		public bool isSwitch
		{
			get
			{
				return this.CELL_isSwitch.data;
			}
			set
			{
				this.CELL_isSwitch.data = value;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x000204BC File Offset: 0x0001E6BC
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x000204CC File Offset: 0x0001E6CC
		[EditableInEditor]
		public string mainTrackName
		{
			get
			{
				return this.CELL_mainTrack.data;
			}
			set
			{
				this.CELL_mainTrack.data = value;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x000204DC File Offset: 0x0001E6DC
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x000204EC File Offset: 0x0001E6EC
		[EditableInEditor]
		public string leftTrackName
		{
			get
			{
				return this.CELL_leftTrack.data;
			}
			set
			{
				this.CELL_leftTrack.data = value;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x000204FC File Offset: 0x0001E6FC
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x0002050C File Offset: 0x0001E70C
		[EditableInEditor]
		public string rightTrackName
		{
			get
			{
				return this.CELL_rightTrack.data;
			}
			set
			{
				this.CELL_rightTrack.data = value;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0002051C File Offset: 0x0001E71C
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x0002053C File Offset: 0x0001E73C
		public NavNode mainTrack
		{
			get
			{
				return this._tingRunner.GetTingUnsafe(this.CELL_mainTrack.data) as NavNode;
			}
			set
			{
				if (value == null)
				{
					this.CELL_mainTrack.data = "";
				}
				else
				{
					this.CELL_mainTrack.data = value.name;
				}
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00020578 File Offset: 0x0001E778
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00020598 File Offset: 0x0001E798
		public NavNode leftTrack
		{
			get
			{
				return this._tingRunner.GetTingUnsafe(this.CELL_leftTrack.data) as NavNode;
			}
			set
			{
				if (value == null)
				{
					this.CELL_leftTrack.data = "";
				}
				else
				{
					this.CELL_leftTrack.data = value.name;
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x000205D4 File Offset: 0x0001E7D4
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x000205F4 File Offset: 0x0001E7F4
		public NavNode rightTrack
		{
			get
			{
				return this._tingRunner.GetTingUnsafe(this.CELL_rightTrack.data) as NavNode;
			}
			set
			{
				if (value == null)
				{
					this.CELL_rightTrack.data = "";
				}
				else
				{
					this.CELL_rightTrack.data = value.name;
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00020630 File Offset: 0x0001E830
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x00020640 File Offset: 0x0001E840
		[EditableInEditor]
		public bool isStation
		{
			get
			{
				return this.CELL_isStation.data;
			}
			set
			{
				this.CELL_isStation.data = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00020650 File Offset: 0x0001E850
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x00020660 File Offset: 0x0001E860
		[EditableInEditor]
		public string stationName
		{
			get
			{
				return this.CELL_stationName.data;
			}
			set
			{
				this.CELL_stationName.data = value;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x00020670 File Offset: 0x0001E870
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040001E8 RID: 488
		public new static string TABLE_NAME = "Tings_NavNode";

		// Token: 0x040001E9 RID: 489
		private ValueEntry<bool> CELL_isSwitch;

		// Token: 0x040001EA RID: 490
		private ValueEntry<string> CELL_mainTrack;

		// Token: 0x040001EB RID: 491
		private ValueEntry<string> CELL_leftTrack;

		// Token: 0x040001EC RID: 492
		private ValueEntry<string> CELL_rightTrack;

		// Token: 0x040001ED RID: 493
		private ValueEntry<bool> CELL_isStation;

		// Token: 0x040001EE RID: 494
		private ValueEntry<string> CELL_stationName;
	}
}
