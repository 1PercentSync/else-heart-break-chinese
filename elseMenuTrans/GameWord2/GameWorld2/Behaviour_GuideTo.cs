using System;
using GameTypes;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000040 RID: 64
	public class Behaviour_GuideTo : TimetableBehaviour
	{
		// Token: 0x0600047C RID: 1148 RVA: 0x00016B60 File Offset: 0x00014D60
		public Behaviour_GuideTo(string pTingName, string pWaitFor)
		{
			this._tingName = pTingName;
			this._waitForCharacterName = pWaitFor;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00016B78 File Offset: 0x00014D78
		private bool AtGoal(Character pCharacter)
		{
			return this._ting.HasInteractionPointHere(pCharacter.position) || this._ting.position == pCharacter.position;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00016BB4 File Offset: 0x00014DB4
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
			if (this._ting == null)
			{
				this._ting = pTingRunner.GetTing(this._tingName);
			}
			if (this._waitForCharacter == null)
			{
				this._waitForCharacter = pTingRunner.GetTing<Character>(this._waitForCharacterName);
			}
			if (pCharacter.room.name != this._currentRoom)
			{
				this._prevRoom = this._currentRoom;
				this._currentRoom = pCharacter.room.name;
				pCharacter.logger.Log(pCharacter + " set prev room to " + this._prevRoom);
			}
			if (this._waitForCharacter.room != pCharacter.room)
			{
				if (this._waitForCharacter.room.name == this._prevRoom)
				{
					pCharacter.logger.Log(this._waitForCharacter.name + " is in previous room, will wait a bit");
					pCharacter.CancelWalking();
					return 3f;
				}
				pCharacter.logger.Log(this._waitForCharacter.name + " has gone off the tracks, will interact with him/her");
				pCharacter.WalkToTingAndInteract(this._waitForCharacter);
				return 3f;
			}
			else
			{
				bool flag = !MimanGrimmApiDefinitions.AreTingsWithinDistance(pCharacter, this._waitForCharacter, 18);
				bool flag2 = this._ting.HasInteractionPointHere(pCharacter.position) || this._ting.position == pCharacter.position;
				bool flag3 = this._ting.position == pCharacter.finalTargetPosition || this._ting.HasInteractionPointHere(pCharacter.finalTargetPosition);
				bool flag4 = pCharacter.actionName == "";
				bool flag5 = pCharacter.actionName == "Walking";
				if (flag && flag5)
				{
					pCharacter.CancelWalking();
					return 1f;
				}
				if (flag2 && flag4)
				{
					return 1f;
				}
				if (flag3 && flag5)
				{
					return 5f;
				}
				if (!flag2 && !flag)
				{
					pCharacter.WalkToTingAndInteract(this._ting);
					return 1f;
				}
				if (!flag2 && flag)
				{
					pCharacter.CancelWalking();
					return 1f;
				}
				D.Log(string.Concat(new object[]
				{
					"GuideTo behaviour has no matching clause for ",
					pCharacter,
					", at goal: ",
					flag2.ToString(),
					", isIdle: ",
					flag4.ToString(),
					", goalIsCorrectlySet: ",
					flag3.ToString(),
					", tooFarAway: ",
					flag.ToString()
				}));
				return 1f;
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00016E7C File Offset: 0x0001507C
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00016E80 File Offset: 0x00015080
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return this.AtGoal(pCharacter);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00016E8C File Offset: 0x0001508C
		public void Reset()
		{
		}

		// Token: 0x04000120 RID: 288
		private string _tingName;

		// Token: 0x04000121 RID: 289
		private string _waitForCharacterName;

		// Token: 0x04000122 RID: 290
		private Ting _ting;

		// Token: 0x04000123 RID: 291
		private Character _waitForCharacter;

		// Token: 0x04000124 RID: 292
		private string _currentRoom;

		// Token: 0x04000125 RID: 293
		private string _prevRoom;
	}
}
