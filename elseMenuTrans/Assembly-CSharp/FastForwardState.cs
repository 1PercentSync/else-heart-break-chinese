using System;
using GameTypes;
using GameWorld2;
using GrimmLib;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class FastForwardState : GameViewState
{
	// Token: 0x06000149 RID: 329 RVA: 0x000086E0 File Offset: 0x000068E0
	public FastForwardState(GameTime pStopTime, Character pAvatar)
	{
		this._stopTime = pStopTime;
		this._avatar = pAvatar;
		this._skin = (GUISkin)Resources.Load("SleepSkin");
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00008724 File Offset: 0x00006924
	public override void OnEnterBegin()
	{
		Time.timeScale = 1f;
		Application.LoadLevel("Empty");
		base.controls.deltaTimeMultiplier = 1f;
		base.controls.updatesPerFrame = 100;
		base.controls.world.dialogueRunner.AddFocusConversationListener(new DialogueRunner.OnFocusConversation(this.OnConversationFocus));
		base.controls.world.dialogueRunner.AddDefocusConversationListener(new DialogueRunner.OnFocusConversation(this.OnConversationDefocus));
		AudioListener.volume = 0.5f;
	}

	// Token: 0x0600014B RID: 331 RVA: 0x000087B0 File Offset: 0x000069B0
	private void OnConversationFocus(string pConversation)
	{
		Debug.Log("FastForwardState received OnConversationFocus message: " + pConversation);
		this._conversationToFocusOnAfterThisState = pConversation;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x000087CC File Offset: 0x000069CC
	private void OnConversationDefocus(string pConversation)
	{
		Debug.Log("FastForwardState received OnConversationDefocus message for conversation " + pConversation);
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000087E0 File Offset: 0x000069E0
	public override GameViewState.RETURN OnEnterLoop()
	{
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x000087E4 File Offset: 0x000069E4
	public override void OnExitBegin()
	{
		base.controls.world.dialogueRunner.RemoveFocusConversationListener(new DialogueRunner.OnFocusConversation(this.OnConversationFocus));
		base.controls.world.dialogueRunner.RemoveDefocusConversationListener(new DialogueRunner.OnFocusConversation(this.OnConversationDefocus));
		if (this._sleepSoundInstance != null)
		{
			global::UnityEngine.Object.Destroy(this._sleepSoundInstance);
		}
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00008850 File Offset: 0x00006A50
	public override GameViewState.RETURN OnExitLoop()
	{
		if (this._conversationToFocusOnAfterThisState != string.Empty)
		{
		}
		if (base.controls.avatar != null)
		{
			base.controls.avatar.StopAction();
		}
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00008894 File Offset: 0x00006A94
	public override void OnUpdate()
	{
		if (this._isEndingState)
		{
			AudioListener.volume = 0.5f * (1f - base.controls.fade.alpha);
		}
		if (!this._isEndingState && base.controls.world.settings.gameTimeClock >= this._stopTime)
		{
			this.EndWithFade();
		}
		if (!this._isEndingState && this._avatar != null && this._avatar.actionName != "Sleeping")
		{
			Debug.Log("Ending ff-state because avatar is not sleeping");
			this.EndWithFade();
		}
		if (this._sleepSoundInstance == null && this._avatar != null && this._avatar.actionName == "Sleeping")
		{
			Debug.Log("Should play sleep music");
			global::UnityEngine.Object @object = Resources.Load("SleepSoundPlayer");
			this._sleepSoundInstance = global::UnityEngine.Object.Instantiate(@object) as GameObject;
			this._sleepSoundInstance.transform.position = RunGameWorld.instance.transform.position;
			this._sleepSoundInstance.audio.Play();
		}
		if (this._sleepSoundInstance == null)
		{
		}
	}

	// Token: 0x06000151 RID: 337 RVA: 0x000089E0 File Offset: 0x00006BE0
	private void QuickSave()
	{
		GameTime gameTimeClock = base.controls.world.settings.gameTimeClock;
		string text = string.Concat(new object[]
		{
			base.controls.avatar.room.name.Replace("_", " "),
			" - Day ",
			gameTimeClock.days,
			" - ",
			gameTimeClock.ToStringWithoutDayAndSeconds().Replace(":", "#")
		});
		base.controls.world.Save(WorldOwner.QUICKSAVE_DATA_PATH + text + " (autosave).json");
	}

	// Token: 0x06000152 RID: 338 RVA: 0x00008A90 File Offset: 0x00006C90
	public void EndWithFade()
	{
		this.QuickSave();
		this._isEndingState = true;
		Debug.Log("FastForwardState swithes scene to " + base.controls.avatar.room.name);
		base.controls.roomChanger.LoadRoomImmediately(base.controls.avatar.room.name);
		base.controls.roomChanger.onRoomHasChanged(string.Empty);
		base.controls.deltaTimeMultiplier = 1f;
		base.controls.updatesPerFrame = 1;
		base.controls.fade.onFadedToTransparent += base.EndState;
		base.controls.fade.speed = 0.5f;
		PlayerRoamingState.performSlowRoomChange = true;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00008B60 File Offset: 0x00006D60
	public override void OnGUI()
	{
		GUI.skin = this._skin;
		GUI.color = new Color(1f, 1f, 1f, base.controls.fade.alpha);
		GUI.Label(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), base.controls.world.settings.gameTimeClock.ToStringWithoutDayAndSeconds());
	}

	// Token: 0x040000CB RID: 203
	private GameTime _stopTime;

	// Token: 0x040000CC RID: 204
	private bool _isEndingState;

	// Token: 0x040000CD RID: 205
	private Character _avatar;

	// Token: 0x040000CE RID: 206
	private GameObject _sleepSoundInstance;

	// Token: 0x040000CF RID: 207
	private GUISkin _skin;

	// Token: 0x040000D0 RID: 208
	private string _conversationToFocusOnAfterThisState = string.Empty;
}
