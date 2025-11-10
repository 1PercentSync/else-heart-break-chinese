using System;
using GameWorld2;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class TramController : MonoBehaviour
{
	// Token: 0x06000450 RID: 1104 RVA: 0x0001E70C File Offset: 0x0001C90C
	private void Start()
	{
		this._pling = base.transform.Find("Pling").audio;
		this._startStopSound = base.transform.Find("StartStop").audio;
		this._going = this.tramIsGoing;
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000451 RID: 1105 RVA: 0x0001E75C File Offset: 0x0001C95C
	private bool tramIsGoing
	{
		get
		{
			return this.tram != null && this.tram.speed > 0.1f;
		}
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x0001E780 File Offset: 0x0001C980
	private void OnDestroy()
	{
		if (WorldOwner.instance.worldIsLoaded && this.tram != null)
		{
			this.tram.safetySwitchOn = false;
			Tram tram = this.tram;
			tram.onNewNavNode = (Vehicle.OnNewNavNode)Delegate.Remove(tram.onNewNavNode, new Vehicle.OnNewNavNode(this.OnNewNavNode));
		}
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x0001E7DC File Offset: 0x0001C9DC
	private void OnNewNavNode(NavNode pOldNavNode, NavNode pNewNavNode)
	{
		if (pNewNavNode.isStation)
		{
			RunGameWorld.instance.ShowNotification("Station: " + pNewNavNode.room.name);
		}
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0001E814 File Offset: 0x0001CA14
	private void Update()
	{
		if (this.tram == null)
		{
			this.tram = WorldOwner.instance.world.tingRunner.GetTingUnsafe(this.tramName) as Tram;
			if (this.tram == null)
			{
				Debug.Log("Tram is null");
				return;
			}
			Tram tram = this.tram;
			tram.onNewNavNode = (Vehicle.OnNewNavNode)Delegate.Combine(tram.onNewNavNode, new Vehicle.OnNewNavNode(this.OnNewNavNode));
		}
		if (this.tramDoor == null)
		{
			this.tramDoor = WorldOwner.instance.world.tingRunner.GetTingUnsafe(this.tramDoorName) as Door;
		}
		if (this.tramDoor == null)
		{
			Debug.Log("Tram door is null");
			return;
		}
		if (this.tramDoor.actionName == "Opening")
		{
			this.tram.safetySwitchOn = true;
		}
		else
		{
			this.tram.safetySwitchOn = false;
		}
		if (this.tramIsGoing)
		{
			this.tramDoor.isLocked = true;
			TramShell.PlaySoundIfNotAlreadyPlaying(base.audio, "Tram Running Sound");
			if (!this._going)
			{
				TramShell.PlaySoundIfNotAlreadyPlaying(this._pling, "Tram Horn Sound");
				TramShell.PlaySoundIfNotAlreadyPlaying(this._startStopSound, "Tram Start Sound");
				this._going = true;
			}
		}
		else
		{
			this.tramDoor.isLocked = false;
			TramShell.PlaySoundIfNotAlreadyPlaying(base.audio, "Tram Idle Sound");
			if (this._going)
			{
				TramShell.PlaySoundIfNotAlreadyPlaying(this._startStopSound, "Tram Stop Sound");
				this._going = false;
			}
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000455 RID: 1109 RVA: 0x0001E9A8 File Offset: 0x0001CBA8
	public float tramSpeed
	{
		get
		{
			if (this.tram == null)
			{
				return 0f;
			}
			return this.tram.speed;
		}
	}

	// Token: 0x04000353 RID: 851
	public string tramName;

	// Token: 0x04000354 RID: 852
	public string tramDoorName;

	// Token: 0x04000355 RID: 853
	public Tram tram;

	// Token: 0x04000356 RID: 854
	public Door tramDoor;

	// Token: 0x04000357 RID: 855
	private AudioSource _pling;

	// Token: 0x04000358 RID: 856
	private AudioSource _startStopSound;

	// Token: 0x04000359 RID: 857
	private bool _going;
}
