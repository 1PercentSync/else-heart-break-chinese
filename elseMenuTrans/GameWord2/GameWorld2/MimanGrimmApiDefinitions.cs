using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GameTypes;
using GrimmLib;
using Pathfinding;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200001C RID: 28
	public class MimanGrimmApiDefinitions
	{
		// Token: 0x06000213 RID: 531 RVA: 0x0000B214 File Offset: 0x00009414
		public MimanGrimmApiDefinitions(World pWorld)
		{
			this._world = pWorld;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000B25C File Offset: 0x0000945C
		public void RegisterFunctions()
		{
			this._world.dialogueRunner.AddFunction("Hint", new DialogueRunner.Function(this.Hint));
			this._world.dialogueRunner.AddFunction("Story", new DialogueRunner.Function(this.Story));
			this._world.dialogueRunner.AddFunction("GetActiveNodes", new DialogueRunner.Function(this.GetActiveNodes));
			this._world.dialogueRunner.AddFunction("SetFocus", new DialogueRunner.Function(this.SetFocus));
			this._world.dialogueRunner.AddFunction("WP", new DialogueRunner.Function(this.WP));
			this._world.dialogueRunner.AddFunction("Log", new DialogueRunner.Function(this.Log));
			this._world.dialogueRunner.AddFunction("Path", new DialogueRunner.Function(this.Path));
			this._world.dialogueRunner.AddFunction("RebuildRoomNetwork", new DialogueRunner.Function(this.RebuildRoomNetwork));
			this._world.dialogueRunner.AddFunction("GetRoomExits", new DialogueRunner.Function(this.GetRoomExits));
			this._world.dialogueRunner.AddFunction("TilePath", new DialogueRunner.Function(this.TilePath));
			this._world.dialogueRunner.AddFunction("StartLogging", new DialogueRunner.Function(this.StartLogging));
			this._world.dialogueRunner.AddFunction("Kill", new DialogueRunner.Function(this.Kill));
			this._world.dialogueRunner.AddFunction("DetachFromNavNode", new DialogueRunner.Function(this.DetachFromNavNode));
			this._world.dialogueRunner.AddFunction("Pos", new DialogueRunner.Function(this.Pos));
			this._world.dialogueRunner.AddFunction("WorldPos", new DialogueRunner.Function(this.WorldPos));
			this._world.dialogueRunner.AddFunction("CarefulPos", new DialogueRunner.Function(this.CarefulPos));
			this._world.dialogueRunner.AddFunction("CarefulLayInBed", new DialogueRunner.Function(this.CarefulLayInBed));
			this._world.dialogueRunner.AddFunction("Dir", new DialogueRunner.Function(this.Dir));
			this._world.dialogueRunner.AddFunction("Load", new DialogueRunner.Function(this.Load));
			this._world.dialogueRunner.AddFunction("StartAction", new DialogueRunner.Function(this.StartAction));
			this._world.dialogueRunner.AddFunction("StopAction", new DialogueRunner.Function(this.StopAction));
			this._world.dialogueRunner.AddFunction("Interact", new DialogueRunner.Function(this.Interact));
			this._world.dialogueRunner.AddFunction("GetUpAndInteract", new DialogueRunner.Function(this.GetUpAndInteract));
			this._world.dialogueRunner.AddFunction("InteractUsingHandItem", new DialogueRunner.Function(this.InteractUsingHandItem));
			this._world.dialogueRunner.AddFunction("UseHandItem", new DialogueRunner.Function(this.UseHandItem));
			this._world.dialogueRunner.AddFunction("TakeOutItem", new DialogueRunner.Function(this.TakeOutItem));
			this._world.dialogueRunner.AddFunction("ClearHandItem", new DialogueRunner.Function(this.ClearHandItem));
			this._world.dialogueRunner.AddFunction("PutIntoInventory", new DialogueRunner.Function(this.PutIntoInventory));
			this._world.dialogueRunner.AddFunction("Tase", new DialogueRunner.Function(this.Tase));
			this._world.dialogueRunner.AddFunction("GetTasedGently", new DialogueRunner.Function(this.GetTasedGently));
			this._world.dialogueRunner.AddFunction("PickUp", new DialogueRunner.Function(this.PickUp));
			this._world.dialogueRunner.AddFunction("Walk", new DialogueRunner.Function(this.Walk));
			this._world.dialogueRunner.AddFunction("GetUpFromSeat", new DialogueRunner.Function(this.GetUpFromSeat));
			this._world.dialogueRunner.AddFunction("CancelWalking", new DialogueRunner.Function(this.CancelWalking));
			this._world.dialogueRunner.AddFunction("TurnLeft", new DialogueRunner.Function(this.TurnLeft));
			this._world.dialogueRunner.AddFunction("Give", new DialogueRunner.Function(this.Give));
			this._world.dialogueRunner.AddFunction("SetClockSpeed", new DialogueRunner.Function(this.SetClockSpeed));
			this._world.dialogueRunner.AddFunction("God", new DialogueRunner.Function(this.God));
			this._world.dialogueRunner.AddFunction("SetAvatar", new DialogueRunner.Function(this.SetAvatar));
			this._world.dialogueRunner.AddFunction("FindAvatar", new DialogueRunner.Function(this.FindAvatar));
			this._world.dialogueRunner.AddFunction("LockDoor", new DialogueRunner.Function(this.LockDoor));
			this._world.dialogueRunner.AddFunction("UnlockDoor", new DialogueRunner.Function(this.UnlockDoor));
			this._world.dialogueRunner.AddFunction("UseForRoomPathfinding", new DialogueRunner.Function(this.UseForRoomPathfinding));
			this._world.dialogueRunner.AddFunction("MuteNotifications", new DialogueRunner.Function(this.MuteNotifications));
			this._world.dialogueRunner.AddFunction("StartRinging", new DialogueRunner.Function(this.StartRinging));
			this._world.dialogueRunner.AddFunction("StartAllRadios", new DialogueRunner.Function(this.StartAllRadios));
			this._world.dialogueRunner.AddFunction("StartAllFuseboxes", new DialogueRunner.Function(this.StartAllFuseboxes));
			this._world.dialogueRunner.AddFunction("SetDoorTarget", new DialogueRunner.Function(this.SetDoorTarget));
			this._world.dialogueRunner.AddFunction("Sleep", new DialogueRunner.Function(this.Sleep));
			this._world.dialogueRunner.AddFunction("SleepUntil", new DialogueRunner.Function(this.SleepUntil));
			this._world.dialogueRunner.AddFunction("BeBored", new DialogueRunner.Function(this.BeBored));
			this._world.dialogueRunner.AddFunction("WakeUp", new DialogueRunner.Function(this.WakeUp));
			this._world.dialogueRunner.AddFunction("SetClock", new DialogueRunner.Function(this.SetClock));
			this._world.dialogueRunner.AddFunction("SetHour", new DialogueRunner.Function(this.SetHour));
			this._world.dialogueRunner.AddFunction("StartMusic", new DialogueRunner.Function(this.StartMusic));
			this._world.dialogueRunner.AddFunction("StopMusic", new DialogueRunner.Function(this.StopMusic));
			this._world.dialogueRunner.AddFunction("SetChannelOnRadio", new DialogueRunner.Function(this.SetChannelOnRadio));
			this._world.dialogueRunner.AddFunction("SetFriendLevel", new DialogueRunner.Function(this.SetFriendLevel));
			this._world.dialogueRunner.AddFunction("SetCorruption", new DialogueRunner.Function(this.SetCorruption));
			this._world.dialogueRunner.AddFunction("StartTalking", new DialogueRunner.Function(this.StartTalking));
			this._world.dialogueRunner.AddFunction("StopTalking", new DialogueRunner.Function(this.StopTalking));
			this._world.dialogueRunner.AddFunction("StartWaitForGift", new DialogueRunner.Function(this.StartWaitForGift));
			this._world.dialogueRunner.AddFunction("StopWaitForGift", new DialogueRunner.Function(this.StopWaitForGift));
			this._world.dialogueRunner.AddFunction("SetKnowledge", new DialogueRunner.Function(this.SetKnowledge));
			this._world.dialogueRunner.AddFunction("SetCameraAutoRotateSpeed", new DialogueRunner.Function(this.SetCameraAutoRotateSpeed));
			this._world.dialogueRunner.AddFunction("SetTimetable", new DialogueRunner.Function(this.SetTimetable));
			this._world.dialogueRunner.AddFunction("SetTimetableTimer", new DialogueRunner.Function(this.SetTimetableTimer));
			this._world.dialogueRunner.AddFunction("SetNeverGetsTired", new DialogueRunner.Function(this.SetNeverGetsTired));
			this._world.dialogueRunner.AddFunction("CreateCharacter", new DialogueRunner.Function(this.CreateCharacter));
			this._world.dialogueRunner.AddFunction("CreateBeerInHand", new DialogueRunner.Function(this.CreateBeerInHand));
			this._world.dialogueRunner.AddFunction("CreateDrinkInHand", new DialogueRunner.Function(this.CreateDrinkInHand));
			this._world.dialogueRunner.AddFunction("CreateCoffeeInHand", new DialogueRunner.Function(this.CreateCoffeeInHand));
			this._world.dialogueRunner.AddFunction("CreateCigarette", new DialogueRunner.Function(this.CreateCigarette));
			this._world.dialogueRunner.AddFunction("SetHandItem", new DialogueRunner.Function(this.SetHandItem));
			this._world.dialogueRunner.AddFunction("DropHandItem", new DialogueRunner.Function(this.DropHandItem));
			this._world.dialogueRunner.AddFunction("PutAwayHandItem", new DialogueRunner.Function(this.PutAwayHandItem));
			this._world.dialogueRunner.AddFunction("SetRain", new DialogueRunner.Function(this.SetRain));
			this._world.dialogueRunner.AddFunction("SetFieldToStringArray", new DialogueRunner.Function(this.SetFieldToStringArray));
			this._world.dialogueRunner.AddFunction("SetFieldToFloat", new DialogueRunner.Function(this.SetFieldToFloat));
			this._world.dialogueRunner.AddFunction("GetMasterProgramStatus", new DialogueRunner.Function(this.GetMasterProgramStatus));
			this._world.dialogueRunner.AddFunction("RunMasterProgram", new DialogueRunner.Function(this.RunMasterProgram));
			this._world.dialogueRunner.AddFunction("RunMasterProgramOnAllComputersInRoom", new DialogueRunner.Function(this.RunMasterProgramOnAllComputersInRoom));
			this._world.dialogueRunner.AddFunction("SetCode", new DialogueRunner.Function(this.SetCode));
			this._world.dialogueRunner.AddFunction("SetCodeAndRun", new DialogueRunner.Function(this.SetCodeAndRun));
			this._world.dialogueRunner.AddFunction("SetResettableCode", new DialogueRunner.Function(this.SetResettableCode));
			this._world.dialogueRunner.AddFunction("SetMemory", new DialogueRunner.Function(this.SetMemory));
			this._world.dialogueRunner.AddFunction("RunFunctionOnComputer", new DialogueRunner.Function(this.RunFunctionOnComputer));
			this._world.dialogueRunner.AddFunction("Throw", delegate(string[] o)
			{
				throw new Exception(o[0]);
			});
			this._world.dialogueRunner.AddFunction("RunMakeTransactionFunctionOnCreditCard", new DialogueRunner.Function(this.RunMakeTransactionFunctionOnCreditCard));
			this._world.dialogueRunner.AddFunction("CharacterTakesMoneyFromCreditCard", new DialogueRunner.Function(this.CharacterTakesMoneyFromCreditCard));
			this._world.dialogueRunner.AddFunction("CheckMoney", new DialogueRunner.Function(this.CheckMoney));
			this._world.dialogueRunner.AddFunction("Info", new DialogueRunner.Function(this.Info));
			this._world.dialogueRunner.AddFunction("Bugtalk", new DialogueRunner.Function(this.Bugtalk));
			this._world.dialogueRunner.AddFunction("GetOutput", new DialogueRunner.Function(this.GetOutput));
			this._world.dialogueRunner.AddFunction("GetSource", new DialogueRunner.Function(this.GetSource));
			this._world.dialogueRunner.AddFunction("GetProgramErrors", new DialogueRunner.Function(this.GetProgramErrors));
			this._world.dialogueRunner.AddFunction("RemoveDanglingDialogueOptions", new DialogueRunner.Function(this.RemoveDanglingDialogueOptions));
			this._world.dialogueRunner.AddFunction("Hack", new DialogueRunner.Function(this.Hack));
			this._world.dialogueRunner.AddFunction("PrintGroupOfTile", new DialogueRunner.Function(this.PrintGroupOfTile));
			this._world.dialogueRunner.AddFunction("CheckNavNodeChain", new DialogueRunner.Function(this.CheckNavNodeChain));
			this._world.dialogueRunner.AddFunction("RunPathfindingTests", new DialogueRunner.Function(this.RunPathfindingTests));
			this._world.dialogueRunner.AddFunction("MoveAllTingsToTing", new DialogueRunner.Function(this.MoveAllTingsToTing));
			this._world.dialogueRunner.AddFunction("TypeIntoComputer", new DialogueRunner.Function(this.TypeIntoComputer));
			this._world.dialogueRunner.AddFunction("Explode", new DialogueRunner.Function(this.Explode));
			this._world.dialogueRunner.AddFunction("SetRunning", new DialogueRunner.Function(this.SetRunning));
			this._world.dialogueRunner.AddFunction("RunLevelIntegrityTests", new DialogueRunner.Function(this.RunLevelIntegrityTests));
			this._world.dialogueRunner.AddFunction("SetLanguage", new DialogueRunner.Function(this.SetLanguage));
			this._world.dialogueRunner.AddFunction("ListTingsOfType", new DialogueRunner.Function(this.ListTingsOfType));
			this._world.dialogueRunner.AddFunction("ListDigitalTrash", new DialogueRunner.Function(this.ListDigitalTrash));
			this._world.dialogueRunner.AddFunction("ListActiveNodes", new DialogueRunner.Function(this.ListActiveNodes));
			this._world.dialogueRunner.AddFunction("ListActivePrograms", new DialogueRunner.Function(this.ListActivePrograms));
			this._world.dialogueRunner.AddFunction("ListAllPrograms", new DialogueRunner.Function(this.ListAllPrograms));
			this._world.dialogueRunner.AddFunction("KillAllPrograms", new DialogueRunner.Function(this.KillAllPrograms));
			this._world.dialogueRunner.AddFunction("TurnOnTv", new DialogueRunner.Function(this.TurnOnTv));
			this._world.dialogueRunner.AddFunction("StoryItemsSanityTests", new DialogueRunner.Function(this.StoryItemsSanityTests));
			this._world.dialogueRunner.AddFunction("SetCameraTarget", new DialogueRunner.Function(this.SetCameraTarget));
			this._world.dialogueRunner.AddFunction("Beat", new DialogueRunner.Function(this.Beat));
			this._world.dialogueRunner.AddFunction("HeartIsBroken", new DialogueRunner.Function(this.HeartIsBroken));
			this._world.dialogueRunner.AddFunction("SetAllComputersToRunProgram", new DialogueRunner.Function(this.SetAllComputersToRunProgram));
			this._world.dialogueRunner.AddFunction("GetAngryAtComputer", new DialogueRunner.Function(this.GetAngryAtComputer));
			this._world.dialogueRunner.AddFunction("StopSimulation", new DialogueRunner.Function(this.StopSimulation));
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000C214 File Offset: 0x0000A414
		private void StopSimulation(string[] args)
		{
			this._world.paused = true;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000C224 File Offset: 0x0000A424
		public void RemoveDanglingDialogueOptions(string[] args)
		{
			if (this.onRemoveDanglingDialogueOptions != null)
			{
				string text = args[0];
				D.Log("Will remove dangling dialogue options");
				BranchingDialogueNode activeBranchingDialogueNode = this._world.dialogueRunner.GetActiveBranchingDialogueNode(text);
				if (activeBranchingDialogueNode != null)
				{
					activeBranchingDialogueNode.Stop();
				}
				this.onRemoveDanglingDialogueOptions(text);
			}
			else
			{
				D.Log("onRemoveDanglingDialogueOptions in MimanGrimmApiDefinitions is null!");
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000C284 File Offset: 0x0000A484
		private void PrintGroupOfTile(string[] args)
		{
			if (args.Length == 3)
			{
				D.Log(this._world.roomRunner.GetRoom(args[0]).GetTile(Convert.ToInt32(args[1]), Convert.ToInt32(args[2])).group.ToString());
			}
			else
			{
				D.Log(this._world.tingRunner.GetTing(args[0]).tile.group.ToString());
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000C304 File Offset: 0x0000A504
		private void ListTingsOfType(string[] args)
		{
			string text = args[0];
			if (text != null)
			{
				if (MimanGrimmApiDefinitions.<>f__switch$map0 == null)
				{
					MimanGrimmApiDefinitions.<>f__switch$map0 = new Dictionary<string, int>(21)
					{
						{ "Radio", 0 },
						{ "Tv", 1 },
						{ "Character", 2 },
						{ "Drink", 3 },
						{ "MysticalCube", 4 },
						{ "Key", 5 },
						{ "Map", 6 },
						{ "Snus", 7 },
						{ "Drug", 8 },
						{ "Lamp", 9 },
						{ "Hackdev", 10 },
						{ "MusicBox", 11 },
						{ "Door", 12 },
						{ "CreditCard", 13 },
						{ "Computer", 14 },
						{ "Locker", 15 },
						{ "Robot", 16 },
						{ "Seat", 17 },
						{ "Sink", 18 },
						{ "FuseBox", 19 },
						{ "Portal", 20 }
					};
				}
				int num;
				if (MimanGrimmApiDefinitions.<>f__switch$map0.TryGetValue(text, out num))
				{
					Type type;
					switch (num)
					{
					case 0:
						type = typeof(Radio);
						break;
					case 1:
						type = typeof(Tv);
						break;
					case 2:
						type = typeof(Character);
						break;
					case 3:
						type = typeof(Drink);
						break;
					case 4:
						type = typeof(MysticalCube);
						break;
					case 5:
						type = typeof(Key);
						break;
					case 6:
						type = typeof(Map);
						break;
					case 7:
						type = typeof(Snus);
						break;
					case 8:
						type = typeof(Drug);
						break;
					case 9:
						type = typeof(Lamp);
						break;
					case 10:
						type = typeof(Hackdev);
						break;
					case 11:
						type = typeof(MusicBox);
						break;
					case 12:
						type = typeof(Door);
						break;
					case 13:
						type = typeof(CreditCard);
						break;
					case 14:
						type = typeof(Computer);
						break;
					case 15:
						type = typeof(Locker);
						break;
					case 16:
						type = typeof(Robot);
						break;
					case 17:
						type = typeof(Seat);
						break;
					case 18:
						type = typeof(Sink);
						break;
					case 19:
						type = typeof(FuseBox);
						break;
					case 20:
						type = typeof(Portal);
						break;
					default:
						goto IL_02ED;
					}
					foreach (Ting ting in this._world.tingRunner.GetTings())
					{
						if (ting.GetType() == type)
						{
							D.Log(string.Concat(new string[]
							{
								ting.name,
								"\t",
								ting.room.name,
								"\t | Prefab: ",
								ting.prefab
							}));
						}
					}
					return;
				}
			}
			IL_02ED:
			throw new Exception("No support for type " + args[0]);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000C6C4 File Offset: 0x0000A8C4
		private void ListDigitalTrash(string[] args)
		{
			D.Log("List digital trash: ");
			for (int i = 0; i < 70; i++)
			{
				string name = "DigitalTrash" + i;
				IEnumerable<Ting> enumerable = from floppy in this._world.tingRunner.GetTings()
					where floppy is Floppy && (floppy as Floppy).masterProgramName == name
					select floppy;
				string text = "";
				for (int j = 0; j < enumerable.Count<Ting>(); j++)
				{
					text += "*";
				}
				D.Log(string.Concat(new object[]
				{
					"Nr of '",
					name,
					"': ",
					enumerable.Count<Ting>(),
					" ",
					text
				}));
			}
			D.Log("-------------------------------------------------");
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000C7A8 File Offset: 0x0000A9A8
		private void ListActiveNodes(string[] args)
		{
			string conversation = args[0];
			IEnumerable<DialogueNode> enumerable = from node in this._world.dialogueRunner.GetAllNodes()
				where node.conversation == conversation && node.isOn
				select node;
			D.Log("Active nodes in '" + conversation + "':");
			foreach (DialogueNode dialogueNode in enumerable)
			{
				D.Log(dialogueNode.name + " " + dialogueNode.ToString());
			}
			D.Log("-------------------------------------------------");
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000C874 File Offset: 0x0000AA74
		private void ListActivePrograms(string[] args)
		{
			D.Log("-------------------------------------------------");
			D.Log("Active programs:");
			foreach (Program program in this._world.programRunner.GetAllPrograms())
			{
				if (program.isOn)
				{
					D.Log(string.Concat(new object[]
					{
						program.ToString(),
						" (",
						program.executionsPerFrame,
						" mhz)"
					}));
				}
			}
			D.Log("-------------------------------------------------");
			this.PrintProgramsReport();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000C910 File Offset: 0x0000AB10
		private void ListAllPrograms(string[] args)
		{
			D.Log("-------------------------------------------------");
			D.Log("All programs:");
			foreach (Program program in this._world.programRunner.GetAllPrograms())
			{
				D.Log(string.Concat(new object[]
				{
					program.ToString(),
					" (",
					program.executionsPerFrame,
					" mhz)"
				}));
			}
			D.Log("-------------------------------------------------");
			this.PrintProgramsReport();
			D.Log("Total nr of programs: " + this._world.programRunner.GetAllPrograms().Length);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000C9C8 File Offset: 0x0000ABC8
		private void PrintProgramsReport()
		{
			D.Log("Total nr of programs = " + this._world.programRunner.GetAllPrograms().Count<Program>());
			D.Log("Nr of memory spaces = " + MemorySpace.nrOfMemorySpacesInMemory);
			D.Log("Nr of scopes = " + Scope.nrOfScopesInMemory);
			D.Log("Nr of sprak runners = " + SprakRunner.nrOfSprakRunnersInMemory);
			D.Log("Nr of interpreters = " + InterpreterTwo.nrOfInterpreters);
			D.Log("Nr of AST:s = " + AST.nrOfASTsInMemory);
			D.Log("COLLECT");
			GC.Collect();
			GC.WaitForPendingFinalizers();
			D.Log("Total nr of programs = " + this._world.programRunner.GetAllPrograms().Count<Program>());
			D.Log("Nr of memory spaces = " + MemorySpace.nrOfMemorySpacesInMemory);
			D.Log("Nr of scopes = " + Scope.nrOfScopesInMemory);
			D.Log("Nr of sprak runners = " + SprakRunner.nrOfSprakRunnersInMemory);
			D.Log("Nr of interpreters = " + InterpreterTwo.nrOfInterpreters);
			D.Log("Nr of AST:s = " + AST.nrOfASTsInMemory);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000CB38 File Offset: 0x0000AD38
		private void KillAllPrograms(string[] args)
		{
			foreach (Program program in this._world.programRunner.GetAllPrograms())
			{
			}
			this.PrintProgramsReport();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000CB74 File Offset: 0x0000AD74
		private void StoryItemsSanityTests(string[] args)
		{
			Computer ting = this._world.tingRunner.GetTing<Computer>("FinanceComputer");
			D.assert(ting.masterProgramName == "FinanceComputer", "FinanceComputer program");
			Computer ting2 = this._world.tingRunner.GetTing<Computer>("Hotel_Lobby_ComputerCashier");
			D.assert(ting2.masterProgramName == "HotelLobbyComputer", "Hotel_Lobby_ComputerCashier program");
			Door ting3 = this._world.tingRunner.GetTing<Door>("Hotel_Office_DoorToBasement");
			D.assert(ting3.isLocked, "Hotel_Office_DoorToBasement not locked");
			Computer ting4 = this._world.tingRunner.GetTing<Computer>("Computer1");
			D.assert(ting4.masterProgramName == "Lodge_Room1_Computer1", "Computer 1 program");
			Computer ting5 = this._world.tingRunner.GetTing<Computer>("Computer2");
			D.assert(ting5.masterProgramName == "Lodge_Room1_Computer2", "Computer 2 program");
			Computer ting6 = this._world.tingRunner.GetTing<Computer>("Computer3");
			D.assert(ting6.masterProgramName == "Lodge_Room1_Computer3", "Computer 3 program");
			Computer ting7 = this._world.tingRunner.GetTing<Computer>("Computer5");
			D.assert(ting7.masterProgramName == "Lodge_Room1_Computer5", "Computer 5 program");
			Computer ting8 = this._world.tingRunner.GetTing<Computer>("Computer6");
			D.assert(ting8.masterProgramName == "Lodge_Room1_Computer6", "Computer 6 program");
			Computer ting9 = this._world.tingRunner.GetTing<Computer>("Computer7");
			D.assert(ting9.masterProgramName == "Lodge_Room1_Computer7", "Computer 7 program");
			Door ting10 = this._world.tingRunner.GetTing<Door>("Lodge_Room1_DoorToRoom2");
			D.assert(ting10.isLocked, "lodge door to room2 locked");
			D.assert(ting10.targetDoorName == "Lodge_Room2_DoorToRoom1", "lodge door to room2 target");
			Button ting11 = this._world.tingRunner.GetTing<Button>("FishGameStartButton");
			D.assert(ting11.masterProgramName == "FishGameButton", "FishGameStartButton program");
			Floppy ting12 = this._world.tingRunner.GetTing<Floppy>("Floppy_100");
			D.assert(ting12.masterProgramName == "StrangeDataFloppy", "StrangeDataFloppy program");
			Computer ting13 = this._world.tingRunner.GetTing<Computer>("FactoryLobbyTrap");
			D.assert(ting13.masterProgramName == "FactoryLobbyTrap", "FactoryLobbyTrap program");
			D.assert(ting13.hasTrapAPI, "FactoryLobbyTrap trap API");
			Computer ting14 = this._world.tingRunner.GetTing<Computer>("FactoryServerDoor");
			D.assert(ting14.masterProgramName == "FactoryServerDoor", "FactoryServerDoor program");
			Computer ting15 = this._world.tingRunner.GetTing<Computer>("FactoryAccessComputer");
			D.assert(ting15.masterProgramName == "FactoryAccessComputer", "FactoryAccessComputer program");
			Computer ting16 = this._world.tingRunner.GetTing<Computer>("LongsonPlaystation");
			D.assert(ting16.masterProgramName == "LongsonPlaystation", "LongsonPlaystation program");
			Door ting17 = this._world.tingRunner.GetTing<Door>("OutsideFelixApartment_Door_2");
			D.assert(ting17.isLocked, "OutsideFelixApartment_Door_2 not locked");
			Door ting18 = this._world.tingRunner.GetTing<Door>("HarborWest_DoorToSodaStorage");
			D.assert(ting18.isLocked, "HarborWest_DoorToSodaStorage not locked");
			Computer ting19 = this._world.tingRunner.GetTing<Computer>("HarborWest_SodaStorageComputer");
			D.assert(ting19.masterProgramName == "SodaStorageDoor", "HarborWest_SodaStorageComputer program");
			Computer ting20 = this._world.tingRunner.GetTing<Computer>("TramComputer1");
			D.assert(ting20.masterProgramName == "TramNr1", "tram1 computer program");
			Computer ting21 = this._world.tingRunner.GetTing<Computer>("TramComputer2");
			D.assert(ting21.masterProgramName == "TramNr1b", "tram2 computer program");
			Computer ting22 = this._world.tingRunner.GetTing<Computer>("TramComputer3");
			D.assert(ting22.masterProgramName == "TramNr1c", "tram3 computer program");
			Computer ting23 = this._world.tingRunner.GetTing<Computer>("TramComputer4");
			D.assert(ting23.masterProgramName == "TramNr1d", "tram4 computer program");
			Door ting24 = this._world.tingRunner.GetTing<Door>("Plaza_DoorToMonadsApartment");
			D.assert(ting24.targetDoorName == "MonadsApartment_DoorToPlaza", "monadDoor has invalid target");
			D.assert(ting24.isLocked, "monadDoor lock");
			D.assert(ting24.code == 38984312, "monadDoor code");
			Door ting25 = this._world.tingRunner.GetTing<Door>("SICP");
			D.assert(ting25.targetDoorName == "Internet_Door_13", "bookDoor has invalid target");
			Computer ting26 = this._world.tingRunner.GetTing<Computer>("Heart");
			D.assert(ting26.masterProgramName == "TheHeart", "The heart program");
			Computer ting27 = this._world.tingRunner.GetTing<Computer>("Internet_Heart_Analyzer");
			D.assert(ting27.masterProgramName == "HeartAnalyzer", "The Internet_Heart_Analyzer program");
			Console.ForegroundColor = ConsoleColor.Green;
			D.Log("All StoryItemsSantiyTests ran");
			Console.ForegroundColor = ConsoleColor.White;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000D108 File Offset: 0x0000B308
		private void RunLevelIntegrityTests(string[] args)
		{
			Tram ting = this._world.tingRunner.GetTing<Tram>("Tram1");
			Tram ting2 = this._world.tingRunner.GetTing<Tram>("Tram1b");
			Tram ting3 = this._world.tingRunner.GetTing<Tram>("Tram1c");
			Tram ting4 = this._world.tingRunner.GetTing<Tram>("Tram1d");
			D.isNull(ting.movingDoor, "missing door tram1");
			D.isNull(ting2.movingDoor, "missing door tram1b");
			D.isNull(ting3.movingDoor, "missing door tram1c");
			D.isNull(ting4.movingDoor, "missing door tram1d");
			D.isNull(ting.movingDoor.targetDoor, "tram1 moving door missing target");
			D.isNull(ting2.movingDoor.targetDoor, "tram1b moving door missing target");
			D.isNull(ting3.movingDoor.targetDoor, "tram1c moving door missing target");
			D.isNull(ting4.movingDoor.targetDoor, "tram1d moving door missing target");
			D.isNull(ting.currentNavNode, "missing current nav node tram1");
			D.isNull(ting2.currentNavNode, "missing current nav node tram1b");
			D.isNull(ting3.currentNavNode, "missing current nav node tram1c");
			D.isNull(ting4.currentNavNode, "missing current nav node tram1d");
			MusicBox ting5 = this._world.tingRunner.GetTing<MusicBox>("RadioStation_Channel7");
			D.isNull(ting5, "channel 7");
			D.assert(ting5.isPlaying, "channel 7 not playing");
			D.assert(ting5.soundName == "Station7", "channel 7 wrong sound");
			MusicBox ting6 = this._world.tingRunner.GetTing<MusicBox>("RadioStation_Channel100");
			D.isNull(ting6, "channel 100");
			D.assert(ting6.isPlaying, "channel 100 not playing");
			D.assert(ting6.soundName == "ExperimentStation", "channel 100 wrong sound");
			Door ting7 = this._world.tingRunner.GetTing<Door>("Hotel_Corridor_Door1");
			D.assert(ting7.isLocked, "hotel door 1 lock");
			Door ting8 = this._world.tingRunner.GetTing<Door>("Hotel_Corridor_Door2");
			D.assert(ting8.isLocked, "hotel door 2 lock");
			Door ting9 = this._world.tingRunner.GetTing<Door>("Hotel_Corridor_Door3");
			D.assert(ting9.isLocked, "hotel door 3 lock");
			Door ting10 = this._world.tingRunner.GetTing<Door>("Hotel_Corridor_Door4");
			D.assert(ting10.isLocked, "hotel door 4 lock");
			Door ting11 = this._world.tingRunner.GetTing<Door>("Hotel_Corridor_Door5");
			D.assert(ting11.isLocked, "hotel door 5 lock");
			Door ting12 = this._world.tingRunner.GetTing<Door>("Ministry_Elevator1_Door1");
			Door ting13 = this._world.tingRunner.GetTing<Door>("Ministry_Elevator2_Door1");
			Door ting14 = this._world.tingRunner.GetTing<Door>("Ministry_Elevator3_Door1");
			Door ting15 = this._world.tingRunner.GetTing<Door>("Ministry_Elevator4_Door1");
			D.assert(ting12.targetDoorName == "Ministry_Lobby_MinistryElevatorDoor_1", "ministry elevator target 1");
			D.assert(ting13.targetDoorName == "Ministry_Lobby_MinistryElevatorDoor_2", "ministry elevator target 2");
			D.assert(ting14.targetDoorName == "Ministry_Lobby_MinistryElevatorDoor_3", "ministry elevator target 3");
			D.assert(ting15.targetDoorName == "Ministry_Lobby_MinistryElevatorDoor_4", "ministry elevator target 4");
			D.assert(3 == ting12.elevatorAlternatives.Length, "ministry elevator 1 alternatives");
			D.assert(4 == ting13.elevatorAlternatives.Length, "ministry elevator 2 alternatives");
			D.assert(4 == ting14.elevatorAlternatives.Length, "ministry elevator 3 alternatives");
			D.assert(3 == ting15.elevatorAlternatives.Length, "ministry elevator 4 alternatives");
			this.CountNrOfUnitializedPrograms<Floppy>("BlankSlate", (Floppy floppy) => floppy.masterProgramName);
			this.CountNrOfUnitializedPrograms<Computer>("HelloWorld", (Computer computer) => computer.masterProgramName);
			int num = 0;
			Door[] tingsOfType = this._world.tingRunner.GetTingsOfType<Door>();
			foreach (Door door in tingsOfType)
			{
				if (door.targetDoor == null && !this.IsInTestingRoom(door))
				{
					num++;
					D.Log(door.name + " has target null");
				}
			}
			D.Log("There are " + num + " doors with null targets");
			D.Log("Total nr of doors: " + tingsOfType.Length);
			D.Log("Total nr of portals: " + this._world.tingRunner.GetTingsOfType<Portal>().Length);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000D5E0 File Offset: 0x0000B7E0
		private bool IsInTestingRoom(Ting door)
		{
			return door.room.name.Contains("Nicke") || door.room.name.Contains("Test");
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000D620 File Offset: 0x0000B820
		private void CountNrOfUnitializedPrograms<T>(string pDefaultProgramName, Func<T, string> pGetter) where T : Ting
		{
			int num = 0;
			int num2 = 0;
			foreach (T t in this._world.tingRunner.GetTingsOfType<T>())
			{
				num++;
				if (pGetter(t) == pDefaultProgramName)
				{
					D.Log("USING DEFAULT PROGRAM: " + t.name + " in " + t.room.name);
					num2++;
				}
			}
			D.Log(string.Concat(new object[]
			{
				"# There are ",
				num2,
				" ",
				typeof(T).ToString(),
				":s with default programs (total count ",
				num,
				")"
			}));
			int num3 = 0;
			int num4 = 0;
			foreach (Room room in this._world.roomRunner.rooms)
			{
				num3++;
				num4 += room.points.Length;
			}
			D.Log(string.Concat(new object[] { "# There are ", num3, " rooms and ", num4, " tile points in the world" }));
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000D7B8 File Offset: 0x0000B9B8
		private void RunPathfindingTests(string[] args)
		{
			MimanTingRunner tingRunner = this._world.tingRunner;
			MimanPathfinder2 mimanPathfinder = new MimanPathfinder2(tingRunner, this._world.roomRunner);
			List<string[]> list = new List<string[]>
			{
				new string[] { "Factory_Floor1_Point1", "Cafe_Room1_Point" },
				new string[] { "Cafe_Room1_Point", "FelixApartment_Point1" },
				new string[] { "FelixApartment_Point1", "Mines_ClubDot_Point" },
				new string[] { "Mines_ClubDot_Point", "Casino_Floor2_Point" },
				new string[] { "MonadsApartment_Point1", "Casino_Floor2_Point" },
				new string[] { "Casino_Floor2_Point", "Factory_Lobby_Point1" },
				new string[] { "Factory_Lobby_Point1", "Lodge_Underwater_Point2" },
				new string[] { "Lodge_Underwater_Point2", "Hotel_Bathroom_Point1" },
				new string[] { "Hotel_Bathroom_Point1", "Factory_Floor1_Point1" },
				new string[] { "Casino_Floor2_Point", "Factory_Lobby_Point2" },
				new string[] { "Factory_Lobby_Point2", "HarborSouth_Trigger_1" },
				new string[] { "HarborSouth_Trigger_1", "PoorDesolateBuilding1_Corridor_Point" },
				new string[] { "PoorDesolateBuilding1_Corridor_Point", "Factory_Floor1_Point1" },
				new string[] { "Factory_Floor1_Point1", "Cafe_Room1_Point" },
				new string[] { "Factory_Office_Point1", "SodaStorage_Point" },
				new string[] { "SodaStorage_Point", "FancyHouse1_Point" },
				new string[] { "FancyHouse1_Point", "PixiesApartment_Point" },
				new string[] { "PixiesApartment_Point", "PetrasApartmentPoint1" },
				new string[] { "PetrasApartmentPoint1", "Cafe_Room1_Point" },
				new string[] { "Cafe_Room1_Point", "SodaStorage_Point" },
				new string[] { "SodaStorage_Point", "MonadsApartment_Point1" },
				new string[] { "BureucratApartment1_Trigger_1", "Ministry_Offices1_Trigger_1" },
				new string[] { "NiniApartment_Trigger_1", "EmmaApartment_Trigger_1" },
				new string[] { "AmandaApartment_Trigger_1", "Ministry_Offices3_Trigger_1" },
				new string[] { "Mines_ClubDot_Point", "IvanApartment_Trigger_1" },
				new string[] { "Plaza_Point", "FancyHouse1BedRoom_BedL_Right_1" },
				new string[] { "Plaza_Point", "GlennApartment_Testing_Poor_Bed_Poor_Bed_6_1" },
				new string[] { "Plaza_Point", "FancyHouse2BedRoom_BedL_Right_1" },
				new string[] { "Plaza_Point", "EmmaBedRoom_Testing_Poor_Bed_Poor_Bed_7_1" },
				new string[] { "Plaza_Point", "AmandasBed" },
				new string[] { "Plaza_Point", "TinyBarn_Poor_Bed_Poor_Bed_1" },
				new string[] { "Casino_Floor2_Point", "TouristGirlApartment_BedL_Right_1" },
				new string[] { "Casino_Floor2_Point", "Casino_Floor1_SecurityPoint" },
				new string[] { "Factory_Office_Point1", "MonadsApartment_BedL_Right_1" },
				new string[] { "Casino_Floor2_Point", "MonadsApartment_BedL_Left_1" },
				new string[] { "Casino_Floor2_Point", "LongsonApartment_Testing_Poor_Bed_Poor_Bed_7_1" },
				new string[] { "Casino_Floor2_Point", "BobSchack_BedL_Left_1" },
				new string[] { "Casino_Floor2_Point", "PandaApartment_Poor_Bed_Poor_Bed_1" },
				new string[] { "Casino_Floor2_Point", "PandaApartment_Poor_Bed_Poor_Bed_2" },
				new string[] { "Casino_Floor2_Point", "PandaApartment_BedL_Left_1" },
				new string[] { "Hotel_Lobby_Point", "Hotel_Corridor_Door1" }
			};
			int num = 0;
			foreach (string[] array in list)
			{
				Ting ting = tingRunner.GetTing(array[0]);
				Ting ting2 = tingRunner.GetTing(array[1]);
				MimanPath mimanPath = mimanPathfinder.Search(ting, ting2);
				if (mimanPath.status != MimanPathStatus.FOUND_GOAL)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					D.LogError(string.Concat(new object[] { "Failed pathfinding from ", ting, " to ", ting2, ": ", mimanPath }));
					num++;
					Console.ForegroundColor = ConsoleColor.White;
				}
			}
			if (num == 0)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				D.Log("All pathfinding tests ran without error!");
				Console.ForegroundColor = ConsoleColor.White;
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				D.Log(num + " PATHFINDING ERRORS!");
				Console.ForegroundColor = ConsoleColor.White;
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000DD80 File Offset: 0x0000BF80
		private void MoveAllTingsToTing(string[] args)
		{
			Ting tingUnsafe = this._world.tingRunner.GetTingUnsafe(args[0]);
			foreach (Ting ting in this._world.tingRunner.GetTings())
			{
				if (ting.canBePickedUp)
				{
					try
					{
						ting.position = tingUnsafe.position;
					}
					catch (Exception ex)
					{
						D.Log(string.Concat(new object[] { "Failed to move prefab ", ting.prefab, ": ", ex }));
					}
				}
			}
			D.Log("Done moving all tings!");
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000DE70 File Offset: 0x0000C070
		private void TypeIntoComputer(string[] args)
		{
			if (args.Length < 2)
			{
				D.Log("Too few args to TypeIntoComputer");
				return;
			}
			Character character = this._world.tingRunner.GetTingUnsafe(args[0]) as Character;
			string text = args[1];
			if (character == null)
			{
				D.Log(args[0] + " can't type");
				return;
			}
			Computer computer = character.actionOtherObject as Computer;
			if (computer == null)
			{
				D.Log(character + " is not interacting with a computer");
				return;
			}
			if (text == "ENTER")
			{
				computer.OnEnterKey();
			}
			else
			{
				string text2 = text;
				for (int i = 0; i < text2.Length; i++)
				{
					computer.OnKeyDown(text2[i].ToString());
				}
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000DF3C File Offset: 0x0000C13C
		private void Explode(string[] args)
		{
			MimanTing mimanTing = this._world.tingRunner.GetTingUnsafe(args[0]) as MimanTing;
			mimanTing.StartAction("Explode", null, 0.1f, 5f);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000DF78 File Offset: 0x0000C178
		private void SetRunning(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			ting.running = args[1] == "true";
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000DFAC File Offset: 0x0000C1AC
		public void SetLanguage(string[] args)
		{
			this._world.settings.translationLanguage = args[0];
			this._world.RefreshTranslationLanguage();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000DFCC File Offset: 0x0000C1CC
		private void CheckNavNodeChain(string[] args)
		{
			NavNode navNode = this._world.tingRunner.GetTingUnsafe(args[0]) as NavNode;
			D.Log("Starting at " + navNode);
			NavNode navNode2 = navNode.mainTrack;
			for (;;)
			{
				Console.ReadLine();
				D.Log("Visiting " + navNode2 + ((!navNode2.isStation) ? "" : " STATION"));
				if (navNode2.mainTrack == null)
				{
					break;
				}
				if (navNode2.mainTrack == navNode)
				{
					goto Block_3;
				}
				navNode2 = navNode2.mainTrack;
			}
			D.Log("Next node is null! Stopping.");
			return;
			Block_3:
			D.Log("Back at start, stopping.");
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000E07C File Offset: 0x0000C27C
		private void Hint(string[] args)
		{
			this._world.settings.Hint(args[0]);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000E094 File Offset: 0x0000C294
		private void Story(string[] args)
		{
			string text = "_" + args[0];
			D.Log(" --- STORY " + text + " --- ");
			string[] namesOfAllStoppedConversationsWithNameContaining = this._world.dialogueRunner.GetNamesOfAllStoppedConversationsWithNameContaining(text);
			foreach (string text2 in namesOfAllStoppedConversationsWithNameContaining)
			{
				int num = text2.IndexOf(text);
				if (num > -1)
				{
					string text3 = text2.Substring(0, num);
					if (text3 != "")
					{
						this._world.dialogueRunner.StopAllConversationsContaining(text3 + "_");
					}
				}
			}
			this._world.dialogueRunner.StartAllConversationsContaining(text);
			this._world.settings.LogStoryEvent("Story(" + text + ")");
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000E170 File Offset: 0x0000C370
		private void GetActiveNodes(string[] args)
		{
			string text = args[0];
			List<DialogueNode> activeNodes = this._world.dialogueRunner.GetActiveNodes(text);
			D.Log("Active nodes: ");
			foreach (DialogueNode dialogueNode in activeNodes)
			{
				D.Log(dialogueNode.ToString());
			}
			D.Log("---");
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000E204 File Offset: 0x0000C404
		private void SetFocus(string[] args)
		{
			string text = args[0];
			this._world.dialogueRunner.FocusConversation(text);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000E228 File Offset: 0x0000C428
		private void Log(string[] args)
		{
			D.Log(args[0]);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000E234 File Offset: 0x0000C434
		private void RebuildRoomNetwork(string[] args)
		{
			MimanPathfinder2 mimanPathfinder = new MimanPathfinder2(this._world.tingRunner, this._world.roomRunner);
			RoomNetwork roomNetwork = mimanPathfinder.RecreateRoomNetwork();
			if (args.Length > 0 && args[0] == "true")
			{
				D.Log("ROOM NETWORK: " + roomNetwork);
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000E290 File Offset: 0x0000C490
		private void GetRoomExits(string[] args)
		{
			string text = args[0];
			Room room = this._world.roomRunner.GetRoom(text);
			RoomNetwork roomNetwork = MimanPathfinder2.roomNetwork;
			if (roomNetwork == null)
			{
				D.Log("network is null");
			}
			for (int i = 0; i < 20; i++)
			{
				RoomGroup roomGroup = new RoomGroup(room, i);
				if (roomNetwork.linkedRoomGroups.ContainsKey(roomGroup))
				{
					Dictionary<RoomGroup, Ting> dictionary = roomNetwork.linkedRoomGroups[roomGroup];
					if (dictionary.Count > 0)
					{
						D.Log("In " + roomGroup + ": ");
						foreach (RoomGroup roomGroup2 in dictionary.Keys)
						{
							D.Log(string.Concat(new object[]
							{
								" ",
								dictionary[roomGroup2],
								" => ",
								roomGroup2
							}));
						}
					}
				}
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000E3C0 File Offset: 0x0000C5C0
		private void Path(string[] args)
		{
			Ting tingUnsafe = this._world.tingRunner.GetTingUnsafe(args[0]);
			Ting tingUnsafe2 = this._world.tingRunner.GetTingUnsafe(args[1]);
			if (tingUnsafe == null)
			{
				D.Log("start is null");
				return;
			}
			if (tingUnsafe2 == null)
			{
				D.Log("goal is null");
				return;
			}
			MimanPathfinder2 mimanPathfinder = new MimanPathfinder2(this._world.tingRunner, this._world.roomRunner);
			MimanPath mimanPath = mimanPathfinder.Search(tingUnsafe, tingUnsafe2);
			D.Log(mimanPath.ToString());
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000E448 File Offset: 0x0000C648
		private void TilePath(string[] args)
		{
			Ting tingUnsafe = this._world.tingRunner.GetTingUnsafe(args[0]);
			Ting tingUnsafe2 = this._world.tingRunner.GetTingUnsafe(args[1]);
			if (tingUnsafe == null)
			{
				D.Log("start is null");
				return;
			}
			if (tingUnsafe2 == null)
			{
				D.Log("goal is null");
				return;
			}
			PathSolver pathSolver = new PathSolver();
			D.Log(pathSolver.FindPath(tingUnsafe.tile, tingUnsafe2.tile, this._world.roomRunner, true).ToString());
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000E4D0 File Offset: 0x0000C6D0
		private void StartLogging(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			ting.logger.AddListener(new D.LogHandler(D.Log));
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000E508 File Offset: 0x0000C708
		private void WP(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			D.Log(ting.name + "'s world pos: " + ting.worldPoint.ToString());
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000E54C File Offset: 0x0000C74C
		private void Info(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PropertyInfo propertyInfo in ting.GetType().GetProperties())
			{
				foreach (Attribute attribute in propertyInfo.GetCustomAttributes(true))
				{
					if (attribute.Match(MimanGrimmApiDefinitions.EDITABLE_IN_EDITOR) || attribute.Match(MimanGrimmApiDefinitions.SHOW_IN_EDITOR))
					{
						object value = propertyInfo.GetValue(ting, null);
						string text;
						if (value == null)
						{
							text = "null";
						}
						else if (value is string)
						{
							text = "\"" + value.ToString() + "\"";
						}
						else
						{
							text = value.ToString();
						}
						stringBuilder.Append(propertyInfo.Name + ": " + text + "\n");
					}
				}
			}
			D.Log(ting.name + " info: " + stringBuilder.ToString());
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000E67C File Offset: 0x0000C87C
		private void Bugtalk(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			ting.Say(string.Concat(new object[]
			{
				"Timetable: ",
				ting.timetable.name,
				", finalTargetPosition: ",
				ting.finalTargetPosition,
				", talking: ",
				ting.talking,
				" sleeping: ",
				ting.sleeping
			}), "Bugtalk");
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000E70C File Offset: 0x0000C90C
		private void Kill(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			this._world.tingRunner.RemoveTing(ting.name);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000E744 File Offset: 0x0000C944
		private void DetachFromNavNode(string[] args)
		{
			Vehicle ting = this._world.tingRunner.GetTing<Vehicle>(args[0]);
			ting.currentNavNode = null;
			ting.nextNavNode = null;
			ting.speed = 0f;
			ting.StopAction();
			ting.masterProgram.StopAndReset();
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000E790 File Offset: 0x0000C990
		private void SetFieldToStringArray(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			string text = args[1];
			string[] array = new string[args.Length - 2];
			for (int i = 2; i < args.Length; i++)
			{
				array[i - 2] = args[i];
			}
			ting.table.SetValue<string[]>(ting.objectId, text, array);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000E7F0 File Offset: 0x0000C9F0
		private void SetFieldToFloat(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			string text = args[1];
			float num = Convert.ToSingle(args[2]);
			ting.table.SetValue<float>(ting.objectId, text, num);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000E834 File Offset: 0x0000CA34
		private void Pos(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			string text = args[1];
			if (this._world.tingRunner.HasTing(text))
			{
				Ting ting2 = this._world.tingRunner.GetTing(text);
				ting.position = ting2.position;
				ting.direction = ting2.direction;
				MimanGrimmApiDefinitions.ClearStuffIfItIsACharacter(ting);
				if (ting2 is Seat && ting is Character)
				{
					(ting as Character).Sit(ting2 as Seat);
				}
				else if (ting2 is Bed && ting is Character)
				{
					(ting as Character).LayInBed(ting2 as Bed);
				}
			}
			else
			{
				D.Log("Can't find Ting with name '" + text + "'");
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000E90C File Offset: 0x0000CB0C
		private void WorldPos(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			string text = args[1];
			int num = int.Parse(args[2]);
			int num2 = int.Parse(args[3]);
			ting.position = new WorldCoordinate(text, num, num2);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000E954 File Offset: 0x0000CB54
		private void CarefulPos(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			string text = args[1];
			if (this._world.tingRunner.HasTing(text))
			{
				Ting ting2 = this._world.tingRunner.GetTing(text);
				ting.position = ting2.position;
				ting.direction = ting2.direction;
			}
			else
			{
				D.Log("Can't find Room or Ting with name '" + text + "'");
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000E9D4 File Offset: 0x0000CBD4
		public void CarefulLayInBed(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			Bed ting2 = this._world.tingRunner.GetTing<Bed>(args[1]);
			ting2.exitPoint = 0;
			ting.LayInBed(ting2);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000EA18 File Offset: 0x0000CC18
		private static void ClearStuffIfItIsACharacter(Ting pTing)
		{
			if (pTing is Character)
			{
				Character character = pTing as Character;
				character.ClearState();
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000EA40 File Offset: 0x0000CC40
		private void Dir(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			Direction direction = Direction.RIGHT;
			string text = args[1].ToLower();
			switch (text)
			{
			case "up":
				direction = Direction.UP;
				break;
			case "down":
				direction = Direction.DOWN;
				break;
			case "left":
				direction = Direction.LEFT;
				break;
			case "right":
				direction = Direction.RIGHT;
				break;
			}
			ting.direction = direction;
			MimanGrimmApiDefinitions.ClearStuffIfItIsACharacter(ting);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000EB14 File Offset: 0x0000CD14
		private void Interact(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			Ting ting2 = this._world.tingRunner.GetTing(args[1]);
			if (ting.CanInteractWith(ting2))
			{
				ting.WalkToTingAndInteract(ting2);
			}
			else
			{
				D.Log(ting.name + " can't interact with " + ting2.name);
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000EB7C File Offset: 0x0000CD7C
		private void GetUpAndInteract(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			if (ting.sitting)
			{
				ting.GetUpFromSeat();
			}
			this.Interact(args);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000EBB8 File Offset: 0x0000CDB8
		private void InteractUsingHandItem(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			if (ting.handItem == null)
			{
				D.Log(ting + " can't InteractUsingHandItem since she/he has no hand item");
			}
			Ting ting2 = this._world.tingRunner.GetTing(args[1]);
			if (ting.handItem.CanInteractWith(ting2))
			{
				ting.WalkToTingAndUseHandItem(ting2);
			}
			else
			{
				D.Log(ting.name + " can't interact (using hand item) with " + ting2.name);
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000EC40 File Offset: 0x0000CE40
		private void Hack(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			MimanTing ting2 = this._world.tingRunner.GetTing<MimanTing>(args[1]);
			ting.WalkToTingAndHack(ting2);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000EC7C File Offset: 0x0000CE7C
		private void UseHandItem(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			if (ting.handItem != null)
			{
				ting.InteractWith(ting.handItem);
			}
			else
			{
				D.Log("No hand item to interact with");
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000ECC4 File Offset: 0x0000CEC4
		private void TakeOutItem(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			MimanTing ting2 = this._world.tingRunner.GetTing<MimanTing>(args[1]);
			if (ting.handItem == ting2)
			{
				D.Log(string.Concat(new object[] { ting, " is already holding ", ting2, ", no need to take it out of inventory" }));
				return;
			}
			if (ting.handItem != null)
			{
				ting.MoveHandItemToInventory();
			}
			ting.TakeOutInventoryItem(ting2);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000ED48 File Offset: 0x0000CF48
		private void PutIntoInventory(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			MimanTing mimanTing = this._world.tingRunner.GetTingUnsafe(args[1]) as MimanTing;
			if (mimanTing == null)
			{
				D.Log("Can't find " + args[1]);
				return;
			}
			mimanTing.position = new WorldCoordinate(ting.inventoryRoomName, 0, 0);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000EDB0 File Offset: 0x0000CFB0
		private void StartWaitForGift(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			ting.waitForGift = true;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000EDD8 File Offset: 0x0000CFD8
		private void StopWaitForGift(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			ting.waitForGift = false;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000EE00 File Offset: 0x0000D000
		private void Tase(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			ting.GetTased();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000EE28 File Offset: 0x0000D028
		private void GetTasedGently(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			ting.GetTasedGently();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000EE50 File Offset: 0x0000D050
		private void PickUp(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			Ting ting = this._world.tingRunner.GetTing(args[1]);
			if (ting.canBePickedUp)
			{
				character.PickUp(ting);
			}
			else
			{
				D.Log(character.name + " can't pick up " + ting.name);
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000EED4 File Offset: 0x0000D0D4
		private void Walk(string[] args)
		{
			Character characterFromFirstArg = this.GetCharacterFromFirstArg(args);
			string text = args[1];
			if (this._world.roomRunner.HasRoom(text))
			{
				int num = Convert.ToInt32(args[2]);
				int num2 = Convert.ToInt32(args[3]);
				characterFromFirstArg.WalkTo(new WorldCoordinate(text, num, num2));
			}
			else if (this._world.tingRunner.HasTing(text))
			{
				Ting ting = this._world.tingRunner.GetTing(text);
				characterFromFirstArg.WalkToTingAndInteract(ting);
			}
			else
			{
				D.Log("Can't find Room or Ting with name '" + text + "'");
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000EF74 File Offset: 0x0000D174
		private void GetUpFromSeat(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			character.GetUpFromSeat();
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		private void TurnLeft(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			character.TurnLeft();
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000EFFC File Offset: 0x0000D1FC
		private void GetAngryAtComputer(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			character.GetAngryAtComputer();
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000F040 File Offset: 0x0000D240
		private void Give(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			character.GiveHandItemToPerson();
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000F084 File Offset: 0x0000D284
		private void Load(string[] args)
		{
			this._world.settings.activeRoom = args[0];
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000F09C File Offset: 0x0000D29C
		private void SetClockSpeed(string[] args)
		{
			this._world.settings.gameTimeSpeed = (float)Convert.ToDouble(args[0]);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000F0B8 File Offset: 0x0000D2B8
		private void SetClock(string[] args)
		{
			int num = Convert.ToInt32(args[0]);
			int num2 = Convert.ToInt32(args[1]);
			int num3 = Convert.ToInt32(args[2]);
			int num4 = Convert.ToInt32(args[3]);
			GameTime gameTime = new GameTime(num, num2, num3, (float)num4);
			this._world.settings.gameTimeSeconds = gameTime.totalSeconds;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000F10C File Offset: 0x0000D30C
		private void SetHour(string[] args)
		{
			int num = 0;
			int num2 = Convert.ToInt32(args[0]);
			int num3 = 0;
			int num4 = 0;
			GameTime gameTime = new GameTime(num, num2, num3, (float)num4);
			this._world.settings.gameTimeSeconds = gameTime.totalSeconds;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000F14C File Offset: 0x0000D34C
		private void God(string[] args)
		{
			Character character = this._world.tingRunner.GetTing("Sebastian") as Character;
			Teleporter teleporter = this._world.tingRunner.CreateTing<Teleporter>("GodModeTeleporter", new WorldCoordinate(character.inventoryRoomName, 0, 0));
			teleporter.prefab = "Teleporter";
			Extractor extractor = this._world.tingRunner.CreateTing<Extractor>("GodModeExtractor", new WorldCoordinate(character.inventoryRoomName, 0, 0));
			extractor.prefab = "Extractor";
			Radio radio = this._world.tingRunner.CreateTing<Radio>("GodModeRadio", new WorldCoordinate(character.inventoryRoomName, 0, 0));
			radio.prefab = "Radio";
			MusicBox musicBox = this._world.tingRunner.CreateTing<MusicBox>("GodModeMusicBox", new WorldCoordinate(character.inventoryRoomName, 0, 0));
			musicBox.prefab = "MusicBox";
			Drink drink = this._world.tingRunner.CreateTing<Drink>("EmergencyBeer", new WorldCoordinate(character.inventoryRoomName, 0, 0));
			drink.prefab = "Beer";
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000F260 File Offset: 0x0000D460
		private void SetAvatar(string[] args)
		{
			this._world.settings.avatarName = args[0];
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000F278 File Offset: 0x0000D478
		private void FindAvatar(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(this._world.settings.avatarName);
			this._world.settings.activeRoom = ting.room.name;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000F2C4 File Offset: 0x0000D4C4
		private void LockDoor(string[] args)
		{
			Door ting = this._world.tingRunner.GetTing<Door>(args[0]);
			ting.isLocked = true;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000F2EC File Offset: 0x0000D4EC
		private void UnlockDoor(string[] args)
		{
			Door ting = this._world.tingRunner.GetTing<Door>(args[0]);
			ting.isLocked = false;
			ting.autoLockTimer = 0f;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000F320 File Offset: 0x0000D520
		private void UseForRoomPathfinding(string[] args)
		{
			Door ting = this._world.tingRunner.GetTing<Door>(args[0]);
			ting.useForRoomPathfinding = bool.Parse(args[1]);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000F350 File Offset: 0x0000D550
		private void MuteNotifications(string[] args)
		{
			this._world.settings.muteNotifications = bool.Parse(args[0]);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000F36C File Offset: 0x0000D56C
		private void StartRinging(string[] args)
		{
			Telephone ting = this._world.tingRunner.GetTing<Telephone>(args[0]);
			ting.ringing = true;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000F394 File Offset: 0x0000D594
		private void StartAllRadios(string[] args)
		{
			Radio[] tingsOfType = this._world.tingRunner.GetTingsOfType<Radio>();
			foreach (Radio radio in tingsOfType)
			{
				radio.masterProgram.Start();
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000F3D8 File Offset: 0x0000D5D8
		private void StartAllFuseboxes(string[] args)
		{
			FuseBox[] tingsOfType = this._world.tingRunner.GetTingsOfType<FuseBox>();
			foreach (FuseBox fuseBox in tingsOfType)
			{
				fuseBox.masterProgram.Start();
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000F41C File Offset: 0x0000D61C
		private void StartMusic(string[] args)
		{
			MusicBox ting = this._world.tingRunner.GetTing<MusicBox>(args[0]);
			ting.isPlaying = true;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000F444 File Offset: 0x0000D644
		private void StopMusic(string[] args)
		{
			MusicBox ting = this._world.tingRunner.GetTing<MusicBox>(args[0]);
			ting.isPlaying = false;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000F46C File Offset: 0x0000D66C
		private void SetChannelOnRadio(string[] args)
		{
			Radio ting = this._world.tingRunner.GetTing<Radio>(args[0]);
			ting.API_SetChannel((float)int.Parse(args[1]));
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000F49C File Offset: 0x0000D69C
		private void SetDoorTarget(string[] args)
		{
			Door ting = this._world.tingRunner.GetTing<Door>(args[0]);
			if (args[1] != "")
			{
				Door ting2 = this._world.tingRunner.GetTing<Door>(args[1]);
				ting.targetDoorName = ting2.name;
			}
			else
			{
				ting.targetDoorName = "";
			}
			ting.SetSourceCodeFromDoorTarget();
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000F508 File Offset: 0x0000D708
		private void Sleep(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			int num = Convert.ToInt32(args[1]);
			character.FallAsleepFromStanding(num);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000F558 File Offset: 0x0000D758
		private void BeBored(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			character.BeBored();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000F59C File Offset: 0x0000D79C
		private void WakeUp(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			character.StopAction();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000F5E0 File Offset: 0x0000D7E0
		private void SleepUntil(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			int num = Convert.ToInt32(args[1]);
			int hours = this._world.settings.gameTimeClock.hours;
			int num2;
			if (num > hours)
			{
				num2 = num - hours;
			}
			else
			{
				num2 = 24 - hours + num;
			}
			character.FallAsleepFromStanding(num2);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000F664 File Offset: 0x0000D864
		private void StartAction(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			character.StartAction(args[1], null, 0f, 1f);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000F6B8 File Offset: 0x0000D8B8
		private void StopAction(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			character.StopAction();
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000F6FC File Offset: 0x0000D8FC
		private void CancelWalking(string[] args)
		{
			this.GetCharacterFromFirstArg(args).CancelWalking();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000F70C File Offset: 0x0000D90C
		private void SetFriendLevel(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			int num = Convert.ToInt32(args[1]);
			character.friendLevel = num;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000F75C File Offset: 0x0000D95C
		private void SetCorruption(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			float num = Convert.ToSingle(args[1]);
			character.corruption = num;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000F7AC File Offset: 0x0000D9AC
		private Character GetCharacterFromFirstArg(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			return character;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000F7EC File Offset: 0x0000D9EC
		private void StartTalking(string[] args)
		{
			this.GetCharacterFromFirstArg(args).StartTalking();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000F7FC File Offset: 0x0000D9FC
		private void StopTalking(string[] args)
		{
			this.GetCharacterFromFirstArg(args).StopTalking();
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000F80C File Offset: 0x0000DA0C
		private void SetKnowledge(string[] args)
		{
			this.GetCharacterFromFirstArg(args).SetKnowledge(args[1]);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000F820 File Offset: 0x0000DA20
		private void SetCameraAutoRotateSpeed(string[] args)
		{
			this._world.settings.cameraAutoRotateSpeed = Convert.ToSingle(args[0]);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000F83C File Offset: 0x0000DA3C
		private void SetCameraTarget(string[] args)
		{
			if (this._world.settings.onCameraTarget != null)
			{
				this._world.settings.onCameraTarget(args[0], args[1]);
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000F87C File Offset: 0x0000DA7C
		private void Beat(string[] args)
		{
			this._world.settings.beaten = true;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000F890 File Offset: 0x0000DA90
		private void HeartIsBroken(string[] args)
		{
			this._world.settings.heartIsBroken = true;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000F8A4 File Offset: 0x0000DAA4
		private void SetAllComputersToRunProgram(string[] args)
		{
			SourceCode sourceCode = this._world.sourceCodeDispenser.GetSourceCode(args[0]);
			Console.WriteLine("SET ALL COMPUTERS TO RUN PROGRAM: " + sourceCode.content);
			Computer[] tingsOfType = this._world.tingRunner.GetTingsOfType<Computer>();
			foreach (Computer computer in tingsOfType)
			{
				computer.hasInternetAPI = true;
				computer.masterProgram.sourceCodeContent = sourceCode.content;
				computer.masterProgram.StopAndReset();
				computer.RunProgram(null);
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000F934 File Offset: 0x0000DB34
		private void SetTimetable(string[] args)
		{
			if (args.Length != 2)
			{
				D.LogError("The number of args to SetTimetable is incorrect: " + args.Length);
			}
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			character.timetableName = args[1];
			character.timetableTimer = 0.5f;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000F9A8 File Offset: 0x0000DBA8
		private void SetTimetableTimer(string[] args)
		{
			if (args.Length != 2)
			{
				D.LogError("The number of args to SetTimetableTimer is incorrect: " + args.Length);
			}
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			character.timetableTimer = float.Parse(args[1]);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000FA14 File Offset: 0x0000DC14
		private void SetNeverGetsTired(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			character.neverGetsTired = bool.Parse(args[1]);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000FA60 File Offset: 0x0000DC60
		private void CreateCharacter(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[1]);
			this._world.tingRunner.CreateTing<Character>(args[0], ting.position, Direction.DOWN, "Sebastian");
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
		private void CreateCigarette(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			Cigarette cigarette = this._world.tingRunner.CreateTing<Cigarette>(args[1], new WorldCoordinate(ting.inventoryRoomName, IntPoint.Zero), Direction.DOWN, "Tagg_Cigarrette");
			cigarette.masterProgramName = "Cigarette";
			cigarette.drugType = "cigarette";
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000FB08 File Offset: 0x0000DD08
		private void CreateBeerInHand(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			string text = string.Concat(new object[]
			{
				"Beer_",
				ting.name,
				"_",
				this._world.settings.tickNr
			});
			Drink drink = this._world.tingRunner.CreateTing<Drink>(text, ting.position, Direction.DOWN, "Beer");
			drink.liquidType = "beer";
			ting.handItem = drink;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000FB98 File Offset: 0x0000DD98
		private void CreateDrinkInHand(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			string text = args[1];
			MimanTing mimanTing = Behaviour_Sell.CreateTingToSell(ting, this._world.tingRunner, text, this._world.settings);
			ting.handItem = mimanTing;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000FBE4 File Offset: 0x0000DDE4
		private void CreateCoffeeInHand(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			string text = string.Concat(new object[]
			{
				"Coffee_",
				ting.name,
				"_",
				this._world.settings.tickNr
			});
			Drink drink = this._world.tingRunner.CreateTingAfterUpdate<Drink>(text, ting.position, Direction.DOWN, "CupOfCoffee");
			drink.masterProgramName = "CafeCoffee";
			drink.liquidType = "coffee";
			drink.PrepareForBeingHacked();
			ting.handItem = drink;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000FC84 File Offset: 0x0000DE84
		private void SetHandItem(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			MimanTing ting2 = this._world.tingRunner.GetTing<MimanTing>(args[1]);
			if (ting2 != null)
			{
				ting.handItem = ting2;
			}
			else
			{
				D.Log("Can't find ting " + args[1] + " to put into hand of " + args[0]);
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000FCE8 File Offset: 0x0000DEE8
		private void DropHandItem(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			ting.DropHandItem();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000FD10 File Offset: 0x0000DF10
		private void ClearHandItem(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			ting.handItem = null;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000FD38 File Offset: 0x0000DF38
		private void PutAwayHandItem(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			ting.PutHandItemIntoInventory();
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000FD60 File Offset: 0x0000DF60
		private void SetRain(string[] args)
		{
			float num = Convert.ToSingle(args[0]);
			this._world.settings.rainTargetValue = num;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000FD88 File Offset: 0x0000DF88
		private void GetMasterProgramStatus(string[] args)
		{
			Computer computer = this._world.tingRunner.GetTing(args[0]) as Computer;
			D.Log("Information for program " + computer.masterProgram.name);
			D.Log("is on: " + computer.masterProgram.isOn);
			D.Log("waitingForInput: " + computer.masterProgram.waitingForInput);
			D.Log("compilationTurnedOn: " + computer.masterProgram.compilationTurnedOn);
			D.Log("Sleep timer: " + computer.masterProgram.sleepTimer);
			D.Log("Max execution time: " + computer.masterProgram.maxExecutionTime);
			D.Log("ContainsErrors: " + computer.masterProgram.ContainsErrors());
			D.Log("Execution time: " + computer.masterProgram.executionTime);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000FEA4 File Offset: 0x0000E0A4
		private void RunMasterProgram(string[] args)
		{
			MimanTing mimanTing = this._world.tingRunner.GetTing(args[0]) as MimanTing;
			if (mimanTing is Computer)
			{
				(mimanTing as Computer).RunProgram(null);
			}
			else if (mimanTing is Radio)
			{
				(mimanTing as Radio).masterProgram.Start();
			}
			else if (mimanTing is Pawn)
			{
				(mimanTing as Pawn).masterProgram.Start();
			}
			else
			{
				D.LogError("Can't run master program on " + args[0]);
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000FF38 File Offset: 0x0000E138
		private void RunMasterProgramOnAllComputersInRoom(string[] args)
		{
			Room room = this._world.roomRunner.GetRoom(args[0]);
			foreach (Computer computer in room.GetTingsOfType<Computer>())
			{
				computer.RunProgram(null);
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000FFB4 File Offset: 0x0000E1B4
		private void TurnOnTv(string[] args)
		{
			Tv tv = this._world.tingRunner.GetTing(args[0]) as Tv;
			tv.on = true;
			tv.masterProgram.Start();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000FFEC File Offset: 0x0000E1EC
		private void SetMemory(string[] args)
		{
			Memory ting = this._world.tingRunner.GetTing<Memory>(args[0]);
			ting.data.Add(args[1], args[2]);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00010020 File Offset: 0x0000E220
		private void SetCode(string[] args)
		{
			MimanTing ting = this._world.tingRunner.GetTing<MimanTing>(args[0]);
			SourceCode sourceCode = this._world.sourceCodeDispenser.GetSourceCode(args[1]);
			ting.masterProgram.sourceCodeContent = sourceCode.content;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00010068 File Offset: 0x0000E268
		private void SetCodeAndRun(string[] args)
		{
			MimanTing ting = this._world.tingRunner.GetTing<MimanTing>(args[0]);
			SourceCode sourceCode = this._world.sourceCodeDispenser.GetSourceCode(args[1]);
			ting.masterProgram.sourceCodeContent = sourceCode.content;
			ting.masterProgram.Start();
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000100BC File Offset: 0x0000E2BC
		private void SetResettableCode(string[] args)
		{
			MimanTing ting = this._world.tingRunner.GetTing<MimanTing>(args[0]);
			ting.masterProgram.sourceCodeName = args[1];
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000100EC File Offset: 0x0000E2EC
		private void GetOutput(string[] args)
		{
			Computer ting = this._world.tingRunner.GetTing<Computer>(args[0]);
			Console.ForegroundColor = ConsoleColor.Yellow;
			foreach (string text in ting.consoleOutput)
			{
				Console.WriteLine(text);
			}
			Console.ForegroundColor = ConsoleColor.White;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00010140 File Offset: 0x0000E340
		private void GetProgramErrors(string[] args)
		{
			Computer ting = this._world.tingRunner.GetTing<Computer>(args[0]);
			Console.ForegroundColor = ConsoleColor.Red;
			int num = 0;
			foreach (Error error in ting.masterProgram.GetErrors())
			{
				D.Log(num++ + ": " + error);
			}
			Console.ForegroundColor = ConsoleColor.White;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000101B4 File Offset: 0x0000E3B4
		private void GetSource(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			Console.ForegroundColor = ConsoleColor.Yellow;
			if (ting is CreditCard)
			{
				Console.WriteLine((ting as CreditCard).masterProgram.sourceCodeContent);
			}
			else if (ting is Computer)
			{
				Console.WriteLine((ting as Computer).masterProgram.sourceCodeContent);
			}
			else if (ting is Hackdev)
			{
				Console.WriteLine((ting as Hackdev).masterProgram.sourceCodeContent);
			}
			else if (ting is Door)
			{
				Console.WriteLine((ting as Door).masterProgram.sourceCodeContent);
			}
			else
			{
				D.Log("Can't print source of tings with type " + ting.GetType());
			}
			Console.ForegroundColor = ConsoleColor.White;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0001028C File Offset: 0x0000E48C
		private void RunFunctionOnComputer(string[] args)
		{
			Computer computer = this._world.tingRunner.GetTing(args[0]) as Computer;
			List<object> list = new List<object>();
			for (int i = 2; i < args.Length; i++)
			{
				list.Add(args[i]);
			}
			if (computer.masterProgram != null)
			{
				computer.masterProgram.StopAndReset();
			}
			computer.masterProgram.maxExecutionTime = 10f;
			computer.masterProgram.StartAtFunction(args[1], list.ToArray(), null);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00010310 File Offset: 0x0000E510
		private void RunMakeTransactionFunctionOnCreditCard(string[] args)
		{
			CreditCard ting = this._world.tingRunner.GetTing<CreditCard>(args[0]);
			if (ting == null)
			{
				D.Log(args[0] + " is not a CreditCard");
				return;
			}
			float num = Convert.ToSingle(args[1]);
			ting.RunMakeTransactionFunction(num);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0001035C File Offset: 0x0000E55C
		private void CharacterTakesMoneyFromCreditCard(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			if (ting.handItem == null)
			{
				D.Log("Can't take money from credit card, " + ting + " has no hand item!");
			}
			float num = Convert.ToSingle(args[1]);
			D.Log(string.Concat(new object[] { ting, " will take $", num, " from the credit card ", ting.handItem }));
			ting.creditCardUsageAmount = num;
			ting.InteractWith(ting.handItem);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000103F0 File Offset: 0x0000E5F0
		private void CheckMoney(string[] args)
		{
			object obj = 0f;
			Computer computer = this._world.tingRunner.GetTing("FinanceComputer") as Computer;
			if (computer == null)
			{
				D.Log("No finance computer");
				return;
			}
			if (!computer.memory.data.TryGetValue(args[0], out obj))
			{
				D.Log("No entry found");
				return;
			}
			if (obj == null)
			{
				D.Log("cashAmount is null");
				return;
			}
			D.Log(string.Concat(new object[]
			{
				"Cash amount: ",
				obj,
				", of type ",
				obj.GetType()
			}));
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0001049C File Offset: 0x0000E69C
		public void RegisterExpressions()
		{
			this._world.dialogueRunner.AddExpression("InSameRoom", new DialogueRunner.Expression(this.InSameRoom));
			this._world.dialogueRunner.AddExpression("NotInSameRoom", new DialogueRunner.Expression(this.NotInSameRoom));
			this._world.dialogueRunner.AddExpression("IsInRoom", new DialogueRunner.Expression(this.IsInRoom));
			this._world.dialogueRunner.AddExpression("RoomIsEmpty", new DialogueRunner.Expression(this.RoomIsEmpty));
			this._world.dialogueRunner.AddExpression("IsInEitherRoom", new DialogueRunner.Expression(this.IsInEitherRoom));
			this._world.dialogueRunner.AddExpression("IsNotInRoom", new DialogueRunner.Expression(this.IsNotInRoom));
			this._world.dialogueRunner.AddExpression("IsNotSlurped", new DialogueRunner.Expression(this.IsNotSlurped));
			this._world.dialogueRunner.AddExpression("IsIdle", new DialogueRunner.Expression(this.IsIdle));
			this._world.dialogueRunner.AddExpression("IsSitting", new DialogueRunner.Expression(this.IsSitting));
			this._world.dialogueRunner.AddExpression("IsLaying", new DialogueRunner.Expression(this.IsLaying));
			this._world.dialogueRunner.AddExpression("IsSleepingOnGround", new DialogueRunner.Expression(this.IsSleepingOnGround));
			this._world.dialogueRunner.AddExpression("IsStanding", new DialogueRunner.Expression(this.IsStanding));
			this._world.dialogueRunner.AddExpression("IsAtPosition", new DialogueRunner.Expression(this.IsAtPosition));
			this._world.dialogueRunner.AddExpression("IsAtTing", new DialogueRunner.Expression(this.IsAtTing));
			this._world.dialogueRunner.AddExpression("IsHour", new DialogueRunner.Expression(this.IsHour));
			this._world.dialogueRunner.AddExpression("IsOverHour", new DialogueRunner.Expression(this.IsOverHour));
			this._world.dialogueRunner.AddExpression("IsBeforeHour", new DialogueRunner.Expression(this.IsBeforeHour));
			this._world.dialogueRunner.AddExpression("IsBetweenHours", new DialogueRunner.Expression(this.IsBetweenHours));
			this._world.dialogueRunner.AddExpression("IsDay", new DialogueRunner.Expression(this.IsDay));
			this._world.dialogueRunner.AddExpression("IsNight", new DialogueRunner.Expression(this.IsNight));
			this._world.dialogueRunner.AddExpression("IsAwake", new DialogueRunner.Expression(this.IsAwake));
			this._world.dialogueRunner.AddExpression("IsSleeping", new DialogueRunner.Expression(this.IsSleeping));
			this._world.dialogueRunner.AddExpression("IsHacking", new DialogueRunner.Expression(this.IsHacking));
			this._world.dialogueRunner.AddExpression("IsUsingComputer", new DialogueRunner.Expression(this.IsUsingComputer));
			this._world.dialogueRunner.AddExpression("Is", new DialogueRunner.Expression(this.Is));
			this._world.dialogueRunner.AddExpression("OneIn", new DialogueRunner.Expression(this.OneIn));
			this._world.dialogueRunner.AddExpression("IsDrunk", new DialogueRunner.Expression(this.IsDrunk));
			this._world.dialogueRunner.AddExpression("HasFriendLevel", new DialogueRunner.Expression(this.HasFriendLevel));
			this._world.dialogueRunner.AddExpression("False", (string[] args) => false);
			this._world.dialogueRunner.AddExpression("NotTalking", new DialogueRunner.Expression(this.NotTalking));
			this._world.dialogueRunner.AddExpression("NotTalkingButIgnore", new DialogueRunner.Expression(this.NotTalkingButIgnore));
			this._world.dialogueRunner.AddExpression("HasConversationTarget", new DialogueRunner.Expression(this.HasConversationTarget));
			this._world.dialogueRunner.AddExpression("HasKnowledge", new DialogueRunner.Expression(this.HasKnowledge));
			this._world.dialogueRunner.AddExpression("IsAtTask", new DialogueRunner.Expression(this.IsAtTask));
			this._world.dialogueRunner.AddExpression("IsNotAtTask", new DialogueRunner.Expression(this.IsNotAtTask));
			this._world.dialogueRunner.AddExpression("IsWithinDistance", new DialogueRunner.Expression(this.IsWithinDistance));
			this._world.dialogueRunner.AddExpression("IsOutsideDistance", new DialogueRunner.Expression(this.IsOutsideDistance));
			this._world.dialogueRunner.AddExpression("IsFieldGreaterThan", new DialogueRunner.Expression(this.IsFieldGreaterThan));
			this._world.dialogueRunner.AddExpression("IsFieldLessThan", new DialogueRunner.Expression(this.IsFieldLessThan));
			this._world.dialogueRunner.AddExpression("HasSoda", new DialogueRunner.Expression(this.HasSoda));
			this._world.dialogueRunner.AddExpression("HasHandItemWithName", new DialogueRunner.Expression(this.HasHandItemWithName));
			this._world.dialogueRunner.AddExpression("HasHandItemOfType", new DialogueRunner.Expression(this.HasHandItemOfType));
			this._world.dialogueRunner.AddExpression("HasHandItemOfDrinkType", new DialogueRunner.Expression(this.HasHandItemOfDrinkType));
			this._world.dialogueRunner.AddExpression("HasAnyHandItem", new DialogueRunner.Expression(this.HasAnyHandItem));
			this._world.dialogueRunner.AddExpression("HasNoHandItem", new DialogueRunner.Expression(this.HasNoHandItem));
			this._world.dialogueRunner.AddExpression("HasItemOfType", new DialogueRunner.Expression(this.HasItemOfType));
			this._world.dialogueRunner.AddExpression("HasDrinkOfType", new DialogueRunner.Expression(this.HasDrinkOfType));
			this._world.dialogueRunner.AddExpression("IsPawnAlive", new DialogueRunner.Expression(this.IsPawnAlive));
			this._world.dialogueRunner.AddExpression("IsPawnDead", new DialogueRunner.Expression(this.IsPawnDead));
			this._world.dialogueRunner.AddExpression("HasMadeMoreMovesThan", new DialogueRunner.Expression(this.HasMadeMoreMovesThan));
			this._world.dialogueRunner.AddExpression("HasNoErrors", new DialogueRunner.Expression(this.HasNoErrors));
			this._world.dialogueRunner.AddExpression("HasErrors", new DialogueRunner.Expression(this.HasErrors));
			this._world.dialogueRunner.AddExpression("IsCubeGlowing", new DialogueRunner.Expression(this.IsCubeGlowing));
			this._world.dialogueRunner.AddExpression("IsDebug", new DialogueRunner.Expression(this.IsDebug));
			this._world.dialogueRunner.AddExpression("IsNotDebug", new DialogueRunner.Expression(this.IsNotDebug));
			this._world.dialogueRunner.AddExpression("BoolIsTrue", new DialogueRunner.Expression(this.BoolIsTrue));
			this._world.dialogueRunner.AddExpression("IsUnlocked", new DialogueRunner.Expression(this.IsUnlocked));
			this._world.dialogueRunner.AddExpression("IsTvOff", new DialogueRunner.Expression(this.IsTvOff));
			this._world.dialogueRunner.AddExpression("IsBeaten", new DialogueRunner.Expression(this.IsBeaten));
			this._world.dialogueRunner.AddExpression("IsProgramStopped", new DialogueRunner.Expression(this.IsProgramStopped));
			this._world.dialogueRunner.AddExpression("DrinkInHandIsAlmostEmpty", new DialogueRunner.Expression(this.DrinkInHandIsAlmostEmpty));
			this._world.dialogueRunner.AddExpression("CharacterHasLessMoneyThan", new DialogueRunner.Expression(this.CharacterHasLessMoneyThan));
			this._world.dialogueRunner.AddExpression("Cheat", new DialogueRunner.Expression(this.Cheat));
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00010CDC File Offset: 0x0000EEDC
		private bool Cheat(string[] args)
		{
			if (args.Length == 0)
			{
				return MimanGrimmApiDefinitions.cheat != "";
			}
			return MimanGrimmApiDefinitions.cheat == args[0];
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00010D04 File Offset: 0x0000EF04
		private bool CharacterHasLessMoneyThan(string[] args)
		{
			float num = Convert.ToSingle(args[1]);
			Computer computer = this._world.tingRunner.GetTingUnsafe("FinanceComputer") as Computer;
			object obj = 0f;
			bool flag = false;
			if (computer != null)
			{
				flag = computer.memory.data.TryGetValue(args[0], out obj);
			}
			D.Log(string.Concat(new object[]
			{
				"Checking if ",
				args[0],
				" has less money than ",
				args[1],
				", he/she has: $",
				obj
			}));
			if (flag)
			{
				return (float)obj < num;
			}
			D.Log("Can't find bank entry for " + args[0]);
			return true;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00010DBC File Offset: 0x0000EFBC
		private bool IsDebug(string[] args)
		{
			return true;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00010DC0 File Offset: 0x0000EFC0
		private bool IsNotDebug(string[] args)
		{
			return false;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00010DC4 File Offset: 0x0000EFC4
		private bool IsUnlocked(string[] args)
		{
			Door door = this._world.tingRunner.GetTing(args[0]) as Door;
			return !door.isLocked;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00010DF4 File Offset: 0x0000EFF4
		private bool IsTvOff(string[] args)
		{
			Tv tv = this._world.tingRunner.GetTing(args[0]) as Tv;
			return !tv.on;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00010E24 File Offset: 0x0000F024
		private bool IsBeaten(string[] args)
		{
			return this._world.settings.beaten;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00010E38 File Offset: 0x0000F038
		private bool IsProgramStopped(string[] args)
		{
			MimanTing mimanTing = this._world.tingRunner.GetTing(args[0]) as MimanTing;
			Program masterProgram = mimanTing.masterProgram;
			return !masterProgram.isOn;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00010E70 File Offset: 0x0000F070
		private bool BoolIsTrue(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			return ting.table.GetValue<bool>(ting.objectId, args[1]);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00010EA8 File Offset: 0x0000F0A8
		private bool InSameRoom(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			Ting ting2 = this._world.tingRunner.GetTing(args[1]);
			return ting.room == ting2.room;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00010EEC File Offset: 0x0000F0EC
		private bool NotInSameRoom(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			Ting ting2 = this._world.tingRunner.GetTing(args[1]);
			return ting.room != ting2.room;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00010F34 File Offset: 0x0000F134
		private bool IsInRoom(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			string text = args[1];
			return ting.room.name == text;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00010F6C File Offset: 0x0000F16C
		private bool RoomIsEmpty(string[] args)
		{
			List<Character> tingsOfType = this._world.roomRunner.GetRoom(args[0]).GetTingsOfType<Character>();
			return tingsOfType.Count == 0;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00010F9C File Offset: 0x0000F19C
		private bool IsInEitherRoom(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			string name = ting.room.name;
			foreach (string text in args)
			{
				if (name == text)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00010FF0 File Offset: 0x0000F1F0
		private bool IsNotSlurped(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			return character.actionName != "InsideComputer";
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00011028 File Offset: 0x0000F228
		private bool IsNotInRoom(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			string text = args[1];
			return ting.room.name != text;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00011060 File Offset: 0x0000F260
		private bool IsIdle(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			return ting.IsIdle();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00011088 File Offset: 0x0000F288
		private bool IsSitting(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			return ting.sitting;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000110B0 File Offset: 0x0000F2B0
		private bool IsLaying(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			return ting.laying;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000110D8 File Offset: 0x0000F2D8
		private bool IsAwake(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			return character.actionName != "Sleeping";
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00011110 File Offset: 0x0000F310
		private bool IsSleeping(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			return character.actionName == "Sleeping";
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00011148 File Offset: 0x0000F348
		private bool IsSleepingOnGround(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			return character.actionName == "Sleeping" && character.bed == null;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00011190 File Offset: 0x0000F390
		private bool IsStanding(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			return !character.sitting && !character.laying;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000111D0 File Offset: 0x0000F3D0
		private bool IsHacking(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			return character.actionName == "Hacking";
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00011208 File Offset: 0x0000F408
		private bool IsUsingComputer(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			return character.actionName == "UsingComputer";
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00011240 File Offset: 0x0000F440
		private bool Is(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			return character.actionName == args[1];
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00011274 File Offset: 0x0000F474
		private bool IsDrunk(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			int num = Convert.ToInt32(args[1]);
			return character.drunkenness >= (float)num;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000112B0 File Offset: 0x0000F4B0
		private bool IsAtPosition(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			int num = Convert.ToInt32(args[1]);
			int num2 = Convert.ToInt32(args[2]);
			IntPoint intPoint = new IntPoint(num, num2);
			return ting.localPoint == intPoint;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000112F8 File Offset: 0x0000F4F8
		private bool IsAtTing(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			Ting ting2 = this._world.tingRunner.GetTing(args[1]);
			return ting.position == ting2.position;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00011340 File Offset: 0x0000F540
		private bool IsHour(string[] args)
		{
			int num = Convert.ToInt32(args[0]);
			return this._world.settings.gameTimeClock.hours == num;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00011374 File Offset: 0x0000F574
		private bool IsOverHour(string[] args)
		{
			int num = Convert.ToInt32(args[0]);
			return this._world.settings.gameTimeClock.hours >= num;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000113A8 File Offset: 0x0000F5A8
		private bool IsBeforeHour(string[] args)
		{
			int num = Convert.ToInt32(args[0]);
			return this._world.settings.gameTimeClock.hours < num;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000113DC File Offset: 0x0000F5DC
		private bool IsBetweenHours(string[] args)
		{
			int num = Convert.ToInt32(args[0]);
			int num2 = Convert.ToInt32(args[1]);
			return this._world.settings.gameTimeClock.hours >= num && this._world.settings.gameTimeClock.hours < num2;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00011438 File Offset: 0x0000F638
		private bool IsDay(string[] args)
		{
			int hours = this._world.settings.gameTimeClock.hours;
			return hours >= 6 && hours <= 22;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00011470 File Offset: 0x0000F670
		private bool IsNight(string[] args)
		{
			return !this.IsDay(args);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0001147C File Offset: 0x0000F67C
		private bool OneIn(string[] args)
		{
			int num = Convert.ToInt32(args[0]);
			return MimanGrimmApiDefinitions.r.Next() % num == 0;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000114A4 File Offset: 0x0000F6A4
		private bool HasFriendLevel(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			int num = Convert.ToInt32(args[1]);
			return character.friendLevel == num;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000114DC File Offset: 0x0000F6DC
		private bool NotTalking(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			return character.actionName != "Talking" && character.conversationTarget == null;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00011524 File Offset: 0x0000F724
		private bool NotTalkingButIgnore(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			return character.actionName != "Talking" && (character.conversationTarget == null || character.conversationTarget.name == args[1]);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00011584 File Offset: 0x0000F784
		private bool HasConversationTarget(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			Character character2 = this._world.tingRunner.GetTing(args[1]) as Character;
			return character.conversationTarget == character2;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x000115CC File Offset: 0x0000F7CC
		public bool HasKnowledge(string[] args)
		{
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			return character.HasKnowledge(args[1]);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00011614 File Offset: 0x0000F814
		private bool IsAtTask(string[] args)
		{
			if (args.Length != 2)
			{
				D.LogError("Wrong nr of args to IsAtTask: " + args.Length);
			}
			Character character = this._world.tingRunner.GetTing(args[0]) as Character;
			if (character == null)
			{
				D.Log(args[0] + " is not a Character");
			}
			string text = args[1];
			return character.IsAtTimetableTask(text);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00011680 File Offset: 0x0000F880
		private bool IsNotAtTask(string[] args)
		{
			return !this.IsAtTask(args);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0001168C File Offset: 0x0000F88C
		private bool IsWithinDistance(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			Ting ting2 = this._world.tingRunner.GetTing(args[1]);
			int num = Convert.ToInt32(args[2]);
			return MimanGrimmApiDefinitions.AreTingsWithinDistance(ting, ting2, num);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000116D4 File Offset: 0x0000F8D4
		private bool IsOutsideDistance(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			Ting ting2 = this._world.tingRunner.GetTing(args[1]);
			int num = Convert.ToInt32(args[2]);
			return !MimanGrimmApiDefinitions.AreTingsWithinDistance(ting, ting2, num);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00011720 File Offset: 0x0000F920
		public static bool AreTingsWithinDistance(Ting t1, Ting t2, int pDistance)
		{
			if (t1.room != t2.room)
			{
				return false;
			}
			int num = t1.localPoint.x - t2.localPoint.x;
			int num2 = t1.localPoint.y - t2.localPoint.y;
			return num * num + num2 * num2 < pDistance * pDistance;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00011790 File Offset: 0x0000F990
		private bool IsFieldGreaterThan(string[] args)
		{
			Ting ting = this._world.tingRunner.GetTing(args[0]);
			string text = args[1];
			float value = ting.table.GetValue<float>(ting.objectId, text);
			float num = Convert.ToSingle(args[2]);
			return value > num;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000117D8 File Offset: 0x0000F9D8
		private bool IsFieldLessThan(string[] args)
		{
			return !this.IsFieldGreaterThan(args);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000117E4 File Offset: 0x0000F9E4
		private bool HasSoda(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			if (ting.handItem is Drink && (ting.handItem as Drink).liquidType == "soda")
			{
				return true;
			}
			Drink[] tingsOfTypeInRoom = this._world.tingRunner.GetTingsOfTypeInRoom<Drink>(ting.inventoryRoomName);
			foreach (Drink drink in tingsOfTypeInRoom)
			{
				if (drink.liquidType == "soda")
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00011884 File Offset: 0x0000FA84
		private bool HasHandItemWithName(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			return ting.handItem != null && ting.handItem.name == args[1];
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000118C8 File Offset: 0x0000FAC8
		private bool HasHandItemOfType(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			if (ting.handItem == null)
			{
				return false;
			}
			string text = ting.handItem.GetType().Name.ToString();
			return text == args[1];
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00011918 File Offset: 0x0000FB18
		private bool HasHandItemOfDrinkType(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			Drink drink = ting.handItem as Drink;
			if (drink == null)
			{
				return false;
			}
			string text = args[1].ToLower();
			return drink.liquidType.ToLower().Contains(text);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00011968 File Offset: 0x0000FB68
		private bool HasAnyHandItem(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			return ting.handItem != null;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00011998 File Offset: 0x0000FB98
		private bool HasNoHandItem(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			return ting.handItem == null;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x000119C4 File Offset: 0x0000FBC4
		private bool HasItemOfType(string[] args)
		{
			if (this.HasHandItemOfType(args))
			{
				return true;
			}
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			return ting.HasInventoryItemOfType(args[1]);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x000119FC File Offset: 0x0000FBFC
		private bool DrinkInHandIsAlmostEmpty(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			if (ting.handItem is Drink)
			{
				Drink drink = ting.handItem as Drink;
				return drink.amount < 10f;
			}
			D.Log("DrinkInHandIsAlmostEmpty doesn't work because " + ting.name + " isn't holding a drink.");
			return false;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00011A64 File Offset: 0x0000FC64
		private bool HasDrinkOfType(string[] args)
		{
			Character ting = this._world.tingRunner.GetTing<Character>(args[0]);
			string text = args[1].ToLower();
			Drink drink = ting.handItem as Drink;
			if (drink != null && drink.liquidType.ToLower().Contains(text))
			{
				return true;
			}
			foreach (Ting ting2 in ting.inventoryItems)
			{
				Drink drink2 = ting2 as Drink;
				if (drink2 != null && drink2.liquidType.ToLower().Contains(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00011B08 File Offset: 0x0000FD08
		private bool IsPawnDead(string[] args)
		{
			Pawn ting = this._world.tingRunner.GetTing<Pawn>(args[0]);
			D.Log(string.Concat(new object[] { "Checking if ", ting.name, " is dead: ", ting.dead }));
			return ting.dead;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00011B68 File Offset: 0x0000FD68
		private bool IsPawnAlive(string[] args)
		{
			Pawn ting = this._world.tingRunner.GetTing<Pawn>(args[0]);
			return !ting.dead;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00011B94 File Offset: 0x0000FD94
		private bool HasMadeMoreMovesThan(string[] args)
		{
			Pawn ting = this._world.tingRunner.GetTing<Pawn>(args[0]);
			int num = Convert.ToInt32(args[1]);
			return ting.moveNr > num;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00011BC8 File Offset: 0x0000FDC8
		private bool HasErrors(string[] args)
		{
			MimanTing ting = this._world.tingRunner.GetTing<MimanTing>(args[0]);
			return ting.containsBrokenPrograms;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00011BF0 File Offset: 0x0000FDF0
		private bool HasNoErrors(string[] args)
		{
			MimanTing ting = this._world.tingRunner.GetTing<MimanTing>(args[0]);
			return !ting.containsBrokenPrograms;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00011C1C File Offset: 0x0000FE1C
		private bool IsCubeGlowing(string[] args)
		{
			MysticalCube ting = this._world.tingRunner.GetTing<MysticalCube>(args[0]);
			return ting.color.x > 0f || ting.color.y > 0f || ting.color.z > 0f;
		}

		// Token: 0x040000A5 RID: 165
		private World _world;

		// Token: 0x040000A6 RID: 166
		private static ShowInEditor SHOW_IN_EDITOR = new ShowInEditor();

		// Token: 0x040000A7 RID: 167
		private static EditableInEditor EDITABLE_IN_EDITOR = new EditableInEditor();

		// Token: 0x040000A8 RID: 168
		public static string cheat = "";

		// Token: 0x040000A9 RID: 169
		public Action<string> onRemoveDanglingDialogueOptions;

		// Token: 0x040000AA RID: 170
		private static Random r = new Random();
	}
}
