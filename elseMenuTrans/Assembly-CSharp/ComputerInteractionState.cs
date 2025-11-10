using System;
using GameTypes;
using GameWorld2;
using TingTing;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class ComputerInteractionState : GameViewState
{
	// Token: 0x06000125 RID: 293 RVA: 0x00007398 File Offset: 0x00005598
	public ComputerInteractionState(Transform pCameraPoint, Transform pTarget, GreatCamera pCameraScript, Computer pComputer, Character pCharacter, TerminalRenderer pTerminalRenderer, ActionMenu pActionMenu)
	{
		this._camera = pCameraScript;
		this._camera.EnterFixedCamera(pCameraPoint, pTarget);
		this._computer = pComputer;
		this._character = pCharacter;
		this._terminalRenderer = pTerminalRenderer;
		Shell shellWithName = ShellManager.GetShellWithName(pCharacter.name);
		if (shellWithName.audio != null)
		{
			SoundDictionary.LoadSingleSound("ZoomComputer", "ZoomComputerSound 0");
			SoundDictionary.PlaySound("ZoomComputer", shellWithName.audio);
			Debug.Log("Playing zoom computer sound");
		}
		else
		{
			Debug.Log("No audio component on " + shellWithName.name + " to play zoom computer sound");
		}
		if (this._terminalRenderer == null)
		{
			Debug.Log("No terminal renderer on the target computer");
		}
		this._actionMenu = pActionMenu;
		this._actionMenu.FadeOut();
		this._backButton = new BackButton(this._easyAnimate);
		this._computerInteractionMenu = RunGameWorld.instance.transform.Find("ComputerInteractionMenu").GetComponent<ActionMenu>();
		if (this._computerInteractionMenu == null)
		{
			Debug.Log("_computerInteractionMenu is null");
		}
		this._computerInteractionMenu.onActionMenuPressed = new ActionMenu.OnActionMenuButtonPressed(this.OnActionMenuPressed);
		this._restartComputerButton = new ActionMenuItem
		{
			text = "Restart [ctrl + r]",
			identifier = "Restart"
		};
		this._hackButton = new ActionMenuItem
		{
			text = "Hack [ctrl + h]",
			identifier = "Hack"
		};
		ActionMenu computerInteractionMenu = this._computerInteractionMenu;
		ActionMenuItem[] array2;
		if (pCharacter.hasHackdev)
		{
			ActionMenuItem[] array = new ActionMenuItem[2];
			array[0] = this._restartComputerButton;
			array2 = array;
			array[1] = this._hackButton;
		}
		else
		{
			(array2 = new ActionMenuItem[1])[0] = this._restartComputerButton;
		}
		computerInteractionMenu.items = array2;
		this._skin = (GUISkin)Resources.Load("ActionMenuStyle");
		if (this._skin == null)
		{
			Debug.LogError("Failed to load skin");
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000126 RID: 294 RVA: 0x00007588 File Offset: 0x00005788
	// (set) Token: 0x06000127 RID: 295 RVA: 0x00007590 File Offset: 0x00005790
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

	// Token: 0x06000128 RID: 296 RVA: 0x00007598 File Offset: 0x00005798
	public override void OnEnterBegin()
	{
		Character avatar = this.avatar;
		avatar.onNewAction = (Ting.OnNewAction)Delegate.Combine(avatar.onNewAction, new Ting.OnNewAction(this.OnAvatarChangesAction));
		this._backButton.Show();
		this._computerInteractionMenu.FadeIn();
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000075D8 File Offset: 0x000057D8
	private void SetAvatarVisible(bool pValue)
	{
		if (this.avatarShell != null)
		{
			foreach (Renderer renderer in this.avatarShell.GetComponentsInChildren<Renderer>())
			{
				renderer.enabled = pValue;
			}
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x0600012A RID: 298 RVA: 0x00007624 File Offset: 0x00005824
	private CharacterShell avatarShell
	{
		get
		{
			if (this.avatar == null)
			{
				return null;
			}
			return ShellManager.GetShellWithName(this.avatar.name) as CharacterShell;
		}
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00007654 File Offset: 0x00005854
	public override GameViewState.RETURN OnEnterLoop()
	{
		if (!this._camera.isInFixedMode)
		{
			return GameViewState.RETURN.RUN_AGAIN;
		}
		this.SetAvatarVisible(false);
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00007670 File Offset: 0x00005870
	private void OnAvatarChangesAction(string pPreviousAction, string pNewAction)
	{
		if (pPreviousAction != "InsideComputer" && pNewAction == "InsideComputer")
		{
			Debug.Log("Going from ComputerInteractionState to InsideComputerState");
			this._backButton.Hide();
			this._backButton = null;
			this._computerInteractionMenu.Hide();
			base.PushState(new InsideComputerState(this.avatar.actionOtherObject.name, this._actionMenu));
		}
		if (pPreviousAction == "InsideComputer" && pNewAction != "InsideComputer")
		{
			base.EndState();
		}
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0000770C File Offset: 0x0000590C
	public override void OnUpdate()
	{
		if (this._terminalRenderer != null)
		{
			base.controls.depthOfField.focalTransform = this._terminalRenderer.transform;
		}
		if (base.IsTopState())
		{
			this.UpdateInput();
		}
		if (this._pushedHackState && base.IsTopState())
		{
			Debug.Log("Came back from hack state, will run program on computer!");
			this.RunProgramAgain();
			this._pushedHackState = false;
		}
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00007784 File Offset: 0x00005984
	private void OnActionMenuPressed(string pIdentifier)
	{
		if (pIdentifier == "Restart")
		{
			this.RunProgramAgain();
		}
		else if (pIdentifier == "Hack")
		{
			this.Hack();
		}
		else
		{
			Debug.Log("Unrecognized identifier: " + pIdentifier);
		}
	}

	// Token: 0x0600012F RID: 303 RVA: 0x000077D8 File Offset: 0x000059D8
	private void RunProgramAgain()
	{
		Floppy floppy = null;
		if (this.avatar != null)
		{
			floppy = this.avatar.handItem as Floppy;
		}
		this._computer.masterProgram.ClearRuntimeErrors();
		this._computer.masterProgram.Compile();
		this._computer.NextLine();
		this._computer.RunProgram(floppy);
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000783C File Offset: 0x00005A3C
	private void Hack()
	{
		if (this._computer == null)
		{
			D.Log("Hackable ting of " + this.avatar.name + " was null!");
			return;
		}
		if (this.avatar == null)
		{
			D.Log("Avatar is null!");
			return;
		}
		Debug.Log("Going to hack " + this._computer.name + " zoomed in");
		this._computer.PrepareForBeingHacked();
		if (this._computer.programs.Length > 0)
		{
			MockProgram mockProgram = new MockProgram(delegate(object retVal)
			{
				if (retVal.GetType() == typeof(bool) && (bool)retVal)
				{
					this.avatar.StartAction("HackingZoomedIn", this._computer, 99999f, 99999f);
					base.controls.world.dialogueRunner.EventHappened(this.avatar.name + "_hack_" + this._computer.name);
					base.PushState(new CodeEditorState(this._computer.programs, this._character, this._computerInteractionMenu, false, this._backButton));
					this._pushedHackState = true;
				}
				else
				{
					D.Log("Hacking not allowed with current device for character " + this.avatar.name);
					base.controls.world.settings.Notify(this.avatar.name, "Not allowed with current device");
					this.avatar.StopAction();
				}
			});
			this.avatar.StartAction("AttemptHacking", this._computer, 1f, 1f);
			this._computer.PrepareForBeingHacked();
			this.avatar.hackdev.PrepareForBeingHacked();
			if (this.avatar.hackdev.masterProgram.HasFunction("Allow", true))
			{
				this.avatar.hackdev.masterProgram.StartAtFunctionWithMockReceiver("Allow", new object[]
				{
					this._computer.name,
					(float)this._computer.securityLevel
				}, mockProgram);
			}
			else
			{
				base.controls.world.settings.Notify(this.avatar.name, "No Allow-function in " + this.avatar.hackdev.name);
			}
		}
		else
		{
			base.controls.world.settings.Notify(this.avatar.name, "No programs to hack in " + this._computer.name);
			this.avatar.StopAction();
		}
	}

	// Token: 0x06000131 RID: 305 RVA: 0x000079FC File Offset: 0x00005BFC
	private void UpdateInput()
	{
		if (KeyboardInput.KEY_PRESET_Quit() || Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			if (this.avatar != null && this.avatar.actionName == "SlurpingIntoComputer")
			{
				Debug.Log("Can't cancel SlurpingIntoComputer");
			}
			else if (base.IsTopState())
			{
				base.EndState();
			}
			else
			{
				Debug.Log("Can't quit ComputerInteractionState, isn't top state.");
			}
		}
		else if ((Input.GetMouseButtonDown(1) || KeyboardInput.KEY_PRESET_Hack()) && this.avatar.hasHackdev)
		{
			this.Hack();
		}
		else if (KeyboardInput.KEY_PRESET_Compile())
		{
			this.RunProgramAgain();
		}
		else if (KeyboardInput.KEY_PRESET_Enter())
		{
			this._computer.OnEnterKey();
		}
		else if (KeyboardInput.KEY_PRESET_Backspace())
		{
			this._computer.OnBackspaceKey();
		}
		else if (KeyboardInput.KEY_PRESET_LeftArrow())
		{
			this._computer.OnDirectionKey("left");
		}
		else if (KeyboardInput.KEY_PRESET_RightArrow())
		{
			this._computer.OnDirectionKey("right");
		}
		else if (KeyboardInput.KEY_PRESET_UpArrow())
		{
			this._computer.OnDirectionKey("up");
		}
		else if (KeyboardInput.KEY_PRESET_DownArrow())
		{
			this._computer.OnDirectionKey("down");
		}
		else if (KeyboardInput.KEY_PRESET_Paste())
		{
			foreach (char c in Input.inputString)
			{
				this._computer.OnKeyDown(c.ToString());
			}
		}
		else if (Input.inputString != string.Empty)
		{
			this._computer.OnKeyDown(Input.inputString);
		}
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00007BE0 File Offset: 0x00005DE0
	public override void OnExitBegin()
	{
		this.SetAvatarVisible(true);
		Character avatar = this.avatar;
		avatar.onNewAction = (Ting.OnNewAction)Delegate.Remove(avatar.onNewAction, new Ting.OnNewAction(this.OnAvatarChangesAction));
		this.avatar.StopAction();
		this._camera.ExitFixedCamera();
		this._actionMenu.FadeIn();
		if (this._backButton != null)
		{
			this._backButton.Hide();
		}
		this._computerInteractionMenu.FadeOut();
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00007C60 File Offset: 0x00005E60
	public override void OnGUI()
	{
		base.OnGUI();
		GUI.skin = this._skin;
		GUI.color = Color.white;
		if (this._backButton != null && this._backButton.RenderAndMaybeGoBack())
		{
			base.EndTopState();
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00007CAC File Offset: 0x00005EAC
	public override GameViewState.RETURN OnExitLoop()
	{
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000135 RID: 309 RVA: 0x00007CB0 File Offset: 0x00005EB0
	private Character avatar
	{
		get
		{
			D.isNull(base.controls, "Controls is null :S");
			return base.controls.avatar as Character;
		}
	}

	// Token: 0x040000B0 RID: 176
	private GreatCamera _camera;

	// Token: 0x040000B1 RID: 177
	private Computer _computer;

	// Token: 0x040000B2 RID: 178
	private Character _character;

	// Token: 0x040000B3 RID: 179
	private TerminalRenderer _terminalRenderer;

	// Token: 0x040000B4 RID: 180
	private ActionMenu _actionMenu;

	// Token: 0x040000B5 RID: 181
	private ActionMenu _computerInteractionMenu;

	// Token: 0x040000B6 RID: 182
	private BackButton _backButton;

	// Token: 0x040000B7 RID: 183
	private ActionMenuItem _restartComputerButton;

	// Token: 0x040000B8 RID: 184
	private ActionMenuItem _hackButton;

	// Token: 0x040000B9 RID: 185
	private GUISkin _skin;

	// Token: 0x040000BA RID: 186
	private bool _pushedHackState;
}
