using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RelayLib;

namespace GameWorld2
{
	// Token: 0x0200002F RID: 47
	public class TimetableRunner
	{
		// Token: 0x0600040D RID: 1037 RVA: 0x000149A4 File Offset: 0x00012BA4
		public TimetableRunner(RelayTwo pRelay)
		{
			this._timetableTable = pRelay.GetTable("Timetables");
			foreach (Timetable timetable in InstantiatorTwo.Process<Timetable>(this._timetableTable))
			{
				this._timetables[timetable.name] = timetable;
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00014A40 File Offset: 0x00012C40
		public void LoadTimetableFromFile(string pFilePath)
		{
			string text = FileHelper.GetNameFromFilepath(pFilePath);
			using (StreamReader streamReader = File.OpenText(pFilePath))
			{
				StringBuilder stringBuilder = new StringBuilder();
				while (!streamReader.EndOfStream)
				{
					string text2 = streamReader.ReadLine().Trim();
					if (text2.Length > 0 && text2.Substring(0, 1) == "[")
					{
						if (stringBuilder.Length > 0)
						{
							this.CreateTimetable(text, stringBuilder.ToString());
							stringBuilder = new StringBuilder();
						}
						text = text2.Substring(1, text2.Length - 2);
					}
					else
					{
						stringBuilder.Append(text2 + "\n");
					}
				}
				this.CreateTimetable(text, stringBuilder.ToString());
				streamReader.Close();
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00014B2C File Offset: 0x00012D2C
		public Timetable CreateTimetable(string pName, string pContent)
		{
			Timetable timetable = new Timetable();
			timetable.CreateNewRelayEntry(this._timetableTable, typeof(Timetable).Name);
			timetable.name = pName;
			timetable.fileContent = pContent;
			this._timetables[pName] = timetable;
			return timetable;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00014B78 File Offset: 0x00012D78
		public Timetable GetTimetable(string pName)
		{
			Timetable timetable = null;
			this._timetables.TryGetValue(pName, out timetable);
			if (timetable == null)
			{
				throw new Exception("Can't find timetable with name " + pName + " in TimetableRunner");
			}
			return timetable;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00014BB4 File Offset: 0x00012DB4
		public override string ToString()
		{
			return string.Format("TimetableRunner ({0} timetables)", this._timetables.Values.Count);
		}

		// Token: 0x040000FC RID: 252
		private TableTwo _timetableTable;

		// Token: 0x040000FD RID: 253
		private Dictionary<string, Timetable> _timetables = new Dictionary<string, Timetable>();
	}
}
