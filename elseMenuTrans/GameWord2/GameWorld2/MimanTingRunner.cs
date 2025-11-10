using System;
using GameTypes;
using GrimmLib;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200001B RID: 27
	public class MimanTingRunner : TingRunner
	{
		// Token: 0x0600020D RID: 525 RVA: 0x0000AF48 File Offset: 0x00009148
		public MimanTingRunner(RelayTwo pRelay, DialogueRunner pDialogueRunner, ProgramRunner pProgramRunner, SourceCodeDispenser pSourceCodeDispenser, RoomRunner pRoomRunner, TimetableRunner pTimetableRunner, WorldSettings pWorldSettings)
			: base(pRelay, pRoomRunner)
		{
			this._dialogueRunner = pDialogueRunner;
			this._dialogueRunner.AddOnSomeoneSaidSomethingListener(new DialogueRunner.OnSomeoneSaidSomething(this.OnSomeoneSaidSomething));
			this._programRunner = pProgramRunner;
			this._sourceCodeDispenser = pSourceCodeDispenser;
			this._timetableRunner = pTimetableRunner;
			this._worldSettings = pWorldSettings;
			foreach (Ting ting in this._tings.Values)
			{
				if (ting is MimanTing)
				{
					(ting as MimanTing).SetMimanRunners(this._programRunner, this._sourceCodeDispenser, this._dialogueRunner, this._worldSettings);
				}
				if (ting is Character)
				{
					(ting as Character).SetTimetableRunner(this._timetableRunner);
				}
			}
			foreach (Ting ting2 in this._tings.Values)
			{
				if (ting2 is MimanTing)
				{
					MimanTing mimanTing = ting2 as MimanTing;
					mimanTing.Init();
					if (mimanTing.autoUnregisterFromUpdate && mimanTing.dialogueLine != "")
					{
						base.Unregister(mimanTing);
					}
				}
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000B0D8 File Offset: 0x000092D8
		private void SetupRunners(Ting pTing)
		{
			if (pTing is Character)
			{
				(pTing as Character).SetTimetableRunner(this._timetableRunner);
			}
			if (pTing is MimanTing)
			{
				(pTing as MimanTing).SetMimanRunners(this._programRunner, this._sourceCodeDispenser, this._dialogueRunner, this._worldSettings);
				(pTing as MimanTing).Init();
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000B13C File Offset: 0x0000933C
		public override T CreateTing<T>(string pName, WorldCoordinate pWorldCoordinate, Direction pDirection)
		{
			T t = base.CreateTing<T>(pName, pWorldCoordinate, pDirection);
			this.SetupRunners(t);
			return t;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000B160 File Offset: 0x00009360
		public override T CreateTing<T>(string pName, WorldCoordinate pWorldCoordinate, Direction pDirection, string pPrefabName)
		{
			T t = base.CreateTing<T>(pName, pWorldCoordinate, pDirection, pPrefabName);
			this.SetupRunners(t);
			return t;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000B188 File Offset: 0x00009388
		public override T CreateTingAfterUpdate<T>(string pName, WorldCoordinate pWorldCoordinate, Direction pDirection, string pPrefabName)
		{
			T t = base.CreateTingAfterUpdate<T>(pName, pWorldCoordinate, pDirection, pPrefabName);
			this.SetupRunners(t);
			return t;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000B1B0 File Offset: 0x000093B0
		public void OnSomeoneSaidSomething(Speech pSpeech)
		{
			if (!base.HasTing(pSpeech.speaker))
			{
				throw new Exception("Can't find speaker with name '" + pSpeech.speaker + "'");
			}
			MimanTing mimanTing = base.GetTing(pSpeech.speaker) as MimanTing;
			mimanTing.Say(pSpeech.line, pSpeech.conversation);
		}

		// Token: 0x040000A0 RID: 160
		private DialogueRunner _dialogueRunner;

		// Token: 0x040000A1 RID: 161
		private ProgramRunner _programRunner;

		// Token: 0x040000A2 RID: 162
		private SourceCodeDispenser _sourceCodeDispenser;

		// Token: 0x040000A3 RID: 163
		private TimetableRunner _timetableRunner;

		// Token: 0x040000A4 RID: 164
		private WorldSettings _worldSettings;
	}
}
