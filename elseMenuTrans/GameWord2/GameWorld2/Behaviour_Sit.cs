using System;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000041 RID: 65
	public class Behaviour_Sit : TimetableBehaviour
	{
		// Token: 0x06000482 RID: 1154 RVA: 0x00016E90 File Offset: 0x00015090
		public Behaviour_Sit(string[] pSeatNames)
		{
			this._seatNames = pSeatNames;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00016EA0 File Offset: 0x000150A0
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			if (pCharacter.busy)
			{
				return 1f;
			}
			if (pCharacter.talking)
			{
				return 1f;
			}
			if (this._seat == null)
			{
				foreach (string text in this._seatNames)
				{
					Seat seat = pTingRunner.GetTing(text) as Seat;
					if (seat != null && !seat.isBeingUsed)
					{
						this._seat = seat;
						break;
					}
				}
			}
			if (this._seat == null)
			{
				return 1f;
			}
			bool flag = pCharacter.seat == this._seat;
			bool flag2 = this._seat.position == pCharacter.finalTargetPosition || this._seat.HasInteractionPointHere(pCharacter.finalTargetPosition);
			if (!flag && !flag2)
			{
				if (this._seat.isBeingUsed)
				{
					this._seat = null;
					return 1f;
				}
				pCharacter.WalkToTingAndInteract(this._seat);
			}
			if (flag)
			{
			}
			return 1f;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00016FC0 File Offset: 0x000151C0
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00016FC4 File Offset: 0x000151C4
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return this._seat != null && pCharacter.seat == this._seat;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00016FE4 File Offset: 0x000151E4
		public void Reset()
		{
		}

		// Token: 0x04000126 RID: 294
		private string[] _seatNames;

		// Token: 0x04000127 RID: 295
		private Seat _seat;
	}
}
