using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class DebugOut : MonoBehaviour
{
	// Token: 0x06000206 RID: 518 RVA: 0x0000F67C File Offset: 0x0000D87C
	private void Start()
	{
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000F680 File Offset: 0x0000D880
	private void Awake()
	{
		Application.RegisterLogCallback(new Application.LogCallback(this.OnLogEvent));
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000F694 File Offset: 0x0000D894
	private void OnGUI()
	{
		this._scrollPos = GUILayout.BeginScrollView(this._scrollPos, new GUILayoutOption[0]);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		foreach (string text in this._messages)
		{
			GUILayout.Label(text, new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000F72C File Offset: 0x0000D92C
	private void OnLogEvent(string pMessage, string pStackTrace, LogType pType)
	{
		this._messages.Add(pType.ToString() + ":" + pMessage);
	}

	// Token: 0x04000155 RID: 341
	private Vector2 _scrollPos;

	// Token: 0x04000156 RID: 342
	private List<string> _messages = new List<string>();
}
