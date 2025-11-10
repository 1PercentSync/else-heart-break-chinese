using System;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000063 RID: 99
	public class ConnectionAPI
	{
		// Token: 0x060005EB RID: 1515 RVA: 0x0001C018 File Offset: 0x0001A218
		public ConnectionAPI(MimanTing pCaller, TingRunner pTingRunner, Program pProgram)
		{
			this._caller = pCaller;
			this._tingRunner = pTingRunner;
			this._program = pProgram;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001C038 File Offset: 0x0001A238
		[SprakAPI(new string[] { "Remove all connections", "" })]
		public void API_DisconnectAll()
		{
			this._caller.connectedTings = new MimanTing[0];
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001C04C File Offset: 0x0001A24C
		[SprakAPI(new string[] { "Connect to something", "name" })]
		public float API_Connect(string name)
		{
			MimanTing tingFromNameOrSourceCodeName = ConnectionAPI_Optimized.GetTingFromNameOrSourceCodeName(this._tingRunner, name);
			if (tingFromNameOrSourceCodeName != null)
			{
				return this._caller.AddConnectionToTing(tingFromNameOrSourceCodeName);
			}
			string text = "Can't find " + name + " to connect to";
			if (this._caller is Computer)
			{
				(this._caller as Computer).API_Print(text);
			}
			else
			{
				this._caller.Say(text, "");
			}
			return -1f;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001C0C8 File Offset: 0x0001A2C8
		private int RemoteFunctionCall(float receiverIndex, string functionName, object[] arguments)
		{
			object[] array = new object[arguments.Length];
			int num = 0;
			foreach (object obj in arguments)
			{
				if (obj is float)
				{
					array[num] = (float)obj;
				}
				else if (obj is string)
				{
					array[num] = (string)obj;
				}
				else
				{
					if (!(obj is bool))
					{
						throw new Exception("Can't handle argument to remote function call.");
					}
					array[num] = (bool)obj;
				}
				num++;
			}
			if (this._caller.connectedTings.Length == 0)
			{
				this._caller.Say("No connected object to call function on", "");
				return 0;
			}
			if (receiverIndex == -1f)
			{
				this._caller.Say("Connection not open, can't call function.", "");
				return 0;
			}
			if (receiverIndex < 0f)
			{
				this._caller.Say("Receiver index is below 0: " + receiverIndex, "");
				return 0;
			}
			if (receiverIndex >= (float)this._caller.connectedTings.Length)
			{
				this._caller.Say("Receiver index is too big (" + this._caller.connectedTings.Length + ")", "");
				return 0;
			}
			MimanTing mimanTing = this._caller.connectedTings[(int)receiverIndex];
			mimanTing.PrepareForBeingHacked();
			if (mimanTing.programs.Length == 0)
			{
				this._caller.Say("Connected thing has no programs to call", "");
				return 0;
			}
			mimanTing.masterProgram.StartAtFunctionIfItExists(functionName, array, this._program);
			this._program.waitingForInput = true;
			return 0;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001C284 File Offset: 0x0001A484
		[SprakAPI(new string[] { "Call remote function on connected objects", "function name", "arguments" })]
		public int API_RemoteFunctionCall(float receiverIndex, string functionName, object[] arguments)
		{
			return this.RemoteFunctionCall(receiverIndex, functionName, arguments);
		}

		// Token: 0x0400018C RID: 396
		private TingRunner _tingRunner;

		// Token: 0x0400018D RID: 397
		private MimanTing _caller;

		// Token: 0x0400018E RID: 398
		private Program _program;
	}
}
