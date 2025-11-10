using System;
using System.Collections.Generic;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200005F RID: 95
	public class CreditCard : MimanTing, TingWithButton
	{
		// Token: 0x0600059A RID: 1434 RVA: 0x0001B2F8 File Offset: 0x000194F8
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_nameOfOwner = base.EnsureCell<string>("nameOfOwner", "");
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "PersonalCreditCard");
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001B338 File Offset: 0x00019538
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001B348 File Offset: 0x00019548
		public override void Update(float dt)
		{
			base.UpdateBubbleTimer();
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x0001B350 File Offset: 0x00019550
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x0001B360 File Offset: 0x00019560
		[EditableInEditor]
		public string nameOfOwner
		{
			get
			{
				return this.CELL_nameOfOwner.data;
			}
			set
			{
				this.CELL_nameOfOwner.data = value;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0001B370 File Offset: 0x00019570
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0001B374 File Offset: 0x00019574
		public override string tooltipName
		{
			get
			{
				return "credit card";
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0001B37C File Offset: 0x0001957C
		public override string verbDescription
		{
			get
			{
				return "check balance";
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0001B384 File Offset: 0x00019584
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x0001B394 File Offset: 0x00019594
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

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0001B3A4 File Offset: 0x000195A4
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(CreditCard)));
					list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ConnectionAPI(this, this._tingRunner, this.masterProgram), typeof(ConnectionAPI)));
					this._program.FunctionDefinitions = list;
				}
				return this._program;
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001B424 File Offset: 0x00019624
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001B444 File Offset: 0x00019644
		public void PushButton(Ting pUser)
		{
			this.logger.Log(base.name + " is getting used in hand");
			Character character = pUser as Character;
			if (character != null && character.creditCardUsageAmount != 0f)
			{
				this.RunMakeTransactionFunction(character.creditCardUsageAmount);
				character.creditCardUsageAmount = 0f;
			}
			else
			{
				this.masterProgram.maxExecutionTime = 10f;
				this.masterProgram.StartAtFunction("CheckBalance", new object[0], null);
			}
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001B4CC File Offset: 0x000196CC
		public override void InteractWith(Ting pTingToInteractWith)
		{
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001B4D0 File Offset: 0x000196D0
		public void RunMakeTransactionFunction(float transactionAmount)
		{
			this.masterProgram.maxExecutionTime = 10f;
			this.masterProgram.StartAtFunctionWithMockReceiver("MakeTransaction", new object[] { transactionAmount }, null);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001B510 File Offset: 0x00019710
		[SprakAPI(new string[] { "Log" })]
		public void API_Log(string text)
		{
			this.logger.Log("LOG: " + text);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001B528 File Offset: 0x00019728
		[SprakAPI(new string[] { "Print" })]
		public void API_Print(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001B538 File Offset: 0x00019738
		[SprakAPI(new string[] { "Get the name of the owner of the card" })]
		public string API_GetNameOfCardOwner()
		{
			return this.CELL_nameOfOwner.data;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001B548 File Offset: 0x00019748
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return false;
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001B54C File Offset: 0x0001974C
		public override int securityLevel
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0400017A RID: 378
		private ValueEntry<string> CELL_nameOfOwner;

		// Token: 0x0400017B RID: 379
		private ValueEntry<string> CELL_programName;

		// Token: 0x0400017C RID: 380
		private Program _program;
	}
}
