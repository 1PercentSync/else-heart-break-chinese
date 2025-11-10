using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
[RequireComponent(typeof(AudioDistortionFilter))]
public class ComputerSoundMaker : MonoBehaviour
{
	// Token: 0x06000302 RID: 770 RVA: 0x00017324 File Offset: 0x00015524
	private void Start()
	{
		this._audioSource = base.GetComponent<AudioSource>();
		if (this._audioSource == null)
		{
			Debug.LogError("Can't find audio source in " + base.name);
		}
		this._lowPassFilter = base.GetComponent<AudioLowPassFilter>();
		if (this._audioSource == null)
		{
			Debug.LogError("Can't find low pass filter in " + base.name);
		}
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00017398 File Offset: 0x00015598
	private float GetTonePitch(string pTone)
	{
		float num = 55f;
		char c = pTone[0];
		if (c == 'R')
		{
			c = (new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' })[global::UnityEngine.Random.Range(0, 7)];
		}
		switch (c)
		{
		case 'A':
			num = 55f;
			break;
		case 'B':
			num = 61.74f;
			break;
		case 'C':
			num = 65.41f;
			break;
		case 'D':
			num = 73.42f;
			break;
		case 'E':
			num = 82.41f;
			break;
		case 'F':
			num = 87.31f;
			break;
		case 'G':
			num = 98f;
			break;
		}
		return num / 55f;
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00017458 File Offset: 0x00015658
	private void Update()
	{
		int num = (int)this._currentTone;
		this._currentTone += Time.deltaTime * this.songSpeed;
		while ((int)this._currentTone > this.sequencer.Length - 1)
		{
			this._currentTone -= (float)this.sequencer.Length;
		}
		if (num != (int)this._currentTone)
		{
			string text = this.sequencer[(int)this._currentTone];
			if (text == " " || text == string.Empty)
			{
				this._audioSource.mute = true;
			}
			else
			{
				this._audioSource.mute = false;
				this._targetTonePitch = this.GetTonePitch(text) * (float)this.extraPitch;
			}
		}
		float num2 = this._audioSource.pitch - this._targetTonePitch;
		if (this.portamento < 1f)
		{
			this.portamento = 1f;
		}
		this._audioSource.pitch -= num2 / this.portamento;
		if (this.filterStepMode == ComputerSoundMaker.FilterStepMode.SIN)
		{
			this._lowPassFilter.cutoffFrequency = this.filterBaseline + this.filterMod * Mathf.Sin(Time.timeSinceLevelLoad * this.filterPhaseSpeed);
		}
		else if (this.filterStepMode == ComputerSoundMaker.FilterStepMode.RANDOM)
		{
			this._filterStepTimer -= Time.deltaTime * this.filterPhaseSpeed;
			if (this._filterStepTimer <= 0f)
			{
				float num3 = this.filterBaseline + this.filterMod * global::UnityEngine.Random.Range(-1f, 1f);
				this._lowPassFilter.cutoffFrequency = num3;
				this._filterStepTimer = 1f;
			}
		}
		else
		{
			this._lowPassFilter.cutoffFrequency = this.filterBaseline;
		}
	}

	// Token: 0x040001F8 RID: 504
	private const float BASE_TONE_FREQ = 55f;

	// Token: 0x040001F9 RID: 505
	public int extraPitch = 1;

	// Token: 0x040001FA RID: 506
	public float songSpeed = 1f;

	// Token: 0x040001FB RID: 507
	public string[] sequencer = new string[8];

	// Token: 0x040001FC RID: 508
	public float filterPhaseSpeed = 0.1f;

	// Token: 0x040001FD RID: 509
	public float filterBaseline = 5000f;

	// Token: 0x040001FE RID: 510
	public float filterMod = 4500f;

	// Token: 0x040001FF RID: 511
	public float portamento = 2f;

	// Token: 0x04000200 RID: 512
	public ComputerSoundMaker.FilterStepMode filterStepMode;

	// Token: 0x04000201 RID: 513
	private AudioSource _audioSource;

	// Token: 0x04000202 RID: 514
	private AudioLowPassFilter _lowPassFilter;

	// Token: 0x04000203 RID: 515
	private float _toneStepTimer;

	// Token: 0x04000204 RID: 516
	private float _filterStepTimer;

	// Token: 0x04000205 RID: 517
	private float _currentTone;

	// Token: 0x04000206 RID: 518
	private float _targetTonePitch = 1f;

	// Token: 0x02000055 RID: 85
	public enum FilterStepMode
	{
		// Token: 0x04000208 RID: 520
		SIN,
		// Token: 0x04000209 RID: 521
		RANDOM,
		// Token: 0x0400020A RID: 522
		OFF
	}
}
