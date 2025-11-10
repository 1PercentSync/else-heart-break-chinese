using System;

// Token: 0x0200003E RID: 62
public interface IDisposable
{
	// Token: 0x17000040 RID: 64
	// (get) Token: 0x0600027F RID: 639
	bool disposed { get; }

	// Token: 0x06000280 RID: 640
	void Dispose();
}
