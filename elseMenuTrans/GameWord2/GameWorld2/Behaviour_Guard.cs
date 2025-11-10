using System;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200003C RID: 60
	public class Behaviour_Guard : TimetableBehaviour
	{
		// Token: 0x06000464 RID: 1124 RVA: 0x00016584 File Offset: 0x00014784
		public Behaviour_Guard(string pRoomName)
		{
			this._roomName = pRoomName;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00016594 File Offset: 0x00014794
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			bool flag = pCharacter.room.name == this._roomName;
			if (pCharacter.busy || pCharacter.talking || pCharacter.actionName == "Walking")
			{
				return 1f;
			}
			if (!flag)
			{
				InteractionHelper.GoToRoom(pCharacter, pRoomRunner, this._roomName, pTingRunner);
			}
			else
			{
				Point randomTing = InteractionHelper.GetRandomTing<Point>(pTingRunner, pCharacter.room.name, pCharacter.tileGroup);
				if (randomTing != null && !randomTing.isBeingUsed)
				{
					pCharacter.WalkToTingAndInteract(randomTing);
					return 1f;
				}
			}
			return 1f;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00016640 File Offset: 0x00014840
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return pCharacter.room.name == this._roomName;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00016658 File Offset: 0x00014858
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001665C File Offset: 0x0001485C
		public override string ToString()
		{
			return string.Format("[BeInRoom] {0}", this._roomName);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00016670 File Offset: 0x00014870
		public void Reset()
		{
		}

		// Token: 0x04000116 RID: 278
		private string _roomName;
	}
}
