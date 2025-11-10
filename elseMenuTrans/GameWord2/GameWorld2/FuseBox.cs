using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000028 RID: 40
	public class FuseBox : MimanTing
	{
		// Token: 0x060003A6 RID: 934 RVA: 0x00013458 File Offset: 0x00011658
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "Electricity");
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00013478 File Offset: 0x00011678
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003A8 RID: 936
		public override string verbDescription
		{
			get
			{
				return "inspect检查";
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060003A9 RID: 937
		public override string tooltipName
		{
			get
			{
				return "fuse box保险丝盒";
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0001348C File Offset: 0x0001168C
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[]
				{
					base.localPoint + IntPoint.Up * 2,
					base.localPoint + IntPoint.Left * 2,
					base.localPoint + IntPoint.Right * 2,
					base.localPoint + IntPoint.Down * 2
				};
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00013528 File Offset: 0x00011728
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00013530 File Offset: 0x00011730
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00013540 File Offset: 0x00011740
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00013550 File Offset: 0x00011750
		[EditableInEditor]
		public string masterProgramName
		{
			get
			{
				return this.CELL_programName.data;
			}
			set
			{
				this.CELL_programName.data = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00013560 File Offset: 0x00011760
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(FuseBox)));
					list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ConnectionAPI(this, this._tingRunner, this.masterProgram), typeof(ConnectionAPI)));
					this._program.FunctionDefinitions = list;
					this._program.maxExecutionTime = 5f;
				}
				return this._program;
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x000135F0 File Offset: 0x000117F0
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
			else
			{
				this.masterProgram.nameOfOwner = base.name;
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00013630 File Offset: 0x00011830
		public void BeInspected(Character pCharacter)
		{
			this._user = pCharacter;
			this.masterProgram.Start();
		}

		// Token: 0x060003B2 RID: 946
		[SprakAPI(new string[] { "Use with caution请谨慎使用" })]
		public void API_Slurp()
		{
			if (this._user != null)
			{
				this._user.SlurpIntoInternet(this);
			}
		}

		// Token: 0x060003B3 RID: 947
		[SprakAPI(new string[] { "Get the name of the fuse box获取保险丝盒的名称" })]
		public string API_GetName()
		{
			return base.name;
		}

		// Token: 0x060003B4 RID: 948
		[SprakAPI(new string[] { "Say something说点什么" })]
		public void API_Say(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x060003B5 RID: 949
		[SprakAPI(new string[] { "Log something记录日志" })]
		public void API_Log(string text)
		{
			D.Log(text);
		}

		// Token: 0x040000E5 RID: 229
		public new static string TABLE_NAME = "Tings_FuseBoxes";

		// Token: 0x040000E6 RID: 230
		private ValueEntry<string> CELL_programName;

		// Token: 0x040000E7 RID: 231
		private Program _program;

		// Token: 0x040000E8 RID: 232
		private Character _user;
	}
}
