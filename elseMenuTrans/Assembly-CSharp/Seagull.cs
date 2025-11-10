using System;
using GrimmLib;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class Seagull : MonoBehaviour
{
	// Token: 0x06000404 RID: 1028 RVA: 0x0001CF1C File Offset: 0x0001B11C
	public static SeagullZone GetRandomZone(SeagullZone.ZoneType[] pIgnoreTypes)
	{
		SeagullZone[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(SeagullZone)) as SeagullZone[];
		if (array.Length == 0)
		{
			return null;
		}
		int num = (int)(global::UnityEngine.Random.value * (float)array.Length);
		int num2 = num;
		for (;;)
		{
			SeagullZone seagullZone = array[num2];
			if (!seagullZone.taken && !Seagull.IsOfType(seagullZone, pIgnoreTypes))
			{
				break;
			}
			num2++;
			if (num2 > array.Length - 1)
			{
				num2 = 0;
			}
			if (num2 == num)
			{
				goto Block_5;
			}
		}
		return array[num2];
		Block_5:
		return null;
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0001CF98 File Offset: 0x0001B198
	public void Poo()
	{
		global::UnityEngine.Object.Instantiate(this.pooPrefab, base.transform.position, Quaternion.Euler(Vector3.down));
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x0001CFC8 File Offset: 0x0001B1C8
	private static bool IsOfType(SeagullZone pZone, SeagullZone.ZoneType[] pTypes)
	{
		foreach (SeagullZone.ZoneType zoneType in pTypes)
		{
			if (pZone.zoneType == zoneType)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x0001D000 File Offset: 0x0001B200
	public void SetANewRandomCurrentZone(SeagullZone.ZoneType[] pIgnoreTypes)
	{
		SeagullZone randomZone = Seagull.GetRandomZone(pIgnoreTypes);
		if (randomZone == null)
		{
			Debug.Log("Can't find targetZone for Seagull " + base.name);
			return;
		}
		this.currentZone.taken = false;
		this.currentZone = randomZone;
		this.currentZone.taken = true;
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0001D058 File Offset: 0x0001B258
	private void Start()
	{
		this.speed = 5f;
		this.animationComponent = base.GetComponentInChildren<Animation>();
		this.agent = base.GetComponent<NavMeshAgent>();
		this.agent.enabled = false;
		this.currentZone = Seagull.GetRandomZone(new SeagullZone.ZoneType[0]);
		if (this.currentZone != null)
		{
			base.transform.position = this.currentZone.transform.position;
			base.transform.Rotate(Vector3.up, global::UnityEngine.Random.Range(0f, 6.2831855f));
			this.currentZone.taken = true;
			this.ChangeState(SeagullState.GetStateFromZoneType(this.currentZone, this));
		}
		else
		{
			Debug.Log(base.name + " can't find any zones");
		}
		if (WorldOwner.instance.worldIsLoaded)
		{
			WorldOwner.instance.world.dialogueRunner.AddOnEventListener(new DialogueRunner.OnEvent(this.OnEvent));
		}
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x0001D158 File Offset: 0x0001B358
	private void OnDestroy()
	{
		if (WorldOwner.instance.worldIsLoaded)
		{
			WorldOwner.instance.world.dialogueRunner.RemoveOnEventListener(new DialogueRunner.OnEvent(this.OnEvent));
		}
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0001D194 File Offset: 0x0001B394
	private void OnEvent(string pEvent)
	{
		if (pEvent == "LampWasKicked")
		{
			this.GetScared();
		}
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x0001D1AC File Offset: 0x0001B3AC
	private void Update()
	{
		this.speed = Mathf.Clamp(this.speed, 0.01f, 15f);
		if (this._state != null)
		{
			if (this._state.GetType() != typeof(Seagull_Fly))
			{
				this.CheckForFlyAway();
			}
			SeagullState seagullState = this._state.Update();
			if (seagullState != null)
			{
				this.ChangeState(seagullState);
			}
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			this.Poo();
		}
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0001D22C File Offset: 0x0001B42C
	private void ChangeState(SeagullState pNextState)
	{
		this._state = pNextState;
		this._state.OnEnter();
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x0001D240 File Offset: 0x0001B440
	private void OnTriggerEnter(Collider pCollider)
	{
		if (pCollider.tag != "FLOOR" && (this._state.GetType() == typeof(Seagull_Fly) || this._state.GetType() == typeof(Seagull_Glide)))
		{
			this.ChangeState(new Seagull_Turn(this));
		}
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x0001D2A4 File Offset: 0x0001B4A4
	private void CheckForFlyAway()
	{
		Type typeFromHandle = typeof(CharacterShell);
		MonoBehaviour[] array = global::UnityEngine.Object.FindObjectsOfType(typeFromHandle) as MonoBehaviour[];
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (Vector3.Distance(base.transform.position, monoBehaviour.transform.position) < 5f)
			{
				this.GetScared();
				break;
			}
		}
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x0001D318 File Offset: 0x0001B518
	public void GetScared()
	{
		this.agent.enabled = false;
		this.ChangeState(new Seagull_Fly(this));
		SoundRandomizer component = base.GetComponent<SoundRandomizer>();
		if (component != null)
		{
			component.PlaySound();
		}
	}

	// Token: 0x0400031A RID: 794
	private SeagullState _state;

	// Token: 0x0400031B RID: 795
	public NavMeshAgent agent;

	// Token: 0x0400031C RID: 796
	public Animation animationComponent;

	// Token: 0x0400031D RID: 797
	public SeagullZone currentZone;

	// Token: 0x0400031E RID: 798
	public float speed;

	// Token: 0x0400031F RID: 799
	public GameObject pooPrefab;
}
