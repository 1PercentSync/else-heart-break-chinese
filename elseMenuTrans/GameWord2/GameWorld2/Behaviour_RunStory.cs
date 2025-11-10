using System;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000032 RID: 50
	public class Behaviour_RunStory : TimetableBehaviour
	{
		// Token: 0x0600041D RID: 1053 RVA: 0x00014C60 File Offset: 0x00012E60
		public Behaviour_RunStory(string pStoryName)
		{
			this._storyName = pStoryName;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00014C70 File Offset: 0x00012E70
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			if (!pDialogueRunner.ConversationIsRunning(this._storyName))
			{
				pDialogueRunner.StartConversation(this._storyName);
			}
			return 10f;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00014CA4 File Offset: 0x00012EA4
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return true;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00014CA8 File Offset: 0x00012EA8
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00014CAC File Offset: 0x00012EAC
		public override string ToString()
		{
			return string.Format("[RunStory] {0}", this._storyName);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00014CC0 File Offset: 0x00012EC0
		public void Reset()
		{
		}

		// Token: 0x040000FF RID: 255
		private string _storyName;
	}
}
