using System;
using GrimmLib;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class DiscoBallScript : MonoBehaviour
{
	// Token: 0x0600001E RID: 30 RVA: 0x00002C14 File Offset: 0x00000E14
	private void Start()
	{
		this._dialogueRunner = WorldOwner.instance.world.dialogueRunner;
		this._dialogueRunner.AddOnEventListener(new DialogueRunner.OnEvent(this.OnEvent));
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002C50 File Offset: 0x00000E50
	private void OnDestroy()
	{
		if (WorldOwner.instance.worldIsLoaded)
		{
			WorldOwner.instance.world.dialogueRunner.RemoveOnEventListener(new DialogueRunner.OnEvent(this.OnEvent));
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002C8C File Offset: 0x00000E8C
	private void OnEvent(string pEventName)
	{
		if (pEventName == "FALL")
		{
			this.broken = true;
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				transform.rigidbody.isKinematic = false;
				transform.rigidbody.AddExplosionForce(1f, global::UnityEngine.Random.insideUnitSphere, 1f);
			}
		}
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002D34 File Offset: 0x00000F34
	private void Update()
	{
		if (!this.broken)
		{
			base.transform.Rotate(Vector3.up, 1f * Time.deltaTime);
		}
	}

	// Token: 0x0400001B RID: 27
	private bool broken;

	// Token: 0x0400001C RID: 28
	private DialogueRunner _dialogueRunner;
}
