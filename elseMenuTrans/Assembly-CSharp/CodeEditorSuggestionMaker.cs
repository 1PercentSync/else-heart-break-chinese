using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameWorld2;
using ProgrammingLanguageNr1;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public class CodeEditorSuggestionMaker
{
	// Token: 0x06000665 RID: 1637 RVA: 0x0002A0D8 File Offset: 0x000282D8
	public CodeEditorSuggestionMaker(TerminalRenderer pRenderer)
	{
		this._renderer = pRenderer;
		this._renderer.SetRect(0, 0, 512, 64);
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0002A1A0 File Offset: 0x000283A0
	public void Update(StringEditor pEditor, Program pProgram)
	{
		this._renderer.UseColor(Color.green);
		string word = pEditor.word;
		int num = this.SeekParameterIndex(pEditor);
		for (int i = 0; i < 8; i++)
		{
			this._renderer.SetLine(i, string.Empty);
		}
		if (pProgram.GetErrors().Length > 0)
		{
			int num2 = 0;
			foreach (Error error in pProgram.GetErrors())
			{
				if (error.getLineNr() < 0)
				{
					this._renderer.SetLine(num2, "〄Error: ");
					this._renderer.SetLine(num2 + 1, "〄 " + error.getMessage());
				}
				else
				{
					this._renderer.SetLine(num2, "〄Error on line " + error.getLineNr() + ": ");
					this._renderer.SetLine(num2 + 1, "〄 " + error.getMessage());
				}
				num2 += 2;
			}
		}
		else if (!pProgram.HasFunction(word, false) || num != -1)
		{
			if (num != -1)
			{
				string text = this.SeekFunction(pEditor);
				this.DisplayFunctionDetails(text, num, pProgram);
			}
			else
			{
				int num3 = 0;
				StringBuilder stringBuilder = new StringBuilder("】Suggestions: 【");
				int num4 = stringBuilder.Length;
				string[] functionSuggestions = this.GetFunctionSuggestions(this.GetDefinitions(pProgram), word);
				foreach (string text2 in functionSuggestions)
				{
					num4 += text2.Length;
					if (num4 >= this._renderer.TextCollumCount)
					{
						this._renderer.SetLine(num3++, stringBuilder.ToString());
						stringBuilder = new StringBuilder("【");
						num4 = stringBuilder.Length;
					}
					stringBuilder.Append(text2 + " ");
					num4 = stringBuilder.Length;
				}
				this._renderer.SetLine(num3, stringBuilder.ToString());
			}
		}
		this._renderer.ApplyTextChanges();
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x0002A3C4 File Offset: 0x000285C4
	public bool TryGetAutoComplete(StringEditor pStringEditor, Program pProgram, out string pOutput, out int pArgumentCount)
	{
		string word = pStringEditor.word;
		Debug.Log("Trying to get autocomplete from word '" + word + "'");
		if (word == string.Empty || !char.IsWhiteSpace(pStringEditor.cursorChar))
		{
			pArgumentCount = 0;
			pOutput = string.Empty;
			return false;
		}
		List<FunctionDefinition> definitions = this.GetDefinitions(pProgram);
		string[] functionSuggestions = this.GetFunctionSuggestions(definitions, word);
		if (functionSuggestions.Length >= 1)
		{
			string text = functionSuggestions[0];
			pArgumentCount = this.GetArgumentCount(definitions, text);
			pOutput = text;
			return true;
		}
		pArgumentCount = 0;
		pOutput = string.Empty;
		return false;
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x0002A454 File Offset: 0x00028654
	private List<FunctionDefinition> GetDefinitions(Program pProgram)
	{
		List<FunctionDefinition> list = new List<FunctionDefinition>(SprakRunner.builtInFunctions);
		list.AddRange(pProgram.FunctionDefinitions);
		list.RemoveAll((FunctionDefinition def) => def.hideInModifier);
		return list;
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0002A4A0 File Offset: 0x000286A0
	private int GetArgumentCount(List<FunctionDefinition> definitions, string symbol)
	{
		return definitions.Find((FunctionDefinition p) => p.functionName.ToLower() == symbol.ToLower()).parameterNames.Count<string>();
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0002A4DC File Offset: 0x000286DC
	public string[] GetFunctionSuggestions(List<FunctionDefinition> definitions, string pWord)
	{
		pWord = pWord.ToLower();
		List<string> list = new List<string>();
		for (int i = 0; i < pWord.Length; i++)
		{
			for (int j = definitions.Count - 1; j >= 0; j--)
			{
				string text = definitions[j].functionName.ToLower();
				if (i >= text.Length || text[i] != pWord[i])
				{
					definitions.RemoveAt(j);
				}
			}
		}
		foreach (FunctionDefinition functionDefinition in definitions)
		{
			list.Add(functionDefinition.functionName);
		}
		return list.ToArray();
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x0002A5C8 File Offset: 0x000287C8
	public string SeekFunction(StringEditor pStringEditor)
	{
		int num = pStringEditor.cursor;
		int num2 = 0;
		foreach (char c in pStringEditor.PeekLeft('\n'))
		{
			if (c == '(')
			{
				num2--;
			}
			else if (c == ')')
			{
				num2++;
			}
			num--;
			if (num2 == -1)
			{
				return pStringEditor.GetWord(num);
			}
		}
		return string.Empty;
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x0002A670 File Offset: 0x00028870
	public int SeekParameterIndex(StringEditor pStringEditor)
	{
		string text = pStringEditor.GetLines(pStringEditor.cursorPosition.y, 1).First<string>();
		text = this.ReplaceSeparatorsWithinQuotes(text);
		int num = 0;
		int num2 = -1;
		int num3 = pStringEditor.cursor - 1;
		while (num3 >= 0 && num != -1)
		{
			if (num3 < text.Length)
			{
				if (text[num3] == ',' && num == 0)
				{
					num2++;
				}
				else if (text[num3] == '(')
				{
					num--;
				}
				else if (text[num3] == ')')
				{
					num++;
				}
			}
			num3--;
		}
		if (num == -1)
		{
			num2++;
		}
		return num2;
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0002A728 File Offset: 0x00028928
	private string ReplaceSeparatorsWithinQuotes(string pString)
	{
		char[] array = pString.ToCharArray();
		bool flag = false;
		for (int i = 0; i < pString.Length; i++)
		{
			if (array[i] == '"')
			{
				flag = !flag;
			}
			else if (StringEditor.DELIMITERS.Contains(array[i]) && flag)
			{
				array[i] = 'A';
			}
		}
		return new string(array);
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x0002A78C File Offset: 0x0002898C
	private void DisplayFunctionDetails(string pFunctionName, int pParameterIndex, Program pProgram)
	{
		FunctionDefinition functionDefinition;
		if (!pProgram.TryGetFunctionDefinition(pFunctionName, out functionDefinition))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append('【' + functionDefinition.functionName);
		stringBuilder.Append("(");
		bool flag = true;
		for (int i = 0; i < functionDefinition.parameterNames.Length; i++)
		{
			if (!flag)
			{
				stringBuilder.Append("【, ");
			}
			else
			{
				stringBuilder.Append(" ");
				flag = false;
			}
			if (i == pParameterIndex)
			{
				stringBuilder.Append('〒' + functionDefinition.parameterNames[i]);
			}
			else
			{
				stringBuilder.Append(functionDefinition.parameterNames[i]);
			}
		}
		stringBuilder.Append("【 )");
		this._renderer.SetLine(0, '〈' + functionDefinition.functionDocumentation.GetFunctionDescription() + '〃');
		this._renderer.SetLine(1, stringBuilder.ToString());
		if (pParameterIndex != -1)
		{
			this._renderer.SetLine(2, '〓' + functionDefinition.functionDocumentation.GetArgumentDescription(pParameterIndex) + '〃');
		}
	}

	// Token: 0x04000434 RID: 1076
	public static readonly Color COLOR_FUNCTION_DESCRIPTION = new Color(0.95f, 0.6f, 0f);

	// Token: 0x04000435 RID: 1077
	public static readonly Color COLOR_PARAMETER_DESCRIPTION = new Color(0.1f, 0.55f, 0.1f);

	// Token: 0x04000436 RID: 1078
	public static readonly Color COLOR_PARAMETER_SELECTION = new Color(0.1f, 0.8f, 0.1f);

	// Token: 0x04000437 RID: 1079
	public static readonly Color COLOR_FUNCTION_NAME = new Color(1f, 1f, 1f);

	// Token: 0x04000438 RID: 1080
	public static readonly Color COLOR_FUNCTION_SUGGESTION_TITLE = new Color(0.6f, 0.6f, 0.3f);

	// Token: 0x04000439 RID: 1081
	public static readonly Color COLOR_FUNCTION_SUGGESTION_BODY = new Color(1f, 1f, 0.7f);

	// Token: 0x0400043A RID: 1082
	private TerminalRenderer _renderer;
}
