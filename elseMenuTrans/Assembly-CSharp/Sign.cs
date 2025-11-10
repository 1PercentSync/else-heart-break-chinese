using System;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class Sign : MonoBehaviour
{
	// Token: 0x06000430 RID: 1072 RVA: 0x0001DE04 File Offset: 0x0001C004
	private void Start()
	{
		this._tooltipTexture = Resources.Load("ToolTipColor2_NOSCALE") as Texture;
		this._skin = Resources.Load("TooltipSkin") as GUISkin;
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0001DE3C File Offset: 0x0001C03C
	private void OnMouseEnter()
	{
		this._showText = true;
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x0001DE48 File Offset: 0x0001C048
	private void OnMouseExit()
	{
		this._showText = false;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x0001DE54 File Offset: 0x0001C054
	private void OnGUI()
	{
		if (!this._showText)
		{
			return;
		}
		GUI.skin = this._skin;
		Vector2 vector = this.AnchorPointOnScreen();
		GUI.color = new Color(1f, 1f, 1f, 1f);
		Rect rect = new Rect(vector.x - 75f, vector.y, 200f, 30f);
		GUI.DrawTexture(rect, this._tooltipTexture);
		GUILayout.BeginArea(rect);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(new GUIContent(this.text), new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x0001DF00 File Offset: 0x0001C100
	private Vector2 AnchorPointOnScreen()
	{
		Vector3 vector = Camera.main.WorldToScreenPoint(base.transform.position + new Vector3(0f, 3f, 0f));
		return new Vector2(vector.x, (float)Screen.height - vector.y);
	}

	// Token: 0x04000336 RID: 822
	public string text = "Somewhere";

	// Token: 0x04000337 RID: 823
	private bool _showText;

	// Token: 0x04000338 RID: 824
	private Texture _tooltipTexture;

	// Token: 0x04000339 RID: 825
	private GUISkin _skin;
}
