using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class KeyboardInput
{
	// Token: 0x0600025F RID: 607 RVA: 0x00011510 File Offset: 0x0000F710
	public static void Update(float pDeltaTime)
	{
		KeyboardInput._repeatTimeout -= pDeltaTime;
		KeyboardInput._newlyPressed = KeyboardInput.ControlButtons.NONE;
		if (Input.GetKey(KeyCode.Delete))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.DELETE;
		}
		if (Input.GetKey(KeyCode.LeftAlt))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.LEFT_ALT;
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.LEFT_SHIFT;
		}
		if (Input.GetKey(KeyCode.LeftControl))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.LEFT_CTRL;
		}
		if (Input.GetKey(KeyCode.RightShift))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.RIGHT_SHIFT;
		}
		if (Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.AltGr))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.RIGHT_ALT;
		}
		if (Input.GetKey(KeyCode.RightControl))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.RIGHT_CTRL;
		}
		if (Input.GetKey(KeyCode.H))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.KEY_H;
		}
		if (Input.GetKey(KeyCode.Tab))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.TAB;
		}
		if (Input.GetKey(KeyCode.Backspace))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.BACKSPACE;
		}
		if (Input.GetKey(KeyCode.Return))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.ENTER;
		}
		if (Input.GetKey(KeyCode.Escape))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.ESC;
		}
		if (Input.GetKey(KeyCode.End))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.END;
		}
		if (Input.GetKey(KeyCode.Home))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.HOME;
		}
		if (Input.GetKey(KeyCode.PageDown))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.PGDWN;
		}
		if (Input.GetKey(KeyCode.PageUp))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.PGUP;
		}
		if (Input.GetKey(KeyCode.Insert))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.INSERT;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.LEFT_ARROW;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.UP_ARROW;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.RIGHT_ARROW;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.DOWN_ARROW;
		}
		if (Input.GetKey(KeyCode.B))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.KEY_B;
		}
		if (Input.GetKey(KeyCode.C))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.KEY_C;
		}
		if (Input.GetKey(KeyCode.Z))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.KEY_Z;
		}
		if (Input.GetKey(KeyCode.R))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.KEY_R;
		}
		if (Input.GetKey(KeyCode.V))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.KEY_V;
		}
		if (Input.GetKey(KeyCode.X))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.KEY_X;
		}
		if (Input.GetKey(KeyCode.K))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.KEY_K;
		}
		if (Input.GetKey(KeyCode.A))
		{
			KeyboardInput._newlyPressed |= KeyboardInput.ControlButtons.KEY_A;
		}
		KeyboardInput.ControlButtons controlButtons = KeyboardInput._lastPressed & KeyboardInput._newlyPressed & ~KeyboardInput.KEYS_THAT_DOES_NOT_INFLICT_KEY_REPEAT;
		if (controlButtons != KeyboardInput.ControlButtons.NONE && controlButtons != KeyboardInput._keyRepeatBlocker)
		{
			KeyboardInput._keyRepeatBlocker = controlButtons;
			KeyboardInput._repeatTimeout = KeyboardInput.KEY_REPEAT_TIMEOUTS[KeyboardInput._keyRepeatIterator];
		}
		if ((KeyboardInput._newlyPressed & KeyboardInput._keyRepeatBlocker) == KeyboardInput.ControlButtons.NONE || KeyboardInput._repeatTimeout < 0f)
		{
			if (KeyboardInput._repeatTimeout < 0f && KeyboardInput._keyRepeatBlocker != KeyboardInput.ControlButtons.NONE)
			{
				KeyboardInput._keyRepeatIterator++;
				if (KeyboardInput._keyRepeatIterator >= KeyboardInput.KEY_REPEAT_TIMEOUTS.Length)
				{
					KeyboardInput._keyRepeatIterator = KeyboardInput.KEY_REPEAT_TIMEOUTS.Length - 1;
				}
			}
			else
			{
				KeyboardInput._keyRepeatIterator = 0;
			}
			KeyboardInput._keyRepeatBlocker = KeyboardInput.ControlButtons.NONE;
		}
		KeyboardInput._lastPressed = KeyboardInput._newlyPressed;
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0001193C File Offset: 0x0000FB3C
	public static KeyboardInput.ControlButtons GetPressedControlKeys(bool pKeyRepeatLatency)
	{
		return KeyboardInput._newlyPressed & ((!pKeyRepeatLatency) ? ((KeyboardInput.ControlButtons)(-1)) : (~KeyboardInput._keyRepeatBlocker));
	}

	// Token: 0x06000261 RID: 609 RVA: 0x00011958 File Offset: 0x0000FB58
	public static bool CheckWithKeyRepeat(KeyboardInput.ControlButtons combination)
	{
		return (KeyboardInput.GetPressedControlKeys(true) & combination) == combination;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00011968 File Offset: 0x0000FB68
	public static bool CheckWithoutRepeat(KeyboardInput.ControlButtons combination)
	{
		return (KeyboardInput.GetPressedControlKeys(false) & combination) == combination;
	}

	// Token: 0x06000263 RID: 611 RVA: 0x00011978 File Offset: 0x0000FB78
	public static bool IsControlModifierHeld()
	{
		return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
	}

	// Token: 0x06000264 RID: 612 RVA: 0x000119C0 File Offset: 0x0000FBC0
	public static bool IsShiftModifierHeld()
	{
		return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
	}

	// Token: 0x06000265 RID: 613 RVA: 0x000119E0 File Offset: 0x0000FBE0
	public static bool KEY_PRESET_Cut()
	{
		return KeyboardInput.IsControlModifierHeld() && Input.GetKeyDown(KeyCode.X);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x000119F8 File Offset: 0x0000FBF8
	public static bool KEY_PRESET_CutLine()
	{
		return KeyboardInput.IsControlModifierHeld() && Input.GetKeyDown(KeyCode.K);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00011A10 File Offset: 0x0000FC10
	public static bool KEY_PRESET_Copy()
	{
		return KeyboardInput.IsControlModifierHeld() && Input.GetKeyDown(KeyCode.C);
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00011A28 File Offset: 0x0000FC28
	public static bool KEY_PRESET_Paste()
	{
		return KeyboardInput.IsControlModifierHeld() && KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.KEY_V);
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00011A44 File Offset: 0x0000FC44
	public static bool KEY_PRESET_Quit()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.ESC);
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00011A50 File Offset: 0x0000FC50
	public static bool KEY_PRESET_Compile()
	{
		return KeyboardInput.IsControlModifierHeld() && (KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.KEY_B) || KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.KEY_R) || KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.ENTER));
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00011A98 File Offset: 0x0000FC98
	public static bool KEY_PRESET_Hack()
	{
		return KeyboardInput.IsControlModifierHeld() && (KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.KEY_H) || KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.ENTER));
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00011AC4 File Offset: 0x0000FCC4
	public static bool KEY_PRESET_Undo()
	{
		return KeyboardInput.IsControlModifierHeld() && !KeyboardInput.IsShiftModifierHeld() && KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.KEY_Z);
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00011AE8 File Offset: 0x0000FCE8
	public static bool KEY_PRESET_Redo()
	{
		return KeyboardInput.IsShiftModifierHeld() && KeyboardInput.IsControlModifierHeld() && KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.KEY_Z);
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00011B0C File Offset: 0x0000FD0C
	public static bool KEY_PRESET_SelectAll()
	{
		return KeyboardInput.IsControlModifierHeld() && KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.KEY_A);
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00011B28 File Offset: 0x0000FD28
	public static bool KEY_PRESET_AutoComplete()
	{
		KeyboardInput.ControlButtons controlButtons = KeyboardInput.ControlButtons.TAB;
		return (KeyboardInput.GetPressedControlKeys(true) & controlButtons) == controlButtons;
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00011B48 File Offset: 0x0000FD48
	public static bool KEY_PRESET_Enter()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.ENTER);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00011B54 File Offset: 0x0000FD54
	public static bool KEY_PRESET_Backspace()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.BACKSPACE);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00011B60 File Offset: 0x0000FD60
	public static bool KEY_PRESET_Delete()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.DELETE);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00011B68 File Offset: 0x0000FD68
	public static bool KEY_PRESET_Return()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.ENTER);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x00011B74 File Offset: 0x0000FD74
	public static bool KEY_PRESET_Tab()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.TAB);
	}

	// Token: 0x06000275 RID: 629 RVA: 0x00011B80 File Offset: 0x0000FD80
	public static bool KEY_PRESET_PageDown()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.PGDWN) || (KeyboardInput.IsControlModifierHeld() && KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.DOWN_ARROW));
	}

	// Token: 0x06000276 RID: 630 RVA: 0x00011BAC File Offset: 0x0000FDAC
	public static bool KEY_PRESET_PageUp()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.PGUP) || (KeyboardInput.IsControlModifierHeld() && KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.UP_ARROW));
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00011BD8 File Offset: 0x0000FDD8
	public static bool KEY_PRESET_End()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.END) || (KeyboardInput.IsControlModifierHeld() && KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.RIGHT_ARROW));
	}

	// Token: 0x06000278 RID: 632 RVA: 0x00011C04 File Offset: 0x0000FE04
	public static bool KEY_PRESET_Home()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.HOME) || (KeyboardInput.IsControlModifierHeld() && KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.LEFT_ARROW));
	}

	// Token: 0x06000279 RID: 633 RVA: 0x00011C30 File Offset: 0x0000FE30
	public static bool KEY_PRESET_GotoStartOfFile()
	{
		return KeyboardInput.IsControlModifierHeld() && (KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.HOME) || KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.UP_ARROW));
	}

	// Token: 0x0600027A RID: 634 RVA: 0x00011C5C File Offset: 0x0000FE5C
	public static bool KEY_PRESET_GotoEndOfFile()
	{
		return KeyboardInput.IsControlModifierHeld() && (KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.END) || KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.DOWN_ARROW));
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00011C88 File Offset: 0x0000FE88
	public static bool KEY_PRESET_RightArrow()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.RIGHT_ARROW);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00011C94 File Offset: 0x0000FE94
	public static bool KEY_PRESET_LeftArrow()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.LEFT_ARROW);
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00011CA0 File Offset: 0x0000FEA0
	public static bool KEY_PRESET_UpArrow()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.UP_ARROW);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00011CAC File Offset: 0x0000FEAC
	public static bool KEY_PRESET_DownArrow()
	{
		return KeyboardInput.CheckWithKeyRepeat(KeyboardInput.ControlButtons.DOWN_ARROW);
	}

	// Token: 0x04000170 RID: 368
	private static readonly float[] KEY_REPEAT_TIMEOUTS = new float[] { 0.3f, 0.025f, 0.025f, 0.025f, 0.025f, 0f };

	// Token: 0x04000171 RID: 369
	private static readonly KeyboardInput.ControlButtons KEYS_THAT_DOES_NOT_INFLICT_KEY_REPEAT = KeyboardInput.ControlButtons.LEFT_CTRL | KeyboardInput.ControlButtons.LEFT_SHIFT | KeyboardInput.ControlButtons.RIGHT_CTRL | KeyboardInput.ControlButtons.RIGHT_SHIFT;

	// Token: 0x04000172 RID: 370
	private static float _repeatTimeout;

	// Token: 0x04000173 RID: 371
	private static int _keyRepeatIterator;

	// Token: 0x04000174 RID: 372
	private static KeyboardInput.ControlButtons _lastPressed;

	// Token: 0x04000175 RID: 373
	private static KeyboardInput.ControlButtons _newlyPressed;

	// Token: 0x04000176 RID: 374
	private static KeyboardInput.ControlButtons _keyRepeatBlocker;

	// Token: 0x0200003D RID: 61
	[Flags]
	public enum ControlButtons
	{
		// Token: 0x04000178 RID: 376
		NONE = 0,
		// Token: 0x04000179 RID: 377
		SUPER = 1,
		// Token: 0x0400017A RID: 378
		DELETE = 2,
		// Token: 0x0400017B RID: 379
		LEFT_ALT = 4,
		// Token: 0x0400017C RID: 380
		LEFT_CTRL = 8,
		// Token: 0x0400017D RID: 381
		LEFT_SHIFT = 16,
		// Token: 0x0400017E RID: 382
		RIGHT_ALT = 32,
		// Token: 0x0400017F RID: 383
		RIGHT_CTRL = 64,
		// Token: 0x04000180 RID: 384
		RIGHT_SHIFT = 128,
		// Token: 0x04000181 RID: 385
		KEY_H = 256,
		// Token: 0x04000182 RID: 386
		TAB = 512,
		// Token: 0x04000183 RID: 387
		BACKSPACE = 1024,
		// Token: 0x04000184 RID: 388
		ENTER = 2048,
		// Token: 0x04000185 RID: 389
		ESC = 4096,
		// Token: 0x04000186 RID: 390
		END = 8192,
		// Token: 0x04000187 RID: 391
		HOME = 16384,
		// Token: 0x04000188 RID: 392
		PGUP = 32768,
		// Token: 0x04000189 RID: 393
		PGDWN = 65536,
		// Token: 0x0400018A RID: 394
		INSERT = 131072,
		// Token: 0x0400018B RID: 395
		UP_ARROW = 262144,
		// Token: 0x0400018C RID: 396
		DOWN_ARROW = 524288,
		// Token: 0x0400018D RID: 397
		LEFT_ARROW = 1048576,
		// Token: 0x0400018E RID: 398
		RIGHT_ARROW = 2097152,
		// Token: 0x0400018F RID: 399
		KEY_Z = 4194304,
		// Token: 0x04000190 RID: 400
		KEY_R = 8388608,
		// Token: 0x04000191 RID: 401
		KEY_C = 16777216,
		// Token: 0x04000192 RID: 402
		KEY_V = 33554432,
		// Token: 0x04000193 RID: 403
		KEY_B = 67108864,
		// Token: 0x04000194 RID: 404
		KEY_X = 134217728,
		// Token: 0x04000195 RID: 405
		KEY_K = 268435456,
		// Token: 0x04000196 RID: 406
		KEY_A = 536870912
	}
}
