using System;
using GameWorld2;
using GrimmLib;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class HeartController : MonoBehaviour
{
	// Token: 0x06000351 RID: 849 RVA: 0x00018D98 File Offset: 0x00016F98
	private void Start()
	{
		if (base.name != "Heart")
		{
			Debug.Log("Don't use this script on any other object than Heart");
			return;
		}
		this._animator = base.transform.GetComponent<Animator>();
		if (this._animator == null)
		{
			Debug.LogError("_animator is null");
		}
		WorldOwner.instance.world.dialogueRunner.AddOnEventListener(new DialogueRunner.OnEvent(this.OnStoryEvent));
		SoundDictionary.LoadSingleSound("HeartCries", "HeartExplotionBleedingSound 1");
		this._heart = WorldOwner.instance.world.tingRunner.GetTing<Computer>("Heart");
		this._heartAtmo2 = GameObject.Find("Heartatmo 2").audio;
		if (this._heart.containsBrokenPrograms || WorldOwner.instance.world.settings.heartIsBroken)
		{
			this.StartBeingBroken();
		}
		else
		{
			this._animator.Play("HeartIdle");
		}
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00018E98 File Offset: 0x00017098
	private void StartBeingBroken()
	{
		this.broken = true;
		this._animator.Play("HeartHurt");
		SoundDictionary.PlaySound("HeartCries", base.audio);
		this._heartAtmo2.Stop();
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00018ED8 File Offset: 0x000170D8
	private void Update()
	{
		if (this.broken)
		{
			return;
		}
		if (this._heart.containsBrokenPrograms)
		{
			this.StartBeingBroken();
		}
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00018F08 File Offset: 0x00017108
	private void OnStoryEvent(string pEventName)
	{
		if (pEventName == "TheHeartIsBroken")
		{
			Debug.Log("The heart got TheHeartIsBroken event");
			this._animator.Play("HeartExplode");
			this.broken = true;
		}
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00018F3C File Offset: 0x0001713C
	private void OnDestroy()
	{
		if (WorldOwner.instance.worldIsLoaded)
		{
			WorldOwner.instance.world.dialogueRunner.RemoveOnEventListener(new DialogueRunner.OnEvent(this.OnStoryEvent));
		}
	}

	// Token: 0x04000270 RID: 624
	private Animator _animator;

	// Token: 0x04000271 RID: 625
	private Computer _heart;

	// Token: 0x04000272 RID: 626
	private AudioSource _heartAtmo2;

	// Token: 0x04000273 RID: 627
	public bool broken;
}
