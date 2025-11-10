using System;
using GameWorld2;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class LookAtMapState : GameViewState
{
	// Token: 0x06000181 RID: 385 RVA: 0x0000A530 File Offset: 0x00008730
	public LookAtMapState(ActionMenu pActionMenu)
	{
		this._skin = (GUISkin)Resources.Load("ActionMenuStyle");
		if (this._skin == null)
		{
			Debug.LogError("Failed to load skin");
		}
		this._actionMenu = pActionMenu;
		this._backButton = new BackButton(this._easyAnimate);
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000A58C File Offset: 0x0000878C
	public override void OnEnterBegin()
	{
		Debug.Log("Map State OnEnterBegin");
		this._actionMenu.FadeOut();
		this._root = base.controls.camera.transform.FindChild("MapScreenArm");
		this._body = this._root.FindChild("Body");
		this._pivot = this._body.FindChild("Pivot");
		this._map = this._pivot.FindChild("MapScreen");
		if (this._root == null || this._body == null || this._pivot == null || this._map == null)
		{
			throw new Exception("mapscreen transform can't be null!");
		}
		this._mapAnimator = this._map.GetComponent<Animator>();
		if (this._mapAnimator == null)
		{
			throw new Exception("mapscreen animator can't be null!");
		}
		this._root.gameObject.SetActive(true);
		this._easyAnimate.Register(this._pivot, "rotate", new EasyAnimState<Vector3>(0.7f, new Vector3(0f, 90f, 0f), new Vector3(90f, 90f, 0f), new EasyAnimState<Vector3>.InterpolationSampler(iTween.vector3easeInOutSine.Sample), delegate(Vector3 o)
		{
			this._pivot.localEulerAngles = o;
		}));
		this._easyAnimate.Register(this._body, "bodyrot", new EasyAnimState<Vector3>(0.5f, new Vector3(0f, 0f, 25f), Vector3.zero, new EasyAnimState<Vector3>.InterpolationSampler(iTween.vector3easeInOutSine.Sample), delegate(Vector3 o)
		{
			this._body.localEulerAngles = o;
		}, null));
		this._mapAnimator.SetTrigger("Open");
		this._backButton.Show();
		base.controls.camera.Input_SetRotation(270f);
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000A788 File Offset: 0x00008988
	public override GameViewState.RETURN OnEnterLoop()
	{
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0000A78C File Offset: 0x0000898C
	public override void OnExitBegin()
	{
		this._actionMenu.FadeIn();
		this._easyAnimate.Register(this._body, "bodyrot", new EasyAnimState<Vector3>(0.5f, Vector3.zero, new Vector3(0f, 0f, 25f), new EasyAnimState<Vector3>.InterpolationSampler(iTween.vector3easeInOutSine.Sample), delegate(Vector3 o)
		{
			this._body.localEulerAngles = o;
		}, null));
		this._backButton.Hide();
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000A808 File Offset: 0x00008A08
	public override GameViewState.RETURN OnExitLoop()
	{
		if (!this._easyAnimate.IsAnimating(this._body, "bodyrot"))
		{
			this._root.gameObject.SetActive(false);
			Debug.Log("Map State FINISHED");
			return GameViewState.RETURN.FINISHED;
		}
		return GameViewState.RETURN.RUN_AGAIN;
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000A850 File Offset: 0x00008A50
	public override void OnUpdate()
	{
		MainMenu.SetVolume(1f - base.controls.fade.alpha);
		base.controls.depthOfField.focalTransform = this._map;
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M) || Input.GetMouseButtonDown(0))
		{
			base.EndState();
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000A8B8 File Offset: 0x00008AB8
	public override void OnGUI()
	{
		GUI.skin = this._skin;
		GUI.color = Color.white;
		if (this._backButton.RenderAndMaybeGoBack())
		{
			base.EndState();
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000188 RID: 392 RVA: 0x0000A8E8 File Offset: 0x00008AE8
	private Character avatar
	{
		get
		{
			return base.controls.avatar as Character;
		}
	}

	// Token: 0x040000F0 RID: 240
	private const float ANIM_TIME = 1f;

	// Token: 0x040000F1 RID: 241
	private ActionMenu _actionMenu;

	// Token: 0x040000F2 RID: 242
	private GUISkin _skin;

	// Token: 0x040000F3 RID: 243
	private Texture _mapTexture;

	// Token: 0x040000F4 RID: 244
	private Transform _root;

	// Token: 0x040000F5 RID: 245
	private Transform _pivot;

	// Token: 0x040000F6 RID: 246
	private Transform _body;

	// Token: 0x040000F7 RID: 247
	private Transform _map;

	// Token: 0x040000F8 RID: 248
	private Animator _mapAnimator;

	// Token: 0x040000F9 RID: 249
	private BackButton _backButton;
}
