using System;
using GrimmLib;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000057 RID: 87
	public class TrapAPI
	{
		// Token: 0x06000564 RID: 1380 RVA: 0x0001A8FC File Offset: 0x00018AFC
		public TrapAPI(Computer pComputer, TingRunner pTingRunner, DialogueRunner pDialogueRunner)
		{
			this._computer = pComputer;
			this._tingRunner = pTingRunner;
			this._dialogueRunner = pDialogueRunner;
			TingRunner tingRunner = this._tingRunner;
			tingRunner.onTingHasNewRoom = (TingRunner.OnNewRoom)Delegate.Combine(tingRunner.onTingHasNewRoom, new TingRunner.OnNewRoom(this.OnTingHasNewRoom));
			this._dialogueRunner.AddOnEventListener(new DialogueRunner.OnEvent(this.OnEvent));
			this._computerRoomCache = this._computer.room.name;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001A978 File Offset: 0x00018B78
		protected override void Finalize()
		{
			try
			{
				TingRunner tingRunner = this._tingRunner;
				tingRunner.onTingHasNewRoom = (TingRunner.OnNewRoom)Delegate.Remove(tingRunner.onTingHasNewRoom, new TingRunner.OnNewRoom(this.OnTingHasNewRoom));
				this._dialogueRunner.RemoveOnEventListener(new DialogueRunner.OnEvent(this.OnEvent));
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001A9EC File Offset: 0x00018BEC
		private void OnTingHasNewRoom(Ting pTing, string pNewRoomName)
		{
			if (pNewRoomName == this._computerRoomCache && !this._computer.masterProgram.isOn)
			{
				this._computer.masterProgram.maxExecutionTime = 10f;
				this._computer.masterProgram.StartAtFunctionIfItExists("OnIntruder", new object[] { pTing.name }, null);
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001AA5C File Offset: 0x00018C5C
		private void OnEvent(string pEvent)
		{
			if (pEvent.Contains("_hack_"))
			{
				string text = pEvent.Substring(0, pEvent.IndexOf('_'));
				Character ting = this._tingRunner.GetTing<Character>(text);
				if (ting.room == this._computer.room)
				{
					this.OnHack(text);
				}
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001AAB8 File Offset: 0x00018CB8
		private void OnHack(string pHackerName)
		{
			if (!this._computer.masterProgram.isOn)
			{
				this._computer.masterProgram.maxExecutionTime = 10f;
				this._computer.masterProgram.StartAtFunctionIfItExists("OnHack", new object[] { pHackerName }, null);
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001AB10 File Offset: 0x00018D10
		[SprakAPI(new string[] { "Checks whether a string contains a substring" })]
		public bool API_StringContains(string s, string substr)
		{
			return s.Contains(substr);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001AB1C File Offset: 0x00018D1C
		[SprakAPI(new string[] { "Move an intruder in the room to the position of another thing" })]
		public void API_MovePerson(string name, string target)
		{
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(name);
			Ting tingUnsafe2 = this._tingRunner.GetTingUnsafe(target);
			if (tingUnsafe == null || tingUnsafe.room != this._computer.room)
			{
				throw new Error("Can't find " + name + " in this room");
			}
			if (tingUnsafe2 == null)
			{
				throw new Error(string.Concat(new string[] { "Can't find ", target, " to send ", name, " to" }));
			}
			tingUnsafe.position = tingUnsafe2.position;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001ABB8 File Offset: 0x00018DB8
		[SprakAPI(new string[] { "Zap an interuder in the room (will make them fall asleep)" })]
		public void API_ZapPerson(string name)
		{
			Character character = this._tingRunner.GetTingUnsafe(name) as Character;
			if (character == null || character.room != this._computer.room)
			{
				throw new Error("Can't find " + name + " in this room");
			}
			character.StopAction();
			character.GetTased();
			this._computer.PlaySound("FishAttack");
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001AC28 File Offset: 0x00018E28
		[SprakAPI(new string[] { "Resets code to default state" })]
		public void API_RestoreCode(string name)
		{
			MimanTing mimanTing = this._tingRunner.GetTingUnsafe(name) as MimanTing;
			if (mimanTing == null || mimanTing.room != this._computer.room)
			{
				throw new Error("Can't find " + name + " in this room");
			}
			foreach (Program program in mimanTing.programs)
			{
				string content = mimanTing.sourceCodeDispenser.GetSourceCode(program.sourceCodeName).content;
				if (program.sprakRunner != null)
				{
					program.sprakRunner.HardReset();
				}
				program.sourceCodeContent = content;
				program.StopAndReset();
			}
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001ACD4 File Offset: 0x00018ED4
		[SprakAPI(new string[] { "Broadcast a message. Use with caution.", "The message to be broadcasted" })]
		public void API_Broadcast(string pMessage)
		{
			this._dialogueRunner.EventHappened(pMessage);
		}

		// Token: 0x04000168 RID: 360
		private Computer _computer;

		// Token: 0x04000169 RID: 361
		private TingRunner _tingRunner;

		// Token: 0x0400016A RID: 362
		private DialogueRunner _dialogueRunner;

		// Token: 0x0400016B RID: 363
		private string _computerRoomCache;
	}
}
