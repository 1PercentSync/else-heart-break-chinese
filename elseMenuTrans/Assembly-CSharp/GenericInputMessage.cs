using System;

// Token: 0x0200003B RID: 59
public class GenericInputMessage<T> : IInputMessage
{
	// Token: 0x06000256 RID: 598 RVA: 0x00011458 File Offset: 0x0000F658
	public GenericInputMessage(string pName, Function pOnAccept, Function pOnDecline, T pData)
	{
		this.name = pName;
		this.onAccepted = pOnAccept;
		this.onDeclined = pOnDecline;
		this.data = pData;
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000257 RID: 599 RVA: 0x00011480 File Offset: 0x0000F680
	// (set) Token: 0x06000258 RID: 600 RVA: 0x00011488 File Offset: 0x0000F688
	public string name { get; private set; }

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000259 RID: 601 RVA: 0x00011494 File Offset: 0x0000F694
	// (set) Token: 0x0600025A RID: 602 RVA: 0x0001149C File Offset: 0x0000F69C
	public T data { get; private set; }

	// Token: 0x0600025B RID: 603 RVA: 0x000114A8 File Offset: 0x0000F6A8
	public void Accept()
	{
		if (this.onAccepted != null)
		{
			this.onAccepted();
		}
	}

	// Token: 0x0600025C RID: 604 RVA: 0x000114C0 File Offset: 0x0000F6C0
	public void Decline()
	{
		if (this.onDeclined != null)
		{
			this.onDeclined();
		}
	}

	// Token: 0x0400016C RID: 364
	private Function onAccepted;

	// Token: 0x0400016D RID: 365
	private Function onDeclined;
}
