using System;
using System.Collections.Generic;
using GameWorld2;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class SinkShell : Shell
{
	// Token: 0x1700009D RID: 157
	// (get) Token: 0x060005F4 RID: 1524 RVA: 0x000278C4 File Offset: 0x00025AC4
	public Sink sink
	{
		get
		{
			return base.ting as Sink;
		}
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x000278D4 File Offset: 0x00025AD4
	public override void CreateTing()
	{
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x000278D8 File Offset: 0x00025AD8
	protected override void Setup()
	{
		base.Setup();
		List<ParticleSystem> list = new List<ParticleSystem>();
		Transform transform = base.transform.Find("Water");
		Transform transform2 = base.transform.Find("Water/Splash");
		if (transform != null)
		{
			list.Add(transform.GetComponent<ParticleSystem>());
		}
		if (transform2 != null)
		{
			list.Add(transform2.GetComponent<ParticleSystem>());
		}
		this._waterParticles = list.ToArray();
		this.RefreshWater();
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x00027958 File Offset: 0x00025B58
	private void SetParticlesEnabled(bool val)
	{
		foreach (ParticleSystem particleSystem in this._waterParticles)
		{
			particleSystem.enableEmission = val;
		}
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0002798C File Offset: 0x00025B8C
	private void RefreshWater()
	{
		if (this.sink.on && !this.sink.containsBrokenPrograms)
		{
			this.SetParticlesEnabled(true);
			if (base.audio != null)
			{
				base.audio.enabled = true;
				if (!base.audio.isPlaying)
				{
					base.audio.Play();
				}
			}
		}
		else
		{
			this.SetParticlesEnabled(false);
			if (base.audio != null)
			{
				base.audio.enabled = false;
			}
		}
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x00027A24 File Offset: 0x00025C24
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
		this.RefreshWater();
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x00027A34 File Offset: 0x00025C34
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x040003F1 RID: 1009
	private ParticleSystem[] _waterParticles;
}
