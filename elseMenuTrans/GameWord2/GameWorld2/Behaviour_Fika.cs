using System;
using GameTypes;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000037 RID: 55
	public class Behaviour_Fika : TimetableBehaviour
	{
		// Token: 0x0600043E RID: 1086 RVA: 0x000152EC File Offset: 0x000134EC
		public Behaviour_Fika(string pRoomName)
		{
			this._roomName = pRoomName;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000152FC File Offset: 0x000134FC
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			if (pCharacter.busy || pCharacter.talking || pCharacter.sleeping)
			{
				return 1f;
			}
			if (!(pCharacter.room.name == this._roomName))
			{
				this._barista = null;
				InteractionHelper.GoToRoom(pCharacter, pRoomRunner, this._roomName, pTingRunner);
				return 10f;
			}
			Drink drink = pCharacter.handItem as Drink;
			if (drink == null)
			{
				if (this._barista == null)
				{
					this._barista = Behaviour_Party.GetBartender(pCharacter, pTingRunner);
				}
				if (this._barista == null)
				{
					return Randomizer.GetValue(3f, 6f);
				}
				if (this._barista.room != pCharacter.room || !this._barista.IsAtTimetableTaskOfType(typeof(Behaviour_Sell)))
				{
					this._barista = null;
					return 1f;
				}
				if (pCharacter.sitting)
				{
					pCharacter.GetUpFromSeat();
					return 1.8f;
				}
				if (this._barista.IsIdle() && (this._barista.conversationTarget == null || !MimanGrimmApiDefinitions.AreTingsWithinDistance(pCharacter, this._barista, 7)))
				{
					pCharacter.WalkToTingAndInteract(this._barista);
				}
				else
				{
					pCharacter.CancelWalking();
				}
				return 0.5f;
			}
			else
			{
				this._barista = null;
				if (!pCharacter.sitting)
				{
					if (this._favSeat == null || this._favSeat.isBeingUsed)
					{
						this._favSeat = InteractionHelper.GetRandomTing<Seat>(pTingRunner, pCharacter.room.name, pCharacter.tileGroup);
					}
					if (this._favSeat != null)
					{
						pCharacter.WalkToTingAndInteract(this._favSeat);
						return Randomizer.GetValue(3f, 6f);
					}
					return Randomizer.GetValue(3f, 6f);
				}
				else
				{
					if (drink.amount > 0f)
					{
						pCharacter.InteractWith(pCharacter.handItem);
						return Randomizer.GetValue(10f, 20f);
					}
					pCharacter.DropHandItemFar();
					return Randomizer.GetValue(30f, 45f);
				}
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001551C File Offset: 0x0001371C
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return pCharacter.room.name == this._roomName;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00015534 File Offset: 0x00013734
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
			Drink drink = pCharacter.handItem as Drink;
			if (drink != null)
			{
				pCharacter.MoveHandItemToInventory();
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00015560 File Offset: 0x00013760
		public override string ToString()
		{
			return string.Format("[Fika] {0}", this._roomName);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00015574 File Offset: 0x00013774
		public void Reset()
		{
			this._favSeat = null;
			this._barista = null;
		}

		// Token: 0x04000106 RID: 262
		private string _roomName;

		// Token: 0x04000107 RID: 263
		private Character _barista;

		// Token: 0x04000108 RID: 264
		private Seat _favSeat;
	}
}
