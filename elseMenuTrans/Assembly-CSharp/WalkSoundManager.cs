using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class WalkSoundManager
{
	// Token: 0x06000642 RID: 1602 RVA: 0x00028AEC File Offset: 0x00026CEC
	public WalkSoundManager()
	{
		if (WalkSoundManager._footDownSounds == null)
		{
			this.LoadSounds();
		}
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00028B0C File Offset: 0x00026D0C
	private void LoadSounds()
	{
		WalkSoundManager._footDownSounds = new Dictionary<string, List<List<AudioClip>>>();
		List<AudioClip> list = new List<AudioClip>();
		this.LoadSoundArray(list, "Footsteps woodplank ", 6);
		List<AudioClip> list2 = new List<AudioClip>();
		this.LoadSoundArray(list2, "Footstep wood sound L", 11);
		List<AudioClip> list3 = new List<AudioClip>();
		this.LoadSoundArray(list3, "Footstep wood sound R", 11);
		List<AudioClip> list4 = new List<AudioClip>();
		this.LoadSoundArray(list4, "Footstep concrete sound L", 11);
		List<AudioClip> list5 = new List<AudioClip>();
		this.LoadSoundArray(list5, "Footstep concrete sound R", 11);
		List<AudioClip> list6 = new List<AudioClip>();
		this.LoadSoundArray(list6, "Footstep tiles sound L", 11);
		List<AudioClip> list7 = new List<AudioClip>();
		this.LoadSoundArray(list7, "Footstep tiles sound R", 11);
		List<AudioClip> list8 = new List<AudioClip>();
		this.LoadSoundArray(list8, "Footstep garvel sound L", 11);
		List<AudioClip> list9 = new List<AudioClip>();
		this.LoadSoundArray(list9, "Footstep garvel sound R", 11);
		List<AudioClip> list10 = new List<AudioClip>();
		this.LoadSoundArray(list10, "Footstep internet sound L", 5);
		List<AudioClip> list11 = new List<AudioClip>();
		this.LoadSoundArray(list11, "Footstep internet sound R", 5);
		WalkSoundManager._footDownSounds.Add("Woodplank", new List<List<AudioClip>> { list, list });
		WalkSoundManager._footDownSounds.Add("Smallwood", new List<List<AudioClip>> { list2, list3 });
		WalkSoundManager._footDownSounds.Add("Concrete", new List<List<AudioClip>> { list4, list5 });
		WalkSoundManager._footDownSounds.Add("Tiles", new List<List<AudioClip>> { list6, list7 });
		WalkSoundManager._footDownSounds.Add("Garvel", new List<List<AudioClip>> { list8, list9 });
		WalkSoundManager._footDownSounds.Add("Internet", new List<List<AudioClip>> { list10, list11 });
		List<AudioClip> list12 = new List<AudioClip>();
		this.LoadSoundArray(list12, "Footsteps woodplank ", 6);
		List<AudioClip> list13 = new List<AudioClip>();
		this.LoadSoundArray(list13, "Footstep wood run sound L", 11);
		List<AudioClip> list14 = new List<AudioClip>();
		this.LoadSoundArray(list14, "Footstep wood run sound R", 11);
		List<AudioClip> list15 = new List<AudioClip>();
		this.LoadSoundArray(list15, "Footstep concrete run sound L", 11);
		List<AudioClip> list16 = new List<AudioClip>();
		this.LoadSoundArray(list16, "Footstep concrete run sound R", 11);
		List<AudioClip> list17 = new List<AudioClip>();
		this.LoadSoundArray(list17, "Footstep tiles run sound L", 11);
		List<AudioClip> list18 = new List<AudioClip>();
		this.LoadSoundArray(list18, "Footstep tiles run sound R", 11);
		List<AudioClip> list19 = new List<AudioClip>();
		this.LoadSoundArray(list19, "Footstep garvel run sound L", 11);
		List<AudioClip> list20 = new List<AudioClip>();
		this.LoadSoundArray(list20, "Footstep garvel run sound R", 11);
		WalkSoundManager._footDownSounds.Add("WoodplankRun", new List<List<AudioClip>> { list12, list12 });
		WalkSoundManager._footDownSounds.Add("SmallwoodRun", new List<List<AudioClip>> { list13, list14 });
		WalkSoundManager._footDownSounds.Add("ConcreteRun", new List<List<AudioClip>> { list15, list16 });
		WalkSoundManager._footDownSounds.Add("TilesRun", new List<List<AudioClip>> { list17, list18 });
		WalkSoundManager._footDownSounds.Add("GarvelRun", new List<List<AudioClip>> { list19, list20 });
		WalkSoundManager._footDownSounds.Add("InternetRun", new List<List<AudioClip>> { list10, list11 });
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x00028EC4 File Offset: 0x000270C4
	private void LoadSoundArray(List<AudioClip> pSoundList, string pName, int pNrOfSounds)
	{
		for (int i = 0; i < pNrOfSounds; i++)
		{
			string text = pName + i;
			AudioClip audioClip = (AudioClip)Resources.Load(text);
			if (audioClip == null)
			{
				Debug.Log("Failed to load walk sound clip: " + text);
			}
			else
			{
				pSoundList.Add(audioClip);
			}
		}
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x00028F24 File Offset: 0x00027124
	public void PlaySound(string pGroundMaterialName, WalkSoundManager.Foot pFoot, AudioSource pAudioSource)
	{
		List<AudioClip> list = WalkSoundManager._footDownSounds[pGroundMaterialName][(int)pFoot];
		if (list.Count == 0)
		{
			Debug.Log(string.Concat(new object[] { "Audio clip list for ", pFoot, " and ground material ", pGroundMaterialName, " was empty" }));
			return;
		}
		int num = this._playInSequence % list.Count;
		if (pFoot == WalkSoundManager.Foot.Left)
		{
			this._playInSequence++;
		}
		AudioClip audioClip = list[num];
		if (audioClip == null)
		{
		}
		pAudioSource.pitch = global::UnityEngine.Random.Range(1f, 1f);
		pAudioSource.clip = audioClip;
		pAudioSource.Play();
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x00028FE4 File Offset: 0x000271E4
	private int GetRandomSoundNr(int pNrOfSounds)
	{
		int num = global::UnityEngine.Random.Range(0, pNrOfSounds);
		if (num == this._lastIndex)
		{
			num = (this._lastIndex + 1) % pNrOfSounds;
		}
		this._lastIndex = num;
		return num;
	}

	// Token: 0x04000409 RID: 1033
	private static Dictionary<string, List<List<AudioClip>>> _footDownSounds;

	// Token: 0x0400040A RID: 1034
	private int _lastIndex = -1;

	// Token: 0x0400040B RID: 1035
	private int _playInSequence;

	// Token: 0x020000DD RID: 221
	public enum Foot
	{
		// Token: 0x0400040D RID: 1037
		Left,
		// Token: 0x0400040E RID: 1038
		Right
	}
}
