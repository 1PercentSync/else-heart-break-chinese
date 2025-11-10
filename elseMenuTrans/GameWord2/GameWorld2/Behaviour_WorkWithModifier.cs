using System;
using GameTypes;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000036 RID: 54
	public class Behaviour_WorkWithModifier : TimetableBehaviour
	{
		// Token: 0x06000438 RID: 1080 RVA: 0x00015090 File Offset: 0x00013290
		public Behaviour_WorkWithModifier(string pRoomName)
		{
			this._roomName = pRoomName;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000150A0 File Offset: 0x000132A0
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			bool flag = pCharacter.room.name == this._roomName;
			if (pCharacter.busy)
			{
				return 1f;
			}
			if (pCharacter.talking)
			{
				return 1f;
			}
			if (!flag || (pCharacter.finalTargetPosition != WorldCoordinate.NONE && pCharacter.finalTargetPosition.roomName != this._roomName))
			{
				InteractionHelper.GoToRoom(pCharacter, pRoomRunner, this._roomName, pTingRunner);
			}
			else
			{
				if (pCharacter.hackdev != null)
				{
					float num = (float)Randomizer.GetIntValue(0, 30);
					if (num < 10f)
					{
						Lamp randomTing = InteractionHelper.GetRandomTing<Lamp>(pTingRunner, pCharacter.room.name, pCharacter.tileGroup);
						if (randomTing != null && !randomTing.HasNoFreeInteractionPoints())
						{
							pCharacter.WalkToTingAndHack(randomTing);
							pCharacter.MoveHandItemToInventory();
							pCharacter.handItem = pCharacter.hackdev;
							return Randomizer.GetValue(10f, 20f);
						}
					}
					else if (num < 20f)
					{
						Computer randomTing2 = InteractionHelper.GetRandomTing<Computer>(pTingRunner, pCharacter.room.name, pCharacter.tileGroup);
						if (randomTing2 != null && !randomTing2.HasNoFreeInteractionPoints())
						{
							pCharacter.WalkToTingAndHack(randomTing2);
							pCharacter.MoveHandItemToInventory();
							pCharacter.handItem = pCharacter.hackdev;
							return Randomizer.GetValue(10f, 60f);
						}
					}
				}
				if (pCharacter.actionName == "Angry" || pCharacter.seat == null || Randomizer.OneIn(7))
				{
					Seat randomTing3 = InteractionHelper.GetRandomTing<Seat>(pTingRunner, pCharacter.room.name, pCharacter.tileGroup);
					if (randomTing3 != null && !randomTing3.isBeingUsed)
					{
						pCharacter.WalkToTingAndInteract(randomTing3);
					}
					else
					{
						Point anyRandomTing = InteractionHelper.GetAnyRandomTing<Point>(pTingRunner, this._roomName, pCharacter.tileGroup);
						pCharacter.WalkToTingAndInteract(anyRandomTing);
					}
				}
			}
			return Randomizer.GetValue(10f, 30f);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x000152A0 File Offset: 0x000134A0
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return pCharacter.room.name == this._roomName;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000152B8 File Offset: 0x000134B8
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
			if (pCharacter.handItem is Hackdev)
			{
				pCharacter.MoveHandItemToInventory();
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000152D4 File Offset: 0x000134D4
		public override string ToString()
		{
			return string.Format("[WorkWithModifier] {0}", this._roomName);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000152E8 File Offset: 0x000134E8
		public void Reset()
		{
		}

		// Token: 0x04000105 RID: 261
		private string _roomName;
	}
}
