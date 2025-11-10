using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class Scaled
{
	// Token: 0x1700001B RID: 27
	// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005D1C File Offset: 0x00003F1C
	public static Vector2 MousePos
	{
		get
		{
			Vector3 mousePosition = Input.mousePosition;
			return Scaled.TransformInputPoint(new Vector2(mousePosition.x, mousePosition.y));
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00005D48 File Offset: 0x00003F48
	public static Vector2 TransformInputPoint(Vector2 pPoint)
	{
		float num = 1f / Scaled.Factor;
		return new Vector2(pPoint.x * num, (Camera.main.pixelHeight - pPoint.y) * num);
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005D84 File Offset: 0x00003F84
	public static float Factor
	{
		get
		{
			return Camera.main.pixelHeight / 768f;
		}
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00005D98 File Offset: 0x00003F98
	public static Vector2 VectorTwo(Vector2 unscaledVector2)
	{
		float factor = Scaled.Factor;
		return new Vector2(unscaledVector2.x * factor, unscaledVector2.y * factor);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00005DC4 File Offset: 0x00003FC4
	public static Rect Rectangle(Rect unscaledRect)
	{
		float factor = Scaled.Factor;
		return new Rect(unscaledRect.x * factor, unscaledRect.y * factor, unscaledRect.width * factor, unscaledRect.height * factor);
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00005E00 File Offset: 0x00004000
	public static Rect Rectangle(float x, float y, float width, float height)
	{
		float factor = Scaled.Factor;
		return new Rect(x * factor, y * factor, width * factor, height * factor);
	}

	// Token: 0x0400007B RID: 123
	public const float SCREEN_WIDTH = 1024f;

	// Token: 0x0400007C RID: 124
	public const float SCREEN_HEIGHT = 768f;
}
