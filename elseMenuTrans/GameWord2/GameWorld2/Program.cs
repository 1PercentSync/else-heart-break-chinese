using System;
using System.Collections.Generic;
using System.IO;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;

namespace GameWorld2
{
	// Token: 0x02000015 RID: 21
	public class Program : RelayObjectTwo, IReturnValueReceiver
	{
		// Token: 0x060001BD RID: 445 RVA: 0x00009ABC File Offset: 0x00007CBC
		protected override void SetupCells()
		{
			this.CELL_sourceCodeName = base.EnsureCell<string>("sourceCodeName", "");
			this.CELL_isOn = base.EnsureCell<bool>("isOn", false);
			this.CELL_name = base.EnsureCell<string>("name", "undefined");
			this.CELL_sourceCodeContent = base.EnsureCell<string>("sourceCode", "");
			this.CELL_sleepTimer = base.EnsureCell<float>("sleepTimer", 0f);
			this.CELL_remoteCaller = base.EnsureCell<int>("remoteCaller", -1);
			this.CELL_waitingForInput = base.EnsureCell<bool>("waitingForInput", false);
			this.CELL_executionCounter = base.EnsureCell<int>("executionCounter", 0);
			this.CELL_executionsPerFrame = base.EnsureCell<int>("executionsPerFrame", 50);
			this.CELL_compilationTurnedOn = base.EnsureCell<bool>("compilationTurnedOn", true);
			this.CELL_executionTime = base.EnsureCell<float>("executionTime", 0f);
			this.CELL_maxExecutionTime = base.EnsureCell<float>("maxExecutionTime", -1f);
			this._isOn_Cache = this.CELL_isOn.data;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009BCC File Offset: 0x00007DCC
		public void Init(ProgramRunner pProgramRunner)
		{
			this._programRunner = pProgramRunner;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009BD8 File Offset: 0x00007DD8
		public void StopAndReset()
		{
			this.uniqueCompilationId++;
			this.isOn = false;
			this.sleepTimer = 0f;
			this.remoteCaller = null;
			this._mockProgram = null;
			this.waitingForInput = false;
			this.executionCounter = 0;
			this.executionTime = 0f;
			if (this._sprakRunner != null)
			{
				this._sprakRunner.returnFromExternalFunctionCall = false;
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00009C44 File Offset: 0x00007E44
		public void ClearErrors()
		{
			if (this._sprakRunner != null)
			{
				this._sprakRunner.getCompileTimeErrorHandler().getErrors().Clear();
				this._sprakRunner.getRuntimeErrorHandler().getErrors().Clear();
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00009C88 File Offset: 0x00007E88
		public void ClearRuntimeErrors()
		{
			if (this._sprakRunner != null)
			{
				this._sprakRunner.getRuntimeErrorHandler().getErrors().Clear();
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00009CB8 File Offset: 0x00007EB8
		public Error[] Compile()
		{
			if (!this.compilationTurnedOn)
			{
				return new Error[]
				{
					new Error("Uncompiled program.")
				};
			}
			this.StopAndReset();
			if (this._sprakRunner == null)
			{
				this._sprakRunner = new SprakRunner(new StringReader(this.sourceCodeContent), this.FunctionDefinitions.ToArray(), this.VariableDefinitions.ToArray());
			}
			else
			{
				this._sprakRunner.Reset();
			}
			this.PrintErrorsToD();
			return this.GetErrors();
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009D40 File Offset: 0x00007F40
		private void EnsureSprakRunner()
		{
			if (this._sprakRunner == null)
			{
				this.Compile();
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00009D54 File Offset: 0x00007F54
		public SprakRunner sprakRunner
		{
			get
			{
				return this._sprakRunner;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00009D5C File Offset: 0x00007F5C
		public Dictionary<string, ProfileData> GetProfileData()
		{
			return this._sprakRunner.GetProfileData();
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009D6C File Offset: 0x00007F6C
		public void Start()
		{
			this.Compile();
			this.isOn = true;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00009D7C File Offset: 0x00007F7C
		public void StartAtFunction(string functionName, object[] args, Program pCaller)
		{
			this.StartAtFunction(functionName, args, pCaller, true);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00009D88 File Offset: 0x00007F88
		public void StartAtFunctionIfItExists(string functionName, object[] args, Program pCaller)
		{
			this.StartAtFunction(functionName, args, pCaller, false);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00009D94 File Offset: 0x00007F94
		public void StartAtFunction(string functionName, object[] args, Program pCaller, bool pEnsureThatFunctionExists)
		{
			if (this._sprakRunner == null)
			{
				Error[] array = this.Compile();
				if (this._sprakRunner == null)
				{
					if (pCaller != null)
					{
						foreach (Error error in array)
						{
							pCaller._sprakRunner.getRuntimeErrorHandler().errorOccured(error);
							D.Log(" - " + error);
						}
					}
					D.Log(string.Concat(new string[] { "Will NOT run ", functionName, " on ", this.nameOfOwner, ", failed to create sprak runner second time" }));
					return;
				}
			}
			else if (this.ContainsErrors())
			{
				D.Log(string.Concat(new object[]
				{
					"Clearing errors (",
					this.GetErrors().Length,
					") on ",
					this.ToString(),
					" because of incoming function call ",
					functionName
				}));
				this.ClearErrors();
			}
			try
			{
				this.remoteCaller = pCaller;
				this._mockProgram = null;
				if (this.remoteCaller != null)
				{
					this.callersUniqueCompilationId = pCaller.uniqueCompilationId;
				}
				InterpreterTwo.ProgramFunctionCallStatus programFunctionCallStatus = this._sprakRunner.ResetAtFunction(functionName, args);
				if (programFunctionCallStatus == InterpreterTwo.ProgramFunctionCallStatus.NO_FUNCTION)
				{
					Error error2 = new Error("Can't find function '" + functionName + "' (forgot quotes?)");
					if (pEnsureThatFunctionExists)
					{
						throw error2;
					}
					if (this.remoteCaller != null)
					{
						this.remoteCaller._sprakRunner.getRuntimeErrorHandler().errorOccured(error2);
					}
					this.StopAndReset();
				}
				else
				{
					if (programFunctionCallStatus == InterpreterTwo.ProgramFunctionCallStatus.NORMAL_FUNCTION)
					{
						this.isOn = true;
					}
					else if (programFunctionCallStatus == InterpreterTwo.ProgramFunctionCallStatus.EXTERNAL_FUNCTION)
					{
						this.isOn = true;
						this.sprakRunner.returnFromExternalFunctionCall = true;
					}
					this.waitingForInput = false;
				}
			}
			catch (Error error3)
			{
				D.Log(string.Concat(new object[]
				{
					"Error when trying to call function: ",
					error3,
					" of type ",
					error3.getErrorType()
				}));
				if (this.remoteCaller != null && error3.getErrorType() != Error.ErrorType.RUNTIME)
				{
					D.Log("Logging error on only remote caller: " + this.remoteCaller);
					this.remoteCaller._sprakRunner.getRuntimeErrorHandler().errorOccured(error3);
				}
				else
				{
					D.Log(string.Concat(new object[] { "Logging error on self: ", this, " and remote caller ", this.remoteCaller }));
					if (this.remoteCaller != null && this.remoteCaller._sprakRunner != null && this.remoteCaller._sprakRunner.getRuntimeErrorHandler() != null)
					{
						this.remoteCaller._sprakRunner.getRuntimeErrorHandler().errorOccured(error3);
					}
					if (this._sprakRunner == null)
					{
						this.Compile();
					}
					if (this._sprakRunner != null)
					{
						this._sprakRunner.getRuntimeErrorHandler().errorOccured(error3);
					}
					else
					{
						D.Log(string.Concat(new object[]
						{
							"Sprak runner in ",
							this.ToString(),
							" still null, when trying to add runtime error: ",
							error3
						}));
					}
				}
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000A0D4 File Offset: 0x000082D4
		public void StartAtFunctionWithMockReceiver(string functionName, object[] args, MockProgram pMockProgram)
		{
			this.Start();
			try
			{
				this.remoteCaller = null;
				this._mockProgram = pMockProgram;
				this._sprakRunner.ResetAtFunction(functionName, args);
			}
			catch (Error error)
			{
				D.Log("Error when trying to call function using mock receiver: " + error);
				if (this.remoteCaller != null)
				{
					this.remoteCaller._sprakRunner.getRuntimeErrorHandler().errorOccured(error);
				}
				this._sprakRunner.getRuntimeErrorHandler().errorOccured(error);
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000A16C File Offset: 0x0000836C
		public void Update(float dt)
		{
			this.executionTime += dt;
			if (this.maxExecutionTime > 0f && this.executionTime >= this.maxExecutionTime)
			{
				this.StopAndReset();
				return;
			}
			if (this.ContainsErrors())
			{
				return;
			}
			if (this.waitingForInput)
			{
				return;
			}
			if (this.sleepTimer > 0f)
			{
				this.sleepTimer -= dt;
				return;
			}
			int executionsPerFrame = this.executionsPerFrame;
			this.Execute(executionsPerFrame);
			this.executionCounter += executionsPerFrame;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000A204 File Offset: 0x00008404
		private void Execute(int pExecutions)
		{
			if (!this.compilationTurnedOn)
			{
				return;
			}
			if (!this._sprakRunner.isStarted && !this._sprakRunner.Start())
			{
				D.Log(this.ToString() + " failed to start, will not execute");
				this.PrintErrorsToD();
				return;
			}
			for (int i = 0; i < pExecutions; i++)
			{
				InterpreterTwo.Status status = this._sprakRunner.Step();
				if (this.sleepTimer > 0f)
				{
					break;
				}
				if (this.waitingForInput)
				{
					break;
				}
				if (this.waitForNextFrame)
				{
					this.waitForNextFrame = false;
					break;
				}
				if (status == InterpreterTwo.Status.FINISHED || this._sprakRunner.returnFromExternalFunctionCall)
				{
					if (this.remoteCaller != null)
					{
						if (this.remoteCaller.uniqueCompilationId != this.callersUniqueCompilationId)
						{
							D.Log("The uniqueExecutionId of " + this.ToString() + " has changed after it called the remote function on " + this.remoteCaller.ToString());
						}
						else
						{
							object finalReturnValue = this._sprakRunner.GetFinalReturnValue();
							this.remoteCaller.OnReturnValue(finalReturnValue);
						}
					}
					else if (this._mockProgram != null)
					{
						object finalReturnValue2 = this._sprakRunner.GetFinalReturnValue();
						this._mockProgram.OnReturnValue(finalReturnValue2);
					}
					this.StopAndReset();
					break;
				}
				if (status == InterpreterTwo.Status.ERROR)
				{
					this.isOn = false;
					this.PrintErrorsToD();
					break;
				}
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000A384 File Offset: 0x00008584
		public void OnReturnValue(object pReturnValue)
		{
			try
			{
				this.SwapStackTopValueTo(pReturnValue);
			}
			catch (Error error)
			{
				this._sprakRunner.getRuntimeErrorHandler().errorOccured(error);
			}
			finally
			{
				this.waitingForInput = false;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000A3F0 File Offset: 0x000085F0
		private void PrintErrorsToD()
		{
			foreach (Error error in this.GetErrors())
			{
				D.Log("Error in program '" + this.ToString() + "': " + error.Message);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000A43C File Offset: 0x0000863C
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000A444 File Offset: 0x00008644
		public List<FunctionDefinition> FunctionDefinitions
		{
			get
			{
				return this._functionDefinitions;
			}
			set
			{
				this._functionDefinitions = value;
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000A450 File Offset: 0x00008650
		public bool HasFunction(string pFunctionName, bool pFullExpensiveCheck)
		{
			if (pFullExpensiveCheck)
			{
				if (this._sprakRunner == null)
				{
					this.Compile();
					if (this._sprakRunner == null)
					{
						D.Log("Failed to Compile() when calling HasFunction with function name " + pFunctionName);
						return false;
					}
				}
				else
				{
					this.Compile();
				}
				if (this._sprakRunner.HasFunction(pFunctionName))
				{
					return true;
				}
			}
			foreach (FunctionDefinition functionDefinition in this._functionDefinitions)
			{
				if (functionDefinition.functionName == pFunctionName)
				{
					return true;
				}
			}
			foreach (FunctionDefinition functionDefinition2 in SprakRunner.builtInFunctions)
			{
				if (functionDefinition2.functionName == pFunctionName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000A58C File Offset: 0x0000878C
		public bool TryGetFunctionDefinition(string pFunctionName, out FunctionDefinition pOutput)
		{
			foreach (FunctionDefinition functionDefinition in this._functionDefinitions)
			{
				if (functionDefinition.functionName == pFunctionName)
				{
					pOutput = functionDefinition;
					return true;
				}
			}
			foreach (FunctionDefinition functionDefinition2 in SprakRunner.builtInFunctions)
			{
				if (functionDefinition2.functionName == pFunctionName)
				{
					pOutput = functionDefinition2;
					return true;
				}
			}
			pOutput = default(FunctionDefinition);
			return false;
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000A688 File Offset: 0x00008888
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000A690 File Offset: 0x00008890
		public List<VariableDefinition> VariableDefinitions
		{
			get
			{
				return this._variableDefinitions;
			}
			set
			{
				this._variableDefinitions = value;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000A69C File Offset: 0x0000889C
		public void SwapStackTopValueTo(object pValue)
		{
			if (this._sprakRunner == null)
			{
				D.Log("Sprak runner is null");
				return;
			}
			if (pValue == null)
			{
				D.Log("pValue is null, won't swap stack top value");
				return;
			}
			this._sprakRunner.SwapStackTopValueTo(pValue);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000A6D4 File Offset: 0x000088D4
		public void ChangeGlobalVariableInitValue(string pName, object pValue)
		{
			this._sprakRunner.ChangeGlobalVariableInitValue(pName, pValue);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000A6E4 File Offset: 0x000088E4
		public object GetGlobalVariableValue(string pName)
		{
			return this._sprakRunner.GetGlobalVariableValue(pName);
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000A6F4 File Offset: 0x000088F4
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x0000A6FC File Offset: 0x000088FC
		public bool isOn
		{
			get
			{
				return this._isOn_Cache;
			}
			private set
			{
				this._isOn_Cache = value;
				this.CELL_isOn.data = value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000A714 File Offset: 0x00008914
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000A724 File Offset: 0x00008924
		public string name
		{
			get
			{
				return this.CELL_name.data;
			}
			set
			{
				this.CELL_name.data = value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000A734 File Offset: 0x00008934
		// (set) Token: 0x060001DD RID: 477 RVA: 0x0000A744 File Offset: 0x00008944
		public string sourceCodeName
		{
			get
			{
				return this.CELL_sourceCodeName.data;
			}
			set
			{
				this.CELL_sourceCodeName.data = value;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000A754 File Offset: 0x00008954
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000A764 File Offset: 0x00008964
		public float sleepTimer
		{
			get
			{
				return this.CELL_sleepTimer.data;
			}
			set
			{
				this.CELL_sleepTimer.data = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000A774 File Offset: 0x00008974
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x0000A784 File Offset: 0x00008984
		public bool waitingForInput
		{
			get
			{
				return this.CELL_waitingForInput.data;
			}
			set
			{
				this.CELL_waitingForInput.data = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000A794 File Offset: 0x00008994
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x0000A7A4 File Offset: 0x000089A4
		public bool compilationTurnedOn
		{
			get
			{
				return this.CELL_compilationTurnedOn.data;
			}
			set
			{
				this.CELL_compilationTurnedOn.data = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000A7B4 File Offset: 0x000089B4
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x0000A7C4 File Offset: 0x000089C4
		public int executionCounter
		{
			get
			{
				return this.CELL_executionCounter.data;
			}
			set
			{
				this.CELL_executionCounter.data = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000A7D4 File Offset: 0x000089D4
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x0000A7E4 File Offset: 0x000089E4
		public string sourceCodeContent
		{
			get
			{
				return this.CELL_sourceCodeContent.data;
			}
			set
			{
				this.CELL_sourceCodeContent.data = value;
				this.DeleteSprakRunner();
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000A7F8 File Offset: 0x000089F8
		private void DeleteSprakRunner()
		{
			if (this._sprakRunner != null)
			{
				this._sprakRunner.HardReset();
			}
			this._sprakRunner = null;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000A818 File Offset: 0x00008A18
		// (set) Token: 0x060001EA RID: 490 RVA: 0x0000A830 File Offset: 0x00008A30
		public Program remoteCaller
		{
			get
			{
				return this._programRunner.GetProgramUnsafe(this.CELL_remoteCaller.data);
			}
			set
			{
				if (value == null)
				{
					this.CELL_remoteCaller.data = -1;
				}
				else
				{
					this.CELL_remoteCaller.data = value.objectId;
				}
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000A868 File Offset: 0x00008A68
		public Error[] GetErrors()
		{
			if (!this.compilationTurnedOn)
			{
				return new Error[0];
			}
			this.EnsureSprakRunner();
			List<Error> list = new List<Error>();
			list.AddRange(this._sprakRunner.getRuntimeErrorHandler().getErrors());
			list.AddRange(this._sprakRunner.getCompileTimeErrorHandler().getErrors());
			return list.ToArray();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000A8C8 File Offset: 0x00008AC8
		public bool ContainsErrors()
		{
			if (!this.compilationTurnedOn)
			{
				return false;
			}
			this.EnsureSprakRunner();
			return this._sprakRunner.getRuntimeErrorHandler().getErrors().Count > 0 || this._sprakRunner.getCompileTimeErrorHandler().getErrors().Count > 0;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000A920 File Offset: 0x00008B20
		public override string ToString()
		{
			return string.Concat(new string[] { this.name, "/", this.sourceCodeName, "/", this.nameOfOwner });
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000A964 File Offset: 0x00008B64
		// (set) Token: 0x060001EF RID: 495 RVA: 0x0000A974 File Offset: 0x00008B74
		public int executionsPerFrame
		{
			get
			{
				return this.CELL_executionsPerFrame.data;
			}
			set
			{
				this.CELL_executionsPerFrame.data = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000A984 File Offset: 0x00008B84
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x0000A994 File Offset: 0x00008B94
		public float executionTime
		{
			get
			{
				return this.CELL_executionTime.data;
			}
			set
			{
				this.CELL_executionTime.data = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000A9A4 File Offset: 0x00008BA4
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		public float maxExecutionTime
		{
			get
			{
				return this.CELL_maxExecutionTime.data;
			}
			set
			{
				this.CELL_maxExecutionTime.data = value;
			}
		}

		// Token: 0x0400007E RID: 126
		public const string TABLE_NAME = "Programs";

		// Token: 0x0400007F RID: 127
		public Logger logger = new Logger();

		// Token: 0x04000080 RID: 128
		private SprakRunner _sprakRunner;

		// Token: 0x04000081 RID: 129
		private ProgramRunner _programRunner;

		// Token: 0x04000082 RID: 130
		private List<FunctionDefinition> _functionDefinitions = new List<FunctionDefinition>();

		// Token: 0x04000083 RID: 131
		private List<VariableDefinition> _variableDefinitions = new List<VariableDefinition>();

		// Token: 0x04000084 RID: 132
		private ValueEntry<string> CELL_name;

		// Token: 0x04000085 RID: 133
		private ValueEntry<string> CELL_sourceCodeName;

		// Token: 0x04000086 RID: 134
		private ValueEntry<bool> CELL_isOn;

		// Token: 0x04000087 RID: 135
		private ValueEntry<string> CELL_sourceCodeContent;

		// Token: 0x04000088 RID: 136
		private ValueEntry<float> CELL_sleepTimer;

		// Token: 0x04000089 RID: 137
		private ValueEntry<int> CELL_remoteCaller;

		// Token: 0x0400008A RID: 138
		private ValueEntry<bool> CELL_waitingForInput;

		// Token: 0x0400008B RID: 139
		private ValueEntry<int> CELL_executionCounter;

		// Token: 0x0400008C RID: 140
		private ValueEntry<int> CELL_executionsPerFrame;

		// Token: 0x0400008D RID: 141
		private ValueEntry<bool> CELL_compilationTurnedOn;

		// Token: 0x0400008E RID: 142
		private ValueEntry<float> CELL_executionTime;

		// Token: 0x0400008F RID: 143
		private ValueEntry<float> CELL_maxExecutionTime;

		// Token: 0x04000090 RID: 144
		private bool _isOn_Cache;

		// Token: 0x04000091 RID: 145
		public bool waitForNextFrame;

		// Token: 0x04000092 RID: 146
		private MockProgram _mockProgram;

		// Token: 0x04000093 RID: 147
		private int uniqueCompilationId = 0;

		// Token: 0x04000094 RID: 148
		private int callersUniqueCompilationId = -1;

		// Token: 0x04000095 RID: 149
		public string nameOfOwner = "unknown";
	}
}
