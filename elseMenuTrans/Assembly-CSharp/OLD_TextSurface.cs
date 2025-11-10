using System;
using GameTypes;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class OLD_TextSurface : MonoBehaviour
{
	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000685 RID: 1669 RVA: 0x0002ADAC File Offset: 0x00028FAC
	public int RowCount
	{
		get
		{
			return this._TargetRowCount;
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000686 RID: 1670 RVA: 0x0002ADB4 File Offset: 0x00028FB4
	public int CollumCount
	{
		get
		{
			return this._TargetCollumCount;
		}
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x0002ADBC File Offset: 0x00028FBC
	public void SetRect(int pX, int pY, int pWidth, int pHeight)
	{
		this._x = pX;
		this._y = pY;
		this._width = pWidth;
		this._height = pHeight;
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000688 RID: 1672 RVA: 0x0002ADDC File Offset: 0x00028FDC
	public int Width
	{
		get
		{
			return this._width;
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000689 RID: 1673 RVA: 0x0002ADE4 File Offset: 0x00028FE4
	public int Height
	{
		get
		{
			return this._height;
		}
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x0002ADEC File Offset: 0x00028FEC
	private void Awake()
	{
		this._FontSheetRowCount = (int)Mathf.Round(base.renderer.material.GetFloat("_FontSheetRowCount"));
		this._FontSheetCollumCount = (int)Mathf.Round(base.renderer.material.GetFloat("_FontSheetCollumCount"));
		this._TargetRowCount = (int)Mathf.Round(base.renderer.material.GetFloat("_TargetRowCount"));
		this._TargetCollumCount = (int)Mathf.Round(base.renderer.material.GetFloat("_TargetCollumCount"));
		this._fontControlTexture = new Texture2D(this._TargetCollumCount, this._TargetRowCount, TextureFormat.RGB24, false);
		this._fontControlTexture.wrapMode = TextureWrapMode.Clamp;
		this._fontControlTexture.filterMode = FilterMode.Point;
		this._fontControlTexture.anisoLevel = 0;
		this.oneHalfPixel = 1f / (float)base.renderer.material.GetTexture("_FontSheet").width;
		base.renderer.sharedMaterial.SetTexture("_LetterControlMap", this._fontControlTexture);
		this._canvasCount = this._TargetCollumCount * this._TargetRowCount;
		this.Clear();
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x0002AF18 File Offset: 0x00029118
	public void Update()
	{
		this.offset += Vector2.up * Time.deltaTime * 0.7f;
		base.renderer.material.SetTextureOffset("_GlowTex", this.offset);
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x0002AF6C File Offset: 0x0002916C
	public void Clear()
	{
		Color[] array = new Color[this._canvasCount];
		for (int i = 0; i < this._canvasCount; i++)
		{
			array[i] = this.GenerateColorFromGlyph(0);
		}
		this._fontControlTexture.SetPixels(array);
		this._fontControlTexture.Apply();
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x0002AFC8 File Offset: 0x000291C8
	public void SetCharacter(int pX, int pY, int character)
	{
		if (pX >= this.Width || pX < 0 || pY >= this.Height || pY < 0)
		{
			D.LogError("writing char outside borders!!");
		}
		this._fontControlTexture.SetPixel(this._x + pX, this._TargetRowCount - 1 - (this._y + pY), this.GenerateColorFromGlyph(character));
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x0002B030 File Offset: 0x00029230
	public void UseColor(float pColor)
	{
		this._color = pColor;
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0002B03C File Offset: 0x0002923C
	public void SetLine(int pY, string pText)
	{
		if (pText == null)
		{
			Debug.Log("text is null");
			return;
		}
		int num = 0;
		while (num < pText.Length && num < this.Width)
		{
			this.SetCharacter(num, pY, (int)pText[num]);
			num++;
		}
		for (int i = pText.Length; i < this.Width; i++)
		{
			this.SetCharacter(i, pY, 32);
		}
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0002B0B4 File Offset: 0x000292B4
	public void Apply()
	{
		this._fontControlTexture.Apply();
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0002B0C4 File Offset: 0x000292C4
	private Color GenerateColorFromGlyph(int character)
	{
		int num = character % this._FontSheetCollumCount;
		int num2 = character / this._FontSheetCollumCount + 1;
		float num3 = (((float)num <= 0f) ? 0f : ((float)num / (float)this._FontSheetCollumCount));
		float num4 = (((float)num2 <= 0f) ? 0f : ((float)num2 / (float)this._FontSheetRowCount));
		num3 += this.oneHalfPixel;
		num4 += this.oneHalfPixel;
		return new Color(num3, 1f - num4, this._color);
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x0002B14C File Offset: 0x0002934C
	private int GenerateCharFromColor(Color pColor)
	{
		return Mathf.RoundToInt((pColor.g - this.oneHalfPixel) * (float)this._FontSheetRowCount + (pColor.b - this.oneHalfPixel) * (float)this._FontSheetCollumCount);
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x0002B18C File Offset: 0x0002938C
	public void SetSingleChar(int pX, int pY, int pChar, float pColor)
	{
		float color = this._color;
		this._color = pColor;
		this.SetCharacter(pX, pY, pChar);
		this._color = color;
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0002B1B8 File Offset: 0x000293B8
	public int GetChar(int pX, int pY)
	{
		return this.GenerateCharFromColor(this._fontControlTexture.GetPixel(this._x + pX, this._TargetRowCount - 1 - (this._y + pY)));
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0002B1F0 File Offset: 0x000293F0
	public float GetColor(int pX, int pY)
	{
		return this._fontControlTexture.GetPixel(this._x + pX, this._TargetRowCount - 1 - (this._y + pY)).b;
	}

	// Token: 0x0400044B RID: 1099
	public const float BLACK = 0f;

	// Token: 0x0400044C RID: 1100
	public const float RED = 0.95f;

	// Token: 0x0400044D RID: 1101
	public const float GREEN = 0.85f;

	// Token: 0x0400044E RID: 1102
	public const float BLUE = 0.75f;

	// Token: 0x0400044F RID: 1103
	public const float YELLOW = 0.65f;

	// Token: 0x04000450 RID: 1104
	public const float GRAY = 0.55f;

	// Token: 0x04000451 RID: 1105
	public const float PURPLE = 0.45f;

	// Token: 0x04000452 RID: 1106
	public const float SELECTED = 0.15f;

	// Token: 0x04000453 RID: 1107
	private int _x;

	// Token: 0x04000454 RID: 1108
	private int _y;

	// Token: 0x04000455 RID: 1109
	private int _width;

	// Token: 0x04000456 RID: 1110
	private int _height;

	// Token: 0x04000457 RID: 1111
	private int _FontSheetRowCount;

	// Token: 0x04000458 RID: 1112
	private int _FontSheetCollumCount;

	// Token: 0x04000459 RID: 1113
	private int _TargetRowCount;

	// Token: 0x0400045A RID: 1114
	private int _TargetCollumCount;

	// Token: 0x0400045B RID: 1115
	private int _canvasCount;

	// Token: 0x0400045C RID: 1116
	private Texture2D _fontControlTexture;

	// Token: 0x0400045D RID: 1117
	private float oneHalfPixel = 0.00048828125f;

	// Token: 0x0400045E RID: 1118
	private Vector2 offset;

	// Token: 0x0400045F RID: 1119
	public float _color;
}
