using System;
using GameTypes;

namespace GameWorld2
{
	// Token: 0x0200007B RID: 123
	public class Boat : Vehicle
	{
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0001FD70 File Offset: 0x0001DF70
		public override IntPoint movingDoorPositionOffset
		{
			get
			{
				return base.localPoint + IntPoint.DirectionToIntPoint(IntPoint.Turn(base.direction, 90)) * 10;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0001FDA4 File Offset: 0x0001DFA4
		public override int movingDoorRotationOffset
		{
			get
			{
				return 90;
			}
		}
	}
}
