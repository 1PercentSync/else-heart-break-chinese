using System;
using GameTypes;
using GameWorld2;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class CodeEditorState : GameViewState
{
	// Token: 0x06000107 RID: 263 RVA: 0x000065D8 File Offset: 0x000047D8
	public CodeEditorState(Program[] pProgramsToEdit, Character pActionOwner, ActionMenu pActionMenu, bool pAnimateBackButton, BackButton pBackButton)
	{
		this._programsToEdit = pProgramsToEdit;
		D.isNull(pActionOwner, "Must Set Character who uses the hack device!");
		this._actionMenu = pActionMenu;
		this._animateBackButton = pAnimateBackButton;
		if (pBackButton == null)
		{
			this._backButton = new BackButton(this._easyAnimate);
		}
		else
		{
			this._backButton = pBackButton;
		}
		this._backButton.text = "Compile and close [ESC]";
		this._skin = (GUISkin)Resources.Load("ActionMenuStyle");
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00006674 File Offset: 0x00004874
	public override void OnEnterBegin()
	{
		if (this._programsToEdit == null || this._programsToEdit.Length <= 0)
		{
			if (CodeEditorState._demoProgram == null)
			{
				CodeEditorState._demoProgram = base.controls.world.programRunner.CreateProgram("DemoProgram", string.Empty, "HackdevDemoProgram");
			}
			this._programsToEdit = new Program[] { CodeEditorState._demoProgram };
		}
		if (this._programsToEdit[0] != CodeEditorState._lastEditedProgram)
		{
			CodeEditorState._lastEditedProgramPosition = 0;
		}
		CodeEditorState._lastEditedProgram = this._programsToEdit[0];
		this._root = base.controls.camera.transform.FindChild("Hackdevice");
		this._body = this._root.FindChild("Body");
		this._pivot = this._body.FindChild("Pivot");
		this._mainScreen = this._pivot.FindChild("MainScreen").GetComponent<TerminalRenderer>();
		this._bottomScreen = this._pivot.FindChild("Suggestionbar").GetComponent<TerminalRenderer>();
		if (this._root == null || this._body == null || this._pivot == null || this._mainScreen == null || this._bottomScreen == null)
		{
			throw new Exception("hackdevice transform can't be null!");
		}
		this._hackdevButtonListener = this._pivot.GetComponent<HackdevButtonListener>();
		HackdevButtonListener hackdevButtonListener = this._hackdevButtonListener;
		hackdevButtonListener.onHackdevButtonClicked = (Action<int>)Delegate.Combine(hackdevButtonListener.onHackdevButtonClicked, new Action<int>(this.OnHackdevButtonClicked));
		this._root.gameObject.SetActive(true);
		this._body.localPosition = Vector3.zero;
		SoundDictionary.LoadSingleSound("TakeOutHackdev", "Take out open hacking device sound 1");
		SoundDictionary.LoadSingleSound("CompileSuccess", "CompileSucces sound 1");
		SoundDictionary.LoadSingleSound("CompileError", "CompileError sound 1");
		this._audio = this._root.GetComponentInChildren<AudioSource>();
		SoundDictionary.PlaySound("TakeOutHackdev", this._audio);
		this._easyAnimate.Register(this._body, "bodyrot", new EasyAnimState<Vector3>(0.5f, new Vector3(45f, 0f, 0f), Vector3.zero, new EasyAnimState<Vector3>.InterpolationSampler(iTween.vector3easeInOutSine.Sample), delegate(Vector3 o)
		{
			this._body.localEulerAngles = o;
		}, new Function(this.ReleaseSpringsWhenDone)));
		this._easyAnimate.Register(this._body, "bodyshake", new EasyAnimState<Vector3>(3f, new Vector3(0f, 0f, 100f), Vector3.zero, new EasyAnimState<Vector3>.InterpolationSampler(iTween.vector3spring.Sample), delegate(Vector3 o)
		{
			this._body.localPosition = o;
		}));
		this._easyAnimate.Register(this._pivot, "rotate", new EasyAnimState<Vector3>(0.7f, new Vector3(90f, 0f, 0f), Vector3.zero, new EasyAnimState<Vector3>.InterpolationSampler(iTween.vector3easeInOutSine.Sample), delegate(Vector3 o)
		{
			this._pivot.localEulerAngles = o;
		}));
		EasyAnimState<float> easyAnimState = new EasyAnimState<float>(0.3f);
		easyAnimState.Then(new EasyAnimState<float>(0.25f, 0f, 1f, new EasyAnimState<float>.InterpolationSampler(iTween.linear), delegate(float ratio)
		{
			float num = 0.01627451f;
			float num2 = Mathf.Sin(31.415928f * (1f - Mathf.Sqrt(ratio)));
			this._mainScreen.renderer.material.SetFloat("_BumpAmt", ratio * ratio * num + (1f - ratio) * num2);
			if (ratio < 0.3f && !this._mainScreen.GetComponent<UVScrambler>().enabled)
			{
				this._mainScreen.GetComponent<UVScrambler>().enabled = true;
			}
		}));
		if (this._animateBackButton)
		{
			this._backButton.Show();
		}
		this._actionMenu.FadeOut();
		this.CreateLineSweep();
		this._suggestionBar = new CodeEditorSuggestionMaker(this._bottomScreen);
		this._codeEditor = new CodeEditor(this._mainScreen, this._programsToEdit[this._programNr], new CodeEditor.AutoCompleteProvider(this._suggestionBar.TryGetAutoComplete), base.controls.world.sourceCodeDispenser);
		this._codeEditor.stringEditor.SetCursor(CodeEditorState._lastEditedProgramPosition);
		this._codeEditor.OnTextChanged += this._suggestionBar.Update;
		this._codeEditor.ForceTextChangedEvent();
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00006A8C File Offset: 0x00004C8C
	private void ReleaseSpringsWhenDone()
	{
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00006A90 File Offset: 0x00004C90
	public override GameViewState.RETURN OnEnterLoop()
	{
		if (!this._easyAnimate.IsAnimating(this._body, "bodyrot"))
		{
			return GameViewState.RETURN.FINISHED;
		}
		this.UpdateInput();
		return GameViewState.RETURN.RUN_AGAIN;
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00006AC4 File Offset: 0x00004CC4
	private void OnSaveButtonPressed(TerminalWidget pWidget)
	{
		this.CompileCommandActivated();
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00006ACC File Offset: 0x00004CCC
	private void OnNextFileButtonPressed(TerminalWidget pWidget)
	{
		this.NextFile();
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00006AD4 File Offset: 0x00004CD4
	private void OnQuitButtonPressed(TerminalWidget pWidget)
	{
		this._codeEditor.CompileAndSave();
		base.EndState();
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00006AE8 File Offset: 0x00004CE8
	public override void OnUpdate()
	{
		base.controls.depthOfField.focalTransform = this._pivot;
		this.UpdateInput();
		this.UpdatePivotDistance();
		if (base.controls.avatar == null)
		{
			Debug.Log("controls.avatar is null");
			return;
		}
		if (base.controls.avatar.actionName != "Hacking" && base.controls.avatar.actionName != "HackingZoomedIn" && base.controls.avatar.actionName != "UsingComputer")
		{
			Debug.Log("Avatar action = '" + base.controls.avatar.actionName + "' - will end code editor state");
			base.EndState();
		}
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00006BBC File Offset: 0x00004DBC
	private void UpdatePivotDistance()
	{
		this._pivot.transform.localPosition = new Vector3(this._pivot.transform.localPosition.x, this._pivot.transform.localPosition.y, this._pivotDistance);
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00006C14 File Offset: 0x00004E14
	private void AnimateScreenWobble()
	{
		if (!this._easyAnimate.IsAnimating(this._mainScreen, "ScreenWobble"))
		{
			this._mainScreen.renderer.material.SetFloat("_BumpAmt", 0.01627451f + Mathf.Sin(Time.time * 12f) * 0.002f);
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00006C74 File Offset: 0x00004E74
	private void NextFile()
	{
		this._programNr++;
		if (this._programNr > this._programsToEdit.Length - 1)
		{
			this._programNr = 0;
		}
		this._codeEditor.SetProgram(this._programsToEdit[this._programNr]);
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00006CC4 File Offset: 0x00004EC4
	private void CompileAndSave()
	{
		this._codeEditor.CompileAndSave();
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00006CD4 File Offset: 0x00004ED4
	private void CompileCommandActivated()
	{
		this.CompileAndSave();
		this.CreateLineSweep();
		this.PlayCompileSound();
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00006CE8 File Offset: 0x00004EE8
	private void PlayCompileSound()
	{
		if (this.currentProgram.ContainsErrors())
		{
			SoundDictionary.PlaySound("CompileError", this._audio);
		}
		else
		{
			SoundDictionary.PlaySound("CompileSuccess", this._audio);
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000116 RID: 278 RVA: 0x00006D20 File Offset: 0x00004F20
	private Program currentProgram
	{
		get
		{
			return this._programsToEdit[this._programNr];
		}
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00006D30 File Offset: 0x00004F30
	private void UpdateInput()
	{
		if (KeyboardInput.KEY_PRESET_Quit())
		{
			this._codeEditor.CompileAndSave();
			base.EndState();
		}
		else if (KeyboardInput.KEY_PRESET_Compile())
		{
			this.CompileCommandActivated();
		}
		else
		{
			this._codeEditor.DoInput();
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] array = Physics.RaycastAll(ray);
		foreach (RaycastHit raycastHit in array)
		{
			if (raycastHit.transform.name == "MainScreenPlane")
			{
			}
		}
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00006DDC File Offset: 0x00004FDC
	public override void OnExitBegin()
	{
		CodeEditorState._lastEditedProgramPosition = this._codeEditor.stringEditor.cursor;
		HackdevButtonListener hackdevButtonListener = this._hackdevButtonListener;
		hackdevButtonListener.onHackdevButtonClicked = (Action<int>)Delegate.Remove(hackdevButtonListener.onHackdevButtonClicked, new Action<int>(this.OnHackdevButtonClicked));
		this._easyAnimate.Register(this._body, "bodyrot", new EasyAnimState<Vector3>(0.5f, Vector3.zero, new Vector3(45f, 0f, 0f), new EasyAnimState<Vector3>.InterpolationSampler(iTween.vector3easeInOutSine.Sample), delegate(Vector3 o)
		{
			this._body.localEulerAngles = o;
		}));
		if (this._animateBackButton)
		{
			this._backButton.Hide();
		}
		this._actionMenu.FadeIn();
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00006E9C File Offset: 0x0000509C
	public override GameViewState.RETURN OnExitLoop()
	{
		if (!this._easyAnimate.IsAnimating(this._body, "bodyrot"))
		{
			this._root.gameObject.SetActive(false);
			this._backButton.ResetText();
			Character character = base.controls.avatar as Character;
			if (base.controls.avatar.actionName == "Hacking")
			{
				if (character != null && (!character.actionOtherObject.canBePickedUp || character.actionOtherObject.isBeingHeld) && character.actionOtherObject != null && !(character.actionOtherObject is CreditCard) && !(character.actionOtherObject is FuseBox) && !(character.actionOtherObject is Door) && !(character.actionOtherObject is Drink) && !(character.actionOtherObject is Teleporter) && !(character.actionOtherObject is Drug))
				{
					base.controls.avatar.InteractWith(base.controls.avatar.actionOtherObject);
				}
				else
				{
					base.controls.avatar.StopAction();
				}
			}
			return GameViewState.RETURN.FINISHED;
		}
		return GameViewState.RETURN.RUN_AGAIN;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00006FDC File Offset: 0x000051DC
	private void CreateLineSweep()
	{
		EasyAnimState<float> easyAnimState = new EasyAnimState<float>(0.4f, 0f, 0.5f, new EasyAnimState<float>.InterpolationSampler(iTween.linear), new EasyAnimState<float>.SetValue(this.SetHilightedLine), delegate
		{
			this.SetHilightedLine(-1f);
		});
		easyAnimState.Then(new EasyAnimState<float>(1f, 0f, 1f, new EasyAnimState<float>.InterpolationSampler(iTween.linear), new EasyAnimState<float>.SetValue(this.DrawProgramStatusSplash)));
		this._easyAnimate.Register(this, "lineSweep", easyAnimState);
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00007068 File Offset: 0x00005268
	private void SetHilightedLine(float pWhere)
	{
		int num = Mathf.RoundToInt((float)this._mainScreen.TextRowCount * pWhere);
		this._mainScreen.ToggleInvertColors(0, 0, this._mainScreen.TextCollumCount, this._mainScreen.TextRowCount, false);
		if (num != -1)
		{
			this._mainScreen.ToggleInvertColors(0, num, this._mainScreen.TextCollumCount, 1, true);
			this._mainScreen.ToggleInvertColors(0, this._mainScreen.TextRowCount - (num + 1), this._mainScreen.TextCollumCount, 1, true);
		}
		this._mainScreen.ApplyTextChanges();
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00007104 File Offset: 0x00005304
	private void DrawProgramStatusSplash(float pInterpolation)
	{
		this._mainScreen.ToggleInvertColors(0, 0, this._mainScreen.TextCollumCount, this._mainScreen.TextRowCount, false);
		if (this._programsToEdit[0].GetErrors().Length > 0)
		{
			this._mainScreen.UseColor(CodeEditor.COLOR_ERROR);
			this._mainScreen.UseInvert(false);
			this._mainScreen.SetLine(14, "<3 <3 <3             OH NO, WHAT WENT WRONG?            <3 <3 <3");
			this._mainScreen.UseInvert(true);
			this._mainScreen.SetLine(15, "1                                                              8");
			this._mainScreen.SetLine(16, "---------- ERROR! ---------- ERROR! ---------- ERROR! ----------");
		}
		else
		{
			this._mainScreen.UseColor(CodeEditor.COLOR_NUMBER);
			this._mainScreen.UseInvert(false);
			this._mainScreen.SetLine(14, "0                                                               ");
			this._mainScreen.SetLine(15, "- - - - - - - - - - - - - - SUCCESS! - - - - - - - - - - - - - -");
			this._mainScreen.SetLine(16, "                                                              64");
		}
		this._mainScreen.ApplyTextChanges();
	}

	// Token: 0x0600011D RID: 285 RVA: 0x0000720C File Offset: 0x0000540C
	private void OnHackdevButtonClicked(int pX)
	{
		if (pX == 0)
		{
			this.CompileCommandActivated();
		}
		else if (pX == 1)
		{
			this._codeEditor.stringEditor.PerformUndo();
		}
		else if (pX == 2)
		{
			this._codeEditor.ResetToOriginalSourceCode();
		}
		else
		{
			Debug.Log("Can't understand button nr " + pX);
		}
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00007274 File Offset: 0x00005474
	public override void OnGUI()
	{
		base.OnGUI();
		GUI.skin = this._skin;
		GUI.color = Color.white;
		if (this._backButton.RenderAndMaybeGoBack())
		{
			this._codeEditor.CompileAndSave();
			base.EndState();
		}
	}

	// Token: 0x0400009C RID: 156
	private const float ORIGINAL_BUMP_AMNT = 0.01627451f;

	// Token: 0x0400009D RID: 157
	private static Program _lastEditedProgram;

	// Token: 0x0400009E RID: 158
	private static int _lastEditedProgramPosition;

	// Token: 0x0400009F RID: 159
	private Transform _root;

	// Token: 0x040000A0 RID: 160
	private Transform _body;

	// Token: 0x040000A1 RID: 161
	private Transform _pivot;

	// Token: 0x040000A2 RID: 162
	private CodeEditor _codeEditor;

	// Token: 0x040000A3 RID: 163
	private CodeEditorSuggestionMaker _suggestionBar;

	// Token: 0x040000A4 RID: 164
	private TerminalRenderer _mainScreen;

	// Token: 0x040000A5 RID: 165
	private TerminalRenderer _bottomScreen;

	// Token: 0x040000A6 RID: 166
	private Program[] _programsToEdit = new Program[0];

	// Token: 0x040000A7 RID: 167
	private static Program _demoProgram;

	// Token: 0x040000A8 RID: 168
	private int _programNr;

	// Token: 0x040000A9 RID: 169
	private HackdevButtonListener _hackdevButtonListener;

	// Token: 0x040000AA RID: 170
	private ActionMenu _actionMenu;

	// Token: 0x040000AB RID: 171
	private AudioSource _audio;

	// Token: 0x040000AC RID: 172
	private bool _animateBackButton;

	// Token: 0x040000AD RID: 173
	private BackButton _backButton;

	// Token: 0x040000AE RID: 174
	private GUISkin _skin;

	// Token: 0x040000AF RID: 175
	private float _pivotDistance = 4.8f;
}
