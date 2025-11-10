using System;
using GameTypes;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class TerminalButton : TerminalWidget
{
	// Token: 0x06000705 RID: 1797 RVA: 0x0002D6C8 File Offset: 0x0002B8C8
	public TerminalButton(Rect pRect, TerminalRenderer pTerminalRenderer, Color pColor, string pText, WidgetEvent pOnPressed)
		: base(pRect, pTerminalRenderer, pColor)
	{
		this._text = pText;
		this._onPressed = pOnPressed;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x0002D6FC File Offset: 0x0002B8FC
	public override void Render()
	{
		Float3 float3color = this._float3color;
		if (this._isHovered)
		{
			float3color = new Float3(0f, 1f, 1f);
		}
		this._terminalRenderer.DrawGLLine(new IntPoint((int)this._rect.x, (int)this._rect.y), new IntPoint((int)this._rect.xMax, (int)this._rect.y), float3color);
		this._terminalRenderer.DrawGLLine(new IntPoint((int)this._rect.xMax, (int)this._rect.y), new IntPoint((int)this._rect.xMax, (int)this._rect.yMax), float3color);
		this._terminalRenderer.DrawGLLine(new IntPoint((int)this._rect.xMax, (int)this._rect.yMax), new IntPoint((int)this._rect.x, (int)this._rect.yMax), float3color);
		this._terminalRenderer.DrawGLLine(new IntPoint((int)this._rect.x, (int)this._rect.yMax), new IntPoint((int)this._rect.x, (int)this._rect.y), float3color);
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x0002D84C File Offset: 0x0002BA4C
	public override void RenderText()
	{
		this._terminalRenderer.UseColor(this._color);
		this._terminalRenderer.SetStringAtPos((int)((this._rect.x + this._leftMargin) / 8f), (int)((this._rect.y + this._topMargin) / 8f), this._text);
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x0002D8B0 File Offset: 0x0002BAB0
	public override bool MouseMoved(Vector2 pMousePosition)
	{
		if (this._rect.Contains(pMousePosition))
		{
			this._isHovered = true;
			return true;
		}
		this._isHovered = false;
		return false;
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0002D8E0 File Offset: 0x0002BAE0
	public override bool MousePressed(Vector2 pMousePosition)
	{
		if (this._rect.Contains(pMousePosition) && this._onPressed != null)
		{
			this._onPressed(this);
			return true;
		}
		return false;
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x0600070A RID: 1802 RVA: 0x0002D910 File Offset: 0x0002BB10
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0002D918 File Offset: 0x0002BB18
	public override string ToString()
	{
		return string.Format("{0} at {1}", this.text, this._rect);
	}

	// Token: 0x040004A0 RID: 1184
	private WidgetEvent _onPressed;

	// Token: 0x040004A1 RID: 1185
	private string _text;

	// Token: 0x040004A2 RID: 1186
	private float _leftMargin = 5f;

	// Token: 0x040004A3 RID: 1187
	private float _topMargin = 8f;

	// Token: 0x040004A4 RID: 1188
	private bool _isHovered;
}
