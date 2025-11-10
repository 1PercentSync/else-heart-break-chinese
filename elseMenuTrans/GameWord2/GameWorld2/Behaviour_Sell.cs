using System;
using System.Collections.Generic;
using GameTypes;
using GrimmLib;
using Pathfinding;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200003A RID: 58
	public class Behaviour_Sell : TimetableBehaviour
	{
		// Token: 0x06000454 RID: 1108 RVA: 0x00015CC4 File Offset: 0x00013EC4
		public Behaviour_Sell(string[] pArgs)
		{
			this._sellPointName = pArgs[1];
			this._collectEmptyDrinks = pArgs[2] == "CollectEmptyDrinks";
			for (int i = 3; i < pArgs.Length; i++)
			{
				this._tingNamesToSell.Add(pArgs[i]);
			}
		}

		// Token: 0x06000456 RID: 1110
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			if (pCharacter.busy)
			{
				return 1f;
			}
			if (pCharacter.talking)
			{
				return 1f;
			}
			if (this._sellPoint == null)
			{
				this._sellPoint = pTingRunner.GetTing(this._sellPointName);
				this._roomName = this._sellPoint.room.name;
			}
			bool flag5 = pCharacter.room.name == this._roomName;
			bool flag2 = pCharacter.handItem is Drink;
			bool flag3 = flag2;
			bool flag4 = pCharacter.actionName == "";
			if (pCharacter.position == this._sellPoint.position)
			{
				pCharacter.direction = this._sellPoint.direction;
			}
			if (flag5)
			{
				if (pCharacter.timetableMemory == "")
				{
					if (flag2)
					{
						pCharacter.logger.Log(pCharacter.name + " will put away drink");
						pCharacter.PutHandItemIntoInventory();
					}
					else if (flag4 && this._collectEmptyDrinks)
					{
						Drink leftOverDrink = this.GetLeftOverDrink(pCharacter, pTingRunner);
						if (leftOverDrink != null)
						{
							if (Behaviour_Sell._solver.FindPath(pCharacter.tile, leftOverDrink.tile, pRoomRunner, true).status == PathStatus.FOUND_GOAL)
							{
								pCharacter.WalkToTingAndInteract(leftOverDrink);
								pCharacter.logger.Log(pCharacter.name + " will walk to left over drink " + leftOverDrink.name);
							}
							else
							{
								pCharacter.logger.Log(pCharacter.name + " can't pathfind to " + leftOverDrink.name + " will delete it...");
								pTingRunner.RemoveTingAfterUpdate(leftOverDrink.name);
							}
						}
						else
						{
							pCharacter.WalkTo(this._sellPoint.position);
							pCharacter.logger.Log(pCharacter.name + " will walk back to sell point");
						}
					}
					else if (pCharacter.finalTargetTing == null && flag4)
					{
						pCharacter.logger.Log(pCharacter.name + " will walk to sell point");
						pCharacter.WalkTo(this._sellPoint.position);
					}
				}
				else
				{
					pCharacter.logger.Log(pCharacter.name + " has a customer");
					if (pCharacter.conversationTarget != null)
					{
						if (flag3)
						{
							if (flag2)
							{
								(pCharacter.handItem as Drink).amount = 100f;
							}
							pCharacter.GiveHandItemToPerson();
							pCharacter.Say("Varsågod!给你！", "OrderingDrinks");
							pCharacter.timetableMemory = "";
							pCharacter.logger.Log(pCharacter.name + " gave thing to sell");
						}
						else
						{
							string text = this._tingNamesToSell[0];
							pCharacter.logger.Log(pCharacter.name + " will create " + text + " to sell");
							pCharacter.handItem = Behaviour_Sell.CreateTingToSell(pCharacter, pTingRunner, text, pWorldSettings);
						}
					}
					else
					{
						Character character = pTingRunner.GetTing(pCharacter.timetableMemory) as Character;
						D.isNull(character);
						pCharacter.WalkToTingAndInteract(character);
						pCharacter.logger.Log(pCharacter.name + " will interact with customer");
					}
				}
				return 2f;
			}
			InteractionHelper.GoToRoom(pCharacter, pRoomRunner, this._roomName, pTingRunner);
			pCharacter.timetableMemory = "";
			return 3f;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x000160A0 File Offset: 0x000142A0
		public Drink GetLeftOverDrink(Character pCharacter, TingRunner pTingRunner)
		{
			foreach (Drink drink in pTingRunner.GetTingsOfTypeInRoom<Drink>(pCharacter.room.name))
			{
				if (!drink.isBeingHeld && drink.amount < 100f)
				{
					return drink;
				}
			}
			return null;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x000160F8 File Offset: 0x000142F8
		public static int CountNrOfTingsWithPrefab(TingRunner pTingRunner, string pPrefabName)
		{
			IEnumerable<Ting> tings = pTingRunner.GetTings();
			int num = 0;
			foreach (Ting ting in tings)
			{
				if (ting.prefab == pPrefabName)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00016170 File Offset: 0x00014370
		public static MimanTing CreateTingToSell(Character pCharacter, TingRunner pTingRunner, string pPrefabName, WorldSettings pWorldSettings)
		{
			string text = pPrefabName + "_sale_" + pWorldSettings.dynamicallyCreatedTingsCount++;
			if (pWorldSettings.dynamicallyCreatedTingsCount > 20)
			{
				pWorldSettings.dynamicallyCreatedTingsCount = 0;
			}
			MimanTing mimanTing = pTingRunner.GetTingUnsafe(text) as MimanTing;
			if (mimanTing != null)
			{
				if (!(mimanTing.room.name == "Sebastian_inventory") && !mimanTing.isBeingHeld)
				{
					D.Log("There's already a " + text + ", will use that one instead!");
					(mimanTing as Drink).amount = 100f;
					mimanTing.position = new WorldCoordinate(pCharacter.inventoryRoomName, IntPoint.Zero);
					return mimanTing;
				}
				D.Log("There's already a " + text + " but a character is holding it (or avatar has it)");
				for (int i = 0; i < 9999; i++)
				{
					string text2 = text + "_safe_" + i;
					if (!(pTingRunner.GetTingUnsafe(text2) is MimanTing))
					{
						text = text2;
						break;
					}
				}
			}
			if (pPrefabName == "Beer")
			{
				mimanTing = pTingRunner.CreateTingAfterUpdate<Drink>(text, new WorldCoordinate(pCharacter.inventoryRoomName, IntPoint.Zero), Direction.DOWN, pPrefabName);
				(mimanTing as Drink).masterProgramName = "FolkBeer";
				(mimanTing as Drink).liquidType = "beer";
			}
			else if (pPrefabName == "WellspringSoda")
			{
				mimanTing = pTingRunner.CreateTingAfterUpdate<Drink>(text, new WorldCoordinate(pCharacter.inventoryRoomName, IntPoint.Zero), Direction.DOWN, pPrefabName);
				(mimanTing as Drink).masterProgramName = "WellspringSoda";
				(mimanTing as Drink).liquidType = "soda";
			}
			else if (pPrefabName == "CoffeeCup_CoffeeCup")
			{
				mimanTing = pTingRunner.CreateTingAfterUpdate<Drink>(text, new WorldCoordinate(pCharacter.inventoryRoomName, IntPoint.Zero), Direction.DOWN, pPrefabName);
				(mimanTing as Drink).masterProgramName = "CafeCoffee";
				(mimanTing as Drink).liquidType = "coffee";
			}
			else
			{
				if (!(pPrefabName == "Margherita_Margherita") && !(pPrefabName == "DryMartini_DryMartini") && !(pPrefabName == "BloodyMary_BloodyMary") && !(pPrefabName == "LongIslandIceTea_LongIslandIceTea"))
				{
					throw new Exception("Don't know how to create item with prefab " + pPrefabName);
				}
				mimanTing = pTingRunner.CreateTingAfterUpdate<Drink>(text, new WorldCoordinate(pCharacter.inventoryRoomName, IntPoint.Zero), Direction.DOWN, pPrefabName);
				(mimanTing as Drink).masterProgramName = "AlcoholicDrink";
				(mimanTing as Drink).liquidType = "drink";
			}
			return mimanTing;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001640C File Offset: 0x0001460C
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00016410 File Offset: 0x00014610
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return pCharacter.room.name == this._roomName;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00016428 File Offset: 0x00014628
		public void Reset()
		{
		}

		// Token: 0x0400010F RID: 271
		private string _sellPointName;

		// Token: 0x04000110 RID: 272
		private string _roomName = "";

		// Token: 0x04000111 RID: 273
		private Ting _sellPoint;

		// Token: 0x04000112 RID: 274
		private List<string> _tingNamesToSell = new List<string>();

		// Token: 0x04000113 RID: 275
		private bool _collectEmptyDrinks;

		// Token: 0x04000114 RID: 276
		private static PathSolver _solver = new PathSolver();
	}
}
