using System;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000031 RID: 49
	public class Behaviour_BeAtPosition : TimetableBehaviour
	{
		// Token: 0x06000416 RID: 1046 RVA: 0x00014BD8 File Offset: 0x00012DD8
		public Behaviour_BeAtPosition(WorldCoordinate pTargetPosition)
		{
			this._targetPosition = pTargetPosition;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00014BE8 File Offset: 0x00012DE8
		private bool ThereYet(Character pCharacter)
		{
			return pCharacter.position == this._targetPosition;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00014BFC File Offset: 0x00012DFC
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			if (!this.ThereYet(pCharacter) && pCharacter.finalTargetPosition != this._targetPosition)
			{
				pCharacter.WalkTo(this._targetPosition);
			}
			return 0f;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00014C34 File Offset: 0x00012E34
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return this.ThereYet(pCharacter);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00014C40 File Offset: 0x00012E40
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00014C44 File Offset: 0x00012E44
		public override string ToString()
		{
			return string.Format("[BeAtPosition] {0}", this._targetPosition);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00014C5C File Offset: 0x00012E5C
		public void Reset()
		{
		}

		// Token: 0x040000FE RID: 254
		private WorldCoordinate _targetPosition;
	}
}
