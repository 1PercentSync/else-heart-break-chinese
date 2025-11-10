using System;
using System.Collections.Generic;
using System.Linq;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000025 RID: 37
	public class Extractor : MimanTing, TingWithButton
	{
		// Token: 0x06000344 RID: 836 RVA: 0x00012838 File Offset: 0x00010A38
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "ExtractorSoftware");
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00012858 File Offset: 0x00010A58
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00012868 File Offset: 0x00010A68
		public override void Update(float dt)
		{
			base.UpdateBubbleTimer();
		}

		// Token: 0x06000347 RID: 839
		[SprakAPI(new string[] { "Get the name of the attached thing获取所连接物品的名称" })]
		public string API_GetName()
		{
			this.API_Sleep(Randomizer.GetValue(1f, 3f));
			if (this._target is Character)
			{
				Character character = this._tingRunner.GetTingUnsafe(this._worldSettings.avatarName) as Character;
				if (character != null)
				{
					character.SetKnowledge(this._target.name);
				}
			}
			return this._target.name;
		}

		// Token: 0x06000348 RID: 840
		[SprakAPI(new string[] { "Get the user defined label of the attached thing获取所连接物品的自定义标签" })]
		public string API_GetLabel()
		{
			this.API_Sleep(Randomizer.GetValue(1f, 2f));
			return this._target.userDefinedLabel;
		}

		// Token: 0x06000349 RID: 841
		[SprakAPI(new string[] { "Set the user defined label of the attached thing设置所连接物品的自定义标签" })]
		public void API_SetLabel(string label)
		{
			this.API_Sleep(Randomizer.GetValue(1f, 2f));
			this._target.userDefinedLabel = label;
		}

		// Token: 0x0600034A RID: 842
		[SprakAPI(new string[] { "Sleepiness of attached character所连接角色的困倦度" })]
		public float API_GetSleepiness()
		{
			this.API_Sleep(Randomizer.GetValue(1f, 3f));
			if (this._target is Character)
			{
				return (this._target as Character).sleepiness;
			}
			throw new Error("Attached thing is not a Character所连接对象不是一个角色");
		}

		// Token: 0x0600034B RID: 843
		[SprakAPI(new string[] { "Speed of attached character所连接角色的速度" })]
		public float API_GetSpeed()
		{
			this.API_Sleep(Randomizer.GetValue(1f, 3f));
			if (this._target is Character)
			{
				return (this._target as Character).walkSpeed;
			}
			throw new Error("Attached thing is not a Character所连接对象不是一个角色");
		}

		// Token: 0x0600034C RID: 844
		[SprakAPI(new string[] { "Charisma of attached character所连接角色的魅力值" })]
		public float API_GetCharisma()
		{
			this.API_Sleep(Randomizer.GetValue(1f, 3f));
			if (this._target is Character)
			{
				return (this._target as Character).charisma;
			}
			throw new Error("Attached thing is not a Character所连接对象不是一个角色");
		}

		// Token: 0x0600034D RID: 845
		[SprakAPI(new string[] { "Get the connections of the attached thing获取所连接物品的连接" })]
		public object[] API_GetConnections()
		{
			this.API_Sleep(Randomizer.GetValue(2f, 3f));
			return this._target.connectedTings.Select((MimanTing t) => t.name).ToArray<string>();
		}

		// Token: 0x0600034E RID: 846
		[SprakAPI(new string[] { "Say something说点什么", "text文本" })]
		public void API_Say(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x0600034F RID: 847
		[SprakAPI(new string[] { "Copy a piece of text to the clipboard复制一段文本到剪贴板", "text" })]
		public void API_CopyToClipboard(string text)
		{
			if (this._worldSettings.onCopyToClipboard != null)
			{
				this._worldSettings.onCopyToClipboard(text);
				return;
			}
			D.Log("copyToClipboard is null");
		}

		// Token: 0x06000350 RID: 848
		[SprakAPI(new string[] { "Pause the master program暂停主程序", "number of seconds to pause for暂停的秒数" })]
		public void API_Sleep(float seconds)
		{
			this.masterProgram.sleepTimer = seconds;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00012AD8 File Offset: 0x00010CD8
		public void Attach(Ting pTarget)
		{
			this._target = pTarget as MimanTing;
			this.masterProgram.Start();
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00012AF4 File Offset: 0x00010CF4
		public void PushButton(Ting pUser)
		{
			base.dialogueLine = "";
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00012B04 File Offset: 0x00010D04
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return true;
		}

		// Token: 0x06000354 RID: 852
		public override string UseTingOnTingDescription(Ting pOtherTing)
		{
			return "Extract from 提取自" + pOtherTing.tooltipName;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00012B1C File Offset: 0x00010D1C
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000356 RID: 854
		public override string verbDescription
		{
			get
			{
				return "reset重置";
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000357 RID: 855
		public override string tooltipName
		{
			get
			{
				return "extractor提取器";
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00012B30 File Offset: 0x00010D30
		// (set) Token: 0x06000359 RID: 857 RVA: 0x00012B40 File Offset: 0x00010D40
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

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00012B50 File Offset: 0x00010D50
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Extractor)));
				}
				return this._program;
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00012BA8 File Offset: 0x00010DA8
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x040000D1 RID: 209
		public new static string TABLE_NAME = "Ting_Extractors";

		// Token: 0x040000D2 RID: 210
		private ValueEntry<string> CELL_programName;

		// Token: 0x040000D3 RID: 211
		private Program _program;

		// Token: 0x040000D4 RID: 212
		private MimanTing _target;
	}
}
