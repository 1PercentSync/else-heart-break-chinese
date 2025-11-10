using System;
using UnityEngine;

// Token: 0x02000091 RID: 145
public class SetCameraDIrectionOnStart : MonoBehaviour
{
	// Token: 0x0600042B RID: 1067 RVA: 0x0001DC80 File Offset: 0x0001BE80
	private void Update()
	{
		Shell shellWithName = ShellManager.GetShellWithName(WorldOwner.instance.world.settings.avatarName);
		if (shellWithName != null)
		{
			if (Vector3.Distance(shellWithName.transform.position, base.transform.position) < 10f)
			{
				GreatCamera component = RunGameWorld.instance.camera.GetComponent<GreatCamera>();
				component.Input_SetRotation(this.targetAngle);
			}
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000331 RID: 817
	public float targetAngle;
}
