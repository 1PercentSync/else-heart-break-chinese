using System;
using GrimmLib;
using UnityEngine;

// Token: 0x0200009C RID: 156
public class TriggeredEventSound : MonoBehaviour
{
	// Token: 0x06000460 RID: 1120 RVA: 0x0001EB58 File Offset: 0x0001CD58
	private void Start()
	{
		WorldOwner.instance.world.dialogueRunner.AddOnEventListener(new DialogueRunner.OnEvent(this.EventHappened));
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0001EB88 File Offset: 0x0001CD88
	private void OnDestroy()
	{
		if (WorldOwner.instance.worldIsLoaded)
		{
			WorldOwner.instance.world.dialogueRunner.RemoveOnEventListener(new DialogueRunner.OnEvent(this.EventHappened));
		}
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0001EBC4 File Offset: 0x0001CDC4
	private void EventHappened(string pEventName)
	{
		if (this.eventName == pEventName)
		{
			if (base.audio != null)
			{
				base.audio.Play();
			}
			else
			{
				Debug.Log("No audio source on " + base.name);
			}
		}
	}

	// Token: 0x0400035D RID: 861
	public string eventName;
}
