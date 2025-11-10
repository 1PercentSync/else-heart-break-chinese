using System;
using System.Collections.Generic;
using System.Linq;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000062 RID: 98
	public class ConnectionAPI_Optimized
	{
		// Token: 0x060005E6 RID: 1510 RVA: 0x0001BCCC File Offset: 0x00019ECC
		public ConnectionAPI_Optimized(MimanTing pCaller, TingRunner pTingRunner, Program pProgram)
		{
			this._caller = pCaller;
			this._tingRunner = pTingRunner;
			this._program = pProgram;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001BCEC File Offset: 0x00019EEC
		public object DisconnectAll(object[] args)
		{
			if (this._caller != null)
			{
				this._caller.connectedTings = new MimanTing[0];
			}
			return VoidType.voidType;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001BD20 File Offset: 0x00019F20
		public static MimanTing GetTingFromNameOrSourceCodeName(TingRunner pTingRunner, string pName)
		{
			MimanTing mimanTing = pTingRunner.GetTingUnsafe(pName) as MimanTing;
			if (mimanTing != null)
			{
				return mimanTing;
			}
			foreach (Ting ting in pTingRunner.GetTings())
			{
				MimanTing mimanTing2 = ting as MimanTing;
				if (mimanTing2.masterProgram != null)
				{
					if (mimanTing2.masterProgram.sourceCodeName == pName)
					{
						return mimanTing2;
					}
				}
			}
			return null;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001BDCC File Offset: 0x00019FCC
		public object Connect(object[] args)
		{
			string text = ReturnValueConversions.SafeUnwrap<string>(args, 0);
			MimanTing tingFromNameOrSourceCodeName = ConnectionAPI_Optimized.GetTingFromNameOrSourceCodeName(this._tingRunner, text);
			if (tingFromNameOrSourceCodeName != null)
			{
				return this._caller.AddConnectionToTing(tingFromNameOrSourceCodeName);
			}
			if (this._caller is Computer)
			{
				(this._caller as Computer).API_Print("Can't find " + text + " to connect to");
			}
			else
			{
				this._caller.Say("Can't find " + text + " to connect to", "");
			}
			return -1f;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001BE68 File Offset: 0x0001A068
		public object RemoteFunctionCall(object[] args)
		{
			float num = (float)((int)ReturnValueConversions.SafeUnwrap<float>(args, 0));
			string text = ReturnValueConversions.SafeUnwrap<string>(args, 1);
			if (args[2].GetType() != typeof(SortedDictionary<KeyWrapper, object>))
			{
				throw new Error("Argument array to " + text + " is not an array");
			}
			SortedDictionary<KeyWrapper, object> sortedDictionary = ReturnValueConversions.SafeUnwrap<SortedDictionary<KeyWrapper, object>>(args, 2);
			if (this._caller.connectedTings.Length == 0)
			{
				this._caller.Say("No connected object to call function on", "");
				return 0f;
			}
			if (num == -1f)
			{
				this._caller.Say("Connection not open, can't call function.", "");
				return 0f;
			}
			if (num < 0f)
			{
				this._caller.Say("Receiver index is below 0: " + num, "");
				return 0f;
			}
			if (num >= (float)this._caller.connectedTings.Length)
			{
				this._caller.Say("Receiver index is too big (" + this._caller.connectedTings.Length + ")", "");
				return 0f;
			}
			MimanTing mimanTing = this._caller.connectedTings[(int)num];
			mimanTing.PrepareForBeingHacked();
			if (mimanTing.programs.Length == 0)
			{
				this._caller.Say("Connected thing has no programs to call", "");
				return 0f;
			}
			object[] array = sortedDictionary.Values.ToArray<object>();
			mimanTing.masterProgram.StartAtFunctionIfItExists(text, array, this._program);
			this._program.waitingForInput = true;
			return 0f;
		}

		// Token: 0x04000189 RID: 393
		private TingRunner _tingRunner;

		// Token: 0x0400018A RID: 394
		private MimanTing _caller;

		// Token: 0x0400018B RID: 395
		private Program _program;
	}
}
