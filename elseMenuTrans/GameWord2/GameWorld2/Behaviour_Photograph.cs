using System;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000045 RID: 69
	public class Behaviour_Photograph : TimetableBehaviour
	{
		// Token: 0x06000499 RID: 1177 RVA: 0x00017450 File Offset: 0x00015650
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			return 0f;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00017458 File Offset: 0x00015658
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return true;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001745C File Offset: 0x0001565C
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00017460 File Offset: 0x00015660
		public void Reset()
		{
		}
	}
}
