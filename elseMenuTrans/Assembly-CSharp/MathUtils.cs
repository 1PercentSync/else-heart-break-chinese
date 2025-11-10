using System;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class MathUtils
{
	// Token: 0x0600028D RID: 653 RVA: 0x00011E14 File Offset: 0x00010014
	public static float GetQuatLength(Quaternion q)
	{
		return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
	}

	// Token: 0x0600028E RID: 654 RVA: 0x00011E68 File Offset: 0x00010068
	public static Quaternion GetQuatConjugate(Quaternion q)
	{
		return new Quaternion(-q.x, -q.y, -q.z, q.w);
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00011E9C File Offset: 0x0001009C
	public static Quaternion GetQuatLog(Quaternion q)
	{
		Quaternion quaternion = q;
		quaternion.w = 0f;
		if (Mathf.Abs(q.w) < 1f)
		{
			float num = Mathf.Acos(q.w);
			float num2 = Mathf.Sin(num);
			if ((double)Mathf.Abs(num2) > 0.0001)
			{
				float num3 = num / num2;
				quaternion.x = q.x * num3;
				quaternion.y = q.y * num3;
				quaternion.z = q.z * num3;
			}
		}
		return quaternion;
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00011F2C File Offset: 0x0001012C
	public static Quaternion GetQuatExp(Quaternion q)
	{
		Quaternion quaternion = q;
		float num = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z);
		float num2 = Mathf.Sin(num);
		quaternion.w = Mathf.Cos(num);
		if ((double)Mathf.Abs(num2) > 0.0001)
		{
			float num3 = num2 / num;
			quaternion.x = num3 * q.x;
			quaternion.y = num3 * q.y;
			quaternion.z = num3 * q.z;
		}
		return quaternion;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00011FD0 File Offset: 0x000101D0
	public static Quaternion GetQuatSquad(float t, Quaternion q0, Quaternion q1, Quaternion a0, Quaternion a1)
	{
		float num = 2f * t * (1f - t);
		Quaternion quaternion = MathUtils.Slerp(q0, q1, t);
		Quaternion quaternion2 = MathUtils.Slerp(a0, a1, t);
		return MathUtils.Slerp(quaternion, quaternion2, num);
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00012008 File Offset: 0x00010208
	public static Quaternion GetSquadIntermediate(Quaternion q0, Quaternion q1, Quaternion q2)
	{
		Quaternion quatConjugate = MathUtils.GetQuatConjugate(q1);
		Quaternion quatLog = MathUtils.GetQuatLog(quatConjugate * q0);
		Quaternion quatLog2 = MathUtils.GetQuatLog(quatConjugate * q2);
		Quaternion quaternion = new Quaternion(-0.25f * (quatLog.x + quatLog2.x), -0.25f * (quatLog.y + quatLog2.y), -0.25f * (quatLog.z + quatLog2.z), -0.25f * (quatLog.w + quatLog2.w));
		return q1 * MathUtils.GetQuatExp(quaternion);
	}

	// Token: 0x06000293 RID: 659 RVA: 0x000120A0 File Offset: 0x000102A0
	public static float Ease(float t, float k1, float k2)
	{
		float num = k1 * 2f / 3.1415927f + k2 - k1 + (1f - k2) * 2f / 3.1415927f;
		float num2;
		if (t < k1)
		{
			num2 = k1 * 0.63661975f * (Mathf.Sin(t / k1 * 3.1415927f / 2f - 1.5707964f) + 1f);
		}
		else if (t < k2)
		{
			num2 = 2f * k1 / 3.1415927f + t - k1;
		}
		else
		{
			num2 = 2f * k1 / 3.1415927f + k2 - k1 + (1f - k2) * 0.63661975f * Mathf.Sin((t - k2) / (1f - k2) * 3.1415927f / 2f);
		}
		return num2 / num;
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00012168 File Offset: 0x00010368
	public static Quaternion Slerp(Quaternion p, Quaternion q, float t)
	{
		float num = Quaternion.Dot(p, q);
		Quaternion quaternion;
		if ((double)(1f + num) > 1E-05)
		{
			float num4;
			float num5;
			if ((double)(1f - num) > 1E-05)
			{
				float num2 = Mathf.Acos(num);
				float num3 = 1f / Mathf.Sin(num2);
				num4 = Mathf.Sin((1f - t) * num2) * num3;
				num5 = Mathf.Sin(t * num2) * num3;
			}
			else
			{
				num4 = 1f - t;
				num5 = t;
			}
			quaternion.x = num4 * p.x + num5 * q.x;
			quaternion.y = num4 * p.y + num5 * q.y;
			quaternion.z = num4 * p.z + num5 * q.z;
			quaternion.w = num4 * p.w + num5 * q.w;
		}
		else
		{
			float num6 = Mathf.Sin((1f - t) * 3.1415927f * 0.5f);
			float num7 = Mathf.Sin(t * 3.1415927f * 0.5f);
			quaternion.x = num6 * p.x - num7 * p.y;
			quaternion.y = num6 * p.y + num7 * p.x;
			quaternion.z = num6 * p.z - num7 * p.w;
			quaternion.w = p.z;
		}
		return quaternion;
	}
}
