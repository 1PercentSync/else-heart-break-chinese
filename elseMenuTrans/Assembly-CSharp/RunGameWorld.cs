using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using GameTypes;
using GameWorld2;
using TingTing;
using UnityEngine;

// Token: 0x02000085 RID: 133
public class RunGameWorld : MonoBehaviour
{
	// Token: 0x060003DB RID: 987 RVA: 0x0001BFF4 File Offset: 0x0001A1F4
	private void Awake()
	{
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		Application.targetFrameRate = 60;
		MainMenu.SetVolume(0f);
		global::UnityEngine.Object @object = Resources.Load("BubbleCanvas");
		GameObject gameObject = global::UnityEngine.Object.Instantiate(@object) as GameObject;
		this._bubbleCanvasController = gameObject.GetComponent<BubbleCanvasController>();
		global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
		base.transform.FindChild("Hackdevice").gameObject.SetActive(false);
		base.transform.FindChild("MapScreenArm").gameObject.SetActive(false);
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		D.onDLog += new D.LogHandler(Debug.Log);
		WorldOwner.instance.UnloadWorld();
		this._commandLine = new CommandLine();
		this._fade = new Fade(Color.black);
		this._roomChanger = new RoomChanger(this._fade);
		this._roomChanger.onRoomHasChanged = new Action<string>(this.OnRoomHasChanged);
		this._sunController = new SunController(this.GetDayCycleSettings());
		this._rainController = new RainController(base.camera, this._roomChanger);
		this._depthOfField = base.GetComponent<DepthOfFieldScatter>();
		this._greatCamera = base.GetComponent<GreatCamera>();
		this._gameViewControls = new GameViewControls(this._fade, this._roomChanger, this._greatCamera, this._commandLine, this._bubbleCanvasController, this._depthOfField);
		this._gameViewStateHandler = new GameViewStateHandler(this._gameViewControls);
		this._actionMenu = base.GetComponent<ActionMenu>();
		if (RunGameWorld.loadThisPath == string.Empty)
		{
			this._gameViewStateHandler.ChangeState(new WorldLoadingState(true, WorldOwner.INIT_DATA_PATH, this._actionMenu, RunGameWorld.loadThisLanguage));
		}
		else
		{
			this._gameViewStateHandler.ChangeState(new WorldLoadingState(false, RunGameWorld.loadThisPath, this._actionMenu, RunGameWorld.loadThisLanguage));
		}
		RunGameWorld.loadThisPath = string.Empty;
		RunGameWorld.loadThisLanguage = string.Empty;
		this._vignetting = base.GetComponent<Vignetting>();
		this._screenOverlay = base.GetComponent<ScreenOverlay>();
		this._notificationsPane = base.GetComponent<NotificationsPane>();
		this._inGameHelpPanel = base.transform.Find("InGameHelpPanel").GetComponent<NotificationsPane>();
		this._cricketsAudio = base.transform.FindChild("Crickets").audio;
		this.SetPostEffects(OptionsPanel.advancedShadersOn);
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0001C24C File Offset: 0x0001A44C
	private void Update()
	{
		if (this._detectTingsEnteringRoom)
		{
			this.DetectTingsEnteringRoom();
		}
		if (!this._gameViewControls.pauseWorld && this.world.isReadyToPlay)
		{
			this.LazySetup();
			KeyboardInput.Update(Time.deltaTime);
			for (int i = 0; i < this.gameViewControls.updatesPerFrame; i++)
			{
				this.world.Update(Time.deltaTime * this._gameViewControls.deltaTimeMultiplier);
			}
			this._fade.Update(Time.deltaTime);
			bool flag = this.IsCurrentRoomExterior();
			this._sunController.UpdateSun(this.world.settings.gameTimeClock.normalizedDayTime, flag);
			this._rainController.Update(flag);
			this._cricketsAudio.enabled = flag;
			RoomToneController.instance.SetRoomType((!flag) ? RoomType.INDOOR : RoomType.OUTDOOR);
		}
		if (this._delayedDetectionOfTingsEnteringRoom)
		{
			this.DetectTingsEnteringRoom();
			this._delayedDetectionOfTingsEnteringRoom = false;
		}
		this.CheatKeys();
		if (this._gameViewStateHandler.currentState == null)
		{
			this._gameViewStateHandler.ChangeState(new PlayerRoamingState(false));
		}
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0001C37C File Offset: 0x0001A57C
	private void LateUpdate()
	{
		if (this._updateState)
		{
			this._gameViewStateHandler.Update();
		}
		if (this.avatarShell != null)
		{
			this._greatCamera.SetOrbitTarget(this.avatarShell.lookTargetPoint);
			this._greatCamera.SetAutoRotate(this.world.settings.cameraAutoRotateSpeed);
		}
		if (this._updateState)
		{
			this._gameViewStateHandler.LatestUpdate();
		}
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0001C3FC File Offset: 0x0001A5FC
	private void CheatKeys()
	{
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F11))
		{
			this.TogglePostEffects();
		}
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F12))
		{
			this.showCommandLineAndInfo = !this.showCommandLineAndInfo;
		}
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0001C45C File Offset: 0x0001A65C
	private void LazySetup()
	{
		if (!this._hasSetupOnTingHasNewRoomListener)
		{
			MimanTingRunner tingRunner = this._gameViewControls.world.tingRunner;
			tingRunner.onTingHasNewRoom = (TingRunner.OnNewRoom)Delegate.Combine(tingRunner.onTingHasNewRoom, new TingRunner.OnNewRoom(this.OnTingChangesRoom));
			this.DetectTingsEnteringRoom();
			this._hasSetupOnTingHasNewRoomListener = true;
		}
		if (this._gameViewControls.world.settings.onNotification == null)
		{
			Debug.Log("Adding notification and hint callbacks in RunGameWorld");
			this._gameViewControls.world.settings.onNotification = delegate(string name, string message)
			{
				if (this.avatarShell != null && name == this.avatarShell.name)
				{
					this._notificationsPane.ShowNotification(message);
				}
			};
			this._gameViewControls.world.settings.onHint = delegate(string message)
			{
				this._inGameHelpPanel.ShowNotification(message);
			};
		}
		if (this._gameViewControls.world.settings.onCopyToClipboard == null)
		{
			WorldSettings settings = this._gameViewControls.world.settings;
			settings.onCopyToClipboard = (WorldSettings.CopyToClipboard)Delegate.Combine(settings.onCopyToClipboard, new WorldSettings.CopyToClipboard(this.CopyToClipboard));
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060003E0 RID: 992 RVA: 0x0001C564 File Offset: 0x0001A764
	// (set) Token: 0x060003E1 RID: 993 RVA: 0x0001C56C File Offset: 0x0001A76C
	public string clipboard
	{
		get
		{
			return ClipboardHelper.clipboard;
		}
		set
		{
			ClipboardHelper.clipboard = value;
		}
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x0001C574 File Offset: 0x0001A774
	private void CopyToClipboard(string text)
	{
		this.clipboard = text;
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x0001C580 File Offset: 0x0001A780
	private void TogglePostEffects()
	{
		this._depthOfField.enabled = !this._depthOfField.enabled;
		this._vignetting.enabled = !this._vignetting.enabled;
		this._screenOverlay.enabled = !this._screenOverlay.enabled;
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x0001C5D8 File Offset: 0x0001A7D8
	public void SetPostEffects(bool pOn)
	{
		Debug.Log("SetPostEffects: " + pOn);
		this._depthOfField.enabled = pOn;
		this._vignetting.enabled = pOn;
		this._screenOverlay.enabled = pOn;
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060003E5 RID: 997 RVA: 0x0001C620 File Offset: 0x0001A820
	public bool isPostEffectsOn
	{
		get
		{
			return this._depthOfField.enabled;
		}
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x0001C630 File Offset: 0x0001A830
	private bool CanSaveRightNow()
	{
		if (this.avatar == null)
		{
			Debug.LogError("Avatar is null!");
			return false;
		}
		return this.avatar.conversationTarget == null && !this.avatar.busy && !(this.avatar.actionName == "Sleeping");
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x0001C698 File Offset: 0x0001A898
	private DayCycleSettings GetDayCycleSettings()
	{
		DayCycleSettings component = base.GetComponent<DayCycleSettings>();
		if (component == null)
		{
			Debug.LogError("Can't find day cycle settings component");
		}
		return component;
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0001C6C4 File Offset: 0x0001A8C4
	private Room activeRoomRef
	{
		get
		{
			return this.world.roomRunner.GetRoomUnsafe(this._roomChanger.currentRoom);
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0001C6EC File Offset: 0x0001A8EC
	private Character avatar
	{
		get
		{
			return this._gameViewControls.avatar as Character;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x060003EA RID: 1002 RVA: 0x0001C700 File Offset: 0x0001A900
	private Shell avatarShell
	{
		get
		{
			if (this._avatarShellCache == null && this._gameViewControls.avatar != null)
			{
				this._avatarShellCache = ShellManager.GetShellWithName(this._gameViewControls.avatar.name);
			}
			return this._avatarShellCache;
		}
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0001C754 File Offset: 0x0001A954
	public void OnLoadedGame(World pNewWorld)
	{
		Debug.Log("OnLoadedGame()");
		this._gameViewControls.world = pNewWorld;
		this._bubbleCanvasController.ClearThoughtBubbles();
		this._avatarShellCache = null;
		this._roomChanger.onRoomHasChanged = new Action<string>(this.OnRoomHasChanged);
		this._hasSetupOnTingHasNewRoomListener = false;
		this._delayedDetectionOfTingsEnteringRoom = true;
		this._gameViewControls.world.dialogueRunner.onGrimmError = new Action<string>(this.OnGrimmError);
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0001C7D0 File Offset: 0x0001A9D0
	private void OnGrimmError(string pMessage)
	{
		this.showCommandLineAndInfo = true;
		this._grimmErrors.Add(pMessage);
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0001C7E8 File Offset: 0x0001A9E8
	public bool IsCurrentRoomExterior()
	{
		return this.activeRoomRef != null && this.activeRoomRef.exterior;
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0001C818 File Offset: 0x0001AA18
	public bool IsRoomExterior(string pRoomName)
	{
		Room roomUnsafe = this.world.roomRunner.GetRoomUnsafe(pRoomName);
		return roomUnsafe != null && roomUnsafe.exterior;
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0001C84C File Offset: 0x0001AA4C
	public void DetectTingsEnteringRoom()
	{
		Debug.LogWarning("DetectTingsEnteringRoom() slow function!");
		foreach (Ting ting in this.world.tingRunner.GetTings())
		{
			this.OnTingChangesRoom(ting, ting.room.name);
		}
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0001C8D0 File Offset: 0x0001AAD0
	private void OnRoomHasChanged(string newRoomName)
	{
		Debug.Log("OnRoomHasChanged was called in RunGameWorld, newRoomName = " + newRoomName);
		this._avatarShellCache = null;
		this._delayedDetectionOfTingsEnteringRoom = true;
		bool flag = this.IsRoomExterior(newRoomName);
		if (MainMenu.autoZoom)
		{
			this._greatCamera.Input_SetZoomDirectly((!flag) ? 1 : 2);
			this._greatCamera.Input_SetTilt((float)((!flag) ? 45 : 30));
		}
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0001C940 File Offset: 0x0001AB40
	private void OnTingChangesRoom(Ting pTing, string pNewRoomName)
	{
		if (pNewRoomName == Application.loadedLevelName)
		{
			Shell shellWithName = ShellManager.GetShellWithName(pTing.name);
			if (shellWithName == null)
			{
				try
				{
					global::UnityEngine.Object @object = Resources.Load(pTing.prefab);
					if (@object == null)
					{
						Debug.LogError("Couldn't find a prefab called '" + pTing.prefab + "'");
						pTing.isDeleted = true;
					}
					else
					{
						global::UnityEngine.Object object2 = global::UnityEngine.Object.Instantiate(@object);
						if (object2 == null)
						{
							Debug.LogError("Failed to instantiate the prefab '" + pTing.prefab + "'");
							pTing.isDeleted = true;
						}
						else
						{
							GameObject gameObject = object2 as GameObject;
							if (gameObject == null)
							{
								Debug.LogError("The loaded prefab was not a game object");
							}
							Shell component = gameObject.GetComponent<Shell>();
							if (component == null)
							{
								Debug.LogError("The new game object didn't have a Shell");
							}
							component.name = pTing.name;
							component.ConnectTing(pTing);
							this.SnapShellToTingPositionIfNotAtCorrectPosAlready(component);
						}
					}
				}
				catch (Exception ex)
				{
					Debug.Log(string.Concat(new string[]
					{
						"Error when loading the prefab '",
						pTing.prefab,
						"' of the ting '",
						pTing.name,
						"' in room ",
						pTing.room.name,
						", exception: ",
						ex.ToString()
					}));
					throw ex;
				}
			}
			else
			{
				this.SnapShellToTingPositionIfNotAtCorrectPosAlready(shellWithName);
			}
		}
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0001CAD8 File Offset: 0x0001ACD8
	private void SnapShellToTingPositionIfNotAtCorrectPosAlready(Shell pShell)
	{
		if (pShell == null)
		{
			D.LogError("pShell is null");
		}
		else if (pShell.ting == null)
		{
			D.Log("pShell.ting of " + pShell.name + " is null");
			return;
		}
		if (pShell.ting.localPoint != MimanHelper.Vector3ToTilePoint(pShell.transform.position))
		{
			pShell.SnapShellToTingPosition();
		}
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0001CB54 File Offset: 0x0001AD54
	private void OnGUI()
	{
		this._fade.Draw();
		this._gameViewStateHandler.DrawGUI();
		if (this.showCommandLineAndInfo)
		{
			this._commandLine.OnGUI();
			this.DrawStateInfo();
			foreach (string text in this._grimmErrors)
			{
				GUILayout.Label(text, new GUILayoutOption[0]);
			}
		}
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0001CBF4 File Offset: 0x0001ADF4
	private void OnDrawGizmos()
	{
		if (this._gameViewStateHandler != null)
		{
			this._gameViewStateHandler.OnGizmos();
		}
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x0001CC0C File Offset: 0x0001AE0C
	private void OnRenderObject()
	{
		this._gameViewStateHandler.OnRenderObject();
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0001CC1C File Offset: 0x0001AE1C
	private void DrawStateInfo()
	{
		GUI.skin = null;
		GUI.color = Color.white;
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (this._gameViewStateHandler.currentState == null)
		{
			GUILayout.Label("No state", new GUILayoutOption[0]);
		}
		else
		{
			GUILayout.Label(string.Concat(new object[]
			{
				" ",
				this.world.settings.gameTimeClock,
				", state: ",
				this._gameViewStateHandler.currentState.ToString(),
				" ,tick: ",
				this.world.settings.tickNr
			}), new GUILayoutOption[0]);
		}
		GUILayout.Label("focusedDialogue: " + this.world.settings.focusedDialogue, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0001CD04 File Offset: 0x0001AF04
	public GameViewState currentGameViewState
	{
		get
		{
			return this._gameViewStateHandler.currentState;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001CD14 File Offset: 0x0001AF14
	public static RunGameWorld instance
	{
		get
		{
			if (RunGameWorld._instance == null)
			{
				RunGameWorld._instance = global::UnityEngine.Object.FindObjectOfType(typeof(RunGameWorld)) as RunGameWorld;
			}
			return RunGameWorld._instance;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0001CD50 File Offset: 0x0001AF50
	// (set) Token: 0x060003FA RID: 1018 RVA: 0x0001CD5C File Offset: 0x0001AF5C
	private World world
	{
		get
		{
			return WorldOwner.instance.world;
		}
		set
		{
			WorldOwner.instance.world = value;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x0001CD6C File Offset: 0x0001AF6C
	public GameViewControls gameViewControls
	{
		get
		{
			return this._gameViewControls;
		}
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0001CD74 File Offset: 0x0001AF74
	public void ShowNotification(string pMessage)
	{
		this._notificationsPane.ShowNotification(pMessage);
	}

	// Token: 0x040002FD RID: 765
	private static RunGameWorld _instance;

	// Token: 0x040002FE RID: 766
	private GameViewStateHandler _gameViewStateHandler;

	// Token: 0x040002FF RID: 767
	private GameViewControls _gameViewControls;

	// Token: 0x04000300 RID: 768
	private CommandLine _commandLine;

	// Token: 0x04000301 RID: 769
	private RoomChanger _roomChanger;

	// Token: 0x04000302 RID: 770
	private Fade _fade;

	// Token: 0x04000303 RID: 771
	private SunController _sunController;

	// Token: 0x04000304 RID: 772
	private RainController _rainController;

	// Token: 0x04000305 RID: 773
	private DepthOfFieldScatter _depthOfField;

	// Token: 0x04000306 RID: 774
	private Vignetting _vignetting;

	// Token: 0x04000307 RID: 775
	private ScreenOverlay _screenOverlay;

	// Token: 0x04000308 RID: 776
	private BubbleCanvasController _bubbleCanvasController;

	// Token: 0x04000309 RID: 777
	private bool _delayedDetectionOfTingsEnteringRoom;

	// Token: 0x0400030A RID: 778
	private Shell _avatarShellCache;

	// Token: 0x0400030B RID: 779
	private NotificationsPane _notificationsPane;

	// Token: 0x0400030C RID: 780
	private NotificationsPane _inGameHelpPanel;

	// Token: 0x0400030D RID: 781
	private AudioSource _cricketsAudio;

	// Token: 0x0400030E RID: 782
	private ActionMenu _actionMenu;

	// Token: 0x0400030F RID: 783
	private GreatCamera _greatCamera;

	// Token: 0x04000310 RID: 784
	private List<string> _grimmErrors = new List<string>();

	// Token: 0x04000311 RID: 785
	public bool _updateState = true;

	// Token: 0x04000312 RID: 786
	public bool showCommandLineAndInfo;

	// Token: 0x04000313 RID: 787
	public bool _detectTingsEnteringRoom;

	// Token: 0x04000314 RID: 788
	private bool _hasSetupOnTingHasNewRoomListener;

	// Token: 0x04000315 RID: 789
	public static string loadThisPath = string.Empty;

	// Token: 0x04000316 RID: 790
	public static string loadThisLanguage = string.Empty;
}
