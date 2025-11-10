using System;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class SoundDayCycleController : MonoBehaviour
{
	// Token: 0x06000439 RID: 1081 RVA: 0x0001E058 File Offset: 0x0001C258
	private void Start()
	{
		this._source = base.transform.GetComponent<AudioSource>();
		if (this._source == null)
		{
			Debug.Log("Can't find audio source component on " + base.transform.name);
		}
		else
		{
			this._source.dopplerLevel = 0f;
			this._source.Play();
			this._source.time = global::UnityEngine.Random.Range(0f, 10f);
		}
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x0001E0DC File Offset: 0x0001C2DC
	private void Update()
	{
		if (this._source == null)
		{
			Debug.Log("No audiosource on " + base.name + ", will deactivate SoundDayCycleController");
			base.enabled = false;
			return;
		}
		if (this._source.enabled && !this._source.isPlaying && this.InTimeRange())
		{
			this.PlaySound();
		}
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x0001E150 File Offset: 0x0001C350
	private bool InTimeRange()
	{
		int num = 0;
		if (this.hourOfDayStart < this.hourOfDayEnd)
		{
			return this.hourOfDayStart <= num && num < this.hourOfDayEnd;
		}
		return this.hourOfDayStart <= num || num < this.hourOfDayEnd;
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x0001E1A4 File Offset: 0x0001C3A4
	private void PlaySound()
	{
		if (this._source.audio.clip == null)
		{
			Debug.LogError("No sound clip on " + base.transform.name);
		}
		this._source.Play();
	}

	// Token: 0x0400033F RID: 831
	public int hourOfDayStart;

	// Token: 0x04000340 RID: 832
	public int hourOfDayEnd = 24;

	// Token: 0x04000341 RID: 833
	private AudioSource _source;
}
