using System;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000054 RID: 84
	public class ElevatorAPI
	{
		// Token: 0x06000551 RID: 1361 RVA: 0x00019EB4 File Offset: 0x000180B4
		public ElevatorAPI(Computer pComputer, TingRunner pTingRunner)
		{
			this._computer = pComputer;
		}

		// Token: 0x06000552 RID: 1362
		private Door GetElevatorDoor()
		{
			foreach (MimanTing ting in this._computer.connectedTings)
			{
				if (ting is Door)
				{
					return ting as Door;
				}
			}
			throw new Error("Can't access elevator door无法访问电梯门");
		}

		// Token: 0x06000553 RID: 1363
		[SprakAPI(new string[] { "Move the elevator to another floor移动电梯到另一楼层", "the floor nr楼层号" })]
		public void API_GotoFloor(float floorNr)
		{
			this.GetElevatorDoor().elevatorFloor = (int)floorNr;
		}

		// Token: 0x06000554 RID: 1364
		[SprakAPI(new string[] { "Get current floor nr获取当前楼层号" })]
		public float API_GetFloor()
		{
			return (float)this.GetElevatorDoor().elevatorFloor;
		}

		// Token: 0x04000161 RID: 353
		private Computer _computer;
	}
}
