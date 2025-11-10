using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class IntroDirector : MonoBehaviour
{
	// Token: 0x06000377 RID: 887 RVA: 0x0001989C File Offset: 0x00017A9C
	private void Awake()
	{
		if (RunGameWorld.instance != null)
		{
			RunGameWorld.instance.camera.GetComponentInChildren<AudioListener>().enabled = false;
			this.expensiveEffects = RunGameWorld.instance.isPostEffectsOn;
			this.intro = !RunGameWorld.instance.gameViewControls.world.settings.beaten;
			Debug.Log("Post effects are " + ((!this.expensiveEffects) ? "off" : "on"));
		}
		else
		{
			Debug.Log("No run game world instance");
		}
		this._cameras = global::UnityEngine.Object.FindObjectsOfType<Camera>();
		Debug.Log("Found " + this._cameras.Length + " cameras");
		foreach (Camera camera in this._cameras)
		{
			this.SetEnabled(camera, false, this.expensiveEffects);
		}
		if (this.intro)
		{
			Debug.Log("Intro");
			base.StartCoroutine(this.IntroSequence());
			base.StartCoroutine(this.SequenceIntroTexts());
		}
		else
		{
			Debug.Log("Outro in scene '" + Application.loadedLevelName + "'");
			if (Application.loadedLevelName == "InternetOutro")
			{
				base.StartCoroutine(this.InternetOutroSequence());
				base.StartCoroutine(this.SequenceOutroTexts());
			}
			else
			{
				Transform transform = GameObject.Find("Boat").transform;
				transform.Rotate(0f, 180f, 0f);
				transform.Translate(Vector3.back * 2000f);
				GameObject.Find("Cylinder").GetComponent<MoveForward>().enabled = false;
				base.StartCoroutine(this.OutroSequence());
				base.StartCoroutine(this.SequenceOutroTexts());
			}
		}
	}

	// Token: 0x06000378 RID: 888 RVA: 0x00019A7C File Offset: 0x00017C7C
	private void Update()
	{
		this.time += Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (this.intro)
			{
				Debug.Log("Can't skip intro");
			}
			else if (this.time < 5f)
			{
				Debug.Log("Must watch 5 seconds of outro");
			}
			else
			{
				Application.Quit();
			}
		}
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00019AE8 File Offset: 0x00017CE8
	private void CreateIntroSound(string pName)
	{
		GameObject gameObject = Resources.Load(pName) as GameObject;
		this._introSoundInstance = global::UnityEngine.Object.Instantiate(gameObject) as GameObject;
		this._introSoundInstance.transform.position = RunGameWorld.instance.transform.position;
		this._introSoundInstance.audio.Play();
		global::UnityEngine.Object.DontDestroyOnLoad(this._introSoundInstance);
		MainMenu.SetVolume(1f);
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00019B58 File Offset: 0x00017D58
	private IEnumerator IntroSequence()
	{
		this.CreateIntroSound("TitleSequenceSoundPlayer");
		this.SwitchToCamera("Camera_Periscope");
		yield return new WaitForSeconds(4f);
		if (WorldOwner.instance.worldIsLoaded)
		{
			Fade fade = RunGameWorld.instance.gameViewControls.fade;
			fade.speed = 0.3f;
			fade.FadeToTransparent();
		}
		else
		{
			Debug.Log("Won't fade to transparent in intro because world is not loaded");
		}
		yield return new WaitForSeconds(10f);
		this.SwitchToCamera("Camera_Top");
		yield return new WaitForSeconds(10f);
		this.SwitchToCamera("Camera_Behind");
		yield return new WaitForSeconds(10f);
		this.SwitchToCamera("Camera_Dorisburg");
		yield break;
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00019B74 File Offset: 0x00017D74
	private IEnumerator SequenceIntroTexts()
	{
		yield return new WaitForSeconds(5f);
		this.introTextsAnimator.SetTrigger("Next");
		yield return new WaitForSeconds(4f);
		this.introTextsAnimator.SetTrigger("Next");
		yield return new WaitForSeconds(5f);
		this.introTextsAnimator.SetTrigger("Next");
		yield return new WaitForSeconds(5f);
		this.introTextsAnimator.SetTrigger("Next");
		yield return new WaitForSeconds(5f);
		this.introTextsAnimator.SetTrigger("Next");
		yield return new WaitForSeconds(8f);
		this.introTextsAnimator.SetTrigger("Next");
		yield return new WaitForSeconds(5f);
		for (;;)
		{
			yield return new WaitForSeconds(5f);
			this.introTextsAnimator.SetTrigger("Next");
		}
		yield break;
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00019B90 File Offset: 0x00017D90
	private IEnumerator OutroSequence()
	{
		this.SwitchToCamera("Camera_GoodBye");
		this.CreateIntroSound("OutroSequenceSoundPlayer");
		yield return new WaitForSeconds(1f);
		if (WorldOwner.instance.worldIsLoaded)
		{
			Fade fade = RunGameWorld.instance.gameViewControls.fade;
			fade.speed = 0.3f;
			fade.FadeToTransparent();
		}
		else
		{
			Debug.Log("Won't fade to transparent in outro because world is not loaded");
		}
		yield break;
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00019BAC File Offset: 0x00017DAC
	private IEnumerator InternetOutroSequence()
	{
		this.SwitchToCamera("Camera_GoodBye");
		this.CreateIntroSound("InternetOutroSequenceSoundPlayer");
		yield return new WaitForSeconds(1f);
		if (WorldOwner.instance.worldIsLoaded)
		{
			Fade fade = RunGameWorld.instance.gameViewControls.fade;
			fade.speed = 0.3f;
			fade.FadeToTransparent();
		}
		else
		{
			Debug.Log("Won't fade to transparent in outro because world is not loaded");
		}
		yield break;
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00019BC8 File Offset: 0x00017DC8
	private IEnumerator SequenceOutroTexts()
	{
		this.introTextsAnimator.Play("Outro");
		for (;;)
		{
			yield return new WaitForSeconds(5f);
			this.introTextsAnimator.SetTrigger("Next");
		}
		yield break;
	}

	// Token: 0x0600037F RID: 895 RVA: 0x00019BE4 File Offset: 0x00017DE4
	private void SwitchToCamera(string pCameraName)
	{
		Camera camera = this.GetCamera(pCameraName);
		this.SetEnabled(this.currentCamera, false, this.expensiveEffects);
		this.SetEnabled(camera, true, this.expensiveEffects);
		this.currentCamera = camera;
	}

	// Token: 0x06000380 RID: 896 RVA: 0x00019C24 File Offset: 0x00017E24
	private Camera GetCamera(string pName)
	{
		foreach (Camera camera in this._cameras)
		{
			if (camera.name == pName)
			{
				return camera;
			}
		}
		return null;
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00019C64 File Offset: 0x00017E64
	private void SetEnabled(Camera pCamera, bool pEnabled, bool pExpensiveEffects)
	{
		if (pCamera == null)
		{
			Debug.Log("pCamera is null");
			return;
		}
		pCamera.enabled = pEnabled;
		Vignetting component = pCamera.GetComponent<Vignetting>();
		ScreenOverlay component2 = pCamera.GetComponent<ScreenOverlay>();
		DepthOfFieldScatter component3 = pCamera.GetComponent<DepthOfFieldScatter>();
		if (component != null)
		{
			component.enabled = pExpensiveEffects;
		}
		if (component2 != null)
		{
			component2.enabled = pExpensiveEffects;
		}
		if (component3 != null)
		{
			component3.enabled = pExpensiveEffects;
		}
		AudioListener component4 = pCamera.gameObject.GetComponent<AudioListener>();
		if (component4 != null)
		{
			component4.enabled = pEnabled;
		}
	}

	// Token: 0x04000294 RID: 660
	private Camera[] _cameras;

	// Token: 0x04000295 RID: 661
	public Camera currentCamera;

	// Token: 0x04000296 RID: 662
	private bool expensiveEffects;

	// Token: 0x04000297 RID: 663
	public Animator introTextsAnimator;

	// Token: 0x04000298 RID: 664
	public bool intro = true;

	// Token: 0x04000299 RID: 665
	private float time;

	// Token: 0x0400029A RID: 666
	private GameObject _introSoundInstance;
}
