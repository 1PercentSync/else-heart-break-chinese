using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class Container
{
	// Token: 0x06000078 RID: 120 RVA: 0x000048A0 File Offset: 0x00002AA0
	public Container(Vector2 pPosition)
	{
		this.init(string.Empty, false, pPosition);
	}

	// Token: 0x06000079 RID: 121 RVA: 0x0000491C File Offset: 0x00002B1C
	public Container(string pTextureName, Vector2 pPosition)
	{
		this.init(pTextureName, true, pPosition);
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00004994 File Offset: 0x00002B94
	public Container(string pTextureName, Vector2 pPosition, Vector2 pTextureScale)
	{
		this.init(pTextureName, true, pPosition);
		this.TextureScale = pTextureScale;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00004A10 File Offset: 0x00002C10
	public Container(string pTextureName, Vector2 pPosition, Vector2 pTextureScale, Container.OnContainerPressed pOnContainerPressed)
	{
		this.init(pTextureName, true, pPosition);
		this._onContainerPressed = pOnContainerPressed;
		this.TextureScale = pTextureScale;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00004A94 File Offset: 0x00002C94
	public Container(string pTextureName, Vector2 pPosition, Container.OnContainerPressed pOnContainerPressed)
	{
		this.init(pTextureName, true, pPosition);
		this._onContainerPressed = pOnContainerPressed;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00004B10 File Offset: 0x00002D10
	public Container(Texture pTexture, Vector2 pPosition)
	{
		this._texture = pTexture;
		this._relativePosition = pPosition;
		this.RefreshScreenPosition(default(Vector2));
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600007F RID: 127 RVA: 0x00004BA8 File Offset: 0x00002DA8
	// (set) Token: 0x06000080 RID: 128 RVA: 0x00004BB0 File Offset: 0x00002DB0
	public bool selected
	{
		get
		{
			return this._selected;
		}
		set
		{
			this._selected = value;
		}
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00004BBC File Offset: 0x00002DBC
	public bool SetHovered()
	{
		if (this._hoverOverlay != null && !this.disabled && this.Visible && !this._selected)
		{
			this._hoverOverlayVisible = true;
			return true;
		}
		return false;
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00004C08 File Offset: 0x00002E08
	public void SetNotHovered()
	{
		foreach (Container container in this._children)
		{
			container.SetNotHovered();
		}
		if (this._hoverOverlay != null)
		{
			this._hoverOverlayVisible = false;
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00004C88 File Offset: 0x00002E88
	public void setHoverOverlay(string pTextureName, Vector2 pOffset)
	{
		this._hoverOverlay = (Texture)Resources.Load(pTextureName);
		this._hoverOverlayOffset = pOffset;
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000084 RID: 132 RVA: 0x00004CA4 File Offset: 0x00002EA4
	// (set) Token: 0x06000085 RID: 133 RVA: 0x00004CB4 File Offset: 0x00002EB4
	public float alpha
	{
		get
		{
			return this.tint.a;
		}
		set
		{
			this.tint = new Color(this.tint.r, this.tint.g, this.tint.b, value);
			foreach (Container container in this._children)
			{
				container.alpha = value;
			}
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00004D48 File Offset: 0x00002F48
	public void SetAlpha(float pAlpha)
	{
		this.alpha = pAlpha;
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00004D54 File Offset: 0x00002F54
	public void SetOnContainerPressedDelegate(Container.OnContainerPressed pDelegateFunction)
	{
		this._onContainerPressed = pDelegateFunction;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00004D60 File Offset: 0x00002F60
	public bool HasTag(string pTagName)
	{
		if (this._tags == null)
		{
			return false;
		}
		foreach (string text in this._tags)
		{
			if (text == pTagName)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00004DE4 File Offset: 0x00002FE4
	public void AddTag(string pTagName)
	{
		if (this._tags == null)
		{
			this._tags = new List<string>();
		}
		this._tags.Add(pTagName);
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x0600008B RID: 139 RVA: 0x00004E24 File Offset: 0x00003024
	// (set) Token: 0x0600008A RID: 138 RVA: 0x00004E14 File Offset: 0x00003014
	public Rect customHitbox
	{
		get
		{
			return this._customHitbox;
		}
		set
		{
			this._hasCustomHitbox = true;
			this._customHitbox = value;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x0600008C RID: 140 RVA: 0x00004E2C File Offset: 0x0000302C
	// (set) Token: 0x0600008D RID: 141 RVA: 0x00004E34 File Offset: 0x00003034
	public bool reactsToMouseClick
	{
		get
		{
			return this._reactsToMouseClick;
		}
		set
		{
			this._reactsToMouseClick = value;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x0600008E RID: 142 RVA: 0x00004E40 File Offset: 0x00003040
	// (set) Token: 0x0600008F RID: 143 RVA: 0x00004E48 File Offset: 0x00003048
	public Vector2 RelativePosition
	{
		get
		{
			return this._relativePosition;
		}
		set
		{
			this._relativePosition = value;
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00004E54 File Offset: 0x00003054
	public void SetRelativePosition(Vector2 pPosition)
	{
		this.RelativePosition = pPosition;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00004E60 File Offset: 0x00003060
	public void SetScreenPosition(Vector2 pPosition)
	{
		this.ScreenPosition = pPosition;
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000092 RID: 146 RVA: 0x00004E6C File Offset: 0x0000306C
	public List<Container> Children
	{
		get
		{
			return this._children;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000093 RID: 147 RVA: 0x00004E74 File Offset: 0x00003074
	// (set) Token: 0x06000094 RID: 148 RVA: 0x00004E7C File Offset: 0x0000307C
	public Vector2 ScreenPosition
	{
		get
		{
			return this._screenPosition;
		}
		set
		{
			this._screenPosition = value;
			foreach (Container container in this._children)
			{
				container.RefreshScreenPosition(this._screenPosition);
			}
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000095 RID: 149 RVA: 0x00004EF0 File Offset: 0x000030F0
	// (set) Token: 0x06000096 RID: 150 RVA: 0x00004EF8 File Offset: 0x000030F8
	public Vector2 TextureScale
	{
		get
		{
			return this._textureScale;
		}
		set
		{
			this._textureScale = value;
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00004F04 File Offset: 0x00003104
	private void init(string pTextureName, bool loadTexture, Vector2 pPosition)
	{
		this._name = pTextureName;
		if (loadTexture)
		{
			this._texture = (Texture)Resources.Load(pTextureName);
			if (this._texture == null)
			{
				throw new Exception("Can't load texture '" + pTextureName + "'");
			}
		}
		this._relativePosition = pPosition;
		this.RefreshScreenPosition(default(Vector2));
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00004F6C File Offset: 0x0000316C
	public void RefreshScreenPosition(Vector2 pParentScreenPosition)
	{
		this._screenPosition = pParentScreenPosition + this._relativePosition;
		foreach (Container container in this._children)
		{
			container.RefreshScreenPosition(this._screenPosition);
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00004FEC File Offset: 0x000031EC
	public void RefreshChildScreenPositions()
	{
		foreach (Container container in this._children)
		{
			container.RefreshScreenPosition(this._screenPosition);
		}
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00005058 File Offset: 0x00003258
	public virtual void PreDraw()
	{
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600009B RID: 155 RVA: 0x0000505C File Offset: 0x0000325C
	public Vector2 CalculatedScreenPosition
	{
		get
		{
			return this._screenPosition + Container.groupClippingOffset;
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00005070 File Offset: 0x00003270
	public virtual void Draw()
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
				GUI.color = new Color(this.colorWhenDisabled.r, this.colorWhenDisabled.g, this.colorWhenDisabled.b, this.colorWhenDisabled.a);
			}
			else
			{
				GUI.color = new Color(this.tint.r, this.tint.g, this.tint.b, this.tint.a);
			}
			GUI.DrawTexture(Scaled.Rectangle(this.CalculatedScreenPosition.x, this.CalculatedScreenPosition.y, (float)this._texture.width * this._textureScale.x, (float)this._texture.height * this._textureScale.y), this._texture);
		}
		foreach (Container container in this._children)
		{
			container.Draw();
		}
		if (this.disabled)
		{
			this.DrawDisabled();
		}
		this.PostDraw();
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000051E8 File Offset: 0x000033E8
	public virtual void Update()
	{
		foreach (Container container in this._children)
		{
			container.Update();
		}
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00005250 File Offset: 0x00003450
	public virtual void PostDraw()
	{
		if (this._hoverOverlay != null && this._hoverOverlayVisible)
		{
			GUI.color = Color.white;
			Rect rect = Scaled.Rectangle(this.CalculatedScreenPosition.x + this._hoverOverlayOffset.x, this.CalculatedScreenPosition.y + this._hoverOverlayOffset.y, (float)this._hoverOverlay.width, (float)this._hoverOverlay.height);
			GUI.DrawTexture(rect, this._hoverOverlay);
		}
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000052E4 File Offset: 0x000034E4
	public void DrawDisabled()
	{
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000052E8 File Offset: 0x000034E8
	public virtual void Pressed()
	{
		if (this._onContainerPressed != null && this.Visible && !this.disabled)
		{
			this._onContainerPressed(this);
		}
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00005318 File Offset: 0x00003518
	public virtual Container GetContainerAtPosition(Vector2 pPosition)
	{
		if (!this._reactsToMouseClick)
		{
			return null;
		}
		Container container = null;
		foreach (Container container2 in this._children)
		{
			Container containerAtPosition = container2.GetContainerAtPosition(pPosition);
			if (containerAtPosition != null)
			{
				container = containerAtPosition;
				break;
			}
		}
		if (container == null && this.Visible && this._texture != null && this.IsInside(new Rect(this._screenPosition.x, this._screenPosition.y, this.width, this.height), pPosition))
		{
			return this;
		}
		return container;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x000053F4 File Offset: 0x000035F4
	protected bool IsInside(Rect r, Vector2 pPosition)
	{
		return pPosition.x > r.x && pPosition.y > r.y && pPosition.x < r.x + r.width && pPosition.y < r.y + r.height;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00005460 File Offset: 0x00003660
	public virtual List<Container> GetContainersAtPosition(Vector2 pPosition)
	{
		List<Container> list = new List<Container>();
		foreach (Container container in this._children)
		{
			List<Container> containersAtPosition = container.GetContainersAtPosition(pPosition);
			list.AddRange(containersAtPosition);
		}
		if (this._reactsToMouseClick && this._texture != null && this.Visible && this.IsInside(new Rect(this._screenPosition.x, this._screenPosition.y, this.width, this.height), pPosition))
		{
			list.Add(this);
		}
		return list;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00005538 File Offset: 0x00003738
	public Container GetChildWithName(string pName)
	{
		foreach (Container container in this._children)
		{
			if (container.name == pName)
			{
				return container;
			}
		}
		throw new Exception("Couldn't find child " + pName + " in container " + this._name);
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x000055CC File Offset: 0x000037CC
	public virtual void AddChild(Container pContainer)
	{
		this._children.Add(pContainer);
		if (this.disabled)
		{
			pContainer.disabled = true;
		}
		if (!this.Visible)
		{
			pContainer.Visible = false;
		}
		pContainer.RefreshScreenPosition(this.ScreenPosition);
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00005618 File Offset: 0x00003818
	public void MoveToBack(Container pChild)
	{
		int num = this._children.IndexOf(pChild);
		if (num != -1)
		{
			this._children.RemoveAt(num);
			this._children.Insert(0, pChild);
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00005654 File Offset: 0x00003854
	public void MoveToFront(Container pChild)
	{
		int num = this._children.IndexOf(pChild);
		if (num != -1)
		{
			this._children.RemoveAt(num);
			this._children.Add(pChild);
		}
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00005690 File Offset: 0x00003890
	public virtual void RemoveChild(Container pContainer)
	{
		pContainer.RemoveChildren();
		this._children.Remove(pContainer);
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x000056A8 File Offset: 0x000038A8
	public virtual void RemoveChildren()
	{
		foreach (Container container in this._children)
		{
			container.RemoveChildren();
		}
		this._children.Clear();
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060000AA RID: 170 RVA: 0x00005718 File Offset: 0x00003918
	// (set) Token: 0x060000AB RID: 171 RVA: 0x00005720 File Offset: 0x00003920
	public bool disabled
	{
		get
		{
			return this._disabled;
		}
		set
		{
			this._disabled = value;
			if (this._disabled && this._hoverOverlay != null)
			{
				this._hoverOverlayVisible = false;
			}
		}
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00005758 File Offset: 0x00003958
	public Vector2 RelativePositionUnder(float pPaddingX, float pPaddingY)
	{
		return this._relativePosition + new Vector2(pPaddingX, this.height + pPaddingY);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00005774 File Offset: 0x00003974
	public Vector2 RelativePositionUnder(float pPaddingY)
	{
		return this.RelativePositionUnder(0f, pPaddingY);
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00005784 File Offset: 0x00003984
	public Vector2 RelativePositionUnder()
	{
		return this.RelativePositionUnder(0f, 0f);
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00005798 File Offset: 0x00003998
	public Vector2 RelativePositionRightOf(float pPaddingX, float pPaddingY)
	{
		return this._relativePosition + new Vector2(this.width + pPaddingX, pPaddingY);
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x000057B4 File Offset: 0x000039B4
	public Vector2 RelativePositionRightOf(float pPaddingX)
	{
		return this.RelativePositionRightOf(pPaddingX, 0f);
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000057C4 File Offset: 0x000039C4
	public Vector2 RelativePositionRightOf()
	{
		return this.RelativePositionRightOf(0f, 0f);
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x060000B2 RID: 178 RVA: 0x000057D8 File Offset: 0x000039D8
	// (set) Token: 0x060000B3 RID: 179 RVA: 0x000057E0 File Offset: 0x000039E0
	public bool Visible
	{
		get
		{
			return this._visible;
		}
		set
		{
			this._visible = value;
			foreach (Container container in this._children)
			{
				container.Visible = value;
			}
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x060000B4 RID: 180 RVA: 0x00005850 File Offset: 0x00003A50
	public float width
	{
		get
		{
			if (this._hasCustomHitbox)
			{
				return this._customHitbox.width;
			}
			float num = 0f;
			if (this._texture != null)
			{
				num = (float)this._texture.width * this._textureScale.x;
			}
			foreach (Container container in this._children)
			{
				float num2 = container.RelativePosition.x + container.width;
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x060000B5 RID: 181 RVA: 0x00005918 File Offset: 0x00003B18
	public float height
	{
		get
		{
			if (this._hasCustomHitbox)
			{
				return this._customHitbox.height;
			}
			float num = 0f;
			if (this._texture != null)
			{
				num = (float)this._texture.height * this._textureScale.y;
			}
			foreach (Container container in this._children)
			{
				float num2 = container.RelativePosition.y + container.height;
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060000B6 RID: 182 RVA: 0x000059E0 File Offset: 0x00003BE0
	public string name
	{
		get
		{
			return this._name;
		}
	}

	// Token: 0x0400005F RID: 95
	public static Vector2 groupClippingOffset = Vector2.zero;

	// Token: 0x04000060 RID: 96
	public string warningText = string.Empty;

	// Token: 0x04000061 RID: 97
	public Color colorWhenDisabled = new Color(0.1f, 1f, 1f, 0.5f);

	// Token: 0x04000062 RID: 98
	public Color tint = Color.white;

	// Token: 0x04000063 RID: 99
	protected List<Container> _children = new List<Container>();

	// Token: 0x04000064 RID: 100
	protected Texture _texture;

	// Token: 0x04000065 RID: 101
	protected Vector2 _relativePosition;

	// Token: 0x04000066 RID: 102
	protected Vector2 _screenPosition;

	// Token: 0x04000067 RID: 103
	protected string _name;

	// Token: 0x04000068 RID: 104
	protected bool _disabled;

	// Token: 0x04000069 RID: 105
	protected bool _visible = true;

	// Token: 0x0400006A RID: 106
	protected bool _hasCustomHitbox;

	// Token: 0x0400006B RID: 107
	protected Rect _customHitbox;

	// Token: 0x0400006C RID: 108
	protected bool _reactsToMouseClick = true;

	// Token: 0x0400006D RID: 109
	protected Texture _hoverOverlay;

	// Token: 0x0400006E RID: 110
	protected Vector2 _hoverOverlayOffset;

	// Token: 0x0400006F RID: 111
	protected bool _hoverOverlayVisible;

	// Token: 0x04000070 RID: 112
	protected bool _selected;

	// Token: 0x04000071 RID: 113
	private List<string> _tags;

	// Token: 0x04000072 RID: 114
	private Container.OnContainerPressed _onContainerPressed;

	// Token: 0x04000073 RID: 115
	private Vector2 _textureScale = Vector2.one;

	// Token: 0x020000F9 RID: 249
	// (Invoke) Token: 0x06000743 RID: 1859
	public delegate void OnContainerPressed(Container pContainer);
}
