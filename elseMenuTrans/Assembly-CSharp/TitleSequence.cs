using System;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class TitleSequence : GameViewState
{
	// Token: 0x060001E2 RID: 482 RVA: 0x0000E928 File Offset: 0x0000CB28
	public TitleSequence(string pEventWhenDone, float pTime, ActionMenu pActionMenu)
	{
		this._eventWhenTimeToEndThisState = pEventWhenDone;
		this._time = pTime;
		this._actionMenu = pActionMenu;
		this._heart = Resources.Load("Heart_NOSCALE") as Texture;
		if (this._heart == null)
		{
			Debug.LogError("_heart is null");
		}
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000E980 File Offset: 0x0000CB80
	public override void OnEnterBegin()
	{
		Debug.Log("controls.fade.alpha: " + base.controls.fade.alpha);
		if (base.controls.fade.alpha < 0.5f)
		{
			this._actionMenu.FadeOut();
			base.controls.fade.speed = 0.3f;
			base.controls.fade.FadeToColor(Color.black);
			Debug.Log("TitleSequence.OnEnterBegin() - Will fade to black");
		}
		else
		{
			base.controls.fade.alpha = 1f;
			Debug.Log("TitleSequence.OnEnterBegin() - controls.fade.alpha = 1.0f");
		}
		this._masterAudioListener = RunGameWorld.instance.camera.GetComponentInChildren<AudioListener>();
		Screen.showCursor = false;
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x0000EA4C File Offset: 0x0000CC4C
	public override GameViewState.RETURN OnEnterLoop()
	{
		if (base.controls.fade.alpha < 1f)
		{
			return GameViewState.RETURN.RUN_AGAIN;
		}
		if (this._eventWhenTimeToEndThisState == "INTERNET_ENDING")
		{
			Application.LoadLevel("InternetOutro");
		}
		else
		{
			Application.LoadLevel("Intro");
		}
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x0000EAA4 File Offset: 0x0000CCA4
	public override void OnGUI()
	{
		base.OnGUI();
		GUI.color = new Color(1f, 1f, 1f, this._heartAlpha);
		GUI.DrawTexture(this.GetCenterPos(this._heart), this._heart);
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x0000EAF0 File Offset: 0x0000CCF0
	private Rect GetCenterPos(Texture pTexture)
	{
		return new Rect((float)(Screen.width / 2 - pTexture.width / 2), (float)(Screen.height / 2 - pTexture.height / 2), (float)pTexture.width, (float)pTexture.height);
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x0000EB34 File Offset: 0x0000CD34
	public override void OnUpdate()
	{
		base.OnUpdate();
		this._time -= Time.deltaTime;
		if (this._time <= 0f && !this._ending)
		{
			Debug.Log("Ending Title Sequence state");
			this.EndWithFade();
		}
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000EB84 File Offset: 0x0000CD84
	private void EndWithFade()
	{
		this._ending = true;
		base.controls.fade.speed = 0.3f;
		base.controls.fade.FadeToColor(Color.black);
		base.controls.fade.onFadedToOpaque = new Fade.FadeCompleteEvent(base.EndState);
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000EBE0 File Offset: 0x0000CDE0
	public override void OnExitBegin()
	{
		base.OnExitBegin();
		this.EnableCorrectCameras();
		this._actionMenu.FadeIn();
		WorldOwner.instance.world.dialogueRunner.EventHappened(this._eventWhenTimeToEndThisState);
		Screen.showCursor = true;
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000EC24 File Offset: 0x0000CE24
	private void EnableCorrectCameras()
	{
		foreach (Camera camera in global::UnityEngine.Object.FindObjectsOfType<Camera>())
		{
			camera.enabled = false;
			AudioListener component = camera.GetComponent<AudioListener>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
		RunGameWorld.instance.camera.enabled = true;
		this._masterAudioListener.enabled = true;
	}

	// Token: 0x04000134 RID: 308
	private string _eventWhenTimeToEndThisState;

	// Token: 0x04000135 RID: 309
	private Texture _heart;

	// Token: 0x04000136 RID: 310
	private float _time;

	// Token: 0x04000137 RID: 311
	private ActionMenu _actionMenu;

	// Token: 0x04000138 RID: 312
	private bool _ending;

	// Token: 0x04000139 RID: 313
	private float _heartAlpha;

	// Token: 0x0400013A RID: 314
	private AudioListener _masterAudioListener;
}
