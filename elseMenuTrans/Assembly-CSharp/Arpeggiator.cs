using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class Arpeggiator : MonoBehaviour
{
	// Token: 0x060002E2 RID: 738 RVA: 0x000139B0 File Offset: 0x00011BB0
	private void Start()
	{
		this._time = 0f;
		base.audio.dopplerLevel = 0f;
		if (this.soundNames != null && this.soundNames.Length > 0)
		{
			this.LoadClips();
		}
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000139F8 File Offset: 0x00011BF8
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

	// Token: 0x060002E4 RID: 740 RVA: 0x00013A50 File Offset: 0x00011C50
	private void Update()
	{
		this._time += Time.deltaTime * (this.bpm / 60f);
		int num = (int)this._time % this.steps;
		if (num != this._step)
		{
			AudioClip audioClip = this._clips[num % this._clips.Length];
			base.audio.clip = audioClip;
			base.audio.pitch = this.basePitch + this.octavePitchAmount * this.GetOctave(num);
			base.audio.Play();
		}
		this._step = num;
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00013AE8 File Offset: 0x00011CE8
	private float GetOctave(int pStep)
	{
		return (float)(pStep % this.octaves);
	}

	// Token: 0x040001CB RID: 459
	public string[] soundNames;

	// Token: 0x040001CC RID: 460
	public float bpm = 145f;

	// Token: 0x040001CD RID: 461
	public int steps = 8;

	// Token: 0x040001CE RID: 462
	public int octaves = 3;

	// Token: 0x040001CF RID: 463
	public float octavePitchAmount = 1f;

	// Token: 0x040001D0 RID: 464
	public float basePitch = 1f;

	// Token: 0x040001D1 RID: 465
	private int _step;

	// Token: 0x040001D2 RID: 466
	private float _time;

	// Token: 0x040001D3 RID: 467
	private AudioClip[] _clips;
}
