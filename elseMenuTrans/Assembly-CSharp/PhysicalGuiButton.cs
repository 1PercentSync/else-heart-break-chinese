using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class PhysicalGuiButton : MonoBehaviour
{
	// Token: 0x060003B0 RID: 944 RVA: 0x0001AA98 File Offset: 0x00018C98
	private void OnMouseDown()
	{
		if (this.onPressed != null)
		{
			this.onPressed(this);
		}
	}

	// Token: 0x040002CB RID: 715
	public PhysicalGuiButton.OnPressed onPressed;

	// Token: 0x02000100 RID: 256
	// (Invoke) Token: 0x0600075F RID: 1887
	public delegate void OnPressed(MonoBehaviour pMonoBehaviour);
}
