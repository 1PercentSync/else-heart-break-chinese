using System;
using System.Collections.Generic;
using GrimmLib;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000E RID: 14
public class OptionsPanel : MonoBehaviour
{
	// Token: 0x06000040 RID: 64 RVA: 0x00003A40 File Offset: 0x00001C40
	public void Awake()
	{
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003A44 File Offset: 0x00001C44
	public void Refresh()
	{
		Debug.Log("Refreshing Options Panel");
		Debug.Log(string.Concat(new object[]
		{
			"Options panel, shaders: ",
			OptionsPanel.advancedShadersOn,
			", volume: ",
			AudioListener.volume
		}));
		Debug.Log("Screen.fullScreen = " + Screen.fullScreen);
		this.fullscreenToggle.isOn = Screen.fullScreen;
		Debug.Log("fullscreenToggle.isOn = " + this.fullscreenToggle.isOn);
		this.shaderToggle.isOn = OptionsPanel.advancedShadersOn;
		this.masterVolumeSlider.value = MainMenu.gameVolume;
		this.invertCameraToggle.isOn = GreatCamera.invertCamera;
		this.textSpeedSlider.value = TimedDialogueNode.speedScaling;
		try
		{
			this.CreateResolutionButtons();
		}
		catch (Exception ex)
		{
			Debug.Log("Exception when creating resolution buttons: " + ex);
		}
		this.RefreshQualityButtons();
		this.autoZoomToggle.isOn = MainMenu.autoZoom;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003B70 File Offset: 0x00001D70
	private void RefreshQualityButtons()
	{
		if (QualitySettings.GetQualityLevel() <= 2)
		{
			Debug.Log("Inactivating GOOD button");
			this.good.interactable = false;
			this.better.interactable = true;
			this.best.interactable = true;
		}
		else if (QualitySettings.GetQualityLevel() == 3)
		{
			Debug.Log("Inactivating GREAT button");
			this.good.interactable = true;
			this.better.interactable = false;
			this.best.interactable = true;
		}
		else if (QualitySettings.GetQualityLevel() == 4)
		{
			Debug.Log("Inactivating BEST button");
			this.good.interactable = true;
			this.better.interactable = true;
			this.best.interactable = false;
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00003C34 File Offset: 0x00001E34
	private void CreateResolutionButtons()
	{
		Transform transform = base.transform.Find("ResolutionButton");
		for (int i = 0; i < transform.childCount; i++)
		{
			global::UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
		}
		float num = 0f;
		Debug.Log(string.Concat(new object[]
		{
			"Screen.currentResolution.width = ",
			Screen.currentResolution.width,
			", Screen.currentResolution.height = ",
			Screen.currentResolution.height
		}));
		this._resolutionButtons.Clear();
		OptionsPanel.Res[] resolutions = OptionsPanel._resolutions;
		for (int j = 0; j < resolutions.Length; j++)
		{
			OptionsPanel.Res res2 = resolutions[j];
			Debug.Log(string.Concat(new object[] { "Creating resolution button for resolution ", res2.x, " x ", res2.y }));
			GameObject gameObject = global::UnityEngine.Object.Instantiate(this.resolutionButtonPrefab) as GameObject;
			gameObject.transform.SetParent(transform);
			Transform transform2 = gameObject.transform.Find("Text");
			Text component = transform2.GetComponent<Text>();
			component.text = res2.x + " x " + res2.y;
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			component2.anchoredPosition = new Vector2(0f, num);
			num -= 34f;
			Button buttonComponent2 = gameObject.GetComponent<Button>();
			if (Screen.currentResolution.width == res2.x && Screen.currentResolution.height == res2.y)
			{
				OptionsPanel._lastResolutionButton = buttonComponent2;
			}
			OptionsPanel.Res res = res2;
			int resx = res.x;
			int resy = res.y;
			buttonComponent2.onClick.AddListener(delegate
			{
				Debug.Log(string.Concat(new object[] { "Setting resolution to resolution.x = ", resx, ", resolution.y = ", resy }));
				Screen.SetResolution(resx, resy, Screen.fullScreen);
				PlayerPrefs.SetInt("res_width", res.x);
				PlayerPrefs.SetInt("res_height", res.y);
				PlayerPrefs.Save();
				Debug.Log(string.Concat(new object[]
				{
					"Wrote to res_width, res_height player prefs: ",
					PlayerPrefs.GetInt("res_width"),
					", ",
					PlayerPrefs.GetInt("res_height")
				}));
				if (OptionsPanel._lastResolutionButton != null)
				{
					OptionsPanel._lastResolutionButton.interactable = true;
				}
				buttonComponent2.interactable = false;
				OptionsPanel._lastResolutionButton = buttonComponent2;
			});
			component2.localScale = new Vector3(1f, 1f, 1f);
			this._resolutionButtons.Add(buttonComponent2);
		}
		Debug.Log("Creating native res button");
		GameObject gameObject2 = global::UnityEngine.Object.Instantiate(this.resolutionButtonPrefab) as GameObject;
		gameObject2.transform.SetParent(transform);
		Transform transform3 = gameObject2.transform.Find("Text");
		Text component3 = transform3.GetComponent<Text>();
		component3.text = string.Concat(new object[]
		{
			"Native (",
			Screen.currentResolution.width,
			" x ",
			Screen.currentResolution.height,
			")"
		});
		RectTransform component4 = gameObject2.GetComponent<RectTransform>();
		component4.anchoredPosition = new Vector2(0f, num);
		num -= 34f;
		Button buttonComponent = gameObject2.GetComponent<Button>();
		buttonComponent.onClick.AddListener(delegate
		{
			Debug.Log("Setting resolution to native");
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Screen.fullScreen);
			PlayerPrefs.SetInt("res_width", Screen.currentResolution.width);
			PlayerPrefs.SetInt("res_height", Screen.currentResolution.height);
			PlayerPrefs.Save();
			Debug.Log(string.Concat(new object[]
			{
				"Wrote to res_width, res_height player prefs: ",
				PlayerPrefs.GetInt("res_width"),
				", ",
				PlayerPrefs.GetInt("res_height")
			}));
			if (OptionsPanel._lastResolutionButton != null)
			{
				OptionsPanel._lastResolutionButton.interactable = true;
			}
			buttonComponent.interactable = false;
			OptionsPanel._lastResolutionButton = buttonComponent;
		});
		component4.localScale = new Vector3(1f, 1f, 1f);
		Debug.Log("Time to highlight res button");
		int num2 = 0;
		foreach (Button button in this._resolutionButtons)
		{
			if (Screen.width == OptionsPanel._resolutions[num2].x && Screen.height == OptionsPanel._resolutions[num2].y)
			{
				OptionsPanel._lastResolutionButton.interactable = true;
				button.interactable = false;
				OptionsPanel._lastResolutionButton = button;
			}
			num2++;
		}
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00004060 File Offset: 0x00002260
	private void SetQuality(int pQuality)
	{
		QualitySettings.SetQualityLevel(pQuality);
		PlayerPrefs.SetInt("Quality", pQuality);
		PlayerPrefs.Save();
		Debug.Log("Wrote quality to player prefs: " + PlayerPrefs.GetInt("Quality"));
		this.RefreshQualityButtons();
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000040A8 File Offset: 0x000022A8
	public void SetQualityGood()
	{
		this.SetQuality(2);
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000040B4 File Offset: 0x000022B4
	public void SetQualityGreat()
	{
		this.SetQuality(3);
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000040C0 File Offset: 0x000022C0
	public void SetQualityBest()
	{
		this.SetQuality(4);
	}

	// Token: 0x06000048 RID: 72 RVA: 0x000040CC File Offset: 0x000022CC
	public void ToggleFullscreen()
	{
		Debug.Log("ToggleFullscreen(), fullscreenToggle.isOn = " + this.fullscreenToggle.isOn);
		Screen.fullScreen = this.fullscreenToggle.isOn;
		PlayerPrefs.SetInt("Fullscreen", (!this.fullscreenToggle.isOn) ? 0 : 1);
		PlayerPrefs.Save();
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00004130 File Offset: 0x00002330
	public void ToggleAutoZoom()
	{
		Debug.Log("ToggleAutoZoom(), MainMenu.autoZoom = " + MainMenu.autoZoom);
		MainMenu.autoZoom = this.autoZoomToggle.isOn;
		PlayerPrefs.SetInt("AutoZoom", (!this.autoZoomToggle.isOn) ? 0 : 1);
		PlayerPrefs.Save();
		Debug.Log("Wrote to AutoZoom player prefs: " + PlayerPrefs.GetInt("AutoZoom"));
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000041AC File Offset: 0x000023AC
	public void ToggleShaders()
	{
		OptionsPanel.advancedShadersOn = this.shaderToggle.isOn;
		if (WorldOwner.instance.worldIsLoaded)
		{
			RunGameWorld.instance.SetPostEffects(OptionsPanel.advancedShadersOn);
		}
		else if (this.mainCamera != null)
		{
			OptionsPanel.SetCameraShaderOnOff(this.mainCamera, OptionsPanel.advancedShadersOn);
		}
		PlayerPrefs.SetInt("AdvancedShaders", (!OptionsPanel.advancedShadersOn) ? 0 : 1);
		PlayerPrefs.Save();
		Debug.Log("Wrote to AdvancedShaders player prefs: " + PlayerPrefs.GetInt("AdvancedShaders"));
	}

	// Token: 0x0600004B RID: 75 RVA: 0x0000424C File Offset: 0x0000244C
	public void ToggleInvertCamera()
	{
		GreatCamera.invertCamera = this.invertCameraToggle.isOn;
		PlayerPrefs.SetInt("InvertCamera", (!GreatCamera.invertCamera) ? 0 : 1);
		PlayerPrefs.Save();
		Debug.Log("Wrote to InvertCamera player prefs: " + PlayerPrefs.GetInt("InvertCamera"));
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000042A8 File Offset: 0x000024A8
	public static void SetCameraShaderOnOff(Camera pCamera, bool pEnabled)
	{
		DepthOfField34 component = pCamera.GetComponent<DepthOfField34>();
		component.enabled = pEnabled;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000042C4 File Offset: 0x000024C4
	public void ChangeMasterVolume()
	{
		if (Application.loadedLevelName == "FullGameStartScene")
		{
			MainMenu.SetVolume(this.masterVolumeSlider.value);
		}
		MainMenu.gameVolume = this.masterVolumeSlider.value;
		PlayerPrefs.SetFloat("MasterVolume", MainMenu.gameVolume);
		PlayerPrefs.Save();
		Debug.Log("Saved MasterVolume: " + MainMenu.gameVolume);
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00004334 File Offset: 0x00002534
	public void ChangeTextSpeed()
	{
		TimedDialogueNode.speedScaling = this.textSpeedSlider.value;
		PlayerPrefs.SetFloat("TextSpeed", TimedDialogueNode.speedScaling);
		PlayerPrefs.Save();
		Debug.Log("Saved TextSpeed: " + TimedDialogueNode.speedScaling);
	}

	// Token: 0x0400003B RID: 59
	public Camera mainCamera;

	// Token: 0x0400003C RID: 60
	public Toggle fullscreenToggle;

	// Token: 0x0400003D RID: 61
	public Toggle shaderToggle;

	// Token: 0x0400003E RID: 62
	public Slider masterVolumeSlider;

	// Token: 0x0400003F RID: 63
	public Toggle invertCameraToggle;

	// Token: 0x04000040 RID: 64
	public Slider textSpeedSlider;

	// Token: 0x04000041 RID: 65
	public Button resolutionButton;

	// Token: 0x04000042 RID: 66
	public Toggle autoZoomToggle;

	// Token: 0x04000043 RID: 67
	public Button good;

	// Token: 0x04000044 RID: 68
	public Button better;

	// Token: 0x04000045 RID: 69
	public Button best;

	// Token: 0x04000046 RID: 70
	public static bool advancedShadersOn = false;

	// Token: 0x04000047 RID: 71
	private static OptionsPanel.Res[] _resolutions = new OptionsPanel.Res[]
	{
		new OptionsPanel.Res
		{
			x = 1024,
			y = 768
		},
		new OptionsPanel.Res
		{
			x = 1280,
			y = 720
		},
		new OptionsPanel.Res
		{
			x = 1280,
			y = 960
		},
		new OptionsPanel.Res
		{
			x = 1344,
			y = 756
		},
		new OptionsPanel.Res
		{
			x = 1344,
			y = 1008
		},
		new OptionsPanel.Res
		{
			x = 1600,
			y = 900
		},
		new OptionsPanel.Res
		{
			x = 1600,
			y = 1200
		},
		new OptionsPanel.Res
		{
			x = 1920,
			y = 1080
		},
		new OptionsPanel.Res
		{
			x = 2048,
			y = 1152
		},
		new OptionsPanel.Res
		{
			x = 2560,
			y = 1440
		}
	};

	// Token: 0x04000048 RID: 72
	public GameObject resolutionButtonPrefab;

	// Token: 0x04000049 RID: 73
	private List<Button> _resolutionButtons = new List<Button>();

	// Token: 0x0400004A RID: 74
	private static Button _lastResolutionButton;

	// Token: 0x0200000F RID: 15
	private struct Res
	{
		// Token: 0x0400004B RID: 75
		public int x;

		// Token: 0x0400004C RID: 76
		public int y;
	}
}
