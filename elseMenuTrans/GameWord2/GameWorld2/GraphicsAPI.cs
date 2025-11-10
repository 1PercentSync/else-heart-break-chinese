using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;

namespace GameWorld2
{
	// Token: 0x0200004E RID: 78
	public class GraphicsAPI
	{
		// Token: 0x0600052C RID: 1324 RVA: 0x0001905C File Offset: 0x0001725C
		public GraphicsAPI(Computer pComputer)
		{
			this._computer = pComputer;
		}

		// Token: 0x0600052D RID: 1325
		[SprakAPI(new string[] { "Draw a line on the screen在屏幕上画一条线" })]
		public void API_Line(float x1, float y1, float x2, float y2)
		{
			if (this._computer.onLineDrawing != null)
			{
				this._computer.onLineDrawing(new IntPoint((int)x1, (int)y1), new IntPoint((int)x2, (int)y2));
			}
		}

		// Token: 0x0600052E RID: 1326
		[SprakAPI(new string[] { "Draw text in a specific place在特定位置绘制文本", "X position in character coordinates X 坐标", "Y position in character coordinates Y 坐标", "The text to print要打印的文本" })]
		public void API_Text(float x, float y, string text)
		{
			if (this._computer.onTextDrawing != null)
			{
				this._computer.onTextDrawing((int)x, (int)y, text);
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000190EC File Offset: 0x000172EC
		public object Lines(object[] args)
		{
			if (args[0].GetType() != typeof(SortedDictionary<KeyWrapper, object>))
			{
				throw new Error("Must get an array of points必须提供一个点数组");
			}
			if (this._computer.onLineDrawing != null)
			{
				SortedDictionary<KeyWrapper, object> sortedDictionary = ReturnValueConversions.SafeUnwrap<SortedDictionary<KeyWrapper, object>>(args, 0);
				float num = 0f;
				float num2 = 0f;
				bool flag = true;
				foreach (KeyValuePair<KeyWrapper, object> keyValuePair in sortedDictionary)
				{
					float num3 = (float)keyValuePair.Value;
					SortedDictionary<KeyWrapper, object>.Enumerator enumerator;
					if (!enumerator.MoveNext())
					{
						break;
					}
					KeyValuePair<KeyWrapper, object> keyValuePair2 = enumerator.Current;
					float num4 = (float)keyValuePair2.Value;
					if (!flag)
					{
						this._computer.onLineDrawing(new IntPoint((int)num3, (int)num4), new IntPoint((int)num, (int)num2));
					}
					num = num3;
					num2 = num4;
					flag = false;
				}
			}
			else
			{
				D.Log(this._computer.name + " doesn't have onLineDrawing set");
			}
			return VoidType.voidType;
		}

		// Token: 0x06000530 RID: 1328
		[SprakAPI(new string[] { "Draw a rectangle on the screen在屏幕上画一个矩形" })]
		public void API_Rect(float x1, float y1, float x2, float y2)
		{
			if (this._computer.onRectDrawing != null)
			{
				this._computer.onRectDrawing(new IntPoint((int)x1, (int)y1), new IntPoint((int)x2, (int)y2));
			}
		}

		// Token: 0x06000531 RID: 1329
		[SprakAPI(new string[] { "Clear the screen and display graphical elements清空屏幕并显示图形" })]
		public void API_DisplayGraphics()
		{
			if (this._computer.onDisplayGraphics != null)
			{
				this._computer.onDisplayGraphics();
			}
		}

		// Token: 0x06000532 RID: 1330
		[SprakAPI(new string[] { "Set the color to draw or print text with设置绘制或打印文本的颜色" })]
		public void API_Color(float r, float g, float b)
		{
			if (this._computer.onSetColor != null)
			{
				this._computer.onSetColor(r, g, b);
			}
		}

		// Token: 0x06000533 RID: 1331
		[SprakAPI(new string[] { "Keep a value between 0 and an upper bound使一个值保持在0和上限之间", "Value值", "Upper bound上限" })]
		public float API_Repeat(float x, float bound)
		{
			return x - (float)Math.Floor((double)(x / bound)) * bound;
		}

		// Token: 0x06000534 RID: 1332
		[SprakAPI(new string[] { "Hue, Saturation, Value -> [r, g, b] 色相,饱和度,亮度 -> [红,绿,蓝]", "Hue色相", "Saturation饱和度", "Value亮度" })]
		public SortedDictionary<KeyWrapper, object> API_HSVtoRGB(float H, float S, float V)
		{
			Float3 @float = new Float3(1f, 1f, 1f);
			if (S == 0f)
			{
				@float.x = V;
				@float.y = V;
				@float.z = V;
			}
			else if (V == 0f)
			{
				@float.x = 0f;
				@float.y = 0f;
				@float.z = 0f;
			}
			else
			{
				@float.x = 0f;
				@float.y = 0f;
				@float.z = 0f;
				float num7 = H * 6f;
				int num2 = (int)Math.Floor((double)num7);
				float num3 = num7 - (float)num2;
				float num4 = V * (1f - S);
				float num5 = V * (1f - S * num3);
				float num6 = V * (1f - S * (1f - num3));
				switch (num2 + 1)
				{
				case 0:
					@float.x = V;
					@float.y = num4;
					@float.z = num5;
					break;
				case 1:
					@float.x = V;
					@float.y = num6;
					@float.z = num4;
					break;
				case 2:
					@float.x = num5;
					@float.y = V;
					@float.z = num4;
					break;
				case 3:
					@float.x = num4;
					@float.y = V;
					@float.z = num6;
					break;
				case 4:
					@float.x = num4;
					@float.y = num5;
					@float.z = V;
					break;
				case 5:
					@float.x = num6;
					@float.y = num4;
					@float.z = V;
					break;
				case 6:
					@float.x = V;
					@float.y = num4;
					@float.z = num5;
					break;
				case 7:
					@float.x = V;
					@float.y = num6;
					@float.z = num4;
					break;
				}
				@float.x = this.Clamp01(@float.x);
				@float.y = this.Clamp01(@float.y);
				@float.z = this.Clamp01(@float.z);
			}
			return new SortedDictionary<KeyWrapper, object>
			{
				{
					new KeyWrapper(0),
					@float.x
				},
				{
					new KeyWrapper(1),
					@float.y
				},
				{
					new KeyWrapper(2),
					@float.z
				}
			};
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001955C File Offset: 0x0001775C
		private float Clamp01(float x)
		{
			if (x < 0f)
			{
				return 0f;
			}
			if (x > 1f)
			{
				return 1f;
			}
			return x;
		}

		// Token: 0x06000536 RID: 1334
		[SprakAPI(new string[] { "[r, g, b] -> Hue, Saturation, Value [红,绿,蓝] -> 色相,饱和度,亮度", "Red红", "Green绿", "Blue蓝" })]
		public SortedDictionary<KeyWrapper, object> API_RGBToHSV(float r, float g, float b)
		{
			float num;
			float num2;
			float num3;
			if (b > g && b > r)
			{
				GraphicsAPI.RGBToHSVHelper(4f, b, r, g, out num, out num2, out num3);
			}
			else if (g > r)
			{
				GraphicsAPI.RGBToHSVHelper(2f, g, b, r, out num, out num2, out num3);
			}
			else
			{
				GraphicsAPI.RGBToHSVHelper(0f, r, g, b, out num, out num2, out num3);
			}
			return new SortedDictionary<KeyWrapper, object>
			{
				{
					new KeyWrapper(0),
					num
				},
				{
					new KeyWrapper(1),
					num2
				},
				{
					new KeyWrapper(2),
					num3
				}
			};
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00019638 File Offset: 0x00017838
		private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float H, out float S, out float V)
		{
			V = dominantcolor;
			if (V != 0f)
			{
				float num;
				if (colorone > colortwo)
				{
					num = colortwo;
				}
				else
				{
					num = colorone;
				}
				float num2 = V - num;
				if (num2 != 0f)
				{
					S = num2 / V;
					H = offset + (colorone - colortwo) / num2;
				}
				else
				{
					S = 0f;
					H = offset + (colorone - colortwo);
				}
				H /= 6f;
				if (H < 0f)
				{
					H += 1f;
				}
			}
			else
			{
				S = 0f;
				H = 0f;
			}
		}

		// Token: 0x04000154 RID: 340
		private Computer _computer;
	}
}
