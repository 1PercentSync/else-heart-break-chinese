using System;
using GameTypes;
using GameWorld2;
using RelayLib;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class MusicBoxShell : Shell
{
	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000597 RID: 1431 RVA: 0x00026714 File Offset: 0x00024914
	public MusicBox musicBox
	{
		get
		{
			return base.ting as MusicBox;
		}
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x00026724 File Offset: 0x00024924
	public override void CreateTing()
	{
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x00026728 File Offset: 0x00024928
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0002672C File Offset: 0x0002492C
	public static void LoadMusicBoxSounds()
	{
		SoundDictionary.LoadSingleSound("Ponty", "CafePontyJukeboxSong1");
		SoundDictionary.LoadSingleSound("Fumes", "HotelJukeboxSong2");
		SoundDictionary.LoadSingleSound("Hunger", "HotelJukeboxSong1");
		SoundDictionary.LoadSingleSound("Ghost", "HarborWestHouseRoomJukeboxSong1");
		SoundDictionary.LoadSingleSound("Stairs", "TinyBarnJukeboxSong1");
		SoundDictionary.LoadSingleSound("Apache", "PandaJukeboxSong1");
		SoundDictionary.LoadSingleSound("BlaKnuten", "BlaKnutenVinylMusic");
		SoundDictionary.LoadSingleSound("Vagabond", "Token1 Vagabond");
		SoundDictionary.LoadSingleSound("Surge", "Token2 Surge");
		SoundDictionary.LoadSingleSound("Knobb", "Token3 Knobb");
		SoundDictionary.LoadSingleSound("Panda", "PandaDisketJukeboxSong1");
		SoundDictionary.LoadSingleSound("Hunger", "Token4 Hunger");
		SoundDictionary.LoadSingleSound("HoneyBee", "Token5 HoneyBee");
		SoundDictionary.LoadSingleSound("Confound", "Token6 Confound");
		SoundDictionary.LoadSingleSound("Wolf", "Token7 Wolf");
		SoundDictionary.LoadSingleSound("Clarity", "Token8 Clarity");
		SoundDictionary.LoadSingleSound("Shelter", "Token9 Shelter");
		SoundDictionary.LoadSingleSound("Rust", "Token10 Rust");
		SoundDictionary.LoadSingleSound("ArcadeMusic", "Arcade music");
		SoundDictionary.LoadSingleSound("AllAboutYouToo", "Token 12 AllAboutYouToo");
		SoundDictionary.LoadSingleSound("PetraDJset", "PetraDJset");
		SoundDictionary.LoadSingleSound("PetrasMix", "PetrasMixMusic");
		SoundDictionary.LoadSingleSound("FelixsMix", "FelixPartyMusic");
		SoundDictionary.LoadSingleSound("BalconyTunes", "FelixTerassMusic");
		SoundDictionary.LoadSingleSound("FrankDJset", "ClubDotFrankDJSet");
		SoundDictionary.LoadSingleSound("CasinoMusicBox", "CasinoMusic");
		SoundDictionary.LoadSingleSound("HospitalExteriorMusicBox", "HospitalExteriorMusicMix");
		SoundDictionary.LoadSingleSound("ChillStation", "ChillStation");
		SoundDictionary.LoadSingleSound("TechnoStation", "TechnoStation");
		SoundDictionary.LoadSingleSound("NoirStation", "NoirStation");
		SoundDictionary.LoadSingleSound("SadStation", "SadStation");
		SoundDictionary.LoadSingleSound("ExperimentStation", "NightBeforeExperimentRadioMusic");
		SoundDictionary.LoadSingleSound("Station7", "HiddenStation");
		SoundDictionary.LoadSingleSound("InternetAtmosphereSound1", "Internet atmosphere sound 1");
		SoundDictionary.LoadSingleSound("InternetAtmosphereSound2", "Internet atmosphere sound 2");
		SoundDictionary.LoadSingleSound("FinanceCentreDroneToneSound", "FinanceCentreDroneTone Sound 0");
		SoundDictionary.LoadSingleSound("Square", "SquareSound");
		SoundDictionary.LoadSingleSound("Button", "ButtonSound");
		SoundDictionary.LoadSingleSound("GunnarHutSound", "GunnarsHutSound 0");
		SoundDictionary.LoadSingleSound("MinistryExperimentHallDrone", "MinistryExperimentHallDrone Sound 0");
		SoundDictionary.LoadSingleSound("BlaKnutenVinylMusicBox", "BlaKnutenVinylMusic");
		SoundDictionary.LoadSingleSound("Meisure", "Meisure");
		SoundDictionary.LoadSingleSound("ThereminSound", "ThereminSound 0");
		SoundDictionary.LoadSingleSound("HereWithOutYouMusicBox", "HereWithOutYouMusic");
		SoundDictionary.LoadSingleSound("TrumpetDudeMusixBox", "TrumpetDudeSound 0");
		SoundDictionary.LoadSingleSound("RatvadersDream", "RatvadersDream Loop1");
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x000269EC File Offset: 0x00024BEC
	protected override void Setup()
	{
		base.Setup();
		MusicBoxShell.LoadMusicBoxSounds();
		this._lowpassFilter = base.GetComponent<AudioLowPassFilter>();
		this.PlaySoundFromRightPosition();
		this.RefreshSound();
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x00026A1C File Offset: 0x00024C1C
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		this.musicBox.AddDataListener<float>("cutoff", new ValueEntry<float>.DataChangeHandler(this.OnCutoffFrequenzyChanged));
		this.musicBox.AddDataListener<float>("resonance", new ValueEntry<float>.DataChangeHandler(this.OnResonanceChanged));
		this.musicBox.AddDataListener<float>("pitch", new ValueEntry<float>.DataChangeHandler(this.OnPitchChanged));
		MusicBox musicBox = this.musicBox;
		musicBox.onPlaySound = (MimanTing.OnPlaySound)Delegate.Combine(musicBox.onPlaySound, new MimanTing.OnPlaySound(this.OnPlaySound));
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x00026AAC File Offset: 0x00024CAC
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		this.musicBox.RemoveDataListener<float>("cutoff", new ValueEntry<float>.DataChangeHandler(this.OnCutoffFrequenzyChanged));
		this.musicBox.RemoveDataListener<float>("resonance", new ValueEntry<float>.DataChangeHandler(this.OnResonanceChanged));
		this.musicBox.RemoveDataListener<float>("pitch", new ValueEntry<float>.DataChangeHandler(this.OnPitchChanged));
		MusicBox musicBox = this.musicBox;
		musicBox.onPlaySound = (MimanTing.OnPlaySound)Delegate.Remove(musicBox.onPlaySound, new MimanTing.OnPlaySound(this.OnPlaySound));
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00026B3C File Offset: 0x00024D3C
	private void OnPlaySound(string pSound)
	{
		D.Log(string.Concat(new string[] { "OnPlaySound called on ", base.name, " with sound ", pSound, ", will play sound from right position" }));
		this.PlaySoundFromRightPosition();
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00026B7C File Offset: 0x00024D7C
	private void OnIsPlayingChanged(bool pPrev, bool pNew)
	{
		this.PlaySoundFromRightPosition();
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x00026B84 File Offset: 0x00024D84
	public new void PlaySoundFromRightPosition()
	{
		if (this.musicBox.isPlaying)
		{
			Debug.Log(string.Concat(new object[]
			{
				base.name,
				" is playing sound ",
				this.musicBox.soundName,
				" at time position ",
				this.musicBox.audioTime
			}));
			base.PlaySound(this.musicBox.soundName);
			base.audio.time = this.musicBox.audioTime;
		}
		else
		{
			base.StopSound();
		}
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x00026C20 File Offset: 0x00024E20
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
		if (this.musicBox.isPlaying && !base.audio.isPlaying)
		{
			D.Log(base.name + "'s ting is playing but audio clip is NOT, will fix");
			this.PlaySoundFromRightPosition();
		}
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x00026C70 File Offset: 0x00024E70
	private void OnCutoffFrequenzyChanged(float pPreviousCutoffFrequency, float pNewCutoffFrequency)
	{
		this.RefreshSound();
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x00026C78 File Offset: 0x00024E78
	private void OnResonanceChanged(float pPreviousResonance, float pNewCutoffResonance)
	{
		this.RefreshSound();
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x00026C80 File Offset: 0x00024E80
	private void OnPitchChanged(float pPrevPitch, float pNewPitch)
	{
		this.RefreshSound();
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x00026C88 File Offset: 0x00024E88
	private void RefreshSound()
	{
		if (this._lowpassFilter != null)
		{
			this._lowpassFilter.cutoffFrequency = this.musicBox.cutoffFrequency;
			this._lowpassFilter.lowpassResonaceQ = this.musicBox.resonance;
		}
		this._audioSource.pitch = this.musicBox.pitch;
	}

	// Token: 0x040003E7 RID: 999
	public bool isProxy;

	// Token: 0x040003E8 RID: 1000
	private AudioLowPassFilter _lowpassFilter;
}
