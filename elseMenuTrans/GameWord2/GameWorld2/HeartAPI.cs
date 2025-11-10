using System;
using GameTypes;
using GrimmLib;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000058 RID: 88
	public class HeartAPI
	{
		// Token: 0x0600056E RID: 1390
		public HeartAPI(Computer pComputer, TingRunner pTingRunner, DialogueRunner pDialogueRunner)
		{
			this._computer = pComputer;
			this._tingRunner = pTingRunner;
			this._dialogueRunner = pDialogueRunner;
		}

		// Token: 0x06000570 RID: 1392
		[SprakAPI(new string[] { "Set numeric data on object设置对象上的数值数据" })]
		public void API_SetNumericData(string objectName, string dataName, float value)
		{
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(objectName);
			if (tingUnsafe == null)
			{
				this._computer.API_Print("Can't find object with name找不到名为 \"" + objectName + "\" 的对象");
				return;
			}
			tingUnsafe.table.SetValue<float>(tingUnsafe.objectId, dataName, value);
			this._computer.API_Print(string.Concat(new object[] { "Set设置 ", dataName, " to为 ", value, " on于 ", objectName }));
		}

		// Token: 0x06000571 RID: 1393
		[SprakAPI(new string[] { "Get numeric data on object获取对象上的数值数据" })]
		public float API_GetNumericData(string objectName, string dataName)
		{
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(objectName);
			if (tingUnsafe == null)
			{
				this._computer.API_Print("Can't find object with name找不到名为 \"" + objectName + "\" 的对象");
				return 0f;
			}
			return tingUnsafe.table.GetValue<float>(tingUnsafe.objectId, dataName);
		}

		// Token: 0x06000572 RID: 1394
		[SprakAPI(new string[] { "Break中断" })]
		public void API_Break()
		{
			this._computer.masterProgram.sourceCodeContent = "a00j锟斤烤烫烫烫 ksdhg 245kljshg a sl34烫烫烫3Monika4 kjghklj4烫烫烫34 651145141 1919810xsdhgklLoongLiveMonika!!!烤斤劏劏劏";
			this._computer.masterProgram.sourceCodeName = "";
			throw new Error("BROKEN已损坏", Error.ErrorType.RUNTIME, -1, -1);
		}

		// Token: 0x06000573 RID: 1395
		[SprakAPI(new string[] { "" })]
		public void API_ZapPersonGently(string name)
		{
			Character character = this._tingRunner.GetTingUnsafe(name) as Character;
			if (character == null)
			{
				throw new Error("Can't find找不到 " + name);
			}
			character.StopAction();
			character.GetTasedGently();
			if (Randomizer.OneIn(3))
			{
				character.Say(Randomizer.RandNth<string>(HeartAPI.zapExclaims), "Misc");
			}
		}

		// Token: 0x0400016C RID: 364
		private Computer _computer;

		// Token: 0x0400016D RID: 365
		private TingRunner _tingRunner;

		// Token: 0x0400016E RID: 366
		private DialogueRunner _dialogueRunner;

		// Token: 0x0400016F RID: 367
		private static string[] zapExclaims = new string[] { "Ahhhh!!!啊啊啊!!!", "1919810AAAA!!!", "OOHH噢噢噢", "UUUUuuuu呜呜呜呜", "114514!!!" };
	}
}
