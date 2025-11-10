using System;
using System.Runtime.InteropServices;

namespace GameWorld2
{
	// Token: 0x0200006A RID: 106
	public class ApproximateSqrt
	{
		// Token: 0x06000642 RID: 1602 RVA: 0x0001D648 File Offset: 0x0001B848
		public static float Sqrt(float z)
		{
			if (z == 0f)
			{
				return 0f;
			}
			ApproximateSqrt.FloatIntUnion floatIntUnion;
			floatIntUnion.tmp = 0;
			floatIntUnion.f = z;
			floatIntUnion.tmp -= 8388608;
			floatIntUnion.tmp >>= 1;
			floatIntUnion.tmp += 536870912;
			return floatIntUnion.f;
		}

		// Token: 0x0200006B RID: 107
		[StructLayout(LayoutKind.Explicit)]
		private struct FloatIntUnion
		{
			// Token: 0x040001A3 RID: 419
			[FieldOffset(0)]
			public float f;

			// Token: 0x040001A4 RID: 420
			[FieldOffset(0)]
			public int tmp;
		}
	}
}
