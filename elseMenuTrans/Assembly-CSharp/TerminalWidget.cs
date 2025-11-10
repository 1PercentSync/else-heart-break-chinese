using System;
using GameTypes;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class TerminalWidget
{
	// Token: 0x06000700 RID: 1792 RVA: 0x0002D664 File Offset: 0x0002B864
	public TerminalWidget(Rect pRect, TerminalRenderer pTerminalRenderer, Color pColor)
	{
		this._rect = pRect;
		this._terminalRenderer = pTerminalRenderer;
		this._color = pColor;
		this._float3color = new Float3(this._color.r, this._color.g, this._color.b);
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x0002D6B8 File Offset: 0x0002B8B8
	public virtual void Render()
	{
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x0002D6BC File Offset: 0x0002B8BC
	public virtual void RenderText()
	{
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x0002D6C0 File Offset: 0x0002B8C0
	public virtual bool MousePressed(Vector2 pMousePosition)
	{
		return false;
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x0002D6C4 File Offset: 0x0002B8C4
	public virtual bool MouseMoved(Vector2 pMousePosition)
	{
		return false;
	}

	// Token: 0x0400049C RID: 1180
	protected Rect _rect;

	// Token: 0x0400049D RID: 1181
	protected TerminalRenderer _terminalRenderer;

	// Token: 0x0400049E RID: 1182
	protected Color _color;

	// Token: 0x0400049F RID: 1183
	protected Float3 _float3color;
}
