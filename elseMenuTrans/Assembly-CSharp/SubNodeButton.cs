using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class SubNodeButton : MonoBehaviour
{
	// Token: 0x06000449 RID: 1097 RVA: 0x0001E590 File Offset: 0x0001C790
	private void OnMouseDown()
	{
		if (RunGameWorld.instance.currentGameViewState is ComputerInteractionState || RunGameWorld.instance.currentGameViewState is CodeEditorState)
		{
			Debug.Log(base.name + "'s collider was clicked");
			base.transform.parent.gameObject.SendMessage("OnSubNodeButtonPressed", this.x);
		}
		else
		{
			Debug.Log(base.name + "'s collider was clicked but is not inside ComputerInteractionState or CodeEditorState so will not send message");
		}
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0001E61C File Offset: 0x0001C81C
	private void OnMouseEnter()
	{
		Debug.Log("Entered " + this.x);
	}

	// Token: 0x04000350 RID: 848
	public int x;
}
