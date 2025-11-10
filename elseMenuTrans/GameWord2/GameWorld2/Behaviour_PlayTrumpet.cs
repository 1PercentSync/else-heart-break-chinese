using System;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000034 RID: 52
	public class Behaviour_PlayTrumpet : TimetableBehaviour
	{
		// Token: 0x0600042A RID: 1066 RVA: 0x00014DC0 File Offset: 0x00012FC0
		public Behaviour_PlayTrumpet(string pTingName)
		{
			this._tingName = pTingName;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00014DD0 File Offset: 0x00012FD0
		private bool ThereYet(Character pCharacter)
		{
			return this._targetTing != null && pCharacter.position == this._targetTing.position;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00014DF8 File Offset: 0x00012FF8
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
			if (this._targetTing == null)
			{
				this._targetTing = pTingRunner.GetTing(this._tingName);
			}
			if (this.ThereYet(pCharacter))
			{
				pCharacter.direction = this._targetTing.direction;
				this.StartTrumpeting(pCharacter);
				return 30f;
			}
			if (pCharacter.finalTargetPosition != this._targetTing.position)
			{
				pCharacter.WalkToTingAndInteract(this._targetTing);
			}
			return 1f;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00014E9C File Offset: 0x0001309C
		private void StartTrumpeting(Character pCharacter)
		{
			pCharacter.StartAction("Trumpeting", null, 99999f, 99999f);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00014EB4 File Offset: 0x000130B4
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return this.ThereYet(pCharacter);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00014EC0 File Offset: 0x000130C0
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00014EC4 File Offset: 0x000130C4
		public override string ToString()
		{
			return string.Format("[BeAtTing] {0}", this._tingName);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00014ED8 File Offset: 0x000130D8
		public void Reset()
		{
		}

		// Token: 0x04000102 RID: 258
		private string _tingName;

		// Token: 0x04000103 RID: 259
		private Ting _targetTing;
	}
}
