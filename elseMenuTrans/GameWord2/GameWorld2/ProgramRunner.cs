using System;
using System.Collections.Generic;
using GameTypes;
using RelayLib;

namespace GameWorld2
{
	// Token: 0x02000018 RID: 24
	public class ProgramRunner
	{
		// Token: 0x060001FA RID: 506 RVA: 0x0000A9F0 File Offset: 0x00008BF0
		public ProgramRunner(RelayTwo pRelay)
		{
			D.isNull(pRelay);
			this._programTable = pRelay.GetTable("Programs");
			this._programsList = InstantiatorTwo.Process<Program>(this._programTable);
			foreach (Program program in this._programsList)
			{
				program.Init(this);
				this._programsDictionary.Add(program.objectId, program);
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000AAB0 File Offset: 0x00008CB0
		public Program CreateProgram(string pName, string pSourceCodeContent, string pNameOfSourceCode)
		{
			Program program = new Program();
			program.CreateNewRelayEntry(this._programTable, typeof(Program).Name);
			program.name = pName;
			program.sourceCodeContent = pSourceCodeContent;
			program.sourceCodeName = pNameOfSourceCode;
			program.Init(this);
			this._newPrograms.Add(program);
			return program;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000AB08 File Offset: 0x00008D08
		public Program CreateProgram(SourceCode pSourceCode)
		{
			return this.CreateProgram(pSourceCode.name, pSourceCode.content, "unknown");
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000AB2C File Offset: 0x00008D2C
		public Program GetProgram(int pObjectId)
		{
			Program program = null;
			this._programsDictionary.TryGetValue(pObjectId, out program);
			if (program != null)
			{
				return program;
			}
			throw new Exception("Can't find program with object id " + pObjectId + " in ProgramRunner");
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000AB6C File Offset: 0x00008D6C
		public Program GetProgramUnsafe(int pObjectId)
		{
			Program program = null;
			if (this._programsDictionary.TryGetValue(pObjectId, out program))
			{
				return program;
			}
			foreach (Program program2 in this._newPrograms)
			{
				if (program2.objectId == pObjectId)
				{
					return program2;
				}
			}
			return null;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000ABFC File Offset: 0x00008DFC
		public void Update(float dt)
		{
			foreach (Program program in this._newPrograms)
			{
				this._programsDictionary.Add(program.objectId, program);
				this._programsList.Add(program);
			}
			this._newPrograms.Clear();
			foreach (Program program2 in this._programsList)
			{
				if (program2.isOn)
				{
					program2.Update(dt);
				}
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000ACE8 File Offset: 0x00008EE8
		public override string ToString()
		{
			return string.Format("ProgramRunner ({0} programs)", this._programsDictionary.Count);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000AD04 File Offset: 0x00008F04
		public Program[] GetAllPrograms()
		{
			return this._programsList.ToArray();
		}

		// Token: 0x04000097 RID: 151
		private TableTwo _programTable;

		// Token: 0x04000098 RID: 152
		private Dictionary<int, Program> _programsDictionary = new Dictionary<int, Program>();

		// Token: 0x04000099 RID: 153
		private List<Program> _programsList;

		// Token: 0x0400009A RID: 154
		private List<Program> _newPrograms = new List<Program>();
	}
}
