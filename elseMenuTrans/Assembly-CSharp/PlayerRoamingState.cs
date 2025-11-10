using System;
using System.Collections.Generic;
using GameTypes;
using GameWorld2;
using GrimmLib;
using RelayLib;
using TingTing;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class PlayerRoamingState : GameViewState
{
	// Token: 0x060001A0 RID: 416 RVA: 0x0000B120 File Offset: 0x00009320
	public PlayerRoamingState(bool pStoryStart)
	{
		this._storyStart = pStoryStart;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000B210 File Offset: 0x00009410
	public override void OnEnterBegin()
	{
		RunGameWorld.instance.DetectTingsEnteringRoom();
		if (this.avatar != null)
		{
			Character avatar = this.avatar;
			avatar.onNewAction = (Ting.OnNewAction)Delegate.Combine(avatar.onNewAction, new Ting.OnNewAction(this.OnAvatarChangesAction));
			this.avatar.AddDataListener<WorldCoordinate>("position", new ValueEntry<WorldCoordinate>.DataChangeHandler(this.OnAvatarPositionChanged));
			this.avatar.AddDataListener<string>("conversationTargetName", new ValueEntry<string>.DataChangeHandler(this.OnAvatarChangesConversationTarget));
			Character avatar2 = this.avatar;
			avatar2.onNewHandItem = (Character.OnNewHandItem)Delegate.Combine(avatar2.onNewHandItem, new Character.OnNewHandItem(this.OnNewHandItem));
		}
		this._cursorDriverPrefab = Resources.Load("CursorClickFX") as GameObject;
		this._cursorSelectPrefab = Resources.Load("CursorSelect") as GameObject;
		this._actionMenu = RunGameWorld.instance.GetComponent<ActionMenu>();
		this._actionMenu.onActionMenuPressed = new ActionMenu.OnActionMenuButtonPressed(this.OnActionMenuPressed);
		this.BuildActionMenu();
		this._actionMenu.alpha = 0f;
		MainMenu.SetVolume(0f);
		SoundDictionary.LoadMultiSound("MouseClick", "MuseClick sound", 4);
		SoundDictionary.LoadSingleSound("MouseSelect", "MuseSelect sound 0");
		base.controls.world.dialogueRunner.AddFocusConversationListener(new DialogueRunner.OnFocusConversation(this.OnConversationFocus));
		base.controls.world.dialogueRunner.AddOnEventListener(new DialogueRunner.OnEvent(this.OnEvent));
		RoomChanger roomChanger = base.controls.roomChanger;
		roomChanger.onRoomHasChanged = (Action<string>)Delegate.Combine(roomChanger.onRoomHasChanged, new Action<string>(this.OnRoomChanged));
		if (this._storyStart)
		{
			if (base.controls.world.dialogueRunner.HasConversation("StoryStart"))
			{
				base.controls.world.dialogueRunner.StartConversation("StoryStart");
			}
			else
			{
				Debug.Log("No StoryStart");
			}
		}
		else if (base.controls.world.settings.focusedDialogue != string.Empty)
		{
			Debug.Log("Will resume saved focusedDialogue " + base.controls.world.settings.focusedDialogue);
			this.OnConversationFocus(base.controls.world.settings.focusedDialogue);
		}
		this._glowEffect = Camera.main.GetComponent<GlowEffect>();
		this._moneyChangeAudioSource = Camera.main.transform.Find("MoneyChange").GetComponent<AudioSource>();
		SoundDictionary.LoadSingleSound("MoneyChange", "MoneyChanged Sound");
		WorldSettings settings = base.controls.world.settings;
		settings.onCameraTarget = (WorldSettings.OnCameraTarget)Delegate.Combine(settings.onCameraTarget, new WorldSettings.OnCameraTarget(this.OnCameraTarget));
		base.controls.fade.Black();
		float num = 0f;
		if (this.GetMoneyLevel(out num))
		{
			D.Log("Found initial displayed cash amount!");
			this._displayedCashAmount = (int)num;
		}
		else
		{
			D.Log("Didn't find initial displayed cash amount, will use default ($565)");
			this._displayedCashAmount = 565;
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000B530 File Offset: 0x00009730
	private void OnEvent(string pEventName)
	{
		if (pEventName == "TitleSequence")
		{
			if (this._dialogueSubState != null)
			{
				this._dialogueSubState.OnExitDialogue();
				this._dialogueSubState.exit = true;
			}
			base.PushState(new TitleSequence("ARRIVED_IN_DORISBURG", 55f, this._actionMenu));
		}
		else if (pEventName == "EndingSequence")
		{
			base.PushState(new TitleSequence("LEFT_DORISBURG", 5000f, this._actionMenu));
		}
		else if (pEventName == "InternetEndingSequence")
		{
			base.PushState(new TitleSequence("INTERNET_ENDING", 5000f, this._actionMenu));
		}
		else if (pEventName == "Boom" || pEventName == "SmallBoom")
		{
			base.controls.camera.Shake(1f, 1f, true);
			Time.timeScale = 1f;
		}
		else if (this.avatar != null && pEventName == this.avatar.name + "_TookFastForwardDrug")
		{
			Time.timeScale = 10f;
		}
		else if (pEventName == "SlowFadeIn")
		{
			Debug.Log("Will make a slow fade in");
			base.controls.fade.alpha = 0f;
			base.controls.fade.speed = 0.2f;
			base.controls.fade.FadeToTransparent();
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000B6C8 File Offset: 0x000098C8
	private void SetupLoggerListenerForTing(string pName)
	{
		base.controls.world.tingRunner.GetTing(pName).logger.AddListener(new D.LogHandler(Debug.Log));
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000B704 File Offset: 0x00009904
	private void SetupLoggerListenerForDialogueRunner()
	{
		base.controls.world.dialogueRunner.logger.AddListener(new D.LogHandler(Debug.Log));
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000B738 File Offset: 0x00009938
	public override void OnExitBegin()
	{
		if (this.avatar != null)
		{
			Character avatar = this.avatar;
			avatar.onNewAction = (Ting.OnNewAction)Delegate.Remove(avatar.onNewAction, new Ting.OnNewAction(this.OnAvatarChangesAction));
			this.avatar.RemoveDataListener<WorldCoordinate>("position", new ValueEntry<WorldCoordinate>.DataChangeHandler(this.OnAvatarPositionChanged));
			this.avatar.RemoveDataListener<string>("conversationTargetName", new ValueEntry<string>.DataChangeHandler(this.OnAvatarChangesConversationTarget));
			Character avatar2 = this.avatar;
			avatar2.onNewHandItem = (Character.OnNewHandItem)Delegate.Remove(avatar2.onNewHandItem, new Character.OnNewHandItem(this.OnNewHandItem));
		}
		base.controls.world.dialogueRunner.RemoveFocusConversationListener(new DialogueRunner.OnFocusConversation(this.OnConversationFocus));
		base.controls.world.dialogueRunner.RemoveOnEventListener(new DialogueRunner.OnEvent(this.OnEvent));
		RoomChanger roomChanger = base.controls.roomChanger;
		roomChanger.onRoomHasChanged = (Action<string>)Delegate.Remove(roomChanger.onRoomHasChanged, new Action<string>(this.OnRoomChanged));
		WorldSettings settings = base.controls.world.settings;
		settings.onCameraTarget = (WorldSettings.OnCameraTarget)Delegate.Remove(settings.onCameraTarget, new WorldSettings.OnCameraTarget(this.OnCameraTarget));
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000B878 File Offset: 0x00009A78
	private void OnCameraTarget(string pLookFromPointName, string pTargetName)
	{
		if (pLookFromPointName == string.Empty || pTargetName == string.Empty)
		{
			base.controls.camera.ExitFixedCamera();
		}
		else
		{
			Transform transformWithName = this.GetTransformWithName(pLookFromPointName);
			Transform transformWithName2 = this.GetTransformWithName(pTargetName);
			if (transformWithName == null)
			{
				Debug.Log("Can't find the game object with name " + transformWithName + " to look with camera from");
			}
			else if (transformWithName2 == null)
			{
				Debug.Log("Can't find the game object with name " + pTargetName + " to focus camera on");
			}
			else
			{
				D.isNull(base.controls.camera, "camera is null");
				D.isNull(this._avatarShell, "_avatarShell is null");
				D.isNull(transformWithName2, "target is null");
				base.controls.camera.EnterFixedCamera(transformWithName, transformWithName2);
			}
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000B958 File Offset: 0x00009B58
	private Transform GetTransformWithName(string pName)
	{
		Transform[] array = global::UnityEngine.Object.FindObjectsOfType<Transform>();
		foreach (Transform transform in array)
		{
			if (pName == transform.name)
			{
				return transform;
			}
		}
		Debug.Log("Failed to find transform with name " + pName);
		return null;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000B9AC File Offset: 0x00009BAC
	private void OnNewHandItem(string pNewItem, bool pGivingItemToSomeoneElse)
	{
		this.BuildActionMenu();
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0000B9B4 File Offset: 0x00009BB4
	private void OnAvatarChangesConversationTarget(string pOld, string pNew)
	{
		this.BuildActionMenu();
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0000B9BC File Offset: 0x00009BBC
	public override GameViewState.RETURN OnEnterLoop()
	{
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x060001AC RID: 428 RVA: 0x0000B9C0 File Offset: 0x00009BC0
	private Vector2 mouseScreenPosition
	{
		get
		{
			return new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000B9F4 File Offset: 0x00009BF4
	public override void OnGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this._lastRayStart, this._lastRayEnd);
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000BA14 File Offset: 0x00009C14
	public override void OnUpdate()
	{
		if (Mathf.Approximately(Time.deltaTime, 0f))
		{
			Debug.Log("Not updating PlayerRoamingState, delta time is 0.0f");
			return;
		}
		if (Camera.main == null)
		{
			Debug.Log("Camera.main == null");
			return;
		}
		this.CheckForRoomChange();
		if (this.skipUpdateUntilRoomIsLoaded)
		{
			return;
		}
		if (this.avatar == null)
		{
			return;
		}
		if (this._avatarShell == null)
		{
			this._avatarShell = ShellManager.GetShellWithName(this.avatar.name) as CharacterShell;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.RaycastAll(ray, 300f);
		MainMenu.SetVolume(1f - base.controls.fade.alpha);
		bool flag = (float)Screen.height - Input.mousePosition.y > 0f && (float)Screen.height - Input.mousePosition.y < (float)Screen.height && Input.mousePosition.x < (float)Screen.width && Input.mousePosition.x > 0f;
		if (this._dialogueSubState != null)
		{
			this._dialogueSubState.OnUpdate();
			if (this._dialogueSubState.exit)
			{
				this._dialogueSubState.OnExitDialogue();
				this._dialogueSubState = null;
				this._ignoreMouseInputTimer = 0.5f;
			}
		}
		else
		{
			if (this.HoveringActionMenu())
			{
				if (Input.GetMouseButtonDown(0))
				{
					Debug.Log("Pressed mouse while hovering action menu, will not call HandleInput()");
				}
			}
			else
			{
				this.MaybeHandleMouseInput(flag);
			}
			this.HandleKeyInput();
		}
		this.ShakeCameraWhenNextToEngines();
		this.CheckForMoneyChange();
		if (KeyboardInput.KEY_PRESET_Quit() && !base.controls.fade.isFading && base.IsTopState())
		{
			base.PushState(new PlayerPauseMenu(this, this._actionMenu));
		}
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F10) && !base.controls.fade.isFading)
		{
			base.PushState(new PauseMenu(this, this._actionMenu));
		}
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F5))
		{
			GC.Collect();
			Resources.UnloadUnusedAssets();
			Debug.Log("Manually ran GC.Collect()!");
		}
		if (this._avatarShell != null)
		{
			if (base.IsTopState())
			{
				base.controls.depthOfField.focalTransform = this._avatarShell.transform;
			}
			PlayerRoamingState.ControlCamera(base.controls.camera, flag, base.controls.world.settings.cameraAutoRotateSpeed);
		}
		if (this._clockItem != null)
		{
			this._clockItem.text = base.controls.world.settings.gameTimeClock.ToStringWithoutDayAndSeconds();
		}
		if (base.controls.roomChanger.currentRoom == "Internet")
		{
			this._glowEffect.enabled = true;
			PlayerRoamingState.CycleGlowEffectColor(this._glowEffect);
		}
		else
		{
			this._glowEffect.enabled = false;
		}
		this._physicalGuiButtonWasPressed = false;
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0000BD6C File Offset: 0x00009F6C
	private bool GetMoneyLevel(out float fcashAmount)
	{
		object obj = 0f;
		Computer computer = base.controls.world.tingRunner.GetTing("FinanceComputer") as Computer;
		if (computer == null)
		{
			D.Log("No finance computer");
			fcashAmount = 0f;
			return false;
		}
		this.foundBankEntryForAvatar = computer.memory.data.TryGetValue(this.avatar.name, out obj);
		if (obj == null)
		{
			fcashAmount = 0f;
			return false;
		}
		if (obj.GetType() == typeof(float))
		{
			fcashAmount = (float)obj;
		}
		else if (obj.GetType() == typeof(double))
		{
			fcashAmount = (float)((double)obj);
		}
		else
		{
			D.Log("Invalid type on fcashAmount");
			fcashAmount = 0f;
		}
		return true;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x0000BE48 File Offset: 0x0000A048
	private float CheckForMoneyChange()
	{
		float num = 0f;
		if (this.GetMoneyLevel(out num))
		{
			if ((float)this._displayedCashAmount < num)
			{
				this._displayedCashAmount += Math.Min((int)num - this._displayedCashAmount, 10);
				this.BuildActionMenu();
				this.PlayMoneyChangeSound();
			}
			else if ((float)this._displayedCashAmount > num)
			{
				this._displayedCashAmount -= Math.Min(this._displayedCashAmount - (int)num, 10);
				this.BuildActionMenu();
				this.PlayMoneyChangeSound();
			}
		}
		return num;
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0000BEDC File Offset: 0x0000A0DC
	private void PlayMoneyChangeSound()
	{
		if (!this._moneyChangeAudioSource.isPlaying || (double)this._moneyChangeAudioSource.time > 0.15)
		{
			SoundDictionary.PlaySound("MoneyChange", this._moneyChangeAudioSource);
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000BF24 File Offset: 0x0000A124
	public static void CycleGlowEffectColor(GlowEffect pGlowEffect)
	{
		float num = 0.3f;
		pGlowEffect.glowTint = new Color(Mathf.Abs(Mathf.Sin(Time.time * 0.2f)) * num, Mathf.Abs(Mathf.Cos(Time.time * 0.2f)) * num, Mathf.Abs(Mathf.Sin(Time.time * 0.5f)) * num);
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0000BF88 File Offset: 0x0000A188
	private void HandleKeyInput()
	{
		if (this.avatar == null || this.avatar.busy || this.avatar.laying)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.H) && this.avatar.handItem != null && this.avatar.hasHackdev)
		{
			this.avatar.Hack(this.avatar.handItem);
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			this._showInventory = !this._showInventory;
			this.BuildActionMenu();
		}
		if (Input.GetKeyDown(KeyCode.M) && this.avatar.HasInventoryItemOfType("Map"))
		{
			this.StartMapState();
		}
		if (this.avatar.handItem != null)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.InteractWithHandItem();
			}
			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				this.avatar.DropHandItem();
			}
			if (Input.GetKeyDown(KeyCode.G))
			{
				this.avatar.GiveHandItemToPerson();
			}
			if (Input.GetKeyDown(KeyCode.P) && this.avatar.handItem != null)
			{
				this.avatar.PutHandItemIntoInventory();
			}
			if (Input.GetKeyDown(KeyCode.O))
			{
				this._showInventory = !this._showInventory;
			}
			if (Input.GetKeyDown(KeyCode.Alpha1) && this.avatar.inventoryItems.Length > 0)
			{
				this.avatar.TakeOutInventoryItem(this.avatar.inventoryItems[0]);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2) && this.avatar.inventoryItems.Length > 1)
			{
				this.avatar.TakeOutInventoryItem(this.avatar.inventoryItems[1]);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3) && this.avatar.inventoryItems.Length > 2)
			{
				this.avatar.TakeOutInventoryItem(this.avatar.inventoryItems[2]);
			}
			if (Input.GetKeyDown(KeyCode.Alpha4) && this.avatar.inventoryItems.Length > 3)
			{
				this.avatar.TakeOutInventoryItem(this.avatar.inventoryItems[3]);
			}
			if (Input.GetKeyDown(KeyCode.Alpha5) && this.avatar.inventoryItems.Length > 4)
			{
				this.avatar.TakeOutInventoryItem(this.avatar.inventoryItems[4]);
			}
			if (Input.GetKeyDown(KeyCode.Alpha6) && this.avatar.inventoryItems.Length > 5)
			{
				this.avatar.TakeOutInventoryItem(this.avatar.inventoryItems[5]);
			}
			if (Input.GetKeyDown(KeyCode.Alpha7) && this.avatar.inventoryItems.Length > 6)
			{
				this.avatar.TakeOutInventoryItem(this.avatar.inventoryItems[6]);
			}
			if (Input.GetKeyDown(KeyCode.Alpha8) && this.avatar.inventoryItems.Length > 7)
			{
				this.avatar.TakeOutInventoryItem(this.avatar.inventoryItems[7]);
			}
			if (Input.GetKeyDown(KeyCode.Alpha9) && this.avatar.inventoryItems.Length > 8)
			{
				this.avatar.TakeOutInventoryItem(this.avatar.inventoryItems[8]);
			}
			if (Input.GetKeyDown(KeyCode.Alpha0) && this.avatar.inventoryItems.Length > 9)
			{
				this.avatar.TakeOutInventoryItem(this.avatar.inventoryItems[9]);
			}
		}
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0000C304 File Offset: 0x0000A504
	private void OnActionMenuPressed(string pIdentifier)
	{
		if (this.avatar == null)
		{
			return;
		}
		if (this.avatar.busy || this.avatar.laying)
		{
			base.controls.world.settings.Notify(this.avatar.name, this.avatar.name + " is busy");
			return;
		}
		MimanTing handItem = this.avatar.handItem;
		if (pIdentifier.Contains("takeout_"))
		{
			string text = pIdentifier.Substring(8);
			Ting ting = base.controls.world.tingRunner.GetTing(text);
			this.avatar.TakeOutInventoryItem(ting);
			this._showInventory = false;
		}
		switch (pIdentifier)
		{
		case "use":
			this.InteractWithHandItem();
			break;
		case "hack":
			if (handItem != null)
			{
				this.avatar.Hack(handItem);
			}
			break;
		case "drop":
			if (handItem != null)
			{
				this.avatar.DropHandItem();
			}
			break;
		case "putaway":
			if (handItem != null)
			{
				this.avatar.PutHandItemIntoInventory();
			}
			break;
		case "give":
			if (handItem != null && this.avatar.conversationTarget != null)
			{
				this.avatar.GiveHandItemToPerson();
			}
			break;
		case "inventory":
			this._showInventory = true;
			this._whichInventory = string.Empty;
			break;
		case "close":
			this._showInventory = false;
			break;
		case "dance":
			this.avatar.StartAction("Dancing", null, 99999f, 99999f);
			break;
		}
		if (pIdentifier.StartsWith("inventory_"))
		{
			this._showInventory = true;
			this._whichInventory = pIdentifier.Substring(10);
			Debug.Log("Will show inventory for bag: '" + this._whichInventory + "'");
		}
		this.BuildActionMenu();
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x0000C584 File Offset: 0x0000A784
	private void BuildActionMenu()
	{
		if (this._showInventory)
		{
			this.BuildInventoryMenu();
		}
		else
		{
			this.BuildInteractionMenu();
		}
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x0000C5A4 File Offset: 0x0000A7A4
	private void AddClockAndCreditLevel(List<ActionMenuItem> items)
	{
		GameTime gameTimeClock = base.controls.world.settings.gameTimeClock;
		this._clockItem = new ActionMenuItem
		{
			text = gameTimeClock.ToStringWithoutDayAndSeconds(),
			type = ActionMenuItemType.SECTION_TITLE
		};
		items.Add(this._clockItem);
		items.Add(new ActionMenuItem
		{
			type = ActionMenuItemType.VERTICAL_SPACE
		});
		if (!this.foundBankEntryForAvatar)
		{
			Debug.Log("Couldn't find bank entry for avatar.");
		}
		else if (this.avatar.creditCard == null)
		{
			Debug.Log("Couldn't find credit card for avatar.");
		}
		else
		{
			ActionMenuItem actionMenuItem = new ActionMenuItem
			{
				text = "$" + this._displayedCashAmount.ToString(),
				type = ActionMenuItemType.SECTION_TITLE
			};
			items.Add(actionMenuItem);
			items.Add(new ActionMenuItem
			{
				type = ActionMenuItemType.VERTICAL_SPACE
			});
		}
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0000C688 File Offset: 0x0000A888
	private void BuildInteractionMenu()
	{
		if (this.avatar == null)
		{
			Debug.Log("Avatar is null, can't build interaction menu");
			return;
		}
		List<ActionMenuItem> list = new List<ActionMenuItem>();
		this.AddClockAndCreditLevel(list);
		MimanTing handItem = this.avatar.handItem;
		if (!this.avatar.busy && !this.avatar.laying)
		{
			list.Add(new ActionMenuItem
			{
				identifier = "inventory",
				text = "open bag"
			});
			foreach (Suitcase suitcase in this.avatar.extraBags)
			{
				list.Add(new ActionMenuItem
				{
					identifier = "inventory_" + suitcase.inventoryRoomName,
					text = "open " + suitcase.name
				});
			}
			if (this.CanDance())
			{
				list.Add(new ActionMenuItem
				{
					identifier = "dance",
					text = "dance"
				});
			}
		}
		list.Add(new ActionMenuItem
		{
			type = ActionMenuItemType.VERTICAL_SPACE
		});
		if (this.avatar.handItem != null)
		{
			list.Add(new ActionMenuItem
			{
				text = handItem.tooltipName + this.MaybeShowUserDefinedLabel(this.avatar.handItem),
				type = ActionMenuItemType.SECTION_TITLE
			});
			if (!this.avatar.busy && !this.avatar.laying)
			{
				if (this.avatar.CanInteractWith(handItem) && !PlayerRoamingState._dontShowUseButtonForTheseItemTypes.Contains(handItem.GetType()) && (this._dialogueSubState == null || !this._dialogueSubState.isFocused || handItem == null || handItem.GetType() != typeof(Teleporter)))
				{
					list.Add(new ActionMenuItem
					{
						identifier = "use",
						text = handItem.verbDescription
					});
				}
				if (this.avatar.conversationTarget != null)
				{
					list.Add(new ActionMenuItem
					{
						identifier = "give",
						text = "give"
					});
				}
				if (this.avatar.conversationTarget == null || this.avatar.handItem is Suitcase)
				{
					list.Add(new ActionMenuItem
					{
						identifier = "drop",
						text = "put down"
					});
				}
				list.Add(new ActionMenuItem
				{
					identifier = "putaway",
					text = "put into bag"
				});
				if (this.CanHackTing(handItem))
				{
					list.Add(new ActionMenuItem
					{
						identifier = "hack",
						text = ((!(handItem is Floppy)) ? "hack [h]" : "read")
					});
				}
			}
		}
		this._actionMenu.items = list.ToArray();
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000C9AC File Offset: 0x0000ABAC
	private bool CanDance()
	{
		if (this.avatar.actionName == "Dancing")
		{
			return false;
		}
		if (this.avatar.sitting || this.avatar.laying)
		{
			return false;
		}
		if (this.avatar.busy)
		{
			return false;
		}
		if (this._avatarShell == null)
		{
			return false;
		}
		this._lastDanceCheck = Time.time;
		List<Shell> shellsInScene = ShellManager.GetShellsInScene();
		foreach (Shell shell in shellsInScene)
		{
			if (shell.name.Contains("Dance") && Vector3.Distance(this._avatarShell.transform.position, shell.transform.position) < 10f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000CAC4 File Offset: 0x0000ACC4
	private void BuildInventoryMenu()
	{
		List<ActionMenuItem> list = new List<ActionMenuItem>();
		this.AddClockAndCreditLevel(list);
		if (this.avatar.laying)
		{
			return;
		}
		list.Add(new ActionMenuItem
		{
			identifier = "close",
			text = "close bag"
		});
		list.Add(new ActionMenuItem
		{
			type = ActionMenuItemType.VERTICAL_SPACE
		});
		Ting[] array;
		if (this._whichInventory != string.Empty)
		{
			Room roomUnsafe = base.controls.world.roomRunner.GetRoomUnsafe(this._whichInventory);
			if (roomUnsafe == null)
			{
				this._showInventory = false;
				D.Log("Failed to get inventory room '" + this._whichInventory + "', it is null");
				return;
			}
			array = roomUnsafe.GetTings().ToArray();
		}
		else
		{
			array = this.avatar.inventoryItems;
		}
		if (array == null)
		{
			D.Log("Failed to get inventory items, items is null");
			this._showInventory = false;
			return;
		}
		foreach (Ting ting in array)
		{
			MimanTing mimanTing = ting as MimanTing;
			list.Add(new ActionMenuItem
			{
				identifier = "takeout_" + ting.name,
				text = ting.tooltipName + this.MaybeShowUserDefinedLabel(mimanTing)
			});
		}
		this._actionMenu.items = list.ToArray();
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000CC38 File Offset: 0x0000AE38
	private string MaybeShowUserDefinedLabel(MimanTing mimanItem)
	{
		return (!(mimanItem.userDefinedLabel != string.Empty)) ? string.Empty : (" / " + mimanItem.userDefinedLabel);
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000CC6C File Offset: 0x0000AE6C
	private bool HoveringActionMenu()
	{
		Vector3 mousePosition = Input.mousePosition;
		Vector2 vector = new Vector2(mousePosition.x, (float)Screen.height - mousePosition.y);
		return this._actionMenu.GetBounds().Contains(vector);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000CCB0 File Offset: 0x0000AEB0
	public static void ControlCamera(GreatCamera pCamera, bool mouseIsWithinScreenBounds, float pAutoRotateSpeed)
	{
		float num = 7f;
		float num2 = 100f;
		float num3 = 140f;
		if (!mouseIsWithinScreenBounds)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
		{
			pCamera.Input_StartDrag();
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
		{
			pCamera.Input_EndDrag();
		}
		float num4 = 0.03f;
		if (Input.GetMouseButton(1))
		{
			pCamera.Input_Drag(Input.GetAxis("Horizontal") * num4 * num, Input.GetAxis("Vertical") * num4 * num);
		}
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			pCamera.Input_Drag(Time.deltaTime * -num3, 0f);
		}
		else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			pCamera.Input_Drag(Time.deltaTime * num3, 0f);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			pCamera.Input_ZoomDiscrete(1);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			pCamera.Input_ZoomDiscrete(-1);
		}
		float num5 = Input.GetAxis("Mouse ScrollWheel") * num4 * num2;
		pCamera.Input_ZoomDiscrete((int)num5);
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x060001BD RID: 445 RVA: 0x0000CE74 File Offset: 0x0000B074
	private Tram tramAvatarIsIn
	{
		get
		{
			GameObject gameObject = GameObject.Find("TramController");
			if (gameObject != null)
			{
				return gameObject.GetComponent<TramController>().tram;
			}
			return null;
		}
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0000CEA8 File Offset: 0x0000B0A8
	private void MaybeHandleMouseInput(bool mouseIsWithinScreenBounds)
	{
		if (this._ignoreMouseInputTimer > 0f)
		{
			this._ignoreMouseInputTimer -= Time.deltaTime;
		}
		if (base.IsTopState() && this._ignoreMouseInputTimer <= 0f && mouseIsWithinScreenBounds)
		{
			this.HandleMouseInput();
		}
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000CF00 File Offset: 0x0000B100
	private void HandleMouseInput()
	{
		Shell interactableShellUnderMouse = this.GetInteractableShellUnderMouse();
		MimanTing mimanTing = null;
		if (interactableShellUnderMouse != null && !(interactableShellUnderMouse is PointShell))
		{
			mimanTing = interactableShellUnderMouse.ting;
			if (base.IsTopState())
			{
				this.RefreshTooltipText(interactableShellUnderMouse, mimanTing);
				this.ShowCursorSelect(interactableShellUnderMouse.transform.position);
			}
		}
		else
		{
			this.HideCursorSelect();
		}
		WorldCoordinate worldCoordinate = new WorldCoordinate("NOT SET", new IntPoint(0, 0));
		bool worldTilePoint = this.GetWorldTilePoint(out worldCoordinate);
		bool flag = true;
		if (Input.GetMouseButtonDown(0))
		{
			if (!this._physicalGuiButtonWasPressed && this.mouseScreenPosition.y > 0f && this.mouseScreenPosition.y < (float)Screen.height && this.mouseScreenPosition.x > 0f && this.mouseScreenPosition.x < (float)Screen.width && this.avatar != null)
			{
				if (this.avatar.actionName == "Sleeping")
				{
					Debug.Log("Avatar is sleeping and can't move");
				}
				else if (this.avatar.busy)
				{
					Debug.Log("Avatar is busy with action: " + this.avatar.actionName);
					this.avatar.rememberToUseDoorAfterWaitingPolitely = null;
					if (this.avatar.actionName == "WalkingThroughDoor" || this.avatar.actionName == "WalkingThroughPortal" || this.avatar.actionName == "GettingTased" || this.avatar.actionName == "FallingAsleep" || this.avatar.actionName == "FallingAsleepInChair" || this.avatar.actionName == "FallAsleepFromStanding")
					{
						Debug.Log("Can't buffer when leaving room or falling asleep");
						this._bufferedWalkTarget = WorldCoordinate.NONE;
						this._bufferedInteractionTarget = null;
					}
					else
					{
						string text = "NOT_SET";
						if (this.avatar.actionOtherObject is Door)
						{
							text = (this.avatar.actionOtherObject as Door).targetDoor.room.name;
						}
						else if (this.avatar.actionOtherObject is Portal)
						{
							text = (this.avatar.actionOtherObject as Portal).targetPortal.room.name;
						}
						else if (this.avatar.actionOtherObject != null)
						{
							text = this.avatar.actionOtherObject.room.name;
						}
						if (mimanTing != null)
						{
							if (this.avatar.actionOtherObject != mimanTing && text == mimanTing.room.name)
							{
								this.BotherIfPossible(mimanTing);
								this._bufferedInteractionTarget = mimanTing;
								this._bufferedWalkTarget = WorldCoordinate.NONE;
								this.PlayMouseSelectSound();
								this._ignoreNextSelectSound = true;
							}
						}
						else if (worldTilePoint)
						{
							this._bufferedInteractionTarget = null;
							this._bufferedWalkTarget = worldCoordinate;
							this.SpawnCursorDriverEffect(this._bufferedWalkTarget.localPosition);
							this._ignoreNextMouseDriverSpawn = true;
						}
					}
				}
				else
				{
					this.avatar.rememberToUseDoorAfterWaitingPolitely = null;
					if (mimanTing != null)
					{
						this.BotherIfPossible(mimanTing);
						this.avatar.WalkToTingAndInteract(mimanTing);
					}
					else if (worldTilePoint && worldCoordinate != this.avatar.targetPositionInRoom)
					{
						this.avatar.WalkTo(worldCoordinate);
					}
					this._bufferedInteractionTarget = null;
					this._bufferedWalkTarget = WorldCoordinate.NONE;
				}
			}
			this._mousePressedTimer += Time.deltaTime;
		}
		if (!this.avatar.busy)
		{
			if (this._bufferedInteractionTarget != null)
			{
				this.avatar.WalkToTingAndInteract(this._bufferedInteractionTarget);
			}
			else if (this._bufferedWalkTarget != WorldCoordinate.NONE && this._bufferedWalkTarget.roomName != string.Empty)
			{
				this.avatar.WalkTo(this._bufferedWalkTarget);
			}
			this._bufferedInteractionTarget = null;
			this._bufferedWalkTarget = WorldCoordinate.NONE;
			if (this._bufferedRun)
			{
				this._bufferedRun = false;
				this.SetAvatarShellRunning(true);
			}
		}
		if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && this._avatarShell != null)
		{
			if (this._doubleClickTimer > 0f)
			{
				if (this.avatar.sleepinessState == Character.SleepinessState.FRESH)
				{
					if (this.avatar.busy)
					{
						this._bufferedRun = true;
					}
					else
					{
						this.SetAvatarShellRunning(true);
					}
					flag = false;
				}
				else
				{
					base.controls.world.settings.Notify(this.avatar.name, "Too tired to run");
				}
			}
			else
			{
				this.SetAvatarShellRunning(false);
			}
			this._doubleClickTimer = 0.5f;
		}
		if (this._doubleClickTimer > 0f)
		{
			this._doubleClickTimer -= Time.deltaTime;
		}
		if (this._mouseDriverTimer > 0f)
		{
			this._mouseDriverTimer -= Time.deltaTime;
		}
		if (this._mouseSelectSoundSpacing > 0f)
		{
			this._mouseSelectSoundSpacing -= Time.deltaTime;
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (this._mousePressedTimer >= 0.5f && this.avatar.finalTargetTing == null)
			{
				this.avatar.CancelWalking();
			}
			this._mousePressedTimer = 0f;
		}
		if (this.avatar.targetPositionInRoom != this._cachedTargetPositionInRoom && this.avatar.targetPositionInRoom != WorldCoordinate.NONE)
		{
			if (this.avatar.finalTargetTing == null && flag)
			{
				this.SpawnCursorDriverEffect(this.avatar.targetPositionInRoom.localPosition);
			}
			else if (this.avatar.finalTargetTing != null)
			{
				this.PlayMouseSelectSound();
			}
		}
		this._cachedTargetPositionInRoom = this.avatar.targetPositionInRoom;
		if (Input.GetMouseButtonDown(1))
		{
			this.avatar.rememberToUseDoorAfterWaitingPolitely = null;
			if (this.avatar.busy)
			{
				Debug.Log("Avatar is busy and can't hack or interact using hand item");
			}
			else if (this.CanUseHandItemOnTing(mimanTing))
			{
				this.avatar.WalkToTingAndUseHandItem(mimanTing);
			}
			else if (this.CanHackTing(mimanTing))
			{
				this.avatar.WalkToTingAndHack(mimanTing);
			}
		}
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000D5BC File Offset: 0x0000B7BC
	private void SetAvatarShellRunning(bool pValue)
	{
		if (this._avatarShell != null)
		{
			this._avatarShell.running = pValue;
		}
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000D5DC File Offset: 0x0000B7DC
	private void PlayMouseSelectSound()
	{
		if (this._ignoreNextSelectSound)
		{
			this._ignoreNextSelectSound = false;
			return;
		}
		if (this._mouseSelectSoundSpacing > 0f)
		{
			return;
		}
		if (this._cursorSelect != null)
		{
			SoundDictionary.PlaySound("MouseSelect", this._cursorSelect.audio);
			this._mouseSelectSoundSpacing = 0.3f;
		}
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000D640 File Offset: 0x0000B840
	private void ShakeCameraWhenNextToEngines()
	{
		if (this._avatarShell == null || this._avatarShell.transform == null)
		{
			return;
		}
		if ((float)((int)Time.time) * 50f % 50f == 0f)
		{
			float num = float.PositiveInfinity;
			foreach (Shell shell in ShellManager.GetShellsInScene())
			{
				TramShell tramShell = shell as TramShell;
				if (tramShell != null && tramShell.tram.speed > 0.1f)
				{
					float num2 = Vector3.Distance(tramShell.transform.position, this._avatarShell.transform.position);
					if (num2 < num)
					{
						num = num2;
					}
				}
				DoorShell doorShell = shell as DoorShell;
				if (doorShell != null && doorShell.door != null && doorShell.door.actionName == "Moving")
				{
					num = 3f;
				}
			}
			GameObject gameObject = GameObject.Find("TramController");
			if (gameObject != null)
			{
				TramController component = gameObject.GetComponent<TramController>();
				if (component.tramSpeed > 0.1f)
				{
					num = 2f;
				}
				else
				{
					num = 20f;
				}
			}
			float num3 = (1f - Mathf.Clamp01(num / 20f)) * 0.08f;
			if (num3 > 0.01f)
			{
				base.controls.camera.Shake(num3 * 3f, 1f, true);
			}
		}
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0000D810 File Offset: 0x0000BA10
	private void ShowCursorSelect(Vector3 pPosition)
	{
		if (this._cursorSelect == null)
		{
			this._cursorSelect = (global::UnityEngine.Object.Instantiate(this._cursorSelectPrefab, pPosition, Quaternion.identity) as GameObject).transform;
		}
		else
		{
			this._cursorSelect.position = pPosition;
		}
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000D860 File Offset: 0x0000BA60
	private void HideCursorSelect()
	{
		if (this._cursorSelect != null)
		{
			global::UnityEngine.Object.Destroy(this._cursorSelect.gameObject);
			this._cursorSelect = null;
		}
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000D898 File Offset: 0x0000BA98
	private void SpawnCursorDriverEffect(IntPoint pPoint)
	{
		if (this._ignoreNextMouseDriverSpawn)
		{
			this._ignoreNextMouseDriverSpawn = false;
			return;
		}
		if (this._mouseDriverTimer > 0f)
		{
			return;
		}
		if (this._mousePressedTimer > 0.1f)
		{
			return;
		}
		Vector3 vector = MimanHelper.TilePositionToVector3(pPoint);
		Vector3 vector2 = new Vector3(vector.x, Shell.GetSurfaceHeight(vector), vector.z);
		this.DeleteCursorDriver();
		this._cursorDriver = (global::UnityEngine.Object.Instantiate(this._cursorDriverPrefab) as GameObject).transform;
		this._cursorDriver.position = vector2;
		SoundDictionary.PlaySound("MouseClick", this._cursorDriver.audio, this._mouseClickCounter % 4, 0f);
		this._mouseClickCounter++;
		this._mouseDriverTimer = 0.3f;
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000D964 File Offset: 0x0000BB64
	private void DeleteCursorDriver()
	{
		if (this._cursorDriver != null)
		{
			global::UnityEngine.Object.Destroy(this._cursorDriver.gameObject);
		}
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000D988 File Offset: 0x0000BB88
	private void BotherIfPossible(Ting pTing)
	{
		Character character = pTing as Character;
		if (character != null && character.isWaitingToBeTalkedTo)
		{
			character.Bother();
		}
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0000D9B4 File Offset: 0x0000BBB4
	private void OnAvatarChangesAction(string pPreviousAction, string pNewAction)
	{
		if (pNewAction == "Walking")
		{
			Time.timeScale = 1f;
		}
		else if (pPreviousAction != "SlurpingIntoComputer" && pNewAction == "SlurpingIntoComputer")
		{
			base.controls.fade.FadeToColor(Color.white);
			base.controls.fade.speed = 0.7f;
		}
		else if (pPreviousAction != "Hacking" && pNewAction == "Hacking")
		{
			MimanTing mimanTing = this.avatar.actionOtherObject as MimanTing;
			if (mimanTing != null)
			{
				this.HideCursorSelect();
				base.PushState(new CodeEditorState(mimanTing.programs, this.avatar, this._actionMenu, true, null));
			}
		}
		else if (pPreviousAction != "Sleeping" && pNewAction == "Sleeping")
		{
			Time.timeScale = 1f;
			if (base.controls.world.settings.beaten)
			{
				Debug.Log("Can't fall asleep because the game is beaten.");
			}
			else
			{
				base.controls.fade.onFadedToOpaque = delegate
				{
					base.PushState(new FastForwardState(this.avatar.alarmTime - new GameTime(0, 1), this.avatar));
				};
				base.controls.fade.speed = 0.25f;
				base.controls.fade.FadeToColor(new Color(0.05f, 0.1f, 0.3f));
			}
		}
		else if (pPreviousAction != "UsingComputer" && pNewAction == "UsingComputer")
		{
			this.HideCursorSelect();
			Shell shellWithName = ShellManager.GetShellWithName(this.avatar.actionOtherObject.name);
			if (shellWithName == null)
			{
				Debug.Log("Can't find computer shell to use for " + this.avatar.name);
			}
			else
			{
				Transform transform = shellWithName.transform.Find("CameraPoint");
				TerminalRenderer componentInChildren = shellWithName.GetComponentInChildren<TerminalRenderer>();
				if (transform && componentInChildren)
				{
					base.PushState(new ComputerInteractionState(transform, componentInChildren.transform, base.controls.camera, this.avatar.actionOtherObject as Computer, this.avatar, componentInChildren, this._actionMenu));
				}
				else if (transform)
				{
					base.PushState(new ComputerInteractionState(transform, shellWithName.transform, base.controls.camera, this.avatar.actionOtherObject as Computer, this.avatar, null, this._actionMenu));
				}
			}
		}
		else if (pPreviousAction != "LookInLocker" && pNewAction == "LookInLocker")
		{
			Locker locker = this.avatar.actionOtherObject as Locker;
			base.PushState(new InsideLockerState(base.controls.camera, locker.inventoryRoomName));
		}
		else if (pPreviousAction != "LookingAtMap" && pNewAction == "LookingAtMap")
		{
			this.StartMapState();
		}
		else if (base.IsTopState() && pPreviousAction != "InsideComputer" && pNewAction == "InsideComputer")
		{
			D.Log("Detected Slurp into internet in PlayerRoamingState");
			this._insideComputerState = new InsideComputerState(this.avatar.actionOtherObject.name, this._actionMenu);
			base.PushState(this._insideComputerState);
		}
		else if (pPreviousAction == "InsideComputer" && pNewAction != "InsideComputer")
		{
			D.Log("Detected Quit out of internet in PlayerRoamingState");
		}
		else if (pPreviousAction != "GettingSeated" && pNewAction == "GettingSeated" && this.tramAvatarIsIn != null && this.tramAvatarIsIn.name != "MineCart")
		{
			base.PushState(new TramRideState(this._actionMenu, this.tramAvatarIsIn, base.controls));
		}
		else if (pPreviousAction == "WalkingThroughDoor" && pNewAction != "WalkingThroughDoor")
		{
			this.MaybeSaveAutosave();
		}
		else if (pPreviousAction == "WalkingThroughPortal" && pNewAction != "WalkingThroughPortal")
		{
			this.MaybeSaveAutosave();
		}
		this.BuildActionMenu();
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0000DE34 File Offset: 0x0000C034
	private void StartMapState()
	{
		if (this.avatar != null)
		{
			this.avatar.CancelWalking();
		}
		base.PushState(new LookAtMapState(this._actionMenu));
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0000DE68 File Offset: 0x0000C068
	private void OnAvatarPositionChanged(WorldCoordinate pPreviousPosition, WorldCoordinate pNewPosition)
	{
		if (this.avatar != null)
		{
			base.controls.world.settings.activeRoom = pNewPosition.roomName;
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000DEA4 File Offset: 0x0000C0A4
	public bool TingIsHackable(MimanTing interactableTingUnderMouse)
	{
		return interactableTingUnderMouse.programs != null && interactableTingUnderMouse.programs.Length > 0;
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0000DEC0 File Offset: 0x0000C0C0
	private void ClearWalkAndInteractionBuffer()
	{
		this._bufferedInteractionTarget = null;
		this._bufferedWalkTarget = WorldCoordinate.NONE;
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000DED4 File Offset: 0x0000C0D4
	private void OnConversationFocus(string pConversation)
	{
		this.ClearWalkAndInteractionBuffer();
		if (this.avatar.actionName == "Walking")
		{
			this.avatar.CancelWalking();
		}
		if (this._avatarShell != null)
		{
		}
		this._dialogueSubState = new DialogueSubstate(base.controls, pConversation, new DialogueSubstate.IsShowingInventory(this.IsShowingInventory));
		this.BuildActionMenu();
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0000DF44 File Offset: 0x0000C144
	private bool IsShowingInventory()
	{
		return this._showInventory;
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000DF4C File Offset: 0x0000C14C
	public override void OnGUI()
	{
		base.OnGUI();
		if (this._dialogueSubState != null)
		{
			this._dialogueSubState.OnGUI();
		}
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x0000DF6C File Offset: 0x0000C16C
	private void OnRoomChanged(string pRoomName)
	{
		this._avatarShell = null;
		this.ClearWalkAndInteractionBuffer();
		if (this.avatar != null && (this.avatar.actionName == "WalkingThroughDoor" || this.avatar.actionName == "WalkingThroughPortal"))
		{
			this.avatar.ForceTriggerCurrentAction();
		}
		if (this.skipUpdateUntilRoomIsLoaded)
		{
			this._actionMenu.FadeIn();
			this.skipUpdateUntilRoomIsLoaded = false;
		}
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0000DFF0 File Offset: 0x0000C1F0
	private void MaybeSaveAutosave()
	{
		if (!base.controls.world.settings.beaten && Time.realtimeSinceStartup > this._lastSaveTime + 600f)
		{
			this._lastSaveTime = Time.realtimeSinceStartup;
			this.SaveAutosave();
		}
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x0000E040 File Offset: 0x0000C240
	private void SaveAutosave()
	{
		WorldOwner.instance.world.Save(WorldOwner.QUICKSAVE_DATA_PATH + "Autosave.json");
		Debug.Log("AUTOSAVED");
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0000E078 File Offset: 0x0000C278
	private void CheckForRoomChange()
	{
		if (this.avatar == null)
		{
			return;
		}
		if (this.AvatarIsInOtherRoom() && this.avatar.actionName != "WalkingThroughDoor")
		{
			this._waitingForDoorGoto = false;
		}
		if (base.controls.roomChanger.busy)
		{
			return;
		}
		if (this._waitingForDoorGoto)
		{
			if (this.avatar.actionName == string.Empty)
			{
				base.controls.fade.FadeToTransparent();
				base.controls.fade.speed = 1f;
				this._waitingForDoorGoto = false;
				return;
			}
			return;
		}
		else
		{
			if (this.avatar.actionName == "WalkingThroughDoor")
			{
				Debug.Log("Fade!");
				this._waitingForDoorGoto = true;
				base.controls.fade.FadeToColor(Color.black);
				base.controls.fade.speed = 1.1f;
				base.controls.fade.onFadedToOpaque = delegate
				{
				};
			}
			if (this.avatar.actionName == "WalkingThroughPortal")
			{
				Portal portal = this.avatar.actionOtherObject as Portal;
				if (portal != null && portal.targetPortal != null)
				{
					D.isNull(portal.targetPortal, "portal.targetPortal is null");
					if (!(base.controls.roomChanger.currentRoom == portal.targetPortal.room.name))
					{
						base.controls.roomChanger.LoadRoom(portal.targetPortal.room.name);
					}
				}
				return;
			}
			if (this.AvatarIsInOtherRoom())
			{
				Debug.Log("Detected immediate change from room " + base.controls.roomChanger.currentRoom + " to " + base.controls.world.settings.activeRoom);
				if (PlayerRoamingState.performSlowRoomChange)
				{
					PlayerRoamingState.performSlowRoomChange = false;
					base.controls.roomChanger.LoadRoom(base.controls.world.settings.activeRoom);
				}
				else
				{
					base.controls.roomChanger.LoadRoomImmediately(base.controls.world.settings.activeRoom);
				}
			}
			return;
		}
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000E2E8 File Offset: 0x0000C4E8
	private bool AvatarIsInOtherRoom()
	{
		return base.controls.world.settings.activeRoom != base.controls.roomChanger.currentRoom;
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000E320 File Offset: 0x0000C520
	private void RefreshTooltipText(Shell pInteractableShell, Ting pInteractableTing)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		if (pInteractableTing != null)
		{
			if (pInteractableTing.canBePickedUp)
			{
				text = "pick up 捡起" + pInteractableTing.tooltipName;
			}
			else
			{
				text = pInteractableTing.verbDescription + " " + pInteractableTing.tooltipName;
			}
			if (pInteractableShell is ComputerShell && (pInteractableShell as ComputerShell).computerScreen == null)
			{
				text = "Start computer (no monitor) 启动电脑 (无显示器)";
			}
			if (pInteractableShell is PortalShell || pInteractableShell is DoorShell)
			{
				HoverExitArrow componentInChildren = pInteractableShell.GetComponentInChildren<HoverExitArrow>();
				if (componentInChildren != null)
				{
					componentInChildren.ShowHoverMaterial();
				}
			}
			if (this.CanUseHandItemOnTing(pInteractableTing))
			{
				text2 = this.avatar.handItem.UseTingOnTingDescription(pInteractableTing);
			}
			else if (this.CanHackTing(pInteractableTing))
			{
				text2 = "hack 黑入" + pInteractableTing.tooltipName;
			}
			pInteractableShell.ShowTooltip(text, text2);
		}
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0000E418 File Offset: 0x0000C618
	private bool CanUseHandItemOnTing(Ting pInteractableTing)
	{
		return pInteractableTing != null && this.avatar != null && this.avatar.handItem != null && this.avatar.handItem.CanInteractWith(pInteractableTing);
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000E45C File Offset: 0x0000C65C
	private bool CanHackTing(Ting pInteractableTing)
	{
		return pInteractableTing != null && this.avatar != null && pInteractableTing is MimanTing && this.TingIsHackable(pInteractableTing as MimanTing) && this.avatar.hasHackdev;
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0000E4A4 File Offset: 0x0000C6A4
	private void InteractWithHandItem()
	{
		D.isNull(this.avatar.handItem);
		this.avatar.InteractWith(this.avatar.handItem);
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000E4D8 File Offset: 0x0000C6D8
	private void DropHandItem()
	{
		this.avatar.CancelWalking();
		D.isNull(this.avatar.handItem);
		this.avatar.DropHandItem();
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000E50C File Offset: 0x0000C70C
	private void PutHandItemIntoInventory()
	{
		this.avatar.CancelWalking();
		D.isNull(this.avatar.handItem);
		this.avatar.PutHandItemIntoInventory();
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0000E540 File Offset: 0x0000C740
	private bool GetWorldTilePoint(out WorldCoordinate pPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] array = Physics.RaycastAll(ray, 300f);
		float num = float.MaxValue;
		pPosition = WorldCoordinate.NONE;
		bool flag = false;
		foreach (RaycastHit raycastHit in array)
		{
			if ((raycastHit.transform.tag == "PhysicsFloor" || raycastHit.transform.tag == "Carpet") && raycastHit.distance < num)
			{
				pPosition = new WorldCoordinate(base.controls.roomChanger.currentRoom, new IntPoint((int)(raycastHit.point.x / 1f), (int)(raycastHit.point.z / 1f)));
				num = raycastHit.distance;
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x060001DC RID: 476 RVA: 0x0000E648 File Offset: 0x0000C848
	private int CompareHits(RaycastHit a, RaycastHit b)
	{
		return (int)((a.distance - b.distance) * 100f);
	}

	// Token: 0x060001DD RID: 477 RVA: 0x0000E660 File Offset: 0x0000C860
	public Shell GetInteractableShellUnderMouse()
	{
		float num = 300f;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] array = Physics.RaycastAll(ray, num);
		this._lastRayStart = ray.origin;
		this._lastRayEnd = ray.origin + ray.direction * num;
		List<RaycastHit> list = new List<RaycastHit>(array);
		list.Sort(new Comparison<RaycastHit>(this.CompareHits));
		foreach (RaycastHit raycastHit in list)
		{
			Transform transform = raycastHit.transform;
			Transform transform2 = transform;
			Shell component;
			while ((component = transform2.GetComponent<Shell>()) == null && transform2.parent != null)
			{
				transform2 = transform2.parent;
			}
			if (transform.tag != "DoorClickThing" && transform2.parent != null)
			{
				HideGroup component2 = transform2.parent.GetComponent<HideGroup>();
				if (component2 != null && component2.currentState == HideGroup.State.HIDING)
				{
					continue;
				}
			}
			if (component != null && this.avatar != null && component.ting != this.avatar.handItem && this.avatar.CanInteractWith(component.ting) && !component.ting.isBeingUsed && !component.ting.isBeingHeld && component.ting.actionName != "WalkingThroughDoor" && component.ting.actionName != "WalkingThroughPortal")
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000E868 File Offset: 0x0000CA68
	public bool IsGameObjectUnderMouse(string pName)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] array = Physics.RaycastAll(ray, 300f);
		foreach (RaycastHit raycastHit in array)
		{
			Transform transform = raycastHit.transform;
			if (transform.name == pName)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060001DF RID: 479 RVA: 0x0000E8D8 File Offset: 0x0000CAD8
	private Character avatar
	{
		get
		{
			return base.controls.avatar as Character;
		}
	}

	// Token: 0x0400010A RID: 266
	private bool _storyStart;

	// Token: 0x0400010B RID: 267
	private CharacterShell _avatarShell;

	// Token: 0x0400010C RID: 268
	private InsideComputerState _insideComputerState;

	// Token: 0x0400010D RID: 269
	private DialogueSubstate _dialogueSubState;

	// Token: 0x0400010E RID: 270
	private ActionMenu _actionMenu;

	// Token: 0x0400010F RID: 271
	private ActionMenuItem _clockItem;

	// Token: 0x04000110 RID: 272
	private bool _actionMenuItemWasPressed;

	// Token: 0x04000111 RID: 273
	private bool _showInventory;

	// Token: 0x04000112 RID: 274
	private string _whichInventory = string.Empty;

	// Token: 0x04000113 RID: 275
	private bool _physicalGuiButtonWasPressed;

	// Token: 0x04000114 RID: 276
	private float _mousePressedTimer;

	// Token: 0x04000115 RID: 277
	private Transform _cursorSelect;

	// Token: 0x04000116 RID: 278
	private Transform _cursorDriver;

	// Token: 0x04000117 RID: 279
	private Transform _walkTargeterTransform;

	// Token: 0x04000118 RID: 280
	private Transform _guiThingy;

	// Token: 0x04000119 RID: 281
	private GameObject _cursorDriverPrefab;

	// Token: 0x0400011A RID: 282
	private GameObject _cursorSelectPrefab;

	// Token: 0x0400011B RID: 283
	private float _doubleClickTimer;

	// Token: 0x0400011C RID: 284
	private int _mouseClickCounter;

	// Token: 0x0400011D RID: 285
	private WorldCoordinate _cachedTargetPositionInRoom;

	// Token: 0x0400011E RID: 286
	private float _mouseDriverTimer;

	// Token: 0x0400011F RID: 287
	private bool _ignoreNextMouseDriverSpawn;

	// Token: 0x04000120 RID: 288
	private bool _ignoreNextSelectSound;

	// Token: 0x04000121 RID: 289
	private Vector3 _lastRayStart;

	// Token: 0x04000122 RID: 290
	private Vector3 _lastRayEnd;

	// Token: 0x04000123 RID: 291
	private float _ignoreMouseInputTimer;

	// Token: 0x04000124 RID: 292
	private float _mouseSelectSoundSpacing;

	// Token: 0x04000125 RID: 293
	private GlowEffect _glowEffect;

	// Token: 0x04000126 RID: 294
	public static bool performSlowRoomChange = true;

	// Token: 0x04000127 RID: 295
	private bool skipUpdateUntilRoomIsLoaded = true;

	// Token: 0x04000128 RID: 296
	private AudioSource _moneyChangeAudioSource;

	// Token: 0x04000129 RID: 297
	private WorldCoordinate _bufferedWalkTarget = WorldCoordinate.NONE;

	// Token: 0x0400012A RID: 298
	private Ting _bufferedInteractionTarget;

	// Token: 0x0400012B RID: 299
	private bool _bufferedRun;

	// Token: 0x0400012C RID: 300
	private bool foundBankEntryForAvatar;

	// Token: 0x0400012D RID: 301
	private int _displayedCashAmount;

	// Token: 0x0400012E RID: 302
	private static HashSet<Type> _dontShowUseButtonForTheseItemTypes = new HashSet<Type>
	{
		typeof(Key),
		typeof(Floppy),
		typeof(Pawn),
		typeof(Extractor),
		typeof(Screwdriver),
		typeof(Hackdev),
		typeof(Suitcase),
		typeof(Memory),
		typeof(Goods)
	};

	// Token: 0x0400012F RID: 303
	private float _lastDanceCheck;

	// Token: 0x04000130 RID: 304
	private float _lastSaveTime;

	// Token: 0x04000131 RID: 305
	private bool _waitingForDoorGoto;
}
