using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GameTypes;
using GrimmLib;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000003 RID: 3
	public class InitialSaveFileCreator
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002510 File Offset: 0x00000710
		public RelayTwo CreateEmptyRelay()
		{
			RelayTwo relayTwo = new RelayTwo();
			relayTwo.CreateTable(Ting.TABLE_NAME);
			relayTwo.CreateTable("Rooms");
			relayTwo.CreateTable("Dialogues");
			relayTwo.CreateTable("SourceCodes");
			relayTwo.CreateTable("Programs");
			relayTwo.CreateTable("WorldSettings");
			relayTwo.CreateTable("Timetables");
			return relayTwo;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002578 File Offset: 0x00000778
		public RelayTwo CreateRelay(string pInputDirectory)
		{
			return this.CreateRelay(pInputDirectory, false);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002584 File Offset: 0x00000784
		public RelayTwo CreateRelay(string pInputDirectory, bool pOnlyLoadTingsAndRooms)
		{
			this._relay = this.CreateEmptyRelay();
			DialogueRunner dialogueRunner = new DialogueRunner(this._relay, Language.SWEDISH);
			this._dialogueScriptLoader = new DialogueScriptLoader(dialogueRunner);
			this._sourceCodeDispenser = new SourceCodeDispenser(this._relay);
			this._timetableRunner = new TimetableRunner(this._relay);
			foreach (string text in this.GetFilesRecursively(pInputDirectory))
			{
				this.FoundFile(text, pOnlyLoadTingsAndRooms);
			}
			return this._relay;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002608 File Offset: 0x00000808
		public IEnumerable<float> LoadFromFile(string pFilename)
		{
			this._relay = new RelayTwo();
			return this._relay.Load(pFilename);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002624 File Offset: 0x00000824
		public IEnumerable<float> LoadRelayFromDirectory(string pInputDirectory)
		{
			return this.LoadRelayFromDirectory(pInputDirectory, false);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002630 File Offset: 0x00000830
		public IEnumerable<float> LoadRelayFromDirectory(string pInputDirectory, bool pOnlyLoadTingsAndRooms)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			this._relay = this.CreateEmptyRelay();
			DialogueRunner dialogueRunner = new DialogueRunner(this._relay, Language.SWEDISH);
			this._dialogueScriptLoader = new DialogueScriptLoader(dialogueRunner);
			this._sourceCodeDispenser = new SourceCodeDispenser(this._relay);
			this._timetableRunner = new TimetableRunner(this._relay);
			string[] files = this.GetFilesRecursively(pInputDirectory);
			for (int i = 0; i < files.Length; i++)
			{
				this.FoundFile(files[i], pOnlyLoadTingsAndRooms);
				yield return (float)i / (float)files.Length;
			}
			sw.Stop();
			this.logger.Log(string.Concat(new object[]
			{
				"Loading relay from directory ",
				pInputDirectory,
				" took ",
				sw.Elapsed.TotalSeconds,
				" s."
			}));
			yield break;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002670 File Offset: 0x00000870
		public RelayTwo GetLoadedRelay()
		{
			return this._relay;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002678 File Offset: 0x00000878
		public void CreateSaveFile(string pInputDirectory, string pOutputFilepath, bool pOnlyLoadTingsAndRooms)
		{
			this.CreateRelay(pInputDirectory, pOnlyLoadTingsAndRooms);
			this._relay.SaveAll(pOutputFilepath);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002690 File Offset: 0x00000890
		public void CreateSaveFile(string pInputDirectory, string pOutputFilepath)
		{
			this.CreateSaveFile(pInputDirectory, pOutputFilepath, false);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000269C File Offset: 0x0000089C
		private string[] GetFilesRecursively(string pPath)
		{
			List<string> list = new List<string>();
			string[] directories = Directory.GetDirectories(pPath);
			string[] files = Directory.GetFiles(pPath);
			foreach (string text in files)
			{
				list.Add(text);
			}
			foreach (string text2 in directories)
			{
				list.AddRange(this.GetFilesRecursively(text2));
			}
			return list.ToArray();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000271C File Offset: 0x0000091C
		private void FoundFile(string pFilepath, bool pOnlyLoadTingsAndRooms)
		{
			if (pFilepath.Contains(".svn"))
			{
				return;
			}
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			if (pFilepath.EndsWith(".dia") && !pOnlyLoadTingsAndRooms)
			{
				this._dialogueScriptLoader.LoadDialogueNodesFromFile(pFilepath);
			}
			else if (pFilepath.EndsWith(".tings"))
			{
				this._relay.AppendTables(pFilepath);
			}
			else if (pFilepath.EndsWith(".json"))
			{
				this._relay.MergeWith(new RelayTwo(pFilepath));
			}
			else if (pFilepath.EndsWith(".sprak") && !pOnlyLoadTingsAndRooms)
			{
				this._sourceCodeDispenser.LoadSourceCode(pFilepath);
			}
			else if (pFilepath.EndsWith(".ttt") && !pOnlyLoadTingsAndRooms)
			{
				this._timetableRunner.LoadTimetableFromFile(pFilepath);
			}
			stopwatch.Stop();
			if (stopwatch.Elapsed.TotalSeconds > 0.10000000149011612)
			{
			}
		}

		// Token: 0x0400000D RID: 13
		public Logger logger = new Logger();

		// Token: 0x0400000E RID: 14
		private RelayTwo _relay;

		// Token: 0x0400000F RID: 15
		private DialogueScriptLoader _dialogueScriptLoader;

		// Token: 0x04000010 RID: 16
		private SourceCodeDispenser _sourceCodeDispenser;

		// Token: 0x04000011 RID: 17
		private TimetableRunner _timetableRunner;
	}
}
