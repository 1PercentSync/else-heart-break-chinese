using System;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200003B RID: 59
	public class Behaviour_RefineGoods : TimetableBehaviour
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x0001642C File Offset: 0x0001462C
		public Behaviour_RefineGoods(string pRoomName)
		{
			this._roomName = pRoomName;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001643C File Offset: 0x0001463C
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			bool flag = pCharacter.room.name == this._roomName;
			if (pCharacter.busy || pCharacter.talking)
			{
				return 1f;
			}
			bool flag2 = pCharacter.handItem is Goods;
			if (!flag)
			{
				InteractionHelper.GoToRoom(pCharacter, pRoomRunner, this._roomName, pTingRunner);
			}
			else if (flag2)
			{
				Goods goods = pCharacter.handItem as Goods;
				if (goods.GetPureness() > 0.8f)
				{
					pCharacter.DropHandItem();
					return 5f;
				}
				return this.TryToRefineIt(pCharacter, pTingRunner);
			}
			else
			{
				Goods randomTing = InteractionHelper.GetRandomTing<Goods>(pTingRunner, pCharacter.room.name, pCharacter.tileGroup);
				if (randomTing != null)
				{
					pCharacter.WalkToTingAndInteract(randomTing);
					return 7f;
				}
			}
			return 1f;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001650C File Offset: 0x0001470C
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return pCharacter.room.name == this._roomName;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00016524 File Offset: 0x00014724
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00016528 File Offset: 0x00014728
		private float TryToRefineIt(Character pCharacter, MimanTingRunner pTingRunner)
		{
			Machine randomTing = InteractionHelper.GetRandomTing<Machine>(pTingRunner, pCharacter.room.name, pCharacter.tileGroup);
			if (randomTing != null && !randomTing.isBeingUsed)
			{
				pCharacter.WalkToTingAndInteract(randomTing);
			}
			return 15f;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001656C File Offset: 0x0001476C
		public override string ToString()
		{
			return string.Format("[RefiningGoods] {0}", this._roomName);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00016580 File Offset: 0x00014780
		public void Reset()
		{
		}

		// Token: 0x04000115 RID: 277
		private string _roomName;
	}
}
