using System;
using System.Collections.Generic;
using GameTypes;
using GameWorld2;
using GrimmLib;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class DialogueSubstate
{
	// Token: 0x06000137 RID: 311 RVA: 0x00007DE4 File Offset: 0x00005FE4
	public DialogueSubstate(GameViewControls pControls, string pConversationName, DialogueSubstate.IsShowingInventory pIsShowingInventory)
	{
		pControls.world.settings.focusedDialogue = pConversationName;
		this._isShowingInventory = pIsShowingInventory;
		this._controls = pControls;
		this._skin = (GUISkin)Resources.Load("DefaultSkin");
		this._conversationName = pConversationName;
		this._clickHereHelpArrow = Resources.Load("ClickHereHelpArrow_NOSCALE") as Texture;
		Debug.Log("Will not show cheats!");
		this._showCheatingDialogueOptions = false;
		this._controls.world.grimmApiDefinitions.onRemoveDanglingDialogueOptions = new Action<string>(this.RemoveDanglingDialogueOptions);
		this._controls.world.dialogueRunner.AddOnEventListener(new DialogueRunner.OnEvent(this.OnDialogueRunnerEvent));
		this._controls.world.dialogueRunner.AddFocusConversationListener(new DialogueRunner.OnFocusConversation(this.OnConversationFocus));
		this._controls.world.dialogueRunner.AddDefocusConversationListener(new DialogueRunner.OnFocusConversation(this.OnConversationDefocus));
		this.previousZoom = (int)this._controls.camera.zoom;
		this.previousTilt = this._controls.camera.tilt;
		this._controls.camera.Input_ZoomDiscrete(2);
		this._controls.camera.Input_SetTilt(70f);
		Time.timeScale = 1f;
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000138 RID: 312 RVA: 0x00007F6C File Offset: 0x0000616C
	public bool isFocused
	{
		get
		{
			return this._conversationName != string.Empty;
		}
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00007F80 File Offset: 0x00006180
	public void RemoveDanglingDialogueOptions(string pConversationName)
	{
		this.RemoveAllOptionsWithFade();
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00007F88 File Offset: 0x00006188
	public void OnExitDialogue()
	{
		Debug.LogWarning("OnExitDialogue()");
		this._controls.world.dialogueRunner.RemoveOnEventListener(new DialogueRunner.OnEvent(this.OnDialogueRunnerEvent));
		this._controls.world.dialogueRunner.RemoveFocusConversationListener(new DialogueRunner.OnFocusConversation(this.OnConversationDefocus));
		this._controls.world.dialogueRunner.RemoveDefocusConversationListener(new DialogueRunner.OnFocusConversation(this.OnConversationDefocus));
		this._controls.world.dialogueRunner.RemoveDefocusConversationListener(new DialogueRunner.OnFocusConversation(this.OnConversationDefocus));
		this._controls.bubbleManager.ClearThoughtBubbles();
		this._controls.camera.Input_SetZoomDirectly(this.previousZoom);
		this._controls.camera.Input_SetTilt(this.previousTilt);
		this._controls.world.settings.focusedDialogue = string.Empty;
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000807C File Offset: 0x0000627C
	public void OnConversationFocus(string pConversation)
	{
		this._conversationName = pConversation;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00008088 File Offset: 0x00006288
	public void OnConversationDefocus(string pConversation)
	{
		if (this._conversationName == pConversation)
		{
			this._controls.world.settings.focusedDialogue = string.Empty;
			this.exit = true;
		}
		else
		{
			Debug.Log("Got DEFOCUS message but is not focused on '" + pConversation + "', so will not end dialogue state");
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x0600013D RID: 317 RVA: 0x000080E4 File Offset: 0x000062E4
	private DialogueRunner dialogueRunner
	{
		get
		{
			return this._controls.world.dialogueRunner;
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x000080F8 File Offset: 0x000062F8
	private void RemoveAllOptionsWithFade()
	{
		Debug.Log("Will remove all dialogue options (using a fade)");
		this._controls.bubbleManager.ClearThoughtBubbles();
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00008114 File Offset: 0x00006314
	public void OnUpdate()
	{
		this.ConversationUpdate();
		this._easyAnimator.Update(Time.deltaTime);
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
		{
			this.dialogueRunner.FastForwardCurrentTimedDialogueNode(this._conversationName);
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.SelectBubbleWithKeyboard(0);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.SelectBubbleWithKeyboard(1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.SelectBubbleWithKeyboard(2);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.SelectBubbleWithKeyboard(3);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			this.SelectBubbleWithKeyboard(4);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			this.SelectBubbleWithKeyboard(5);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			this.SelectBubbleWithKeyboard(6);
		}
		if (Input.GetKeyDown(KeyCode.F9))
		{
			this.exit = true;
		}
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00008218 File Offset: 0x00006418
	private void SelectBubbleWithKeyboard(int i)
	{
		Debug.Log("Selecting bubble with keyboard: " + i);
		this._choice = i;
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00008238 File Offset: 0x00006438
	private void ConversationUpdate()
	{
		BranchingDialogueNode activeBranchingDialogueNode = this.dialogueRunner.GetActiveBranchingDialogueNode(this._conversationName);
		Shell shellWithName = ShellManager.GetShellWithName(this.avatar.name);
		if (shellWithName == null)
		{
			Debug.Log("Can't find shell for avatar " + this.avatar.name);
		}
		else
		{
			if (this._activeDialogueNode != activeBranchingDialogueNode)
			{
				this._activeDialogueNode = activeBranchingDialogueNode;
				if (this._activeDialogueNode != null)
				{
					this._controls.bubbleManager.ClearThoughtBubbles();
					int num = 0;
					string[] nextNodes = this._activeDialogueNode.nextNodes;
					if (nextNodes.Length == 0)
					{
						Debug.Log("nextNodes.Length == 0 in dialogue " + this._conversationName);
						this.exit = true;
					}
					else if (nextNodes.Length == 1)
					{
						this._choice = 0;
					}
					else
					{
						string[] array = nextNodes;
						for (int i = 0; i < array.Length; i++)
						{
							string text = array[i];
							TimedDialogueNode timedDialogueNode = this.dialogueRunner.GetDialogueNode(this._conversationName, text) as TimedDialogueNode;
							if (!this._showCheatingDialogueOptions && timedDialogueNode.line[0] == '[')
							{
								Debug.Log("Skipping cheating option node: " + timedDialogueNode.line);
								num++;
							}
							else
							{
								this._dialogueOptions.Add(timedDialogueNode.line);
								if (this._avatarGameObject == null)
								{
									this._avatarGameObject = shellWithName.gameObject;
								}
								D.isNull(this._avatarGameObject);
								int alternativeId = num++;
								string text2 = WorldOwner.instance.world.translator.Get(timedDialogueNode.line, timedDialogueNode.conversation);
								this._controls.bubbleManager.CreateBubble(false, null, text2, BubbleType.THINK, delegate
								{
									this._choice = alternativeId;
								}, 0f, "Option");
							}
						}
						this._autoAnswerTimer = 15f;
					}
				}
			}
			if (activeBranchingDialogueNode != null && this._autoAnswerTimer > 0f)
			{
				this._autoAnswerTimer -= Time.deltaTime;
				if (this._autoAnswerTimer <= 0f)
				{
					this._choice = Randomizer.GetIntValue(0, this._dialogueOptions.Count);
				}
			}
			if (this._choice > -1)
			{
				if (activeBranchingDialogueNode == null)
				{
					Debug.Log("Branch node is null!");
				}
				else if (this._choice >= activeBranchingDialogueNode.nextNodes.Length)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Can't choose option ",
						this._choice,
						", there are only ",
						activeBranchingDialogueNode.nextNodes.Length,
						" option nodes."
					}));
				}
				else
				{
					activeBranchingDialogueNode.Choose(this._choice);
					this._dialogueOptions.Clear();
					this._choice = -1;
					this._controls.bubbleManager.ClearThoughtBubbles();
				}
			}
		}
		GUI.FocusControl(string.Empty);
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00008540 File Offset: 0x00006740
	public void OnDialogueRunnerEvent(string pEvent)
	{
		if (pEvent == "ShowClickHereHelpArrow" && !this._isShowingInventory())
		{
			this.ShowClickHereHelpArrow();
		}
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00008574 File Offset: 0x00006774
	public void OnGUI()
	{
		GUI.skin = this._skin;
		GUI.color = Color.white;
		GUI.DrawTexture(new Rect(150f, this._clickHereHelpArrowYPosition, (float)this._clickHereHelpArrow.width, (float)this._clickHereHelpArrow.height), this._clickHereHelpArrow);
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000144 RID: 324 RVA: 0x000085CC File Offset: 0x000067CC
	private Character avatar
	{
		get
		{
			string avatarName = this._controls.world.settings.avatarName;
			return this._controls.world.tingRunner.GetTing(avatarName) as Character;
		}
	}

	// Token: 0x06000145 RID: 325 RVA: 0x0000860C File Offset: 0x0000680C
	private void ShowClickHereHelpArrow()
	{
		float num = 0.25f;
		float num2 = 5f;
		float num3 = -100f;
		float num4 = 120f;
		EasyAnimState<float> easyAnimState = new EasyAnimState<float>(num, num3, num4, new EasyAnimState<float>.InterpolationSampler(iTween.easeInQuad), delegate(float o)
		{
			this._clickHereHelpArrowYPosition = o;
		}, null);
		EasyAnimState<float> easyAnimState2 = easyAnimState.Then(new EasyAnimState<float>(num2, num4, num4, new EasyAnimState<float>.InterpolationSampler(iTween.easeInQuad), delegate(float o)
		{
			this._clickHereHelpArrowYPosition = o;
		}, null));
		easyAnimState2.Then(new EasyAnimState<float>(num, num4, num3, new EasyAnimState<float>.InterpolationSampler(iTween.easeInQuad), delegate(float o)
		{
			this._clickHereHelpArrowYPosition = o;
		}, null));
		this._easyAnimator.Register(this, "ClickHereHelpArrow", easyAnimState);
	}

	// Token: 0x040000BB RID: 187
	private GameViewControls _controls;

	// Token: 0x040000BC RID: 188
	private BranchingDialogueNode _activeDialogueNode;

	// Token: 0x040000BD RID: 189
	private int _choice = -1;

	// Token: 0x040000BE RID: 190
	private List<string> _dialogueOptions = new List<string>();

	// Token: 0x040000BF RID: 191
	private string _conversationName;

	// Token: 0x040000C0 RID: 192
	private GUISkin _skin;

	// Token: 0x040000C1 RID: 193
	private GameObject _avatarGameObject;

	// Token: 0x040000C2 RID: 194
	private int previousZoom;

	// Token: 0x040000C3 RID: 195
	private float previousTilt;

	// Token: 0x040000C4 RID: 196
	private float _autoAnswerTimer = -1f;

	// Token: 0x040000C5 RID: 197
	private Texture _clickHereHelpArrow;

	// Token: 0x040000C6 RID: 198
	private float _clickHereHelpArrowYPosition = -200f;

	// Token: 0x040000C7 RID: 199
	private EasyAnimateTwo _easyAnimator = new EasyAnimateTwo();

	// Token: 0x040000C8 RID: 200
	private bool _showCheatingDialogueOptions;

	// Token: 0x040000C9 RID: 201
	public bool exit;

	// Token: 0x040000CA RID: 202
	private DialogueSubstate.IsShowingInventory _isShowingInventory;

	// Token: 0x020000FA RID: 250
	// (Invoke) Token: 0x06000747 RID: 1863
	public delegate bool IsShowingInventory();
}
