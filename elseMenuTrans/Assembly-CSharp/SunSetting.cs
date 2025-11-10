using System;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class SunSetting
{
	// Token: 0x0600064B RID: 1611 RVA: 0x00029544 File Offset: 0x00027744
	public static SunSetting Mix(SunSetting pFirst, SunSetting pSecond, float pFraction)
	{
		SunSetting sunSetting = new SunSetting();
		float num = 1f - pFraction;
		sunSetting.angle = pFirst.angle * num + pSecond.angle * pFraction;
		sunSetting.shadowStrength = pFirst.shadowStrength * num + pSecond.shadowStrength * pFraction;
		sunSetting.intensity = pFirst.intensity * num + pSecond.intensity * pFraction;
		sunSetting.color = pFirst.color * num + pSecond.color * pFraction;
		sunSetting.backlightIntensity = pFirst.backlightIntensity * num + pSecond.backlightIntensity * pFraction;
		sunSetting.backlightColor = pFirst.backlightColor * num + pSecond.backlightColor * pFraction;
		return sunSetting;
	}

	// Token: 0x04000414 RID: 1044
	public float angle = 45f;

	// Token: 0x04000415 RID: 1045
	public float shadowStrength = 0.5f;

	// Token: 0x04000416 RID: 1046
	public float intensity = 1f;

	// Token: 0x04000417 RID: 1047
	public Color color = Color.white;

	// Token: 0x04000418 RID: 1048
	public float backlightIntensity = 0.1f;

	// Token: 0x04000419 RID: 1049
	public Color backlightColor = Color.white;
}
