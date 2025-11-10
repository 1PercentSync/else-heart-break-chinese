using System;
using System.Collections.Generic;
using GameTypes;
using GameWorld2;
using RelayLib;
using TingTing;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class CharacterShell : Shell
{
	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060004CE RID: 1230 RVA: 0x00020F28 File Offset: 0x0001F128
	public Character character
	{
		get
		{
			return base.ting as Character;
		}
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x00020F38 File Offset: 0x0001F138
	public override void CreateTing()
	{
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00020F3C File Offset: 0x0001F13C
	public override Transform mouthPosition
	{
		get
		{
			return this._mouthPosition;
		}
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00020F44 File Offset: 0x0001F144
	protected override void Setup()
	{
		if (!base.hasSetupTingRef)
		{
			return;
		}
		base.Setup();
		this.SnapShellToTingPosition();
		this._hideForAWhileWhenEnteringRoom = 0.25f;
		this._isAvatar = WorldOwner.instance.world.settings.avatarName == base.ting.name;
		this._animator = base.transform.GetComponentInChildren<Animator>();
		this._agent = base.GetComponentInChildren<NavMeshAgent>();
		if (this._agent == null)
		{
			Debug.LogError("Can't find nav mesh agent for " + base.name);
		}
		this.SetEnableOnNavmeshAgent();
		if (this._audioSource == null)
		{
			Debug.Log("Can't find audio source in " + base.name);
		}
		else
		{
			GameObject gameObject = new GameObject("CreakAudioSource");
			this._creakAudioSource = gameObject.AddComponent<AudioSource>();
			this._creakAudioSource.transform.parent = this._audioSource.transform;
			this._creakAudioSource.audio.minDistance = this._audioSource.audio.minDistance;
			this._creakAudioSource.audio.maxDistance = this._audioSource.audio.maxDistance;
			this._creakAudioSource.transform.localPosition = Vector3.zero;
			if (!this._isAvatar)
			{
				this._audioSource.volume = 0.7f;
				this._audioSource.pitch = 0.85f;
				this._audioSource.maxDistance = 30f;
				if (base.GetComponent<AudioListener>() == null)
				{
					AudioLowPassFilter audioLowPassFilter = base.gameObject.AddComponent<AudioLowPassFilter>();
					audioLowPassFilter.cutoffFrequency = 600f;
					audioLowPassFilter.lowpassResonaceQ = 1f;
				}
			}
		}
		if (this._isAvatar)
		{
			GameObject gameObject2 = GameObject.Find("MasterAudioListener");
			if (gameObject2 == null)
			{
				Debug.LogError(base.name + " can't find master audio listener game object in scene");
			}
			else
			{
				this._ear = gameObject2.transform;
				this._audioListener = this._ear.GetComponent<AudioListener>();
				D.isNull(this._audioListener, "No audio listener on MasterAudioListener");
				this._audioListener.enabled = true;
			}
		}
		this._leftFoot = MimanHelper.GetTransformWithNameRecursively(base.transform, "LFoot");
		this._rightFoot = MimanHelper.GetTransformWithNameRecursively(base.transform, "RFoot");
		if (this._leftFoot == null)
		{
		}
		if (this._rightFoot == null)
		{
		}
		this._walkSoundManager = new WalkSoundManager();
		SoundDictionary.LoadSingleSound("SlurpSound", "Get sucked in to internet sound 0");
		SoundDictionary.LoadMultiSound("LockedDoor", "LockedDoor wood sound", 1);
		SoundDictionary.LoadMultiSound("GettingSeatedFront", "SitDownChairFrontSound", 2);
		SoundDictionary.LoadMultiSound("GettingSeatedSide", "SitDownChairRightSound", 2);
		SoundDictionary.LoadMultiSound("Drinking", "Drinking", 4);
		SoundDictionary.LoadMultiSound("GettingUpFromSeat", "GettingUpFromSeat chair sound", 1);
		SoundDictionary.LoadMultiSound("KickingLamp", "KickingLamp sound", 3);
		SoundDictionary.LoadSingleSound("LayingDown", "LayingDown sound0");
		SoundDictionary.LoadMultiSound("GettingUpFromBed", "GettingUpFromBed sound", 2);
		SoundDictionary.LoadMultiSound("PickingUp", "PickingUp sound", 1);
		SoundDictionary.LoadMultiSound("Dropping", "Dropping sound", 1);
		SoundDictionary.LoadMultiSound("DroppingFar", "Dropping sound", 1);
		SoundDictionary.LoadMultiSound("UsingDoorWithKey", "UsingDoorWithKey sound", 1);
		SoundDictionary.LoadMultiSound("PutHandItemIntoInventory", "PutHandItemIntoInventory sound", 1);
		SoundDictionary.LoadMultiSound("TakeOutInventoryItem", "TakeOutInventoryItem sound", 1);
		SoundDictionary.LoadMultiSound("PushingButton", "PushingButton sound", 2);
		SoundDictionary.LoadMultiSound("PuttingTingIntoSendPipe", "Put gods in pipe Sound", 1);
		SoundDictionary.LoadSingleSound("ActivatingVendingMachine", "Coke bought");
		SoundDictionary.LoadSingleSound("UsingTv", "PushObjectInHandSound 0");
		SoundDictionary.LoadMultiSound("SittingUsingComputer", "SitUseComputerSound", 3);
		SoundDictionary.LoadSingleSound("Hello", "HelloSound 0");
		SoundDictionary.LoadSingleSound("Shrug", "ShrugSound 0");
		SoundDictionary.LoadMultiSound("TakingDrug", "EatPillSound", 2);
		SoundDictionary.LoadMultiSound("Eat", "EatSound", 3);
		SoundDictionary.LoadMultiSound("SmokingCigarette", "SmokingCigaretteSound", 4);
		SoundDictionary.LoadMultiSound("TakingSnus", "SnusaSound", 2);
		SoundDictionary.LoadSingleSound("GivingHandItem", "GiveThingSound 0");
		SoundDictionary.LoadMultiSound("ReceivingHandItem", "ReceiveThingSound", 2);
		SoundDictionary.LoadSingleSound("PushingButtonOnHandItem", "PushObjectInHandSound 0");
		SoundDictionary.LoadSingleSound("ThrowingTingIntoTrashCan", "ThrowAwaySound 0");
		SoundDictionary.LoadSingleSound("UseSink", "UseSinkSound 0");
		SoundDictionary.LoadSingleSound("UseStove", "UseStoveSound 0");
		SoundDictionary.LoadMultiSound("RefillingDrink", "RefillingDrinkSound", 2);
		SoundDictionary.LoadSingleSound("UseSink", "UseSinkSound 0");
		SoundDictionary.LoadSingleSound("TalkingInTelephone", "TalkingInTelephoneSound 0");
		SoundDictionary.LoadSingleSound("ComputerTruble1", "ComputerTruble1Sound 0");
		SoundDictionary.LoadSingleSound("ComputerTruble2", "ComputerTruble2Sound 0");
		SoundDictionary.LoadMultiSound("Yawn", "Sleepy1Sound", 3);
		SoundDictionary.LoadSingleSound("StartingJukebox", "StartingJukeboxSound 0");
		SoundDictionary.LoadSingleSound("UsingTv", "UsingTvSound 0");
		SoundDictionary.LoadMultiSound("FallAsleepFromStanding_Standing", "FallTogetherSound", 2);
		SoundDictionary.LoadSingleSound("FallAsleepFromStanding_Sitting", "SitSleepingIdleSound 0");
		SoundDictionary.LoadMultiSound("PickingUpGround", "PickUpGroundSound", 2);
		SoundDictionary.LoadMultiSound("PickingUpHigh", "PickUpHighSound", 2);
		SoundDictionary.LoadSingleSound("PickingUpTable", "PickUpTableSound 0");
		SoundDictionary.LoadMultiSound("DroppingGround", "PutDownGroundSound", 2);
		SoundDictionary.LoadMultiSound("DroppingTable", "PutDownTableSound", 2);
		SoundDictionary.LoadMultiSound("FootstepCreak", "Footstep wood creak sound", 11);
		SoundDictionary.LoadMultiSound("AngryAtComputer", "Angry2Sound", 1);
		SoundDictionary.LoadSingleSound("Tasing", "TazeSound 0");
		SoundDictionary.LoadSingleSound("GettingTasedGently", "Fish game attack sound 1");
		SoundDictionary.LoadSingleSound("Extracting", "ExtractingSound 0");
		SoundDictionary.LoadMultiSound("SlurpingIntoComputer", "Get sucked in to internet sound", 3);
		this._lowpassFilter = base.GetComponent<AudioLowPassFilter>();
		this._mouthPosition = MimanHelper.GetTransformWithNameRecursively(base.transform, "head_bn");
		if (this._mouthPosition == null)
		{
			this._mouthPosition = MimanHelper.GetTransformWithNameRecursively(base.transform, "Head");
		}
		if (this._mouthPosition == null)
		{
			this._mouthPosition = new GameObject("mouth_position")
			{
				transform = 
				{
					parent = base.transform,
					localPosition = new Vector3(0f, 3.5f, 0f)
				}
			}.transform;
		}
		this.SetupHandPoint();
		if (this.character.handItem != null)
		{
			this.OnNewHandItem(this.character.handItem.name, false);
		}
		if (this._animator != null)
		{
			this._animator.applyRootMotion = false;
		}
		this.SelectAnimationAndSoundToPlay();
		this.RefreshGhostParticles();
		this.RefreshHackerEffect(this.character.actionName);
		this.RefreshSmellyCloudOfFlies(this.character.smelliness);
		if (this.character.isAvatar)
		{
			this.CheckForNewShoes();
		}
		if (this.character.sitting)
		{
			this._animator.Play("SitIdleA1");
		}
		else if (this.character.laying)
		{
			if (this.character.bed != null && this.character.bed.exitPoint == 0)
			{
				this._animator.Play("SleepingRight");
			}
			else if (this.character.bed != null)
			{
				this._animator.Play("SleepingLeft");
			}
			else
			{
				this._animator.Play("LayingOnGround");
			}
		}
		this._lookTargetPoint = base.transform.position;
		RunGameWorld.instance.gameViewControls.camera.UpdateStates(0.01f);
		this.character.timetableTimer = 0f;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00021724 File Offset: 0x0001F924
	private void SetupHandPoint()
	{
		this._handPoint = MimanHelper.GetTransformWithNameRecursively(base.transform, "RHandPoint");
		if (this._handPoint == null)
		{
			this._handPoint = MimanHelper.GetTransformWithNameRecursively(base.transform, "RHandPoint_nonAnimated");
		}
		if (this._handPoint == null)
		{
			this._handPoint = MimanHelper.GetTransformWithNameRecursively(base.transform, "r_palm_bn");
		}
		if (this._handPoint == null)
		{
			this._handPoint = MimanHelper.GetTransformWithNameRecursively(base.transform, "Rigg:r_palm_bn");
		}
		if (this._handPoint == null)
		{
			this._handPoint = MimanHelper.GetTransformWithNameRecursively(base.transform, "Ivan_Skinned:r_ik_palm_jnt");
		}
		if (this._handPoint == null)
		{
			D.Log("Can't find handpoint in " + base.name);
		}
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0002180C File Offset: 0x0001FA0C
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		base.ting.AddDataListener<Character.WalkMode>("walkMode", new ValueEntry<Character.WalkMode>.DataChangeHandler(this.OnWalkModeChange));
		Character character = this.character;
		character.onNewAction = (Ting.OnNewAction)Delegate.Combine(character.onNewAction, new Ting.OnNewAction(this.OnNewAction));
		Character character2 = this.character;
		character2.onNewHandItem = (Character.OnNewHandItem)Delegate.Combine(character2.onNewHandItem, new Character.OnNewHandItem(this.OnNewHandItem));
		this.character.AddDataListener<float>("corruption", new ValueEntry<float>.DataChangeHandler(this.OnCorruptionChange));
		this.character.AddDataListener<float>("smelliness", new ValueEntry<float>.DataChangeHandler(this.OnSmellinessChange));
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x000218C4 File Offset: 0x0001FAC4
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		base.ting.RemoveDataListener<Character.WalkMode>("walkMode", new ValueEntry<Character.WalkMode>.DataChangeHandler(this.OnWalkModeChange));
		Character character = this.character;
		character.onNewAction = (Ting.OnNewAction)Delegate.Remove(character.onNewAction, new Ting.OnNewAction(this.OnNewAction));
		Character character2 = this.character;
		character2.onNewHandItem = (Character.OnNewHandItem)Delegate.Remove(character2.onNewHandItem, new Character.OnNewHandItem(this.OnNewHandItem));
		this.character.RemoveDataListener<float>("corruption", new ValueEntry<float>.DataChangeHandler(this.OnCorruptionChange));
		this.character.RemoveDataListener<float>("smelliness", new ValueEntry<float>.DataChangeHandler(this.OnSmellinessChange));
		if (this.character.HasWalkBehaviour())
		{
			this.character.CreateNewWalkBehaviour();
		}
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00021994 File Offset: 0x0001FB94
	private void OnCorruptionChange(float pOld, float pNew)
	{
		this.RefreshGhostParticles();
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0002199C File Offset: 0x0001FB9C
	private void OnSmellinessChange(float pOld, float pNew)
	{
		this.RefreshSmellyCloudOfFlies(this.character.smelliness);
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x000219B0 File Offset: 0x0001FBB0
	protected override void SelectAnimationAndSoundToPlay()
	{
		if (this._animator == null)
		{
			return;
		}
		this.RefreshAnimationControllerParameters();
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x000219CC File Offset: 0x0001FBCC
	private void RefreshAnimationControllerParameters()
	{
		this._animator.SetInteger(CharacterShell.DRUNKENNESS, (int)this.character.drunkenness);
		this._animator.SetInteger(CharacterShell.SMELLINESS, (int)this.character.smelliness);
		this._animator.SetInteger(CharacterShell.SLEEPINESS, (int)this.character.sleepiness);
		this._animator.SetInteger(CharacterShell.CHARISMA, (int)this.character.charisma);
		this._animator.SetInteger(CharacterShell.HAPPINESS, (int)this.character.happiness);
		this._animator.SetInteger(CharacterShell.SUPREMACY, (int)this.character.supremacy);
		string actionName = base.ting.actionName;
		this.PlayActionSound(actionName);
		this._animator.SetBool("DoingAnAction", actionName != string.Empty);
		if (!(actionName == "UsingComputer") || this._isAvatar)
		{
		}
		if (actionName == "GettingSeated")
		{
			float num = (this.character.seat.position.localPosition - this.character.position.localPosition).Degrees();
			float num2 = IntPoint.DirectionToIntPoint(this.character.seat.direction).Degrees();
			float num3;
			for (num3 = num2 - num; num3 < 0f; num3 += 360f)
			{
			}
			if (num3 > 225f && num3 < 315f)
			{
				this._animator.CrossFade("SitDownRight", 0f);
				SoundDictionary.PlaySound("GettingSeatedSide", this._audioSource);
			}
			else if (num3 > 45f && num3 < 135f)
			{
				this._animator.CrossFade("SitDownLeft", 0f);
				SoundDictionary.PlaySound("GettingSeatedSide", this._audioSource);
			}
			else
			{
				this._animator.CrossFade("SitDownFront", 0f);
				SoundDictionary.PlaySound("GettingSeatedFront", this._audioSource);
			}
			return;
		}
		this._animator.SetBool("GettingSeated", false);
		this._animator.SetBool("GettingSeatedRight", false);
		this._animator.SetBool("GettingSeatedLeft", false);
		if (actionName == "LayingDown")
		{
			float num4 = 90f;
			float num5 = (this.character.bed.position.localPosition - this.character.position.localPosition).Degrees();
			float num6 = IntPoint.DirectionToIntPoint(this.character.bed.direction).Degrees() + num4;
			float num7;
			for (num7 = num6 - num5; num7 < 0f; num7 += 360f)
			{
			}
			Debug.Log("entering bed with diff: " + num7);
			if (num7 > 225f && num7 < 315f)
			{
				this._animator.SetBool("LayingDownRight", true);
			}
			else if (num7 > 45f && num7 < 135f)
			{
				this._animator.SetBool("LayingDownLeft", true);
			}
			else
			{
				Debug.Log("Can't lay down from foot side!");
			}
			return;
		}
		this._animator.SetBool("LayingDownLeft", false);
		this._animator.SetBool("LayingDownRight", false);
		string empty = string.Empty;
		float num8 = ((!CharacterShell.s_immediateAnimations.Contains(actionName)) ? 0.1f : 0f);
		if (actionName == string.Empty && this.character.sitting)
		{
			this._animator.CrossFade("SitIdleA1", 0.2f);
		}
		else if (actionName == string.Empty && this.character.laying && this.character.bed != null)
		{
			if (this.character.bed.exitPoint == 1)
			{
				this._animator.CrossFade("SleepingLeft", 0f);
			}
			else
			{
				this._animator.CrossFade("SleepingRight", 0f);
			}
		}
		else if (actionName == "FallAsleepFromStanding")
		{
			if (this.character.sitting)
			{
				SoundDictionary.PlaySound("FallAsleepFromStanding_Sitting", this._audioSource);
			}
			else if (this.character.laying)
			{
				Debug.Log(base.name + " is already laing down, will not play a falling asleep from standing animation");
			}
			else
			{
				this._animator.CrossFade("FallAsleepFromStanding", num8);
				SoundDictionary.PlaySound("FallAsleepFromStanding_Standing", this._audioSource);
			}
		}
		else if (actionName == "UsingTv")
		{
			float num9 = this.DiffInHeightToObjectUpFront();
			if (num9 > 4f)
			{
				this._animator.CrossFade("PushTVhigh", num8);
			}
			else
			{
				this._animator.CrossFade("PushButtonOnPillar", num8);
			}
		}
		else if (!this.character.sitting && CharacterShell.animStateLookup.TryGetValue(actionName, out empty))
		{
			this._animator.CrossFade(empty, num8);
		}
		else if (this.character.sitting && CharacterShell.sittingAnimStateLookup.TryGetValue(actionName, out empty))
		{
			this._animator.CrossFade(empty, num8);
		}
		else if (this.character.actionName == "DoneWalkingThroughFence")
		{
			Debug.Log("Fading to Idle state REALLY REALLY fast after walking through fence");
			this._animator.CrossFade("Idle", 0f);
		}
		else if (!this.character.sitting && actionName == "PickingUp")
		{
			float num10 = this.DiffInHeightToObjectUpFront();
			if (num10 > 4f)
			{
				this._animator.CrossFade("PickUpHigh", num8);
				SoundDictionary.PlaySound("PickingUpHigh", this._audioSource);
			}
			else if (num10 > 0.5f)
			{
				this._animator.CrossFade("PickUpTable", num8);
				SoundDictionary.PlaySound("PickingUpTable", this._audioSource);
			}
			else
			{
				this._animator.CrossFade("PickUpGround", num8);
				SoundDictionary.PlaySound("PickingUpGround", this._audioSource);
			}
		}
		else if (actionName == "Stealing")
		{
			if ((this.character.actionOtherObject as Character).sitting)
			{
				this._animator.CrossFade("PickUpTable", num8);
			}
			else
			{
				this._animator.CrossFade("PickUpGround", num8);
			}
		}
		else if (this.character.sitting && (actionName == "Dropping" || actionName == "DroppingFar"))
		{
			this._animator.CrossFade("SitPutDown", num8);
			SoundDictionary.PlaySound("DroppingGround", this._audioSource);
		}
		else if (!this.character.sitting && (actionName == "Dropping" || actionName == "DroppingFar"))
		{
			if (this.SurfaceInFrontIsTable())
			{
				this._animator.CrossFade("PutDownTable", num8);
				SoundDictionary.PlaySound("DroppingTable", this._audioSource);
			}
			else
			{
				this._animator.CrossFade("PutDownGround", num8);
				SoundDictionary.PlaySound("DroppingGround", this._audioSource);
			}
		}
		else if (actionName == "WalkingThroughDoor")
		{
			Door door = this.character.actionOtherObject as Door;
			if (door.isElevatorEntrance || !door.hasDoorKnob)
			{
				this._animator.CrossFade("WalkingThroughPortal", num8);
			}
			else
			{
				this._animator.CrossFade("OpenDoor", num8);
			}
		}
		else if (actionName == "LockedDoor")
		{
			Door door2 = this.character.actionOtherObject as Door;
			if (!door2.isElevatorEntrance && door2.hasDoorKnob)
			{
				this._animator.CrossFade("LockedDoor", num8);
			}
		}
		else if (actionName == "WalkingThroughPortal")
		{
			this._animator.CrossFade("WalkingThroughPortal", num8);
		}
		else if (actionName == "Hacking" && this.character.handItem is Hackdev)
		{
			this._animator.CrossFade("UseHackDev", num8);
		}
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x000222D4 File Offset: 0x000204D4
	private float DiffInHeightToObjectUpFront()
	{
		if (this.character.actionOtherObject == null)
		{
			Debug.Log(base.name + " can't messure, other object is null");
			return 0f;
		}
		if (this.character.actionOtherObject == null)
		{
			Debug.Log(base.name + " can't measure DiffInHeightToObjectUpFront, character.actionOtherObject is null");
			return 0f;
		}
		Shell shellWithName = ShellManager.GetShellWithName(this.character.actionOtherObject.name);
		if (shellWithName == null)
		{
			Debug.Log(base.name + " can't measure DiffInHeightToObjectUpFront, other object's shell is not in the scene");
			return 0f;
		}
		float y = shellWithName.transform.position.y;
		return y - base.transform.position.y;
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x000223A0 File Offset: 0x000205A0
	private bool SurfaceInFrontIsTable()
	{
		IntPoint intPoint = base.ting.localPoint + IntPoint.DirectionToIntPoint(base.ting.direction);
		Vector3 vector = MimanHelper.TilePositionToVector3(intPoint);
		float surfaceHeight = Shell.GetSurfaceHeight(vector);
		float num = surfaceHeight - base.transform.position.y;
		return num > 1f;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x000223FC File Offset: 0x000205FC
	private void OnWalkModeChange(Character.WalkMode pPreviousWalkMode, Character.WalkMode pNewWalkMode)
	{
		if (pNewWalkMode == Character.WalkMode.NO_TARGET)
		{
			this.StopAgent();
			base.SnapTingToShellPosition();
		}
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00022410 File Offset: 0x00020610
	private void StopAgent()
	{
		if (this._agent != null && this._agent.enabled)
		{
			this._agent.Stop();
		}
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x0002244C File Offset: 0x0002064C
	private void OnNewHandItem(string pNewItem, bool pGivingItemToSomeoneElse)
	{
		if (pNewItem == string.Empty)
		{
			if (this._shellInHand == null)
			{
				return;
			}
			if (!pGivingItemToSomeoneElse)
			{
				this._shellInHand.transform.parent = null;
			}
			this._shellInHand = null;
		}
		else
		{
			if (this._shellInHand != null)
			{
				this._shellInHand.transform.parent = null;
			}
			if (this._handPoint == null)
			{
				Debug.Log("Hand point is null, can't set hand item for " + base.name);
				return;
			}
			this._shellInHand = ShellManager.GetShellWithName(pNewItem);
			if (this._shellInHand == null)
			{
				return;
			}
			this._shellInHand.transform.parent = this._handPoint.transform;
			this.FixPositionAndRotationOfShellInHand();
		}
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x0002252C File Offset: 0x0002072C
	private void FixPositionAndRotationOfShellInHand()
	{
		Transform transform = this._shellInHand.transform.Find("Handle");
		if (transform != null)
		{
			this._handPoint.transform.localRotation = transform.localRotation;
			this._handPoint.transform.localPosition = transform.localPosition;
		}
		else
		{
			this._handPoint.transform.localRotation = Quaternion.identity;
			this._handPoint.transform.localPosition = Vector3.zero;
		}
		this._shellInHand.transform.localRotation = Quaternion.identity;
		this._shellInHand.transform.localPosition = Vector3.zero;
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x000225E0 File Offset: 0x000207E0
	public override void SnapShellToTingPosition()
	{
		if (this._agent != null)
		{
			this._agent.enabled = false;
		}
		base.SnapShellToTingPosition();
		this.SetEnableOnNavmeshAgent();
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x0002260C File Offset: 0x0002080C
	public void SetEnableOnNavmeshAgent()
	{
		if (this._agent == null)
		{
			return;
		}
		bool flag = this.character.sitting || this.character.actionName == "GettingSeated" || this.character.laying || this.character.actionName == "LayingDown" || this.character.actionName == "WalkingThroughPortal";
		if (this._agent.enabled && flag)
		{
			this._agent.enabled = false;
		}
		else if (!this._agent.enabled && !flag)
		{
			this._agent.enabled = true;
		}
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x000226E4 File Offset: 0x000208E4
	private void OnNewAction(string pPreviousAction, string pNewAction)
	{
		if (this.character.isAvatar)
		{
			this.CheckForNewShoes();
		}
		if (this._agent != null)
		{
			this._agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
		}
		if (pNewAction != "Walking")
		{
			this.running = false;
		}
		this.RefreshHackerEffect(pNewAction);
		if (pNewAction == string.Empty)
		{
			return;
		}
		if (CharacterShell.snapTingToShellOnTheseNewActions.Contains(pNewAction))
		{
			base.SnapTingToShellPosition();
		}
		else
		{
			base.SnapShellToTingDirection();
		}
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00022774 File Offset: 0x00020974
	private static Material GetNewShoesMaterial()
	{
		if (CharacterShell.s_newShoesMaterial == null)
		{
			CharacterShell.s_newShoesMaterial = Resources.Load("Sebastian_AlternateSHOE") as Material;
		}
		if (CharacterShell.s_newShoesMaterial == null)
		{
			D.Log("Failed to load Sebastian_AlternateSHOE material");
		}
		return CharacterShell.s_newShoesMaterial;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x000227C4 File Offset: 0x000209C4
	private void CheckForNewShoes()
	{
		if (this.character.HasKnowledge("NewShoes") && !this._setNewShoes)
		{
			SkinnedMeshRenderer componentInChildren = base.GetComponentInChildren<SkinnedMeshRenderer>();
			if (componentInChildren != null)
			{
				if (this._originalMaterials == null)
				{
					this._originalMaterials = componentInChildren.materials;
				}
				List<Material> list = new List<Material>();
				for (int i = 0; i < componentInChildren.materials.Length; i++)
				{
					if (i == 5)
					{
						list.Add(CharacterShell.GetNewShoesMaterial());
					}
					else
					{
						list.Add(this._originalMaterials[i]);
					}
				}
				componentInChildren.materials = list.ToArray();
				this._setNewShoes = true;
			}
			else
			{
				Debug.Log("Found no mesh renderer for " + base.name);
			}
		}
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x0002288C File Offset: 0x00020A8C
	private void RefreshHackerEffect(string pNewAction)
	{
		if (pNewAction == "Hacking")
		{
			if (this._handPoint == null)
			{
				Debug.LogError("hand point is null");
			}
			if (this._hackEffect == null)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("HackEffectSpawnPoint") as GameObject, this._handPoint.transform.position, Quaternion.identity) as GameObject;
				if (gameObject == null)
				{
					Debug.LogError("Hack effect instance is null");
				}
				this._hackEffect = gameObject.GetComponent<HackEffectSpawnPoint>();
				if (this._hackEffect == null)
				{
					Debug.LogError("_hackEffect component is null");
				}
				if (this.character.actionOtherObject == null)
				{
					Debug.LogError("character.actionOtherObject is null");
				}
				if (this.actionOtherObjectShell == null)
				{
					Debug.LogError("targetShell is null");
				}
				this._hackEffect.goalPoint = this.actionOtherObjectShell.transform;
				this._hackEffect.transform.parent = this._handPoint.transform;
			}
			else
			{
				Debug.Log("Already has a hack effect (sparkles)");
			}
			if (this._mouthPosition != null)
			{
				if (this._gloria == null)
				{
					GameObject gameObject2 = global::UnityEngine.Object.Instantiate(Resources.Load("HackingGloria") as GameObject, this._mouthPosition.position, Quaternion.identity) as GameObject;
					if (gameObject2 == null)
					{
						Debug.LogError("Gloria instance instance is null");
					}
					this._gloria = gameObject2.transform;
					this._gloria.parent = this._mouthPosition;
					if (this._isAvatar)
					{
						this._gloria.audio.volume = 0.1f;
					}
				}
				else
				{
					Debug.Log(base.name + " already has a gloria");
				}
			}
			else
			{
				Debug.Log("Found no mouth point for gloria effect in " + base.name);
			}
		}
		else
		{
			if (this._hackEffect != null)
			{
				global::UnityEngine.Object.Destroy(this._hackEffect.gameObject);
				this._hackEffect = null;
			}
			if (this._gloria != null)
			{
				global::UnityEngine.Object.Destroy(this._gloria.gameObject);
				this._gloria = null;
			}
		}
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00022AE0 File Offset: 0x00020CE0
	private void RefreshSmellyCloudOfFlies(float pSmelliness)
	{
		if (pSmelliness < 1f)
		{
			if (this._flies != null)
			{
				global::UnityEngine.Object.Destroy(this._flies.gameObject);
				this._flies = null;
			}
			return;
		}
		if (this._mouthPosition != null)
		{
			if (this._flies == null)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("Flies") as GameObject, this._mouthPosition.position, Quaternion.identity) as GameObject;
				if (gameObject == null)
				{
					Debug.LogError("Flies instance is null");
				}
				this._flies = gameObject.transform;
				this._flies.parent = this._mouthPosition;
			}
		}
		else
		{
			Debug.Log("Found no mouth point for flies effect in " + base.name);
		}
		if (this._flies != null)
		{
			this._flies.particleSystem.emissionRate = 0.01f + pSmelliness / 15f;
			this._flies.audio.volume = 1f;
			if (this._isAvatar)
			{
				this._flies.audio.volume = 0.2f;
			}
		}
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00022C24 File Offset: 0x00020E24
	private void PlayActionSound(string pNewAction)
	{
		if (!(pNewAction == string.Empty))
		{
			if (CharacterShell.actionsThatDontTriggerSound.Contains(pNewAction))
			{
				this._audioSource.clip = null;
			}
			else
			{
				SoundDictionary.PlaySound(pNewAction, this._audioSource);
				if (this._audioSource.clip != null)
				{
					float num = this._audioSource.clip.length * this.character.actionPercentage;
					this._audioSource.time = num;
				}
			}
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00022CB4 File Offset: 0x00020EB4
	private Shell actionOtherObjectShell
	{
		get
		{
			return ShellManager.GetShellWithName(this.character.actionOtherObject.name);
		}
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00022CCC File Offset: 0x00020ECC
	private void CalculateNavMeshPath()
	{
		this._agent.enabled = true;
		if (this.character.targetPositionInRoom.roomName != "undefined_room" && this.character.targetPositionInRoom.roomName != this.character.room.name)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				this.character.name,
				" has targetPositionInRoom.roomName set to ",
				this.character.targetPositionInRoom.roomName,
				" but is in room ",
				this.character.room.name
			}));
			return;
		}
		if (this.character.room.GetTile(this.character.targetPositionInRoom.localPosition) == null)
		{
			Debug.Log("No friggin tile there, let's not walk");
			this.character.CancelWalking();
			return;
		}
		Vector3 vector = this.OffsetedPositionOnTargetTile();
		Vector3 vector2 = vector;
		Ray ray = new Ray(vector + Vector3.up * 200f, Vector3.down);
		RaycastHit[] array = Physics.RaycastAll(ray, 300f);
		float num = -50f;
		foreach (RaycastHit raycastHit in array)
		{
			if (raycastHit.transform.tag == "PhysicsFloor" && raycastHit.point.y > num)
			{
				num = raycastHit.point.y;
			}
		}
		if (num > -50f)
		{
			vector2 += new Vector3(0f, num, 0f);
		}
		this._agent.SetDestination(vector2);
		this._agent.angularSpeed = 360f;
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00022EC0 File Offset: 0x000210C0
	private Vector3 OffsetedPositionOnTargetTile()
	{
		return MimanHelper.TilePositionToVector3(this.character.targetPositionInRoom.localPosition);
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00022EE8 File Offset: 0x000210E8
	private void TurnOffNavMeshNavigation()
	{
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00022EEC File Offset: 0x000210EC
	protected override void ShellUpdate()
	{
		if (this._hideForAWhileWhenEnteringRoom >= 0f)
		{
			this._hideForAWhileWhenEnteringRoom -= Time.deltaTime;
			if (this._hideForAWhileWhenEnteringRoom <= 0f)
			{
				base.GetComponentInChildren<Renderer>().enabled = true;
			}
		}
		if (this._agent == null)
		{
			Debug.LogError("Agent is null for " + base.name);
		}
		if (this._waitForThisTramGateToClose != null)
		{
			if (!this._waitForThisTramGateToClose.canWalkOverIt)
			{
				return;
			}
			this._waitForThisTramGateToClose = null;
		}
		if (this.character.walkMode != Character.WalkMode.NO_TARGET && this.character.actionName == "Walking")
		{
			this.CalculateNavMeshPath();
		}
		this.SetEnableOnNavmeshAgent();
		this._movementDelta = Vector3.Distance(base.transform.position, this._prevFramePosition);
		this._prevFramePosition = base.transform.position;
		float num = this._movementDelta / Time.deltaTime;
		if (float.IsNaN(num))
		{
			Debug.LogError("NaN!");
		}
		if (this._isAvatar)
		{
			if (this._deadZoneOn)
			{
				if (num > 0.2f && this.IsOutsideDeadzone())
				{
					this._deadZoneOn = false;
				}
			}
			else if (num < 0.2f && this.IsOutsideDeadzone())
			{
				this._deadZoneOn = true;
				this._deadZoneCenter = base.transform.position;
			}
			float num2 = 1f + this._movementDelta * 0.4f;
			float num3 = 2f * num2;
			float num4 = this._lookAheadDistance - num3;
			this._lookAheadDistance -= num4 * 0.05f;
			Vector3 vector = this.CalculateLookAhead() - this._lookTargetPoint;
			Vector3 vector2 = this._pid.Update(vector, Time.deltaTime);
			this._lookTargetPoint += vector2;
		}
		RaycastHit[] array = Physics.SphereCastAll(base.transform.position, 2f, base.transform.forward, 1f);
		TramGate tramGate = null;
		foreach (RaycastHit raycastHit in array)
		{
			TramGate component = raycastHit.transform.GetComponent<TramGate>();
			if (component != null)
			{
				tramGate = component;
				break;
			}
		}
		if (tramGate != null)
		{
			if (tramGate.canWalkOverIt)
			{
				Debug.Log("Walkable tram gate in front of " + base.name + ", keep on walking");
			}
			else
			{
				Debug.Log("Tram gate in the way for " + base.name);
				this.character.CancelWalking();
				this._agent.Stop();
				base.transform.Translate(Vector3.back * 1f);
				if (!this._isAvatar)
				{
					this._waitForThisTramGateToClose = tramGate;
				}
			}
		}
		if (this._agent.enabled)
		{
			Vector3 position = base.transform.position;
			position.Scale(new Vector3(1f, 0f, 1f));
			float magnitude = (this.OffsetedPositionOnTargetTile() - position).magnitude;
			if (magnitude > 2f)
			{
				IntPoint intPoint = MimanHelper.Vector3ToTilePoint(base.transform.position);
				this.character.position = new WorldCoordinate(this.character.room.name, intPoint);
				this.character.direction = GridMath.DegreesToDirection((int)base.transform.rotation.eulerAngles.y);
				this.character.walkTimer = 0f;
			}
			this._agent.speed = ((!(this.character.actionName == "Walking")) ? 0f : this.character.calculateFinalWalkSpeed());
			if (this.IsTryingToRunAndSucceeds())
			{
				this._agent.speed *= 2f;
			}
			if (this.character.actionName == "Walking")
			{
				if ((double)num < 2.0)
				{
					this._slowTimer += Time.deltaTime;
				}
				if (this._slowTimer > 0.25f)
				{
					this._agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
					this._reactivateAgentCollider = 1f;
				}
			}
			else
			{
				this._slowTimer = 0f;
			}
			if (this._reactivateAgentCollider > 0f)
			{
				this._reactivateAgentCollider -= Time.deltaTime;
				if (this._reactivateAgentCollider < 0.01f)
				{
					this._agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
					Debug.Log("Agent collider activated again!");
				}
			}
			if (magnitude < 0.5f)
			{
				base.ApplyTransformPositionToTing();
				base.ApplyTransformRotationToTing();
				this.character.AnalyzeNewTile();
			}
		}
		if (this._isAvatar)
		{
			this._ear.transform.position = base.transform.position + new Vector3(0f, 2f, 0f);
			this._ear.transform.eulerAngles = Camera.main.transform.eulerAngles;
		}
		if ((this.character.handItem != null && this._shellInHand == null) || (this.character.handItem == null && this._shellInHand != null) || (this.character.handItem != null && this._shellInHand != null && this.character.handItem != this._shellInHand.ting))
		{
			string text = ((this.character.handItem != null) ? this.character.handItem.name : string.Empty);
			this.OnNewHandItem(text, false);
		}
		if (this.character.handItem != null && this._shellInHand != null && this._shellInHand.audio != null && this.character.actionName == "PutHandItemIntoInventory")
		{
			this._shellInHand.audio.volume -= Time.deltaTime * 0.5f;
		}
		if (this._shellInHand != null)
		{
			this.FixPositionAndRotationOfShellInHand();
		}
		if (this._animator != null)
		{
			this._animator.SetBool(CharacterShell.SLEEPING, this.character.actionName == "Sleeping" || this.character.actionName == "FallAsleepFromStanding");
			this._animator.SetBool(CharacterShell.LAYING, this.character.laying);
			this._animator.SetBool(CharacterShell.SITTING, this.character.sitting);
			this._animator.SetBool(CharacterShell.RUNNING, this.running || this.character.running);
			this._animator.SetBool(CharacterShell.DANCING, this.character.actionName == "Dancing");
			this._animator.SetBool(CharacterShell.MIXING, this.character.actionName == "Mixing");
			this._animator.SetBool(CharacterShell.TRUMPETING, this.character.actionName == "Trumpeting");
			this._animator.SetBool(CharacterShell.HACKING, this.character.actionName == "Hacking");
			this._animator.SetBool(CharacterShell.TALKING_IN_TELEPHONE, this.character.actionName == "TalkingInTelephone");
			bool flag = this.character.actionName == "UsingComputer" && !this._isAvatar && this.ComputerHasKeyboard();
			this._animator.SetBool("UsingComputer", flag);
			this._animator.SetFloat("RandomNr", global::UnityEngine.Random.value);
			this._animator.SetFloat("MovementSpeed", (!(this.character.actionName == "Walking")) ? 0f : 1f);
			if (flag)
			{
				if (!this._audioSource.isPlaying)
				{
					SoundDictionary.PlaySound("SittingUsingComputer", this._audioSource);
					this._audioSource.time = Randomizer.GetValue(0f, this._audioSource.clip.length * 0.5f);
				}
				if (Randomizer.GetIntValue(0, 50) == 0)
				{
					this.TypeRandomShit();
				}
			}
			if (Time.frameCount % 150 == 0)
			{
				this._idleRandomizerTargetValue = Randomizer.GetValue(0f, 1f);
			}
			float num5 = this._animator.GetFloat(CharacterShell.IDLE_RANDOMIZER);
			if (num5 < this._idleRandomizerTargetValue - 0.14f)
			{
				num5 += Time.deltaTime * 0.13f;
			}
			else if (num5 > this._idleRandomizerTargetValue + 0.14f)
			{
				num5 -= Time.deltaTime * 0.13f;
			}
			this._animator.SetFloat(CharacterShell.IDLE_RANDOMIZER, num5);
		}
		if (this._lowpassFilter != null)
		{
			this._lowpassFilter.enabled = this._onCarpet && this.character.actionName == "Walking";
			this._lowpassFilter.cutoffFrequency = 600f;
		}
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x000238E0 File Offset: 0x00021AE0
	private void TypeRandomShit()
	{
		Computer computer = this.character.actionOtherObject as Computer;
		if (computer == null)
		{
			D.Log("Computer (actionOtherObject) is null for " + base.name + " when typing random stuff.");
			return;
		}
		if (computer.containsBrokenPrograms || !computer.masterProgram.isOn)
		{
			computer.masterProgram.ClearErrors();
			computer.masterProgram.Start();
		}
		string text = Randomizer.RandNth<string>(CharacterShell.randomInput);
		for (int i = 0; i < text.Length; i++)
		{
			computer.OnKeyDown(text[i].ToString());
		}
		computer.OnEnterKey();
		computer.OnDirectionKey(Randomizer.RandNth<string>(CharacterShell.randomDirection));
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x000239A0 File Offset: 0x00021BA0
	private bool IsOutsideDeadzone()
	{
		Vector3 vector = Camera.main.WorldToScreenPoint(base.transform.position);
		float num = 0.25f;
		float num2 = 0.35f;
		float num3 = vector.x / (float)Screen.width;
		float num4 = 1f - vector.y / (float)Screen.height;
		return num3 < num || num3 > 1f - num || num4 < num2 || num4 > 1f - num2;
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00023A20 File Offset: 0x00021C20
	private bool ComputerHasKeyboard()
	{
		ComputerShell computerShell = this.actionOtherObjectShell as ComputerShell;
		return !(computerShell == null) && computerShell.hasKeyboard;
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00023A50 File Offset: 0x00021C50
	private bool IsInAnimationTransition()
	{
		bool flag = this._animator.GetAnimatorTransitionInfo(0).IsName(string.Empty);
		return !flag;
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00023A7C File Offset: 0x00021C7C
	public void ANIMATION_EVENT_OnLeftFootDown()
	{
		if (this.IsInAnimationTransition())
		{
			return;
		}
		if (this._leftFoot != null)
		{
			this.CheckGroundMaterial(this._leftFoot.position);
		}
		else
		{
			this.CheckGroundMaterial(base.transform.position);
		}
		this._walkSoundManager.PlaySound(this._currentGroundMaterial + ((!this.running) ? string.Empty : "Run"), WalkSoundManager.Foot.Left, this._audioSource);
		this.MaybePlayCreak();
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x00023B0C File Offset: 0x00021D0C
	public void ANIMATION_EVENT_OnRightFootDown()
	{
		if (this.IsInAnimationTransition())
		{
			return;
		}
		if (this._leftFoot != null)
		{
			this.CheckGroundMaterial(this._rightFoot.position);
		}
		else
		{
			this.CheckGroundMaterial(base.transform.position);
		}
		this._walkSoundManager.PlaySound(this._currentGroundMaterial + ((!this.running) ? string.Empty : "Run"), WalkSoundManager.Foot.Right, this._audioSource);
		this.MaybePlayCreak();
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00023B9C File Offset: 0x00021D9C
	private void MaybePlayCreak()
	{
		if (this._currentGroundMaterial == "Smallwood" && Randomizer.OneIn(5))
		{
			SoundDictionary.PlaySoundOneShot("FootstepCreak", this._creakAudioSource);
		}
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00023BDC File Offset: 0x00021DDC
	public void ANIMATION_EVENT_OnScuffFeet()
	{
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00023BE0 File Offset: 0x00021DE0
	protected bool IsTryingToRunAndSucceeds()
	{
		return (this.running || this.character.running) && this.character.sleepinessState == Character.SleepinessState.FRESH;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00023C1C File Offset: 0x00021E1C
	private bool IsPlayingGetUpFromSeatAnimation()
	{
		return this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.GetUpChairLeft") || this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.GetUpChairRight") || this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.GetUpChairFront");
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00023C84 File Offset: 0x00021E84
	private bool IsPlayingGetUpFromBedAnimation()
	{
		return this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.GetUpBedLeft") || this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.GetUpBedRight");
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x00023CCC File Offset: 0x00021ECC
	private void LateUpdate()
	{
		if (this._animator == null)
		{
			return;
		}
		if (this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.SitIdleA1") && !this.character.sitting && this.character.actionName != "GettingUpFromSeat")
		{
			if (this.character.seat != null)
			{
				this.character.Sit(this.character.seat);
			}
			else
			{
				Debug.Log("Character has no seat, current actionName is " + this.character.actionName);
				this._animator.Play("Idle");
			}
		}
		if (this.character.actionName == "GettingUpFromSeat" && this.character.seat != null && !this.IsPlayingGetUpFromSeatAnimation())
		{
			IntPoint intPoint = this.character.seat.GetCurrentExitPoint() - this.character.seat.position.localPosition;
			float num = IntPoint.DirectionToIntPoint(this.character.seat.direction).Degrees();
			float num2;
			for (num2 = num - intPoint.Degrees(); num2 < 0f; num2 += 360f)
			{
			}
			if (num2 > 225f && num2 < 315f)
			{
				this._animator.Play("GetUpChairLeft");
			}
			else if (num2 > 45f && num2 < 135f)
			{
				this._animator.Play("GetUpChairRight");
			}
			else
			{
				this._animator.Play("GetUpChairFront");
			}
		}
		if (this.IsPlayingGetUpFromSeatAnimation() && this.character.actionName != string.Empty && this.character.seat != null)
		{
			if (this.character.seat != null)
			{
				IntPoint intPoint2 = this.character.seat.GetCurrentExitPoint() - this.character.seat.position.localPosition;
				float num3 = IntPoint.DirectionToIntPoint(this.character.seat.direction).Degrees();
				float num4;
				for (num4 = num3 - intPoint2.Degrees(); num4 < 0f; num4 += 360f)
				{
				}
				if (num4 > 225f && num4 < 315f)
				{
					this.character.direction = intPoint2.ToDirection();
				}
				else if (num4 > 45f && num4 < 135f)
				{
					this.character.direction = intPoint2.ToDirection();
				}
				else
				{
					this.character.direction = intPoint2.ToDirection();
				}
				this.character.GetUpSeatSnap();
			}
			else
			{
				Debug.Log("Character has no seat, current actionName is " + this.character.actionName);
			}
		}
		if (this.IsPlayingGetUpFromSeatAnimation() && this._animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f && this.character.actionName == "GettingUpFromSeat")
		{
			this.character.AfterGettingUpFromSeat();
		}
		if ((this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.SleepingLeft") || this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.SleepingRight")) && !this.character.laying && this.character.actionName != "GettingUpFromBed")
		{
			if (this.character.bed != null)
			{
				this.character.LayInBed(this.character.bed);
			}
			else
			{
				Debug.Log("Character has no bed, current actionName is " + this.character.actionName);
				this._animator.Play("Idle");
			}
		}
		if (this.character.actionName == "GettingUpFromBed" && this.character.bed != null && !this.IsPlayingGetUpFromBedAnimation())
		{
			float num5 = 90f;
			IntPoint intPoint3 = this.character.bed.GetCurrentExitPoint() - this.character.bed.position.localPosition;
			float num6 = IntPoint.DirectionToIntPoint(this.character.bed.direction).Degrees() + num5;
			float num7;
			for (num7 = num6 - intPoint3.Degrees(); num7 < 0f; num7 += 360f)
			{
			}
			if (num7 > 225f && num7 < 315f)
			{
				this._animator.Play("GetUpBedLeft");
			}
			else if (num7 > 45f && num7 < 135f)
			{
				this._animator.Play("GetUpBedRight");
			}
			else
			{
				Debug.LogError("Can't get up at footside of bed!");
			}
		}
		if (this.IsPlayingGetUpFromBedAnimation() && this.character.actionName != string.Empty && this.character.bed != null)
		{
			if (this.character.bed != null)
			{
				this.character.GetUpBedSnap();
			}
			else
			{
				Debug.LogError("Character has no bed, current actionName is " + this.character.actionName);
			}
		}
		if (this.IsPlayingGetUpFromBedAnimation() && this._animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f && this.character.actionName == "GettingUpFromBed")
		{
			this.character.AfterGettingUpFromBed();
			this._animator.CrossFade("Idle", 0f);
		}
		if (this._shellInHand != null)
		{
		}
		if (this._ghostParticles != null)
		{
			this._ghostParticles.transform.rotation = Quaternion.identity;
		}
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x0002431C File Offset: 0x0002251C
	private void OnAnimatorIK()
	{
		if (this._animator == null)
		{
			return;
		}
		if (this.character.actionName == "PickingUp" && this.character.actionOtherObject != null)
		{
			Shell shellWithName = ShellManager.GetShellWithName(this.character.actionOtherObject.name);
			if (shellWithName != null)
			{
				this._animator.SetIKPosition(AvatarIKGoal.RightHand, shellWithName.transform.position);
				this._animator.SetIKPositionWeight(AvatarIKGoal.RightHand, this.ikIntensityOverActionTime());
			}
		}
		if (this.character.conversationTarget == null || this.character.actionOtherObject is Computer)
		{
			return;
		}
		if (this.dontLookAtOtherPersonWhenDoing.Contains(this.character.actionName))
		{
			return;
		}
		CharacterShell characterShell = ShellManager.GetShellWithName(this.character.conversationTarget.name) as CharacterShell;
		if (characterShell == null)
		{
			return;
		}
		Vector3 vector = ((!this.character.sitting) ? new Vector3(0f, 4f, 0f) : new Vector3(0f, 2.5f, 0f));
		this._animator.SetLookAtPosition(characterShell.transform.position + vector);
		this._animator.SetLookAtWeight(1f);
		if (this.character.actionName == "GivingHandItem")
		{
			this._animator.SetIKPosition(AvatarIKGoal.RightHand, characterShell._handPoint.transform.position);
			this._animator.SetIKPositionWeight(AvatarIKGoal.RightHand, this.ikIntensityOverActionTime());
		}
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x000244CC File Offset: 0x000226CC
	private float ikIntensityOverActionTime()
	{
		return Mathf.Abs(0.5f + (this.character.actionPercentage - 0.5f) - 0.1f);
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x000244FC File Offset: 0x000226FC
	private void CheckGroundMaterial(Vector3 pPosition)
	{
		Vector3 vector = pPosition + new Vector3(0f, 20f, 0f);
		Ray ray = new Ray(vector + Vector3.up * 200f, Vector3.down);
		RaycastHit[] array = Physics.RaycastAll(ray);
		if (array != null)
		{
			this._onCarpet = false;
			foreach (RaycastHit raycastHit in array)
			{
				if (raycastHit.transform.tag == "Carpet")
				{
					this._onCarpet = true;
				}
				if ((raycastHit.transform.tag == "PhysicsFloor" || raycastHit.transform.tag == "Carpet") && raycastHit.collider != null)
				{
					string text = raycastHit.collider.material.name.ToLower();
					if (text.Contains("smallwood"))
					{
						this._currentGroundMaterial = "Smallwood";
					}
					else if (text.Contains("concrete"))
					{
						this._currentGroundMaterial = "Concrete";
					}
					else if (text.Contains("tiles"))
					{
						this._currentGroundMaterial = "Tiles";
					}
					else if (text.Contains("woodplank"))
					{
						this._currentGroundMaterial = "Woodplank";
					}
					else if (text.Contains("garvel"))
					{
						this._currentGroundMaterial = "Garvel";
					}
					else if (text.Contains("internet"))
					{
						this._currentGroundMaterial = "Internet";
					}
				}
			}
		}
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x000246C8 File Offset: 0x000228C8
	protected override bool ShouldSnapPosAndDir()
	{
		return this._agent == null || this.character.actionName != "Walking";
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00024700 File Offset: 0x00022900
	protected override void ShellDrawGizmos()
	{
		base.ShellDrawGizmos();
		if (base.hasSetupTingRef)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube(MimanHelper.TilePositionToVector3(this.character.targetPositionInRoom.localPosition), new Vector3(0.2f, 3f, 0.2f));
			if (this._deadZoneOn)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(this._deadZoneCenter, 0.5f);
			}
		}
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00024780 File Offset: 0x00022980
	private Vector3 CalculateLookAhead()
	{
		float num = base.transform.rotation.eulerAngles.y - 90f;
		float num2 = Mathf.Cos(num * 0.017453292f) * this._lookAheadDistance;
		float num3 = Mathf.Sin(num * 0.017453292f) * this._lookAheadDistance;
		float num4 = 20f;
		Vector3 vector = new Vector3(Mathf.Clamp(num2, -num4, num4), 0f, Mathf.Clamp(-num3, -num4, num4));
		Vector3 vector2 = new Vector3(0f, (!this.character.sitting && !this.character.laying) ? 4f : 3f, 0f);
		Vector3 vector3 = Vector3.zero;
		if (this.character.conversationTarget != null)
		{
			vector3 = vector2 + base.transform.position;
		}
		else if (this._deadZoneOn)
		{
			vector3 = vector2 + this._deadZoneCenter;
		}
		else
		{
			vector3 = vector2 + base.transform.position + vector;
		}
		if (NewCameraState.ValidVector3(vector3))
		{
			return vector3;
		}
		return Vector3.zero;
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x000248BC File Offset: 0x00022ABC
	private void RefreshGhostParticles()
	{
		if (this.character.corruption < 0.01f)
		{
			if (this._ghostParticles != null)
			{
				this._ghostParticles.particleSystem.enableEmission = false;
			}
		}
		else
		{
			if (this._ghostParticles == null)
			{
				GameObject gameObject = Resources.Load("GhostParticles") as GameObject;
				GameObject gameObject2 = global::UnityEngine.Object.Instantiate(gameObject) as GameObject;
				this._ghostParticles = gameObject2.transform;
				this._ghostParticles.parent = base.transform;
				this._ghostParticles.localPosition = new Vector3(0f, 2.5f, 0f);
			}
			this._ghostParticles.particleSystem.enableEmission = true;
			this._ghostParticles.particleSystem.emissionRate = this.character.corruption;
		}
		if (this.corruptionMaterial == null)
		{
			this.corruptionMaterial = Resources.Load("CorruptionMaterial") as Material;
		}
		SkinnedMeshRenderer componentInChildren = base.GetComponentInChildren<SkinnedMeshRenderer>();
		if (componentInChildren != null)
		{
			if (this._originalMaterials == null)
			{
				this._originalMaterials = componentInChildren.materials;
			}
			List<Material> list = new List<Material>();
			for (int i = 0; i < componentInChildren.materials.Length; i++)
			{
				if (i == 5 && this._isAvatar && this.character.HasKnowledge("NewShoes"))
				{
					Material newShoesMaterial = CharacterShell.GetNewShoesMaterial();
					list.Add(newShoesMaterial);
				}
				else if ((float)Randomizer.GetIntValue(0, 100) < this.character.corruption)
				{
					list.Add(this.corruptionMaterial);
				}
				else
				{
					list.Add(this._originalMaterials[i]);
				}
			}
			componentInChildren.materials = list.ToArray();
		}
		else
		{
			Debug.Log("Found no mesh renderer for " + base.name);
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060004FF RID: 1279 RVA: 0x00024AA8 File Offset: 0x00022CA8
	public override Vector3 lookTargetPoint
	{
		get
		{
			return this._lookTargetPoint;
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00024AB0 File Offset: 0x00022CB0
	public void ResetLookTargetPoint()
	{
		this._lookTargetPoint = base.transform.position;
		this._deadZoneOn = false;
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000501 RID: 1281 RVA: 0x00024ACC File Offset: 0x00022CCC
	protected override bool useDigitalBubbles
	{
		get
		{
			return false;
		}
	}

	// Token: 0x04000392 RID: 914
	private NavMeshAgent _agent;

	// Token: 0x04000393 RID: 915
	private MessageCollector _animationEventCollector;

	// Token: 0x04000394 RID: 916
	private Animator _animator;

	// Token: 0x04000395 RID: 917
	private static readonly int DRUNKENNESS = Animator.StringToHash("Drunkenness");

	// Token: 0x04000396 RID: 918
	private static readonly int SMELLINESS = Animator.StringToHash("Smelliness");

	// Token: 0x04000397 RID: 919
	private static readonly int SLEEPINESS = Animator.StringToHash("Sleepiness");

	// Token: 0x04000398 RID: 920
	private static readonly int CHARISMA = Animator.StringToHash("Charisma");

	// Token: 0x04000399 RID: 921
	private static readonly int HAPPINESS = Animator.StringToHash("Happiness");

	// Token: 0x0400039A RID: 922
	private static readonly int SUPREMACY = Animator.StringToHash("Supremacy");

	// Token: 0x0400039B RID: 923
	private static readonly int SLEEPING = Animator.StringToHash("Sleeping");

	// Token: 0x0400039C RID: 924
	private static readonly int LAYING = Animator.StringToHash("Laying");

	// Token: 0x0400039D RID: 925
	private static readonly int SITTING = Animator.StringToHash("Sitting");

	// Token: 0x0400039E RID: 926
	private static readonly int RUNNING = Animator.StringToHash("Running");

	// Token: 0x0400039F RID: 927
	private static readonly int DANCING = Animator.StringToHash("Dancing");

	// Token: 0x040003A0 RID: 928
	private static readonly int HACKING = Animator.StringToHash("Hacking");

	// Token: 0x040003A1 RID: 929
	private static readonly int MIXING = Animator.StringToHash("Mixing");

	// Token: 0x040003A2 RID: 930
	private static readonly int TRUMPETING = Animator.StringToHash("Trumpeting");

	// Token: 0x040003A3 RID: 931
	private static readonly int TALKING_IN_TELEPHONE = Animator.StringToHash("TalkingInTelephone");

	// Token: 0x040003A4 RID: 932
	private static readonly int IDLE_RANDOMIZER = Animator.StringToHash("IdleRandomizer");

	// Token: 0x040003A5 RID: 933
	private string _currentGroundMaterial = "Concrete";

	// Token: 0x040003A6 RID: 934
	private AudioListener _audioListener;

	// Token: 0x040003A7 RID: 935
	private Transform _ear;

	// Token: 0x040003A8 RID: 936
	private WalkSoundManager _walkSoundManager;

	// Token: 0x040003A9 RID: 937
	private Transform _leftFoot;

	// Token: 0x040003AA RID: 938
	private Transform _rightFoot;

	// Token: 0x040003AB RID: 939
	private bool _onCarpet;

	// Token: 0x040003AC RID: 940
	private AudioLowPassFilter _lowpassFilter;

	// Token: 0x040003AD RID: 941
	private AudioSource _creakAudioSource;

	// Token: 0x040003AE RID: 942
	private Transform _mouthPosition;

	// Token: 0x040003AF RID: 943
	private float _hideForAWhileWhenEnteringRoom;

	// Token: 0x040003B0 RID: 944
	public bool running;

	// Token: 0x040003B1 RID: 945
	private bool _isAvatar;

	// Token: 0x040003B2 RID: 946
	private Transform _handPoint;

	// Token: 0x040003B3 RID: 947
	private Shell _shellInHand;

	// Token: 0x040003B4 RID: 948
	private Transform _ghostParticles;

	// Token: 0x040003B5 RID: 949
	private Transform _gloria;

	// Token: 0x040003B6 RID: 950
	private Transform _flies;

	// Token: 0x040003B7 RID: 951
	private float _movementDelta;

	// Token: 0x040003B8 RID: 952
	private Vector3 _prevFramePosition;

	// Token: 0x040003B9 RID: 953
	private float _slowTimer;

	// Token: 0x040003BA RID: 954
	private HackEffectSpawnPoint _hackEffect;

	// Token: 0x040003BB RID: 955
	private float _idleRandomizerTargetValue;

	// Token: 0x040003BC RID: 956
	private Vector3 _lookTargetPoint;

	// Token: 0x040003BD RID: 957
	private bool _deadZoneOn;

	// Token: 0x040003BE RID: 958
	private float _lookAheadDistance;

	// Token: 0x040003BF RID: 959
	private Vector3 _deadZoneCenter;

	// Token: 0x040003C0 RID: 960
	private PidVector3 _pid = new PidVector3();

	// Token: 0x040003C1 RID: 961
	private float _reactivateAgentCollider;

	// Token: 0x040003C2 RID: 962
	private TramGate _waitForThisTramGateToClose;

	// Token: 0x040003C3 RID: 963
	private string _debugMessage;

	// Token: 0x040003C4 RID: 964
	private static HashSet<string> s_immediateAnimations = new HashSet<string> { "WalkingThroughDoorPhase2", "WalkingThroughPortalPhase2" };

	// Token: 0x040003C5 RID: 965
	private static readonly Dictionary<string, string> animStateLookup = new Dictionary<string, string>
	{
		{ "WalkingThroughDoorPhase2", "OpenDoorPhase2" },
		{ "WalkingThroughPortalPhase2", "WalkingThroughPortalPhase2" },
		{ "KickingLamp", "Kick" },
		{ "PushingButton", "PushButtonOnPillar" },
		{ "ActivatingVendingMachine", "PushButtonOnPillar" },
		{ "UsingDoorWithKey", "UsingDoorWithKey" },
		{ "Screwing", "UsingDoorWithKey" },
		{ "WalkingThroughFence", "WalkingThroughPortal" },
		{ "Angry", "Angry" },
		{ "Hello", "Hello" },
		{ "Shrug", "Shrug" },
		{ "Mixing", "Mixing" },
		{ "Trumpeting", "Trumpeting" },
		{ "Talking", "StartTalking" },
		{ "Drinking", "Drinking" },
		{ "Dancing", "Dancing" },
		{ "TakingDrug", "EatPill" },
		{ "Eat", "Eat" },
		{ "SmokingCigarette", "SmokingCigarette" },
		{ "TakingSnus", "TakingSnus" },
		{ "BeBored", "BeBored" },
		{ "ThrowingTingIntoTrashCan", "ThrowAway" },
		{ "PuttingTingIntoSendPipe", "ThrowAway" },
		{ "GivingHandItem", "GiveThing" },
		{ "ReceivingHandItem", "ReceiveThing" },
		{ "PutHandItemIntoInventory", "PutBackItem" },
		{ "TakeOutInventoryItem", "TakeOutItem" },
		{ "TalkingInTelephone", "TalkTelephone" },
		{ "PushingButtonOnHandItem", "PushObjectInHand" },
		{ "StartingJukebox", "PushButtonOnPillar" },
		{ "UseSink", "TurnWaterTap" },
		{ "RefillingDrink", "RefillWater" },
		{ "Extracting", "UsingDoorWithKey" },
		{ "UseStove", "PushButtonOnPillar" },
		{ "Tasing", "Taze" },
		{ "GettingTased", "Tazed" },
		{ "GettingTasedGently", "Tazed" },
		{ "AngryAtComputer", "ComputerTrouble1" },
		{ "PuttingTingIntoLocker", "PutDownGround" }
	};

	// Token: 0x040003C6 RID: 966
	private static readonly Dictionary<string, string> sittingAnimStateLookup = new Dictionary<string, string>
	{
		{ "SmokingCigarette", "SitSmoke" },
		{ "Drinking", "SitDrink" },
		{ "Shrug", "DontKnowSitting" },
		{ "GivingHandItem", "SitGiveThing" },
		{ "ReceivingHandItem", "SitGiveThing" },
		{ "Eat", "SitEat" },
		{ "TakingDrug", "SitEat" },
		{ "TalkingInTelephone", "SitPhone" },
		{ "TakeOutInventoryItem", "SitTakeOutItem" },
		{ "PutHandItemIntoInventory", "SitTakeOutItem" },
		{ "GettingTased", "SitTased" },
		{ "GettingTasedGently", "SitTased" },
		{ "TakingSnus", "SitSnusa" }
	};

	// Token: 0x040003C7 RID: 967
	private static HashSet<string> snapTingToShellOnTheseNewActions = new HashSet<string> { "Walking", "BeingBothered" };

	// Token: 0x040003C8 RID: 968
	private static Material s_newShoesMaterial;

	// Token: 0x040003C9 RID: 969
	private bool _setNewShoes;

	// Token: 0x040003CA RID: 970
	private static readonly HashSet<string> actionsThatDontTriggerSound = new HashSet<string> { "Sleeping", "FallAsleepFromStanding", "Walking" };

	// Token: 0x040003CB RID: 971
	private static List<string> randomInput = new List<string> { "calc", "help", "connect", "boot", "sort", "net" };

	// Token: 0x040003CC RID: 972
	private static List<string> randomDirection = new List<string> { "left", "right", "up", "down" };

	// Token: 0x040003CD RID: 973
	private HashSet<string> dontLookAtOtherPersonWhenDoing = new HashSet<string> { "Drinking", "Eat", "TakingSnus", "SmokingCigarette", "TakingDrug" };

	// Token: 0x040003CE RID: 974
	private Material[] _originalMaterials;

	// Token: 0x040003CF RID: 975
	private Material corruptionMaterial;
}
