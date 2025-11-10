using System;
using System.Collections.Generic;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000059 RID: 89
	public class FloppyAPI
	{
		// Token: 0x06000574 RID: 1396 RVA: 0x0001AECC File Offset: 0x000190CC
		public FloppyAPI(Computer pComputer, TingRunner pTingRunner)
		{
			this._computer = pComputer;
		}

		// Token: 0x06000575 RID: 1397
		[SprakAPI(new string[] { "Is there a floppy in the drive?驱动器中是否有软盘？" })]
		public bool API_HasFloppy()
		{
			return this._computer.floppyInDrive != null;
		}

		// Token: 0x06000576 RID: 1398
		[SprakAPI(new string[] { "Load data from the floppy, split by lines从软盘加载数据，按行分割" })]
		public SortedDictionary<KeyWrapper, object> API_LoadData()
		{
			Floppy floppyInDrive = this._computer.floppyInDrive;
			if (floppyInDrive == null)
			{
				throw new Error("No floppy in drive, can't load data驱动器中没有软盘，无法加载数据");
			}
			SortedDictionary<KeyWrapper, object> sortedDictionary = new SortedDictionary<KeyWrapper, object>();
			int num = 0;
			foreach (string text in floppyInDrive.masterProgram.sourceCodeContent.Split(new char[] { '\n' }, 9999))
			{
				sortedDictionary.Add(new KeyWrapper((float)num++), text);
			}
			return sortedDictionary;
		}

		// Token: 0x06000577 RID: 1399
		[SprakAPI(new string[] { "Clear all data on the floppy清除软盘上的所有数据" })]
		public void API_ClearData()
		{
			Floppy floppyInDrive = this._computer.floppyInDrive;
			if (floppyInDrive == null)
			{
				throw new Error("No floppy in drive, can't clear data驱动器中没有软盘, 无法清除数据");
			}
			floppyInDrive.masterProgram.sourceCodeContent = "";
		}

		// Token: 0x06000578 RID: 1400
		[SprakAPI(new string[] { "Save data to the floppy by appending a line at the end在末尾追加一空行，再保存数据到软盘" })]
		public void API_SaveData(string data)
		{
			Floppy floppyInDrive = this._computer.floppyInDrive;
			if (floppyInDrive == null)
			{
				throw new Error("No floppy in drive, can't save data驱动器中没有软盘, 无法保存数据");
			}
			Program masterProgram = floppyInDrive.masterProgram;
			masterProgram.sourceCodeContent = masterProgram.sourceCodeContent + "\n" + data;
		}

		// Token: 0x06000579 RID: 1401
		[SprakAPI(new string[] { "Restart the computer but run the code on the floppy instead重启电脑并改为从软盘启动" })]
		public void API_BootFromFloppy()
		{
			Floppy floppyInDrive = this._computer.floppyInDrive;
			if (floppyInDrive == null)
			{
				throw new Error("No floppy in drive, can't boot from it驱动器中没有软盘，无法从中启动");
			}
			this._computer.masterProgram.StopAndReset();
			this._computer.floppyBootProgram.sourceCodeContent = floppyInDrive.masterProgram.sourceCodeContent;
			this._computer.floppyBootProgram.Start();
			this._computer.floppyBootProgram.maxExecutionTime = 60f;
		}

		// Token: 0x04000170 RID: 368
		private Computer _computer;
	}
}
