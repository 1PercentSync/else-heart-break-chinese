using System;
using System.Text;
using GameTypes;
using GameWorld2;
using RelayLib;
using TingTing;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class ComputerShell : Shell
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x0600050A RID: 1290 RVA: 0x00024BC4 File Offset: 0x00022DC4
	public Computer computer
	{
		get
		{
			return base.ting as Computer;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x0600050B RID: 1291 RVA: 0x00024BD4 File Offset: 0x00022DD4
	public Transform computerScreen
	{
		get
		{
			return this._computerScreen;
		}
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00024BDC File Offset: 0x00022DDC
	public override void CreateTing()
	{
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00024BE0 File Offset: 0x00022DE0
	protected override void Setup()
	{
		base.Setup();
		this._renderer = base.GetComponentInChildren<TerminalRenderer>();
		if (this._renderer != null)
		{
			this._computerScreen = this._renderer.transform;
			this.leftMargin = 1;
			this.topMargin = 1;
			int num = (this._renderer.renderTextureWidth - 16) / 8;
			int num2 = (this._renderer.renderTextureHeight - 16) / 8;
			this.computer.charsOnLine = num;
			this.computer.nrOfLines = num2;
			this.lengthOfFullLine = num;
			if (this.computer.consoleOutput.Length < this.computer.nrOfLines)
			{
				this.computer.consoleOutput = new string[this.computer.nrOfLines];
				Debug.Log("Fixed consoleOutput.Length of " + base.name);
			}
			this._renderer.ClearScreen();
			this.computer.screenWidth = this._renderer.renderTextureWidth;
			this.computer.screenHeight = this._renderer.renderTextureHeight;
		}
		else
		{
			Debug.Log(base.name + " has got no TerminalRenderer");
		}
		SoundDictionary.LoadSingleSound("WooperOpen", "MinisteryComputerFoldOutSound 0");
		SoundDictionary.LoadSingleSound("Blip 1", "Blip 1");
		SoundDictionary.LoadSingleSound("Blip 2", "Blip 2");
		SoundDictionary.LoadSingleSound("Blip 3", "Blip 3");
		SoundDictionary.LoadSingleSound("Coin 1", "Coin 1");
		SoundDictionary.LoadSingleSound("Coin 2", "Coin 2");
		SoundDictionary.LoadSingleSound("Coin 3", "Coin 3");
		SoundDictionary.LoadSingleSound("Coin 4", "Coin 4");
		SoundDictionary.LoadSingleSound("Error", "Error");
		SoundDictionary.LoadSingleSound("Warning", "Warning");
		SoundDictionary.LoadSingleSound("Explosion 1", "Explosion 1");
		SoundDictionary.LoadSingleSound("Explosion 2", "Explosion 2");
		SoundDictionary.LoadSingleSound("Hit 1", "Hit 1");
		SoundDictionary.LoadSingleSound("Hit 2", "Hit 2");
		SoundDictionary.LoadSingleSound("Hit 3", "Hit 3");
		SoundDictionary.LoadSingleSound("Jump 1", "Jump 1");
		SoundDictionary.LoadSingleSound("Jump 2", "Jump 2");
		SoundDictionary.LoadSingleSound("Jump 3", "Jump 3");
		SoundDictionary.LoadSingleSound("Laser 1", "Laser 1");
		SoundDictionary.LoadSingleSound("Laser 2", "Laser 2");
		SoundDictionary.LoadSingleSound("Win", "Win");
		SoundDictionary.LoadSingleSound("Lose", "Lose");
		SoundDictionary.LoadSingleSound("Powerup 1", "Powerup 1");
		SoundDictionary.LoadSingleSound("Powerup 2", "Powerup 2");
		SoundDictionary.LoadSingleSound("Powerup 3", "Powerup 3");
		SoundDictionary.LoadSingleSound("Powerup 4", "Powerup 4");
		SoundDictionary.LoadSingleSound("Powerup 5", "Powerup 5");
		SoundDictionary.LoadSingleSound("Shoot 1", "Shoot 1");
		SoundDictionary.LoadSingleSound("Shoot 2", "Shoot 2");
		SoundDictionary.LoadSingleSound("Shoot 3", "Shoot 3");
		SoundDictionary.LoadSingleSound("Arcade music", "Arcade music");
		SoundDictionary.LoadSingleSound("Modem 1", "ModemConectionSound 1");
		SoundDictionary.LoadSingleSound("Modem 2", "ModemConectionSound 2");
		SoundDictionary.LoadSingleSound("Modem 3", "ModemConectionSound 3");
		SoundDictionary.LoadSingleSound("Startup", "ComputerStartUpSound 1");
		SoundDictionary.LoadSingleSound("Complete 1", "CodeTaskSuccessSound 1");
		SoundDictionary.LoadSingleSound("Complete 2", "CodeTaskSuccessSound 2");
		SoundDictionary.LoadSingleSound("Fail 1", "CodeTaskFailSound 2");
		SoundDictionary.LoadSingleSound("Fail 2", "CodeTaskFailSound 2");
		SoundDictionary.LoadSingleSound("Blackbird 1", "Blackbird short song sound 00");
		SoundDictionary.LoadSingleSound("Blackbird 2", "Blackbird short song sound 01");
		SoundDictionary.LoadSingleSound("Blackbird 3", "Blackbird short song sound 02");
		SoundDictionary.LoadSingleSound("Blackbird 4", "Blackbird short song sound 03");
		SoundDictionary.LoadSingleSound("Blackbird 5", "Blackbird short song sound 04");
		SoundDictionary.LoadSingleSound("Sine", "SINUS WAVE SHORT 03");
		SoundDictionary.LoadSingleSound("Square", "SQUARE WAVE");
		SoundDictionary.LoadSingleSound("FishAttack", "Fish game attack sound 1");
		SoundDictionary.LoadMultiSound("CasinoSpinn", "SlotMachineSpinning Sound", 3);
		SoundDictionary.LoadMultiSound("CasinoWin", "SlotMachineWin Sound", 3);
		SoundDictionary.LoadMultiSound("CasinoLost", "SlotMachineLost Sound", 3);
		SoundDictionary.LoadSingleSound("Thunder 1", "Thunder rumble sound 0");
		SoundDictionary.LoadSingleSound("Thunder 2", "Thunder rumble sound 1");
		SoundDictionary.LoadSingleSound("ComputerSound 1", "LargeComputerL3_TheLodge_ComputerTerminalSound 0");
		SoundDictionary.LoadSingleSound("ComputerSound 2", "LargeComputerMSound 0");
		SoundDictionary.LoadSingleSound("ComputerSound 3", "MinistryFacadeComputerSound 0");
		SoundDictionary.LoadSingleSound("ComputerSound 4", "MediumCompiterSound 0");
		SoundDictionary.LoadSingleSound("Electricity 1", "CurcuitLocker hum electricity sound");
		SoundDictionary.LoadSingleSound("Electricity 2", "CurcuitLocker hum electricity sound 1");
		SoundDictionary.LoadSingleSound("Atmosphere 1", "Atmosphere emotions 1 sound");
		SoundDictionary.LoadSingleSound("Atmosphere 2", "Internet atmosphere sound 2");
		SoundDictionary.LoadSingleSound("Guardian", "InternetATTACKfx Robot sound");
		this._animation = base.GetComponent<Animation>();
		this._blinkingLeds = base.GetComponent<LerumBlinker>();
		this.RefreshTextOnScreen();
		if (this.TheComputerIsBeingUsed())
		{
			this.PlayTurningOnAnimation();
		}
		this.RefreshSound();
		if (this._audioSource)
		{
			this._audioSource.pitch *= Randomizer.GetValue(0.7f, 1.3f);
		}
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x0002510C File Offset: 0x0002330C
	private bool TheComputerIsBeingUsed()
	{
		PointTileNode tile = this.computer.room.GetTile(this.computer.interactionPoints[0]);
		return tile != null && tile.HasOccupants();
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00025150 File Offset: 0x00023350
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		this.computer.AddDataListener<string[]>("consoleOutput", new ValueEntry<string[]>.DataChangeHandler(this.OnConsoleOutputChanged));
		Computer computer = this.computer;
		computer.onLineDrawing = (Computer.OnLineDrawing)Delegate.Combine(computer.onLineDrawing, new Computer.OnLineDrawing(this.OnLineDrawing));
		Computer computer2 = this.computer;
		computer2.onSetColor = (Computer.OnSetColor)Delegate.Combine(computer2.onSetColor, new Computer.OnSetColor(this.OnSetColor));
		Computer computer3 = this.computer;
		computer3.onRectDrawing = (Computer.OnLineDrawing)Delegate.Combine(computer3.onRectDrawing, new Computer.OnLineDrawing(this.OnQuadDrawing));
		Computer computer4 = this.computer;
		computer4.onTextDrawing = (Computer.OnTextDrawing)Delegate.Combine(computer4.onTextDrawing, new Computer.OnTextDrawing(this.OnTextDrawing));
		Computer computer5 = this.computer;
		computer5.onDisplayGraphics = (Action)Delegate.Combine(computer5.onDisplayGraphics, new Action(this.OnDisplayGraphics));
		Computer computer6 = this.computer;
		computer6.onClearScreen = (Computer.OnClearScreen)Delegate.Combine(computer6.onClearScreen, new Computer.OnClearScreen(this.OnClearScreen));
		this.computer.masterProgram.AddDataListener<bool>("isOn", new ValueEntry<bool>.DataChangeHandler(this.OnProgramStateChanged));
		Computer computer7 = this.computer;
		computer7.isKeyPressed = (Computer.IsKeyPressed)Delegate.Combine(computer7.isKeyPressed, new Computer.IsKeyPressed(this.IsKeyPressed));
		this.computer.AddDataListener<float>("pitch", new ValueEntry<float>.DataChangeHandler(this.OnPitchChanged));
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x000252D0 File Offset: 0x000234D0
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		this.computer.RemoveDataListener<string[]>("consoleOutput", new ValueEntry<string[]>.DataChangeHandler(this.OnConsoleOutputChanged));
		Computer computer = this.computer;
		computer.onLineDrawing = (Computer.OnLineDrawing)Delegate.Remove(computer.onLineDrawing, new Computer.OnLineDrawing(this.OnLineDrawing));
		Computer computer2 = this.computer;
		computer2.onSetColor = (Computer.OnSetColor)Delegate.Remove(computer2.onSetColor, new Computer.OnSetColor(this.OnSetColor));
		Computer computer3 = this.computer;
		computer3.onRectDrawing = (Computer.OnLineDrawing)Delegate.Remove(computer3.onRectDrawing, new Computer.OnLineDrawing(this.OnQuadDrawing));
		Computer computer4 = this.computer;
		computer4.onTextDrawing = (Computer.OnTextDrawing)Delegate.Remove(computer4.onTextDrawing, new Computer.OnTextDrawing(this.OnTextDrawing));
		Computer computer5 = this.computer;
		computer5.onDisplayGraphics = (Action)Delegate.Remove(computer5.onDisplayGraphics, new Action(this.OnDisplayGraphics));
		Computer computer6 = this.computer;
		computer6.onClearScreen = (Computer.OnClearScreen)Delegate.Remove(computer6.onClearScreen, new Computer.OnClearScreen(this.OnClearScreen));
		this.computer.masterProgram.RemoveDataListener<bool>("isOn", new ValueEntry<bool>.DataChangeHandler(this.OnProgramStateChanged));
		Computer computer7 = this.computer;
		computer7.isKeyPressed = (Computer.IsKeyPressed)Delegate.Remove(computer7.isKeyPressed, new Computer.IsKeyPressed(this.IsKeyPressed));
		this.computer.RemoveDataListener<float>("pitch", new ValueEntry<float>.DataChangeHandler(this.OnPitchChanged));
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x00025450 File Offset: 0x00023650
	private void OnPitchChanged(float pPrevPitch, float pNewPitch)
	{
		this.RefreshSound();
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00025458 File Offset: 0x00023658
	private void RefreshSound()
	{
		if (this._audioSource)
		{
			this._audioSource.pitch = this.computer.pitch;
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0002548C File Offset: 0x0002368C
	private void OnProgramStateChanged(bool pPrevious, bool pNewState)
	{
		if (pNewState)
		{
			this.PlayTurningOnAnimation();
			this.SetLedSpeed(10f);
		}
		else
		{
			this.SetLedSpeed(3f);
		}
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x000254B8 File Offset: 0x000236B8
	public void SetLedSpeed(float pSpeed)
	{
		if (this._blinkingLeds != null)
		{
			this._blinkingLeds.scrollSpeed = pSpeed;
		}
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x000254D8 File Offset: 0x000236D8
	public void PlayTurningOnAnimation()
	{
		if (!(this._animation == null))
		{
			if (!this.inFoldedOutMode)
			{
				this._animation.Play();
				if (base.name.Contains("WooperComputer") || base.name.Contains("Hugin") || base.name.Contains("Meteorology") || base.name.Contains("PowerTap") || base.name.Contains("Wellspringer"))
				{
					base.PlaySound("WooperOpen");
				}
				this.inFoldedOutMode = true;
			}
		}
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x00025594 File Offset: 0x00023794
	public void PlayTurningOffAnimation()
	{
		if (this._animation != null)
		{
			this.inFoldedOutMode = false;
		}
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x000255B0 File Offset: 0x000237B0
	private void OnConsoleOutputChanged(string[] pPreviousOutput, string[] pNewOutput)
	{
		this.RefreshTextOnScreen();
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x000255B8 File Offset: 0x000237B8
	private void OnLineDrawing(IntPoint p1, IntPoint p2)
	{
		if (this._computerScreen != null)
		{
			this._renderer.DrawGLLine(p1, p2, this.drawContextColor);
		}
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x000255EC File Offset: 0x000237EC
	private void OnQuadDrawing(IntPoint p1, IntPoint p2)
	{
		if (this._computerScreen != null)
		{
			this._renderer.DrawGLQuad(p1, p2, this.drawContextColor);
		}
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00025620 File Offset: 0x00023820
	private void OnSetColor(float r, float g, float b)
	{
		this.drawContextColor = new Float3(r, g, b);
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00025630 File Offset: 0x00023830
	private void OnTextDrawing(int x, int y, string text)
	{
		for (int i = 0; i < text.Length; i++)
		{
			int num = x + i + this.leftMargin;
			int num2 = y + this.topMargin;
			this._renderer.SetCharacter(num, num2, text[i], new Color(this.drawContextColor.x, this.drawContextColor.y, this.drawContextColor.z));
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x000256A4 File Offset: 0x000238A4
	private void OnDisplayGraphics()
	{
		if (this._renderer != null)
		{
			this._renderer.SwapBuffers();
			this._renderer.ApplyTextChanges();
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x000256D0 File Offset: 0x000238D0
	private void OnClearScreen()
	{
		if (this._renderer != null)
		{
			this._renderer.ClearScreen();
		}
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x000256F0 File Offset: 0x000238F0
	private bool IsKeyPressed(string pKey)
	{
		if (pKey == "left")
		{
			return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
		}
		if (pKey == "right")
		{
			return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
		}
		if (pKey == "up")
		{
			return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
		}
		if (pKey == "down")
		{
			return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
		}
		return pKey == "space" && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return));
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x000257D0 File Offset: 0x000239D0
	private void OnUserCreatedButtonPressed(TerminalWidget pWidget)
	{
		this.computer.API_Print((pWidget as TerminalButton).text);
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x000257E8 File Offset: 0x000239E8
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x000257EC File Offset: 0x000239EC
	private void RefreshTextOnScreen()
	{
		if (this._computerScreen == null)
		{
			return;
		}
		this._renderer.ClearInvertColors();
		this._renderer.UseForegroundColor();
		for (int i = 0; i < this.computer.nrOfLines; i++)
		{
			int num = (this.computer.currentTopLine + i) % this.computer.nrOfLines;
			string text = this.computer.consoleOutput[num];
			if (text != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num2 = this.lengthOfFullLine;
				foreach (char c in text)
				{
					stringBuilder.Append(c);
					num2--;
					if (num2 <= 0)
					{
						break;
					}
				}
				this.RenderTextLine(i, stringBuilder);
			}
		}
		int num3 = this.computer.currentLine - this.computer.currentTopLine;
		if (num3 < 0)
		{
			num3 += this.computer.nrOfLines;
		}
		num3 %= this.computer.nrOfLines;
		char c2 = ((!this.positionMarkerOn) ? ' ' : '_');
		this._renderer.SetCharacter(this.leftMargin + Mathf.Clamp(this.computer.currentInputXPos, 0, this.lengthOfFullLine - 1), this.topMargin + num3, c2);
		this._renderer.ApplyTextChanges();
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x00025960 File Offset: 0x00023B60
	private void RenderTextLine(int screenLine, StringBuilder sb)
	{
		for (int i = 0; i < sb.Length; i++)
		{
			this._renderer.SetCharacter(i + this.leftMargin, screenLine + this.topMargin, sb[i]);
		}
		for (int j = sb.Length; j < this.lengthOfFullLine; j++)
		{
			this._renderer.SetCharacter(j + this.leftMargin, screenLine + this.topMargin, ' ');
		}
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x000259E0 File Offset: 0x00023BE0
	private void OnSubNodeButtonPressed(int x)
	{
		Debug.Log("OnSubNodeButtonPressed with value: " + x);
		this.computer.masterProgram.StartAtFunction("OnButtonPressed", new object[] { x }, null);
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x00025A28 File Offset: 0x00023C28
	protected override void ShellUpdate()
	{
		base.ShellUpdate();
		if (this.computer.masterProgram.waitingForInput)
		{
			bool flag = Mathf.Sin(Time.time * 5f) > 0f;
			if (flag && this.positionMarkerOn)
			{
				this.positionMarkerOn = false;
				this.RefreshTextOnScreen();
			}
			else if (!flag && !this.positionMarkerOn)
			{
				this.positionMarkerOn = true;
				this.RefreshTextOnScreen();
			}
		}
	}

	// Token: 0x040003D1 RID: 977
	private Transform _computerScreen;

	// Token: 0x040003D2 RID: 978
	private TerminalRenderer _renderer;

	// Token: 0x040003D3 RID: 979
	private Animation _animation;

	// Token: 0x040003D4 RID: 980
	private Float3 drawContextColor = new Float3(1f, 1f, 1f);

	// Token: 0x040003D5 RID: 981
	private int leftMargin = 1;

	// Token: 0x040003D6 RID: 982
	private int topMargin = 1;

	// Token: 0x040003D7 RID: 983
	private int lengthOfFullLine = 55;

	// Token: 0x040003D8 RID: 984
	public bool hasKeyboard = true;

	// Token: 0x040003D9 RID: 985
	private bool positionMarkerOn;

	// Token: 0x040003DA RID: 986
	private bool inFoldedOutMode;

	// Token: 0x040003DB RID: 987
	private LerumBlinker _blinkingLeds;
}
