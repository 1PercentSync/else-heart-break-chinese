using System;
using System.Collections.Generic;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200000C RID: 12
	public class Drink : MimanTing
	{
		// Token: 0x0600010E RID: 270 RVA: 0x00007C2C File Offset: 0x00005E2C
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_ammount = base.EnsureCell<float>("ammount", 100f);
			this.CELL_liquidType = base.EnsureCell<string>("liquidType", "beer啤酒");
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "FolkBeer");
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00007C84 File Offset: 0x00005E84
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00007C94 File Offset: 0x00005E94
		[EditableInEditor]
		public float amount
		{
			get
			{
				return this.CELL_ammount.data;
			}
			set
			{
				this.CELL_ammount.data = value;
				if (this.amount < 0f)
				{
					this.amount = 0f;
				}
				else if (this.amount > 99999f || float.IsInfinity(this.amount))
				{
					this.amount = 99999f;
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00007CF8 File Offset: 0x00005EF8
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00007D08 File Offset: 0x00005F08
		[EditableInEditor]
		public string liquidType
		{
			get
			{
				return this.CELL_liquidType.data;
			}
			set
			{
				this.CELL_liquidType.data = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00007D18 File Offset: 0x00005F18
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00007D1C File Offset: 0x00005F1C
		public override string tooltipName
		{
			get
			{
				return this.liquidType + ((this.amount > 0f) ? (" (" + this.amount + "%)") : " (empty空)");
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00007D68 File Offset: 0x00005F68
		public override string verbDescription
		{
			get
			{
				return "drink喝";
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00007D70 File Offset: 0x00005F70
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00007D80 File Offset: 0x00005F80
		public override void FixBeforeSaving()
		{
			if (base.name.ToLower().Contains("coffee") || base.name.ToLower().Contains("cup"))
			{
				this.masterProgramName = "Coffee";
				this.liquidType = "coffee咖啡";
			}
			else if (base.name.ToLower().Contains("soda"))
			{
				this.masterProgramName = "WellspringSoda";
				this.liquidType = "Wellspring soda汽水";
			}
			else if (base.name.ToLower().Contains("cola") || base.name.ToLower().Contains("coke"))
			{
				this.masterProgramName = "WellspringSoda";
				this.liquidType = "cola可乐";
			}
			else if (base.name.ToLower().Contains("beer"))
			{
				this.masterProgramName = "FolkBeer";
				this.liquidType = "beer啤酒";
			}
			else if (base.name.ToLower().Contains("booze"))
			{
				this.masterProgramName = "AlcoholicDrink";
				this.liquidType = "booze 烈酒";
			}
			else if (base.name.ToLower().Contains("margherita") || base.name.ToLower().Contains("longisland") || base.name.ToLower().Contains("bloodymary") || base.name.ToLower().Contains("drymartini"))
			{
				this.masterProgramName = "AlcoholicDrink";
				this.liquidType = "drink鸡尾酒";
			}
			else if (base.name.ToLower().Contains("water") || base.name.ToLower().Contains("glass"))
			{
				this.masterProgramName = "Water";
				this.liquidType = "water水";
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007F98 File Offset: 0x00006198
		private float CalculateEffect(float d)
		{
			if (d > 0f && d > this.amount)
			{
				return this.amount;
			}
			if (d < 0f && d < -this.amount)
			{
				return -this.amount;
			}
			return d;
		}

		// Token: 0x06000119 RID: 281
		[SprakAPI(new string[] { "Change the speed of the drinker改变饮用者的速度", "amount数量" })]
		public void API_Speed(float d)
		{
			if (this._drinker == null)
			{
				return;
			}
			float num = this.CalculateEffect(d);
			this._drinker.walkSpeed += num / 25f;
			if (this._drinker.walkSpeed < 2f)
			{
				this._drinker.walkSpeed = 2f;
			}
			else if (this._drinker.walkSpeed > 8f)
			{
				this._drinker.walkSpeed = 8f;
			}
			this.amount -= Math.Abs(num);
		}

		// Token: 0x0600011A RID: 282
		[SprakAPI(new string[] { "Change the charisma of the drinker改变饮用者的魅力", "amount数量" })]
		public void API_Charisma(float d)
		{
			if (this._drinker == null)
			{
				return;
			}
			float num = this.CalculateEffect(d);
			this._drinker.charisma += num;
			this.amount -= Math.Abs(num);
		}

		// Token: 0x0600011B RID: 283
		[SprakAPI(new string[] { "Change the smelliness of the drinker改变饮用者的体味", "amount" })]
		public void API_Smelliness(float d)
		{
			if (this._drinker == null)
			{
				return;
			}
			float num = this.CalculateEffect(d);
			this._drinker.smelliness += num;
			this.amount -= Math.Abs(num);
		}

		// Token: 0x0600011C RID: 284
		[SprakAPI(new string[] { "Change the sleepiness of the drinker改变饮用者的困倦度", "amount" })]
		public void API_Sleepiness(float d)
		{
			if (this._drinker == null)
			{
				return;
			}
			float num = this.CalculateEffect(d);
			this._drinker.sleepiness += num;
			this.amount -= Math.Abs(num);
		}

		// Token: 0x0600011D RID: 285
		[SprakAPI(new string[] { "Change the drunkenness of the drinker改变饮用者的醉酒度", "amount" })]
		public void API_Drunkenness(float d)
		{
			if (this._drinker == null)
			{
				return;
			}
			float num = this.CalculateEffect(d);
			this._drinker.drunkenness += num;
			this.amount -= Math.Abs(num);
		}

		// Token: 0x0600011E RID: 286
		[SprakAPI(new string[] { "Just lower the amount of liquid仅减少液体量", "x" })]
		public void API_Drink(float d)
		{
			if (this._drinker == null)
			{
				return;
			}
			this.amount -= Math.Max(0f, d);
		}

		// Token: 0x0600011F RID: 287
		[SprakAPI(new string[] { "Undocumented effect未记录的效果", "amount" })]
		public void API_Corruption(float d)
		{
			if (this._drinker == null)
			{
				return;
			}
			float num = this.CalculateEffect(d);
			this._drinker.corruption += num;
			this.amount -= Math.Abs(num);
		}

		// Token: 0x06000120 RID: 288
		[SprakAPI(new string[] { "Test if the drinker has a certain name测试饮用者是否为特定名称" })]
		public bool API_IsUser(string name)
		{
			if (this._drinker != null)
			{
				return this._drinker.name.ToLower() == name.ToLower();
			}
			return name == "";
		}

		// Token: 0x06000121 RID: 289
		[SprakAPI(new string[] { "Get the name of the drinker获取饮用者的名称" })]
		public string API_GetUser()
		{
			if (this._drinker != null)
			{
				return this._drinker.name;
			}
			return "";
		}

		// Token: 0x06000122 RID: 290
		[SprakAPI(new string[] { "Get the room of the drink获取饮料所在的房间" })]
		public string API_GetRoom()
		{
			return base.room.name;
		}

		// Token: 0x06000123 RID: 291
		[SprakAPI(new string[] { "Say something说点什么" })]
		public void API_Say(string something)
		{
			this.Say(something, "");
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000829C File Offset: 0x0000649C
		public void DrinkFrom(Character pDrinker)
		{
			this._drinker = pDrinker;
			this.masterProgram.maxExecutionTime = 10f;
			this.masterProgram.Start();
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000082CC File Offset: 0x000064CC
		// (set) Token: 0x06000126 RID: 294 RVA: 0x000082DC File Offset: 0x000064DC
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000082EC File Offset: 0x000064EC
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Drink)));
					list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ConnectionAPI(this, this._tingRunner, this.masterProgram), typeof(ConnectionAPI)));
					this._program.FunctionDefinitions = list;
				}
				return this._program;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000836C File Offset: 0x0000656C
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
			this._drinker = null;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000083B4 File Offset: 0x000065B4
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Sink || pTingToInteractWith is Locker || pTingToInteractWith is TrashCan || pTingToInteractWith is SendPipe || pTingToInteractWith is Stove;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000083FC File Offset: 0x000065FC
		public override void InteractWith(Ting pTingToInteractWith)
		{
			if (pTingToInteractWith is Sink)
			{
				this.amount = 100f;
			}
		}

		// Token: 0x0400005B RID: 91
		public new static string TABLE_NAME = "Ting_Drinks";

		// Token: 0x0400005C RID: 92
		private ValueEntry<float> CELL_ammount;

		// Token: 0x0400005D RID: 93
		private ValueEntry<string> CELL_liquidType;

		// Token: 0x0400005E RID: 94
		private ValueEntry<string> CELL_programName;

		// Token: 0x0400005F RID: 95
		private Program _program;

		// Token: 0x04000060 RID: 96
		private Character _drinker;
	}
}
