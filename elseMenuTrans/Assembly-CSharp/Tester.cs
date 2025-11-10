using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class Tester : MonoBehaviour
{
	// Token: 0x06000053 RID: 83 RVA: 0x0000438C File Offset: 0x0000258C
	private void Start()
	{
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00004390 File Offset: 0x00002590
	private void Update()
	{
		TerminalRenderer component = base.GetComponent<TerminalRenderer>();
		component.SetCharacter(0, 0, '*');
		component.SetCharacter(62, 30, '#');
		component.SetCharacter(10, 10, 'E');
		component.SetCharacter(11, 10, 'r');
		component.SetCharacter(12, 10, 'i');
		component.SetCharacter(13, 10, 'k');
		component.ApplyTextChanges();
	}
}
