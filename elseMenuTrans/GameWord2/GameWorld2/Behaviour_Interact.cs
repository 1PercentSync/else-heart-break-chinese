using System;
using GameTypes;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200003E RID: 62
	public class Behaviour_Interact : TimetableBehaviour
	{
		// Token: 0x0600046F RID: 1135 RVA: 0x000167B0 File Offset: 0x000149B0
		public Behaviour_Interact(string pTingName, bool pHackTargetTing)
		{
			this._tingName = pTingName;
			this._hackTargetTing = pHackTargetTing;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000167D0 File Offset: 0x000149D0
		private bool AtGoal(Character pCharacter)
		{
			if (this._ting == null)
			{
				D.Log(string.Concat(new string[] { "Ting ", this._tingName, " can't be found for character ", pCharacter.name, " in Interact behaviour" }));
				return false;
			}
			return this._ting.HasInteractionPointHere(pCharacter.position) || this._ting.position == pCharacter.position;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00016854 File Offset: 0x00014A54
		private void ListenForInteraction(string pEvent)
		{
			if (pEvent.Contains(this._tingName))
			{
				this._hasInteracted = true;
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00016870 File Offset: 0x00014A70
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
			bool flag = this.AtGoal(pCharacter);
			bool flag2 = this._ting.position == pCharacter.finalTargetPosition || this._ting.HasInteractionPointHere(pCharacter.finalTargetPosition);
			bool flag3 = pCharacter.actionName == "";
			if (!flag || !flag3 || !this._hasInteracted)
			{
				if (!flag && !flag2)
				{
					if (this._hackTargetTing)
					{
						pCharacter.WalkToTingAndHack(this._ting as MimanTing);
					}
					else
					{
						pCharacter.WalkToTingAndInteract(this._ting);
					}
				}
				else if (!flag)
				{
					D.Log(pCharacter + " will do new thing: StopAction() in Behaviour_Interact");
					pCharacter.StopAction();
				}
			}
			return 1f;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00016988 File Offset: 0x00014B88
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
			this._hasInteracted = false;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00016994 File Offset: 0x00014B94
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return this.AtGoal(pCharacter);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000169A0 File Offset: 0x00014BA0
		public void Reset()
		{
		}

		// Token: 0x04000119 RID: 281
		private string _tingName;

		// Token: 0x0400011A RID: 282
		private Ting _ting;

		// Token: 0x0400011B RID: 283
		private bool _hackTargetTing;

		// Token: 0x0400011C RID: 284
		private bool _hasInteracted = false;
	}
}
