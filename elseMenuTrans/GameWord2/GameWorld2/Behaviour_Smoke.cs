using System;
using GameTypes;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000038 RID: 56
	public class Behaviour_Smoke : TimetableBehaviour
	{
		// Token: 0x06000444 RID: 1092 RVA: 0x00015584 File Offset: 0x00013784
		public Behaviour_Smoke(string pTingName)
		{
			this._tingName = pTingName;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00015594 File Offset: 0x00013794
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			if (this._targetTing == null)
			{
				this._targetTing = pTingRunner.GetTing(this._tingName);
			}
			bool flag = pCharacter.room.name == this._targetTing.room.name;
			bool flag2 = MimanGrimmApiDefinitions.AreTingsWithinDistance(pCharacter, this._targetTing, 7);
			if (pCharacter.busy || pCharacter.talking)
			{
				return 1f;
			}
			bool flag3 = pCharacter.handItem is Cigarette;
			if (!flag)
			{
				pCharacter.WalkToTingAndInteract(this._targetTing);
				return 5f;
			}
			if (!pCharacter.sitting && this._targetTing is Seat)
			{
				pCharacter.WalkToTingAndInteract(this._targetTing);
				return 5f;
			}
			if (!flag2)
			{
				pCharacter.WalkTo(this._targetTing.position);
				return 3f;
			}
			if (flag3)
			{
				(pCharacter.handItem as Cigarette).charges = 3;
				pCharacter.InteractWith(pCharacter.handItem);
				return Randomizer.GetValue(7f, 15f);
			}
			if (pCharacter.HasInventoryItemOfType("Cigarette"))
			{
				Cigarette cigarette = null;
				foreach (Ting ting in pCharacter.inventoryItems)
				{
					if (ting is Cigarette)
					{
						cigarette = ting as Cigarette;
						break;
					}
				}
				if (cigarette != null)
				{
					pCharacter.TakeOutInventoryItem(cigarette);
				}
				else
				{
					D.Log("Cig was null!");
				}
				return 4f;
			}
			string text = "Tagg_Cigarrette";
			WorldCoordinate worldCoordinate = new WorldCoordinate(pCharacter.inventoryRoomName, IntPoint.Zero);
			string text2 = text + "_toSmoke_" + pWorldSettings.dynamicallyCreatedTingsCount++;
			Cigarette cigarette2 = pTingRunner.GetTingUnsafe(text2) as Cigarette;
			if (cigarette2 != null)
			{
				if (cigarette2.room.name == "Sebastian_inventory" || cigarette2.isBeingHeld)
				{
					D.Log("There's already a " + text2 + " but a character is holding it (or avatar has it)");
					for (int j = 0; j < 9999; j++)
					{
						string text3 = text2 + "_safe_" + j;
						if (!(pTingRunner.GetTingUnsafe(text3) is MimanTing))
						{
							text2 = text3;
							break;
						}
					}
				}
				else
				{
					D.Log("There's already a " + text2 + ", will use that one instead!");
					cigarette2.charges = 3;
					cigarette2.position = worldCoordinate;
				}
			}
			pTingRunner.CreateTingAfterUpdate<Cigarette>(text2, worldCoordinate, Direction.DOWN, text);
			return 3f;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00015844 File Offset: 0x00013A44
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return this._targetTing != null && pCharacter.room.name == this._targetTing.room.name;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00015880 File Offset: 0x00013A80
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00015884 File Offset: 0x00013A84
		public override string ToString()
		{
			return string.Format("[Smoke] {0}", this._tingName);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00015898 File Offset: 0x00013A98
		public void Reset()
		{
		}

		// Token: 0x04000109 RID: 265
		private string _tingName;

		// Token: 0x0400010A RID: 266
		private Ting _targetTing;
	}
}
