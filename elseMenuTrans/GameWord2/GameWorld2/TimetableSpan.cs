using System;
using GameTypes;

namespace GameWorld2
{
	// Token: 0x02000046 RID: 70
	public struct TimetableSpan
	{
		// Token: 0x0600049D RID: 1181 RVA: 0x00017464 File Offset: 0x00015664
		public bool IsTimeWithinBounds(GameTime pTime)
		{
			return pTime.IsWithinMinuteBounds(this.startTime, this.endTime);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001747C File Offset: 0x0001567C
		public override string ToString()
		{
			return string.Format("[TimetableSpan] From {0} , to {1} with behaviour {2}", this.startTime, this.endTime, this.behaviour);
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x000174B0 File Offset: 0x000156B0
		public static TimetableSpan NULL
		{
			get
			{
				return new TimetableSpan
				{
					name = "NULL",
					startTime = default(GameTime),
					endTime = default(GameTime),
					behaviour = null
				};
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000174FC File Offset: 0x000156FC
		public override bool Equals(object obj)
		{
			return obj is TimetableSpan && (TimetableSpan)obj == this;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001751C File Offset: 0x0001571C
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00017530 File Offset: 0x00015730
		public static bool operator ==(TimetableSpan g1, TimetableSpan g2)
		{
			return g1.behaviour == g2.behaviour && g1.name == g2.name && g1.startTime == g2.startTime && g1.name == g2.name;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00017598 File Offset: 0x00015798
		public static bool operator !=(TimetableSpan g1, TimetableSpan g2)
		{
			return !(g1 == g2);
		}

		// Token: 0x0400012A RID: 298
		public string name;

		// Token: 0x0400012B RID: 299
		public GameTime startTime;

		// Token: 0x0400012C RID: 300
		public GameTime endTime;

		// Token: 0x0400012D RID: 301
		public TimetableBehaviour behaviour;
	}
}
