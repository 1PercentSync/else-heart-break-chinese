using System;
using GameTypes;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class TextEditorSidebar : MonoBehaviour
{
	// Token: 0x060006BD RID: 1725 RVA: 0x0002BE3C File Offset: 0x0002A03C
	public void Start()
	{
		this._renderer = base.GetComponent<TerminalRenderer>();
		D.isNull(this._renderer, "Could not find renderer for TextEditorSidebar");
		this._renderer.SetRect(0, 0, 128, 256);
		for (int i = 0; i < TextEditorSidebar.demoText.Length; i++)
		{
			this._renderer.SetLine(i, TextEditorSidebar.demoText[i]);
		}
		this._renderer.ApplyTextChanges();
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x0002BEB4 File Offset: 0x0002A0B4
	public void Update()
	{
	}

	// Token: 0x0400047F RID: 1151
	private TerminalRenderer _renderer;

	// Token: 0x04000480 RID: 1152
	private static readonly string[] demoText = new string[]
	{
		"//Move the poltergeist to a position012345678901234567890123456789", "//returns true on successfull move012345678901234567890123456789", "//parameter x012345678901234567890123456789", "//parameter y012345678901234567890123456789", "bool walkTo( number pX, number pY )012345678901234567890123456789", "//Move the poltergeist to a position012345678901234567890123456789", "//returns true on successfull move012345678901234567890123456789", "//parameter x012345678901234567890123456789", "//parameter y012345678901234567890123456789", "//Move the poltergeist to a position012345678901234567890123456789",
		"//returns true on successfull move012345678901234567890123456789", "//parameter x012345678901234567890123456789", "//parameter y012345678901234567890123456789", "bool walkTo( number pX, number pY )012345678901234567890123456789", "//Move the poltergeist to a position012345678901234567890123456789", "//returns true on successfull move012345678901234567890123456789"
	};
}
