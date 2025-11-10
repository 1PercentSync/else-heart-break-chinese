using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class BlinkingLeds : MonoBehaviour
{
	// Token: 0x060002E7 RID: 743 RVA: 0x00013B64 File Offset: 0x00011D64
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(global::UnityEngine.Random.Range(0f, 2f));
		if (this.colors.Length == 0)
		{
			this.colors = this.defaultColors;
		}
		Renderer renderer = base.GetComponent<Renderer>();
		this._mask = renderer.material.GetTexture("_Illum") as Texture2D;
		if (this._mask == null)
		{
			throw new Exception("Found no Illum property for material");
		}
		Texture2D original = renderer.material.GetTexture("_MainTex") as Texture2D;
		if (original == null)
		{
			throw new Exception("Found no MainTex property for material");
		}
		this._mainTexture = this.CopyTexture(original);
		renderer.material.SetTexture("_MainTex", this._mainTexture);
		base.StartCoroutine(this.Blink());
		yield break;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x00013B80 File Offset: 0x00011D80
	private Texture2D CopyTexture(Texture2D texture)
	{
		Texture2D texture2D = new Texture2D(texture.width, texture.height);
		texture2D.filterMode = FilterMode.Point;
		texture2D.SetPixels(texture.GetPixels());
		return texture2D;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00013BB4 File Offset: 0x00011DB4
	private IEnumerator Blink()
	{
		Dictionary<BlinkingLeds.Mode, Func<int, Color>> functionsForModes = new Dictionary<BlinkingLeds.Mode, Func<int, Color>>
		{
			{
				BlinkingLeds.Mode.NORMAL,
				new Func<int, Color>(this.DeterministicColor)
			},
			{
				BlinkingLeds.Mode.RANDOM,
				new Func<int, Color>(this.SemiRandomColor)
			}
		};
		for (;;)
		{
			this._randomNr = global::UnityEngine.Random.Range(0f, 1f);
			this.ForEachPixel(functionsForModes[this.mode]);
			this._blinkCounter++;
			yield return new WaitForSeconds(1f / this.rate);
		}
		yield break;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00013BD0 File Offset: 0x00011DD0
	private Color DeterministicColor(int pixelIndex)
	{
		return this.colors[(pixelIndex + this._blinkCounter) % this.colors.Length];
	}

	// Token: 0x060002EB RID: 747 RVA: 0x00013BF4 File Offset: 0x00011DF4
	private Color SemiRandomColor(int pixelIndex)
	{
		return this.colors[(int)(Math.Abs(Math.Sin((double)pixelIndex)) * 10000.0 * (double)this._randomNr) % this.colors.Length];
	}

	// Token: 0x060002EC RID: 748 RVA: 0x00013C3C File Offset: 0x00011E3C
	private void ForEachPixel(Func<int, Color> mutator)
	{
		int width = this._mask.width;
		int height = this._mask.height;
		int width2 = this._mainTexture.width;
		int height2 = this._mainTexture.height;
		float num = (float)width / (float)width2;
		float num2 = (float)height / (float)height2;
		for (int i = 0; i < height2; i++)
		{
			for (int j = 0; j < width2; j++)
			{
				int num3 = (int)((float)j * num);
				int num4 = (int)((float)i * num2);
				if (this._mask.GetPixel(num3, num4).a < 1f)
				{
					int num5 = num4 * width + num3;
					this._mainTexture.SetPixel(j, i, mutator(num5));
				}
			}
		}
		this._mainTexture.Apply();
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00013D14 File Offset: 0x00011F14
	private Color TweakColorRandomly(Color color, float amount)
	{
		return new Color(color.r + global::UnityEngine.Random.Range(-amount, amount), color.g + global::UnityEngine.Random.Range(-amount, amount), color.b + global::UnityEngine.Random.Range(-amount, amount));
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00013D4C File Offset: 0x00011F4C
	private Color RandomColor()
	{
		return new Color(global::UnityEngine.Random.Range(0f, 1f), global::UnityEngine.Random.Range(0f, 1f), global::UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x040001D4 RID: 468
	public BlinkingLeds.Mode mode;

	// Token: 0x040001D5 RID: 469
	[Range(0.01f, 10f)]
	public float rate = 3f;

	// Token: 0x040001D6 RID: 470
	public Color[] colors;

	// Token: 0x040001D7 RID: 471
	private Texture2D _mainTexture;

	// Token: 0x040001D8 RID: 472
	private Texture2D _mask;

	// Token: 0x040001D9 RID: 473
	private int _blinkCounter;

	// Token: 0x040001DA RID: 474
	private float _randomNr;

	// Token: 0x040001DB RID: 475
	private Color[] defaultColors = new Color[]
	{
		Color.cyan,
		Color.green,
		Color.yellow,
		Color.red
	};

	// Token: 0x0200004F RID: 79
	public enum Mode
	{
		// Token: 0x040001DD RID: 477
		NORMAL,
		// Token: 0x040001DE RID: 478
		RANDOM
	}
}
