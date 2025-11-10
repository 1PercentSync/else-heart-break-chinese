using System;
using System.Collections.Generic;
using System.Text;
using GameTypes;
using GrimmLib;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200002E RID: 46
	public class Timetable : RelayObjectTwo
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x000141AC File Offset: 0x000123AC
		protected override void SetupCells()
		{
			this.CELL_name = base.EnsureCell<string>("name", "unnamed");
			this.CELL_fileContent = base.EnsureCell<string>("fileContent", "");
			this.GenerateTimetableSpansFromContentString();
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000141EC File Offset: 0x000123EC
		public void CreateTimetableSpanInternal(GameTime pStartTime, GameTime pEndTime, TimetableBehaviour pBehaviour)
		{
			TimetableSpan timetableSpan = new TimetableSpan
			{
				startTime = pStartTime,
				endTime = pEndTime,
				behaviour = pBehaviour
			};
			this._timetableSpans.Add(timetableSpan);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001422C File Offset: 0x0001242C
		public void Update(float dt, GameTime pCurrentTime, Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			TimetableSpan currentSpan = this.GetCurrentSpan(pCurrentTime);
			if (currentSpan != TimetableSpan.NULL)
			{
				if (pCharacter.isAvatar)
				{
					return;
				}
				if (pCharacter.lastTimetableSpan != currentSpan)
				{
					if (pCharacter.lastTimetableSpan != TimetableSpan.NULL)
					{
						pCharacter.logger.Log(pCharacter.name + " ended span " + pCharacter.lastTimetableSpan.name);
						pCharacter.lastTimetableSpan.behaviour.OnFinish(pCharacter, pTingRunner, pRoomRunner, pDialogueRunner);
					}
					else
					{
						pCharacter.logger.Log(pCharacter.name + " ended span NULL");
					}
				}
				pCharacter.lastTimetableSpan = currentSpan;
				if (pCharacter.timetableTimer <= 0f)
				{
					pCharacter.timetableTimer = currentSpan.behaviour.Execute(pCharacter, pTingRunner, pRoomRunner, pDialogueRunner, pWorldSettings);
				}
				else
				{
					pCharacter.timetableTimer -= dt;
				}
			}
			else
			{
				D.Log(string.Concat(new object[] { "Found no matching time span in Timetable for character ", pCharacter.name, " at time ", pCurrentTime }));
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001435C File Offset: 0x0001255C
		public TimetableSpan GetCurrentSpan(GameTime pCurrentTime)
		{
			foreach (TimetableSpan timetableSpan in this._timetableSpans)
			{
				if (timetableSpan.IsTimeWithinBounds(pCurrentTime))
				{
					return timetableSpan;
				}
			}
			return TimetableSpan.NULL;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x000143D8 File Offset: 0x000125D8
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x000143E8 File Offset: 0x000125E8
		public string name
		{
			get
			{
				return this.CELL_name.data;
			}
			set
			{
				this.CELL_name.data = value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x000143F8 File Offset: 0x000125F8
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x00014408 File Offset: 0x00012608
		public string fileContent
		{
			get
			{
				return this.CELL_fileContent.data;
			}
			set
			{
				this.CELL_fileContent.data = value;
				this.GenerateTimetableSpansFromContentString();
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0001441C File Offset: 0x0001261C
		public TimetableSpan[] timetableSpans
		{
			get
			{
				return this._timetableSpans.ToArray();
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001442C File Offset: 0x0001262C
		public string TimetableSpansToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (TimetableSpan timetableSpan in this._timetableSpans)
			{
				stringBuilder.Append(timetableSpan.ToString() + "\n");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x000144B8 File Offset: 0x000126B8
		private void GenerateTimetableSpansFromContentString()
		{
			int num = 0;
			foreach (string text in this.fileContent.Split(new char[] { '\n' }))
			{
				try
				{
					this.ProcessLine(text);
				}
				catch (Exception ex)
				{
					string text2 = string.Concat(new object[]
					{
						"[",
						num,
						"] Couldn't process line '",
						text,
						"', error: ",
						ex.ToString()
					});
					throw new Exception(text2);
				}
				num++;
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00014570 File Offset: 0x00012770
		private void ProcessLine(string pLine)
		{
			string[] array = pLine.Split(new char[] { ' ', '\t', ':' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				text.Trim();
			}
			if (array.Length <= 6)
			{
				return;
			}
			int num = Convert.ToInt32(array[0]);
			int num2 = Convert.ToInt32(array[1]);
			int num3 = Convert.ToInt32(array[3]);
			int num4 = Convert.ToInt32(array[4]);
			string text2 = array[5];
			string[] array3 = new string[array.Length - 6];
			for (int j = 0; j < array3.Length; j++)
			{
				array3[j] = array[j + 6];
			}
			this._timetableSpans.Add(this.CreateTimetableSpan(num, num2, num3, num4, text2, array3));
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001463C File Offset: 0x0001283C
		private TimetableSpan CreateTimetableSpan(int pStartHour, int pStartMinute, int pEndHour, int pEndMinute, string pName, string[] pTokens)
		{
			GameTime gameTime = new GameTime(pStartHour, pStartMinute);
			GameTime gameTime2 = new GameTime(pEndHour, pEndMinute);
			return new TimetableSpan
			{
				name = pName,
				startTime = gameTime,
				endTime = gameTime2,
				behaviour = this.CreateBehaviourFromTokens(pTokens)
			};
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00014690 File Offset: 0x00012890
		private TimetableBehaviour CreateBehaviourFromTokens(string[] pTokens)
		{
			string text = pTokens[0];
			string text2 = text;
			switch (text2)
			{
			case "BeAtPosition":
				return new Behaviour_BeAtPosition(this.GetWorldCoordinateFromTokens(pTokens, 1));
			case "BeAtTing":
				return new Behaviour_BeAtTing(pTokens[1]);
			case "RunStory":
				return new Behaviour_RunStory(pTokens[1]);
			case "BeInRoom":
				return new Behaviour_BeInRoom(pTokens[1]);
			case "WorkWithModifier":
				return new Behaviour_WorkWithModifier(pTokens[1]);
			case "Fika":
				return new Behaviour_Fika(pTokens[1]);
			case "Party":
				return new Behaviour_Party(pTokens[1]);
			case "Guard":
				return new Behaviour_Guard(pTokens[1]);
			case "Sleep":
				return new Behaviour_Sleep(pTokens[1]);
			case "Interact":
				return new Behaviour_Interact(pTokens[1], false);
			case "Hack":
				return new Behaviour_Interact(pTokens[1], true);
			case "Drink":
				return new Behaviour_Drink(pTokens[1]);
			case "ServeDrinks":
				return new Behaviour_ServeDrinks(pTokens[1]);
			case "Sit":
			{
				List<string> list = new List<string>();
				for (int i = 1; i < pTokens.Length; i++)
				{
					list.Add(pTokens[i]);
				}
				return new Behaviour_Sit(list.ToArray());
			}
			case "Photograph":
				return new Behaviour_Photograph();
			case "RefineGoods":
				return new Behaviour_RefineGoods(pTokens[1]);
			case "GuideTo":
				return new Behaviour_GuideTo(pTokens[1], pTokens[2]);
			case "Sell":
				return new Behaviour_Sell(pTokens);
			case "Smoke":
				return new Behaviour_Smoke(pTokens[1]);
			case "PlayTrumpet":
				return new Behaviour_PlayTrumpet(pTokens[1]);
			case "Dj":
			{
				List<string> list2 = new List<string>();
				for (int j = 2; j < pTokens.Length; j++)
				{
					list2.Add(pTokens[j]);
				}
				return new Behaviour_Dj(pTokens[1], list2.ToArray());
			}
			}
			throw new Exception("Can't understand token " + pTokens[0]);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00014970 File Offset: 0x00012B70
		private WorldCoordinate GetWorldCoordinateFromTokens(string[] pTokens, int pStartIndex)
		{
			int num = Convert.ToInt32(pTokens[pStartIndex + 1]);
			int num2 = Convert.ToInt32(pTokens[pStartIndex + 2]);
			return new WorldCoordinate(pTokens[pStartIndex], new IntPoint(num, num2));
		}

		// Token: 0x040000F7 RID: 247
		public const string TABLE_NAME = "Timetables";

		// Token: 0x040000F8 RID: 248
		private ValueEntry<string> CELL_name;

		// Token: 0x040000F9 RID: 249
		private ValueEntry<string> CELL_fileContent;

		// Token: 0x040000FA RID: 250
		private List<TimetableSpan> _timetableSpans = new List<TimetableSpan>();
	}
}
