using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
public class SoundRandomizer : MonoBehaviour
{
	// Token: 0x0600043E RID: 1086 RVA: 0x0001E254 File Offset: 0x0001C454
	private void Start()
	{
		this._timer = global::UnityEngine.Random.Range(this.StartTimeMin, this.StartTimeMax);
		this._source = base.transform.GetComponent<AudioSource>();
		if (this._source == null)
		{
			Debug.LogError("Can't find audio source component on " + base.transform.name);
		}
		this._source.dopplerLevel = 0f;
		if (this.soundNames != null && this.soundNames.Length > 0)
		{
			this.LoadClips();
		}
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x0001E2E4 File Offset: 0x0001C4E4
	private void LoadClips()
	{
		this._clips = new AudioClip[this.soundNames.Length];
		int num = 0;
		foreach (string text in this.soundNames)
		{
			this._clips[num] = (AudioClip)Resources.Load(text);
			num++;
		}
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x0001E33C File Offset: 0x0001C53C
	private void Update()
	{
		if (this._source == null)
		{
			Debug.Log("No audiosource on " + base.name + ", will deactivate SoundRandomizer");
			base.enabled = false;
			return;
		}
		this._timer -= Time.deltaTime;
		if (this._timer <= 0f && this.InTimeRange() && !this._source.isPlaying && this._source.enabled)
		{
			this.PlaySound();
			this.RestartTimer();
		}
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x0001E3D8 File Offset: 0x0001C5D8
	private bool InTimeRange()
	{
		if (!WorldOwner.instance.worldIsLoaded)
		{
			return false;
		}
		int hours = WorldOwner.instance.world.settings.gameTimeClock.hours;
		if (this.hourOfDayStart < this.hourOfDayEnd)
		{
			return this.hourOfDayStart <= hours && hours < this.hourOfDayEnd;
		}
		return this.hourOfDayStart <= hours || hours < this.hourOfDayEnd;
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0001E458 File Offset: 0x0001C658
	public void PlaySound()
	{
		this.RandomizeBetweenClips();
		if (this._source.audio.clip == null)
		{
			Debug.LogError("No sound clip on " + base.transform.name);
		}
		this._source.pitch = global::UnityEngine.Random.Range(this.MinPitch, this.MaxPitch);
		this._source.Play();
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x0001E4C8 File Offset: 0x0001C6C8
	private void RandomizeBetweenClips()
	{
		if (this._clips != null && this._clips.Length > 0)
		{
			int num = global::UnityEngine.Random.Range(0, this._clips.Length) % this._clips.Length;
			this._source.audio.clip = this._clips[num];
		}
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x0001E520 File Offset: 0x0001C720
	private void RestartTimer()
	{
		this._timer = global::UnityEngine.Random.Range(this.MinInterval, this.MaxInterval);
	}

	// Token: 0x04000342 RID: 834
	public float StartTimeMin = 1f;

	// Token: 0x04000343 RID: 835
	public float StartTimeMax = 2f;

	// Token: 0x04000344 RID: 836
	public float MinInterval = 2f;

	// Token: 0x04000345 RID: 837
	public float MaxInterval = 10f;

	// Token: 0x04000346 RID: 838
	public float MinPitch = 1f;

	// Token: 0x04000347 RID: 839
	public float MaxPitch = 1f;

	// Token: 0x04000348 RID: 840
	public int hourOfDayStart;

	// Token: 0x04000349 RID: 841
	public int hourOfDayEnd = 24;

	// Token: 0x0400034A RID: 842
	public string[] soundNames;

	// Token: 0x0400034B RID: 843
	private float _timer;

	// Token: 0x0400034C RID: 844
	private AudioSource _source;

	// Token: 0x0400034D RID: 845
	private AudioClip[] _clips;
}
