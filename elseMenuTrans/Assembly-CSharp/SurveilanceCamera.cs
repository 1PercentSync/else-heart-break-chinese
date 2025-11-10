using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class SurveilanceCamera : MonoBehaviour
{
	// Token: 0x0600044C RID: 1100 RVA: 0x0001E64C File Offset: 0x0001C84C
	private void Start()
	{
		base.StartCoroutine("FindTarget");
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0001E65C File Offset: 0x0001C85C
	private IEnumerator FindTarget()
	{
		for (;;)
		{
			this.target = null;
			float closestDistance = float.MaxValue;
			List<Shell> shells = ShellManager.GetShellsWithTingConnectionInScene();
			foreach (Shell shell in shells)
			{
				float d = Vector3.Distance(shell.transform.position, base.transform.position);
				if (shell is CharacterShell && d < closestDistance)
				{
					this.target = shell.transform;
					closestDistance = d;
				}
			}
			yield return new WaitForSeconds(2f);
		}
		yield break;
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0001E678 File Offset: 0x0001C878
	private void Update()
	{
		if (this.target != null)
		{
			Vector3 vector = this.target.transform.position + new Vector3(0f, 2.5f, 0f);
			Quaternion quaternion = Quaternion.LookRotation(vector - base.transform.position);
			Quaternion quaternion2 = Quaternion.Slerp(base.transform.rotation, quaternion, Time.deltaTime * this.damping);
			base.transform.rotation = quaternion2;
		}
	}

	// Token: 0x04000351 RID: 849
	public Transform target;

	// Token: 0x04000352 RID: 850
	public float damping = 0.9f;
}
