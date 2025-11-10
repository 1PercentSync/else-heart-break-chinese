using System;
using GameWorld2;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class TramShell : Shell
{
	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x0600061B RID: 1563 RVA: 0x00027C40 File Offset: 0x00025E40
	public Tram tram
	{
		get
		{
			return base.ting as Tram;
		}
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x00027C50 File Offset: 0x00025E50
	public override void CreateTing()
	{
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00027C54 File Offset: 0x00025E54
	protected override void Setup()
	{
		base.Setup();
		this._animator = base.GetComponentInChildren<Animator>();
		this._sparks = base.GetComponentInChildren<ParticleSystem>();
		this._engineSound = base.transform.FindChild("EngineSound").audio;
		this._sparksSound = this._sparks.audio;
		this._tramHornSound = base.transform.FindChild("PlingSound").audio;
		this._startStopSound = base.transform.FindChild("StartStopSound").audio;
		this._doorSnapPoint = base.transform.Find("Tram").transform.Find("TramBody").transform.Find("TramBody_TramBody").transform.Find("DoorSnapPoint");
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00027D24 File Offset: 0x00025F24
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x00027D28 File Offset: 0x00025F28
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
		this.SafetyCollisionChecks();
		if (this.targetNode != null && !this._startSnap)
		{
			base.transform.LookAt(this.targetNode.transform.position);
			this._startSnap = true;
		}
		if (this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.TramStop"))
		{
			this._sparkAmount = 100f;
			TramShell.PlaySoundIfNotAlreadyPlaying(this._startStopSound, "Tram Stop Sound");
		}
		else if (this._animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.TramStart"))
		{
			TramShell.PlaySoundIfNotAlreadyPlaying(this._startStopSound, "Tram Start Sound");
			TramShell.PlaySoundIfNotAlreadyPlaying(this._tramHornSound, "Tram Horn Sound");
		}
		else if (this.tram.speed < 1f)
		{
			this._sparkAmount = 0f;
		}
		else
		{
			this._sparkAmount = 15f;
		}
		NavNode currentNavNode = this.tram.currentNavNode;
		NavNode nextNavNode = this.tram.nextNavNode;
		if (currentNavNode == null)
		{
			Debug.Log(base.name + "'s current node is null");
			return;
		}
		if (nextNavNode == null)
		{
			Debug.Log(base.name + "'s next node is null");
			return;
		}
		if (currentNavNode.position.roomName != nextNavNode.position.roomName)
		{
			base.transform.position = new Vector3(1000f, 1000f, 1000f);
			return;
		}
		Vector3 vector = MimanHelper.TilePositionToVector3(currentNavNode.localPoint);
		Vector3 vector2 = MimanHelper.TilePositionToVector3(nextNavNode.localPoint);
		Vector3 vector3 = vector2 - vector;
		Vector3 vector4 = vector + vector3 * this.tram.distanceFraction;
		base.transform.position = vector4;
		if (this.targetNode)
		{
			Vector3 vector5 = new Vector3(this.targetNode.transform.position.x, 0f, this.targetNode.transform.position.z);
			Vector3 vector6 = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
			Quaternion quaternion = Quaternion.LookRotation(vector5 - vector6);
			float num = Mathf.Abs(quaternion.eulerAngles.y - base.transform.rotation.eulerAngles.y);
			if (num > 1f && num < 359f)
			{
				this._sparkAmount = 100f;
			}
			this._inTurn = num > 1.5f;
			Quaternion quaternion2 = Quaternion.Slerp(base.transform.rotation, quaternion, Time.deltaTime * this.turnSpeed);
			base.transform.rotation = quaternion2;
		}
		else
		{
			Debug.Log("No node to look at for " + base.name);
		}
		this._animator.SetFloat(TramShell.SPEED_ID, this.tram.speed);
		this._animator.SetBool(TramShell.SAFETY_SWITCH_ID, this.tram.safetySwitchOn);
		this._sparks.emissionRate = this._sparkAmount;
		this._sparksSound.volume = this._sparkAmount * 0.01f;
		if (this.tram.speed > 0.1f)
		{
			TramShell.PlaySoundIfNotAlreadyPlaying(this._engineSound, "Tram Running Sound");
		}
		else
		{
			TramShell.PlaySoundIfNotAlreadyPlaying(this._engineSound, "Tram Idle Sound");
		}
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x000280EC File Offset: 0x000262EC
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Vector3 vector = base.transform.position + base.transform.forward * this.colliderDistance;
		Gizmos.DrawSphere(vector, this.colliderRadius);
		Vector3 vector2 = base.transform.position + base.transform.forward * 10f;
		Gizmos.DrawSphere(vector2, this.colliderRadius);
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00028168 File Offset: 0x00026368
	private void SafetyCollisionChecks()
	{
		if (this._inTurn)
		{
			return;
		}
		RaycastHit[] array = Physics.SphereCastAll(base.transform.position + base.transform.forward * 10f, this.colliderRadius, base.transform.forward, this.colliderDistance);
		CharacterShell characterShell = null;
		TramGate tramGate = null;
		TramShell tramShell = null;
		foreach (RaycastHit raycastHit in array)
		{
			TramShell component = raycastHit.transform.GetComponent<TramShell>();
			if (component != null && component != this)
			{
				tramShell = component;
			}
			else
			{
				CharacterShell component2 = raycastHit.transform.GetComponent<CharacterShell>();
				if (component2 != null)
				{
					characterShell = component2;
				}
				else
				{
					TramGate component3 = raycastHit.transform.GetComponent<TramGate>();
					if (component3 != null && raycastHit.distance < 10f)
					{
						tramGate = component3;
					}
				}
			}
		}
		if (characterShell != null)
		{
			Debug.Log("Character in the way!");
			this.tram.safetySwitchOn = true;
		}
		else if (tramShell != null)
		{
			Debug.Log("Tram in the way!");
			this.tram.safetySwitchOn = true;
		}
		else if (tramGate != null)
		{
			if (!tramGate.opened)
			{
				if (this.tram.speed > 0f)
				{
					tramGate.Open();
				}
				this.tram.safetySwitchOn = true;
			}
		}
		RaycastHit[] array3 = Physics.SphereCastAll(base.transform.position + base.transform.forward * -5f, this.colliderRadius, base.transform.forward * -1f, this.colliderDistance);
		foreach (RaycastHit raycastHit2 in array3)
		{
			TramGate component4 = raycastHit2.transform.GetComponent<TramGate>();
			if (component4 != null)
			{
				component4.Close();
			}
		}
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x000283A4 File Offset: 0x000265A4
	private void LateUpdate()
	{
		if (this._doorSnapPoint != null)
		{
			if (this._doorTransform == null)
			{
				GameObject gameObject = GameObject.Find(this.tram.movingDoorName);
				if (gameObject != null)
				{
					this._doorTransform = gameObject.transform;
				}
			}
			if (this._doorTransform != null)
			{
				this._doorTransform.position = this._doorSnapPoint.position;
				this._doorTransform.rotation = this._doorSnapPoint.rotation;
			}
		}
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002843C File Offset: 0x0002663C
	public static void PlaySoundIfNotAlreadyPlaying(AudioSource pAudioSource, string pName)
	{
		if (pAudioSource.clip == null || pAudioSource.audio.name != pName)
		{
			pAudioSource.clip = Resources.Load(pName) as AudioClip;
			if (!pAudioSource.isPlaying)
			{
				pAudioSource.Play();
			}
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06000624 RID: 1572 RVA: 0x00028494 File Offset: 0x00026694
	private Shell targetNode
	{
		get
		{
			return ShellManager.GetShellWithName(this.tram.nextNavNodeName);
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06000625 RID: 1573 RVA: 0x000284A8 File Offset: 0x000266A8
	public override Vector3 lookTargetPoint
	{
		get
		{
			return base.transform.position + base.transform.forward * 3f;
		}
	}

	// Token: 0x040003F3 RID: 1011
	private Animator _animator;

	// Token: 0x040003F4 RID: 1012
	private static readonly int SPEED_ID = Animator.StringToHash("Speed");

	// Token: 0x040003F5 RID: 1013
	private static readonly int SAFETY_SWITCH_ID = Animator.StringToHash("SafetySwitch");

	// Token: 0x040003F6 RID: 1014
	public float turnSpeed = 3f;

	// Token: 0x040003F7 RID: 1015
	private ParticleSystem _sparks;

	// Token: 0x040003F8 RID: 1016
	private float _sparkAmount;

	// Token: 0x040003F9 RID: 1017
	private AudioSource _engineSound;

	// Token: 0x040003FA RID: 1018
	private AudioSource _sparksSound;

	// Token: 0x040003FB RID: 1019
	private AudioSource _tramHornSound;

	// Token: 0x040003FC RID: 1020
	private AudioSource _startStopSound;

	// Token: 0x040003FD RID: 1021
	private bool _startSnap;

	// Token: 0x040003FE RID: 1022
	private bool _inTurn;

	// Token: 0x040003FF RID: 1023
	private Transform _doorSnapPoint;

	// Token: 0x04000400 RID: 1024
	private Transform _doorTransform;

	// Token: 0x04000401 RID: 1025
	private float colliderRadius = 3f;

	// Token: 0x04000402 RID: 1026
	private float colliderDistance = 15f;
}
