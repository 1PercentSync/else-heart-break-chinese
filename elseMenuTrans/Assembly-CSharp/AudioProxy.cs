using System;
using GameWorld2;
using RelayLib;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class AudioProxy : MonoBehaviour
{
	// Token: 0x060004B7 RID: 1207 RVA: 0x00020570 File Offset: 0x0001E770
	private void Start()
	{
		MusicBoxShell.LoadMusicBoxSounds();
		if (this.ting != null)
		{
			Debug.Log("Starting " + base.name);
			this.SetupDataListeners();
			this.PlaySoundFromRightPosition();
		}
		else
		{
			Debug.Log(string.Concat(new string[] { "Ting ", this.tingName, " for AudioProxy '", base.name, "' is null" }));
		}
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x000205F0 File Offset: 0x0001E7F0
	private void OnDestroy()
	{
		if (WorldOwner.instance.worldIsLoaded && this.ting != null)
		{
			this.RemoveDataListeners();
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00020620 File Offset: 0x0001E820
	public MimanTing ting
	{
		get
		{
			if (this._ting == null)
			{
				this._ting = WorldOwner.instance.world.tingRunner.GetTingUnsafe(this.tingName) as MimanTing;
			}
			return this._ting;
		}
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00020664 File Offset: 0x0001E864
	private void SetupDataListeners()
	{
		MimanTing ting = this.ting;
		ting.onPlaySound = (MimanTing.OnPlaySound)Delegate.Combine(ting.onPlaySound, new MimanTing.OnPlaySound(this.PlaySound));
		this.ting.AddDataListener<bool>("isPlaying", new ValueEntry<bool>.DataChangeHandler(this.OnIsPlayingChanged));
		this.ting.AddDataListener<string>("soundName", new ValueEntry<string>.DataChangeHandler(this.OnSoundNameChanged));
		this.ting.AddDataListener<float>("pitch", new ValueEntry<float>.DataChangeHandler(this.OnPitchChanged));
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x000206EC File Offset: 0x0001E8EC
	private void RemoveDataListeners()
	{
		MimanTing ting = this.ting;
		ting.onPlaySound = (MimanTing.OnPlaySound)Delegate.Remove(ting.onPlaySound, new MimanTing.OnPlaySound(this.PlaySound));
		this.ting.RemoveDataListener<bool>("isPlaying", new ValueEntry<bool>.DataChangeHandler(this.OnIsPlayingChanged));
		this.ting.RemoveDataListener<string>("soundName", new ValueEntry<string>.DataChangeHandler(this.OnSoundNameChanged));
		this.ting.RemoveDataListener<float>("pitch", new ValueEntry<float>.DataChangeHandler(this.OnPitchChanged));
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00020774 File Offset: 0x0001E974
	private void OnIsPlayingChanged(bool pPrev, bool pNew)
	{
		this.PlaySoundFromRightPosition();
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0002077C File Offset: 0x0001E97C
	private void PlaySoundFromRightPosition()
	{
		if (this.ting.isPlaying)
		{
			Debug.Log(string.Concat(new object[]
			{
				"PROXY for ",
				this.ting.name,
				" is playing at position ",
				this.ting.audioTime
			}));
			this.PlaySound(this.ting.soundName);
			base.audio.time = this.ting.audioTime;
		}
		else
		{
			Debug.Log(this.ting.name + " is not playing");
			this.StopSound();
		}
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x00020828 File Offset: 0x0001EA28
	private void OnSoundNameChanged(string pPrevName, string pNewName)
	{
		this.PlaySoundFromRightPosition();
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x00020830 File Offset: 0x0001EA30
	private void OnPitchChanged(float pPrevPitch, float pNewPitch)
	{
		this.RefreshSound();
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00020838 File Offset: 0x0001EA38
	private void RefreshSound()
	{
		base.audio.pitch = this.ting.pitch;
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x0002085C File Offset: 0x0001EA5C
	protected void PlaySound(string pKey)
	{
		if (base.audio == null)
		{
			Debug.Log("The shell for " + base.name + " doesn't have an audio source");
			return;
		}
		SoundDictionary.PlaySound(pKey, base.audio);
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x000208A4 File Offset: 0x0001EAA4
	protected void StopSound()
	{
		if (base.audio == null)
		{
			Debug.LogError("The shell for " + base.name + " doesn't have an audio source");
		}
		base.audio.Stop();
	}

	// Token: 0x04000390 RID: 912
	public string tingName;

	// Token: 0x04000391 RID: 913
	private MimanTing _ting;
}
