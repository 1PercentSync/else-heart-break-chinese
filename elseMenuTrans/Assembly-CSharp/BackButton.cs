using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class BackButton
{
	// Token: 0x060000C7 RID: 199 RVA: 0x00005E24 File Offset: 0x00004024
	public BackButton(EasyAnimateTwo pEasyAnimate)
	{
		this._easyAnimate = pEasyAnimate;
		this.ResetText();
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00005E44 File Offset: 0x00004044
	public void ResetText()
	{
		this.text = "Back [ESC]";
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00005E54 File Offset: 0x00004054
	public void Show()
	{
		this._easyAnimate.Register(this._backButtonX, "backbutton", new EasyAnimState<float>(0.5f, -this._width, 0f, new EasyAnimState<float>.InterpolationSampler(iTween.easeInCubic), delegate(float o)
		{
			this._backButtonX = o;
		}, null));
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00005EAC File Offset: 0x000040AC
	public void Hide()
	{
		this._easyAnimate.Register(this._backButtonX, "backbutton", new EasyAnimState<float>(0.5f, 0f, -this._width, new EasyAnimState<float>.InterpolationSampler(iTween.easeInCubic), delegate(float o)
		{
			this._backButtonX = o;
		}, null));
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00005F04 File Offset: 0x00004104
	public bool RenderAndMaybeGoBack()
	{
		this._width = GUI.skin.label.CalcSize(new GUIContent(this.text)).x;
		return GUI.Button(new Rect(this._backButtonX, 20f, this._width, 30f), this.text);
	}

	// Token: 0x0400007D RID: 125
	private EasyAnimateTwo _easyAnimate;

	// Token: 0x0400007E RID: 126
	private float _width = 124f;

	// Token: 0x0400007F RID: 127
	private float _backButtonX;

	// Token: 0x04000080 RID: 128
	public string text;
}
