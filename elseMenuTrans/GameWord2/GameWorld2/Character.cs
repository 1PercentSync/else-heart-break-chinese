using System;
using System.Collections.Generic;
using GameTypes;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000007 RID: 7
	public class Character : MimanTing
	{
		// Token: 0x06000065 RID: 101 RVA: 0x0000339C File Offset: 0x0000159C
		public override bool DoesMasterProgramExist()
		{
			return false;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000033A0 File Offset: 0x000015A0
		public override void Init()
		{
			base.Init();
			if (!this._roomRunner.HasRoom(this.inventoryRoomName))
			{
				SimpleRoomBuilder simpleRoomBuilder = new SimpleRoomBuilder(this._roomRunner);
				simpleRoomBuilder.CreateRoomWithSize(this.inventoryRoomName, 1, 1);
			}
			if (this.timetableName != "")
			{
				this.RefreshTimetable();
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003400 File Offset: 0x00001600
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_handItemObjectName = base.EnsureCell<string>("handItemObjectName", "");
			this.CELL_finalTargetPosition = base.EnsureCell<WorldCoordinate>("finalTargetPosition", WorldCoordinate.NONE);
			this.CELL_finalTargetTing = base.EnsureCell<string>("finalTargetTing", "");
			this.CELL_targetPositionInRoom = base.EnsureCell<WorldCoordinate>("targetPositionInRoom", WorldCoordinate.NONE);
			this.CELL_walkMode = base.EnsureCell<Character.WalkMode>("walkMode", Character.WalkMode.NO_TARGET);
			this.CELL_walkSpeed = base.EnsureCell<float>("walkSpeed", 4f);
			this.CELL_walkTimer = base.EnsureCell<float>("walkTimer", 0f);
			this.CELL_walkIterator = base.EnsureCell<int>("walkIterator", -1);
			this.CELL_charisma = base.EnsureCell<float>("charisma", 0f);
			this.CELL_smelliness = base.EnsureCell<float>("smelliness", 0f);
			this.CELL_sleepiness = base.EnsureCell<float>("sleepiness", 0f);
			this.CELL_drunkenness = base.EnsureCell<float>("drunkenness", 0f);
			this.CELL_happiness = base.EnsureCell<float>("happiness", 0f);
			this.CELL_supremacy = base.EnsureCell<float>("supremacy", 0f);
			this.CELL_alarmTime = base.EnsureCell<GameTime>("alarmTime", new GameTime(0, 0));
			this.CELL_friendLevel = base.EnsureCell<int>("friendLevel", 0);
			this.CELL_knowledge = base.EnsureCell<string[]>("knowledge", new string[0]);
			this.CELL_timetableName = base.EnsureCell<string>("timetableName", "");
			this.CELL_timetableMemory = base.EnsureCell<string>("timetableMemory", "");
			this.CELL_timetableTimer = base.EnsureCell<float>("timetableTimer", 0f);
			this.CELL_talking = base.EnsureCell<bool>("talking", false);
			this.CELL_conversationTargetName = base.EnsureCell<string>("conversationTargetName", "");
			this.CELL_sitting = base.EnsureCell<bool>("sitting", false);
			this.CELL_laying = base.EnsureCell<bool>("laying", false);
			this.CELL_running = base.EnsureCell<bool>("running", false);
			this.CELL_seatName = base.EnsureCell<string>("seatName", "");
			this.CELL_bedName = base.EnsureCell<string>("bedName", "");
			this.CELL_corruption = base.EnsureCell<float>("corruption", 0f);
			this.CELL_waitForGift = base.EnsureCell<bool>("waitForGift", false);
			this.CELL_neverGetsTired = base.EnsureCell<bool>("neverGetsTired", false);
			this.CELL_creditCardUsageAmount = base.EnsureCell<float>("creditCardUsageAmount", 0f);
			base.AddDataListener<WorldCoordinate>("position", new ValueEntry<WorldCoordinate>.DataChangeHandler(this.OnPositionChanged));
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000036B4 File Offset: 0x000018B4
		~Character()
		{
			base.RemoveDataListener<WorldCoordinate>("position", new ValueEntry<WorldCoordinate>.DataChangeHandler(this.OnPositionChanged));
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003700 File Offset: 0x00001900
		private void OnPositionChanged(WorldCoordinate pPreviousPosition, WorldCoordinate pNewPosition)
		{
			this.UpdateHandItemPosition();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003708 File Offset: 0x00001908
		public void ClearState()
		{
			this.CancelWalking();
			this.sitting = false;
			this.laying = false;
			this.timetableTimer = 0.5f;
			this.seat = null;
			this.bed = null;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003744 File Offset: 0x00001944
		public override string tooltipName
		{
			get
			{
				Character character = this._tingRunner.GetTingUnsafe(this._worldSettings.avatarName) as Character;
				if (character != null && character.HasKnowledge(base.name))
				{
					return base.name;
				}
				return "person人";
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003790 File Offset: 0x00001990
		public override string verbDescription
		{
			get
			{
				if (this.sleeping)
				{
					return "can't talk to sleeping不能和睡觉的人交谈";
				}
				return "talk to交谈";
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000037A8 File Offset: 0x000019A8
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[]
				{
					base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 3,
					base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 4,
					base.localPoint + IntPoint.DirectionToIntPoint(IntPoint.Turn(base.direction, 90)) * 3,
					base.localPoint + IntPoint.DirectionToIntPoint(IntPoint.Turn(base.direction, -90)) * 3
				};
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006E RID: 110 RVA: 0x0000386C File Offset: 0x00001A6C
		public override IntPoint[] interactionPointsTryTheseFirst
		{
			get
			{
				return new IntPoint[] { base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 3 };
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000038A8 File Offset: 0x00001AA8
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith != this && (pTingToInteractWith is Drink || pTingToInteractWith is Character || pTingToInteractWith is Bed || pTingToInteractWith is Seat || pTingToInteractWith is MysticalCube || pTingToInteractWith is Door || pTingToInteractWith is Lamp || pTingToInteractWith is Teleporter || pTingToInteractWith is Extractor || pTingToInteractWith is Radio || pTingToInteractWith is MusicBox || pTingToInteractWith is Portal || pTingToInteractWith is Drug || pTingToInteractWith is Computer || pTingToInteractWith is TrashCan || pTingToInteractWith is FuseBox || pTingToInteractWith is Hackdev || pTingToInteractWith is CreditCard || pTingToInteractWith is Button || pTingToInteractWith is Point || pTingToInteractWith is Goods || pTingToInteractWith is Robot || pTingToInteractWith is Key || pTingToInteractWith is Machine || pTingToInteractWith is Sink || pTingToInteractWith is Tv || pTingToInteractWith is Fountain || pTingToInteractWith is Floppy || pTingToInteractWith is Locker || pTingToInteractWith is Jewellery || pTingToInteractWith is Stove || pTingToInteractWith is FryingPan || pTingToInteractWith is SendPipe || pTingToInteractWith is Fence || pTingToInteractWith is Tram || pTingToInteractWith is Screwdriver || pTingToInteractWith is Map || pTingToInteractWith is VendingMachine || pTingToInteractWith is Telephone || pTingToInteractWith is Taser || pTingToInteractWith is Pawn || pTingToInteractWith is Memory);
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003A90 File Offset: 0x00001C90
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003A94 File Offset: 0x00001C94
		public void GetAngry()
		{
			base.StopAction();
			base.StartAction("Angry", null, 1.7f, 1.7f);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003AB4 File Offset: 0x00001CB4
		public void GetAngryAtComputer()
		{
			base.StopAction();
			base.StartAction("AngryAtComputer", null, 2f, 2f);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003AD4 File Offset: 0x00001CD4
		public void Yawn()
		{
			base.StopAction();
			base.StartAction("Yawn", null, 2f, 2f);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003AF4 File Offset: 0x00001CF4
		public override void InteractWith(Ting pTingToInteractWith)
		{
			if (this.busy)
			{
				if (this.isAvatar)
				{
					this._worldSettings.Notify(base.name, base.name + " is busy!正忙！");
					return;
				}
				D.Log(string.Concat(new object[] { base.name, " is trying to interact with ", pTingToInteractWith, " but she/he is busy." }));
				return;
			}
			else
			{
				if (pTingToInteractWith != this.handItem && !this.sitting && !this.laying)
				{
					this.FaceTing(pTingToInteractWith);
				}
				if (pTingToInteractWith.isBeingUsed && !(pTingToInteractWith is Point))
				{
					this.GetAngry();
					return;
				}
				if (pTingToInteractWith is Drink)
				{
					if ((pTingToInteractWith as Drink).amount > 0f)
					{
						base.StartAction("Drinking", pTingToInteractWith, 1.5f, 3.3f);
					}
					else
					{
						this._worldSettings.Notify(base.name, "It is empty!它是空的！");
					}
				}
				else if (pTingToInteractWith is Snus)
				{
					if ((pTingToInteractWith as Snus).charges > 0)
					{
						base.StartAction("TakingSnus", pTingToInteractWith, 0.5f, 1f);
					}
					else
					{
						this._worldSettings.Notify(base.name, "No snus left!没有鼻烟了！");
					}
				}
				else if (pTingToInteractWith is Cigarette)
				{
					if ((pTingToInteractWith as Cigarette).charges > 0)
					{
						base.StartAction("SmokingCigarette", pTingToInteractWith, 0.7f, 2.5f);
					}
					else
					{
						this._worldSettings.Notify(base.name, "It's all used up!已经用完了！");
					}
				}
				else if (pTingToInteractWith is Drug)
				{
					if (pTingToInteractWith.name.ToLower().Contains("baguette"))
					{
						base.StartAction("Eat", pTingToInteractWith, 1.5f, 2.5f);
					}
					else
					{
						base.StartAction("TakingDrug", pTingToInteractWith, 0.5f, 1f);
					}
				}
				else if (pTingToInteractWith is Point)
				{
					base.StopAction();
				}
				else if (pTingToInteractWith is MusicBox && (pTingToInteractWith as MusicBox).mixer)
				{
					if (this.isAvatar)
					{
						base.StartAction("Shrug", null, 1f, 1f);
					}
					else
					{
						base.StartAction("Mixing", pTingToInteractWith, 0.5f, 99999f);
					}
				}
				else if (pTingToInteractWith is Button)
				{
					base.StartAction("PushingButton", pTingToInteractWith, 0.5f, 1f);
				}
				else if (pTingToInteractWith is Tv)
				{
					base.StartAction("UsingTv", pTingToInteractWith, 0.5f, 1f);
				}
				else if (pTingToInteractWith is Stove)
				{
					base.StartAction("UseStove", pTingToInteractWith, 1f, 1.5f);
				}
				else if (pTingToInteractWith is VendingMachine)
				{
					VendingMachine vendingMachine = pTingToInteractWith as VendingMachine;
					if (vendingMachine.dispensedCoke == null)
					{
						base.StartAction("ActivatingVendingMachine", pTingToInteractWith, 0.5f, 1f);
					}
					else
					{
						this.PickUp(vendingMachine.dispensedCoke);
					}
				}
				else if (pTingToInteractWith is Character)
				{
					Character character = pTingToInteractWith as Character;
					if (this.isAvatar && character.sleeping)
					{
						this._worldSettings.Notify(base.name, "The person is sleeping这个人正在睡觉");
					}
					else
					{
						this.CancelWalking();
						this.conversationTarget = character;
						this.conversationTarget.conversationTarget = this;
						if (!this.sitting && !this.laying)
						{
							this.FaceTing(this.conversationTarget);
						}
						if (!this.conversationTarget.sitting && !this.conversationTarget.laying && this.conversationTarget.actionName == "")
						{
							this.conversationTarget.FaceTing(this);
						}
						if (!this.isAvatar && character.IsAtTimetableTaskOfType(typeof(Behaviour_Sell)))
						{
							character.timetableMemory = base.name;
							this.Say(this.GenerateRandomBuySentence(), "OrderingDrinks");
						}
						else
						{
							this._dialogueRunner.EventHappened(base.name + "_talk_" + pTingToInteractWith.name);
						}
					}
				}
				else if (pTingToInteractWith is Door)
				{
					Door door = pTingToInteractWith as Door;
					if (door.isLocked)
					{
						base.StartAction("LockedDoor", pTingToInteractWith, 1f, 2.7f);
						return;
					}
					if (this.isAvatar && door.autoLockTimer > 0f)
					{
						D.Log(base.name + " can't open the door '" + pTingToInteractWith.name + "' since it's auto-locking");
						base.StartAction("LockedDoor", pTingToInteractWith, 1f, 2.7f);
						return;
					}
					if (door.targetDoor == null)
					{
						this._worldSettings.Notify(base.name, "The door is broken门坏了");
						base.StopAction();
						return;
					}
					if (door.isElevatorEntrance)
					{
						Character[] tingsOfTypeInRoom = this._tingRunner.GetTingsOfTypeInRoom<Character>(door.targetDoor.room.name);
						if (tingsOfTypeInRoom.Length > 0 && door.elevatorFloor != door.targetDoor.elevatorFloor)
						{
							this._worldSettings.Notify(base.name, "The elevator is in use电梯正在使用中");
							base.StopAction();
							return;
						}
					}
					base.StartAction("WalkingThroughDoor", pTingToInteractWith, (!this.isAvatar) ? 3f : 1f, 3f);
					door.Open();
					this._dialogueRunner.EventHappened(base.name + "_open_" + pTingToInteractWith.name);
				}
				else if (pTingToInteractWith is Portal)
				{
					base.StartAction("WalkingThroughPortal", pTingToInteractWith, 1.8f, 1.8f);
				}
				else if (pTingToInteractWith is Bed)
				{
					if (this.laying)
					{
						D.Log("Can't roll over to other side of the bed");
						return;
					}
					this.bed = pTingToInteractWith as Bed;
					int num = 0;
					foreach (IntPoint intPoint in this.bed.interactionPoints)
					{
						if (intPoint == base.localPoint)
						{
							this.bed.exitPoint = num;
							break;
						}
						num++;
					}
					base.StartAction("LayingDown", pTingToInteractWith, 3f, 3f);
					this._dialogueRunner.EventHappened(base.name + "_lay_" + pTingToInteractWith.name);
				}
				else if (pTingToInteractWith is Seat)
				{
					this.seat = pTingToInteractWith as Seat;
					base.StartAction("GettingSeated", pTingToInteractWith, 5f, 5f);
					this._dialogueRunner.EventHappened(base.name + "_sit_" + pTingToInteractWith.name);
				}
				else if (pTingToInteractWith is TingWithButton)
				{
					MusicBox musicBox = pTingToInteractWith as MusicBox;
					if (musicBox != null && musicBox.isJukebox)
					{
						base.StartAction("StartingJukebox", pTingToInteractWith, 0.8f, 1f);
					}
					else
					{
						base.StartAction("PushingButtonOnHandItem", pTingToInteractWith, 0.8f, 1f);
					}
				}
				else if (pTingToInteractWith is Lamp)
				{
					base.StartAction("KickingLamp", pTingToInteractWith, 0.75f, 1.2f);
				}
				else if (pTingToInteractWith is Telephone)
				{
					base.StartAction("TalkingInTelephone", pTingToInteractWith, 1.5f, 99999f);
				}
				else if (pTingToInteractWith is Computer)
				{
					base.StartAction("UsingComputer", pTingToInteractWith, 1.2f, 99999f);
					this._dialogueRunner.EventHappened(base.name + "_use_" + pTingToInteractWith.name);
				}
				else if (pTingToInteractWith is FuseBox)
				{
					base.StartAction("Inspect", pTingToInteractWith, 0.5f, 99999f);
				}
				else if (pTingToInteractWith is Fountain)
				{
					base.StartAction("Inspect", pTingToInteractWith, 0.5f, 99999f);
				}
				else if (pTingToInteractWith is TrashCan)
				{
					base.StartAction("Inspect", pTingToInteractWith, 0.5f, 99999f);
				}
				else if (!(pTingToInteractWith is Locker))
				{
					if (pTingToInteractWith is CreditCard)
					{
						base.StartAction("PushingButtonOnHandItem", pTingToInteractWith, 0.8f, 1f);
					}
					else if (pTingToInteractWith is Fence)
					{
						base.StartAction("WalkingThroughFence", pTingToInteractWith, 2.19f, 2.19f);
						(pTingToInteractWith as Fence).StartedWalkingThrough(this);
					}
					else if (pTingToInteractWith is Map)
					{
						base.StartAction("LookingAtMap", pTingToInteractWith, 0.5f, 1f);
					}
					else if (pTingToInteractWith is Sink)
					{
						base.StartAction("UseSink", pTingToInteractWith, 1f, 2f);
					}
					else if (pTingToInteractWith is Machine)
					{
						base.StartAction("Inspect", pTingToInteractWith, 0.5f, 99999f);
					}
					else
					{
						base.StopAction();
						D.Log(base.name + " is trying to but can't interact with " + pTingToInteractWith.name);
					}
				}
				return;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004464 File Offset: 0x00002664
		private string GenerateRandomBuySentence()
		{
			Character.buySentenceCounter++;
			return Character.buySentences[Character.buySentenceCounter % Character.buySentences.Length];
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004488 File Offset: 0x00002688
		public void UseHandItemToInteractWith(Ting pTingToInteractWith)
		{
			if (pTingToInteractWith != this.handItem)
			{
				this.FaceTing(pTingToInteractWith);
			}
			if (this.handItem is Key && pTingToInteractWith is Door)
			{
				base.StartAction("UsingDoorWithKey", pTingToInteractWith, 1.5f, 1.8f);
			}
			else if (pTingToInteractWith is Locker && this.handItem.CanInteractWith(pTingToInteractWith))
			{
				base.StartAction("PuttingTingIntoLocker", pTingToInteractWith, 0.5f, 1.2f);
			}
			else if (pTingToInteractWith != this.handItem && this.handItem is Extractor)
			{
				D.Log("Using extractor on " + pTingToInteractWith);
				base.StartAction("Extracting", pTingToInteractWith, 0.1f, 1f);
			}
			else if (pTingToInteractWith is SendPipe && this.handItem != null)
			{
				base.StartAction("PuttingTingIntoSendPipe", pTingToInteractWith, 1f, 2f);
			}
			else if (pTingToInteractWith is TrashCan && this.handItem != null)
			{
				base.StartAction("ThrowingTingIntoTrashCan", pTingToInteractWith, 0.9f, 1.6f);
			}
			else if (pTingToInteractWith is Sink && this.handItem is Drink)
			{
				(pTingToInteractWith as Sink).on = true;
				base.StartAction("RefillingDrink", pTingToInteractWith, 0.5f, 1.2f);
			}
			else if (pTingToInteractWith is Stove && this.handItem != null)
			{
				(pTingToInteractWith as Stove).Fry(this, this.handItem);
			}
			else if (pTingToInteractWith is Character && this.handItem is Taser)
			{
				base.StartAction("Tasing", pTingToInteractWith, 0.5f, 1.5f);
			}
			else if (pTingToInteractWith is Computer && this.handItem is Screwdriver)
			{
				base.StartAction("Screwing", pTingToInteractWith, 0.5f, 1.2f);
			}
			else
			{
				base.StopAction();
				D.Log(string.Concat(new object[] { base.name, " at position ", base.position, " is trying to but can't interact using hand item '", this.handItem, "' with target '", pTingToInteractWith.name, "'" }));
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000046F8 File Offset: 0x000028F8
		public static bool ArePointsWithinDistance(IntPoint p1, IntPoint p2, int pDistance)
		{
			int num = p1.x - p2.x;
			int num2 = p1.y - p2.y;
			return num * num + num2 * num2 < pDistance * pDistance;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004734 File Offset: 0x00002934
		protected override void ActionTriggered(Ting pOtherTing)
		{
			if (base.actionName == "Drinking")
			{
				Drink drink = (Drink)pOtherTing;
				D.isNull(drink, "drink is null");
				drink.DrinkFrom(this);
			}
			else if (base.actionName == "TakingDrug" || base.actionName == "Eat")
			{
				Drug drug = (Drug)pOtherTing;
				D.isNull(drug, "drug is null");
				drug.Take(this);
				base.actionOtherObject = null;
				this.SetNoHandItem();
				this._dialogueRunner.EventHappened(base.name + "_took_" + drug.name);
				this._tingRunner.RemoveTingAfterUpdate(drug.name);
			}
			else if (base.actionName == "TakingSnus")
			{
				Snus snus = pOtherTing as Snus;
				D.isNull(snus, "snus is null");
				snus.Take(this);
				this._dialogueRunner.EventHappened(base.name + "_snusade");
			}
			else if (base.actionName == "SmokingCigarette")
			{
				Cigarette cigarette = pOtherTing as Cigarette;
				D.isNull(cigarette, "cigarette is null");
				cigarette.Take(this);
			}
			else if (base.actionName == "LockedDoor")
			{
				this._worldSettings.Notify(base.name, "The door is locked门锁了");
				D.isNull(pOtherTing, "pOtherTing is null");
				this._dialogueRunner.EventHappened(base.name + "_yank_" + pOtherTing.name);
			}
			else if (base.actionName == "UseDoorReallySoon")
			{
				D.Log(base.name + " is triggering UseDoorReallySoon: " + pOtherTing.name);
				Door door = pOtherTing as Door;
				if (door.autoLockTimer <= 0f && Character.ArePointsWithinDistance(base.localPoint, door.waitingPoint, 1))
				{
					base.StopAction();
					this.InteractWith(pOtherTing);
				}
				else
				{
					this._worldSettings.Notify(base.name, "Door was just locked门刚锁上");
				}
			}
			else if (base.actionName == "WalkingThroughDoor")
			{
				Door door2 = pOtherTing as Door;
				D.isNull(door2, "door is null");
				door2.WalkThrough(this);
				door2.targetDoor.Open();
			}
			else if (base.actionName == "WalkingThroughDoorPhase2")
			{
				base.StopAction();
				if (this._walkBehaviour != null)
				{
					this._walkBehaviour.StartWalkingAgain();
				}
			}
			else if (base.actionName == "WalkingThroughPortal")
			{
				Portal portal = pOtherTing as Portal;
				D.isNull(portal, "portal is null");
				portal.WalkThrough(this);
			}
			else if (base.actionName == "WalkingThroughPortalPhase2")
			{
				base.StopAction();
				if (this._walkBehaviour != null)
				{
					this._walkBehaviour.StartWalkingAgain();
				}
			}
			else if (base.actionName == "WalkingThroughFence")
			{
				base.StartAction("DoneWalkingThroughFence", null, 0.01f, 0.02f);
				Fence fence = pOtherTing as Fence;
				D.isNull(fence, "fence is null");
				base.position = fence.goalPosition;
				fence.user = null;
				this.timetableTimer = 0f;
			}
			else if (base.actionName == "UsingDoorWithKey")
			{
				Door door3 = pOtherTing as Door;
				D.isNull(door3, "door is null");
				Key key = this.handItem as Key;
				if (key == null)
				{
					D.Log(base.name + " is UsingDoorWithKey but has no key in hand, will interact normally with it instead.");
					this.InteractWith(door3);
					return;
				}
				key.InteractWith(door3);
			}
			else if (base.actionName == "PickingUp")
			{
				if (pOtherTing.isBeingHeld)
				{
					return;
				}
				D.isNull(pOtherTing, "pOtherTing is null");
				this.SetHandItem((MimanTing)pOtherTing);
				this._dialogueRunner.EventHappened(base.name + "_pickup_" + pOtherTing.name);
				this.handItem.isBeingHeld = true;
			}
			else if (base.actionName == "Dropping" || base.actionName == "DroppingFar")
			{
				if (this.handItem == null)
				{
					D.Log(base.name + " is trying to drop hand item but it is null");
					return;
				}
				int num = ((!(base.actionName == "Dropping")) ? 2 : 1);
				WorldCoordinate worldCoordinate = new WorldCoordinate(base.room.name, base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * num);
				this.handItem.position = worldCoordinate;
				this.handItem.isBeingHeld = false;
				this.handItem.direction = base.direction;
				this.handItem.OnPutDown();
				this.SetNoHandItem();
			}
			else if (base.actionName == "PutHandItemIntoInventory")
			{
				this.MoveHandItemToInventory();
			}
			else if (base.actionName == "TakeOutInventoryItem")
			{
				this.SetHandItem((MimanTing)pOtherTing);
				this.handItem.position = base.position;
				this.handItem.isBeingHeld = true;
			}
			else if (base.actionName == "LayingDown")
			{
				D.isNull(this.bed, "bed is null");
				this.LayInBed(this.bed);
			}
			else if (base.actionName == "FallingAsleep")
			{
				int num2 = 8;
				if (!this.isAvatar)
				{
					num2 = 3;
				}
				this.Sleep(base.gameClock + new GameTime(num2, 0));
			}
			else if (base.actionName == "FallingAsleepInChair")
			{
				this.Sleep(base.gameClock + new GameTime(3, 0));
			}
			else if (base.actionName == "FallAsleepFromStanding")
			{
				this.Sleep(this.alarmTime);
				this.laying = true;
			}
			else if (base.actionName == "GettingSeated")
			{
				Seat seat = pOtherTing as Seat;
				D.isNull(seat, "seat is null");
				this.Sit(seat);
			}
			else if (base.actionName == "GettingUpFromSeat")
			{
				this.AfterGettingUpFromSeat();
				if (this.seat != null)
				{
					this.GetUpSeatSnap();
				}
			}
			else if (base.actionName == "GettingUpFromBed")
			{
				this.AfterGettingUpFromBed();
				if (this.bed != null)
				{
					this.GetUpBedSnap();
				}
			}
			else if (base.actionName == "PushingButtonOnHandItem" || base.actionName == "StartingJukebox")
			{
				D.Log("Triggering action " + base.actionName);
				TingWithButton tingWithButton = pOtherTing as TingWithButton;
				D.isNull(tingWithButton, "tingWithButton is null");
				if (tingWithButton == null)
				{
					D.LogError(pOtherTing.name + " is not a TingWithButton");
				}
				tingWithButton.PushButton(this);
			}
			else if (base.actionName == "PushingButton")
			{
				Button button = pOtherTing as Button;
				D.isNull(button, "button is null");
				button.Push(this);
				this._dialogueRunner.EventHappened(base.name + "_pressed_" + pOtherTing.name);
			}
			else if (base.actionName == "UsingTv")
			{
				Tv tv = pOtherTing as Tv;
				D.isNull(tv, "tv is null");
				tv.Flip();
			}
			else if (base.actionName == "TurnLeft")
			{
				base.TurnDegrees(90);
			}
			else if (base.actionName == "TurnRight")
			{
				base.TurnDegrees(-90);
			}
			else if (base.actionName == "KickingLamp")
			{
				Lamp lamp = pOtherTing as Lamp;
				D.isNull(lamp, "lamp is null");
				lamp.Kick();
			}
			else if (base.actionName == "Extracting")
			{
				Extractor extractor = this.handItem as Extractor;
				if (extractor == null)
				{
					D.LogError(base.name + " is trying to use a hand item that is not an Extractor to extract things");
				}
				else
				{
					extractor.Attach(pOtherTing);
				}
			}
			else if (base.actionName == "UsingComputer")
			{
				Computer computer = pOtherTing as Computer;
				D.isNull(computer, "computer is null");
				Floppy floppy = this.handItem as Floppy;
				computer.GetUsedBy(this, floppy);
				this._dialogueRunner.EventHappened(base.name + "_start_Computer");
			}
			else if (base.actionName == "SlurpingIntoComputer")
			{
				base.StartAction("InsideComputer", pOtherTing, 99999f, 99999f);
				this._dialogueRunner.EventHappened(base.name + "_slurped_" + pOtherTing.name);
			}
			else if (base.actionName == "Inspect")
			{
				if (base.actionOtherObject is FuseBox)
				{
					(base.actionOtherObject as FuseBox).BeInspected(this);
					this._dialogueRunner.EventHappened(base.name + "_inspect_Fusebox");
				}
				else if (base.actionOtherObject is Fountain)
				{
					this._dialogueRunner.EventHappened(base.name + "_inspect_Fountain");
				}
				else if (base.actionOtherObject is TrashCan)
				{
					this._dialogueRunner.EventHappened(base.name + "_inspect_TrashCan");
				}
			}
			else if (base.actionName == "ThrowingTingIntoTrashCan")
			{
				MimanTing handItem = this.handItem;
				TrashCan trashCan = pOtherTing as TrashCan;
				D.isNull(trashCan, "trashCan is null");
				trashCan.Throw(handItem);
			}
			else if (base.actionName == "PuttingTingIntoSendPipe")
			{
				SendPipe sendPipe = pOtherTing as SendPipe;
				D.isNull(sendPipe, "pipe is null");
				MimanTing handItem2 = this.handItem;
				this.handItem.isBeingHeld = false;
				this.SetNoHandItem();
				sendPipe.PutStuffIntoIt(handItem2);
			}
			else if (base.actionName == "PuttingTingIntoLocker")
			{
				Locker locker = pOtherTing as Locker;
				D.isNull(locker, "locker is null");
				this.handItem.isBeingHeld = false;
				bool flag = locker.PutTingIntoRandomFreeSpot(this.handItem);
				if (flag)
				{
					this.SetNoHandItem();
				}
			}
			else if (base.actionName == "GivingHandItem")
			{
				Character character = pOtherTing as Character;
				D.isNull(character, "The receiver is null");
				this._dialogueRunner.EventHappened(base.name + "_give_" + character.name);
				if (this.onNewHandItem != null)
				{
					this.onNewHandItem("", true);
				}
				if (character.handItem != null)
				{
					character.MoveHandItemToInventoryForcefully();
				}
				character.SetHandItem(this.handItem);
				this.handItem = null;
			}
			else if (base.actionName == "BeingBothered")
			{
				this.timetableTimer = 0.5f;
			}
			else if (base.actionName == "Tasing")
			{
				Character character2 = pOtherTing as Character;
				D.isNull(character2, "other character is null");
				character2.GetTased();
				this._dialogueRunner.EventHappened(base.name + "_tase_" + character2);
			}
			else if (base.actionName == "GettingTased")
			{
				this.ClearWalkingData();
				this.FallAsleepFromStanding(2);
			}
			else if (base.actionName == "Angry")
			{
				this.timetableTimer = 0.5f;
			}
			else if (base.actionName == "UseSink")
			{
				Sink sink = base.actionOtherObject as Sink;
				D.isNull(sink, "sink is null");
				sink.Toggle();
				if (!sink.on)
				{
					this._dialogueRunner.EventHappened(base.name + "_turnOff_" + sink.name);
				}
			}
			else if (base.actionName == "RefillingDrink")
			{
				Drink drink2 = this.handItem as Drink;
				D.isNull(drink2, "drink is null");
				Sink sink2 = base.actionOtherObject as Sink;
				if (sink2 == null)
				{
					D.Log("Sink was null, can't refill");
				}
				else
				{
					sink2.UseDrinkOnSink(drink2);
				}
			}
			else if (base.actionName == "Screwing")
			{
				Screwdriver screwdriver = this.handItem as Screwdriver;
				D.isNull(screwdriver, "screwdriver is null");
				Computer computer2 = base.actionOtherObject as Computer;
				if (computer2 == null)
				{
					D.Log("Computer was null, can't screw it");
				}
				else
				{
					screwdriver.UseOnComputer(computer2);
				}
			}
			else if (base.actionName == "Mixing")
			{
				MusicBox musicBox = pOtherTing as MusicBox;
				D.isNull(musicBox, "mixer is null");
				musicBox.isPlaying = true;
			}
			else if (base.actionName == "UseStove")
			{
				Stove stove = pOtherTing as Stove;
				D.isNull(stove, "stove is null");
				stove.on = !stove.on;
			}
			else if (base.actionName == "TalkingInTelephone")
			{
				Telephone telephone = pOtherTing as Telephone;
				D.isNull(telephone, "phone is null");
				telephone.Use();
				this._dialogueRunner.EventHappened(base.name + "_phone_" + pOtherTing.name);
			}
			else if (base.actionName == "ActivatingVendingMachine")
			{
				VendingMachine vendingMachine = pOtherTing as VendingMachine;
				D.isNull(vendingMachine, "vendingMachine is null");
				vendingMachine.PushCokeDispenserButton(this);
			}
			else if (base.actionName == "Stealing")
			{
				Character character3 = base.actionOtherObject as Character;
				D.isNull(character3, "other character is null");
				Ting[] inventoryItems = character3.inventoryItems;
				if (inventoryItems.Length == 0)
				{
					this._worldSettings.Notify(base.name, "Nothing to steal没什么可偷的");
				}
				else
				{
					Ting ting = Randomizer.RandNth<Ting>(inventoryItems);
					this.MoveHandItemToInventory();
					this.SetHandItem(ting as MimanTing);
					this._worldSettings.Notify(base.name, "You stole你偷到了: " + ting.tooltipName);
				}
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005668 File Offset: 0x00003868
		public void GetTased()
		{
			base.StartAction("GettingTased", null, 1.5f, 1.5f);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005680 File Offset: 0x00003880
		public void GetTasedGently()
		{
			base.StartAction("GettingTasedGently", null, 1.5f, 1.5f);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00005698 File Offset: 0x00003898
		public void AfterGettingUpFromSeat()
		{
			this.sitting = false;
			base.StopAction();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000056A8 File Offset: 0x000038A8
		public void AfterGettingUpFromBed()
		{
			this.laying = false;
			base.direction = IntPoint.Turn(base.direction, 180);
			base.StopAction();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000056D8 File Offset: 0x000038D8
		public bool MoveHandItemToInventory()
		{
			if (this.isAvatar && this.inventoryIsFull)
			{
				this._worldSettings.Notify(base.name, "Inventory is full");
				return false;
			}
			if (this.handItem == null)
			{
				return false;
			}
			this.MoveHandItemToInventoryForcefully();
			return true;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005728 File Offset: 0x00003928
		private void MoveHandItemToInventoryForcefully()
		{
			this.handItem.isBeingHeld = false;
			this.handItem.position = new WorldCoordinate(this.inventoryRoomName, IntPoint.Zero);
			this.SetNoHandItem();
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00005764 File Offset: 0x00003964
		public bool inventoryIsFull
		{
			get
			{
				return this.inventoryItems.Length >= 20;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005778 File Offset: 0x00003978
		private void SetHandItem(MimanTing pNewHandItem)
		{
			if (pNewHandItem == null)
			{
				D.Log("pNewHandItem is null!");
				return;
			}
			if (this.handItem != null)
			{
				this.MoveHandItemToInventoryForcefully();
				if (this.onNewHandItem != null)
				{
					this.onNewHandItem("", false);
				}
			}
			this.handItem = pNewHandItem;
			this.handItem.isBeingHeld = true;
			this.handItem.PrepareForBeingHacked();
			if (this.onNewHandItem != null)
			{
				this.onNewHandItem(this.handItem.name, false);
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005804 File Offset: 0x00003A04
		private void SetNoHandItem()
		{
			this.handItem.isBeingHeld = false;
			this.handItem = null;
			if (this.onNewHandItem != null)
			{
				this.onNewHandItem("", false);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00005840 File Offset: 0x00003A40
		public string inventoryRoomName
		{
			get
			{
				return base.name + "_inventory";
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00005854 File Offset: 0x00003A54
		public Ting[] inventoryItems
		{
			get
			{
				return this._tingRunner.GetTingsInRoom(this.inventoryRoomName);
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005868 File Offset: 0x00003A68
		private void FaceTing(Ting pTing)
		{
			base.direction = (pTing.position.localPosition - base.localPoint).ToDirection();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000589C File Offset: 0x00003A9C
		public void LayInBed(Bed pBed)
		{
			if (pBed == null)
			{
				D.LogError(base.name + " is trying to sit on pOtherTing that is null");
				return;
			}
			if (this.bed == null)
			{
				this.bed = pBed;
			}
			base.position = pBed.position;
			base.direction = ((pBed.exitPoint != 1) ? IntPoint.Turn(pBed.direction, 180) : pBed.direction);
			this.laying = true;
			if (base.actionName != "Sleeping")
			{
				base.StopAction();
				bool flag = this.sleepiness > 20f;
				bool isDaytime = this._worldSettings.gameTimeClock.isDaytime;
				if (this.isAvatar && !flag)
				{
					this._worldSettings.Notify(base.name, "Not feeling tired enough to sleep还不太困，睡不着");
				}
				else if (this.isAvatar && isDaytime && this.sleepiness < 80f)
				{
					this._worldSettings.Notify(base.name, "It's too bright to sleep太亮了，睡不着");
				}
				else
				{
					base.StartAction("FallingAsleep", null, 3f, 99999f);
				}
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000059D4 File Offset: 0x00003BD4
		public void Sit(Seat pSeat)
		{
			if (pSeat == null)
			{
				D.LogError(base.name + " is trying to sit on pOtherTing that is null");
				return;
			}
			if (this.seat == null)
			{
				this.seat = pSeat;
			}
			base.direction = pSeat.direction;
			base.position = pSeat.position;
			base.StopAction();
			this.sitting = true;
			PointTileNode tile = base.room.GetTile(this.seat.computerPoint);
			if (tile != null)
			{
				Computer occupantOfType = tile.GetOccupantOfType<Computer>();
				if (occupantOfType != null)
				{
					if (this.rememberToHackComputerAfterSittingDown)
					{
						this.Hack(occupantOfType);
					}
					else
					{
						this.InteractWith(occupantOfType);
					}
				}
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005A7C File Offset: 0x00003C7C
		public void GetUpSeatSnap()
		{
			D.isNull(this.seat, "seat is null");
			base.position = new WorldCoordinate(base.room.name, this.seat.GetCurrentExitPoint());
			this.seat = null;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005AC4 File Offset: 0x00003CC4
		public void GetUpBedSnap()
		{
			D.isNull(this.bed, "bed is null");
			int exitPoint = this.bed.exitPoint;
			base.position = new WorldCoordinate(base.room.name, this.bed.GetCurrentExitPoint());
			this.bed = null;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005B18 File Offset: 0x00003D18
		public void PickUp(Ting pTingToPickUp)
		{
			if (!pTingToPickUp.canBePickedUp)
			{
				throw new Exception("Can't pick up '" + pTingToPickUp + "'");
			}
			if (pTingToPickUp.isBeingHeld)
			{
				return;
			}
			bool flag = true;
			if (this.handItem != null)
			{
				flag = this.MoveHandItemToInventory();
			}
			base.StopAction();
			if (flag)
			{
				this.FaceTing(pTingToPickUp);
				base.StartAction("PickingUp", pTingToPickUp, 0.6f, 1.82f);
			}
			else
			{
				this._worldSettings.Notify(base.name, "Can't put current hand item into bag无法将当前物品放入背包");
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005BAC File Offset: 0x00003DAC
		public void DropHandItem()
		{
			if (this.isAvatar && this.busy)
			{
				this._worldSettings.Notify(base.name, base.name + " is busy!正忙！");
				return;
			}
			if (this.handItem == null)
			{
				D.Log("Don't have a hand item");
				return;
			}
			WorldCoordinate worldCoordinate = new WorldCoordinate(base.room.name, base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 1);
			PointTileNode tile = base.room.GetTile(worldCoordinate.localPosition);
			if (tile == null || tile.HasOccupants())
			{
				this._worldSettings.Notify(base.name, "Can't put 不能把 " + this.handItem.name + " there放在那里");
				return;
			}
			base.StartAction("Dropping", this.handItem, 1f, 1.5f);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005CA0 File Offset: 0x00003EA0
		public void DropHandItemFar()
		{
			if (this.isAvatar && this.busy)
			{
				this._worldSettings.Notify(base.name, base.name + " is busy!正忙！");
				return;
			}
			if (this.handItem == null)
			{
				D.Log("Don't have a hand item");
				return;
			}
			WorldCoordinate worldCoordinate = new WorldCoordinate(base.room.name, base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 2);
			PointTileNode tile = base.room.GetTile(worldCoordinate.localPosition);
			if (tile == null || (this.isAvatar && tile.HasOccupants()))
			{
				this._worldSettings.Notify(base.name, "Can't put " + this.handItem.name + " there");
				return;
			}
			base.StartAction("DroppingFar", this.handItem, 1f, 1.5f);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005DA0 File Offset: 0x00003FA0
		public void PutHandItemIntoInventory()
		{
			if (this.isAvatar && this.busy)
			{
				this._worldSettings.Notify(base.name, base.name + " is busy so can't put hand item into inventory!正忙，无法将物品放入背包!");
				return;
			}
			if (this.isAvatar && this.inventoryIsFull)
			{
				this._worldSettings.Notify(base.name, "Inventory is full");
				return;
			}
			if (this.handItem == null)
			{
				D.Log("Don't have a hand item");
			}
			base.StartAction("PutHandItemIntoInventory", this.handItem, 0.5f, 1.4f);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005E44 File Offset: 0x00004044
		public void TakeOutInventoryItem(Ting pTingInInventory)
		{
			if (pTingInInventory == this)
			{
				D.Log("Can't take out yourself from the inventory");
				return;
			}
			if (this.handItem != null)
			{
				this.MoveHandItemToInventoryForcefully();
			}
			base.StartAction("TakeOutInventoryItem", pTingInInventory, 0.5f, 1.4f);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005E8C File Offset: 0x0000408C
		public void GiveHandItemToPerson()
		{
			if (this.handItem == null)
			{
				D.Log(base.name + " can't give hand item since it is null");
				return;
			}
			Character conversationTarget = this.conversationTarget;
			if (conversationTarget != null)
			{
				if (this.isAvatar && !conversationTarget.waitForGift)
				{
					D.Log(string.Concat(new object[] { base.name, " can't give item to ", conversationTarget, ", won't accept gift." }));
					this._worldSettings.Notify(base.name, "Person won't accept gift.对方不接受礼物");
				}
				else
				{
					base.StartAction("GivingHandItem", this.conversationTarget, 0.7f, 0.7f);
					conversationTarget.ReceiveHandItem();
				}
			}
			else
			{
				D.Log("Can't give hand item to conversationTarget: " + this.conversationTarget + " because it is not set to a Character");
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00005F68 File Offset: 0x00004168
		public bool isAvatar
		{
			get
			{
				return this._worldSettings.avatarName == base.name;
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005F80 File Offset: 0x00004180
		public void ReceiveHandItem()
		{
			if (this.handItem != null)
			{
				this.MoveHandItemToInventoryForcefully();
			}
			base.StartAction("ReceivingHandItem", base.actionOtherObject, 1.6f, 2f);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005FBC File Offset: 0x000041BC
		public void StartTalking()
		{
			if (this._walkBehaviour != null)
			{
				this.CancelWalking();
			}
			this.talking = true;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005FD8 File Offset: 0x000041D8
		public void StopTalking()
		{
			this.timetableTimer = 0.5f;
			this.talking = false;
			this.ClearConversationTarget();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005FF4 File Offset: 0x000041F4
		public void ClearConversationTarget()
		{
			if (this.conversationTarget != null)
			{
				this.conversationTarget.conversationTarget = null;
			}
			this.conversationTarget = null;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00006020 File Offset: 0x00004220
		public void FallAsleepFromStanding(int pHours)
		{
			base.StartAction("FallAsleepFromStanding", null, 3f, 3f);
			this.alarmTime = base.gameClock + new GameTime(pHours, 0);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000605C File Offset: 0x0000425C
		public void Sleep(int pHours)
		{
			this.ClearConversationTarget();
			this.CancelWalking();
			this.Sleep(base.gameClock + new GameTime(pHours, 0));
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006090 File Offset: 0x00004290
		public void Sleep(GameTime pAlarmTime)
		{
			if (this.isAvatar)
			{
				D.Log("Sleep was called on avatar with alarm time: " + pAlarmTime);
				if (pAlarmTime.hours > 0 && pAlarmTime.hours < 8)
				{
					pAlarmTime.hours = 8;
					pAlarmTime.minutes = Randomizer.GetIntValue(0, 60);
					D.Log("Prevented avatar from waking up too early, set hours to " + pAlarmTime.hours);
				}
				else if (this._tingRunner.gameClock.hours < 8 && pAlarmTime.hours > 11 && pAlarmTime.hours < 18)
				{
					pAlarmTime.hours = Randomizer.GetIntValue(8, 11);
					pAlarmTime.minutes = Randomizer.GetIntValue(0, 60);
					D.Log("Prevented avatar from waking up too late, set hours to " + pAlarmTime.hours);
				}
			}
			this.alarmTime = pAlarmTime;
			base.StartAction("Sleeping", null, 99999f, 99999f);
			this._dialogueRunner.EventHappened(base.name + "_fellAsleep");
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000061B4 File Offset: 0x000043B4
		public void Hack(MimanTing pHackableTing)
		{
			if (pHackableTing == null)
			{
				D.Log("Hackable ting of " + base.name + " was null!");
				return;
			}
			if (this.hackdev == null)
			{
				D.Log(base.name + " has got no hackdev to hack with");
				return;
			}
			if (pHackableTing != this.handItem)
			{
				this.FaceTing(pHackableTing);
			}
			pHackableTing.PrepareForBeingHacked();
			if (pHackableTing == this.hackdev)
			{
				this._worldSettings.Notify(base.name, "Modifier can't modify itself修改器不能修改它自己");
			}
			else if (pHackableTing.programs.Length > 0)
			{
				MockProgram mockProgram = new MockProgram(delegate(object retVal)
				{
					if (retVal.GetType() == typeof(bool) && (bool)retVal)
					{
						this.StartAction("Hacking", pHackableTing, 99999f, 99999f);
						this._dialogueRunner.EventHappened(this.name + "_hack_" + pHackableTing.name);
					}
					else
					{
						D.Log("Hacking not allowed with current device for character " + this.name);
						this._worldSettings.Notify(this.name, "Not allowed with current device 当前设备不允许此操作");
						this.StopAction();
					}
				});
				base.StartAction("AttemptHacking", pHackableTing, 1f, 1f);
				pHackableTing.PrepareForBeingHacked();
				this.hackdev.PrepareForBeingHacked();
				if (this.hackdev.masterProgram.HasFunction("Allow", true))
				{
					this.hackdev.masterProgram.StartAtFunctionWithMockReceiver("Allow", new object[]
					{
						pHackableTing.name,
						(float)pHackableTing.securityLevel
					}, mockProgram);
				}
				else
				{
					this._worldSettings.Notify(base.name, "No Allow-function in 缺少Allow函数于 " + this.hackdev.name);
				}
			}
			else
			{
				this._worldSettings.Notify(base.name, "No programs to hack in 没有可骇入的程序，在 " + pHackableTing.name);
				base.StopAction();
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000637C File Offset: 0x0000457C
		public void SetKnowledge(string pKnowledge)
		{
			foreach (string text in this.knowledge)
			{
				if (text == pKnowledge)
				{
					return;
				}
			}
			string[] array = new string[this.knowledge.Length + 1];
			int num = 0;
			foreach (string text2 in this.knowledge)
			{
				array[num++] = text2;
			}
			array[num] = pKnowledge;
			this.knowledge = array;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00006408 File Offset: 0x00004608
		public bool HasKnowledge(string pKnowledge)
		{
			foreach (string text in this.knowledge)
			{
				if (text == pKnowledge)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00006444 File Offset: 0x00004644
		public void TurnLeft()
		{
			base.StartAction("TurnLeft", base.actionOtherObject, 1f, 1f);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00006464 File Offset: 0x00004664
		public void TurnRight()
		{
			base.StartAction("TurnRight", base.actionOtherObject, 1f, 1f);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00006484 File Offset: 0x00004684
		public void WalkTo(WorldCoordinate pPosition)
		{
			this.PrepareForNewWalkBehaviour(pPosition, null, Character.WalkMode.WALK_TO_POINT);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00006490 File Offset: 0x00004690
		public void WalkToTingAndInteract(Ting pOtherTing)
		{
			this.rememberToHackComputerAfterSittingDown = false;
			if (this.isAvatar && pOtherTing is Door)
			{
				Door door = pOtherTing as Door;
				if (door.isBusy)
				{
					this._worldSettings.Notify(base.name, "Door in use, will wait in line 门正在使用，请排队");
					this.rememberToUseDoorAfterWaitingPolitely = door;
					this.WalkTo(new WorldCoordinate(base.room.name, door.waitingPoint));
					return;
				}
			}
			if (pOtherTing.HasInteractionPointHere(base.position))
			{
				if (pOtherTing.canBePickedUp)
				{
					this.logger.Log(string.Concat(new object[] { base.name, " will pick up ", pOtherTing, " directly, no need to move" }));
					this.PickUp(pOtherTing);
				}
				else
				{
					this.logger.Log(string.Concat(new object[] { base.name, " will interact directly with ", pOtherTing, ", no need to move" }));
					this.InteractWith(pOtherTing);
				}
				return;
			}
			if (pOtherTing is Character && MimanGrimmApiDefinitions.AreTingsWithinDistance(this, pOtherTing, 10))
			{
				Character character = pOtherTing as Character;
				if (this.sitting || this.laying || character.HasNoFreeInteractionPoints())
				{
					this.InteractWith(pOtherTing);
					return;
				}
			}
			this.PrepareForNewWalkBehaviour(pOtherTing.position, pOtherTing, Character.WalkMode.WALK_TO_TING_AND_INTERACT);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000065F4 File Offset: 0x000047F4
		public void WalkToTingAndUseHandItem(Ting pOtherTing)
		{
			this.rememberToHackComputerAfterSittingDown = false;
			if (pOtherTing.HasInteractionPointHere(base.position))
			{
				this.UseHandItemToInteractWith(pOtherTing);
				return;
			}
			this.PrepareForNewWalkBehaviour(pOtherTing.position, pOtherTing, Character.WalkMode.WALK_TO_TING_AND_USE_HAND_ITEM);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00006630 File Offset: 0x00004830
		public void WalkToTingAndHack(MimanTing pHackableTing)
		{
			this.rememberToHackComputerAfterSittingDown = true;
			if (pHackableTing.HasInteractionPointHere(base.position))
			{
				this.Hack(pHackableTing);
				return;
			}
			this.PrepareForNewWalkBehaviour(pHackableTing.position, pHackableTing, Character.WalkMode.WALK_TO_TING_AND_HACK);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000666C File Offset: 0x0000486C
		private void PrepareForNewWalkBehaviour(WorldCoordinate pFinalTargetPosition, Ting pFinalTargetTing, Character.WalkMode pWalkMode)
		{
			if (pFinalTargetTing is Computer)
			{
				PointTileNode tile = pFinalTargetTing.room.GetTile(pFinalTargetTing.interactionPoints[0]);
				if (tile != null)
				{
					Seat occupantOfType = tile.GetOccupantOfType<Seat>();
					if (occupantOfType != null)
					{
						pFinalTargetTing = occupantOfType;
						pWalkMode = Character.WalkMode.WALK_TO_TING_AND_INTERACT;
					}
				}
				else
				{
					D.Log(string.Concat(new object[] { "Tile at interaction point for computer ", pFinalTargetTing, " is null, ", base.name, " can't walk there." }));
				}
			}
			else if (pFinalTargetTing is Tram)
			{
				Tram tram = pFinalTargetTing as Tram;
				if (tram.movingDoor != null)
				{
					this.WalkToTingAndInteract(tram.movingDoor);
					D.Log(string.Concat(new object[] { "Switched target for ", base.name, " from ", tram, " to ", tram.movingDoor }));
					return;
				}
			}
			if (pFinalTargetTing is Seat && pFinalTargetTing == this.seat)
			{
				return;
			}
			if (pFinalTargetTing is Bed && pFinalTargetTing == this.bed)
			{
				return;
			}
			this.finalTargetPosition = pFinalTargetPosition;
			this.finalTargetTing = pFinalTargetTing;
			this.walkMode = pWalkMode;
			this.walkIterator = 0;
			if (this.busy)
			{
				return;
			}
			if (this.sitting)
			{
				if (this.seat != null)
				{
					this.GetUpFromSeat();
					return;
				}
				this.sitting = false;
			}
			else if (this.laying)
			{
				if (this.bed != null)
				{
					this.GetUpFromBed();
					return;
				}
				this.laying = false;
			}
			this._walkBehaviour = null;
			this.ClearConversationTarget();
			this.seat = null;
			this.bed = null;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00006824 File Offset: 0x00004A24
		public void GetUpFromSeat()
		{
			if (this.seat == null)
			{
				D.Log("Seat is null for character " + base.name);
				return;
			}
			this.seat.CalculateNewExitPoint();
			base.StartAction("GettingUpFromSeat", this.seat, 1.9f, 1.95f);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00006878 File Offset: 0x00004A78
		public void GetUpFromBed()
		{
			if (this.bed == null)
			{
				D.Log("Bed is null for character " + base.name);
				return;
			}
			base.StartAction("GettingUpFromBed", this.bed, 2.5f, 2.5f);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000068C4 File Offset: 0x00004AC4
		private bool IsGettingUp()
		{
			return base.actionName == "GettingUpFromSeat" || base.actionName == "GettingUpFromBed";
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000068FC File Offset: 0x00004AFC
		public void BeBored()
		{
			base.StartAction("BeBored", null, 99999f, 99999f);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00006914 File Offset: 0x00004B14
		public void CancelWalking()
		{
			base.StopAction();
			this.ClearWalkingData();
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00006924 File Offset: 0x00004B24
		public void Bother()
		{
			if (!(this.timetableName == ""))
			{
				if (Character.canBeBotheredWhen.Contains(base.actionName))
				{
					this.CancelWalking();
					base.StartAction("BeingBothered", null, 4f, 4f);
				}
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00006984 File Offset: 0x00004B84
		private string RandomNoTimeToTalkLine()
		{
			return Character.noTimeToTalkLines[Randomizer.GetIntValue(0, Character.noTimeToTalkLines.Count)];
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000069A0 File Offset: 0x00004BA0
		internal void ClearWalkingData()
		{
			this.walkMode = Character.WalkMode.NO_TARGET;
			this.walkIterator = -1;
			this.targetPositionInRoom = WorldCoordinate.NONE;
			this.finalTargetPosition = WorldCoordinate.NONE;
			this.finalTargetTing = null;
			if (this.onRemovedPath != null)
			{
				this.onRemovedPath();
			}
			this._walkBehaviour = null;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000069F8 File Offset: 0x00004BF8
		public void AnalyzeNewTile()
		{
			if (this._walkBehaviour != null)
			{
				this._walkBehaviour.AnalyzeNewTile();
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00006A10 File Offset: 0x00004C10
		public override void Update(float dt)
		{
			if (this._timetable != null)
			{
				this._timetable.Update(dt, base.gameClock, this, this._tingRunner as MimanTingRunner, this._roomRunner, this._dialogueRunner, this._worldSettings);
			}
			if (this.handItem != null && !this.handItem.isBeingHeld)
			{
				this.handItem = null;
			}
			if (this.rememberToUseDoorAfterWaitingPolitely != null)
			{
				if (!this.rememberToUseDoorAfterWaitingPolitely.isBusy)
				{
					if (!(base.actionName != ""))
					{
						D.Log("Door is free now!");
						float num = 0.1f;
						base.StartAction("UseDoorReallySoon", this.rememberToUseDoorAfterWaitingPolitely, num, num);
						this.rememberToUseDoorAfterWaitingPolitely = null;
					}
				}
			}
			if (base.actionName == "Sleeping")
			{
				this.sleepiness -= this._worldSettings.gameTimeSpeed * dt * 0.03f;
				if (this.sleepiness < 0f)
				{
					this.sleepiness = 0f;
				}
				this.corruption -= this._worldSettings.gameTimeSpeed * dt * 0.01f;
				if (this.corruption < 0f)
				{
					this.corruption = 0f;
				}
				this.drunkenness -= this._worldSettings.gameTimeSpeed * dt * 0.02f;
				if (this.drunkenness < 0f)
				{
					this.drunkenness = 0f;
				}
				if (base.room.exterior)
				{
					this.smelliness += dt * 0.5f;
					if (this.smelliness > 100f)
					{
						this.smelliness = 100f;
					}
				}
				if (base.gameClock > this.alarmTime)
				{
					base.StopAction();
					base.dialogueLine = "";
					if (this.bed == null)
					{
						this.laying = false;
					}
				}
			}
			else
			{
				if (this.walkMode != Character.WalkMode.NO_TARGET && !this.IsGettingUp())
				{
					this.EnsureWalkBehaviour();
					this._walkBehaviour.Update(dt);
				}
				if (this.conversationTarget == null && !this.neverGetsTired && !this.talking && !this.laying && !this.sitting)
				{
					this.sleepiness += this._worldSettings.gameTimeSpeed * dt * 0.0015f;
					if (this.sleepiness > 99f && this.IsIdle() && !this.talking)
					{
						if (this.sitting)
						{
							base.StartAction("FallingAsleepInChair", this.seat, 2f, 2f);
						}
						else
						{
							this.FallAsleepFromStanding(8);
						}
					}
				}
				if (base.room.exterior && this._worldSettings.rain > 10f)
				{
					this.smelliness -= dt * 4f;
					if (this.smelliness < 0f)
					{
						this.smelliness = 0f;
					}
				}
			}
			if (this.isAvatar && (base.actionName == "InsideComputer" || base.room.name == "Internet"))
			{
				this.corruption += dt * 0.001f;
			}
			if (this.corruption > 100f)
			{
				this.corruption = 100f;
			}
			else if (this.corruption < 0f)
			{
				this.corruption = 0f;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00006DDC File Offset: 0x00004FDC
		public override bool autoUnregisterFromUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00006DE0 File Offset: 0x00004FE0
		private void EnsureWalkBehaviour()
		{
			if (this._walkBehaviour == null)
			{
				this.CreateNewWalkBehaviour();
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00006DF4 File Offset: 0x00004FF4
		public bool HasWalkBehaviour()
		{
			return this._walkBehaviour != null;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00006E04 File Offset: 0x00005004
		public void CreateNewWalkBehaviour()
		{
			this._walkBehaviour = new SmartWalkBehaviour(this, this._roomRunner, this._tingRunner, this._worldSettings);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00006E24 File Offset: 0x00005024
		private void UpdateHandItemPosition()
		{
			if (this.handItem != null)
			{
				this.handItem.position = base.position;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00006E50 File Offset: 0x00005050
		private void RefreshTimetable()
		{
			if (this.timetableName == "")
			{
				this._timetable = null;
			}
			else
			{
				this._timetable = this._timetableRunner.GetTimetable(this.timetableName);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00006E98 File Offset: 0x00005098
		public bool IsIdle()
		{
			return this.walkMode == Character.WalkMode.NO_TARGET && base.actionName == "";
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00006EB8 File Offset: 0x000050B8
		public bool sleeping
		{
			get
			{
				return base.actionName == "Sleeping";
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00006ECC File Offset: 0x000050CC
		[ShowInEditor]
		public string handItemName
		{
			get
			{
				return this.CELL_handItemObjectName.data;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00006EDC File Offset: 0x000050DC
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00006F20 File Offset: 0x00005120
		[ShowInEditor]
		public Character conversationTarget
		{
			get
			{
				if (this.CELL_conversationTargetName.data == "")
				{
					return null;
				}
				return this._tingRunner.GetTing(this.CELL_conversationTargetName.data) as Character;
			}
			set
			{
				if (value == null)
				{
					this.CELL_conversationTargetName.data = "";
				}
				else
				{
					if (value == this)
					{
						throw new Exception(base.name + " can't have itself as conversation target");
					}
					this.CELL_conversationTargetName.data = value.name;
				}
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00006F78 File Offset: 0x00005178
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00006FBC File Offset: 0x000051BC
		[ShowInEditor]
		public Seat seat
		{
			get
			{
				if (this.CELL_seatName.data == "")
				{
					return null;
				}
				return this._tingRunner.GetTing(this.CELL_seatName.data) as Seat;
			}
			set
			{
				if (value == null)
				{
					this.CELL_seatName.data = "";
				}
				else
				{
					this.CELL_seatName.data = value.name;
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00006FF8 File Offset: 0x000051F8
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x0000703C File Offset: 0x0000523C
		[ShowInEditor]
		public Bed bed
		{
			get
			{
				if (this.CELL_bedName.data == "")
				{
					return null;
				}
				return this._tingRunner.GetTing(this.CELL_bedName.data) as Bed;
			}
			set
			{
				if (value == null)
				{
					this.CELL_bedName.data = "";
				}
				else
				{
					this.CELL_bedName.data = value.name;
				}
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00007078 File Offset: 0x00005278
		// (set) Token: 0x060000BB RID: 187 RVA: 0x000070F4 File Offset: 0x000052F4
		public MimanTing handItem
		{
			get
			{
				if (this.CELL_handItemObjectName.data == "")
				{
					return null;
				}
				MimanTing mimanTing = this._tingRunner.GetTingUnsafe(this.CELL_handItemObjectName.data) as MimanTing;
				if (mimanTing == null)
				{
					D.Log(base.name + "'s hand item '" + this.CELL_handItemObjectName.data + "' was not found, setting to null");
					this.handItem = null;
					return null;
				}
				return mimanTing;
			}
			set
			{
				if (value == null)
				{
					this.CELL_handItemObjectName.data = "";
				}
				else
				{
					if (value == this)
					{
						throw new Exception(base.name + " can't hold itself as hand item");
					}
					if (this.handItem != null)
					{
						D.Log(string.Concat(new string[]
						{
							"Setting hand item of ",
							base.name,
							" to ",
							value.name,
							", will move current item ",
							this.handItem.name,
							" to inventory."
						}));
						this.handItem.isBeingHeld = false;
						this.handItem.position = new WorldCoordinate(this.inventoryRoomName, IntPoint.Zero);
					}
					this.CELL_handItemObjectName.data = value.name;
					value.position = base.position;
					value.isBeingHeld = true;
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000071E0 File Offset: 0x000053E0
		// (set) Token: 0x060000BD RID: 189 RVA: 0x000071F0 File Offset: 0x000053F0
		[ShowInEditor]
		public WorldCoordinate finalTargetPosition
		{
			get
			{
				return this.CELL_finalTargetPosition.data;
			}
			set
			{
				this.CELL_finalTargetPosition.data = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00007200 File Offset: 0x00005400
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00007244 File Offset: 0x00005444
		[ShowInEditor]
		public Ting finalTargetTing
		{
			get
			{
				if (this.CELL_finalTargetTing.data == "")
				{
					return null;
				}
				return this._tingRunner.GetTingUnsafe(this.CELL_finalTargetTing.data) as MimanTing;
			}
			set
			{
				if (value == null)
				{
					this.CELL_finalTargetTing.data = "";
				}
				else
				{
					this.CELL_finalTargetTing.data = value.name;
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00007280 File Offset: 0x00005480
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00007290 File Offset: 0x00005490
		[ShowInEditor]
		public WorldCoordinate targetPositionInRoom
		{
			get
			{
				return this.CELL_targetPositionInRoom.data;
			}
			set
			{
				this.CELL_targetPositionInRoom.data = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x000072A0 File Offset: 0x000054A0
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x000072B0 File Offset: 0x000054B0
		[ShowInEditor]
		public Character.WalkMode walkMode
		{
			get
			{
				return this.CELL_walkMode.data;
			}
			set
			{
				this.CELL_walkMode.data = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000072C0 File Offset: 0x000054C0
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x000072D0 File Offset: 0x000054D0
		[EditableInEditor]
		public float walkSpeed
		{
			get
			{
				return this.CELL_walkSpeed.data;
			}
			set
			{
				this.CELL_walkSpeed.data = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000072E0 File Offset: 0x000054E0
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x000072F0 File Offset: 0x000054F0
		[ShowInEditor]
		public float walkTimer
		{
			get
			{
				return this.CELL_walkTimer.data;
			}
			set
			{
				this.CELL_walkTimer.data = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00007300 File Offset: 0x00005500
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00007310 File Offset: 0x00005510
		[EditableInEditor]
		public float charisma
		{
			get
			{
				return this.CELL_charisma.data;
			}
			set
			{
				this.CELL_charisma.data = ((value >= 0f) ? value : 0f);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00007334 File Offset: 0x00005534
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00007344 File Offset: 0x00005544
		[EditableInEditor]
		public float smelliness
		{
			get
			{
				return this.CELL_smelliness.data;
			}
			set
			{
				this.CELL_smelliness.data = ((value >= 0f) ? value : 0f);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00007368 File Offset: 0x00005568
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00007378 File Offset: 0x00005578
		[EditableInEditor]
		public float sleepiness
		{
			get
			{
				return this.CELL_sleepiness.data;
			}
			set
			{
				this.CELL_sleepiness.data = ((value >= 0f) ? value : 0f);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000CE RID: 206 RVA: 0x0000739C File Offset: 0x0000559C
		// (set) Token: 0x060000CF RID: 207 RVA: 0x000073AC File Offset: 0x000055AC
		[EditableInEditor]
		public float drunkenness
		{
			get
			{
				return this.CELL_drunkenness.data;
			}
			set
			{
				this.CELL_drunkenness.data = ((value >= 0f) ? value : 0f);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000073D0 File Offset: 0x000055D0
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x000073E0 File Offset: 0x000055E0
		[EditableInEditor]
		public float supremacy
		{
			get
			{
				return this.CELL_supremacy.data;
			}
			set
			{
				this.CELL_supremacy.data = ((value >= 0f) ? value : 0f);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00007404 File Offset: 0x00005604
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00007414 File Offset: 0x00005614
		[EditableInEditor]
		public float happiness
		{
			get
			{
				return this.CELL_happiness.data;
			}
			set
			{
				this.CELL_happiness.data = ((value >= 0f) ? value : 0f);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00007438 File Offset: 0x00005638
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00007448 File Offset: 0x00005648
		[EditableInEditor]
		public float corruption
		{
			get
			{
				return this.CELL_corruption.data;
			}
			set
			{
				this.CELL_corruption.data = ((value >= 0f) ? value : 0f);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000746C File Offset: 0x0000566C
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x0000747C File Offset: 0x0000567C
		[ShowInEditor]
		public GameTime alarmTime
		{
			get
			{
				return this.CELL_alarmTime.data;
			}
			set
			{
				this.CELL_alarmTime.data = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000748C File Offset: 0x0000568C
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x0000749C File Offset: 0x0000569C
		[EditableInEditor]
		public int friendLevel
		{
			get
			{
				return this.CELL_friendLevel.data;
			}
			set
			{
				this.CELL_friendLevel.data = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000074AC File Offset: 0x000056AC
		// (set) Token: 0x060000DB RID: 219 RVA: 0x000074BC File Offset: 0x000056BC
		[EditableInEditor]
		public string timetableName
		{
			get
			{
				return this.CELL_timetableName.data;
			}
			set
			{
				this.CELL_timetableName.data = value;
				this.RefreshTimetable();
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000074D0 File Offset: 0x000056D0
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000074E0 File Offset: 0x000056E0
		[ShowInEditor]
		public string timetableMemory
		{
			get
			{
				return this.CELL_timetableMemory.data;
			}
			set
			{
				this.CELL_timetableMemory.data = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000074F0 File Offset: 0x000056F0
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00007500 File Offset: 0x00005700
		[EditableInEditor]
		public float timetableTimer
		{
			get
			{
				return this.CELL_timetableTimer.data;
			}
			set
			{
				this.CELL_timetableTimer.data = value;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00007510 File Offset: 0x00005710
		public void ResetCurrentTimetableTask()
		{
			if (this._timetable == null)
			{
				return;
			}
			TimetableBehaviour behaviour = this._timetable.GetCurrentSpan(base.gameClock).behaviour;
			if (behaviour == null)
			{
				D.Log("ResetCurrentTimetableTask for " + base.name + ", current behaviour is null!");
				return;
			}
			D.Log("Reset Current Timetable Task " + behaviour.ToString() + " for " + base.name);
			this.timetableTimer = 0f;
			behaviour.Reset();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00007598 File Offset: 0x00005798
		public bool IsAtTimetableTask(string pTimetableTaskName)
		{
			if (this._timetable == null)
			{
				return false;
			}
			bool flag = this._timetable.GetCurrentSpan(base.gameClock).name == pTimetableTaskName;
			TimetableBehaviour behaviour = this._timetable.GetCurrentSpan(base.gameClock).behaviour;
			if (behaviour == null)
			{
				D.Log("Checking IsAtTimetableTask for " + base.name + ", current behaviour is null!");
				return false;
			}
			bool flag2 = behaviour.IsAtFinalPartOfTask(this);
			return flag && flag2;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007624 File Offset: 0x00005824
		public bool IsAtTimetableTaskOfType(Type pTimetableTaskType)
		{
			if (this._timetable == null)
			{
				return false;
			}
			TimetableBehaviour behaviour = this._timetable.GetCurrentSpan(base.gameClock).behaviour;
			return behaviour != null && behaviour.GetType() == pTimetableTaskType;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x0000766C File Offset: 0x0000586C
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x0000767C File Offset: 0x0000587C
		public string[] knowledge
		{
			get
			{
				return this.CELL_knowledge.data;
			}
			set
			{
				this.CELL_knowledge.data = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000768C File Offset: 0x0000588C
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x0000769C File Offset: 0x0000589C
		public int walkIterator
		{
			get
			{
				return this.CELL_walkIterator.data;
			}
			set
			{
				this.CELL_walkIterator.data = value;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000076AC File Offset: 0x000058AC
		public void SetTimetableRunner(TimetableRunner pTimetableRunner)
		{
			this._timetableRunner = pTimetableRunner;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000076B8 File Offset: 0x000058B8
		public Timetable timetable
		{
			get
			{
				return this._timetable;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000076C0 File Offset: 0x000058C0
		// (set) Token: 0x060000EA RID: 234 RVA: 0x000076D0 File Offset: 0x000058D0
		[EditableInEditor]
		public bool talking
		{
			get
			{
				return this.CELL_talking.data;
			}
			set
			{
				this.CELL_talking.data = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000076E0 File Offset: 0x000058E0
		// (set) Token: 0x060000EC RID: 236 RVA: 0x000076F0 File Offset: 0x000058F0
		[EditableInEditor]
		public bool sitting
		{
			get
			{
				return this.CELL_sitting.data;
			}
			set
			{
				this.CELL_sitting.data = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00007700 File Offset: 0x00005900
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00007710 File Offset: 0x00005910
		[EditableInEditor]
		public bool laying
		{
			get
			{
				return this.CELL_laying.data;
			}
			set
			{
				this.CELL_laying.data = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00007720 File Offset: 0x00005920
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00007730 File Offset: 0x00005930
		[EditableInEditor]
		public bool running
		{
			get
			{
				return this.CELL_running.data;
			}
			set
			{
				this.CELL_running.data = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00007740 File Offset: 0x00005940
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00007750 File Offset: 0x00005950
		[EditableInEditor]
		public bool waitForGift
		{
			get
			{
				return this.CELL_waitForGift.data;
			}
			set
			{
				this.CELL_waitForGift.data = value;
				if (value && this.conversationTarget != null && this.conversationTarget.name == this._worldSettings.avatarName)
				{
					this._dialogueRunner.EventHappened("ShowClickHereHelpArrow");
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x000077AC File Offset: 0x000059AC
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x000077BC File Offset: 0x000059BC
		[EditableInEditor]
		public bool neverGetsTired
		{
			get
			{
				return this.CELL_neverGetsTired.data;
			}
			set
			{
				this.CELL_neverGetsTired.data = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x000077CC File Offset: 0x000059CC
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x000077DC File Offset: 0x000059DC
		[ShowInEditor]
		public float creditCardUsageAmount
		{
			get
			{
				return this.CELL_creditCardUsageAmount.data;
			}
			set
			{
				this.CELL_creditCardUsageAmount.data = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000077EC File Offset: 0x000059EC
		[ShowInEditor]
		public bool isWaitingToBeTalkedTo
		{
			get
			{
				string text = "Sebastian";
				string text2 = text + "_talk_" + base.name;
				return this._dialogueRunner.IsWaitingOnEvent(text2);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00007820 File Offset: 0x00005A20
		[ShowInEditor]
		public Character.SleepinessState sleepinessState
		{
			get
			{
				if (this.sleepiness < 80f)
				{
					return Character.SleepinessState.FRESH;
				}
				if (this.sleepiness < 95f)
				{
					return Character.SleepinessState.CAN_NOT_RUN;
				}
				return Character.SleepinessState.SLOW;
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00007848 File Offset: 0x00005A48
		public float calculateFinalWalkSpeed()
		{
			float num = ((this.sleepinessState != Character.SleepinessState.SLOW) ? 1f : 0.75f);
			return this.walkSpeed * num;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000787C File Offset: 0x00005A7C
		public bool busy
		{
			get
			{
				return !(base.actionName == "") && !(base.actionName == "FallingAsleep") && !(base.actionName == "FallingAsleepInChair") && !(base.actionName == "Mixing") && !(base.actionName == "Hacking") && !(base.actionName == "Inspect") && !(base.actionName == "UsingComputer") && !(base.actionName == "Walking") && !(base.actionName == "Sitting") && !(base.actionName == "Sleeping") && !(base.actionName == "Dancing") && !(base.actionName == "Trumpeting") && !(base.actionName == "TalkingInTelephone");
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000079A0 File Offset: 0x00005BA0
		public bool hasHackdev
		{
			get
			{
				return this.hackdev != null;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000079B0 File Offset: 0x00005BB0
		public Hackdev hackdev
		{
			get
			{
				if (this.handItem is Hackdev)
				{
					return this.handItem as Hackdev;
				}
				Hackdev[] tingsOfTypeInRoom = this._tingRunner.GetTingsOfTypeInRoom<Hackdev>(this.inventoryRoomName);
				if (tingsOfTypeInRoom.Length > 0)
				{
					return tingsOfTypeInRoom[0];
				}
				return null;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000079FC File Offset: 0x00005BFC
		public Suitcase[] extraBags
		{
			get
			{
				List<Suitcase> list = new List<Suitcase>();
				if (this.handItem is Suitcase)
				{
					list.Add(this.handItem as Suitcase);
				}
				Suitcase[] tingsOfTypeInRoom = this._tingRunner.GetTingsOfTypeInRoom<Suitcase>(this.inventoryRoomName);
				list.AddRange(tingsOfTypeInRoom);
				return list.ToArray();
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00007A50 File Offset: 0x00005C50
		public CreditCard creditCard
		{
			get
			{
				if (this.handItem is CreditCard)
				{
					return this.handItem as CreditCard;
				}
				CreditCard[] tingsOfTypeInRoom = this._tingRunner.GetTingsOfTypeInRoom<CreditCard>(this.inventoryRoomName);
				if (tingsOfTypeInRoom.Length > 0)
				{
					return tingsOfTypeInRoom[0];
				}
				return null;
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00007A9C File Offset: 0x00005C9C
		public bool HasInventoryItemOfType(string pTingTypeName)
		{
			if (this.handItem != null && this.handItem.GetType().Name == pTingTypeName)
			{
				return true;
			}
			foreach (Ting ting in this.inventoryItems)
			{
				if (ting.GetType().Name == pTingTypeName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00007B0C File Offset: 0x00005D0C
		public string PrettyPrintableInfo()
		{
			return string.Concat(new string[]
			{
				base.position.ToString(),
				" Action: '",
				base.actionName,
				"' ",
				this.actionOtherObjectToStr
			});
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00007B58 File Offset: 0x00005D58
		private string actionOtherObjectToStr
		{
			get
			{
				if (base.actionOtherObject == null)
				{
					return "";
				}
				return "(" + base.actionOtherObject.name + ")";
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00007B90 File Offset: 0x00005D90
		public void SlurpIntoInternet(MimanTing pStartingTing)
		{
			if (base.actionName != "UsingComputer" && base.actionName != "Inspect")
			{
				D.Log(base.name + " can't slurp because she/he is " + base.actionName);
				return;
			}
			base.StartAction("SlurpingIntoComputer", pStartingTing, 2f, 2f);
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00007BFC File Offset: 0x00005DFC
		public int tileGroup
		{
			get
			{
				if (base.tile == null)
				{
					return -1;
				}
				return base.tile.group;
			}
		}

		// Token: 0x04000022 RID: 34
		public const float LONG_TIME = 99999f;

		// Token: 0x04000023 RID: 35
		public const int INVENTORY_SIZE = 20;

		// Token: 0x04000024 RID: 36
		public new static string TABLE_NAME = "Ting_Characters";

		// Token: 0x04000025 RID: 37
		public Character.CharacterEvent onNewPath;

		// Token: 0x04000026 RID: 38
		public Character.CharacterEvent onRemovedPath;

		// Token: 0x04000027 RID: 39
		public Character.OnNewHandItem onNewHandItem;

		// Token: 0x04000028 RID: 40
		public TimetableSpan lastTimetableSpan = TimetableSpan.NULL;

		// Token: 0x04000029 RID: 41
		private ValueEntry<string> CELL_handItemObjectName;

		// Token: 0x0400002A RID: 42
		private ValueEntry<WorldCoordinate> CELL_finalTargetPosition;

		// Token: 0x0400002B RID: 43
		private ValueEntry<string> CELL_finalTargetTing;

		// Token: 0x0400002C RID: 44
		private ValueEntry<WorldCoordinate> CELL_targetPositionInRoom;

		// Token: 0x0400002D RID: 45
		private ValueEntry<Character.WalkMode> CELL_walkMode;

		// Token: 0x0400002E RID: 46
		private ValueEntry<float> CELL_walkSpeed;

		// Token: 0x0400002F RID: 47
		private ValueEntry<float> CELL_walkTimer;

		// Token: 0x04000030 RID: 48
		private ValueEntry<int> CELL_walkIterator;

		// Token: 0x04000031 RID: 49
		private ValueEntry<float> CELL_charisma;

		// Token: 0x04000032 RID: 50
		private ValueEntry<float> CELL_smelliness;

		// Token: 0x04000033 RID: 51
		private ValueEntry<float> CELL_sleepiness;

		// Token: 0x04000034 RID: 52
		private ValueEntry<float> CELL_drunkenness;

		// Token: 0x04000035 RID: 53
		private ValueEntry<float> CELL_supremacy;

		// Token: 0x04000036 RID: 54
		private ValueEntry<float> CELL_happiness;

		// Token: 0x04000037 RID: 55
		private ValueEntry<float> CELL_corruption;

		// Token: 0x04000038 RID: 56
		private ValueEntry<int> CELL_friendLevel;

		// Token: 0x04000039 RID: 57
		private ValueEntry<GameTime> CELL_alarmTime;

		// Token: 0x0400003A RID: 58
		private ValueEntry<string[]> CELL_knowledge;

		// Token: 0x0400003B RID: 59
		private ValueEntry<string> CELL_timetableName;

		// Token: 0x0400003C RID: 60
		private ValueEntry<string> CELL_timetableMemory;

		// Token: 0x0400003D RID: 61
		private ValueEntry<float> CELL_timetableTimer;

		// Token: 0x0400003E RID: 62
		private ValueEntry<bool> CELL_talking;

		// Token: 0x0400003F RID: 63
		private ValueEntry<string> CELL_conversationTargetName;

		// Token: 0x04000040 RID: 64
		private ValueEntry<bool> CELL_sitting;

		// Token: 0x04000041 RID: 65
		private ValueEntry<bool> CELL_laying;

		// Token: 0x04000042 RID: 66
		private ValueEntry<bool> CELL_running;

		// Token: 0x04000043 RID: 67
		private ValueEntry<string> CELL_seatName;

		// Token: 0x04000044 RID: 68
		private ValueEntry<string> CELL_bedName;

		// Token: 0x04000045 RID: 69
		private ValueEntry<bool> CELL_waitForGift;

		// Token: 0x04000046 RID: 70
		private ValueEntry<bool> CELL_neverGetsTired;

		// Token: 0x04000047 RID: 71
		private ValueEntry<float> CELL_creditCardUsageAmount;

		// Token: 0x04000048 RID: 72
		private SmartWalkBehaviour _walkBehaviour;

		// Token: 0x04000049 RID: 73
		private Timetable _timetable;

		// Token: 0x0400004A RID: 74
		private TimetableRunner _timetableRunner;

		// Token: 0x0400004B RID: 75
		public bool rememberToHackComputerAfterSittingDown;

		// Token: 0x0400004C RID: 76
		public Door rememberToUseDoorAfterWaitingPolitely;

		// Token: 0x0400004D RID: 77
		private static string[] buySentences = new string[] { "Hej, kan jag få köpa något?你好，我能买点东西吗?", "Det vanliga tack老样子，谢谢", "Tjena!嘿！", "Jag tar en我来一个", "En till om jag får be可以的话，再来一个" };

		// Token: 0x0400004E RID: 78
		private static int buySentenceCounter = Randomizer.GetIntValue(0, 100);

		// Token: 0x0400004F RID: 79
		private static List<string> canBeBotheredWhen = new List<string> { "", "Walking", "GettingUpFromSeat" };

		// Token: 0x04000050 RID: 80
		private static List<string> noTimeToTalkLines = new List<string> { "Ursäkta, jag har inte tid att prata nu抱歉，我现在没时间说话", "Kan vi ta det lite senare?我们能待会儿再说吗？", "Va? Vi kan ta det imorgon kanske?啥？也许明天再说？", "Nej, jag vill inte prata just nu不，我现在不想说话", "?!", "Jag hinner inte nu我现在没空", "Oj, jag har inte tid att snacka nu哎呀，我现在没时间聊天", "Huh?哈？" };

		// Token: 0x02000008 RID: 8
		public enum WalkMode
		{
			// Token: 0x04000052 RID: 82
			NO_TARGET,
			// Token: 0x04000053 RID: 83
			WALK_TO_POINT,
			// Token: 0x04000054 RID: 84
			WALK_TO_TING_AND_INTERACT,
			// Token: 0x04000055 RID: 85
			WALK_TO_TING_AND_HACK,
			// Token: 0x04000056 RID: 86
			WALK_TO_TING_AND_USE_HAND_ITEM
		}

		// Token: 0x02000009 RID: 9
		public enum SleepinessState
		{
			// Token: 0x04000058 RID: 88
			FRESH,
			// Token: 0x04000059 RID: 89
			CAN_NOT_RUN,
			// Token: 0x0400005A RID: 90
			SLOW
		}

		// Token: 0x0200000A RID: 10
		// (Invoke) Token: 0x06000105 RID: 261
		public delegate void CharacterEvent();

		// Token: 0x0200000B RID: 11
		// (Invoke) Token: 0x06000109 RID: 265
		public delegate void OnNewHandItem(string pNameOfNewHandItem, bool pGivingItemToSomeoneElse);
	}
}
