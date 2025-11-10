using System;
using System.IO;
using GameTypes;
using UnityEngine;

// Token: 0x02000032 RID: 50
public static class MimanHelper
{
	// Token: 0x06000229 RID: 553 RVA: 0x00010B08 File Offset: 0x0000ED08
	public static Vector3 TilePositionToVector3(IntPoint pPosition)
	{
		return new Vector3((float)pPosition.x * 1f + 0.5f, 0f, (float)pPosition.y * 1f + 0.5f);
	}

	// Token: 0x0600022A RID: 554 RVA: 0x00010B48 File Offset: 0x0000ED48
	public static IntPoint Vector3ToTilePoint(Vector3 pPosition)
	{
		return new IntPoint(Mathf.RoundToInt((pPosition.x - 0.5f) / 1f), Mathf.RoundToInt((pPosition.z - 0.5f) / 1f));
	}

	// Token: 0x0600022B RID: 555 RVA: 0x00010B80 File Offset: 0x0000ED80
	public static Transform GetTransformWithNameRecursively(Transform pTransform, string pName)
	{
		foreach (object obj in pTransform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == pName)
			{
				return transform;
			}
			Transform transformWithNameRecursively = MimanHelper.GetTransformWithNameRecursively(transform, pName);
			if (transformWithNameRecursively != null)
			{
				return transformWithNameRecursively;
			}
		}
		return null;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x00010C1C File Offset: 0x0000EE1C
	public static Color ScaleColor(float r, float g, float b)
	{
		return new Color(MimanHelper.Scale(r), MimanHelper.Scale(g), MimanHelper.Scale(b));
	}

	// Token: 0x0600022D RID: 557 RVA: 0x00010C38 File Offset: 0x0000EE38
	private static float Scale(float p255Scale)
	{
		return p255Scale / 255f;
	}

	// Token: 0x0600022E RID: 558 RVA: 0x00010C44 File Offset: 0x0000EE44
	public static void TreeCopy(string pSource, string pDestination)
	{
		if (!Directory.Exists(pDestination))
		{
			Directory.CreateDirectory(pDestination);
		}
		string[] files = Directory.GetFiles(pSource);
		foreach (string text in files)
		{
			if (text.LastIndexOf(".meta") == -1)
			{
				try
				{
					File.Copy(text, pDestination + "/" + MimanHelper.RemovePath(text), true);
				}
				catch (Exception ex)
				{
					Debug.Log(string.Concat(new string[]
					{
						"problems copying..",
						pSource,
						" to ",
						pDestination,
						"/",
						MimanHelper.RemovePath(text)
					}));
					Debug.Log(ex);
				}
			}
		}
		string[] directories = Directory.GetDirectories(pSource);
		foreach (string text2 in directories)
		{
			if (text2.LastIndexOf(".svn") == -1)
			{
				MimanHelper.TreeCopy(text2, pDestination + "/" + MimanHelper.RemovePath(text2));
			}
		}
	}

	// Token: 0x0600022F RID: 559 RVA: 0x00010D78 File Offset: 0x0000EF78
	private static string RemovePath(string p)
	{
		int num = p.LastIndexOf("/");
		int num2 = p.LastIndexOf("\\");
		int num3 = num;
		if (num < num2)
		{
			num3 = num2;
		}
		if (num3 == -1)
		{
			return p;
		}
		return p.Substring(num3 + 1);
	}
}
