using System;
using GrimmLib;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000055 RID: 85
	public class VoiceAPI
	{
		// Token: 0x06000555 RID: 1365 RVA: 0x00019F40 File Offset: 0x00018140
		public VoiceAPI(Computer pComputer, TingRunner pTingRunner, DialogueRunner pDialogueRunner)
		{
			this._computer = pComputer;
			this._dialogueRunner = pDialogueRunner;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00019F58 File Offset: 0x00018158
		[SprakAPI(new string[] { "Listen" })]
		public string API_Listen()
		{
			this._computer.masterProgram.waitingForInput = true;
			this._dialogueRunner.AddOnSomeoneSaidSomethingListener(new DialogueRunner.OnSomeoneSaidSomething(this.OnSomeoneSaidSomething));
			return "LISTENING";
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00019F88 File Offset: 0x00018188
		public void OnSomeoneSaidSomething(Speech pSpeech)
		{
			this._computer.masterProgram.SwapStackTopValueTo(pSpeech.line);
			this._computer.masterProgram.waitingForInput = false;
			this._dialogueRunner.RemoveOnSomeoneSaidSomethingListener(new DialogueRunner.OnSomeoneSaidSomething(this.OnSomeoneSaidSomething));
		}

		// Token: 0x04000162 RID: 354
		private Computer _computer;

		// Token: 0x04000163 RID: 355
		private DialogueRunner _dialogueRunner;
	}
}
