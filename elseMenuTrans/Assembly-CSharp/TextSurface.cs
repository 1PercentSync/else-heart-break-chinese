using System;
using GameTypes;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class TextSurface : MonoBehaviour
{
	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x060006C0 RID: 1728 RVA: 0x0002BECC File Offset: 0x0002A0CC
	public int RowCount
	{
		get
		{
			return this._TargetRowCount;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0002BED4 File Offset: 0x0002A0D4
	public int CollumCount
	{
		get
		{
			return this._TargetCollumCount;
		}
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x0002BEDC File Offset: 0x0002A0DC
	public void SetRect(int pX, int pY, int pWidth, int pHeight)
	{
		this._x = pX;
		this._y = pY;
		this._width = pWidth;
		this._height = pHeight;
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0002BEFC File Offset: 0x0002A0FC
	public int Width
	{
		get
		{
			return this._width;
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0002BF04 File Offset: 0x0002A104
	public int Height
	{
		get
		{
			return this._height;
		}
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0002BF0C File Offset: 0x0002A10C
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

	// Token: 0x060006C6 RID: 1734 RVA: 0x0002C038 File Offset: 0x0002A238
	public void Update()
	{
		this.offset += Vector2.up * Time.deltaTime * 0.7f;
		base.renderer.material.SetTextureOffset("_GlowTex", this.offset);
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x0002C08C File Offset: 0x0002A28C
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

	// Token: 0x060006C8 RID: 1736 RVA: 0x0002C0E8 File Offset: 0x0002A2E8
	public void SetCharacter(int pX, int pY, int character)
	{
		if (pX >= this.Width || pX < 0 || pY >= this.Height || pY < 0)
		{
			D.LogError("writing char outside borders!!");
		}
		this._fontControlTexture.SetPixel(this._x + pX, this._TargetRowCount - 1 - (this._y + pY), this.GenerateColorFromGlyph(character));
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x0002C150 File Offset: 0x0002A350
	public void UseColor(float pColor)
	{
		this._color = pColor;
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x0002C15C File Offset: 0x0002A35C
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

	// Token: 0x060006CB RID: 1739 RVA: 0x0002C1D4 File Offset: 0x0002A3D4
	public void Apply()
	{
		this._fontControlTexture.Apply();
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x0002C1E4 File Offset: 0x0002A3E4
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

	// Token: 0x060006CD RID: 1741 RVA: 0x0002C26C File Offset: 0x0002A46C
	private int GenerateCharFromColor(Color pColor)
	{
		return Mathf.RoundToInt((pColor.g - this.oneHalfPixel) * (float)this._FontSheetRowCount + (pColor.b - this.oneHalfPixel) * (float)this._FontSheetCollumCount);
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x0002C2AC File Offset: 0x0002A4AC
	public void SetSingleChar(int pX, int pY, int pChar, float pColor)
	{
		float color = this._color;
		this._color = pColor;
		this.SetCharacter(pX, pY, pChar);
		this._color = color;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x0002C2D8 File Offset: 0x0002A4D8
	public int GetChar(int pX, int pY)
	{
		return this.GenerateCharFromColor(this._fontControlTexture.GetPixel(this._x + pX, this._TargetRowCount - 1 - (this._y + pY)));
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x0002C310 File Offset: 0x0002A510
	public float GetColor(int pX, int pY)
	{
		return this._fontControlTexture.GetPixel(this._x + pX, this._TargetRowCount - 1 - (this._y + pY)).b;
	}

	// Token: 0x04000481 RID: 1153
	public const float BLACK = 0f;

	// Token: 0x04000482 RID: 1154
	public const float RED = 0.95f;

	// Token: 0x04000483 RID: 1155
	public const float GREEN = 0.85f;

	// Token: 0x04000484 RID: 1156
	public const float BLUE = 0.75f;

	// Token: 0x04000485 RID: 1157
	public const float YELLOW = 0.65f;

	// Token: 0x04000486 RID: 1158
	public const float GRAY = 0.55f;

	// Token: 0x04000487 RID: 1159
	public const float PURPLE = 0.45f;

	// Token: 0x04000488 RID: 1160
	public const float SELECTED = 0.15f;

	// Token: 0x04000489 RID: 1161
	private int _x;

	// Token: 0x0400048A RID: 1162
	private int _y;

	// Token: 0x0400048B RID: 1163
	private int _width;

	// Token: 0x0400048C RID: 1164
	private int _height;

	// Token: 0x0400048D RID: 1165
	private int _FontSheetRowCount;

	// Token: 0x0400048E RID: 1166
	private int _FontSheetCollumCount;

	// Token: 0x0400048F RID: 1167
	private int _TargetRowCount;

	// Token: 0x04000490 RID: 1168
	private int _TargetCollumCount;

	// Token: 0x04000491 RID: 1169
	private int _canvasCount;

	// Token: 0x04000492 RID: 1170
	private Texture2D _fontControlTexture;

	// Token: 0x04000493 RID: 1171
	private float oneHalfPixel = 0.00048828125f;

	// Token: 0x04000494 RID: 1172
	private Vector2 offset;

	// Token: 0x04000495 RID: 1173
	public float _color;
}
