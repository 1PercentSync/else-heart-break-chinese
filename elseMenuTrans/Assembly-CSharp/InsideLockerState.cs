using System;
using System.Collections.Generic;
using GameTypes;
using GameWorld2;
using TingTing;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class InsideLockerState : GameViewState
{
	// Token: 0x06000170 RID: 368 RVA: 0x0000A044 File Offset: 0x00008244
	public InsideLockerState(GreatCamera pCameraScript, string pLockerName)
	{
		this._lockerName = pLockerName;
		this._cameraScript = pCameraScript;
		this._skin = Resources.Load("InsideComputerSkin") as GUISkin;
		Application.LoadLevel(this._lockerName);
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000A084 File Offset: 0x00008284
	private void NextShell()
	{
		this._currentIndex++;
		List<Shell> shellsInScene = ShellManager.GetShellsInScene();
		if (shellsInScene.Count == 0)
		{
			this._currentFocusedShell = null;
			return;
		}
		this._currentIndex %= shellsInScene.Count;
		this._currentFocusedShell = shellsInScene[this._currentIndex];
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000A0E0 File Offset: 0x000082E0
	public override void OnEnterBegin()
	{
		Character avatar = this.avatar;
		avatar.onNewAction = (Ting.OnNewAction)Delegate.Combine(avatar.onNewAction, new Ting.OnNewAction(this.OnAvatarChangesAction));
		this.NextShell();
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000A110 File Offset: 0x00008310
	public override void OnExitBegin()
	{
		Character avatar = this.avatar;
		avatar.onNewAction = (Ting.OnNewAction)Delegate.Remove(avatar.onNewAction, new Ting.OnNewAction(this.OnAvatarChangesAction));
		this.avatar.StopAction();
		if (this.avatar.room.name != base.controls.roomChanger.currentRoom)
		{
			base.controls.roomChanger.LoadRoom(this.avatar.room.name);
		}
		this._cameraScript.ExitFixedCamera();
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000A1A4 File Offset: 0x000083A4
	public override void OnUpdate()
	{
		if (!this._delayedInstantiationHasTriggered)
		{
			this._delayedInstantiationHasTriggered = true;
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			this.avatar.StopAction();
		}
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000A1D0 File Offset: 0x000083D0
	private void OnAvatarChangesAction(string pPreviousAction, string pNewAction)
	{
		Debug.Log("New action: " + pNewAction);
		if (pPreviousAction == "LookInLocker" && pNewAction != "LookInLocker")
		{
			base.EndState();
		}
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0000A214 File Offset: 0x00008414
	public override void OnGUI()
	{
		GUI.color = Color.white;
		GUI.skin = this._skin;
		GUILayout.Label("Looking inside locker", new GUILayoutOption[0]);
		if (GUILayout.Button("EXIT", new GUILayoutOption[0]))
		{
			this.avatar.StopAction();
		}
		if (ShellManager.GetShellsInScene().Count > 0 && GUILayout.Button("NEXT", new GUILayoutOption[0]))
		{
			this.NextShell();
		}
		if (this._currentFocusedShell != null && GUILayout.Button("TAKE ITEM", new GUILayoutOption[0]))
		{
			this._currentFocusedShell.ting.position = new WorldCoordinate(this.avatar.inventoryRoomName, new IntPoint(0, 0));
			this._currentFocusedShell = null;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000177 RID: 375 RVA: 0x0000A2E8 File Offset: 0x000084E8
	private Character avatar
	{
		get
		{
			return base.controls.avatar as Character;
		}
	}

	// Token: 0x040000E7 RID: 231
	private GreatCamera _cameraScript;

	// Token: 0x040000E8 RID: 232
	private bool _delayedInstantiationHasTriggered;

	// Token: 0x040000E9 RID: 233
	private string _lockerName;

	// Token: 0x040000EA RID: 234
	private GUISkin _skin;

	// Token: 0x040000EB RID: 235
	private Shell _currentFocusedShell;

	// Token: 0x040000EC RID: 236
	private int _currentIndex = -1;
}
