using System;
using GameWorld2;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class PlayerPauseMenu : GameViewState
{
	// Token: 0x06000196 RID: 406 RVA: 0x0000AE94 File Offset: 0x00009094
	public PlayerPauseMenu(PlayerRoamingState pRoamingState, ActionMenu pActionMenu)
	{
		this._roamingState = pRoamingState;
		this._actionMenu = pActionMenu;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000AEAC File Offset: 0x000090AC
	private static MainMenu InstantiatePauseMenuGuiController()
	{
		if (PlayerPauseMenu.s_pauseMenuGuiControllerPrefab == null)
		{
			PlayerPauseMenu.s_pauseMenuGuiControllerPrefab = Resources.Load("PauseMenuGuiController") as GameObject;
		}
		if (PlayerPauseMenu.s_pauseMenuGuiControllerPrefab == null)
		{
			Debug.LogError("Can't find s_pauseMenuGuiControllerPrefab");
		}
		GameObject gameObject = global::UnityEngine.Object.Instantiate(PlayerPauseMenu.s_pauseMenuGuiControllerPrefab) as GameObject;
		if (gameObject == null)
		{
			Debug.LogError("Can't instantiate s_pauseMenuGuiControllerPrefab");
		}
		return gameObject.GetComponent<MainMenu>();
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000AF24 File Offset: 0x00009124
	public override void OnEnterBegin()
	{
		this._actionMenu.FadeOut();
		this._pauseMenuGuiController = PlayerPauseMenu.InstantiatePauseMenuGuiController();
		if (this._pauseMenuGuiController == null)
		{
			Debug.LogError("Can't find PauseMenuGuiController component on _pauseMenuGuiControllerInstance");
		}
		this._pauseMenuGuiController.onResume = new Action(this.OnResume);
		this._pauseMenuGuiController.onLoadGame = new Action<string>(this.OnLoadGame);
		this._pauseMenuGuiController.controls = base.controls;
		this._pauseMenuGuiController.animator.Play("FadeInPauseMenu");
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000AFB8 File Offset: 0x000091B8
	public override GameViewState.RETURN OnEnterLoop()
	{
		this.SetVolumeFromAlpha();
		if (this._actionMenu.alpha > 0.001f)
		{
			return GameViewState.RETURN.RUN_AGAIN;
		}
		Time.timeScale = 0f;
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000AFF0 File Offset: 0x000091F0
	private void OnResume()
	{
		Debug.Log("OnResume()");
		Time.timeScale = 1f;
		base.EndState();
		this._actionMenu.FadeIn();
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000B018 File Offset: 0x00009218
	private void OnLoadGame(string pPath)
	{
		Time.timeScale = 1f;
		Debug.Log("OnLoadGame() in PlayerPauseMenu, path: " + pPath);
		this._roamingState.EndState();
		base.ChangeState(new WorldLoadingState(false, pPath, this._actionMenu, string.Empty));
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000B064 File Offset: 0x00009264
	public override void OnExitBegin()
	{
		MainMenu pauseMenuGuiController = this._pauseMenuGuiController;
		pauseMenuGuiController.onResume = (Action)Delegate.Remove(pauseMenuGuiController.onResume, new Action(this.OnResume));
		MainMenu pauseMenuGuiController2 = this._pauseMenuGuiController;
		pauseMenuGuiController2.onLoadGame = (Action<string>)Delegate.Remove(pauseMenuGuiController2.onLoadGame, new Action<string>(this.OnLoadGame));
		global::UnityEngine.Object.Destroy(this._pauseMenuGuiController.gameObject);
		this._pauseMenuGuiController = null;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000B0D8 File Offset: 0x000092D8
	private void SetVolumeFromAlpha()
	{
		MainMenu.SetVolume(this._actionMenu.alpha);
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000B0EC File Offset: 0x000092EC
	public override void OnUpdate()
	{
		this.SetVolumeFromAlpha();
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this._pauseMenuGuiController.Resume();
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x0600019F RID: 415 RVA: 0x0000B10C File Offset: 0x0000930C
	private Character avatar
	{
		get
		{
			return base.controls.avatar as Character;
		}
	}

	// Token: 0x04000105 RID: 261
	private const float TARGET_ALPHA = 0.6f;

	// Token: 0x04000106 RID: 262
	private static GameObject s_pauseMenuGuiControllerPrefab;

	// Token: 0x04000107 RID: 263
	private PlayerRoamingState _roamingState;

	// Token: 0x04000108 RID: 264
	private ActionMenu _actionMenu;

	// Token: 0x04000109 RID: 265
	private MainMenu _pauseMenuGuiController;
}
