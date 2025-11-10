using System;
using GameWorld2;
using TingTing;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class DoorShell : Shell
{
	// Token: 0x1700007B RID: 123
	// (get) Token: 0x0600052A RID: 1322 RVA: 0x00025AF0 File Offset: 0x00023CF0
	public Door door
	{
		get
		{
			return base.ting as Door;
		}
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00025B00 File Offset: 0x00023D00
	public override void CreateTing()
	{
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00025B04 File Offset: 0x00023D04
	protected override void Setup()
	{
		base.Setup();
		SoundDictionary.LoadSingleSound("DoorUnlock", "DoorUnlock sound 0");
		SoundDictionary.LoadSingleSound("DoorLock", "DoorLock sound 0");
		SoundDictionary.LoadSingleSound("DoorMetalLock", "LockedDoor metal sound 0");
		SoundDictionary.LoadSingleSound("ElevatorOldDoorOpen", "Elevator old door open sound 1");
		SoundDictionary.LoadSingleSound("ElevatorOldDoorClose", "Elevator old door close sound 1");
		SoundDictionary.LoadMultiSound("WalkingThroughDoor_Wood", "WalkingThroughDoor sound", 3);
		SoundDictionary.LoadMultiSound("WalkingThroughDoor_Wood1", "WalkingThroughDoor1 sound", 3);
		SoundDictionary.LoadMultiSound("WalkingThroughDoor_Dubbledoor", "WalkingThroughDubbledoor sound", 3);
		SoundDictionary.LoadMultiSound("WalkingThroughDoor_Metal", "WalkingThroughMetalDoor sound", 3);
		SoundDictionary.LoadMultiSound("WalkingThroughDoor_Metal1", "WalkingThroughMetal1Door sound", 3);
		SoundDictionary.LoadSingleSound("WalkingThroughDoor_Elevator", "WalkingThroughElevatorDoor sound 0");
		SoundDictionary.LoadSingleSound("WalkingThroughDoor_Elevator1", "WalkingThroughElevator1Door sound 0");
		SoundDictionary.LoadSingleSound("WalkingThroughDoor_Ministry", "WalkingThroughMinistryDoor sound 0");
		SoundDictionary.LoadSingleSound("WalkingThroughDoor_Tram", "WalkingThroughTramDoor sound 0");
		SoundDictionary.LoadSingleSound("WalkingThroughDoor_Burrows", "WalkingThroughBurrowsDoor sound 0");
		SoundDictionary.LoadSingleSound("WalkingThroughDoor_Lodge", "WalkingThroughLodgeDoorSound 0");
		SoundDictionary.LoadSingleSound("ElevatorRide_Wood", "Elevator old ride sound 1");
		SoundDictionary.LoadSingleSound("ElevatorRide_Metal", "Elevator old ride sound 1");
		SoundDictionary.LoadSingleSound("ElevatorRide_Elevator", "Elevator old ride sound 1");
		SoundDictionary.LoadSingleSound("ElevatorRide_Elevator1", "ElevatorMinistryRide Sound 0");
		this._animator = base.GetComponentInChildren<Animator>();
		if (this._animator == null)
		{
			Debug.Log("Couldn't find animator on " + base.name);
		}
		else if (this.door.actionName == "Opening")
		{
			float num = ((!(this.openSoundName == "Elevator")) ? 2.15f : 4f);
			float num2 = this.door.actionPercentage * num;
			this.PlayOpenSound();
			if (this._audioSource != null)
			{
				this._audioSource.time = num2;
				Debug.Log("Playing door sound from time: " + this._audioSource.time);
			}
			this._animator.SetTrigger(DoorShell.OPEN_TRIGGER);
			this._animator.playbackTime = num2;
		}
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00025D2C File Offset: 0x00023F2C
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		Door door = this.door;
		door.onNewAction = (Ting.OnNewAction)Delegate.Combine(door.onNewAction, new Ting.OnNewAction(this.OnNewAction));
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x00025D68 File Offset: 0x00023F68
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		Door door = this.door;
		door.onNewAction = (Ting.OnNewAction)Delegate.Remove(door.onNewAction, new Ting.OnNewAction(this.OnNewAction));
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x00025DA4 File Offset: 0x00023FA4
	private void OnNewAction(string pPreviousAction, string pNewAction)
	{
		if (pPreviousAction != "Opening" && pNewAction == "Opening" && this._animator != null)
		{
			this._animator.SetTrigger(DoorShell.OPEN_TRIGGER);
			this.PlayOpenSound();
		}
		if (pNewAction == "Moving" && this._audioSource != null)
		{
			string text = "ElevatorRide_" + this.openSoundName;
			SoundDictionary.PlaySound(text, this._audioSource);
		}
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00025E38 File Offset: 0x00024038
	public void PlayOpenSound()
	{
		if (this._audioSource != null)
		{
			string text = "WalkingThroughDoor_" + this.openSoundName;
			SoundDictionary.PlaySound(text, this._audioSource);
		}
		else
		{
			Debug.Log(base.name + " has no audio source, can't play open sound");
		}
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00025E90 File Offset: 0x00024090
	protected override bool ShouldSnapPosAndDir()
	{
		return this.door.isMoveable;
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00025EA0 File Offset: 0x000240A0
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
	}

	// Token: 0x040003DC RID: 988
	public string openSoundName = "Wood";

	// Token: 0x040003DD RID: 989
	private Animator _animator;

	// Token: 0x040003DE RID: 990
	private static readonly int OPEN_TRIGGER = Animator.StringToHash("Open");
}
