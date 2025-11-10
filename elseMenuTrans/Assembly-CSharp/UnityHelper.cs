using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class UnityHelper
{
	// Token: 0x06000234 RID: 564 RVA: 0x00010E80 File Offset: 0x0000F080
	public static Transform FindRecursive(Transform pBase, string pName)
	{
		foreach (object obj in pBase)
		{
			Transform transform = (Transform)obj;
			if (transform.name.ToLower() == pName.ToLower())
			{
				return transform;
			}
			if (transform.childCount > 0)
			{
				Transform transform2 = UnityHelper.FindRecursive(transform, pName);
				if (transform2 != null)
				{
					return transform2;
				}
			}
		}
		return null;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00010F34 File Offset: 0x0000F134
	public static bool Contains(List<SerializableKeyValuePair> pList, string pKey)
	{
		foreach (SerializableKeyValuePair serializableKeyValuePair in pList)
		{
			if (serializableKeyValuePair.Key == pKey)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x00010FAC File Offset: 0x0000F1AC
	public static void Set(List<SerializableKeyValuePair> pList, string pKey, float pValue)
	{
		foreach (SerializableKeyValuePair serializableKeyValuePair in pList)
		{
			if (serializableKeyValuePair.Key == pKey)
			{
				serializableKeyValuePair.Value = pValue;
				return;
			}
		}
		pList.Add(new SerializableKeyValuePair(pKey, pValue));
	}

	// Token: 0x06000237 RID: 567 RVA: 0x00011030 File Offset: 0x0000F230
	public static void Remove(List<SerializableKeyValuePair> pList, string pKey)
	{
		if (UnityHelper.Contains(pList, pKey))
		{
			pList.Remove(UnityHelper.GetPair(pList, pKey));
		}
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0001104C File Offset: 0x0000F24C
	public static float Get(List<SerializableKeyValuePair> pList, string pKey)
	{
		foreach (SerializableKeyValuePair serializableKeyValuePair in pList)
		{
			if (serializableKeyValuePair.Key == pKey)
			{
				return serializableKeyValuePair.Value;
			}
		}
		return -1f;
	}

	// Token: 0x06000239 RID: 569 RVA: 0x000110CC File Offset: 0x0000F2CC
	public static SerializableKeyValuePair GetPair(List<SerializableKeyValuePair> pList, string pKey)
	{
		foreach (SerializableKeyValuePair serializableKeyValuePair in pList)
		{
			if (serializableKeyValuePair.Key == pKey)
			{
				return serializableKeyValuePair;
			}
		}
		return null;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00011144 File Offset: 0x0000F344
	public static Color[] CreateColorQuad(Color pColor, int pWidth, int pHeight)
	{
		Color[] array = new Color[pWidth * pHeight];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = pColor;
		}
		return array;
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0001117C File Offset: 0x0000F37C
	public static string StripPath(string p)
	{
		return p.Substring(p.LastIndexOf('/') + 1);
	}

	// Token: 0x0600023C RID: 572 RVA: 0x00011190 File Offset: 0x0000F390
	public static string StripFiletype(string p)
	{
		int num = p.LastIndexOf(".");
		if (num != -1)
		{
			return p.Substring(0, num);
		}
		return p;
	}

	// Token: 0x0600023D RID: 573 RVA: 0x000111BC File Offset: 0x0000F3BC
	public static bool TryPlayOneShotAudio(GameObject g, string pClipName)
	{
		AudioSource component = g.GetComponent<AudioSource>();
		AudioClip audioClip = (AudioClip)Resources.Load(pClipName, typeof(AudioClip));
		if (component != null && audioClip != null)
		{
			component.PlayOneShot(audioClip);
			return true;
		}
		return false;
	}
}
