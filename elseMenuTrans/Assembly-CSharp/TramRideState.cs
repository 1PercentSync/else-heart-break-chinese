using System;
using GameWorld2;
using RelayLib;
using TingTing;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class TramRideState : GameViewState
{
	// Token: 0x060001EB RID: 491 RVA: 0x0000EC8C File Offset: 0x0000CE8C
	public TramRideState(ActionMenu pActionMenu, Tram pTram, GameViewControls pGameViewControls)
	{
		this._tram = pTram;
		this._skin = (GUISkin)Resources.Load("ActionMenuStyle");
		if (this._skin == null)
		{
			Debug.LogError("Failed to load skin");
		}
		this._actionMenu = pActionMenu;
		this._backButton = new BackButton(this._easyAnimate);
		this._controls = pGameViewControls;
		this._world = this._controls.world;
		this._previousAvatarName = this._world.settings.avatarName;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0000ED1C File Offset: 0x0000CF1C
	public override void OnEnterBegin()
	{
		this._actionMenu.FadeOut();
		this._backButton.Show();
		this._world.settings.avatarName = this._tram.name;
		this.avatar.AddDataListener<WorldCoordinate>("position", new ValueEntry<WorldCoordinate>.DataChangeHandler(this.OnAvatarPositionChanged));
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0000ED78 File Offset: 0x0000CF78
	private void OnAvatarPositionChanged(WorldCoordinate pPreviousPosition, WorldCoordinate pNewPosition)
	{
		base.controls.world.settings.activeRoom = pNewPosition.roomName;
	}

	// Token: 0x060001EE RID: 494 RVA: 0x0000EDA4 File Offset: 0x0000CFA4
	public override GameViewState.RETURN OnEnterLoop()
	{
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x060001EF RID: 495 RVA: 0x0000EDA8 File Offset: 0x0000CFA8
	public override void OnExitBegin()
	{
		this.avatar.RemoveDataListener<WorldCoordinate>("position", new ValueEntry<WorldCoordinate>.DataChangeHandler(this.OnAvatarPositionChanged));
		this._actionMenu.FadeIn();
		this._backButton.Hide();
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x0000EDE8 File Offset: 0x0000CFE8
	public override GameViewState.RETURN OnExitLoop()
	{
		if (base.controls.roomChanger.busy)
		{
			return GameViewState.RETURN.RUN_AGAIN;
		}
		this._world.settings.avatarName = this._previousAvatarName;
		Ting ting = this._world.tingRunner.GetTing(this._previousAvatarName);
		string name = ting.room.name;
		Debug.Log("Will change back to character avatar's room: " + name);
		base.controls.world.settings.activeRoom = name;
		base.controls.roomChanger.LoadRoom(name);
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000EE80 File Offset: 0x0000D080
	public override void OnUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			base.EndState();
		}
		PlayerRoamingState.ControlCamera(this._controls.camera, true, 0f);
		this.CheckForRoomChange();
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000EEBC File Offset: 0x0000D0BC
	private void CheckForRoomChange()
	{
		if (base.controls.roomChanger.busy)
		{
			Debug.Log("Room changer is busy in tram ride state");
			return;
		}
		if (base.controls.world.settings.activeRoom != base.controls.roomChanger.currentRoom)
		{
			Debug.Log("Detected immediate change from room " + base.controls.roomChanger.currentRoom + " to " + base.controls.world.settings.activeRoom);
			base.controls.roomChanger.LoadRoom(base.controls.world.settings.activeRoom);
		}
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000EF78 File Offset: 0x0000D178
	public override void OnGUI()
	{
		GUI.skin = this._skin;
		GUI.color = Color.white;
		if (this._backButton.RenderAndMaybeGoBack())
		{
			base.EndState();
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000EFA8 File Offset: 0x0000D1A8
	private Tram avatar
	{
		get
		{
			return base.controls.avatar as Tram;
		}
	}

	// Token: 0x0400013B RID: 315
	private ActionMenu _actionMenu;

	// Token: 0x0400013C RID: 316
	private GUISkin _skin;

	// Token: 0x0400013D RID: 317
	private BackButton _backButton;

	// Token: 0x0400013E RID: 318
	private Tram _tram;

	// Token: 0x0400013F RID: 319
	private GameViewControls _controls;

	// Token: 0x04000140 RID: 320
	private string _previousAvatarName;

	// Token: 0x04000141 RID: 321
	private World _world;
}
