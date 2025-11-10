using System;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class MessageCollector : MonoBehaviour
{
	// Token: 0x06000395 RID: 917 RVA: 0x0001A19C File Offset: 0x0001839C
	protected void OnAnimation(AnimationEvent pEvent)
	{
		if (this.onAnimationEvent != null)
		{
			this.onAnimationEvent(pEvent);
		}
	}

	// Token: 0x06000396 RID: 918 RVA: 0x0001A1B8 File Offset: 0x000183B8
	public void OnDestroy()
	{
		this.onAnimationEvent = null;
	}

	// Token: 0x040002A7 RID: 679
	public AnimationHandler onAnimationEvent;
}
