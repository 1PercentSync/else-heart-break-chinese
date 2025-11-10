using System;
using GameTypes;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000043 RID: 67
	public class Behaviour_Drink : TimetableBehaviour
	{
		// Token: 0x0600048E RID: 1166 RVA: 0x000171E8 File Offset: 0x000153E8
		public Behaviour_Drink(string pRoomName)
		{
			this._roomName = pRoomName;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000171F8 File Offset: 0x000153F8
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			if (pCharacter.busy)
			{
				return 1f;
			}
			bool flag = pCharacter.room.name == this._roomName;
			bool flag2 = pCharacter.handItem is Drink;
			bool flag3 = pCharacter.actionName == "Talking";
			bool flag4 = pCharacter.actionName == "";
			if (flag)
			{
				if (flag2)
				{
					if (flag4)
					{
						Drink drink = pCharacter.handItem as Drink;
						if (drink.amount > 0f)
						{
							pCharacter.InteractWith(pCharacter.handItem);
							return Randomizer.GetValue(2f, 7f);
						}
						pCharacter.DropHandItem();
					}
					else if (flag3)
					{
						pCharacter.StopAction();
					}
				}
				else if (!(pCharacter.finalTargetTing is Drink))
				{
					if (flag4)
					{
						Drink randomTing = InteractionHelper.GetRandomTing<Drink>(pTingRunner, this._roomName, pCharacter.tileGroup);
						if (randomTing != null)
						{
							pCharacter.WalkToTingAndInteract(randomTing);
						}
					}
				}
				return 1f;
			}
			InteractionHelper.GoToRoom(pCharacter, pRoomRunner, this._roomName, pTingRunner);
			return 1f;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00017324 File Offset: 0x00015524
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00017328 File Offset: 0x00015528
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return pCharacter.room.name == this._roomName;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00017340 File Offset: 0x00015540
		public void Reset()
		{
		}

		// Token: 0x04000128 RID: 296
		private string _roomName;
	}
}
