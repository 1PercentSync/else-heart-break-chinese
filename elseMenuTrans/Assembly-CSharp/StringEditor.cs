using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GameTypes;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class StringEditor
{
	// Token: 0x060006D1 RID: 1745 RVA: 0x0002C34C File Offset: 0x0002A54C
	public StringEditor(string pData, StringEditor.AutoCompleteProvider pAutoCompleter)
	{
		this.TryGetAutoComplete = pAutoCompleter;
		this.buffer = new GapBuffer(pData);
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0002C394 File Offset: 0x0002A594
	// (set) Token: 0x060006D4 RID: 1748 RVA: 0x0002C39C File Offset: 0x0002A59C
	public string clipboard
	{
		get
		{
			return ClipboardHelper.clipboard;
		}
		set
		{
			ClipboardHelper.clipboard = value;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0002C3A4 File Offset: 0x0002A5A4
	private bool isSelecting
	{
		get
		{
			return this._selectionStart != -1;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0002C3B4 File Offset: 0x0002A5B4
	public int cursor
	{
		get
		{
			return this.buffer.left.Length;
		}
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x0002C3C8 File Offset: 0x0002A5C8
	public void PerformUndo()
	{
		this.buffer.Undo();
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x0002C3D8 File Offset: 0x0002A5D8
	public void PerformRedo()
	{
		this.buffer.Redo();
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x0002C3E8 File Offset: 0x0002A5E8
	public bool TakeInputAndMakeChanges()
	{
		bool flag = false;
		string text = Input.inputString;
		KeyboardInput.GetPressedControlKeys(false);
		if (KeyboardInput.KEY_PRESET_Undo())
		{
			if (this.isSelecting)
			{
				this.CancelSelect();
			}
			this.PerformUndo();
			flag = true;
			text = string.Empty;
		}
		else if (KeyboardInput.KEY_PRESET_Redo())
		{
			if (this.isSelecting)
			{
				this.CancelSelect();
			}
			this.PerformRedo();
			flag = true;
			text = string.Empty;
		}
		else if (KeyboardInput.KEY_PRESET_Copy())
		{
			this.CopySelection();
			text = string.Empty;
		}
		else
		{
			this.buffer.BeginRecord();
			string text2;
			int num;
			if (KeyboardInput.KEY_PRESET_AutoComplete() && this.TryGetAutoComplete != null && this.TryGetAutoComplete(this, out text2, out num))
			{
				if (this.isSelecting)
				{
					this.CancelSelect();
				}
				this.EraseCurrentWord();
				if (num > 0)
				{
					this.KeyboardTypeText(text2 + "(");
				}
				else
				{
					this.KeyboardTypeText(text2 + "()");
				}
			}
			else if (KeyboardInput.KEY_PRESET_Cut())
			{
				if (this.isSelecting)
				{
					this.CopySelection();
					this.DeleteSelection();
					text = string.Empty;
				}
			}
			else if (KeyboardInput.KEY_PRESET_CutLine())
			{
				this.CancelSelect();
				this.BeginSelect();
				this.KeyboardEnd();
				this.CopySelection();
				this.DeleteSelection();
				text = string.Empty;
			}
			else if (KeyboardInput.KEY_PRESET_GotoEndOfFile())
			{
				if (this.isSelecting && !KeyboardInput.IsShiftModifierHeld())
				{
					this.CancelSelect();
				}
				this.KeyboardMoveToBottom();
			}
			else if (KeyboardInput.KEY_PRESET_GotoStartOfFile())
			{
				if (this.isSelecting && !KeyboardInput.IsShiftModifierHeld())
				{
					this.CancelSelect();
				}
				this.KeyboardMoveToTop();
			}
			else if (KeyboardInput.KEY_PRESET_Home())
			{
				if (this.isSelecting && !KeyboardInput.IsShiftModifierHeld())
				{
					this.CancelSelect();
				}
				this.KeyboardHome();
			}
			else if (KeyboardInput.KEY_PRESET_End())
			{
				if (this.isSelecting && !KeyboardInput.IsShiftModifierHeld())
				{
					this.CancelSelect();
				}
				this.KeyboardEnd();
			}
			else if (KeyboardInput.KEY_PRESET_PageUp())
			{
				if (this.isSelecting && !KeyboardInput.IsShiftModifierHeld())
				{
					this.CancelSelect();
				}
				this.KeyboardPageUp();
			}
			else if (KeyboardInput.KEY_PRESET_PageDown())
			{
				if (this.isSelecting && !KeyboardInput.IsShiftModifierHeld())
				{
					this.CancelSelect();
				}
				this.KeyboardPageDown();
			}
			else if (KeyboardInput.KEY_PRESET_Paste())
			{
				if (this.isSelecting)
				{
					this.DeleteSelection();
				}
				this.KeyboardTypeText(this.clipboard);
				text = string.Empty;
			}
			else if (KeyboardInput.KEY_PRESET_Delete())
			{
				if (this.isSelecting)
				{
					this.DeleteSelection();
				}
				else
				{
					this.KeyboardDelete();
				}
				text = string.Empty;
			}
			else if (KeyboardInput.KEY_PRESET_Return())
			{
				if (this.isSelecting)
				{
					this.DeleteSelection();
				}
				else
				{
					this.KeyboardReturn();
				}
				text = string.Empty;
			}
			else if (KeyboardInput.KEY_PRESET_DownArrow())
			{
				if (this.isSelecting && !KeyboardInput.IsShiftModifierHeld())
				{
					this.CancelSelect();
				}
				this.KeyboardDownArrow();
			}
			else if (KeyboardInput.KEY_PRESET_UpArrow())
			{
				if (this.isSelecting && !KeyboardInput.IsShiftModifierHeld())
				{
					this.CancelSelect();
				}
				this.KeyboardUpArrow();
			}
			else if (KeyboardInput.KEY_PRESET_LeftArrow())
			{
				if (this.isSelecting && !KeyboardInput.IsShiftModifierHeld())
				{
					int num2 = Mathf.Max(0, this.cursor - this._selectionStart);
					this.CancelSelect();
					for (int i = 0; i < num2; i++)
					{
						this.KeyboardLeftArrow();
					}
				}
				else
				{
					this.KeyboardLeftArrow();
				}
			}
			else if (KeyboardInput.KEY_PRESET_RightArrow())
			{
				if (this.isSelecting && !KeyboardInput.IsShiftModifierHeld())
				{
					int num3 = Mathf.Max(0, this._selectionStart - this.cursor);
					this.CancelSelect();
					for (int j = 0; j < num3; j++)
					{
						this.KeyboardRightArrow();
					}
				}
				else
				{
					this.KeyboardRightArrow();
				}
			}
			else if (KeyboardInput.KEY_PRESET_Backspace())
			{
				if (this.isSelecting)
				{
					this.DeleteSelection();
				}
				else
				{
					this.KeyboardBackspace();
				}
			}
			else if (KeyboardInput.KEY_PRESET_SelectAll())
			{
				this.CancelSelect();
				this.KeyboardMoveToTop();
				this.BeginSelect();
				this.KeyboardMoveToBottom();
			}
			else if (KeyboardInput.KEY_PRESET_Tab())
			{
				if (this.isSelecting)
				{
					this.DeleteSelection();
				}
				else
				{
					this.KeyboardTab();
				}
			}
			else
			{
				if (KeyboardInput.IsShiftModifierHeld() && !this.isSelecting)
				{
					this.BeginSelect();
				}
				else if (this.isSelecting && !KeyboardInput.IsShiftModifierHeld() && this._selectionStart == this.cursor)
				{
					this.CancelSelect();
				}
				if (text != null && text.Length > 0 && this.MatchString(text))
				{
					if (this.isSelecting)
					{
						this.DeleteSelection();
					}
					this.KeyboardTypeText(text);
				}
			}
			flag = flag || this.buffer.EndRecord();
		}
		return flag;
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x0002C950 File Offset: 0x0002AB50
	private void EraseCurrentWord()
	{
		while (this.buffer.left.Length > 0 && this.IsAlphanumeric(this.buffer.left[0]))
		{
			this.KeyboardBackspace();
		}
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0002C99C File Offset: 0x0002AB9C
	private bool IsAlphanumeric(char c)
	{
		return Regex.IsMatch(c.ToString(), "[0-9A-Za-z\\_]");
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0002C9B0 File Offset: 0x0002ABB0
	private bool MatchString(string pString)
	{
		return Regex.IsMatch(pString, "[0-9A-Za-z\\.\\*\\}\\ \\{\\[\\]\\(\\)\\+\\-\\/\\#\\,\\!\\=\\'\\&\\?\\:\\;\\<\\>\\_\\^\\~\\@\\\\\\|]|[\"]");
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x0002C9C0 File Offset: 0x0002ABC0
	private void DeleteSelection()
	{
		while (this.cursor < this._selectionStart)
		{
			this.KeyboardDelete();
			this._selectionStart--;
		}
		while (this.cursor > this._selectionStart)
		{
			this.KeyboardBackspace();
		}
		this.CancelSelect();
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x0002CA1C File Offset: 0x0002AC1C
	private void CopySelection()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		while (num < this.buffer.left.Length && this.isCharSelected(this.cursor - num - 1))
		{
			stringBuilder.Append(this.buffer.left[num]);
			num++;
		}
		char[] array = stringBuilder.ToString().ToCharArray();
		Array.Reverse(array);
		stringBuilder = new StringBuilder(new string(array));
		int num2 = 0;
		while (num2 < this.buffer.right.Length && this.isCharSelected(this.cursor + num2))
		{
			stringBuilder.Append(this.buffer.right[num2]);
			num2++;
		}
		this.clipboard = stringBuilder.ToString();
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x0002CAF4 File Offset: 0x0002ACF4
	private void KeyboardPageUp()
	{
		for (int i = 0; i < 32; i++)
		{
			this.KeyboardUpArrow();
		}
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x0002CB1C File Offset: 0x0002AD1C
	private void KeyboardPageDown()
	{
		for (int i = 0; i < 32; i++)
		{
			this.KeyboardDownArrow();
		}
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x0002CB44 File Offset: 0x0002AD44
	private void KeyboardTab()
	{
		this.KeyboardTypeText("    ");
		this.SaveMaxCursorXPos();
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x0002CB58 File Offset: 0x0002AD58
	private void KeyboardEnd()
	{
		while (this.buffer.right.Length > 0 && this.buffer.right[0] != '\n')
		{
			this.KeyboardRightArrow();
		}
		this.SaveMaxCursorXPos();
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x0002CBA4 File Offset: 0x0002ADA4
	public void KeyboardHome()
	{
		while (this.buffer.left.Length > 0 && this.buffer.left[0] != '\n')
		{
			this.KeyboardLeftArrow();
		}
		this.SaveMaxCursorXPos();
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x0002CBF0 File Offset: 0x0002ADF0
	private void KeyboardReturn()
	{
		int num = this.GetCurrentIndentationLevel();
		int num2 = this.NrOfIndentsOnIndentNextLine();
		for (int i = 0; i < num2; i++)
		{
			num += 4;
		}
		this.buffer.PushLeft('\n');
		for (int j = 0; j < num; j++)
		{
			this.buffer.PushLeft(' ');
		}
		this.SaveMaxCursorXPos();
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0002CC54 File Offset: 0x0002AE54
	private void KeyboardMoveToBottom()
	{
		while (this.buffer.right.Length > 0)
		{
			this.KeyboardRightArrow();
		}
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0002CC78 File Offset: 0x0002AE78
	private void KeyboardMoveToTop()
	{
		while (this.buffer.left.Length > 0)
		{
			this.KeyboardLeftArrow();
		}
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x0002CC9C File Offset: 0x0002AE9C
	private int NrOfIndentsOnIndentNextLine()
	{
		Debug.Log("Calculating nr of indents on next line");
		int num = 0;
		for (int i = 0; i < this.buffer.left.Length; i++)
		{
			if (this.buffer.left[i] == '\n')
			{
				break;
			}
			num++;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = num - 1; j >= 0; j--)
		{
			stringBuilder.Append(this.buffer.left[j]);
		}
		string text = stringBuilder.ToString();
		Debug.Log("This line: '" + text + "'");
		if (StringEditor.ContainsWordButNotEqSign(text, "else"))
		{
			Debug.Log("Contains 'else', will return 1");
			return 1;
		}
		if (text.Contains("loop") || StringEditor.ContainsWordButNotEqSign(text, "if") || StringEditor.ContainsWordButNotEqSign(text, "void") || StringEditor.ContainsWordButNotEqSign(text, "number") || StringEditor.ContainsWordButNotEqSign(text, "bool") || StringEditor.ContainsWordButNotEqSign(text, "string") || StringEditor.ContainsWordButNotEqSign(text, "array"))
		{
			Debug.Log("Contains some other code word, will return 1");
			return 1;
		}
		Debug.Log("Contains no code word, will return 0");
		return 0;
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x0002CDF4 File Offset: 0x0002AFF4
	private static bool ContainsWordButNotEqSign(string s, string word)
	{
		return s.Contains(word) && !s.Contains(" = ");
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0002CE14 File Offset: 0x0002B014
	private int GetCurrentIndentationLevel()
	{
		int num = 0;
		for (int i = 0; i < this.buffer.left.Length; i++)
		{
			char c = this.buffer.left[i];
			if (c == ' ')
			{
				num++;
			}
			else
			{
				if (c == '\n')
				{
					break;
				}
				num = 0;
			}
		}
		Debug.Log("Indent: " + num);
		return num;
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x0002CE8C File Offset: 0x0002B08C
	private void KeyboardDelete()
	{
		if (this.buffer.right.Length > 0)
		{
			this.buffer.PopRight();
			this.SaveMaxCursorXPos();
		}
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x0002CEC4 File Offset: 0x0002B0C4
	private void SaveMaxCursorXPos()
	{
		this._maxCursorXPos = this.cursorPosition.x;
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x0002CEE8 File Offset: 0x0002B0E8
	public void KeyboardDownArrow()
	{
		int maxCursorXPos = this._maxCursorXPos;
		this.KeyboardEnd();
		this.KeyboardRightArrow();
		while (this.buffer.right.Length > 0 && this.buffer.right[0] != '\n' && this.cursorPosition.x < maxCursorXPos)
		{
			this.KeyboardRightArrow();
		}
		this._maxCursorXPos = maxCursorXPos;
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x0002CF5C File Offset: 0x0002B15C
	public void KeyboardUpArrow()
	{
		int maxCursorXPos = this._maxCursorXPos;
		this.KeyboardHome();
		this.KeyboardLeftArrow();
		while (this.cursorPosition.x > maxCursorXPos)
		{
			this.KeyboardLeftArrow();
		}
		this._maxCursorXPos = maxCursorXPos;
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x0002CFA4 File Offset: 0x0002B1A4
	public void KeyboardLeftArrow()
	{
		if (this.buffer.left.Length > 0)
		{
			this.buffer.PushRight(this.buffer.PopLeft());
		}
		this.SaveMaxCursorXPos();
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x0002CFE4 File Offset: 0x0002B1E4
	public void KeyboardRightArrow()
	{
		if (this.buffer.right.Length > 0)
		{
			this.buffer.PushLeft(this.buffer.PopRight());
		}
		this.SaveMaxCursorXPos();
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0002D024 File Offset: 0x0002B224
	private void KeyboardBackspace()
	{
		if (this.buffer.left.Length > 0)
		{
			if (this.buffer.left[0] == ' ')
			{
				int num = 4;
				while (this.buffer.left.Length > 0 && this.buffer.left[0] == ' ' && num > 0)
				{
					this.buffer.PopLeft();
					num--;
				}
			}
			else
			{
				this.buffer.PopLeft();
			}
			this.SaveMaxCursorXPos();
		}
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x0002D0C4 File Offset: 0x0002B2C4
	private void KeyboardTypeText(string pString)
	{
		foreach (char c in pString)
		{
			this.buffer.PushLeft(c);
		}
		this.SaveMaxCursorXPos();
		this.CheckForOutdentOnCurrentLine();
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x0002D10C File Offset: 0x0002B30C
	private void CheckForOutdentOnCurrentLine()
	{
		if (Regex.IsMatch(this.line, "\\x20\\x20\\x20\\x20\\x20*(end|else)") && (this.line.EndsWith("end") || this.line.EndsWith("else")))
		{
			this.KeyboardHome();
			for (int i = 0; i < 4; i++)
			{
				this.KeyboardDelete();
			}
			this.KeyboardEnd();
		}
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0002D17C File Offset: 0x0002B37C
	public void SetCursor(int pPosition)
	{
		this.KeyboardMoveToTop();
		for (int i = 0; i < pPosition; i++)
		{
			this.KeyboardRightArrow();
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0002D1A8 File Offset: 0x0002B3A8
	public IntPoint cursorPosition
	{
		get
		{
			int num = 0;
			int num2 = 0;
			while (num < this.buffer.left.Length && this.buffer.left[num] != '\n')
			{
				num++;
			}
			for (int i = 0; i < this.buffer.left.Length; i++)
			{
				if (this.buffer.left[i] == '\n')
				{
					num2++;
				}
			}
			return new IntPoint(num, num2);
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0002D234 File Offset: 0x0002B434
	public string word
	{
		get
		{
			string text2;
			try
			{
				int i = 0;
				while (i < this.buffer.left.Length && this.buffer.left[i] != '\n' && !StringEditor.DELIMITERS.Contains(this.buffer.left[i]))
				{
					i++;
				}
				int num = 0;
				while (num < this.buffer.right.Length && this.buffer.right[num] != '\n' && !StringEditor.DELIMITERS.Contains(this.buffer.right[num]))
				{
					num++;
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (i--; i >= 0; i--)
				{
					stringBuilder.Append(this.buffer.left[i]);
				}
				for (int j = 0; j < num; j++)
				{
					stringBuilder.Append(this.buffer.right[j]);
				}
				string text = stringBuilder.ToString();
				text2 = text;
			}
			catch (Exception ex)
			{
				Debug.Log(ex.ToString());
				text2 = "ERROR";
			}
			return text2;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0002D3A4 File Offset: 0x0002B5A4
	public string line
	{
		get
		{
			string text;
			try
			{
				int i = 0;
				while (i < this.buffer.left.Length && this.buffer.left[i] != '\n')
				{
					i++;
				}
				int num = 0;
				while (num < this.buffer.right.Length && this.buffer.right[num] != '\n')
				{
					num++;
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (i--; i >= 0; i--)
				{
					stringBuilder.Append(this.buffer.left[i]);
				}
				for (int j = 0; j < num; j++)
				{
					stringBuilder.Append(this.buffer.right[j]);
				}
				text = stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				Debug.Log(ex.ToString());
				text = "ERROR";
			}
			return text;
		}
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0002D4D0 File Offset: 0x0002B6D0
	public string GetWord(int cursor)
	{
		StringEditor stringEditor = new StringEditor(this.GetData(), null);
		for (int i = 0; i < cursor; i++)
		{
			stringEditor.KeyboardRightArrow();
		}
		return stringEditor.word;
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x0002D508 File Offset: 0x0002B708
	public IEnumerable<char> PeekLeft(char pStopChar)
	{
		int i = 0;
		while (i < this.buffer.left.Length && this.buffer.left[i] != pStopChar)
		{
			yield return this.buffer.left[i];
			i++;
		}
		yield break;
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0002D53C File Offset: 0x0002B73C
	public IEnumerable<string> GetLines(int pFirstLineIndex, int pLineCount)
	{
		string r = this.GetData();
		int row = 0;
		StringBuilder lineBuilder = new StringBuilder();
		int i = 0;
		while (i < r.Length && row < pFirstLineIndex)
		{
			if (r[i] == '\n')
			{
				row++;
			}
			i++;
		}
		row = 0;
		bool lastCharWasSelected = false;
		while (i < r.Length && row < pLineCount)
		{
			if (this.isSelecting)
			{
				if (this.isCharSelected(i) && !lastCharWasSelected)
				{
					lastCharWasSelected = true;
					lineBuilder.Append('〔');
				}
				else if (lastCharWasSelected && !this.isCharSelected(i))
				{
					lastCharWasSelected = false;
					lineBuilder.Append('〕');
				}
			}
			lineBuilder.Append(r[i]);
			if (r[i] == '\n')
			{
				yield return lineBuilder.ToString();
				lineBuilder = new StringBuilder();
				row++;
			}
			i++;
		}
		yield return lineBuilder.ToString();
		yield break;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x0002D57C File Offset: 0x0002B77C
	public string GetData()
	{
		return this.buffer.data;
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x0002D58C File Offset: 0x0002B78C
	public IEnumerable<char> GetChars(int pFirstLineIndex)
	{
		string r = this.GetData();
		int row = 0;
		int i = 0;
		while (i < r.Length && row < pFirstLineIndex)
		{
			if (r[i] == '\n')
			{
				row++;
			}
			i++;
		}
		row = 0;
		bool lastCharWasSelected = false;
		while (i < r.Length)
		{
			if (this.isSelecting)
			{
				if (this.isCharSelected(i) && !lastCharWasSelected)
				{
					lastCharWasSelected = true;
					yield return '〔';
				}
				else if (lastCharWasSelected && !this.isCharSelected(i))
				{
					lastCharWasSelected = false;
					yield return '〕';
				}
			}
			yield return r[i];
			i++;
		}
		yield return '\u0004';
		yield break;
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060006FC RID: 1788 RVA: 0x0002D5C0 File Offset: 0x0002B7C0
	public char cursorChar
	{
		get
		{
			if (this.buffer.right.Length > 0)
			{
				return this.buffer.right[0];
			}
			return ' ';
		}
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0002D5F8 File Offset: 0x0002B7F8
	private bool isCharSelected(int pCharIndex)
	{
		return this.isSelecting && ((pCharIndex < this._selectionStart && pCharIndex >= this.cursor) || (pCharIndex >= this._selectionStart && pCharIndex < this.cursor));
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0002D648 File Offset: 0x0002B848
	private void BeginSelect()
	{
		this._selectionStart = this.cursor;
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0002D658 File Offset: 0x0002B858
	private void CancelSelect()
	{
		this._selectionStart = -1;
	}

	// Token: 0x04000496 RID: 1174
	public const string TAB = "    ";

	// Token: 0x04000497 RID: 1175
	public StringEditor.AutoCompleteProvider TryGetAutoComplete;

	// Token: 0x04000498 RID: 1176
	public static readonly char[] DELIMITERS = new char[] { '"', '(', ')', '[', ']', ',', ' ' };

	// Token: 0x04000499 RID: 1177
	private int _selectionStart = -1;

	// Token: 0x0400049A RID: 1178
	private GapBuffer buffer;

	// Token: 0x0400049B RID: 1179
	private int _maxCursorXPos;

	// Token: 0x02000104 RID: 260
	// (Invoke) Token: 0x0600076F RID: 1903
	public delegate bool AutoCompleteProvider(StringEditor pEditor, out string pText, out int pArgumentCount);
}
