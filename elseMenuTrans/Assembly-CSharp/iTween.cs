using System;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class iTween
{
	// Token: 0x060002A4 RID: 676 RVA: 0x0001298C File Offset: 0x00010B8C
	public static Vector3 nonCappedLerp(Vector3 start, Vector3 end, float value)
	{
		return start + (end - start) * value;
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x000129A4 File Offset: 0x00010BA4
	public static float linear(float start, float end, float value)
	{
		return start + (end - start) * value;
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x000129B0 File Offset: 0x00010BB0
	public static float clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float num5;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			num5 = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			num5 = start + num4;
		}
		else
		{
			num5 = start + (end - start) * value;
		}
		return num5;
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00012A28 File Offset: 0x00010C28
	public static float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00012A8C File Offset: 0x00010C8C
	public static float spring2(float start, float end, float t)
	{
		float num = Mathf.Pow(16.30968f, -5f * (t + 0.26f)) * Mathf.Sin(28.274334f * (t + 0.21f)) + 1f;
		return start + (end - start) * num;
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x00012AD4 File Offset: 0x00010CD4
	public static float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	// Token: 0x060002AA RID: 682 RVA: 0x00012AE4 File Offset: 0x00010CE4
	public static float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2f) + start;
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00012AFC File Offset: 0x00010CFC
	public static float easeInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value + start;
		}
		value -= 1f;
		return -end / 2f * (value * (value - 2f) - 1f) + start;
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00012B54 File Offset: 0x00010D54
	public static float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00012B64 File Offset: 0x00010D64
	public static float easeOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00012B84 File Offset: 0x00010D84
	public static float easeInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value + 2f) + start;
	}

	// Token: 0x060002AF RID: 687 RVA: 0x00012BD8 File Offset: 0x00010DD8
	public static float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x00012BEC File Offset: 0x00010DEC
	public static float easeOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * (value * value * value * value - 1f) + start;
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00012C1C File Offset: 0x00010E1C
	public static float easeInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value + start;
		}
		value -= 2f;
		return -end / 2f * (value * value * value * value - 2f) + start;
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00012C78 File Offset: 0x00010E78
	public static float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00012C8C File Offset: 0x00010E8C
	public static float easeOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00012CB0 File Offset: 0x00010EB0
	public static float easeInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value * value * value + 2f) + start;
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00012D0C File Offset: 0x00010F0C
	public static float easeInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value / 1f * 1.5707964f) + end + start;
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00012D2C File Offset: 0x00010F2C
	public static float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value / 1f * 1.5707964f) + start;
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00012D4C File Offset: 0x00010F4C
	public static float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.1415927f * value / 1f) - 1f) + start;
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00012D84 File Offset: 0x00010F84
	public static float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00012DB8 File Offset: 0x00010FB8
	public static float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00012DE4 File Offset: 0x00010FE4
	public static float easeInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00012E58 File Offset: 0x00011058
	public static float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00012E78 File Offset: 0x00011078
	public static float easeOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00012EA8 File Offset: 0x000110A8
	public static float easeInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00012F18 File Offset: 0x00011118
	public static float bounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.95454544f;
		return end * (7.5625f * value * value + 0.984375f) + start;
	}

	// Token: 0x060002BF RID: 703 RVA: 0x00012FC0 File Offset: 0x000111C0
	public static float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00012FF4 File Offset: 0x000111F4
	public static float easeOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value = value / 1f - 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00013034 File Offset: 0x00011234
	public static float easeInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x000130B4 File Offset: 0x000112B4
	public static float punch(float amplitude, float value)
	{
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num = 0.3f;
		float num2 = num / 6.2831855f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.2831855f / num);
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0001312C File Offset: 0x0001132C
	public static float elastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) + end + start;
	}

	// Token: 0x040001A0 RID: 416
	public static readonly iTween.MultiDimTween<Vector3> vector3Bounce = new iTween.MultiDimTween<Vector3>(new iTween.MultiDimTween<Vector3>.InterpolationSampler(Vector3.Lerp), new iTween.MultiDimTween<Vector3>.OneDimLerp(iTween.bounce));

	// Token: 0x040001A1 RID: 417
	public static readonly iTween.MultiDimTween<Vector3> vector3spring = new iTween.MultiDimTween<Vector3>(new iTween.MultiDimTween<Vector3>.InterpolationSampler(iTween.nonCappedLerp), new iTween.MultiDimTween<Vector3>.OneDimLerp(iTween.spring2));

	// Token: 0x040001A2 RID: 418
	public static readonly iTween.MultiDimTween<Vector3> vector3easeInOutSine = new iTween.MultiDimTween<Vector3>(new iTween.MultiDimTween<Vector3>.InterpolationSampler(Vector3.Lerp), new iTween.MultiDimTween<Vector3>.OneDimLerp(iTween.easeInOutSine));

	// Token: 0x040001A3 RID: 419
	public static readonly iTween.MultiDimTween<Vector2> vector2Bounce = new iTween.MultiDimTween<Vector2>(new iTween.MultiDimTween<Vector2>.InterpolationSampler(Vector2.Lerp), new iTween.MultiDimTween<Vector2>.OneDimLerp(iTween.bounce));

	// Token: 0x040001A4 RID: 420
	public static readonly iTween.MultiDimTween<Vector2> vector2easeInOutSine = new iTween.MultiDimTween<Vector2>(new iTween.MultiDimTween<Vector2>.InterpolationSampler(Vector2.Lerp), new iTween.MultiDimTween<Vector2>.OneDimLerp(iTween.easeInOutSine));

	// Token: 0x040001A5 RID: 421
	public static readonly iTween.MultiDimTween<Vector2> vector2easeInOutExpo = new iTween.MultiDimTween<Vector2>(new iTween.MultiDimTween<Vector2>.InterpolationSampler(Vector2.Lerp), new iTween.MultiDimTween<Vector2>.OneDimLerp(iTween.easeInOutExpo));

	// Token: 0x040001A6 RID: 422
	public static readonly iTween.MultiDimTween<Vector2> vector2spring = new iTween.MultiDimTween<Vector2>(new iTween.MultiDimTween<Vector2>.InterpolationSampler(Vector2.Lerp), new iTween.MultiDimTween<Vector2>.OneDimLerp(iTween.spring));

	// Token: 0x02000046 RID: 70
	public class MultiDimTween<T>
	{
		// Token: 0x060002C4 RID: 708 RVA: 0x000131DC File Offset: 0x000113DC
		public MultiDimTween(iTween.MultiDimTween<T>.InterpolationSampler pMultiDimSampler, iTween.MultiDimTween<T>.OneDimLerp pSingleTimensionLerp)
		{
			this.a = pMultiDimSampler;
			this.b = pSingleTimensionLerp;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000131F4 File Offset: 0x000113F4
		public T Sample(T pStart, T pEnd, float pValue)
		{
			return this.a(pStart, pEnd, this.b(0f, 1f, pValue));
		}

		// Token: 0x040001A7 RID: 423
		private iTween.MultiDimTween<T>.InterpolationSampler a;

		// Token: 0x040001A8 RID: 424
		private iTween.MultiDimTween<T>.OneDimLerp b;

		// Token: 0x020000FD RID: 253
		// (Invoke) Token: 0x06000753 RID: 1875
		public delegate float OneDimLerp(float pStart, float pEnd, float pValue);

		// Token: 0x020000FE RID: 254
		// (Invoke) Token: 0x06000757 RID: 1879
		public delegate T InterpolationSampler(T pStart, T pEnd, float pValue);
	}
}
