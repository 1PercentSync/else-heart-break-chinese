using System;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000044 RID: 68
	public class Behaviour_ServeDrinks : TimetableBehaviour
	{
		// Token: 0x06000493 RID: 1171 RVA: 0x00017344 File Offset: 0x00015544
		public Behaviour_ServeDrinks(string pRoomName)
		{
			this._roomName = pRoomName;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00017354 File Offset: 0x00015554
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			bool flag = pCharacter.room.name == this._roomName;
			bool flag2 = pCharacter.handItem is Drink;
			bool flag3 = pCharacter.actionName == "";
			if (flag)
			{
				if (flag2)
				{
					if (flag3)
					{
						pCharacter.DropHandItem();
					}
				}
				else
				{
					bool flag4 = true;
					foreach (Drink drink in pTingRunner.GetTingsOfTypeInRoom<Drink>(pCharacter.room.name))
					{
						if (drink.amount > 0f)
						{
							flag4 = false;
							break;
						}
					}
					if (flag4)
					{
					}
				}
				return 1f;
			}
			InteractionHelper.GoToRoom(pCharacter, pRoomRunner, this._roomName, pTingRunner);
			return 1f;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00017428 File Offset: 0x00015628
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001742C File Offset: 0x0001562C
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return pCharacter.room.name == this._roomName;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00017444 File Offset: 0x00015644
		public void Reset()
		{
		}

		// Token: 0x04000129 RID: 297
		private string _roomName;
	}
}
