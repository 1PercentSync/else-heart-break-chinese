using System;

namespace GameWorld2
{
	// Token: 0x02000016 RID: 22
	public class MockProgram : IReturnValueReceiver
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x0000A9C4 File Offset: 0x00008BC4
		public MockProgram(MockProgram.OnReturnValueDelegate pOnReturnValue)
		{
			this.onReturnValue = pOnReturnValue;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A9D4 File Offset: 0x00008BD4
		public void OnReturnValue(object pReturnValue)
		{
			if (this.onReturnValue != null)
			{
				this.onReturnValue(pReturnValue);
			}
		}

		// Token: 0x04000096 RID: 150
		public MockProgram.OnReturnValueDelegate onReturnValue;

		// Token: 0x02000017 RID: 23
		// (Invoke) Token: 0x060001F7 RID: 503
		public delegate void OnReturnValueDelegate(object pReturnValue);
	}
}
