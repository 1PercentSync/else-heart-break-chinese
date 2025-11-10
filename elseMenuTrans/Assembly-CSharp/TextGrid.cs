using System;
using System.Collections.Generic;
using GameTypes;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class TextGrid
{
	// Token: 0x0600070C RID: 1804 RVA: 0x0002D938 File Offset: 0x0002BB38
	public TextGrid(int pCollums, int pRows, int pFontSheetWidth, int pFontSheetHeight, Mesh pMesh, Vector3 pOffsetPosition)
	{
		pMesh.Clear();
		this._offsetPosition = pOffsetPosition;
		this._targetSurfaceRows = pRows;
		this._targetSurfaceCollums = pCollums;
		this._fontSheetWidth = pFontSheetWidth;
		this._fontSheetHeight = pFontSheetHeight;
		this._sheetCollumCount = pFontSheetWidth / TextGrid.GLYPH_SIZE;
		this._sheetRowCount = pFontSheetHeight / TextGrid.GLYPH_SIZE;
		float num = (float)TextGrid.GLYPH_SIZE / (float)pFontSheetWidth;
		float num2 = (float)TextGrid.GLYPH_SIZE / (float)pFontSheetHeight;
		this._UV_BOTTOM_LEFT = new Vector3(0f, 0f);
		this._UV_TOP_LEFT = new Vector3(0f, num2);
		this._UV_TOP_RIGHT = new Vector3(num, num2);
		this._UV_BOTTOM_RIGHT = new Vector3(num, 0f);
		for (int i = 0; i < pCollums; i++)
		{
			for (int j = 0; j < pRows; j++)
			{
				TextGrid.GlyphInfo glyphInfo = new TextGrid.GlyphInfo();
				glyphInfo.gridPosition.x = i;
				glyphInfo.gridPosition.y = j;
				this.AppendGlyph(glyphInfo);
				this._gridInfo.Add(glyphInfo.gridPosition.x + this._targetSurfaceCollums * glyphInfo.gridPosition.y, glyphInfo);
			}
		}
		this.Finalize(pMesh);
		for (int k = 0; k < pCollums; k++)
		{
			for (int l = 0; l < pRows; l++)
			{
				this.SetCharacter(new IntPoint(k, l), ' ');
			}
		}
		this.ApplyToMesh(pMesh);
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0002DAF4 File Offset: 0x0002BCF4
	public void UseColor(Color pColor)
	{
		this._typingColor = pColor;
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0002DB00 File Offset: 0x0002BD00
	public void SetLine(int pCharPosY, int pXOffset, string pString, Color pColor)
	{
		this.UseColor(pColor);
		int i = pXOffset;
		foreach (char c in pString)
		{
			if (!this.DetectColor(c))
			{
				this.SetCharacter(new IntPoint(i++, pCharPosY), c, this._useInvertedColors);
			}
		}
		while (i < this._targetSurfaceCollums)
		{
			this.SetCharacter(new IntPoint(i, pCharPosY), ' ');
			i++;
		}
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0002DB80 File Offset: 0x0002BD80
	public void SetLine(int pCharPosY, int pXOffset, string pString)
	{
		this.SetLine(pCharPosY, pXOffset, pString, this._typingColor);
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0002DB94 File Offset: 0x0002BD94
	private TextGrid.GlyphInfo GetGlyph(int pX, int pY)
	{
		if (this._gridInfo.ContainsKey(pX + this._targetSurfaceCollums * pY))
		{
			return this._gridInfo[pX + this._targetSurfaceCollums * pY];
		}
		return null;
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x0002DBC8 File Offset: 0x0002BDC8
	private void AppendGlyph(TextGrid.GlyphInfo g)
	{
		g.meshIndex = this._vertices.Count;
		float num = (float)(this._targetSurfaceRows * TextGrid.GLYPH_SIZE);
		float num2 = num - ((float)((g.gridPosition.y + 1) * TextGrid.GLYPH_SIZE) + this._offsetPosition.y) - 1f;
		this.AddQuad(new Vector3(this._offsetPosition.x + (float)(g.gridPosition.x * TextGrid.GLYPH_SIZE), num2 + (float)TextGrid.GLYPH_SIZE), new Vector3(this._offsetPosition.x + (float)(g.gridPosition.x * TextGrid.GLYPH_SIZE) + (float)TextGrid.GLYPH_SIZE, num2 + (float)TextGrid.GLYPH_SIZE), new Vector3(this._offsetPosition.x + (float)(g.gridPosition.x * TextGrid.GLYPH_SIZE), num2), new Vector3(this._offsetPosition.x + (float)(g.gridPosition.x * TextGrid.GLYPH_SIZE) + (float)TextGrid.GLYPH_SIZE, num2));
		g.color = Color.white;
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0002DCDC File Offset: 0x0002BEDC
	private void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		int count = this._vertices.Count;
		this._vertices.Add(a);
		this._uvs.Add(new Vector2(0f, 0f));
		this._colors.Add(Color.white);
		this._vertices.Add(b);
		this._uvs.Add(new Vector2(1f, 0f));
		this._colors.Add(Color.white);
		this._vertices.Add(c);
		this._uvs.Add(new Vector2(0f, 1f));
		this._colors.Add(Color.white);
		this._vertices.Add(d);
		this._uvs.Add(new Vector2(1f, 1f));
		this._colors.Add(Color.white);
		int[] array = new int[]
		{
			count,
			count + 1,
			count + 3,
			count,
			count + 3,
			count + 2
		};
		this._triangles.AddRange(array);
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x0002DE04 File Offset: 0x0002C004
	private bool IsOutsideBounds(int x, int y)
	{
		return x >= this._targetSurfaceCollums || y >= this._targetSurfaceRows || x < 0 || y < 0;
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x0002DE38 File Offset: 0x0002C038
	public void SetCharacter(IntPoint pGridPos, char pChar, bool pUseInvert)
	{
		if (this.IsOutsideBounds(pGridPos.x, pGridPos.y))
		{
			return;
		}
		TextGrid.GlyphInfo glyph = this.GetGlyph(pGridPos.x, pGridPos.y);
		glyph.color = this._typingColor;
		glyph.character = pChar;
		glyph.inverted = pUseInvert;
		this.SetUv(glyph);
		this.SetColor(glyph);
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x0002DE9C File Offset: 0x0002C09C
	public void SetCharacter(IntPoint pGridPos, char pChar)
	{
		if (this.IsOutsideBounds(pGridPos.x, pGridPos.y))
		{
			return;
		}
		TextGrid.GlyphInfo glyph = this.GetGlyph(pGridPos.x, pGridPos.y);
		glyph.color = this._typingColor;
		glyph.character = pChar;
		this.SetUv(glyph);
		this.SetColor(glyph);
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0002DEFC File Offset: 0x0002C0FC
	public Color GetColor(int pX, int pY)
	{
		if (this.IsOutsideBounds(pX, pY))
		{
			return Color.cyan;
		}
		return this.GetGlyph(pX, pY).color;
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x0002DF2C File Offset: 0x0002C12C
	public void ToggleInvert(int pX, int pY, int pWidth, int pHeight, bool pInverted)
	{
		int num = pX + pWidth;
		int num2 = pY + pHeight;
		for (int i = pX; i < num; i++)
		{
			for (int j = pY; j < num2; j++)
			{
				this.ToggleInvert(i, j, pInverted);
			}
		}
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x0002DF70 File Offset: 0x0002C170
	public void ToggleInvert(int pX, int pY, bool pInverted)
	{
		if (this.IsOutsideBounds(pX, pY))
		{
			return;
		}
		TextGrid.GlyphInfo glyph = this.GetGlyph(pX, pY);
		if (glyph != null)
		{
			glyph.inverted = pInverted;
			this.SetUv(glyph);
		}
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x0002DFA8 File Offset: 0x0002C1A8
	public void ClearInvertColors()
	{
		this.UseInvert(false);
		foreach (TextGrid.GlyphInfo glyphInfo in this._gridInfo.Values)
		{
			glyphInfo.inverted = false;
			this.SetUv(glyphInfo);
		}
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x0002E024 File Offset: 0x0002C224
	private void Finalize(Mesh pMesh)
	{
		pMesh.vertices = this._vertices.ToArray();
		pMesh.uv = (this._uvsF = this._uvs.ToArray());
		pMesh.colors = (this._colorsF = this._colors.ToArray());
		pMesh.SetTriangles(this._triangles.ToArray(), 0);
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x0002E088 File Offset: 0x0002C288
	public void ApplyToMesh(Mesh pMesh)
	{
		pMesh.uv = this._uvsF;
		pMesh.colors = this._colorsF;
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x0002E0A4 File Offset: 0x0002C2A4
	private void SetUv(TextGrid.GlyphInfo pGlyph)
	{
		int num = (pGlyph.fontMapIndex = ((!pGlyph.inverted) ? this.GetCharIndex(pGlyph.character) : (this.GetCharIndex(pGlyph.character) + 128)));
		int num2 = num % this._sheetCollumCount;
		int num3 = this._sheetRowCount - 1 - num / this._sheetCollumCount;
		float num4 = (float)(num2 * TextGrid.GLYPH_SIZE) / (float)this._fontSheetWidth;
		float num5 = (float)(num3 * TextGrid.GLYPH_SIZE) / (float)this._fontSheetHeight;
		Vector3 vector = new Vector3(num4, num5, 0f);
		this._uvsF[pGlyph.meshIndex + 2] = TextGrid.ToVec2(vector + this._UV_BOTTOM_LEFT);
		this._uvsF[pGlyph.meshIndex] = TextGrid.ToVec2(vector + this._UV_TOP_LEFT);
		this._uvsF[pGlyph.meshIndex + 1] = TextGrid.ToVec2(vector + this._UV_TOP_RIGHT);
		this._uvsF[pGlyph.meshIndex + 3] = TextGrid.ToVec2(vector + this._UV_BOTTOM_RIGHT);
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x0002E1E0 File Offset: 0x0002C3E0
	private void SetColor(TextGrid.GlyphInfo pGlyph)
	{
		this._colorsF[pGlyph.meshIndex] = Color.Lerp(pGlyph.color, Color.white, 0.3f);
		this._colorsF[pGlyph.meshIndex + 1] = Color.Lerp(pGlyph.color, Color.white, 0.3f);
		this._colorsF[pGlyph.meshIndex + 2] = pGlyph.color;
		this._colorsF[pGlyph.meshIndex + 3] = pGlyph.color;
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x0002E284 File Offset: 0x0002C484
	private static Vector2 ToVec2(Vector3 pVec)
	{
		return new Vector2(pVec.x, pVec.y);
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x0002E29C File Offset: 0x0002C49C
	public bool DetectColor(char pChar)
	{
		switch (pChar)
		{
		case '\u3000':
			this.UseColor(CodeEditor.COLOR_BUILT_IN_TYPE);
			return true;
		case '、':
			this.UseColor(CodeEditor.COLOR_COMMENT);
			return true;
		case '。':
			this.UseColor(CodeEditor.COLOR_CURSOR);
			return true;
		case '〃':
			this.UseColor(CodeEditor.COLOR_DEFAULT);
			return true;
		case '〄':
			this.UseColor(CodeEditor.COLOR_ERROR);
			return true;
		case '々':
			this.UseColor(CodeEditor.COLOR_KEYWORD);
			return true;
		case '〆':
			this.UseColor(CodeEditor.COLOR_NUMBER);
			return true;
		case '〇':
			this.UseColor(CodeEditor.COLOR_QUOTED_STRING);
			return true;
		case '〈':
			this.UseColor(CodeEditorSuggestionMaker.COLOR_FUNCTION_DESCRIPTION);
			return true;
		case '〉':
			this.UseColor(CodeEditorSuggestionMaker.COLOR_FUNCTION_NAME);
			return true;
		case '【':
			this.UseColor(CodeEditorSuggestionMaker.COLOR_FUNCTION_SUGGESTION_BODY);
			return true;
		case '】':
			this.UseColor(CodeEditorSuggestionMaker.COLOR_FUNCTION_SUGGESTION_TITLE);
			return true;
		case '〒':
			this.UseColor(CodeEditorSuggestionMaker.COLOR_PARAMETER_SELECTION);
			return true;
		case '〓':
			this.UseColor(CodeEditorSuggestionMaker.COLOR_PARAMETER_DESCRIPTION);
			return true;
		case '〔':
			this.UseInvert(true);
			return true;
		case '〕':
			this.UseInvert(false);
			return true;
		}
		return false;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x0002E3E0 File Offset: 0x0002C5E0
	public void UseInvert(bool pToggle)
	{
		this._useInvertedColors = pToggle;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x0002E3EC File Offset: 0x0002C5EC
	private int GetCharIndex(char pChar)
	{
		switch (pChar)
		{
		case ' ':
			return 63;
		case '!':
			return 0;
		case '"':
			return 1;
		case '#':
			return 2;
		case '$':
			return 3;
		case '%':
			return 4;
		case '&':
			return 5;
		case '\'':
			return 97;
		case '(':
			return 7;
		case ')':
			return 8;
		case '*':
			return 9;
		case '+':
			return 10;
		case ',':
			return 11;
		case '-':
			return 12;
		case '.':
			return 13;
		case '/':
			return 14;
		case '0':
			return 15;
		case '1':
			return 16;
		case '2':
			return 17;
		case '3':
			return 18;
		case '4':
			return 19;
		case '5':
			return 20;
		case '6':
			return 21;
		case '7':
			return 22;
		case '8':
			return 23;
		case '9':
			return 24;
		case ':':
			return 25;
		case ';':
			return 26;
		case '<':
			return 27;
		case '=':
			return 28;
		case '>':
			return 29;
		case '?':
			return 30;
		case '@':
			return 31;
		case 'A':
			return 32;
		case 'B':
			return 33;
		case 'C':
			return 34;
		case 'D':
			return 35;
		case 'E':
			return 36;
		case 'F':
			return 37;
		case 'G':
			return 38;
		case 'H':
			return 39;
		case 'I':
			return 40;
		case 'J':
			return 41;
		case 'K':
			return 42;
		case 'L':
			return 43;
		case 'M':
			return 44;
		case 'N':
			return 45;
		case 'O':
			return 46;
		case 'P':
			return 47;
		case 'Q':
			return 48;
		case 'R':
			return 49;
		case 'S':
			return 50;
		case 'T':
			return 51;
		case 'U':
			return 52;
		case 'V':
			return 53;
		case 'W':
			return 54;
		case 'X':
			return 55;
		case 'Y':
			return 56;
		case 'Z':
			return 57;
		case '[':
			return 90;
		case '\\':
			return 110;
		case ']':
			return 92;
		case '^':
			return 93;
		case '_':
			return 98;
		case 'a':
			return 64;
		case 'b':
			return 65;
		case 'c':
			return 66;
		case 'd':
			return 67;
		case 'e':
			return 68;
		case 'f':
			return 69;
		case 'g':
			return 70;
		case 'h':
			return 71;
		case 'i':
			return 72;
		case 'j':
			return 73;
		case 'k':
			return 74;
		case 'l':
			return 75;
		case 'm':
			return 76;
		case 'n':
			return 77;
		case 'o':
			return 78;
		case 'p':
			return 79;
		case 'q':
			return 80;
		case 'r':
			return 81;
		case 's':
			return 82;
		case 't':
			return 83;
		case 'u':
			return 84;
		case 'v':
			return 85;
		case 'w':
			return 86;
		case 'x':
			return 87;
		case 'y':
			return 88;
		case 'z':
			return 89;
		case '{':
			return 61;
		case '|':
			return 60;
		case '}':
			return 62;
		case '~':
			return 95;
		}
		return 63;
	}

	// Token: 0x040004A5 RID: 1189
	public const char CONTROL_END_OF_TRANSMISSION = '\u0004';

	// Token: 0x040004A6 RID: 1190
	public const char CONTROL_SELECTION_ON = '〔';

	// Token: 0x040004A7 RID: 1191
	public const char CONTROL_SELECTION_OFF = '〕';

	// Token: 0x040004A8 RID: 1192
	private Dictionary<int, TextGrid.GlyphInfo> _gridInfo = new Dictionary<int, TextGrid.GlyphInfo>();

	// Token: 0x040004A9 RID: 1193
	private int _targetSurfaceCollums;

	// Token: 0x040004AA RID: 1194
	private int _targetSurfaceRows;

	// Token: 0x040004AB RID: 1195
	private int _sheetRowCount;

	// Token: 0x040004AC RID: 1196
	private int _sheetCollumCount;

	// Token: 0x040004AD RID: 1197
	private List<Vector3> _vertices = new List<Vector3>();

	// Token: 0x040004AE RID: 1198
	private List<Vector2> _uvs = new List<Vector2>();

	// Token: 0x040004AF RID: 1199
	private List<Color> _colors = new List<Color>();

	// Token: 0x040004B0 RID: 1200
	private List<int> _triangles = new List<int>();

	// Token: 0x040004B1 RID: 1201
	private Vector2[] _uvsF;

	// Token: 0x040004B2 RID: 1202
	private Color[] _colorsF;

	// Token: 0x040004B3 RID: 1203
	private static int GLYPH_SIZE = 8;

	// Token: 0x040004B4 RID: 1204
	private Vector3 _UV_BOTTOM_LEFT;

	// Token: 0x040004B5 RID: 1205
	private Vector3 _UV_TOP_LEFT;

	// Token: 0x040004B6 RID: 1206
	private Vector3 _UV_TOP_RIGHT;

	// Token: 0x040004B7 RID: 1207
	private Vector3 _UV_BOTTOM_RIGHT;

	// Token: 0x040004B8 RID: 1208
	private Vector3 _offsetPosition;

	// Token: 0x040004B9 RID: 1209
	private int _fontSheetWidth;

	// Token: 0x040004BA RID: 1210
	private int _fontSheetHeight;

	// Token: 0x040004BB RID: 1211
	private Color _typingColor;

	// Token: 0x040004BC RID: 1212
	private bool _useInvertedColors;

	// Token: 0x020000F2 RID: 242
	private class GlyphInfo
	{
		// Token: 0x040004BD RID: 1213
		public int fontMapIndex;

		// Token: 0x040004BE RID: 1214
		public int meshIndex;

		// Token: 0x040004BF RID: 1215
		public Color color;

		// Token: 0x040004C0 RID: 1216
		public IntPoint gridPosition;

		// Token: 0x040004C1 RID: 1217
		public bool inverted;

		// Token: 0x040004C2 RID: 1218
		public char character;
	}
}
