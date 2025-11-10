using System;
using GameTypes;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000035 RID: 53
	public class Behaviour_BeInRoom : TimetableBehaviour
	{
		// Token: 0x06000432 RID: 1074 RVA: 0x00014EDC File Offset: 0x000130DC
		public Behaviour_BeInRoom(string pRoomName)
		{
			this._roomName = pRoomName;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00014EEC File Offset: 0x000130EC
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
			else if (pCharacter.actionName == "Angry" || pCharacter.seat == null || Randomizer.OneIn(7))
			{
				Seat randomTingWhere = InteractionHelper.GetRandomTingWhere<Seat>(pRoomRunner, pCharacter.room.name, (Seat s) => s.tile.group == pCharacter.tileGroup && !s.isBeingUsed);
				if (randomTingWhere != null)
				{
					pCharacter.WalkToTingAndInteract(randomTingWhere);
				}
				else
				{
					Point anyRandomTing = InteractionHelper.GetAnyRandomTing<Point>(pTingRunner, this._roomName, pCharacter.tileGroup);
					if (anyRandomTing != null)
					{
						pCharacter.WalkToTingAndInteract(anyRandomTing);
					}
				}
			}
			return Randomizer.GetValue(10f, 30f);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001505C File Offset: 0x0001325C
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return pCharacter.room.name == this._roomName;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00015074 File Offset: 0x00013274
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00015078 File Offset: 0x00013278
		public override string ToString()
		{
			return string.Format("[BeInRoom] {0}", this._roomName);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0001508C File Offset: 0x0001328C
		public void Reset()
		{
		}

		// Token: 0x04000104 RID: 260
		private string _roomName;
	}
}
