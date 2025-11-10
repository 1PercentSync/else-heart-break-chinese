using System;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class SunController
{
	// Token: 0x06000647 RID: 1607 RVA: 0x00029018 File Offset: 0x00027218
	public SunController(DayCycleSettings pDayCycleSettings)
	{
		this._dayCycleSettings = pDayCycleSettings;
		this.SetupSunSettings();
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x00029030 File Offset: 0x00027230
	private void SetupSunSettings()
	{
		this._sunSettings = new SunSetting[]
		{
			new SunSetting(),
			new SunSetting(),
			new SunSetting(),
			new SunSetting()
		};
		this._sunSettings[0].angle = this._dayCycleSettings.NIGHT_angle;
		this._sunSettings[0].shadowStrength = this._dayCycleSettings.NIGHT_shadowStrength;
		this._sunSettings[0].intensity = this._dayCycleSettings.NIGHT_intensity;
		this._sunSettings[0].color = this._dayCycleSettings.NIGHT_color;
		this._sunSettings[0].backlightIntensity = this._dayCycleSettings.NIGHT_backlightIntensity;
		this._sunSettings[0].backlightColor = this._dayCycleSettings.NIGHT_backlightColor;
		this._sunSettings[1].angle = this._dayCycleSettings.MORNING_angle;
		this._sunSettings[1].shadowStrength = this._dayCycleSettings.MORNING_shadowStrength;
		this._sunSettings[1].intensity = this._dayCycleSettings.MORNING_intensity;
		this._sunSettings[1].color = this._dayCycleSettings.MORNING_color;
		this._sunSettings[1].backlightIntensity = this._dayCycleSettings.MORNING_backlightIntensity;
		this._sunSettings[1].backlightColor = this._dayCycleSettings.MORNING_backlightColor;
		this._sunSettings[2].angle = this._dayCycleSettings.LUNCH_angle;
		this._sunSettings[2].shadowStrength = this._dayCycleSettings.LUNCH_shadowStrength;
		this._sunSettings[2].intensity = this._dayCycleSettings.LUNCH_intensity;
		this._sunSettings[2].color = this._dayCycleSettings.LUNCH_color;
		this._sunSettings[2].backlightIntensity = this._dayCycleSettings.LUNCH_backlightIntensity;
		this._sunSettings[2].backlightColor = this._dayCycleSettings.LUNCH_backlightColor;
		this._sunSettings[3].angle = this._dayCycleSettings.EVENING_angle;
		this._sunSettings[3].shadowStrength = this._dayCycleSettings.EVENING_shadowStrength;
		this._sunSettings[3].intensity = this._dayCycleSettings.EVENING_intensity;
		this._sunSettings[3].color = this._dayCycleSettings.EVENING_color;
		this._sunSettings[3].backlightIntensity = this._dayCycleSettings.EVENING_backlightIntensity;
		this._sunSettings[3].backlightColor = this._dayCycleSettings.EVENING_backlightColor;
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x000292AC File Offset: 0x000274AC
	public void UpdateSun(float pNormalizedTime, bool pExteriorScene)
	{
		if (this._sun == null)
		{
			GameObject gameObject = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Sun"));
			this._sun = gameObject.GetComponent<Light>();
			GameObject gameObject2 = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Sun"));
			this._backlight = gameObject2.GetComponent<Light>();
		}
		if (this._sun != null)
		{
			float num = Mathf.Sin(pNormalizedTime * 3.1415927f);
			float num2 = 1f;
			float num3 = 1f;
			float num4 = 1f;
			float num5 = 0f;
			bool flag = true;
			if (!pExteriorScene)
			{
				num3 = 0.7f;
				num2 = 0.3f;
				num4 = 0.5f;
				num5 = 30f;
				flag = false;
			}
			float num6 = pNormalizedTime * 4f;
			int num7 = (int)num6 + 1;
			float num8 = (float)num7 - num6;
			float num9 = 1f - num8;
			if (num7 >= 4)
			{
				num7 = 0;
			}
			SunSetting sunSetting = SunSetting.Mix(this._sunSettings[(int)num6], this._sunSettings[num7], num9);
			this._sun.transform.rotation = Quaternion.identity;
			float num10 = Mathf.Clamp(sunSetting.angle + num5, 0f, 90f);
			float num11 = pNormalizedTime * 360f + 180f;
			this._sun.transform.Rotate(num10, num11, 0f, Space.World);
			this._backlight.transform.rotation = Quaternion.identity;
			this._backlight.transform.Rotate(num10 + 180f, num11, 0f, Space.World);
			this._backlight.enabled = flag;
			this._sun.color = sunSetting.color;
			this._sun.shadowStrength = sunSetting.shadowStrength * num4;
			this._sun.intensity = sunSetting.intensity * num3;
			this._backlight.color = sunSetting.backlightColor;
			this._backlight.shadowStrength = 0f;
			this._backlight.intensity = sunSetting.backlightIntensity * num3;
			float num12 = Mathf.Clamp(num2 * num, 0f, 255f);
			RenderSettings.ambientLight = new Color(num12, num12, num12);
		}
	}

	// Token: 0x0400040F RID: 1039
	private Light _sun;

	// Token: 0x04000410 RID: 1040
	private Light _backlight;

	// Token: 0x04000411 RID: 1041
	private SunController _sunController;

	// Token: 0x04000412 RID: 1042
	private SunSetting[] _sunSettings;

	// Token: 0x04000413 RID: 1043
	private DayCycleSettings _dayCycleSettings;
}
