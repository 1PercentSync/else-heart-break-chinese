using System;
using System.Collections;
using GameWorld2;
using RelayLib;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class TvShell : Shell
{
	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x0600062A RID: 1578 RVA: 0x000284F8 File Offset: 0x000266F8
	public Tv tv
	{
		get
		{
			return base.ting as Tv;
		}
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x00028508 File Offset: 0x00026708
	public override void CreateTing()
	{
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0002850C File Offset: 0x0002670C
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x00028510 File Offset: 0x00026710
	protected override void Setup()
	{
		base.Setup();
		SoundDictionary.LoadSingleSound("Terminator", "tv action movie sound");
		SoundDictionary.LoadSingleSound("Anime", "tv cartoon sound");
		SoundDictionary.LoadSingleSound("Bergman", "tv doden sound");
		SoundDictionary.LoadSingleSound("News", "tv news sound");
		SoundDictionary.LoadSingleSound("Talkshow", "tv talk show sound");
		SoundDictionary.LoadSingleSound("Noise", "tv ant war sound");
		this._tvScreen = base.transform.Find("Screen");
		Transform transform = base.transform.Find("Light");
		if (transform != null)
		{
			this._tvLamp = transform.GetComponent<Light>();
		}
		this._noise = Resources.Load("NoiseProgramMaterial") as Material;
		this._black = Resources.Load("BlackMaterial") as Material;
		this.RefreshAppearance();
		base.StartCoroutine("RefreshAgain");
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x000285F8 File Offset: 0x000267F8
	private IEnumerator RefreshAgain()
	{
		for (;;)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Refresh again. Tv.on = ",
				this.tv.on,
				", masterProgram.isOn = ",
				this.tv.masterProgram.isOn
			}));
			if (this.tv.on && !this.tv.masterProgram.isOn)
			{
				this.tv.masterProgram.Start();
			}
			yield return new WaitForSeconds(global::UnityEngine.Random.Range(40f, 60f));
		}
		yield break;
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x00028614 File Offset: 0x00026814
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		this.tv.AddDataListener<bool>("on", new ValueEntry<bool>.DataChangeHandler(this.OnOnChanged));
		this.tv.AddDataListener<string>("channelName", new ValueEntry<string>.DataChangeHandler(this.OnChannelChanged));
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00028660 File Offset: 0x00026860
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		this.tv.RemoveDataListener<bool>("on", new ValueEntry<bool>.DataChangeHandler(this.OnOnChanged));
		this.tv.RemoveDataListener<string>("channelName", new ValueEntry<string>.DataChangeHandler(this.OnChannelChanged));
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x000286AC File Offset: 0x000268AC
	private void OnOnChanged(bool pOldValue, bool pNewValue)
	{
		this.RefreshAppearance();
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x000286B4 File Offset: 0x000268B4
	private void OnChannelChanged(string pOldValue, string pNewValue)
	{
		this.RefreshAppearance();
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x000286BC File Offset: 0x000268BC
	private void RefreshAppearance()
	{
		bool containsBrokenPrograms = this.tv.containsBrokenPrograms;
		if (this.tv.on && !containsBrokenPrograms)
		{
			if (!this._audioSource.isPlaying || this._audioSource.clip.name != this.tv.channelName)
			{
				SoundDictionary.PlaySound(this.tv.channelName, this._audioSource);
			}
			else
			{
				Debug.Log("Tv already playing '" + this.tv.channelName + "'");
			}
		}
		else
		{
			this._audioSource.Stop();
		}
		if (this._tvLamp != null)
		{
			this._tvLamp.enabled = this.tv.on && !containsBrokenPrograms;
		}
		if (this._tvScreen != null)
		{
			if (this.tv.on && containsBrokenPrograms)
			{
				this._tvScreen.renderer.material = this._noise;
				SoundDictionary.PlaySound("Noise", this._audioSource);
			}
			else if (this.tv.on)
			{
				string text = this.tv.channelName + "Material";
				Material material = Resources.Load(text) as Material;
				if (material == null)
				{
					Debug.Log("No material named " + text + " found in resources");
					material = this._noise;
					SoundDictionary.PlaySound("Noise", this._audioSource);
				}
				this._tvScreen.renderer.material = material;
			}
			else
			{
				this._tvScreen.renderer.material = this._black;
			}
		}
	}

	// Token: 0x04000403 RID: 1027
	private Transform _tvScreen;

	// Token: 0x04000404 RID: 1028
	private Light _tvLamp;

	// Token: 0x04000405 RID: 1029
	private Material _noise;

	// Token: 0x04000406 RID: 1030
	private Material _black;
}
