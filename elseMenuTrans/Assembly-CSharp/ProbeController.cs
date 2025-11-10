using System;
using GrimmLib;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class ProbeController : MonoBehaviour
{
	// Token: 0x060003BE RID: 958 RVA: 0x0001B1C4 File Offset: 0x000193C4
	private void Start()
	{
		this._arm = base.transform.Find("MinistryPROBEArm");
		this._animation = this._arm.GetComponent<Animation>();
		if (this._animation == null)
		{
			Debug.LogError("_animation is null");
		}
		this._arm.gameObject.SetActive(false);
		WorldOwner.instance.world.dialogueRunner.AddOnEventListener(new DialogueRunner.OnEvent(this.OnStoryEvent));
		this._glowEffect = Camera.main.GetComponent<GlowEffect>();
		this._glowEffect.enabled = false;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001B260 File Offset: 0x00019460
	private void OnStoryEvent(string pEventName)
	{
		if (pEventName == "FoldUpProbe")
		{
			Debug.Log("Got event FoldUpProbe!");
			this._arm.gameObject.SetActive(true);
			this._animation.Play("PROBEFoldUp");
		}
		else if (pEventName == "FoldOutFlower")
		{
			Debug.Log("Got event FoldOutFlower!");
			Transform transform = GameObject.Find("MinistryPROBE").transform;
			if (transform == null)
			{
				Debug.Log("Can't find MinistryPROBE");
			}
			transform.animation.Play("PROBEFlowerWarmUp");
			this._laser = GameObject.Find("Laser").transform;
			if (this._laser == null)
			{
				Debug.Log("Can't find Laser");
			}
			this._laser.particleSystem.enableEmission = true;
		}
		else if (pEventName == "StartShootingWithProbe")
		{
			Debug.Log("Got event StartShootingWithProbe!");
			Transform transform2 = GameObject.Find("MinistryPROBE").transform;
			if (transform2 == null)
			{
				Debug.Log("Can't find MinistryPROBE");
			}
			transform2.animation.Play("PROBEFlowerShoot");
			this._startedShooting = true;
		}
		else if (pEventName == "Boom")
		{
			this._beam = GameObject.Find("LaserBeam").transform;
			this._beam.particleSystem.Play();
		}
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0001B3D8 File Offset: 0x000195D8
	private void Update()
	{
		if (this._startedShooting)
		{
			this._glowEffect.enabled = true;
		}
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0001B3F4 File Offset: 0x000195F4
	private void OnDestroy()
	{
		this._glowEffect.enabled = false;
		if (WorldOwner.instance.worldIsLoaded)
		{
			WorldOwner.instance.world.dialogueRunner.RemoveOnEventListener(new DialogueRunner.OnEvent(this.OnStoryEvent));
		}
	}

	// Token: 0x040002D5 RID: 725
	private Animation _animation;

	// Token: 0x040002D6 RID: 726
	private bool _startedShooting;

	// Token: 0x040002D7 RID: 727
	private GlowEffect _glowEffect;

	// Token: 0x040002D8 RID: 728
	private Transform _arm;

	// Token: 0x040002D9 RID: 729
	private Transform _laser;

	// Token: 0x040002DA RID: 730
	private Transform _beam;
}
