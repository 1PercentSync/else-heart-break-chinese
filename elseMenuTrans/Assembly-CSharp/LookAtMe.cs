using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class LookAtMe : MonoBehaviour
{
	// Token: 0x0600038E RID: 910 RVA: 0x00019F7C File Offset: 0x0001817C
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00019F8C File Offset: 0x0001818C
	private void Update()
	{
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00019F90 File Offset: 0x00018190
	private void OnAnimatorIK()
	{
		Debug.Log("look at stuff");
		this.anim.SetLookAtWeight(1f);
		this.anim.SetLookAtPosition(Camera.main.transform.position);
	}

	// Token: 0x040002A1 RID: 673
	private Animator anim;
}
