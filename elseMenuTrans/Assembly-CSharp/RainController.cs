using System;
using GameTypes;
using TingTing;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class RainController
{
	// Token: 0x060003C2 RID: 962 RVA: 0x0001B43C File Offset: 0x0001963C
	public RainController(Camera pCamera, RoomChanger pRoomChanger)
	{
		this._camera = pCamera;
		this._rainPrefab = Resources.Load("Rain");
		this._splashPrefab = Resources.Load("Splash");
		this._roomChanger = pRoomChanger;
		this._thunderTimer = global::UnityEngine.Random.Range(1f, 3f);
		this._delayedRainAmount = WorldOwner.instance.world.settings.rain;
		SoundDictionary.LoadSingleSound("MediumRain", "Rain medium sound");
		SoundDictionary.LoadMultiSound("ThunderLightning", "Thunder lightning sound", 2);
		SoundDictionary.LoadMultiSound("ThunderRumble", "Thunder rumble sound", 3);
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x0001B4DC File Offset: 0x000196DC
	public void Update(bool isCurrentRoomExterior)
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		if (this._rainTransform == null)
		{
			this._rainTransform = (global::UnityEngine.Object.Instantiate(this._rainPrefab) as GameObject).transform;
			global::UnityEngine.Object.DontDestroyOnLoad(this._rainTransform);
			this._rainParticleSystem = this._rainTransform.GetComponent<ParticleSystem>();
			this._thunderLamp = this._rainTransform.GetComponent<Light>();
			this._rainAudio = this._rainTransform.GetComponent<AudioSource>();
			this._thunderTransform = this._rainTransform.Find("Thunder");
			this._thunderLightningTransform = this._rainTransform.Find("Lightning");
			this._thunderRumbleAudio = this._thunderTransform.GetComponent<AudioSource>();
			this._thunderLightningAudio = this._thunderLightningTransform.GetComponent<AudioSource>();
			this._lowPassFilter = this._rainTransform.GetComponent<AudioLowPassFilter>();
		}
		if (this._splashParticleSystem == null)
		{
			this._splashParticleSystem = (global::UnityEngine.Object.Instantiate(this._splashPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject).particleSystem;
		}
		float rain = WorldOwner.instance.world.settings.rain;
		float rainTargetValue = WorldOwner.instance.world.settings.rainTargetValue;
		float num = Mathf.Sign(rainTargetValue - rain);
		if (rain < 1f)
		{
			this._rainAudio.Stop();
		}
		else if (!this._rainAudio.isPlaying && rain > 1f)
		{
			SoundDictionary.PlaySound("MediumRain", this._rainAudio);
		}
		float num2 = 0.9f * (rain / 250f);
		if (!isCurrentRoomExterior || this._roomChanger.currentRoom == "Empty")
		{
			this._lowPassFilter.cutoffFrequency = 800f;
			this._rainAudio.volume = 0f;
			this._thunderRumbleAudio.volume = 0.2f;
			this.StopAllFx();
			return;
		}
		float num3 = ((num <= 0f) ? 1000f : 20f);
		float num4 = WorldOwner.instance.world.settings.rain - this._delayedRainAmount;
		this._delayedRainAmount += Time.deltaTime * Mathf.Sign(num4) * num3;
		this._rainAudio.volume = num2;
		this._thunderRumbleAudio.volume = 0.3f;
		this._lowPassFilter.cutoffFrequency = 44000f;
		this._rainParticleSystem.enableEmission = true;
		this._thunderLamp.enabled = true;
		this._splashParticleSystem.enableEmission = true;
		this._rainTransform.position = this._camera.transform.position + this._camera.transform.forward * 30f + new Vector3(0f, 5f, 0f);
		this._rainParticleSystem.emissionRate = this._delayedRainAmount - 10f;
		Room room = WorldOwner.instance.world.roomRunner.GetRoom(this._roomChanger.currentRoom);
		int num5 = room.points.Length;
		float num6 = (float)num5 * 7E-05f;
		int num7 = (int)(this._delayedRainAmount * num6);
		float num8 = 0.5f;
		for (int i = 0; i < num7; i++)
		{
			IntPoint intPoint = room.points[global::UnityEngine.Random.Range(0, num5)];
			this._splashParticleSystem.Emit(MimanHelper.TilePositionToVector3(intPoint) + new Vector3(Randomizer.GetValue(-num8, num8), 0.2f, Randomizer.GetValue(-num8, num8)), Vector3.zero, global::UnityEngine.Random.Range(0.8f, 2.5f), 0.75f, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		}
		if (WorldOwner.instance.world.settings.rain >= 100f)
		{
			if (this._thunderTimer < 0f)
			{
				this.ThunderStrike();
				this._thunderTimer = global::UnityEngine.Random.Range(20f, 40f);
			}
			else
			{
				this._thunderTimer -= Time.deltaTime;
				if (this._thunderLamp.intensity > 0f)
				{
					this._thunderLamp.intensity -= 200f * Time.deltaTime;
				}
			}
		}
		else
		{
			this._thunderLamp.intensity = 0f;
		}
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0001B980 File Offset: 0x00019B80
	private void StopAllFx()
	{
		this._rainParticleSystem.enableEmission = false;
		this._rainParticleSystem.Clear();
		this._thunderLamp.enabled = false;
		this._splashParticleSystem.enableEmission = false;
		this._thunderRumbleAudio.Stop();
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0001B9C8 File Offset: 0x00019BC8
	private void ThunderStrike()
	{
		this._thunderLamp.intensity = 200f;
		this._thunderRumbleAudio.pitch = global::UnityEngine.Random.Range(1f, 1.1f);
		this._thunderLightningAudio.pitch = global::UnityEngine.Random.Range(1f, 1.1f);
		if (Randomizer.OneIn(4))
		{
			SoundDictionary.PlaySound("ThunderLightning", this._thunderLightningAudio);
		}
		SoundDictionary.PlaySoundDelayed("ThunderRumble", this._thunderRumbleAudio, global::UnityEngine.Random.Range(0.1f, 3f));
	}

	// Token: 0x040002DB RID: 731
	private Camera _camera;

	// Token: 0x040002DC RID: 732
	private Transform _rainTransform;

	// Token: 0x040002DD RID: 733
	private ParticleSystem _rainParticleSystem;

	// Token: 0x040002DE RID: 734
	private ParticleSystem _splashParticleSystem;

	// Token: 0x040002DF RID: 735
	private global::UnityEngine.Object _rainPrefab;

	// Token: 0x040002E0 RID: 736
	private global::UnityEngine.Object _splashPrefab;

	// Token: 0x040002E1 RID: 737
	private RoomChanger _roomChanger;

	// Token: 0x040002E2 RID: 738
	private float _thunderTimer;

	// Token: 0x040002E3 RID: 739
	private Light _thunderLamp;

	// Token: 0x040002E4 RID: 740
	private AudioSource _rainAudio;

	// Token: 0x040002E5 RID: 741
	private AudioSource _thunderRumbleAudio;

	// Token: 0x040002E6 RID: 742
	private AudioSource _thunderLightningAudio;

	// Token: 0x040002E7 RID: 743
	private Transform _thunderTransform;

	// Token: 0x040002E8 RID: 744
	private Transform _thunderLightningTransform;

	// Token: 0x040002E9 RID: 745
	private AudioLowPassFilter _lowPassFilter;

	// Token: 0x040002EA RID: 746
	private float _delayedRainAmount;
}
