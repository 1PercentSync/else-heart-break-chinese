using System;
using System.Collections.Generic;
using GameTypes;
using GrimmLib;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class CommandLine
{
	// Token: 0x06000725 RID: 1829 RVA: 0x0002E6C4 File Offset: 0x0002C8C4
	public void OnGUI()
	{
		GUI.skin = null;
		GUI.color = Color.white;
		bool flag = false;
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Command: ", new GUILayoutOption[0]);
		bool flag2 = false;
		if (Event.current.Equals(Event.KeyboardEvent("return")))
		{
			flag = true;
		}
		else if (Event.current.Equals(Event.KeyboardEvent("up")))
		{
			this._currentHistory--;
			flag2 = true;
		}
		else if (Event.current.Equals(Event.KeyboardEvent("down")))
		{
			this._currentHistory++;
			flag2 = true;
		}
		if (this._currentHistory < 0)
		{
			this._currentHistory = this._history.Count - 1;
		}
		else if (this._currentHistory > this._history.Count - 1)
		{
			this._currentHistory = 0;
		}
		if (this._history.Count > 0 && flag2)
		{
			this._command = this._history[this._currentHistory];
		}
		this._command = GUILayout.TextField(this._command, new GUILayoutOption[] { GUILayout.Width(400f) });
		if (GUILayout.Button("Run", new GUILayoutOption[0]))
		{
			flag = true;
		}
		if (flag)
		{
			try
			{
				this.dialogueRunner.RunStringAsFunction(this._command);
				D.Log("Command \"" + this._command + "\" was executed successfully");
				this._history.Add(this._command);
				this._currentHistory = this._history.Count - 1;
				this._command = string.Empty;
			}
			catch (Exception ex)
			{
				D.Log("Error in command: " + ex.Message + ", stack trace:\n" + ex.StackTrace);
			}
		}
		GUILayout.EndHorizontal();
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06000726 RID: 1830 RVA: 0x0002E8D0 File Offset: 0x0002CAD0
	private DialogueRunner dialogueRunner
	{
		get
		{
			return WorldOwner.instance.world.dialogueRunner;
		}
	}

	// Token: 0x040004C3 RID: 1219
	private string _command = string.Empty;

	// Token: 0x040004C4 RID: 1220
	private List<string> _history = new List<string>();

	// Token: 0x040004C5 RID: 1221
	private int _currentHistory;
}
