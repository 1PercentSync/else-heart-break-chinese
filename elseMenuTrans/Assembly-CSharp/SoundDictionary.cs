using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class SoundDictionary
{
	// Token: 0x0600063B RID: 1595 RVA: 0x00028938 File Offset: 0x00026B38
	public static void LoadSingleSound(string pKey, string pAudioName)
	{
		if (SoundDictionary._animationSounds.ContainsKey(pKey))
		{
			return;
		}
		SoundDictionary._animationSounds.Add(pKey, new AudioClip[] { (AudioClip)Resources.Load(pAudioName) });
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00028978 File Offset: 0x00026B78
	public static void LoadMultiSound(string pKey, string pAudioBaseName, int pSoundCount)
	{
		if (SoundDictionary._animationSounds.ContainsKey(pKey))
		{
			return;
		}
		AudioClip[] array = new AudioClip[pSoundCount];
		for (int i = 0; i < pSoundCount; i++)
		{
			string text = pAudioBaseName + " " + i;
			AudioClip audioClip = (AudioClip)Resources.Load(text);
			if (audioClip == null)
			{
				throw new Exception("Can't find multi sound with name '" + text);
			}
			array[i] = audioClip;
		}
		SoundDictionary._animationSounds.Add(pKey, array);
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x000289FC File Offset: 0x00026BFC
	private static AudioClip GetRandomClip(string pKey, int pIndex)
	{
		if (!SoundDictionary._animationSounds.ContainsKey(pKey))
		{
			return null;
		}
		AudioClip[] array = SoundDictionary._animationSounds[pKey];
		if (pIndex < 0)
		{
			pIndex = global::UnityEngine.Random.Range(0, array.Length);
		}
		if (pIndex < 0 || pIndex >= array.Length)
		{
			Debug.LogError(string.Concat(new object[] { "Trying to get clip with key ", pKey, " and index ", pIndex }));
			return null;
		}
		return array[pIndex];
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x00028A7C File Offset: 0x00026C7C
	public static void PlaySound(string pKey, AudioSource pAudioSource, int pIndex, float pDelay)
	{
		AudioClip randomClip = SoundDictionary.GetRandomClip(pKey, pIndex);
		if (randomClip == null)
		{
			pAudioSource.clip = null;
			return;
		}
		pAudioSource.clip = randomClip;
		pAudioSource.PlayDelayed(pDelay);
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00028AB4 File Offset: 0x00026CB4
	public static void PlaySound(string pKey, AudioSource pAudioSource)
	{
		SoundDictionary.PlaySound(pKey, pAudioSource, -1, 0f);
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x00028AC4 File Offset: 0x00026CC4
	public static void PlaySoundDelayed(string pKey, AudioSource pAudioSource, float pDelay)
	{
		SoundDictionary.PlaySound(pKey, pAudioSource, -1, pDelay);
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x00028AD0 File Offset: 0x00026CD0
	public static void PlaySoundOneShot(string pKey, AudioSource pAudioSource)
	{
		AudioClip randomClip = SoundDictionary.GetRandomClip(pKey, -1);
		pAudioSource.PlayOneShot(randomClip);
	}

	// Token: 0x04000408 RID: 1032
	private static Dictionary<string, AudioClip[]> _animationSounds = new Dictionary<string, AudioClip[]>();
}
