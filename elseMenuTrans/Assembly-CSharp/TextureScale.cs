using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class TextureScale : MonoBehaviour
{
	// Token: 0x0600001C RID: 28 RVA: 0x00002B64 File Offset: 0x00000D64
	private static Texture2D Scale(Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D texture2D = new Texture2D(targetWidth, targetHeight, source.format, true);
		Color[] pixels = texture2D.GetPixels(0);
		float num = 1f / (float)source.width * ((float)source.width / (float)targetWidth);
		float num2 = 1f / (float)source.height * ((float)source.height / (float)targetHeight);
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = source.GetPixelBilinear(num * ((float)i % (float)targetWidth), num2 * Mathf.Floor((float)(i / targetWidth)));
		}
		texture2D.SetPixels(pixels, 0);
		texture2D.Apply();
		return texture2D;
	}
}
