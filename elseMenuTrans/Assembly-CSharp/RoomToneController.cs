using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class RoomToneController : MonoBehaviour
{
	// Token: 0x060003D0 RID: 976 RVA: 0x0001BD38 File Offset: 0x00019F38
	private void Awake()
	{
		global::UnityEngine.Object.DontDestroyOnLoad(this);
		if (RoomToneController._tones == null)
		{
			RoomToneController.LoadSounds();
		}
		this._audioSource = base.GetComponent<AudioSource>();
		this._audioSource.loop = true;
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0001BD68 File Offset: 0x00019F68
	private static void LoadSounds()
	{
		RoomToneController._tones = new Dictionary<RoomType, AudioClip>();
		RoomToneController._tones.Add(RoomType.INDOOR, Resources.Load("General_indoor_roomtone") as AudioClip);
		RoomToneController._tones.Add(RoomType.OUTDOOR, Resources.Load("General_outdoor_roomtone") as AudioClip);
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0001BDB4 File Offset: 0x00019FB4
	private void Update()
	{
		if (!this._audioSource.isPlaying)
		{
			Debug.Log("Why not playing sound?! Trying to start it again");
			this.StartCorrectSound();
		}
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x0001BDDC File Offset: 0x00019FDC
	public void SetRoomType(RoomType pRoomType)
	{
		if (pRoomType != this._currentRoomType)
		{
			this._currentRoomType = pRoomType;
			this.StartCorrectSound();
		}
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0001BDFC File Offset: 0x00019FFC
	private void StartCorrectSound()
	{
		this._audioSource.clip = RoomToneController._tones[this._currentRoomType];
		this._audioSource.Play();
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060003D5 RID: 981 RVA: 0x0001BE30 File Offset: 0x0001A030
	public static RoomToneController instance
	{
		get
		{
			if (RoomToneController._instance == null)
			{
				RoomToneController._instance = global::UnityEngine.Object.FindObjectOfType(typeof(RoomToneController)) as RoomToneController;
			}
			if (RoomToneController._instance == null)
			{
				GameObject gameObject = Resources.Load("RoomTone") as GameObject;
				global::UnityEngine.Object.Instantiate(gameObject);
				RoomToneController._instance = global::UnityEngine.Object.FindObjectOfType(typeof(RoomToneController)) as RoomToneController;
				RoomToneController._instance.transform.position = new Vector3(0f, 0f, 0f);
			}
			return RoomToneController._instance;
		}
	}

	// Token: 0x040002F5 RID: 757
	private static RoomToneController _instance;

	// Token: 0x040002F6 RID: 758
	private static Dictionary<RoomType, AudioClip> _tones;

	// Token: 0x040002F7 RID: 759
	private RoomType _currentRoomType;

	// Token: 0x040002F8 RID: 760
	private AudioSource _audioSource;
}
