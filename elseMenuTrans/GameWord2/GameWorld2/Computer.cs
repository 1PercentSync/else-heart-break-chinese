using System;
using System.Collections.Generic;
using System.Text;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000048 RID: 72
	public class Computer : MimanTing
	{
		// Token: 0x060004A8 RID: 1192 RVA: 0x00017630 File Offset: 0x00015830
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "HelloWorld");
			this.CELL_floppyBootProgramName = base.EnsureCell<string>("floppyBootProgramName", "BlankSlate");
			this.CELL_nrOfLines = base.EnsureCell<int>("nrOfLines", 24);
			this.CELL_charsOnLine = base.EnsureCell<int>("charsOnLine", 100);
			this.CELL_currentLine = base.EnsureCell<int>("currentLine", 0);
			this.CELL_currentTopLine = base.EnsureCell<int>("currentTopLine", 0);
			this.CELL_consoleOutput = base.EnsureCell<string[]>("consoleOutput", new string[64]);
			this.CELL_currentInput = base.EnsureCell<string>("currentInput", "");
			this.CELL_hasInternetAPI = base.EnsureCell<bool>("hasInternetAPI", true);
			this.CELL_hasGraphicsAPI = base.EnsureCell<bool>("hasGraphicsAPI", true);
			this.CELL_hasWeatherAPI = base.EnsureCell<bool>("hasWeatherAPI", false);
			this.CELL_hasLampAPI = base.EnsureCell<bool>("hasLampAPI", false);
			this.CELL_hasDoorAPI = base.EnsureCell<bool>("hasDoorAPI", false);
			this.CELL_hasMemoryAPI = base.EnsureCell<bool>("hasMemoryAPI", false);
			this.CELL_hasVoiceAPI = base.EnsureCell<bool>("hasVoiceAPI", false);
			this.CELL_hasElevatorAPI = base.EnsureCell<bool>("hasElevatorAPI", false);
			this.CELL_hasTingrunnerAPI = base.EnsureCell<bool>("hasTingrunnerAPI", false);
			this.CELL_hasTrapAPI = base.EnsureCell<bool>("hasTrapAPI", false);
			this.CELL_hasHeartAPI = base.EnsureCell<bool>("hasHeartAPI", false);
			this.CELL_hasArcadeMachineAPI = base.EnsureCell<bool>("hasArcadeMachineAPI", false);
			this.CELL_hasFloppyAPI = base.EnsureCell<bool>("hasFloppyAPI", false);
			this.CELL_mhz = base.EnsureCell<int>("mhz", 30);
			this.CELL_maxExecutionTime = base.EnsureCell<float>("maxExecutionTime", -1f);
			this.CELL_memoryUnitName = base.EnsureCell<string>("memoryUnit", "");
			this.CELL_floppyInDrive = base.EnsureCell<string>("floppyInDrive", "");
			this.CELL_screenWidth = base.EnsureCell<int>("screenWidth", 512);
			this.CELL_screenHeight = base.EnsureCell<int>("screenHeight", 256);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00017854 File Offset: 0x00015A54
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001785C File Offset: 0x00015A5C
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001786C File Offset: 0x00015A6C
		public void RemovePrograms()
		{
			this._program = null;
			this._floppyBootProgram = null;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001787C File Offset: 0x00015A7C
		public override void Update(float dt)
		{
			base.UpdateBubbleTimer();
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00017884 File Offset: 0x00015A84
		public override int securityLevel
		{
			get
			{
				if (this.masterProgramName == "CashRegister")
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x000178A0 File Offset: 0x00015AA0
		public Program activeProgram
		{
			get
			{
				if (this.floppyBootProgram != null && this.floppyBootProgram.isOn)
				{
					return this.floppyBootProgram;
				}
				return this.masterProgram;
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x000178D8 File Offset: 0x00015AD8
		public override void Say(string pLine, string pConversation)
		{
			if (this.onTextDrawing != null)
			{
				this.API_Print(pLine);
			}
			if (base.name.ToLower().Contains("bandit"))
			{
				if (this.user != null && !this.user.isAvatar)
				{
					return;
				}
				if (this.user == null)
				{
					return;
				}
			}
			base.Say(pLine, pConversation);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00017944 File Offset: 0x00015B44
		public override void FixBeforeSaving()
		{
			base.FixBeforeSaving();
			this.hasInternetAPI = true;
			if (base.name.Contains("TriPod"))
			{
				this.nrOfLines = 22;
				this.ReplaceHelloWorldProgram("TriPodOS");
				this.hasFloppyAPI = true;
				this.hasMemoryAPI = true;
			}
			else if (base.name.Contains("MediumSewerComputer"))
			{
				this.nrOfLines = 14;
				this.ReplaceHelloWorldProgram("MediumSewerComputer");
				this.hasFloppyAPI = true;
				this.hasMemoryAPI = true;
			}
			else if (base.name.Contains("MediumComputer1"))
			{
				this.nrOfLines = 14;
				this.ReplaceHelloWorldProgram("Moonlander");
				this.hasFloppyAPI = true;
			}
			else if (base.name.Contains("LargeComputerL4"))
			{
				this.nrOfLines = 16;
				this.ReplaceHelloWorldProgram("Wayfinder");
				this.hasFloppyAPI = true;
				this.hasDoorAPI = true;
			}
			else if (base.name.Contains("LargeComputerL3"))
			{
				this.nrOfLines = 16;
				this.ReplaceHelloWorldProgram("EmailClient");
				this.hasFloppyAPI = true;
			}
			else if (base.name.Contains("LargeComputerL2"))
			{
				this.ReplaceHelloWorldProgram("PedestrianOS");
				this.hasFloppyAPI = true;
				this.hasDoorAPI = true;
			}
			else if (base.name.Contains("HugeComputer"))
			{
				this.ReplaceHelloWorldProgram("ConnectionServer");
				this.hasFloppyAPI = true;
				this.hasMemoryAPI = true;
				this.hasGraphicsAPI = false;
			}
			else if (base.name.Contains("PillarComputer"))
			{
				this.ReplaceHelloWorldProgram("ConnectionServer2");
				this.hasFloppyAPI = true;
			}
			else if (base.name.Contains("Ministry"))
			{
				if (base.name.Contains("LargeRecorder"))
				{
					this.ReplaceHelloWorldProgram("MinistryLargeRecorder");
					this.hasTrapAPI = true;
					this.hasFloppyAPI = true;
				}
				else if (base.name.Contains("ModernComputer") || base.name.Contains("NewComputerScreen") || base.name.Contains("ComputerTerminal") || base.name.Contains("LapTop"))
				{
					this.ReplaceHelloWorldProgram("MinistryOS");
					this.hasFloppyAPI = true;
					this.hasMemoryAPI = true;
					this.nrOfLines = 16;
				}
				else if (base.name.Contains("FacadeComputer"))
				{
					this.ReplaceHelloWorldProgram("MinistryFacadeComputer");
					this.hasFloppyAPI = true;
				}
				else if (base.name.Contains("Wooper"))
				{
					this.ReplaceHelloWorldProgram("MinistryWooper");
					this.hasFloppyAPI = true;
				}
				else if (base.name.Contains("Cubbard"))
				{
					this.ReplaceHelloWorldProgram("MinistryInformationBoard");
					this.hasFloppyAPI = true;
				}
			}
			else if (base.name.Contains("RecorderComputer") || base.name.Contains("LargeRecorder"))
			{
				this.ReplaceHelloWorldProgram("WorldRecorderComputer");
				this.hasTrapAPI = true;
			}
			else if (base.name.Contains("SteeringComputer"))
			{
				this.ReplaceHelloWorldProgram("WorldSteeringComputer");
				this.hasFloppyAPI = true;
			}
			else if (base.name.Contains("LapTop"))
			{
				this.ReplaceHelloWorldProgram("LapTopOS");
				this.hasFloppyAPI = true;
			}
			else if (base.name.Contains("GardenBoxComputer"))
			{
				this.ReplaceHelloWorldProgram("WorldGardenBoxComputer");
			}
			else if (base.name.Contains("NewComputerScreen") || base.name.Contains("FlatScreen"))
			{
				this.ReplaceHelloWorldProgram("WorldNewComputerScreen");
			}
			else if (base.name.Contains("ComputerTerminalBoard"))
			{
				this.ReplaceHelloWorldProgram("ComputerTerminalBoard");
			}
			else if (base.name.Contains("Arcade"))
			{
				this.hasArcadeMachineAPI = true;
				this.hasFloppyAPI = true;
				this.hasMemoryAPI = true;
			}
			else if (base.name.Contains("CashRegister"))
			{
				this.nrOfLines = 6;
				this.hasFloppyAPI = true;
				this.hasMemoryAPI = true;
				this.ReplaceHelloWorldProgram("CashSystem");
			}
			else if (base.name.Contains("Cashier"))
			{
				this.hasMemoryAPI = true;
			}
			else if (base.name.Contains("Arcade"))
			{
				this.hasFloppyAPI = true;
				this.hasMemoryAPI = true;
				this.hasArcadeMachineAPI = true;
				this.maxExecutionTime = 30f;
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00017E34 File Offset: 0x00016034
		private void ReplaceHelloWorldProgram(string pNewProgramName)
		{
			if (this.masterProgramName == "HelloWorld")
			{
				this.masterProgramName = pNewProgramName;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00017E54 File Offset: 0x00016054
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00017E58 File Offset: 0x00016058
		public override string tooltipName
		{
			get
			{
				return "computer";
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00017E60 File Offset: 0x00016060
		public override string verbDescription
		{
			get
			{
				return "use";
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00017E68 File Offset: 0x00016068
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00017E78 File Offset: 0x00016078
		[EditableInEditor]
		public int mhz
		{
			get
			{
				return this.CELL_mhz.data;
			}
			set
			{
				this.CELL_mhz.data = value;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00017E88 File Offset: 0x00016088
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x00017E98 File Offset: 0x00016098
		[EditableInEditor]
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

		// Token: 0x060004B9 RID: 1209 RVA: 0x00017EA8 File Offset: 0x000160A8
		public void GetUsedBy(Character pUser, Floppy pFloppy)
		{
			this._user = pUser;
			this.RunProgram(pFloppy);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00017EB8 File Offset: 0x000160B8
		public void RunProgram(Floppy pFloppy)
		{
			if (this.floppyBootProgram != null)
			{
				this.floppyBootProgram.StopAndReset();
			}
			this.floppyInDrive = pFloppy;
			this.masterProgram.executionsPerFrame = this.mhz;
			if (this.maxExecutionTime > 0f)
			{
				this.masterProgram.maxExecutionTime = this.maxExecutionTime;
			}
			else if (this.maxExecutionTime <= -2f)
			{
				this.masterProgram.maxExecutionTime = -2f;
			}
			else
			{
				this.masterProgram.maxExecutionTime = 60f;
			}
			this.masterProgram.Start();
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00017F5C File Offset: 0x0001615C
		private string nSpaces(int n)
		{
			if (n < 0)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < n; i++)
			{
				stringBuilder.Append(" ");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00017FA0 File Offset: 0x000161A0
		[SprakAPI(new string[] { "Get information about the system" })]
		public void API_Info()
		{
			this.API_Print(base.name);
			this.API_Print("Speed: " + this.mhz + " mhz");
			if (this.CELL_hasInternetAPI.data)
			{
				this.API_Print("Has internet modem");
			}
			if (this.CELL_hasFloppyAPI.data)
			{
				this.API_Print("Has floppy drive");
			}
			if (this.CELL_hasMemoryAPI.data)
			{
				this.API_Print("Has memory unit");
			}
			this.API_Print("Screen width " + this.screenWidth + " pixels");
			this.API_Print("Screen height " + this.screenHeight + " pixels");
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001806C File Offset: 0x0001626C
		private void Checkbox(string pName, bool pTrue)
		{
			string text = ((!pTrue) ? " " : "X");
			this.API_Print("[" + text + "] " + pName);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000180A8 File Offset: 0x000162A8
		[SprakAPI(new string[] { "Get a random number between 0.0 and 1.0" })]
		public float API_Random()
		{
			return Randomizer.GetValue(0f, 1f);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000180BC File Offset: 0x000162BC
		[SprakAPI(new string[] { "Get the name of who is using the computer, if any" })]
		public string API_GetUser()
		{
			if (this._user == null)
			{
				return "";
			}
			return this._user.name;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000180DC File Offset: 0x000162DC
		[SprakAPI(new string[] { "Get the name of the computer" })]
		public string API_Name()
		{
			return base.name;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000180E4 File Offset: 0x000162E4
		[SprakAPI(new string[] { "Get the total time as a float" })]
		public float API_Time()
		{
			return this._worldSettings.totalWorldTime;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000180F4 File Offset: 0x000162F4
		[SprakAPI(new string[] { "Get the screen width" })]
		public float API_Width()
		{
			return (float)this.screenWidth;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00018100 File Offset: 0x00016300
		[SprakAPI(new string[] { "Get the screen height" })]
		public float API_Height()
		{
			return (float)this.screenHeight;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001810C File Offset: 0x0001630C
		[SprakAPI(new string[] { "Get the current hour" })]
		public float API_GetHour()
		{
			return (float)this._worldSettings.gameTimeClock.hours;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00018130 File Offset: 0x00016330
		[SprakAPI(new string[] { "Get the current minute" })]
		public float API_GetMinute()
		{
			return (float)this._worldSettings.gameTimeClock.minutes;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00018154 File Offset: 0x00016354
		[SprakAPI(new string[] { "Pause the master program", "number of seconds to pause for" })]
		public void API_Sleep(float seconds)
		{
			this.activeProgram.sleepTimer = seconds;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00018164 File Offset: 0x00016364
		[SprakAPI(new string[] { "Stop the program" })]
		public void API_Quit()
		{
			this.activeProgram.StopAndReset();
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00018174 File Offset: 0x00016374
		[SprakAPI(new string[] { "Convert a single character to a numeric value, 'a' equals 0" })]
		public float API_CharToInt(string character)
		{
			if (character == "")
			{
				return 0f;
			}
			char c = character[0];
			return (float)(c - 'a');
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000181A4 File Offset: 0x000163A4
		[SprakAPI(new string[] { "Convert a number to a character, 0 equals 'a'" })]
		public string API_IntToChar(float number)
		{
			return ((char)(97 + (int)number)).ToString();
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x000181C0 File Offset: 0x000163C0
		private void InternalPrint(string pText, bool pNewLine)
		{
			if (this.logger != null)
			{
				this.logger.Log("Printing on " + base.name + ": " + pText);
			}
			if (this.currentLine >= this.nrOfLines)
			{
				D.Log(string.Concat(new object[] { "Error in ", base.name, ", trying to write to line ", this.currentLine, " on console with only ", this.nrOfLines, " lines" }));
				this.currentLine = 0;
			}
			string[] consoleOutput = this.consoleOutput;
			int num = 0;
			for (;;)
			{
				int num2 = pText.Length - num;
				int num3 = Math.Min(this.charsOnLine, num2);
				string[] array;
				int currentLine;
				(array = consoleOutput)[currentLine = this.currentLine] = array[currentLine] + pText.Substring(num, num3);
				num += num3;
				if (num >= pText.Length)
				{
					break;
				}
				this.NextLine();
			}
			if (pNewLine)
			{
				this.NextLine();
				consoleOutput[this.currentLine] = "";
			}
			this.consoleOutput = consoleOutput;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000182EC File Offset: 0x000164EC
		[SprakAPI(new string[] { "Remove all text from the screen" })]
		public void API_ClearText()
		{
			string[] consoleOutput = this.consoleOutput;
			for (int i = 0; i < this.nrOfLines; i++)
			{
				consoleOutput[i] = "";
			}
			this.consoleOutput = consoleOutput;
			this.currentLine = 0;
			this.currentTopLine = 0;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00018334 File Offset: 0x00016534
		[SprakAPI(new string[] { "Print text to the screen" })]
		public void API_Print(string text)
		{
			this.InternalPrint(text, true);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00018340 File Offset: 0x00016540
		[SprakAPI(new string[] { "Say something through speaker" })]
		public void API_Say(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00018350 File Offset: 0x00016550
		[SprakAPI(new string[] { "Play a sound" })]
		public void API_PlaySound(string soundName)
		{
			base.PlaySound(soundName);
			base.audioLoop = false;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00018360 File Offset: 0x00016560
		[SprakAPI(new string[] { "Set the pitch of the sound" })]
		public void API_Pitch(float pitch)
		{
			base.pitch = pitch;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001836C File Offset: 0x0001656C
		[SprakAPI(new string[] { "The sinus function", "x" })]
		public float API_Sin(float x)
		{
			return (float)Math.Sin((double)x);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00018378 File Offset: 0x00016578
		[SprakAPI(new string[] { "The cosinus function", "x" })]
		public float API_Cos(float x)
		{
			return (float)Math.Cos((double)x);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00018384 File Offset: 0x00016584
		public void NextLine()
		{
			if (this.currentLine == this.currentTopLine - 1 || (this.currentLine == this.nrOfLines - 1 && this.currentTopLine == 0))
			{
				this.currentTopLine++;
			}
			this.currentLine++;
			this.consoleOutput[this.currentLine] = "";
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x000183F0 File Offset: 0x000165F0
		[SprakAPI(new string[] { "Print text without skipping to a new line afterwards" })]
		public void API_PrintS(string text)
		{
			this.InternalPrint(text, false);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000183FC File Offset: 0x000165FC
		[SprakAPI(new string[] { "Display a prompt and receive text input from the keyboard" })]
		public string API_Input(string prompt)
		{
			this.API_PrintS(prompt);
			this.activeProgram.waitingForInput = true;
			return "WAITING_FOR_INPUT";
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00018418 File Offset: 0x00016618
		public void OnKeyDown(string pKey)
		{
			if ("ABCDEFGHIKJLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ,.:;()_<>+-*/&%#$@!?\"'[]{}=\\".LastIndexOf(pKey) < 0)
			{
				return;
			}
			if (this.activeProgram.waitingForInput)
			{
				this.currentInput += pKey;
				string[] consoleOutput = this.consoleOutput;
				string[] array;
				int currentLine;
				(array = consoleOutput)[currentLine = this.currentLine] = array[currentLine] + pKey;
				this.consoleOutput = consoleOutput;
			}
			this.activeProgram.executionTime = 0f;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001848C File Offset: 0x0001668C
		public void OnEnterKey()
		{
			if (this.activeProgram.waitingForInput)
			{
				this.activeProgram.SwapStackTopValueTo(this.currentInput);
				this.activeProgram.waitingForInput = false;
				this.currentInput = "";
				this.NextLine();
				string[] consoleOutput = this.consoleOutput;
				consoleOutput[this.currentLine] = "";
				this.consoleOutput = consoleOutput;
			}
			this.activeProgram.executionTime = 0f;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00018504 File Offset: 0x00016704
		public void OnBackspaceKey()
		{
			if (this.activeProgram.waitingForInput && this.currentInput.Length > 0)
			{
				this.currentInput = this.currentInput.Substring(0, this.currentInput.Length - 1);
				string[] consoleOutput = this.consoleOutput;
				string text = consoleOutput[this.currentLine];
				consoleOutput[this.currentLine] = text.Substring(0, text.Length - 1);
				this.consoleOutput = consoleOutput;
			}
			this.activeProgram.executionTime = 0f;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00018590 File Offset: 0x00016790
		public void OnDirectionKey(string pKey)
		{
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00018594 File Offset: 0x00016794
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x000185A4 File Offset: 0x000167A4
		[EditableInEditor]
		public string masterProgramName
		{
			get
			{
				return this.CELL_programName.data;
			}
			set
			{
				this.CELL_programName.data = value;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x000185B4 File Offset: 0x000167B4
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x000185C4 File Offset: 0x000167C4
		[EditableInEditor]
		public string floppyBootProgramName
		{
			get
			{
				return this.CELL_floppyBootProgramName.data;
			}
			set
			{
				this.CELL_floppyBootProgramName.data = value;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x000185D4 File Offset: 0x000167D4
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x000185E4 File Offset: 0x000167E4
		[ShowInEditor]
		public string[] consoleOutput
		{
			get
			{
				return this.CELL_consoleOutput.data;
			}
			set
			{
				this.CELL_consoleOutput.data = value;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x000185F4 File Offset: 0x000167F4
		[ShowInEditor]
		public string programStatus
		{
			get
			{
				if (this._program == null)
				{
					return "NO PROGRAM";
				}
				return (!this.masterProgram.isOn) ? "OFF" : "ON";
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00018634 File Offset: 0x00016834
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x00018644 File Offset: 0x00016844
		[ShowInEditor]
		public string currentInput
		{
			get
			{
				return this.CELL_currentInput.data;
			}
			set
			{
				this.CELL_currentInput.data = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00018654 File Offset: 0x00016854
		[ShowInEditor]
		public int currentInputXPos
		{
			get
			{
				D.isNull(this.CELL_consoleOutput.data, "consoleOutput.data in " + base.name + " is null");
				if (this.CELL_consoleOutput.data[this.CELL_currentLine.data] == null)
				{
					return 0;
				}
				return this.CELL_consoleOutput.data[this.CELL_currentLine.data].Length;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x000186C4 File Offset: 0x000168C4
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this.GenerateProgramAPI(this._program);
				}
				return this._program;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00018708 File Offset: 0x00016908
		public Program floppyBootProgram
		{
			get
			{
				if (this._floppyBootProgram == null)
				{
					this._floppyBootProgram = base.EnsureProgram("FloppyBootProgram", this.floppyBootProgramName);
					this.GenerateProgramAPI(this._floppyBootProgram);
				}
				return this._floppyBootProgram;
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001874C File Offset: 0x0001694C
		private void GenerateProgramAPI(Program pProgram)
		{
			List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Computer)));
			if (this.hasGraphicsAPI)
			{
				GraphicsAPI graphicsAPI = new GraphicsAPI(this);
				list.AddRange(FunctionDefinitionCreator.CreateDefinitions(graphicsAPI, typeof(GraphicsAPI)));
				FunctionDefinition functionDefinition = new FunctionDefinition("void", "Lines", new string[] { "array" }, new string[] { "points" }, new ExternalFunctionCreator.OnFunctionCall(graphicsAPI.Lines), FunctionDocumentation.Default());
				list.Add(functionDefinition);
			}
			if (this.hasInternetAPI)
			{
				list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new InternetAPI(this, this._tingRunner), typeof(InternetAPI)));
				ConnectionAPI_Optimized connectionAPI_Optimized = new ConnectionAPI_Optimized(this, this._tingRunner, pProgram);
				list.Add(new FunctionDefinition("number", "Connect", new string[] { "string" }, new string[] { "name" }, new ExternalFunctionCreator.OnFunctionCall(connectionAPI_Optimized.Connect), FunctionDocumentation.Default()));
				list.Add(new FunctionDefinition("void", "DisconnectAll", new string[0], new string[0], new ExternalFunctionCreator.OnFunctionCall(connectionAPI_Optimized.DisconnectAll), new FunctionDocumentation("Remove all connections", new string[0])));
				list.Add(new FunctionDefinition("number", "RemoteFunctionCall", new string[] { "number", "string", "array" }, new string[] { "receiverIndex", "functionName", "arguments" }, new ExternalFunctionCreator.OnFunctionCall(connectionAPI_Optimized.RemoteFunctionCall), FunctionDocumentation.Default())
				{
					hideInModifier = true
				});
			}
			if (this.hasWeatherAPI)
			{
				list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new WeatherAPI(this, this._worldSettings), typeof(WeatherAPI)));
			}
			if (this.hasLampAPI)
			{
				list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new LampAPI(this, this._tingRunner), typeof(LampAPI)));
			}
			if (this.hasDoorAPI)
			{
				list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new DoorAPI(this, this._tingRunner, this._roomRunner), typeof(DoorAPI)));
			}
			if (this.hasMemoryAPI)
			{
				MemoryAPI memoryAPI = new MemoryAPI(this, this._tingRunner);
				list.AddRange(FunctionDefinitionCreator.CreateDefinitions(memoryAPI, typeof(MemoryAPI)));
				list.Add(new FunctionDefinition("void", "SaveMemory", new string[] { "string", "var" }, new string[] { "key", "value" }, new ExternalFunctionCreator.OnFunctionCall(memoryAPI.SaveMemory), FunctionDocumentation.Default()));
				list.Add(new FunctionDefinition("var", "LoadMemory", new string[] { "string" }, new string[] { "key" }, new ExternalFunctionCreator.OnFunctionCall(memoryAPI.LoadMemory), FunctionDocumentation.Default()));
			}
			if (this.hasVoiceAPI)
			{
				list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new VoiceAPI(this, this._tingRunner, this._dialogueRunner), typeof(VoiceAPI)));
			}
			if (this.hasElevatorAPI)
			{
				list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ElevatorAPI(this, this._tingRunner), typeof(ElevatorAPI)));
			}
			if (this.hasTingrunnerAPI)
			{
				list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new TingrunnerAPI(this, this._tingRunner, this._roomRunner), typeof(TingrunnerAPI)));
			}
			if (this.hasTrapAPI)
			{
				list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new TrapAPI(this, this._tingRunner, this._dialogueRunner), typeof(TrapAPI)));
			}
			if (this.hasHeartAPI)
			{
				list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new HeartAPI(this, this._tingRunner, this._dialogueRunner), typeof(HeartAPI)));
			}
			list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ArcadeMachineAPI(this), typeof(ArcadeMachineAPI)));
			list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new FloppyAPI(this, this._tingRunner), typeof(FloppyAPI)));
			pProgram.FunctionDefinitions = list;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00018B8C File Offset: 0x00016D8C
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
			else
			{
				this.masterProgram.nameOfOwner = base.name;
			}
			if (this.floppyBootProgram == null)
			{
				this.logger.Log("There was a problem generating the floppy boot program");
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00018BE8 File Offset: 0x00016DE8
		public void EnsureMemoryUnit()
		{
			if (this.memory == null)
			{
				string text = base.name + "_builtInMemoryUnit";
				Ting tingUnsafe = this._tingRunner.GetTingUnsafe(text);
				if (tingUnsafe != null)
				{
					this.memory = tingUnsafe as Memory;
				}
				else
				{
					Memory memory = this._tingRunner.CreateTing<Memory>(text, base.position, base.direction, "InvisibleHardDrive");
					this.memory = memory;
				}
				if (this.memory == null)
				{
					throw new Error("Failed to find/create memory unit");
				}
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00018C70 File Offset: 0x00016E70
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[] { base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 2 };
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00018CAC File Offset: 0x00016EAC
		public Character user
		{
			get
			{
				return this._user;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00018CB4 File Offset: 0x00016EB4
		// (set) Token: 0x060004EB RID: 1259 RVA: 0x00018CF4 File Offset: 0x00016EF4
		public Memory memory
		{
			get
			{
				if (this.CELL_memoryUnitName.data == "")
				{
					return null;
				}
				return this._tingRunner.GetTing<Memory>(this.CELL_memoryUnitName.data);
			}
			set
			{
				if (value == null)
				{
					this.CELL_memoryUnitName.data = "";
				}
				else
				{
					this.CELL_memoryUnitName.data = value.name;
				}
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00018D30 File Offset: 0x00016F30
		// (set) Token: 0x060004ED RID: 1261 RVA: 0x00018D40 File Offset: 0x00016F40
		[EditableInEditor]
		public string memoryUnitName
		{
			get
			{
				return this.CELL_memoryUnitName.data;
			}
			set
			{
				this.CELL_memoryUnitName.data = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x00018D50 File Offset: 0x00016F50
		// (set) Token: 0x060004EF RID: 1263 RVA: 0x00018D90 File Offset: 0x00016F90
		public Floppy floppyInDrive
		{
			get
			{
				if (this.CELL_floppyInDrive.data == "")
				{
					return null;
				}
				return this._tingRunner.GetTing<Floppy>(this.CELL_floppyInDrive.data);
			}
			set
			{
				if (value == null)
				{
					this.CELL_floppyInDrive.data = "";
				}
				else
				{
					this.CELL_floppyInDrive.data = value.name;
				}
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00018DCC File Offset: 0x00016FCC
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x00018DDC File Offset: 0x00016FDC
		[EditableInEditor]
		public string floppyInDriveName
		{
			get
			{
				return this.CELL_floppyInDrive.data;
			}
			set
			{
				this.CELL_floppyInDrive.data = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00018DEC File Offset: 0x00016FEC
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x00018DFC File Offset: 0x00016FFC
		[EditableInEditor]
		public int currentLine
		{
			get
			{
				return this.CELL_currentLine.data;
			}
			set
			{
				this.CELL_currentLine.data = value % this.nrOfLines;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00018E14 File Offset: 0x00017014
		// (set) Token: 0x060004F5 RID: 1269 RVA: 0x00018E24 File Offset: 0x00017024
		[EditableInEditor]
		public int nrOfLines
		{
			get
			{
				return this.CELL_nrOfLines.data;
			}
			set
			{
				this.CELL_nrOfLines.data = value;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00018E34 File Offset: 0x00017034
		// (set) Token: 0x060004F7 RID: 1271 RVA: 0x00018E44 File Offset: 0x00017044
		[EditableInEditor]
		public int charsOnLine
		{
			get
			{
				return this.CELL_charsOnLine.data;
			}
			set
			{
				this.CELL_charsOnLine.data = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00018E54 File Offset: 0x00017054
		// (set) Token: 0x060004F9 RID: 1273 RVA: 0x00018E64 File Offset: 0x00017064
		[EditableInEditor]
		public int currentTopLine
		{
			get
			{
				return this.CELL_currentTopLine.data;
			}
			set
			{
				this.CELL_currentTopLine.data = value % this.nrOfLines;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00018E7C File Offset: 0x0001707C
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x00018E8C File Offset: 0x0001708C
		[EditableInEditor]
		public bool hasInternetAPI
		{
			get
			{
				return this.CELL_hasInternetAPI.data;
			}
			set
			{
				this.CELL_hasInternetAPI.data = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00018E9C File Offset: 0x0001709C
		// (set) Token: 0x060004FD RID: 1277 RVA: 0x00018EAC File Offset: 0x000170AC
		[EditableInEditor]
		public bool hasGraphicsAPI
		{
			get
			{
				return this.CELL_hasGraphicsAPI.data;
			}
			set
			{
				this.CELL_hasGraphicsAPI.data = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00018EBC File Offset: 0x000170BC
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x00018ECC File Offset: 0x000170CC
		[EditableInEditor]
		public bool hasWeatherAPI
		{
			get
			{
				return this.CELL_hasWeatherAPI.data;
			}
			set
			{
				this.CELL_hasWeatherAPI.data = value;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x00018EDC File Offset: 0x000170DC
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x00018EEC File Offset: 0x000170EC
		[EditableInEditor]
		public bool hasLampAPI
		{
			get
			{
				return this.CELL_hasLampAPI.data;
			}
			set
			{
				this.CELL_hasLampAPI.data = value;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x00018EFC File Offset: 0x000170FC
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x00018F0C File Offset: 0x0001710C
		[EditableInEditor]
		public bool hasDoorAPI
		{
			get
			{
				return this.CELL_hasDoorAPI.data;
			}
			set
			{
				this.CELL_hasDoorAPI.data = value;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00018F1C File Offset: 0x0001711C
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x00018F2C File Offset: 0x0001712C
		[EditableInEditor]
		public bool hasMemoryAPI
		{
			get
			{
				return this.CELL_hasMemoryAPI.data;
			}
			set
			{
				this.CELL_hasMemoryAPI.data = value;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00018F3C File Offset: 0x0001713C
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00018F4C File Offset: 0x0001714C
		[EditableInEditor]
		public bool hasVoiceAPI
		{
			get
			{
				return this.CELL_hasVoiceAPI.data;
			}
			set
			{
				this.CELL_hasVoiceAPI.data = value;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00018F5C File Offset: 0x0001715C
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x00018F6C File Offset: 0x0001716C
		[EditableInEditor]
		public bool hasElevatorAPI
		{
			get
			{
				return this.CELL_hasElevatorAPI.data;
			}
			set
			{
				this.CELL_hasElevatorAPI.data = value;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00018F7C File Offset: 0x0001717C
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x00018F8C File Offset: 0x0001718C
		[EditableInEditor]
		public bool hasTingrunnerAPI
		{
			get
			{
				return this.CELL_hasTingrunnerAPI.data;
			}
			set
			{
				this.CELL_hasTingrunnerAPI.data = value;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x00018F9C File Offset: 0x0001719C
		// (set) Token: 0x0600050D RID: 1293 RVA: 0x00018FAC File Offset: 0x000171AC
		[EditableInEditor]
		public bool hasTrapAPI
		{
			get
			{
				return this.CELL_hasTrapAPI.data;
			}
			set
			{
				this.CELL_hasTrapAPI.data = value;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x00018FBC File Offset: 0x000171BC
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x00018FCC File Offset: 0x000171CC
		[EditableInEditor]
		public bool hasHeartAPI
		{
			get
			{
				return this.CELL_hasHeartAPI.data;
			}
			set
			{
				this.CELL_hasHeartAPI.data = value;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x00018FDC File Offset: 0x000171DC
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x00018FEC File Offset: 0x000171EC
		[EditableInEditor]
		public bool hasArcadeMachineAPI
		{
			get
			{
				return this.CELL_hasArcadeMachineAPI.data;
			}
			set
			{
				this.CELL_hasArcadeMachineAPI.data = value;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00018FFC File Offset: 0x000171FC
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x0001900C File Offset: 0x0001720C
		[EditableInEditor]
		public bool hasFloppyAPI
		{
			get
			{
				return this.CELL_hasFloppyAPI.data;
			}
			set
			{
				this.CELL_hasFloppyAPI.data = value;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0001901C File Offset: 0x0001721C
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x0001902C File Offset: 0x0001722C
		[ShowInEditor]
		public int screenWidth
		{
			get
			{
				return this.CELL_screenWidth.data;
			}
			set
			{
				this.CELL_screenWidth.data = value;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0001903C File Offset: 0x0001723C
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x0001904C File Offset: 0x0001724C
		[ShowInEditor]
		public int screenHeight
		{
			get
			{
				return this.CELL_screenHeight.data;
			}
			set
			{
				this.CELL_screenHeight.data = value;
			}
		}

		// Token: 0x0400012E RID: 302
		public new static string TABLE_NAME = "Ting_Computers";

		// Token: 0x0400012F RID: 303
		private ValueEntry<string> CELL_programName;

		// Token: 0x04000130 RID: 304
		private ValueEntry<string> CELL_floppyBootProgramName;

		// Token: 0x04000131 RID: 305
		private ValueEntry<string[]> CELL_consoleOutput;

		// Token: 0x04000132 RID: 306
		private ValueEntry<int> CELL_nrOfLines;

		// Token: 0x04000133 RID: 307
		private ValueEntry<int> CELL_charsOnLine;

		// Token: 0x04000134 RID: 308
		private ValueEntry<int> CELL_currentLine;

		// Token: 0x04000135 RID: 309
		private ValueEntry<int> CELL_currentTopLine;

		// Token: 0x04000136 RID: 310
		private ValueEntry<string> CELL_currentInput;

		// Token: 0x04000137 RID: 311
		private ValueEntry<bool> CELL_hasInternetAPI;

		// Token: 0x04000138 RID: 312
		private ValueEntry<bool> CELL_hasGraphicsAPI;

		// Token: 0x04000139 RID: 313
		private ValueEntry<bool> CELL_hasWeatherAPI;

		// Token: 0x0400013A RID: 314
		private ValueEntry<bool> CELL_hasLampAPI;

		// Token: 0x0400013B RID: 315
		private ValueEntry<bool> CELL_hasDoorAPI;

		// Token: 0x0400013C RID: 316
		private ValueEntry<bool> CELL_hasMemoryAPI;

		// Token: 0x0400013D RID: 317
		private ValueEntry<bool> CELL_hasVoiceAPI;

		// Token: 0x0400013E RID: 318
		private ValueEntry<bool> CELL_hasElevatorAPI;

		// Token: 0x0400013F RID: 319
		private ValueEntry<bool> CELL_hasTingrunnerAPI;

		// Token: 0x04000140 RID: 320
		private ValueEntry<bool> CELL_hasTrapAPI;

		// Token: 0x04000141 RID: 321
		private ValueEntry<bool> CELL_hasHeartAPI;

		// Token: 0x04000142 RID: 322
		private ValueEntry<bool> CELL_hasArcadeMachineAPI;

		// Token: 0x04000143 RID: 323
		private ValueEntry<bool> CELL_hasFloppyAPI;

		// Token: 0x04000144 RID: 324
		private ValueEntry<int> CELL_mhz;

		// Token: 0x04000145 RID: 325
		private ValueEntry<string> CELL_memoryUnitName;

		// Token: 0x04000146 RID: 326
		private ValueEntry<float> CELL_maxExecutionTime;

		// Token: 0x04000147 RID: 327
		private ValueEntry<int> CELL_screenWidth;

		// Token: 0x04000148 RID: 328
		private ValueEntry<int> CELL_screenHeight;

		// Token: 0x04000149 RID: 329
		private ValueEntry<string> CELL_floppyInDrive;

		// Token: 0x0400014A RID: 330
		private Program _program;

		// Token: 0x0400014B RID: 331
		private Program _floppyBootProgram;

		// Token: 0x0400014C RID: 332
		private Character _user;

		// Token: 0x0400014D RID: 333
		public Computer.OnLineDrawing onLineDrawing;

		// Token: 0x0400014E RID: 334
		public Computer.OnLineDrawing onRectDrawing;

		// Token: 0x0400014F RID: 335
		public Action onDisplayGraphics;

		// Token: 0x04000150 RID: 336
		public Computer.IsKeyPressed isKeyPressed;

		// Token: 0x04000151 RID: 337
		public Computer.OnClearScreen onClearScreen;

		// Token: 0x04000152 RID: 338
		public Computer.OnSetColor onSetColor;

		// Token: 0x04000153 RID: 339
		public Computer.OnTextDrawing onTextDrawing;

		// Token: 0x02000049 RID: 73
		// (Invoke) Token: 0x06000519 RID: 1305
		public delegate void OnLineDrawing(IntPoint p1, IntPoint p2);

		// Token: 0x0200004A RID: 74
		// (Invoke) Token: 0x0600051D RID: 1309
		public delegate bool IsKeyPressed(string key);

		// Token: 0x0200004B RID: 75
		// (Invoke) Token: 0x06000521 RID: 1313
		public delegate void OnClearScreen();

		// Token: 0x0200004C RID: 76
		// (Invoke) Token: 0x06000525 RID: 1317
		public delegate void OnSetColor(float r, float g, float b);

		// Token: 0x0200004D RID: 77
		// (Invoke) Token: 0x06000529 RID: 1321
		public delegate void OnTextDrawing(int x, int y, string text);
	}
}
