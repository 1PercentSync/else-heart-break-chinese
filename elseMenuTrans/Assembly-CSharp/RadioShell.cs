using System;
using GameTypes;
using GameWorld2;
using RelayLib;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class RadioShell : Shell
{
	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060005CC RID: 1484 RVA: 0x00027208 File Offset: 0x00025408
	public Radio radio
	{
		get
		{
			return base.ting as Radio;
		}
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x00027218 File Offset: 0x00025418
	public override void CreateTing()
	{
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0002721C File Offset: 0x0002541C
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x00027220 File Offset: 0x00025420
	protected override void Setup()
	{
		base.Setup();
		SoundDictionary.LoadMultiSound("Blip", "Blip", 3);
		SoundDictionary.LoadSingleSound("Noise", "tv ant war sound");
		MusicBoxShell.LoadMusicBoxSounds();
		this.RefreshRadioConnection();
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00027260 File Offset: 0x00025460
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		this.radio.AddDataListener<string[]>("connections", new ValueEntry<string[]>.DataChangeHandler(this.OnNewConnectedTings));
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x00027290 File Offset: 0x00025490
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		this.radio.RemoveDataListener<string[]>("connections", new ValueEntry<string[]>.DataChangeHandler(this.OnNewConnectedTings));
		this.RemoveRadioStationListeners();
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x000272C8 File Offset: 0x000254C8
	private void OnNewConnectedTings(string[] pOldConnections, string[] pNewConnections)
	{
		Debug.Log("Connections for " + base.name + " changed");
		this.RefreshRadioConnection();
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x000272F8 File Offset: 0x000254F8
	private void RefreshRadioConnection()
	{
		if (this.radio.connectedTings.Length == 0)
		{
			Debug.Log("Removed radio station listeners for " + base.name);
			this.RemoveRadioStationListeners();
			this._radioStation = null;
			base.StopSound();
		}
		else
		{
			this._radioStation = this.radio.connectedTings[0];
			Debug.Log("Connected to radio station " + this._radioStation);
			this.SetupRadioStationListeners();
			this.PlayRadioStationSoundFromRightPosition();
		}
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00027378 File Offset: 0x00025578
	private void SetupRadioStationListeners()
	{
		if (this._radioStation == null)
		{
			return;
		}
		this._radioStation.AddDataListener<bool>("isPlaying", new ValueEntry<bool>.DataChangeHandler(this.OnIsRadioStationPlayingChanged));
		this._radioStation.AddDataListener<string>("soundName", new ValueEntry<string>.DataChangeHandler(this.OnRadioStationSoundNameChanged));
		this._radioStation.AddDataListener<float>("pitch", new ValueEntry<float>.DataChangeHandler(this.OnRadioStationPitchChanged));
		this.radio.AddDataListener<string[]>("connections", new ValueEntry<string[]>.DataChangeHandler(this.OnIsRadioStationConnectionsChanged));
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x00027404 File Offset: 0x00025604
	private void RemoveRadioStationListeners()
	{
		if (this._radioStation == null)
		{
			return;
		}
		this._radioStation.RemoveDataListener<bool>("isPlaying", new ValueEntry<bool>.DataChangeHandler(this.OnIsRadioStationPlayingChanged));
		this._radioStation.RemoveDataListener<string>("soundName", new ValueEntry<string>.DataChangeHandler(this.OnRadioStationSoundNameChanged));
		this._radioStation.RemoveDataListener<float>("pitch", new ValueEntry<float>.DataChangeHandler(this.OnRadioStationPitchChanged));
		this.radio.RemoveDataListener<string[]>("connections", new ValueEntry<string[]>.DataChangeHandler(this.OnIsRadioStationConnectionsChanged));
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x00027490 File Offset: 0x00025690
	private void OnIsRadioStationPlayingChanged(bool pPrev, bool pNew)
	{
		this.PlayRadioStationSoundFromRightPosition();
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x00027498 File Offset: 0x00025698
	private void OnIsRadioStationChannelChanged(int pPrev, int pNew)
	{
		this.PlayRadioStationSoundFromRightPosition();
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x000274A0 File Offset: 0x000256A0
	private void OnIsRadioStationConnectionsChanged(string[] pPrev, string[] pNew)
	{
		this.PlayRadioStationSoundFromRightPosition();
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x000274A8 File Offset: 0x000256A8
	private void PlayRadioStationSoundFromRightPosition()
	{
		if (this._radioStation == null)
		{
			return;
		}
		if (this.radio == null)
		{
			Debug.Log("radio == null in " + base.name);
			return;
		}
		if (this.radio.containsBrokenPrograms)
		{
			base.PlaySound("Noise");
		}
		else if (this.radio.isPlaying && this._radioStation.isPlaying)
		{
			Debug.Log(string.Concat(new object[]
			{
				base.name,
				"'s radio station ",
				this._radioStation.name,
				" is playing sound ",
				this._radioStation.soundName,
				" at time ",
				this._radioStation.audioTime
			}));
			base.PlaySound(this._radioStation.soundName);
			base.audio.time = this._radioStation.audioTime;
		}
		else
		{
			Debug.Log(base.name + "'s radio station " + this._radioStation.name + " is playing no sound");
			base.StopSound();
		}
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x000275DC File Offset: 0x000257DC
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
		if (this._radioStation != null && this.radio.isPlaying && this._radioStation.isPlaying && base.audio != null && !base.audio.isPlaying)
		{
			D.Log(base.name + "'s radiostation is playing but audio clip is NOT, will fix");
			this.PlayRadioStationSoundFromRightPosition();
		}
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x00027658 File Offset: 0x00025858
	private void OnRadioStationSoundNameChanged(string pPrevName, string pNewName)
	{
		base.PlaySoundFromRightPosition();
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x00027660 File Offset: 0x00025860
	private void OnRadioStationPitchChanged(float pPrevPitch, float pNewPitch)
	{
		this.RadioStationRefreshSound();
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x00027668 File Offset: 0x00025868
	private void RadioStationRefreshSound()
	{
		if (this._radioStation == null)
		{
			return;
		}
		base.audio.pitch = this._radioStation.pitch;
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x00027698 File Offset: 0x00025898
	protected void PlayRadioStationSound(string pKey)
	{
		if (base.audio == null)
		{
			Debug.Log("The shell for " + base.name + " doesn't have an audio source");
			return;
		}
		SoundDictionary.PlaySound(pKey, base.audio);
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x000276E0 File Offset: 0x000258E0
	protected void StopRadioStationSound()
	{
		if (base.audio == null)
		{
			Debug.LogError("The shell for " + base.name + " doesn't have an audio source");
		}
		base.audio.Stop();
	}

	// Token: 0x040003EF RID: 1007
	private MimanTing _radioStation;
}
