using System;

// Token: 0x0200003A RID: 58
public interface IInputMessage
{
	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000253 RID: 595
	string name { get; }

	// Token: 0x06000254 RID: 596
	void Accept();

	// Token: 0x06000255 RID: 597
	void Decline();
}
