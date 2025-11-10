using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000087 RID: 135
	public class VendingMachine : MimanTing
	{
		// Token: 0x0600078D RID: 1933 RVA: 0x000210F8 File Offset: 0x0001F2F8
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "VendingMachine");
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00021118 File Offset: 0x0001F318
		public override void Update(float dt)
		{
			base.UpdateBubbleTimer();
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x00021120 File Offset: 0x0001F320
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[] { base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * -2 };
			}
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002115C File Offset: 0x0001F35C
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x00021164 File Offset: 0x0001F364
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x00021168 File Offset: 0x0001F368
		public override string tooltipName
		{
			get
			{
				return "vending machine";
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00021170 File Offset: 0x0001F370
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x00021180 File Offset: 0x0001F380
		public override string verbDescription
		{
			get
			{
				if (this.dispensedCoke == null)
				{
					return "press button on";
				}
				return "pick up coke from";
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x00021198 File Offset: 0x0001F398
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x000211A8 File Offset: 0x0001F3A8
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

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x000211B8 File Offset: 0x0001F3B8
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					if (this.masterProgramName == "BlankSlate")
					{
						this.masterProgramName = "VendingMachine";
					}
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(VendingMachine)));
				}
				return this._program;
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00021230 File Offset: 0x0001F430
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00021250 File Offset: 0x0001F450
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return false;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00021254 File Offset: 0x0001F454
		[SprakAPI(new string[] { "Get a random number between 0.0 and 1.0" })]
		public float API_Random()
		{
			return Randomizer.GetValue(0f, 1f);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00021268 File Offset: 0x0001F468
		[SprakAPI(new string[] { "Get the name of who is using the vending machine, if any" })]
		public string API_GetUser()
		{
			if (this._user == null)
			{
				return "";
			}
			return this._user.name;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00021288 File Offset: 0x0001F488
		[SprakAPI(new string[] { "Get the name of the vending machine" })]
		public string API_Name()
		{
			return base.name;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00021290 File Offset: 0x0001F490
		[SprakAPI(new string[] { "Get the total time as a float" })]
		public float API_Time()
		{
			return this._worldSettings.totalWorldTime;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x000212A0 File Offset: 0x0001F4A0
		[SprakAPI(new string[] { "Create a drink", "The type of drink", "How much liquid the drink should have" })]
		public void API_CreateDrink(string type, float liquidAmount)
		{
			string text = type.ToLower();
			if (text != null)
			{
				if (VendingMachine.<>f__switch$map4 == null)
				{
					VendingMachine.<>f__switch$map4 = new Dictionary<string, int>(7)
					{
						{ "coke", 0 },
						{ "beer", 1 },
						{ "coffee", 2 },
						{ "margherita", 3 },
						{ "drymartini", 4 },
						{ "bloodymary", 5 },
						{ "longislandicetea", 6 }
					};
				}
				int num;
				if (VendingMachine.<>f__switch$map4.TryGetValue(text, out num))
				{
					string text2;
					string text3;
					string text4;
					switch (num)
					{
					case 0:
						text2 = "Coke";
						text3 = "coke";
						text4 = "Coke";
						break;
					case 1:
						text2 = "Beer";
						text3 = "beer";
						text4 = "FolkBeer";
						break;
					case 2:
						text2 = "CoffeeCup_CoffeeCup";
						text3 = "coffee";
						text4 = "CafeCoffee";
						break;
					case 3:
						text2 = "Margherita_Margherita";
						text3 = "drink";
						text4 = "AlcoholicDrink";
						break;
					case 4:
						text2 = "DryMartini_DryMartini";
						text3 = "drink";
						text4 = "AlcoholicDrink";
						break;
					case 5:
						text2 = "BloodyMary_BloodyMary";
						text3 = "drink";
						text4 = "AlcoholicDrink";
						break;
					case 6:
						text2 = "LongIslandIceTea_LongIslandIceTea";
						text3 = "drink";
						text4 = "AlcoholicDrink";
						break;
					default:
						goto IL_016E;
					}
					int num2 = Behaviour_Sell.CountNrOfTingsWithPrefab(this._tingRunner, text2);
					string text5 = text2 + "_Drink_dispensed_" + num2;
					Ting tingUnsafe = this._tingRunner.GetTingUnsafe(text5);
					if (tingUnsafe != null)
					{
						if (!(tingUnsafe.room.name == "Sebastian_inventory") && !tingUnsafe.isBeingHeld)
						{
							D.Log("VENDING MACHINE: There's already a " + text5 + ", will use that one instead!");
							Drink drink = tingUnsafe as Drink;
							drink.position = base.position;
							drink.masterProgramName = text4;
							drink.liquidType = text3;
							drink.amount = liquidAmount;
							return;
						}
						D.Log("VENDING MACHINE: There's already a " + text5 + " but a character is holding it (or avatar has it)");
						for (int i = 0; i < 9999; i++)
						{
							string text6 = text5 + "_safe_" + i;
							if (!(this._tingRunner.GetTingUnsafe(text6) is MimanTing))
							{
								text5 = text6;
								break;
							}
						}
					}
					Drink drink2 = this._tingRunner.CreateTingAfterUpdate<Drink>(text5, base.position, Direction.DOWN, text2);
					drink2.masterProgramName = text4;
					drink2.liquidType = text3;
					drink2.amount = liquidAmount;
					return;
				}
			}
			IL_016E:
			throw new Error("Can't create drink of type " + type);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00021570 File Offset: 0x0001F770
		public void PushCokeDispenserButton(Character pUser)
		{
			this._user = pUser;
			if (this.dispensedCoke == null)
			{
				this.masterProgram.Start();
			}
			else
			{
				this.Say("Can't dispense coke, tray is full", "");
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x000215B0 File Offset: 0x0001F7B0
		public Drink dispensedCoke
		{
			get
			{
				return base.tile.GetOccupantOfType<Drink>();
			}
		}

		// Token: 0x040001FB RID: 507
		public new static string TABLE_NAME = "Ting_VendingMachine";

		// Token: 0x040001FC RID: 508
		private ValueEntry<string> CELL_programName;

		// Token: 0x040001FD RID: 509
		private Program _program;

		// Token: 0x040001FE RID: 510
		private Character _user;
	}
}
