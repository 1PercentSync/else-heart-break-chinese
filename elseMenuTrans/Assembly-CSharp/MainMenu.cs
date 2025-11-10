using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameTypes;
using GameWorld2;
using GrimmLib;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000D RID: 13
public class MainMenu : MonoBehaviour
{
	// Token: 0x0600002C RID: 44 RVA: 0x000030AC File Offset: 0x000012AC
	public static void SetVolume(float percent)
	{
		AudioListener.volume = MainMenu.gameVolume * percent;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x000030BC File Offset: 0x000012BC
	private void Start()
	{
		Debug.Log("Called Start() in MainMenu");
		this.ListSavedGames();
		if (Application.loadedLevelName == "FullGameStartScene")
		{
			if (PlayerPrefs.HasKey("MasterVolume"))
			{
				MainMenu.gameVolume = PlayerPrefs.GetFloat("MasterVolume");
				Debug.Log("Set game volume from player prefs to " + MainMenu.gameVolume);
			}
			OptionsPanel.SetCameraShaderOnOff(this.mainMenuCamera, OptionsPanel.advancedShadersOn);
			MainMenu.SetVolume(1f);
			int num = Screen.currentResolution.width;
			int num2 = Screen.currentResolution.height;
			bool flag = true;
			if (PlayerPrefs.HasKey("res_width") && PlayerPrefs.HasKey("res_height"))
			{
				num = PlayerPrefs.GetInt("res_width");
				num2 = PlayerPrefs.GetInt("res_height");
				Debug.Log(string.Concat(new object[] { "Got resolution setting from player prefs: ", num, " x ", num2 }));
			}
			if (PlayerPrefs.HasKey("Fullscreen"))
			{
				flag = PlayerPrefs.GetInt("Fullscreen") != 0;
				Debug.Log("Got fullscreen setting from player prefs: " + flag);
			}
			Debug.Log(string.Concat(new object[]
			{
				"Setting resolution to ",
				num,
				" x ",
				num2,
				", ",
				(!flag) ? "NOT FULLSCREEN" : "FULLSCREEN"
			}));
			Screen.SetResolution(num, num2, flag);
			if (PlayerPrefs.HasKey("Quality"))
			{
				QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
				Debug.Log("Got Quality setting from player prefs: " + QualitySettings.GetQualityLevel());
			}
			else
			{
				QualitySettings.SetQualityLevel(3);
				Debug.Log("Got no Quality setting from player prefs, using 3 (middle).");
			}
			if (PlayerPrefs.HasKey("AdvancedShaders"))
			{
				OptionsPanel.advancedShadersOn = PlayerPrefs.GetInt("AdvancedShaders") != 0;
				Debug.Log("Got AdvancedShaders setting from player prefs: " + OptionsPanel.advancedShadersOn);
			}
			if (PlayerPrefs.HasKey("InvertCamera"))
			{
				GreatCamera.invertCamera = PlayerPrefs.GetInt("InvertCamera") != 0;
				Debug.Log("Got InvertCamera setting from player prefs: " + GreatCamera.invertCamera);
			}
			if (PlayerPrefs.HasKey("TextSpeed"))
			{
				TimedDialogueNode.speedScaling = PlayerPrefs.GetFloat("TextSpeed");
				Debug.Log("Got TextSpeed setting from player prefs: " + TimedDialogueNode.speedScaling);
			}
			if (PlayerPrefs.HasKey("AutoZoom"))
			{
				MainMenu.autoZoom = PlayerPrefs.GetInt("AutoZoom") != 0;
				Debug.Log("Got AutoZoom setting from player prefs: " + MainMenu.autoZoom);
			}
		}
		if (this.GetSaveData().Count == 0)
		{
			GameObject.Find("LoadGame_Button").GetComponent<global::UnityEngine.UI.Button>().interactable = false;
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000033B4 File Offset: 0x000015B4
	public void Exit()
	{
		Application.Quit();
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000033BC File Offset: 0x000015BC
	public void NewGame(string pLanguage)
	{
		if (Input.GetKey(KeyCode.H))
		{
			MimanGrimmApiDefinitions.cheat = "FirstLecture";
		}
		else if (Input.GetKey(KeyCode.X))
		{
			MimanGrimmApiDefinitions.cheat = "Experiment";
		}
		Debug.Log("Will start new game with language: " + pLanguage);
		RunGameWorld.loadThisPath = string.Empty;
		RunGameWorld.loadThisLanguage = pLanguage;
		base.StartCoroutine("NewOrLoadGameDelayed");
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00003428 File Offset: 0x00001628
	public void SetLanguage(string pLanguage)
	{
		this.ToggleLanguagesPanel();
		if (this.controls == null)
		{
			Debug.Log("controls is null, can't set language to " + pLanguage);
		}
		else
		{
			Debug.Log("Will set language to: " + pLanguage);
			this.controls.world.settings.translationLanguage = pLanguage;
			this.controls.world.RefreshTranslationLanguage();
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003494 File Offset: 0x00001694
	public void LoadGame(string pPath)
	{
		Time.timeScale = 1f;
		Debug.Log("Load game from path: " + pPath);
		RunGameWorld.loadThisPath = pPath;
		PlayerRoamingState.performSlowRoomChange = true;
		base.StartCoroutine("NewOrLoadGameDelayed");
	}

	// Token: 0x06000032 RID: 50 RVA: 0x000034D4 File Offset: 0x000016D4
	private void Update()
	{
		this._easyAnimate.Update(Time.deltaTime);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x000034E8 File Offset: 0x000016E8
	private IEnumerator NewOrLoadGameDelayed()
	{
		Debug.Log("Starting to fade to black, getting ready to load new level");
		this.animator.SetTrigger("FadeToBlack");
		this._masterVolumeStandard = MainMenu.gameVolume;
		EasyAnimState<float> s = new EasyAnimState<float>(1f, this._masterVolumeStandard, 0f, new EasyAnimState<float>.InterpolationSampler(iTween.linear), new EasyAnimState<float>.SetValue(MainMenu.SetVolume));
		this._easyAnimate.Register(this, "fadeOutMusic", s);
		yield return new WaitForSeconds(1.5f);
		GameObject mainMenuSound = GameObject.Find("MenuSound");
		if (mainMenuSound != null)
		{
			mainMenuSound.audio.Stop();
		}
		MainMenu.SetVolume(this._masterVolumeStandard);
		if (this.onLoadGame != null)
		{
			this.onLoadGame(RunGameWorld.loadThisPath);
		}
		else
		{
			Debug.Log("Will load the StartScene now, that in turn should load the correct level");
			Application.LoadLevel("StartScene");
		}
		yield break;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00003504 File Offset: 0x00001704
	public void ToggleLoadedGamesPanel()
	{
		this.animator.SetTrigger("ToggleLoadedGames");
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003518 File Offset: 0x00001718
	public void ToggleLanguagesPanel()
	{
		this.animator.SetTrigger("ShowLanguages");
	}

	// Token: 0x06000036 RID: 54 RVA: 0x0000352C File Offset: 0x0000172C
	public void ToggleOptionsPanel()
	{
		this.optionsPanel.Refresh();
		this.animator.SetTrigger("ShowOptions");
	}

	// Token: 0x06000037 RID: 55 RVA: 0x0000354C File Offset: 0x0000174C
	public void Resume()
	{
		Debug.Log("Resume");
		Time.timeScale = 1f;
		base.StartCoroutine("ResumeDelayed");
	}

	// Token: 0x06000038 RID: 56 RVA: 0x0000357C File Offset: 0x0000177C
	private IEnumerator ResumeDelayed()
	{
		this.animator.Play("FadeOutPauseMenu");
		yield return new WaitForSeconds(0.6f);
		if (this.onResume != null)
		{
			this.onResume();
		}
		yield break;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00003598 File Offset: 0x00001798
	public void ListSavedGames()
	{
		Transform transform = this.loadGameButtons.Find("Saves");
		for (int i = 0; i < transform.childCount; i++)
		{
			global::UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
		}
		float num = 0f;
		foreach (SaveData saveData in this.GetSaveData())
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate(this.savedGameButtonPrefab) as GameObject;
			gameObject.transform.parent = transform;
			Transform transform2 = gameObject.transform.Find("Text");
			Text component = transform2.GetComponent<Text>();
			component.text = saveData.name;
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			component2.anchoredPosition = new Vector2(0f, num);
			num -= 38f;
			global::UnityEngine.UI.Button component3 = gameObject.GetComponent<global::UnityEngine.UI.Button>();
			string path = saveData.path;
			component3.onClick.AddListener(delegate
			{
				this.LoadGame(path);
			});
			component2.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00003700 File Offset: 0x00001900
	private List<SaveData> GetSaveData()
	{
		string quicksave_DATA_PATH = WorldOwner.QUICKSAVE_DATA_PATH;
		List<FileInfo> list = (from f in new DirectoryInfo(quicksave_DATA_PATH).GetFiles()
			orderby f.LastWriteTime
			select f).Reverse<FileInfo>().ToList<FileInfo>();
		List<SaveData> list2 = new List<SaveData>();
		foreach (FileInfo fileInfo in list)
		{
			string name = fileInfo.Name;
			if (name.EndsWith(".json"))
			{
				list2.Add(new SaveData
				{
					name = name.Replace("#", ":").Remove(name.Length - 5),
					path = fileInfo.FullName
				});
			}
		}
		return list2;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003800 File Offset: 0x00001A00
	public void SaveGame()
	{
		this.animator.SetBool("Saving", true);
		base.StartCoroutine(this.ActualSave());
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003820 File Offset: 0x00001A20
	private IEnumerator ActualSave()
	{
		float pauseEndTime = Time.realtimeSinceStartup + 1.5f;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0f;
		}
		Debug.Log("Time to do actual save");
		if (this.controls != null)
		{
			GameTime clock = this.controls.world.settings.gameTimeClock;
			string fileName = string.Concat(new object[]
			{
				this.controls.avatar.room.name.Replace("_", " "),
				" - Day ",
				clock.days,
				" - ",
				clock.ToStringWithoutDayAndSeconds().Replace(":", "#")
			});
			this.controls.world.Save(WorldOwner.QUICKSAVE_DATA_PATH + fileName + ".json");
		}
		else
		{
			Debug.Log("Controls is null, can't save game");
		}
		this.ListSavedGames();
		this.animator.SetBool("Saving", false);
		yield break;
	}

	// Token: 0x0400002E RID: 46
	public Transform loadGameButtons;

	// Token: 0x0400002F RID: 47
	public Animator animator;

	// Token: 0x04000030 RID: 48
	public GameObject savedGameButtonPrefab;

	// Token: 0x04000031 RID: 49
	public GameViewControls controls;

	// Token: 0x04000032 RID: 50
	public OptionsPanel optionsPanel;

	// Token: 0x04000033 RID: 51
	public Action onResume;

	// Token: 0x04000034 RID: 52
	public Action<string> onLoadGame;

	// Token: 0x04000035 RID: 53
	private EasyAnimateTwo _easyAnimate = new EasyAnimateTwo();

	// Token: 0x04000036 RID: 54
	public static float gameVolume = 1f;

	// Token: 0x04000037 RID: 55
	public static bool autoZoom = true;

	// Token: 0x04000038 RID: 56
	public Camera mainMenuCamera;

	// Token: 0x04000039 RID: 57
	private float _masterVolumeStandard = -9999999f;
}
