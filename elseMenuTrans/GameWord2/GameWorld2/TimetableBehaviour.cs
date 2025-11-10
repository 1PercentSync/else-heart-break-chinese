using System;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000030 RID: 48
	public interface TimetableBehaviour
	{
		// Token: 0x06000412 RID: 1042
		float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings);

		// Token: 0x06000413 RID: 1043
		void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner);

		// Token: 0x06000414 RID: 1044
		bool IsAtFinalPartOfTask(Character pCharacter);

		// Token: 0x06000415 RID: 1045
		void Reset();
	}
}
