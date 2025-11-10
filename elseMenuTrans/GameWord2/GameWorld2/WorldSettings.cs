using System;
using System.Collections.Generic;
using System.Linq;
using GameTypes;
using RelayLib;

namespace GameWorld2
{
	// Token: 0x0200001D RID: 29
	public class WorldSettings
	{
		// Token: 0x060002D7 RID: 727 RVA: 0x00011CA8 File Offset: 0x0000FEA8
		public WorldSettings(RelayTwo pRelay)
		{
			this._table = pRelay.GetTable("WorldSettings");
			this.SetupCells();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00011CC8 File Offset: 0x0000FEC8
		public void Notify(string pName, string pMessage)
		{
			if (this.onNotification != null && !this.muteNotifications)
			{
				this.onNotification(pName, pMessage);
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00011CF0 File Offset: 0x0000FEF0
		public void Hint(string pMessage)
		{
			if (this.onHint != null)
			{
				this.onHint(pMessage);
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00011D0C File Offset: 0x0000FF0C
		private void SetupCells()
		{
			while ((float)this._table.GetRows().Length < 16f)
			{
				this._table.CreateRow();
			}
			this.CELL_activeRoom = this._table.GetValueEntryEnsureDefault<string>(0, "activeRoom", "Oblivion");
			this.CELL_avatarName = this._table.GetValueEntryEnsureDefault<string>(1, "avatarName", "Sebastian");
			this.CELL_totalWorldTime = this._table.GetValueEntryEnsureDefault<float>(2, "totalWorldTime", 0f);
			this.CELL_gameTimeSeconds = this._table.GetValueEntryEnsureDefault<float>(3, "gameTimeSeconds", 72000f);
			this.CELL_gameTimeSpeed = this._table.GetValueEntryEnsureDefault<float>(4, "gameTimeSpeed", 100f);
			this.CELL_cameraAutoRotateSpeed = this._table.GetValueEntryEnsureDefault<float>(5, "cameraAutoRotateSpeed", 0f);
			this.CELL_rain = this._table.GetValueEntryEnsureDefault<float>(6, "rain", 0f);
			this.CELL_tickNr = this._table.GetValueEntryEnsureDefault<int>(7, "tickNr", 0);
			this.CELL_rainTargetValue = this._table.GetValueEntryEnsureDefault<float>(8, "rainTargetValue", 0f);
			this.CELL_dynamicallyCreatedTingsCount = this._table.GetValueEntryEnsureDefault<int>(9, "dynamicallyCreatedTingsCount", 0);
			this.CELL_translationLanguage = this._table.GetValueEntryEnsureDefault<string>(10, "translationLanguage", "swe");
			this.CELL_muteNotifications = this._table.GetValueEntryEnsureDefault<bool>(11, "muteNotifications", false);
			this.CELL_focusedDialogue = this._table.GetValueEntryEnsureDefault<string>(12, "focusedDialogue", "");
			this.CELL_beaten = this._table.GetValueEntryEnsureDefault<bool>(13, "beaten", false);
			this.CELL_heartIsBroken = this._table.GetValueEntryEnsureDefault<bool>(14, "heartIsBroken", false);
			this.CELL_storyEventLog = this._table.GetValueEntryEnsureDefault<string[]>(15, "storyEventLog", new string[0]);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00011EF8 File Offset: 0x000100F8
		public void UpdateRain(float dt)
		{
			float num = 50f;
			float num2 = this.rainTargetValue - this.rain;
			if (Math.Abs(num2) < 20f)
			{
				this.rain = this.rainTargetValue;
			}
			else if (num2 < 0f)
			{
				this.rain -= num * dt;
			}
			else if (num2 > 0f)
			{
				this.rain += num * dt;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00011F78 File Offset: 0x00010178
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00011F88 File Offset: 0x00010188
		public string activeRoom
		{
			get
			{
				return this.CELL_activeRoom.data;
			}
			set
			{
				this.CELL_activeRoom.data = value;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00011F98 File Offset: 0x00010198
		// (set) Token: 0x060002DF RID: 735 RVA: 0x00011FA8 File Offset: 0x000101A8
		public string avatarName
		{
			get
			{
				return this.CELL_avatarName.data;
			}
			set
			{
				this.CELL_avatarName.data = value;
				D.Log("avatarName was set to " + value);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00011FC8 File Offset: 0x000101C8
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x00011FD8 File Offset: 0x000101D8
		public float totalWorldTime
		{
			get
			{
				return this.CELL_totalWorldTime.data;
			}
			set
			{
				this.CELL_totalWorldTime.data = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00011FE8 File Offset: 0x000101E8
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x00011FF8 File Offset: 0x000101F8
		public float gameTimeSeconds
		{
			get
			{
				return this.CELL_gameTimeSeconds.data;
			}
			set
			{
				this.CELL_gameTimeSeconds.data = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00012008 File Offset: 0x00010208
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00012018 File Offset: 0x00010218
		public GameTime gameTimeClock
		{
			get
			{
				return new GameTime(this.gameTimeSeconds);
			}
			private set
			{
				this.gameTimeSeconds = value.totalSeconds;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00012028 File Offset: 0x00010228
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x00012038 File Offset: 0x00010238
		public float gameTimeSpeed
		{
			get
			{
				return this.CELL_gameTimeSpeed.data;
			}
			set
			{
				this.CELL_gameTimeSpeed.data = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00012048 File Offset: 0x00010248
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x00012058 File Offset: 0x00010258
		public float cameraAutoRotateSpeed
		{
			get
			{
				return this.CELL_cameraAutoRotateSpeed.data;
			}
			set
			{
				this.CELL_cameraAutoRotateSpeed.data = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00012068 File Offset: 0x00010268
		// (set) Token: 0x060002EB RID: 747 RVA: 0x00012078 File Offset: 0x00010278
		public float rain
		{
			get
			{
				return this.CELL_rain.data;
			}
			set
			{
				this.CELL_rain.data = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00012088 File Offset: 0x00010288
		// (set) Token: 0x060002ED RID: 749 RVA: 0x00012098 File Offset: 0x00010298
		public int tickNr
		{
			get
			{
				return this.CELL_tickNr.data;
			}
			set
			{
				this.CELL_tickNr.data = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002EE RID: 750 RVA: 0x000120A8 File Offset: 0x000102A8
		// (set) Token: 0x060002EF RID: 751 RVA: 0x000120B8 File Offset: 0x000102B8
		public float rainTargetValue
		{
			get
			{
				return this.CELL_rainTargetValue.data;
			}
			set
			{
				this.CELL_rainTargetValue.data = value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x000120C8 File Offset: 0x000102C8
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x000120D8 File Offset: 0x000102D8
		public int dynamicallyCreatedTingsCount
		{
			get
			{
				return this.CELL_dynamicallyCreatedTingsCount.data;
			}
			set
			{
				this.CELL_dynamicallyCreatedTingsCount.data = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x000120E8 File Offset: 0x000102E8
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x000120F8 File Offset: 0x000102F8
		public string translationLanguage
		{
			get
			{
				return this.CELL_translationLanguage.data;
			}
			set
			{
				this.CELL_translationLanguage.data = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00012108 File Offset: 0x00010308
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x00012118 File Offset: 0x00010318
		public bool muteNotifications
		{
			get
			{
				return this.CELL_muteNotifications.data;
			}
			set
			{
				this.CELL_muteNotifications.data = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00012128 File Offset: 0x00010328
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x00012138 File Offset: 0x00010338
		public string focusedDialogue
		{
			get
			{
				return this.CELL_focusedDialogue.data;
			}
			set
			{
				this.CELL_focusedDialogue.data = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00012148 File Offset: 0x00010348
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x00012158 File Offset: 0x00010358
		public bool beaten
		{
			get
			{
				return this.CELL_beaten.data;
			}
			set
			{
				this.CELL_beaten.data = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00012168 File Offset: 0x00010368
		// (set) Token: 0x060002FB RID: 763 RVA: 0x00012178 File Offset: 0x00010378
		public bool heartIsBroken
		{
			get
			{
				return this.CELL_heartIsBroken.data;
			}
			set
			{
				this.CELL_heartIsBroken.data = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00012188 File Offset: 0x00010388
		// (set) Token: 0x060002FD RID: 765 RVA: 0x00012198 File Offset: 0x00010398
		public string[] storyEventLog
		{
			get
			{
				return this.CELL_storyEventLog.data;
			}
			set
			{
				this.CELL_storyEventLog.data = value;
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000121A8 File Offset: 0x000103A8
		public void LogStoryEvent(string e)
		{
			List<string> list = this.storyEventLog.ToList<string>();
			list.Add(e);
			this.storyEventLog = list.ToArray();
		}

		// Token: 0x040000B1 RID: 177
		public const string TABLE_NAME = "WorldSettings";

		// Token: 0x040000B2 RID: 178
		public WorldSettings.OnNotification onNotification;

		// Token: 0x040000B3 RID: 179
		public WorldSettings.OnHint onHint;

		// Token: 0x040000B4 RID: 180
		public WorldSettings.OnCameraTarget onCameraTarget;

		// Token: 0x040000B5 RID: 181
		public WorldSettings.CopyToClipboard onCopyToClipboard;

		// Token: 0x040000B6 RID: 182
		private TableTwo _table;

		// Token: 0x040000B7 RID: 183
		private ValueEntry<string> CELL_activeRoom;

		// Token: 0x040000B8 RID: 184
		private ValueEntry<string> CELL_avatarName;

		// Token: 0x040000B9 RID: 185
		private ValueEntry<float> CELL_totalWorldTime;

		// Token: 0x040000BA RID: 186
		private ValueEntry<float> CELL_gameTimeSeconds;

		// Token: 0x040000BB RID: 187
		private ValueEntry<float> CELL_gameTimeSpeed;

		// Token: 0x040000BC RID: 188
		private ValueEntry<float> CELL_cameraAutoRotateSpeed;

		// Token: 0x040000BD RID: 189
		private ValueEntry<float> CELL_rain;

		// Token: 0x040000BE RID: 190
		private ValueEntry<int> CELL_tickNr;

		// Token: 0x040000BF RID: 191
		private ValueEntry<float> CELL_rainTargetValue;

		// Token: 0x040000C0 RID: 192
		private ValueEntry<int> CELL_dynamicallyCreatedTingsCount;

		// Token: 0x040000C1 RID: 193
		private ValueEntry<string> CELL_translationLanguage;

		// Token: 0x040000C2 RID: 194
		private ValueEntry<bool> CELL_muteNotifications;

		// Token: 0x040000C3 RID: 195
		private ValueEntry<string> CELL_focusedDialogue;

		// Token: 0x040000C4 RID: 196
		private ValueEntry<bool> CELL_beaten;

		// Token: 0x040000C5 RID: 197
		private ValueEntry<bool> CELL_heartIsBroken;

		// Token: 0x040000C6 RID: 198
		private ValueEntry<string[]> CELL_storyEventLog;

		// Token: 0x0200001E RID: 30
		// (Invoke) Token: 0x06000300 RID: 768
		public delegate void OnNotification(string pName, string pMessage);

		// Token: 0x0200001F RID: 31
		// (Invoke) Token: 0x06000304 RID: 772
		public delegate void OnHint(string pMessage);

		// Token: 0x02000020 RID: 32
		// (Invoke) Token: 0x06000308 RID: 776
		public delegate void OnCameraTarget(string pLookFromHere, string pTargetName);

		// Token: 0x02000021 RID: 33
		// (Invoke) Token: 0x0600030C RID: 780
		public delegate void CopyToClipboard(string text);
	}
}
