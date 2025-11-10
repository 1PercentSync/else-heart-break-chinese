using System;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200003D RID: 61
	public class Behaviour_Sleep : TimetableBehaviour
	{
		// Token: 0x0600046A RID: 1130 RVA: 0x00016674 File Offset: 0x00014874
		public Behaviour_Sleep(string pBedName)
		{
			this._bedName = pBedName;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00016684 File Offset: 0x00014884
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
			if (this._bed == null)
			{
				this._bed = pTingRunner.GetTing(this._bedName);
			}
			bool flag = pCharacter.bed == this._bed;
			bool flag2 = this._bed.position == pCharacter.finalTargetPosition || this._bed.HasInteractionPointHere(pCharacter.finalTargetPosition);
			bool flag3 = pCharacter.actionName == "Sleeping";
			if (!flag3 && this._bed is Point && MimanGrimmApiDefinitions.AreTingsWithinDistance(pCharacter, this._bed, 2))
			{
				pCharacter.Sleep(10);
				return 30f;
			}
			if (!flag && !flag2)
			{
				pCharacter.WalkToTingAndInteract(this._bed);
				return 5f;
			}
			if (flag3)
			{
				return 30f;
			}
			return 5f;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00016788 File Offset: 0x00014988
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001678C File Offset: 0x0001498C
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return this._bed != null && pCharacter.bed == this._bed;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000167AC File Offset: 0x000149AC
		public void Reset()
		{
		}

		// Token: 0x04000117 RID: 279
		private string _bedName;

		// Token: 0x04000118 RID: 280
		private Ting _bed;
	}
}
