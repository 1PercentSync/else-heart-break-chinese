using System;
using System.Collections.Generic;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000026 RID: 38
	public class Radio : MimanTing, TingWithButton
	{
		// Token: 0x0600035F RID: 863 RVA: 0x00012BE4 File Offset: 0x00010DE4
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "RadiOS");
			this.CELL_channel = base.EnsureCell<int>("channel", 1);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00012C20 File Offset: 0x00010E20
		public override void Update(float dt)
		{
			if (base.isPlaying)
			{
				base.audioTime += dt;
				if (base.audioTime > base.audioTotalLength)
				{
					base.audioTime = 0f;
					base.PlaySound(base.soundName);
				}
			}
			base.UpdateBubbleTimer();
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00012C74 File Offset: 0x00010E74
		public override bool autoUnregisterFromUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00012C78 File Offset: 0x00010E78
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00012C88 File Offset: 0x00010E88
		[SprakAPI(new string[] { "Print", "text" })]
		public void API_Print(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00012C98 File Offset: 0x00010E98
		[SprakAPI(new string[] { "Make a blip sound" })]
		public void API_Blip()
		{
			base.PlaySound("Blip");
			base.audioLoop = true;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00012CAC File Offset: 0x00010EAC
		[SprakAPI(new string[] { "Get the nr of the current channel" })]
		public int API_GetChannel()
		{
			return this.channel;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00012CB4 File Offset: 0x00010EB4
		[SprakAPI(new string[] { "Set the channel" })]
		public void API_SetChannel(float newChannel)
		{
			this.channel = (int)newChannel;
			base.audioLoop = true;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00012CC8 File Offset: 0x00010EC8
		[SprakAPI(new string[] { "" })]
		public void API_TurnOffSound()
		{
			base.isPlaying = false;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00012CD4 File Offset: 0x00010ED4
		[SprakAPI(new string[] { "" })]
		public void API_TurnOnSound()
		{
			base.isPlaying = true;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00012CE0 File Offset: 0x00010EE0
		public void PushButton(Ting pUser)
		{
			this.masterProgram.Start();
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00012CF0 File Offset: 0x00010EF0
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is SendPipe || pTingToInteractWith is Stove;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00012D0C File Offset: 0x00010F0C
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00012D10 File Offset: 0x00010F10
		public override string verbDescription
		{
			get
			{
				return "turn wheel";
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00012D18 File Offset: 0x00010F18
		public override string tooltipName
		{
			get
			{
				return "radio";
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00012D20 File Offset: 0x00010F20
		// (set) Token: 0x0600036F RID: 879 RVA: 0x00012D30 File Offset: 0x00010F30
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00012D40 File Offset: 0x00010F40
		// (set) Token: 0x06000371 RID: 881 RVA: 0x00012D50 File Offset: 0x00010F50
		[EditableInEditor]
		public int channel
		{
			get
			{
				return this.CELL_channel.data;
			}
			set
			{
				this.CELL_channel.data = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00012D60 File Offset: 0x00010F60
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Radio)));
					list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ConnectionAPI(this, this._tingRunner, this.masterProgram), typeof(ConnectionAPI)));
					this._program.FunctionDefinitions = list;
				}
				return this._program;
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00012DE0 File Offset: 0x00010FE0
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x040000D6 RID: 214
		public new static string TABLE_NAME = "Ting_Radios";

		// Token: 0x040000D7 RID: 215
		private ValueEntry<string> CELL_programName;

		// Token: 0x040000D8 RID: 216
		private ValueEntry<int> CELL_channel;

		// Token: 0x040000D9 RID: 217
		private Program _program;
	}
}
