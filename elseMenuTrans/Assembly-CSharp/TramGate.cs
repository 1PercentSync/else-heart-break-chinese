using System;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class TramGate : MonoBehaviour
{
	// Token: 0x06000458 RID: 1112 RVA: 0x0001E9E4 File Offset: 0x0001CBE4
	private void Start()
	{
		this._animator = base.GetComponent<Animator>();
		SoundDictionary.LoadSingleSound("TramBridgeClose", "Tram bridge close sound 0");
		SoundDictionary.LoadSingleSound("TramBridgeOpen", "Tram bridge open sound 0");
		SoundDictionary.LoadSingleSound("TramDoorBell", "Tram bridge warning bell sound 0");
		this._bell = base.transform.Find("Bell").GetComponent<AudioSource>();
		this._bell.Stop();
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0001EA50 File Offset: 0x0001CC50
	public void Open()
	{
		this._animator.SetBool(TramGate.OPEN, true);
		if (!this.opened && !base.audio.isPlaying)
		{
			this._bell.Play();
		}
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0001EA94 File Offset: 0x0001CC94
	public void Close()
	{
		this._animator.SetBool(TramGate.OPEN, false);
		if (this.opened && !base.audio.isPlaying)
		{
			this._bell.Stop();
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x0600045B RID: 1115 RVA: 0x0001EAD8 File Offset: 0x0001CCD8
	public bool opened
	{
		get
		{
			return this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Opened");
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x0600045C RID: 1116 RVA: 0x0001EB00 File Offset: 0x0001CD00
	public bool canWalkOverIt
	{
		get
		{
			return this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Closed");
		}
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0001EB28 File Offset: 0x0001CD28
	public void PlayOpenSound()
	{
		SoundDictionary.PlaySound("TramBridgeOpen", base.audio);
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0001EB3C File Offset: 0x0001CD3C
	public void PlayCloseSound()
	{
		SoundDictionary.PlaySound("TramBridgeClose", base.audio);
	}

	// Token: 0x0400035A RID: 858
	private static readonly int OPEN = Animator.StringToHash("Open");

	// Token: 0x0400035B RID: 859
	private Animator _animator;

	// Token: 0x0400035C RID: 860
	private AudioSource _bell;
}
