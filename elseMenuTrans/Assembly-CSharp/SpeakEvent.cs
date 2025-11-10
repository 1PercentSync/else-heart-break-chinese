using System;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public struct SpeakEvent : BaseEvent
{
	// Token: 0x060004A8 RID: 1192 RVA: 0x0002024C File Offset: 0x0001E44C
	public SpeakEvent(GameObject pSayer, string pMessage)
	{
		this.sayer = pSayer;
		this.message = pMessage;
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0002025C File Offset: 0x0001E45C
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"SpeakEvent[ -",
			this.sayer.name,
			": ",
			this.message,
			"]"
		});
	}

	// Token: 0x0400038A RID: 906
	public GameObject sayer;

	// Token: 0x0400038B RID: 907
	public string message;
}
