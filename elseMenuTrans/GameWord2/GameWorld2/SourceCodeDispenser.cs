using System;
using System.Collections.Generic;
using System.IO;
using RelayLib;

namespace GameWorld2
{
	// Token: 0x02000019 RID: 25
	public class SourceCodeDispenser
	{
		// Token: 0x06000202 RID: 514 RVA: 0x0000AD14 File Offset: 0x00008F14
		public SourceCodeDispenser(RelayTwo pRelay)
		{
			this._sourceCodeTable = pRelay.GetTable("SourceCodes");
			this._sourceCodes = InstantiatorTwo.Process<SourceCode>(this._sourceCodeTable);
			foreach (SourceCode sourceCode in this._sourceCodes)
			{
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000ADA8 File Offset: 0x00008FA8
		public SourceCode CreateSourceCodeFromString(string pName, string pContent)
		{
			SourceCode sourceCode = new SourceCode();
			sourceCode.CreateNewRelayEntry(this._sourceCodeTable, typeof(SourceCode).Name);
			sourceCode.content = pContent;
			sourceCode.name = pName;
			this._sourceCodes.Add(sourceCode);
			return sourceCode;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000ADF4 File Offset: 0x00008FF4
		public void LoadSourceCode(string pFilePath)
		{
			string nameFromFilepath = FileHelper.GetNameFromFilepath(pFilePath);
			using (StreamReader streamReader = File.OpenText(pFilePath))
			{
				string text = streamReader.ReadToEnd();
				this.CreateSourceCodeFromString(nameFromFilepath, text);
				streamReader.Close();
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000AE54 File Offset: 0x00009054
		public SourceCode GetSourceCode(string pName)
		{
			SourceCode sourceCode = this._sourceCodes.Find((SourceCode o) => o.name == pName);
			if (sourceCode != null)
			{
				return sourceCode;
			}
			throw new Exception("Can't find SourceCode with name '" + pName + "' in Source Code Dispenser");
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000AEA8 File Offset: 0x000090A8
		public override string ToString()
		{
			return string.Format("SourceCodeDispenser ({0} source codes)", this._sourceCodes.Count);
		}

		// Token: 0x0400009B RID: 155
		private TableTwo _sourceCodeTable;

		// Token: 0x0400009C RID: 156
		private List<SourceCode> _sourceCodes = new List<SourceCode>();
	}
}
