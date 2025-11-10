using System;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class Fade
{
	// Token: 0x06000465 RID: 1125 RVA: 0x0001EC60 File Offset: 0x0001CE60
	public Fade(Color pStartingColor)
	{
		this.SetColor(pStartingColor);
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000466 RID: 1126 RVA: 0x0001EC7C File Offset: 0x0001CE7C
	// (remove) Token: 0x06000467 RID: 1127 RVA: 0x0001EC98 File Offset: 0x0001CE98
	public event Fade.FadeCompleteEvent onFadedToTransparent;

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000468 RID: 1128 RVA: 0x0001ECB4 File Offset: 0x0001CEB4
	// (set) Token: 0x06000469 RID: 1129 RVA: 0x0001ECBC File Offset: 0x0001CEBC
	public float alpha { get; set; }

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600046B RID: 1131 RVA: 0x0001ECD4 File Offset: 0x0001CED4
	// (set) Token: 0x0600046A RID: 1130 RVA: 0x0001ECC8 File Offset: 0x0001CEC8
	public float speed
	{
		get
		{
			return this._speed;
		}
		set
		{
			this._speed = value;
		}
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0001ECDC File Offset: 0x0001CEDC
	public void SetColor(Color pStartingColor)
	{
		this._fadeMode = Fade.FadeMode.NONE;
		this._texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		this._texture.wrapMode = TextureWrapMode.Repeat;
		this._texture.SetPixel(0, 0, pStartingColor);
		this._texture.Apply();
		this._startAlpha = this.alpha;
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600046D RID: 1133 RVA: 0x0001ED30 File Offset: 0x0001CF30
	public Fade.FadeMode mode
	{
		get
		{
			return this._fadeMode;
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0001ED38 File Offset: 0x0001CF38
	public void Draw()
	{
		switch (this._fadeMode)
		{
		case Fade.FadeMode.OPAQUE:
			this.DrawCover();
			break;
		case Fade.FadeMode.FADING_TO_OPAQUE:
			this.UpdateFadeIn();
			break;
		case Fade.FadeMode.FADING_TO_TRANSPARENT:
			this.UpdateFadeOut();
			break;
		}
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0001ED9C File Offset: 0x0001CF9C
	public void Update(float dt)
	{
		this._iterator += this._speed * dt;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0001EDB4 File Offset: 0x0001CFB4
	public void FadeToColor(Color pColor)
	{
		this._iterator = 0f;
		this._targetColor = pColor;
		this._fadeMode = Fade.FadeMode.FADING_TO_OPAQUE;
		this._startAlpha = this.alpha;
		Debug.Log("Will fade to color: " + pColor);
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0001EDFC File Offset: 0x0001CFFC
	public void Black()
	{
		this.SetColor(Color.black);
		this._fadeMode = Fade.FadeMode.OPAQUE;
		this.alpha = 0f;
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0001EE1C File Offset: 0x0001D01C
	public void FadeToTransparent()
	{
		this._iterator = 0f;
		this._fadeMode = Fade.FadeMode.FADING_TO_TRANSPARENT;
		this._startAlpha = this.alpha;
		Debug.Log("Will fade to transparent");
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0001EE54 File Offset: 0x0001D054
	private void DrawCover()
	{
		GUI.DrawTexture(new Rect(0f, 0f, (float)(Screen.width + 1), (float)(Screen.height + 1)), this._texture, ScaleMode.StretchToFill);
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0001EE84 File Offset: 0x0001D084
	private void AdjustAlpha(float pAlpha)
	{
		Color targetColor = this._targetColor;
		targetColor.a = pAlpha * this._targetColor.a;
		this._texture.SetPixel(0, 0, targetColor);
		this._texture.Apply();
		this.alpha = pAlpha;
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0001EECC File Offset: 0x0001D0CC
	private void UpdateFadeIn()
	{
		if (this._iterator > 1f)
		{
			this._fadeMode = Fade.FadeMode.OPAQUE;
			this.AdjustAlpha(1f);
			if (this.onFadedToOpaque != null)
			{
				Debug.Log("onFadedToOpaque()");
				this.onFadedToOpaque();
				this.onFadedToOpaque = null;
			}
		}
		else
		{
			this.AdjustAlpha(iTween.easeOutQuad(this._startAlpha, 1f, this._iterator));
		}
		this.DrawCover();
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0001EF4C File Offset: 0x0001D14C
	private void UpdateFadeOut()
	{
		if (this._iterator > 1f)
		{
			this._fadeMode = Fade.FadeMode.TRANSPARENT;
			if (this.onFadedToTransparent != null)
			{
				Debug.Log("onFadedToTransparent()");
				this.onFadedToTransparent();
				this.onFadedToTransparent = null;
			}
		}
		else
		{
			this.AdjustAlpha(iTween.easeInQuad(this._startAlpha, 0f, this._iterator));
			this.DrawCover();
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000477 RID: 1143 RVA: 0x0001EFC0 File Offset: 0x0001D1C0
	public bool isFading
	{
		get
		{
			return this._fadeMode == Fade.FadeMode.FADING_TO_OPAQUE || this._fadeMode == Fade.FadeMode.FADING_TO_TRANSPARENT;
		}
	}

	// Token: 0x0400035E RID: 862
	public Fade.FadeCompleteEvent onFadedToOpaque;

	// Token: 0x0400035F RID: 863
	private Fade.FadeMode _fadeMode;

	// Token: 0x04000360 RID: 864
	private Texture2D _texture;

	// Token: 0x04000361 RID: 865
	private float _iterator;

	// Token: 0x04000362 RID: 866
	private Color _targetColor;

	// Token: 0x04000363 RID: 867
	public static Fade _instance;

	// Token: 0x04000364 RID: 868
	private float _speed = 2.5f;

	// Token: 0x04000365 RID: 869
	private float _startAlpha;

	// Token: 0x0200009F RID: 159
	public enum FadeMode
	{
		// Token: 0x04000369 RID: 873
		NONE,
		// Token: 0x0400036A RID: 874
		TRANSPARENT,
		// Token: 0x0400036B RID: 875
		OPAQUE,
		// Token: 0x0400036C RID: 876
		FADING_TO_OPAQUE,
		// Token: 0x0400036D RID: 877
		FADING_TO_TRANSPARENT
	}

	// Token: 0x02000101 RID: 257
	// (Invoke) Token: 0x06000763 RID: 1891
	public delegate void FadeCompleteEvent();
}
