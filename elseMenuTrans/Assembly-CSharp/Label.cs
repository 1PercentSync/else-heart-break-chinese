using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class Label : Container
{
	// Token: 0x060000B7 RID: 183 RVA: 0x000059E8 File Offset: 0x00003BE8
	public Label(string pTextureName, Vector2 pPosition)
		: base(pTextureName, pPosition)
	{
		this._label = null;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00005A10 File Offset: 0x00003C10
	public Label(string pTextureName, string pLabel, Vector2 pPosition)
		: base(pTextureName, pPosition)
	{
		this._label = pLabel;
		this._skin = (GUISkin)Resources.Load("DefaultSkin");
		if (this._skin == null)
		{
			throw new Exception("Can't find skin");
		}
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00005A74 File Offset: 0x00003C74
	public Label(string pTextureName, string pLabel, Vector2 pPosition, string pSkinName)
		: base(pTextureName, pPosition)
	{
		this._label = pLabel;
		this._skin = (GUISkin)Resources.Load(pSkinName);
		if (this._skin == null)
		{
			throw new Exception("Can't find skin");
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x060000BB RID: 187 RVA: 0x00005AEC File Offset: 0x00003CEC
	// (set) Token: 0x060000BA RID: 186 RVA: 0x00005AD4 File Offset: 0x00003CD4
	public bool highlighted
	{
		get
		{
			return this._highlighted;
		}
		set
		{
			this._highlighted = value;
			this._overlayWhenHighlighted.Visible = value;
		}
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00005AF4 File Offset: 0x00003CF4
	public void SetOverlayForWhenHighlighted(string pTextureName)
	{
		this._overlayWhenHighlighted = new Container(pTextureName, new Vector2(0f, 0f));
		this.AddChild(this._overlayWhenHighlighted);
		this._overlayWhenHighlighted.Visible = false;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00005B2C File Offset: 0x00003D2C
	public override void Draw()
	{
		if (!this._visible)
		{
			return;
		}
		this.PreDraw();
		if (this._texture != null)
		{
			if (this._disabled)
			{
				GUI.color = new Color(0.1f, 1f, 1f, 0.5f);
			}
			else
			{
				GUI.color = this.tint;
			}
			Rect rect = new Rect(base.CalculatedScreenPosition.x * Scaled.Factor, base.CalculatedScreenPosition.y * Scaled.Factor, (float)this._texture.width * Scaled.Factor, (float)this._texture.height * Scaled.Factor);
			GUI.DrawTexture(rect, this._texture);
			if (this._label != null)
			{
				Rect rect2 = Scaled.Rectangle(base.CalculatedScreenPosition.x + this.textMargin, base.CalculatedScreenPosition.y + this.textMargin, base.width, base.height);
				this._skin.label.alignment = this.textAlignment;
				GUI.skin = this._skin;
				GUI.color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, this.tint.a);
				GUI.Label(rect2, this._label);
			}
		}
		foreach (Container container in this._children)
		{
			container.Draw();
		}
		this.PostDraw();
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x060000BF RID: 191 RVA: 0x00005D0C File Offset: 0x00003F0C
	// (set) Token: 0x060000BE RID: 190 RVA: 0x00005D00 File Offset: 0x00003F00
	public string text
	{
		get
		{
			return this._label;
		}
		set
		{
			this._label = value;
		}
	}

	// Token: 0x04000074 RID: 116
	private string _label;

	// Token: 0x04000075 RID: 117
	private bool _highlighted;

	// Token: 0x04000076 RID: 118
	private GUISkin _skin;

	// Token: 0x04000077 RID: 119
	public Color textColor = Color.black;

	// Token: 0x04000078 RID: 120
	public float textMargin = 5f;

	// Token: 0x04000079 RID: 121
	private Container _overlayWhenHighlighted;

	// Token: 0x0400007A RID: 122
	public TextAnchor textAlignment;
}
