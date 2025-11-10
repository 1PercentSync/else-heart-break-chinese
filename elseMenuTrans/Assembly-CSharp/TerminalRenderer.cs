using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using UnityEngine;

// Token: 0x020000E8 RID: 232
[RequireComponent(typeof(MeshRenderer))]
public class TerminalRenderer : MonoBehaviour
{
	// Token: 0x06000697 RID: 1687 RVA: 0x0002B2B8 File Offset: 0x000294B8
	public void Awake()
	{
		if (this.fontSheet == null || this.fontSheet.mainTexture == null)
		{
			Debug.LogError("fontsheet is null!");
		}
		this._targetTexture = new RenderTexture(this.renderTextureWidth, this.renderTextureHeight, 0, RenderTextureFormat.ARGB32);
		this._targetTexture.isPowerOfTwo = true;
		this._targetTexture.wrapMode = TextureWrapMode.Clamp;
		this._targetTexture.filterMode = FilterMode.Bilinear;
		this._targetTexture.Create();
		this._fontSheetTexture = (Texture2D)this.fontSheet.mainTexture;
		this._textMesh = new Mesh();
		this._textMesh.hideFlags = HideFlags.HideAndDontSave;
		int num = 0;
		int num2 = 0;
		this.SetRect(num, -num2, this.renderTextureWidth - num, this.renderTextureHeight - num2);
		this._textMesh.RecalculateBounds();
		if (base.renderer.material.HasProperty("_MainTex"))
		{
			base.renderer.material.SetTexture("_MainTex", this._targetTexture);
		}
		else
		{
			base.renderer.material.mainTexture = this._targetTexture;
		}
		TerminalRenderer.CreateLineMaterial();
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0002B3EC File Offset: 0x000295EC
	public void SetCharacter(int pCharPosX, int pCharPosY, char pChar, Color pColor)
	{
		this._textRenderer.UseColor(pColor);
		this._textRenderer.SetCharacter(new IntPoint(pCharPosX, pCharPosY), pChar);
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0002B41C File Offset: 0x0002961C
	public void SetCharacter(int pCharPosX, int pCharPosY, char pChar)
	{
		this._textRenderer.SetCharacter(new IntPoint(pCharPosX, pCharPosY), pChar);
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0002B434 File Offset: 0x00029634
	public void SetStringAtPos(int pCharPosX, int pCharPosY, string pString)
	{
		for (int i = 0; i < pString.Length; i++)
		{
			int num = pCharPosX + i;
			this._textRenderer.SetCharacter(new IntPoint(num, pCharPosY), pString[i]);
		}
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0002B478 File Offset: 0x00029678
	public void ApplyTextChanges()
	{
		this._textRenderer.ApplyToMesh(this._textMesh);
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0002B48C File Offset: 0x0002968C
	private void DrawClearQuad()
	{
		TerminalRenderer.lineMaterial.SetPass(0);
		GL.Begin(7);
		GL.Color(new Color(this._currentBackgroundColor.r, this._currentBackgroundColor.g, this._currentBackgroundColor.b, this.clearBetweenFramesAmount));
		GL.Vertex3(-10f, -10f, 0f);
		GL.Vertex3(-10f, (float)this._targetTexture.height + 10f, 0f);
		GL.Vertex3((float)this._targetTexture.width + 10f, (float)this._targetTexture.height + 10f, 0f);
		GL.Vertex3((float)this._targetTexture.width + 10f, -10f, 0f);
		GL.End();
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0002B568 File Offset: 0x00029768
	private static void CreateLineMaterial()
	{
		if (!TerminalRenderer.lineMaterial)
		{
			TerminalRenderer.lineMaterial = new Material("Shader \"Lines/Colored Blended\" {SubShader { Pass {  Blend SrcAlpha OneMinusSrcAlpha  ZWrite Off Cull Off Fog { Mode Off }  BindChannels { Bind \"vertex\", vertex Bind \"color\", color }} } }");
			TerminalRenderer.lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			TerminalRenderer.lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x0002B5A8 File Offset: 0x000297A8
	public void ClearScreen()
	{
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = this._targetTexture;
		this.GLBegin();
		this.DrawClearQuad();
		this.GLEnd();
		RenderTexture.active = active;
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x0002B5E0 File Offset: 0x000297E0
	public void OnRenderObject()
	{
		if (this.screenRefreshCounter > 0f)
		{
			return;
		}
		this.screenRefreshCounter = 0.016666668f;
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = this._targetTexture;
		this.GLBegin();
		this.DrawClearQuad();
		if (this.readBuffer.Count > 0)
		{
			TerminalRenderer.lineMaterial.SetPass(0);
			foreach (TerminalRenderer.IGraphicsDrawCommand graphicsDrawCommand in this.readBuffer)
			{
				graphicsDrawCommand.Draw(this._rectX, this._rectY, this._rectWidth, this._rectHeight);
			}
		}
		if (this._textMesh != null)
		{
			for (int i = 0; i < this.fontSheet.passCount; i++)
			{
				this.fontSheet.SetPass(i);
				Graphics.DrawMeshNow(this._textMesh, Camera.current.cameraToWorldMatrix);
			}
		}
		this.GLEnd();
		RenderTexture.active = active;
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x0002B710 File Offset: 0x00029910
	public void Update()
	{
		this.screenRefreshCounter -= Time.deltaTime;
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0002B724 File Offset: 0x00029924
	private void GLBegin()
	{
		GL.InvalidateState();
		GL.PushMatrix();
		GL.LoadPixelMatrix(0f, (float)this._targetTexture.width, 0f, (float)this._targetTexture.height);
		GL.Viewport(new Rect(0f, 0f, (float)this._targetTexture.width, (float)this._targetTexture.height));
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x0002B790 File Offset: 0x00029990
	private void GLEnd()
	{
		GL.PopMatrix();
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0002B798 File Offset: 0x00029998
	public void SetRect(int pX, int pY, int pWidth, int pHeight)
	{
		this._rectX = pX;
		this._rectY = pY;
		this._rectWidth = pWidth;
		this._rectHeight = pHeight;
		this._textRenderer = new TextGrid(this._rectWidth / 8, this._rectHeight / 8, this._fontSheetTexture.width, this._fontSheetTexture.height, this._textMesh, new Vector3((float)this._rectX, (float)this._rectY, 0f));
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x0002B814 File Offset: 0x00029A14
	public void ClearInvertColors()
	{
		this._textRenderer.ClearInvertColors();
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x0002B824 File Offset: 0x00029A24
	public void ToggleInvertColors(int pX, int pY, int pWidth, int pHeight, bool pInverted)
	{
		this._textRenderer.ToggleInvert(pX, pY, pWidth, pHeight, pInverted);
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x0002B838 File Offset: 0x00029A38
	public void ToggleInvertColor(int pX, int pY, bool pInverted)
	{
		this._textRenderer.ToggleInvert(pX, pY, pInverted);
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x0002B848 File Offset: 0x00029A48
	public void SetLine(int pCharPosY, string pString)
	{
		this.SetLine(pCharPosY, 0, pString);
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x0002B854 File Offset: 0x00029A54
	public void SetLine(int pCharPosY, int pXOffset, string pString)
	{
		this._textRenderer.SetLine(pCharPosY, pXOffset, pString);
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x0002B864 File Offset: 0x00029A64
	private Color[] ModColor(Color[] pSource, Color pForegroundColor)
	{
		return this.ModColor(pSource, pForegroundColor, this._currentBackgroundColor);
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x0002B874 File Offset: 0x00029A74
	private Color[] ModColor(Color[] pSource, Color pForegroundColor, Color pBackgroundColor)
	{
		Color[] array = new Color[pSource.Length];
		for (int i = 0; i < pSource.Length; i++)
		{
			float num = pSource[i].grayscale * pForegroundColor.r + (1f - pSource[i].grayscale) * pBackgroundColor.r;
			float num2 = pSource[i].grayscale * pForegroundColor.g + (1f - pSource[i].grayscale) * pBackgroundColor.g;
			float num3 = pSource[i].grayscale * pForegroundColor.b + (1f - pSource[i].grayscale) * pBackgroundColor.b;
			array[i] = new Color(num, num2, num3);
		}
		return array;
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x0002B948 File Offset: 0x00029B48
	public void UseInvert(bool pToggle)
	{
		this._textRenderer.UseInvert(pToggle);
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x0002B958 File Offset: 0x00029B58
	public void UseColor(Color pForegroundColor)
	{
		this._textRenderer.UseColor(pForegroundColor);
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x0002B968 File Offset: 0x00029B68
	public void UseForegroundColor()
	{
		this._textRenderer.UseColor(this._foregroundColor);
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x0002B97C File Offset: 0x00029B7C
	public Color GetColor(int pX, int pY)
	{
		return this._textRenderer.GetColor(pX, pY);
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x060006AF RID: 1711 RVA: 0x0002B98C File Offset: 0x00029B8C
	public int TextRowCount
	{
		get
		{
			return this._rectHeight / 8;
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0002B998 File Offset: 0x00029B98
	public int TextCollumCount
	{
		get
		{
			return this._rectWidth / 8;
		}
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x0002B9A4 File Offset: 0x00029BA4
	public void DrawGLLine(IntPoint pA, IntPoint pB, Float3 pColor)
	{
		if (this.writeBuffer.Count > 256)
		{
			throw new Error("Can't draw more objects!");
		}
		TerminalRenderer.LineDrawCommand lineDrawCommand = default(TerminalRenderer.LineDrawCommand);
		lineDrawCommand.p1 = pA;
		lineDrawCommand.p2 = pB;
		lineDrawCommand.color.r = pColor.x;
		lineDrawCommand.color.g = pColor.y;
		lineDrawCommand.color.b = pColor.z;
		lineDrawCommand.color.a = 1f;
		this.writeBuffer.Enqueue(lineDrawCommand);
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x0002BA44 File Offset: 0x00029C44
	public void DrawGLQuad(IntPoint pA, IntPoint pB, Float3 pColor)
	{
		if (this.writeBuffer.Count > 256)
		{
			throw new Error("Can't draw more objects!");
		}
		TerminalRenderer.QuadDrawCommand quadDrawCommand = default(TerminalRenderer.QuadDrawCommand);
		IntPoint intPoint = new IntPoint(Mathf.Max(pA.x, pB.x), Mathf.Max(pA.y, pB.y));
		IntPoint intPoint2 = new IntPoint(Mathf.Min(pA.x, pB.x), Mathf.Min(pA.y, pB.y));
		quadDrawCommand.p1 = new IntPoint(intPoint2.x, intPoint.y);
		quadDrawCommand.p2 = new IntPoint(intPoint.x, intPoint.y);
		quadDrawCommand.p3 = new IntPoint(intPoint.x, intPoint2.y);
		quadDrawCommand.p4 = new IntPoint(intPoint2.x, intPoint2.y);
		quadDrawCommand.color.r = pColor.x;
		quadDrawCommand.color.g = pColor.y;
		quadDrawCommand.color.b = pColor.z;
		quadDrawCommand.color.a = 1f;
		this.writeBuffer.Enqueue(quadDrawCommand);
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x0002BB98 File Offset: 0x00029D98
	public void SwapBuffers()
	{
		Queue<TerminalRenderer.IGraphicsDrawCommand> queue = this.writeBuffer;
		this.writeBuffer = this.readBuffer;
		this.readBuffer = queue;
		this.writeBuffer.Clear();
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x0002BBCC File Offset: 0x00029DCC
	public bool DetectColor(char pChar)
	{
		return this._textRenderer.DetectColor(pChar);
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0002BBDC File Offset: 0x00029DDC
	public Vector2 dimensions
	{
		get
		{
			return new Vector2((float)this.renderTextureWidth, (float)this.renderTextureHeight);
		}
	}

	// Token: 0x04000460 RID: 1120
	private const float SCREEN_REFRESH_RATE = 0.016666668f;

	// Token: 0x04000461 RID: 1121
	public const int GLYPH_SIZE = 8;

	// Token: 0x04000462 RID: 1122
	private float screenRefreshCounter = 0.016666668f;

	// Token: 0x04000463 RID: 1123
	private TextGrid _textRenderer;

	// Token: 0x04000464 RID: 1124
	private Texture2D _fontSheetTexture;

	// Token: 0x04000465 RID: 1125
	private RenderTexture _targetTexture;

	// Token: 0x04000466 RID: 1126
	private Queue<TerminalRenderer.IGraphicsDrawCommand> writeBuffer = new Queue<TerminalRenderer.IGraphicsDrawCommand>();

	// Token: 0x04000467 RID: 1127
	private Queue<TerminalRenderer.IGraphicsDrawCommand> readBuffer = new Queue<TerminalRenderer.IGraphicsDrawCommand>();

	// Token: 0x04000468 RID: 1128
	private Mesh _textMesh;

	// Token: 0x04000469 RID: 1129
	private float _uvWidth;

	// Token: 0x0400046A RID: 1130
	private float _uvHeight;

	// Token: 0x0400046B RID: 1131
	private Color _lineColor;

	// Token: 0x0400046C RID: 1132
	private static Material lineMaterial;

	// Token: 0x0400046D RID: 1133
	private int _rectX = 20;

	// Token: 0x0400046E RID: 1134
	private int _rectY = 20;

	// Token: 0x0400046F RID: 1135
	private int _rectWidth = 100;

	// Token: 0x04000470 RID: 1136
	private int _rectHeight = 100;

	// Token: 0x04000471 RID: 1137
	public Material fontSheet;

	// Token: 0x04000472 RID: 1138
	public int renderTextureWidth = 512;

	// Token: 0x04000473 RID: 1139
	public int renderTextureHeight = 256;

	// Token: 0x04000474 RID: 1140
	public float clearBetweenFramesAmount = 1f;

	// Token: 0x04000475 RID: 1141
	public Color _currentBackgroundColor = Color.black;

	// Token: 0x04000476 RID: 1142
	public Color _foregroundColor = Color.white;

	// Token: 0x020000E9 RID: 233
	public interface IGraphicsDrawCommand
	{
		// Token: 0x060006B6 RID: 1718
		void Draw(int pContextX, int pContextY, int pContextWidth, int pContextHeight);
	}

	// Token: 0x020000EA RID: 234
	public struct LineDrawCommand : TerminalRenderer.IGraphicsDrawCommand
	{
		// Token: 0x060006B7 RID: 1719 RVA: 0x0002BBF4 File Offset: 0x00029DF4
		public void Draw(int pContextX, int pContextY, int pContextWidth, int pContextHeight)
		{
			TerminalRenderer.lineMaterial.SetPass(0);
			GL.Begin(1);
			GL.Color(this.color);
			GL.Vertex3((float)this.p1.x, (float)(pContextHeight - this.p1.y), 0f);
			GL.Color(this.color);
			GL.Vertex3((float)this.p2.x, (float)(pContextHeight - this.p2.y), 0f);
			GL.End();
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0002BC78 File Offset: 0x00029E78
		public override string ToString()
		{
			return string.Format("p1:{0} p2:{1} color:{2}", this.p1, this.p2, this.color);
		}

		// Token: 0x04000477 RID: 1143
		public Color color;

		// Token: 0x04000478 RID: 1144
		public IntPoint p1;

		// Token: 0x04000479 RID: 1145
		public IntPoint p2;
	}

	// Token: 0x020000EB RID: 235
	public struct QuadDrawCommand : TerminalRenderer.IGraphicsDrawCommand
	{
		// Token: 0x060006B9 RID: 1721 RVA: 0x0002BCA8 File Offset: 0x00029EA8
		public void Draw(int pContextX, int pContextY, int pContextWidth, int pContextHeight)
		{
			GL.Begin(7);
			GL.Color(this.color);
			GL.Vertex3((float)this.p1.x, (float)(pContextHeight - this.p1.y), 0f);
			GL.Color(this.color);
			GL.Vertex3((float)this.p2.x, (float)(pContextHeight - this.p2.y), 0f);
			GL.Color(this.color);
			GL.Vertex3((float)this.p3.x, (float)(pContextHeight - this.p3.y), 0f);
			GL.Color(this.color);
			GL.Vertex3((float)this.p4.x, (float)(pContextHeight - this.p4.y), 0f);
			GL.End();
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002BD80 File Offset: 0x00029F80
		public override string ToString()
		{
			return string.Format("[QuadDrawCommand]", new object[0]);
		}

		// Token: 0x0400047A RID: 1146
		public Color color;

		// Token: 0x0400047B RID: 1147
		public IntPoint p1;

		// Token: 0x0400047C RID: 1148
		public IntPoint p2;

		// Token: 0x0400047D RID: 1149
		public IntPoint p3;

		// Token: 0x0400047E RID: 1150
		public IntPoint p4;
	}
}
