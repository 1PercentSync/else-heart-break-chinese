using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class HackdevButtonListener : MonoBehaviour
{
	// Token: 0x0600034D RID: 845 RVA: 0x00018C48 File Offset: 0x00016E48
	private void OnSubNodeButtonPressed(int x)
	{
		Debug.Log("Hackdev OnSubNodeButtonPressed with value: " + x);
		this.onHackdevButtonClicked(x);
	}

	// Token: 0x0400026B RID: 619
	public Action<int> onHackdevButtonClicked;
}
