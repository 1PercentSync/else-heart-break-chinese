using System;

// Token: 0x02000014 RID: 20
public interface IEasyAnimState
{
	// Token: 0x06000071 RID: 113
	bool Interpolate(float pDeltaTime);

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000072 RID: 114
	IEasyAnimState nextState { get; }
}
