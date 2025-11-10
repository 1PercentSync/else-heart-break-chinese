using System;
using System.Collections.Generic;
using GameTypes;
using GameWorld2;
using RelayLib;
using TingTing;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class WorldLoadingState : GameViewState
{
	// Token: 0x060001F5 RID: 501 RVA: 0x0000EFBC File Offset: 0x0000D1BC
	public WorldLoadingState(bool isInitialLoad, string pInitDataPath, ActionMenu pActionMenu, string pLanguage)
	{
		this._isInitialLoad = isInitialLoad;
		this._initDataPath = pInitDataPath;
		this._actionMenu = pActionMenu;
		this._language = pLanguage;
		this._actionMenu.Hide();
		this._town = Resources.Load("Town_NOSCALE") as Texture;
		this._bar = Resources.Load("LoadingLabel_NOSCALE") as Texture;
		this._barFlipped = Resources.Load("LoadingLabelFlipped_NOSCALE") as Texture;
		this._border = Resources.Load("Border_NOSCALE") as Texture;
		this._frame = Resources.Load("BlackBorder_NOSCALE") as Texture;
		this._loadStepsPerUpdate = 200;
		MainMenu.SetVolume(0f);
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0000F078 File Offset: 0x0000D278
	public override void OnEnterBegin()
	{
		base.OnEnterBegin();
		Application.LoadLevel("Empty");
		Screen.showCursor = false;
		this._waitABit = 100;
		this._hasStartedLoading = false;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000F0A0 File Offset: 0x0000D2A0
	public override GameViewState.RETURN OnEnterLoop()
	{
		if (this._waitABit > 0)
		{
			this._waitABit--;
			return GameViewState.RETURN.RUN_AGAIN;
		}
		this._saveCreator = new InitialSaveFileCreator();
		this._saveCreator.logger.AddListener(new D.LogHandler(Debug.Log));
		if (this._isInitialLoad)
		{
			this._relayEnumerator = this._saveCreator.LoadRelayFromDirectory(this._initDataPath).GetEnumerator();
		}
		else
		{
			this._relayEnumerator = this._saveCreator.LoadFromFile(this._initDataPath).GetEnumerator();
		}
		this._hasStartedLoading = true;
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000F140 File Offset: 0x0000D340
	private IEnumerator<float> FakeLoad()
	{
		for (float x = 0f; x < 1f; x += 5E-06f)
		{
			yield return x;
		}
		yield break;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000F154 File Offset: 0x0000D354
	public override void OnGUI()
	{
		base.OnGUI();
		GUI.color = Color.white;
		GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this._border);
		Rect centerPos = this.GetCenterPos(this._bar);
		Rect rect = new Rect(centerPos.x - (float)this._bar.width + (float)this._bar.width * this._progress, centerPos.y + 8f, centerPos.width, centerPos.height);
		Rect rect2 = new Rect(centerPos.x + (float)this._bar.width - (float)this._bar.width * this._progress, centerPos.y + 8f, centerPos.width, centerPos.height);
		GUI.DrawTexture(rect, this._bar);
		GUI.DrawTexture(rect2, this._barFlipped);
		Rect centerPos2 = this.GetCenterPos(this._town);
		GUI.DrawTexture(centerPos2, this._town);
		GUI.DrawTexture(this.GetCenterPos(this._frame), this._frame);
		GUI.DrawTexture(new Rect(0f, 0f, centerPos2.x, (float)Screen.height), this._border);
		GUI.DrawTexture(new Rect((float)(Screen.width / 2) + centerPos2.width / 2f, 0f, ((float)Screen.width - centerPos2.width) / 2f, (float)Screen.height), this._border);
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0000F2EC File Offset: 0x0000D4EC
	private Rect GetCenterPos(Texture pTexture)
	{
		return new Rect((float)(Screen.width / 2 - pTexture.width / 2), (float)(Screen.height / 2 - pTexture.height / 2), (float)pTexture.width, (float)pTexture.height);
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000F330 File Offset: 0x0000D530
	public override void OnUpdate()
	{
		base.OnUpdate();
		for (int i = 0; i < this._loadStepsPerUpdate; i++)
		{
			this.LoadStuff();
		}
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000F360 File Offset: 0x0000D560
	private void LoadStuff()
	{
		if (!this._hasStartedLoading)
		{
			return;
		}
		if (this._isDoneLoading)
		{
			return;
		}
		if (this._relayEnumerator != null)
		{
			if (!this._relayEnumerator.MoveNext())
			{
				this._relayEnumerator = null;
				this._loadedWorld = new World(this._saveCreator.GetLoadedRelay());
				this._preloadEnumerator = this._loadedWorld.Preload().GetEnumerator();
				this._loadedWorld.translator.LoadTranslationFiles(WorldOwner.INIT_DATA_PATH + "/Translations");
			}
			else
			{
				this._progress = this._relayEnumerator.Current;
			}
		}
		if (this._preloadEnumerator != null && !this._preloadEnumerator.MoveNext())
		{
			RunGameWorld.instance.OnLoadedGame(this._loadedWorld);
			base.ChangeState(new PlayerRoamingState(this._isInitialLoad));
			this._isDoneLoading = true;
			return;
		}
	}

	// Token: 0x060001FD RID: 509 RVA: 0x0000F44C File Offset: 0x0000D64C
	public override void OnExitBegin()
	{
		base.OnExitBegin();
		if (this._isInitialLoad)
		{
			Ting ting = this._loadedWorld.tingRunner.GetTing(this._loadedWorld.settings.avatarName);
			this._loadedWorld.settings.activeRoom = ting.room.name;
			if (this._language != string.Empty)
			{
				this._loadedWorld.settings.translationLanguage = this._language;
				this._loadedWorld.RefreshTranslationLanguage();
			}
		}
		Screen.showCursor = true;
		this._saveCreator.logger.RemoveListener(new D.LogHandler(Debug.Log));
		this._loadedWorld.settings.onCameraTarget = null;
		base.controls.camera.Reset();
	}

	// Token: 0x04000142 RID: 322
	private RelayTwo _relayToLoad;

	// Token: 0x04000143 RID: 323
	private InitialSaveFileCreator _saveCreator;

	// Token: 0x04000144 RID: 324
	private IEnumerator<float> _relayEnumerator;

	// Token: 0x04000145 RID: 325
	private IEnumerator<string> _preloadEnumerator;

	// Token: 0x04000146 RID: 326
	private float _progress;

	// Token: 0x04000147 RID: 327
	private World _loadedWorld;

	// Token: 0x04000148 RID: 328
	private bool _isInitialLoad;

	// Token: 0x04000149 RID: 329
	private string _initDataPath;

	// Token: 0x0400014A RID: 330
	private bool _isDoneLoading;

	// Token: 0x0400014B RID: 331
	private ActionMenu _actionMenu;

	// Token: 0x0400014C RID: 332
	private int _loadStepsPerUpdate;

	// Token: 0x0400014D RID: 333
	private string _language;

	// Token: 0x0400014E RID: 334
	private int _waitABit;

	// Token: 0x0400014F RID: 335
	private bool _hasStartedLoading;

	// Token: 0x04000150 RID: 336
	private Texture _town;

	// Token: 0x04000151 RID: 337
	private Texture _bar;

	// Token: 0x04000152 RID: 338
	private Texture _barFlipped;

	// Token: 0x04000153 RID: 339
	private Texture _border;

	// Token: 0x04000154 RID: 340
	private Texture _frame;
}
