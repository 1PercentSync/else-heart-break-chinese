using System;
using System.Text;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200006D RID: 109
	public class MimanPath
	{
		// Token: 0x06000644 RID: 1604 RVA: 0x0001D6D0 File Offset: 0x0001B8D0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new object[]
			{
				"MimanPath [",
				this.status.ToString(),
				"] (",
				this.iterations,
				" iterations) with tings: "
			}));
			int num = 0;
			foreach (Ting ting in this.tings)
			{
				stringBuilder.Append(ting.name);
				num++;
				if (num < this.tings.Length)
				{
					stringBuilder.Append(", ");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040001AA RID: 426
		public MimanPathStatus status = MimanPathStatus.NOT_SET;

		// Token: 0x040001AB RID: 427
		public Ting[] tings = new Ting[0];

		// Token: 0x040001AC RID: 428
		public int iterations;
	}
}
