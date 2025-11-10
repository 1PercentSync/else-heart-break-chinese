using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameTypes;
using GameWorld2;
using ProgrammingLanguageNr1;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public class CodeEditor
{
	// Token: 0x06000650 RID: 1616 RVA: 0x00029694 File Offset: 0x00027894
	public CodeEditor(TerminalRenderer pRenderer, Program pProgram, CodeEditor.AutoCompleteProvider pAutoCompleteProvider, SourceCodeDispenser pSourceCodeDispenser)
	{
		this._tokenizer = new Tokenizer(this._errorHandler, false);
		this._renderer = pRenderer;
		int num = 0;
		int num2 = 0;
		this._renderer.SetRect(num, num2, 512 - num, 256 - num2 * 2);
		this._renderer.UseColor(CodeEditor.COLOR_DEFAULT);
		this.TryGetAutoComplete = pAutoCompleteProvider;
		this.SetProgram(pProgram);
		this._sourceCodeDispenser = pSourceCodeDispenser;
	}

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000652 RID: 1618 RVA: 0x00029878 File Offset: 0x00027A78
	// (remove) Token: 0x06000653 RID: 1619 RVA: 0x00029894 File Offset: 0x00027A94
	public event CodeEditor.UpdateHandler OnTextChanged;

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000654 RID: 1620 RVA: 0x000298B0 File Offset: 0x00027AB0
	public int scrollPosition
	{
		get
		{
			return this._scrollPosition;
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000655 RID: 1621 RVA: 0x000298B8 File Offset: 0x00027AB8
	public StringEditor stringEditor
	{
		get
		{
			return this._stringEditor;
		}
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x000298C0 File Offset: 0x00027AC0
	private Color TokenToColor(Token t)
	{
		Token.TokenType tokenType = t.getTokenType();
		switch (tokenType)
		{
		case Token.TokenType.IF:
		case Token.TokenType.LOOP:
		case Token.TokenType.IN:
		case Token.TokenType.BREAK:
		case Token.TokenType.RETURN:
		case Token.TokenType.FROM:
		case Token.TokenType.TO:
			break;
		default:
			switch (tokenType)
			{
			case Token.TokenType.NAME:
				if (this._program.HasFunction(t.getTokenString(), false))
				{
					return CodeEditor.COLOR_FUNCTION;
				}
				return CodeEditor.COLOR_DEFAULT;
			default:
				switch (tokenType)
				{
				case Token.TokenType.BLOCK_END:
					goto IL_00A6;
				case Token.TokenType.VAR_DECLARATION:
					goto IL_00AC;
				}
				return CodeEditor.COLOR_DEFAULT;
			case Token.TokenType.NUMBER:
				return CodeEditor.COLOR_NUMBER;
			case Token.TokenType.QUOTED_STRING:
				return CodeEditor.COLOR_QUOTED_STRING;
			case Token.TokenType.BOOLEAN_VALUE:
				return CodeEditor.COLOR_BOOL;
			case Token.TokenType.BUILT_IN_TYPE_NAME:
				break;
			case Token.TokenType.ELSE:
				goto IL_00A6;
			}
			IL_00AC:
			return CodeEditor.COLOR_TYPE_NAME;
		case Token.TokenType.COMMENT:
			return CodeEditor.COLOR_COMMENT;
		}
		IL_00A6:
		return CodeEditor.COLOR_KEYWORD;
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x000299A8 File Offset: 0x00027BA8
	public void SetProgram(Program pProgram)
	{
		this._program = pProgram;
		this._stringEditor = new StringEditor(pProgram.sourceCodeContent, new StringEditor.AutoCompleteProvider(this.OnEditorTryGetAutoComplete));
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x000299DC File Offset: 0x00027BDC
	private bool OnEditorTryGetAutoComplete(StringEditor pEditor, out string pText, out int pArgumentCount)
	{
		if (this.TryGetAutoComplete == null)
		{
			pArgumentCount = 0;
			pText = string.Empty;
			return false;
		}
		return this.TryGetAutoComplete(pEditor, this._program, out pText, out pArgumentCount);
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00029A0C File Offset: 0x00027C0C
	public void DoInput()
	{
		bool flag = this.UpdateCursorBlink();
		bool flag2 = this._stringEditor.TakeInputAndMakeChanges();
		if (flag2)
		{
			this._program.StopAndReset();
			this._program.ClearErrors();
		}
		if (flag2 || flag)
		{
			IntPoint cursorPosition = this._stringEditor.cursorPosition;
			this.FocusScrollOnCursor(cursorPosition);
			this.DrawText(this._scrollPosition, this._renderer.TextRowCount, cursorPosition);
			if (this.OnTextChanged != null && flag2)
			{
				this.OnTextChanged(this.stringEditor, this._program);
			}
		}
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x00029AA8 File Offset: 0x00027CA8
	public void ForceTextChangedEvent()
	{
		this.OnTextChanged(this.stringEditor, this._program);
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x00029AC4 File Offset: 0x00027CC4
	private void DrawErrorLine(int startX, int pScreenLine, string pText)
	{
		this._renderer.UseInvert(true);
		this._renderer.UseColor(CodeEditor.COLOR_ERROR);
		this._renderer.SetLine(pScreenLine, startX, pText);
		this._renderer.UseInvert(false);
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x00029B08 File Offset: 0x00027D08
	private void DrawSyntaxColoredLine(int startX, int pScreenLine, string pText)
	{
		IEnumerator<Token> enumerator = this._tokenizer.process(new StringReader(pText)).GetEnumerator();
		bool flag = enumerator.MoveNext();
		int i;
		for (i = 0; i < pText.Length; i++)
		{
			if (flag && i == enumerator.Current.LinePosition)
			{
				if (enumerator.Current.getTokenType() != Token.TokenType.EOF)
				{
					this._renderer.UseColor(this.TokenToColor(enumerator.Current));
				}
				flag = enumerator.MoveNext();
			}
			this._renderer.SetCharacter(i + startX, pScreenLine, pText[i]);
		}
		while (i + startX < this._renderer.TextCollumCount)
		{
			this._renderer.SetCharacter(i + startX, pScreenLine, ' ');
			i++;
		}
		this._errorHandler.Reset();
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x00029BE4 File Offset: 0x00027DE4
	private string RowNumberToString(int pRow)
	{
		return ((pRow >= 10) ? string.Empty : " ") + pRow.ToString() + "  ";
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x00029C1C File Offset: 0x00027E1C
	private int RowNumberLength(int pRow)
	{
		return 3;
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x00029C20 File Offset: 0x00027E20
	private void DrawText(int pLineNumber, int pCount, IntPoint cursor)
	{
		this._renderer.ClearInvertColors();
		this._renderer.UseColor(Color.white);
		int i = 0;
		int num = 0;
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder();
		int num2 = pLineNumber;
		foreach (char c in this._stringEditor.GetChars(num2))
		{
			string text = this.RowNumberToString(num2);
			bool flag2 = false;
			char c2 = c;
			if (c2 != '〔')
			{
				if (c2 != '〕')
				{
					if (c2 != '\u0004' && c2 != '\n')
					{
						stringBuilder.Append(c);
						this._renderer.ToggleInvertColor(this.RowNumberLength(num2) + num++, i, flag);
					}
					else
					{
						if (!CodeEditor.INVALID_CHARS.Contains(c))
						{
							stringBuilder.Append(c);
						}
						this._renderer.ToggleInvertColor(this.RowNumberLength(num2) + num++, i, flag);
						flag2 = true;
					}
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			if (flag2)
			{
				if (i < pCount)
				{
					this._renderer.UseColor(CodeEditor.COLOR_ROW_NUMBER);
					for (int j = 0; j < text.Length; j++)
					{
						this._renderer.SetCharacter(j, i, text[j]);
					}
					this._renderer.UseColor(CodeEditor.COLOR_DEFAULT);
					if (this.HasErrorLine(num2))
					{
						this.DrawErrorLine(this.RowNumberLength(num2), i++, stringBuilder.ToString());
					}
					else
					{
						this.DrawSyntaxColoredLine(this.RowNumberLength(num2), i++, stringBuilder.ToString());
					}
				}
				num2++;
				num = 0;
				stringBuilder = new StringBuilder();
			}
			if (i >= pCount)
			{
				break;
			}
		}
		this._renderer.UseInvert(false);
		this._renderer.UseColor(CodeEditor.COLOR_COMMENT);
		while (i < pCount)
		{
			this._renderer.SetLine(i++, "~");
		}
		int num3 = cursor.x + this.RowNumberLength(cursor.y);
		int num4 = cursor.y - pLineNumber;
		Color color = this._renderer.GetColor(num3, num4);
		if (this._cursorPulse)
		{
			this._renderer.SetCharacter(num3, num4, '|', CodeEditor.COLOR_CURSOR);
		}
		else
		{
			this._renderer.SetCharacter(num3, num4, this._stringEditor.cursorChar, color);
		}
		this._renderer.ApplyTextChanges();
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x00029EE4 File Offset: 0x000280E4
	private bool HasErrorLine(int documentLine)
	{
		foreach (Error error in this._program.GetErrors())
		{
			if (error.getLineNr() - 1 == documentLine)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x00029F28 File Offset: 0x00028128
	private bool UpdateCursorBlink()
	{
		if (this._cursorPulseTimer >= 0.25f)
		{
			this._cursorPulseTimer = 0f;
			this._cursorPulse = !this._cursorPulse;
			return true;
		}
		this._cursorPulseTimer += Time.deltaTime;
		return false;
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00029F74 File Offset: 0x00028174
	private void FocusScrollOnCursor(IntPoint pCursorPosition)
	{
		if (pCursorPosition.y >= this._scrollPosition + this._renderer.TextRowCount)
		{
			this._scrollPosition = pCursorPosition.y - (this._renderer.TextRowCount - 1);
		}
		if (pCursorPosition.y < this._scrollPosition)
		{
			this._scrollPosition = pCursorPosition.y;
		}
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00029FDC File Offset: 0x000281DC
	internal void CompileAndSave()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in this._stringEditor.GetChars(0))
		{
			if (!CodeEditor.INVALID_CHARS.Contains(c))
			{
				stringBuilder.Append(c);
			}
		}
		this._program.sourceCodeContent = stringBuilder.ToString();
		this._program.Compile();
		this.ForceTextChangedEvent();
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x0002A080 File Offset: 0x00028280
	internal void ResetToOriginalSourceCode()
	{
		string content = this._sourceCodeDispenser.GetSourceCode(this._program.sourceCodeName).content;
		Debug.Log("Reverting to original source code: " + content);
		this._program.sourceCodeContent = content;
		this.SetProgram(this._program);
		this.CompileAndSave();
	}

	// Token: 0x0400041B RID: 1051
	public static readonly char[] INVALID_CHARS = new char[] { '〔', '〕', '\u0004' };

	// Token: 0x0400041C RID: 1052
	public static readonly Color COLOR_CURSOR = Color.white;

	// Token: 0x0400041D RID: 1053
	public static readonly Color COLOR_BACKGROUND = new Color(0.01f, 0.02f, 0.01f);

	// Token: 0x0400041E RID: 1054
	public static readonly Color COLOR_COMMENT = new Color(0.4f, 0.4f, 0.4f);

	// Token: 0x0400041F RID: 1055
	public static readonly Color COLOR_ERROR = new Color(1f, 0.1f, 0.1f);

	// Token: 0x04000420 RID: 1056
	public static readonly Color COLOR_ROW_NUMBER = new Color(1f, 0.5f, 0f);

	// Token: 0x04000421 RID: 1057
	public static readonly Color COLOR_QUOTED_STRING = new Color(1f, 0.85f, 0.2f);

	// Token: 0x04000422 RID: 1058
	public static readonly Color COLOR_NUMBER = new Color(1f, 1f, 0.9f);

	// Token: 0x04000423 RID: 1059
	public static readonly Color COLOR_BOOL = new Color(1f, 0.3f, 0.3f);

	// Token: 0x04000424 RID: 1060
	public static readonly Color COLOR_TYPE_NAME = new Color(0.25f, 1f, 0.2f);

	// Token: 0x04000425 RID: 1061
	public static readonly Color COLOR_BUILT_IN_TYPE = new Color(0.5f, 0f, 0.6f);

	// Token: 0x04000426 RID: 1062
	public static readonly Color COLOR_KEYWORD = new Color(0.25f, 1f, 0.2f);

	// Token: 0x04000427 RID: 1063
	public static readonly Color COLOR_FUNCTION = new Color(0.2f, 0.4f, 1f);

	// Token: 0x04000428 RID: 1064
	public static readonly Color COLOR_DEFAULT = new Color(0.5f, 0.7f, 1f);

	// Token: 0x04000429 RID: 1065
	private CodeEditor.AutoCompleteProvider TryGetAutoComplete;

	// Token: 0x0400042A RID: 1066
	private TerminalRenderer _renderer;

	// Token: 0x0400042B RID: 1067
	private int _scrollPosition;

	// Token: 0x0400042C RID: 1068
	private StringEditor _stringEditor;

	// Token: 0x0400042D RID: 1069
	private ErrorHandler _errorHandler = new ErrorHandler();

	// Token: 0x0400042E RID: 1070
	private Tokenizer _tokenizer;

	// Token: 0x0400042F RID: 1071
	private Program _program;

	// Token: 0x04000430 RID: 1072
	private SourceCodeDispenser _sourceCodeDispenser;

	// Token: 0x04000431 RID: 1073
	private float _cursorPulseTimer;

	// Token: 0x04000432 RID: 1074
	private bool _cursorPulse;

	// Token: 0x02000102 RID: 258
	// (Invoke) Token: 0x06000767 RID: 1895
	public delegate bool AutoCompleteProvider(StringEditor pStringEditor, Program pProgram, out string pOutput, out int pArgumentCount);

	// Token: 0x02000103 RID: 259
	// (Invoke) Token: 0x0600076B RID: 1899
	public delegate void UpdateHandler(StringEditor pEditor, Program pProgram);
}
