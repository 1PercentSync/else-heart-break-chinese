using System;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000033 RID: 51
	public class Behaviour_BeAtTing : TimetableBehaviour
	{
		// Token: 0x06000423 RID: 1059 RVA: 0x00014CC4 File Offset: 0x00012EC4
		public Behaviour_BeAtTing(string pTingName)
		{
			this._tingName = pTingName;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00014CD4 File Offset: 0x00012ED4
		private bool ThereYet(Character pCharacter)
		{
			return this._targetTing != null && pCharacter.position == this._targetTing.position;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00014CFC File Offset: 0x00012EFC
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
			}
			else if (pCharacter.finalTargetPosition != this._targetTing.position)
			{
				pCharacter.WalkToTingAndInteract(this._targetTing);
			}
			return 1f;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00014D98 File Offset: 0x00012F98
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return this.ThereYet(pCharacter);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00014DA4 File Offset: 0x00012FA4
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00014DA8 File Offset: 0x00012FA8
		public override string ToString()
		{
			return string.Format("[BeAtTing] {0}", this._tingName);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00014DBC File Offset: 0x00012FBC
		public void Reset()
		{
		}

		// Token: 0x04000100 RID: 256
		private string _tingName;

		// Token: 0x04000101 RID: 257
		private Ting _targetTing;
	}
}
