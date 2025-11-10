using System;
using System.Collections.Generic;
using System.Text;
using GameTypes;
using GameWorld2;
using TingTing;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class InsideComputerState : GameViewState
{
	// Token: 0x06000154 RID: 340 RVA: 0x00008BE0 File Offset: 0x00006DE0
	public InsideComputerState(string pNameOfStartTing, ActionMenu pActionMenu)
	{
		this._nameOfStartTing = pNameOfStartTing;
		this._actionMenu = pActionMenu;
		this._actionMenu.FadeOut();
		this._skin = Resources.Load("InsideComputerSkin") as GUISkin;
		this._lookatPositionPID.pFactor = 1f;
		SoundDictionary.LoadSingleSound("InternetTravelStopSound", "InternetTravelStopSound 0");
		SoundDictionary.LoadSingleSound("InternetTravellingSound", "InternetFlashingConnections Sound");
		if (Application.loadedLevelName != "Internet")
		{
			Application.LoadLevel("Internet");
		}
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00008D60 File Offset: 0x00006F60
	private void DelayedInstantation()
	{
		this.CreateNetNodes();
		this._delayedInstantiationHasTriggered = true;
		this._glowEffect = Camera.main.GetComponent<GlowEffect>();
		this._glowEffect.enabled = true;
		this._sentries = this.GetSentries();
		Shell shellWithName = ShellManager.GetShellWithName("DataVehicle");
		if (shellWithName == null)
		{
			Debug.Log("Failed to find Shell for data vehicle, will end state");
			base.EndState();
		}
		this._dataVehicle = shellWithName.gameObject;
		if (this._dataVehicle == null)
		{
			Debug.Log("Failed to find GameObject on Shell for data vehicle, will end state");
			base.EndState();
		}
		this._audio = this._dataVehicle.transform.Find("Audio").GetComponent<AudioSource>();
		if (this._audio == null)
		{
			Debug.Log("Failed to find audio transform with audio source.");
		}
		GameObject gameObject = GameObject.Find("MasterAudioListener");
		if (gameObject == null)
		{
			Debug.LogError("Can't find master audio listener InsideComputerState");
		}
		else
		{
			this._ear = gameObject.transform;
			this._audioListener = this._ear.GetComponent<AudioListener>();
			D.isNull(this._audioListener, "No audio listener on MasterAudioListener");
			this._audioListener.enabled = true;
		}
		this.MakeShellsAppearInRoom();
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00008E98 File Offset: 0x00007098
	private InternetSentry[] GetSentries()
	{
		return global::UnityEngine.Object.FindObjectsOfType<InternetSentry>();
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00008EA0 File Offset: 0x000070A0
	public override void OnEnterBegin()
	{
		MainMenu.SetVolume(1f);
		this.jackedInAvatar = base.controls.avatar as Character;
		if (this.jackedInAvatar == null)
		{
			Debug.LogError("Jacked in avatar is null");
		}
		Character character = this.jackedInAvatar;
		character.onNewAction = (Ting.OnNewAction)Delegate.Combine(character.onNewAction, new Ting.OnNewAction(this.OnAvatarChangesAction));
		base.controls.camera.ExitFixedCamera();
		base.controls.fade.FadeToTransparent();
		base.controls.world.settings.avatarName = "DataVehicle";
		this.MakeShellsAppearInRoom();
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00008F4C File Offset: 0x0000714C
	public override void OnExitBegin()
	{
		this._actionMenu.FadeIn();
		Character character = this.jackedInAvatar;
		character.onNewAction = (Ting.OnNewAction)Delegate.Remove(character.onNewAction, new Ting.OnNewAction(this.OnAvatarChangesAction));
		this.jackedInAvatar.StopAction();
		this._glowEffect.enabled = false;
		if (this.jackedInAvatar.room.name != base.controls.roomChanger.currentRoom)
		{
			Debug.Log("Jacked in avatar is in room " + this.jackedInAvatar.room.name + ", will load that");
			base.controls.roomChanger.LoadRoom(this.jackedInAvatar.room.name);
		}
		else
		{
			Debug.Log("Jacked in avatar is in current room " + this.jackedInAvatar.room.name + ", will not load any room");
		}
		if (this.jackedInAvatar.room.name == "Internet")
		{
			this.MakeShellsAppearInRoom();
		}
		CharacterShell characterShell = ShellManager.GetShellWithName(this.jackedInAvatar.name) as CharacterShell;
		if (characterShell != null)
		{
			characterShell.ResetLookTargetPoint();
		}
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00009088 File Offset: 0x00007288
	private void OnAvatarChangesAction(string pPreviousAction, string pNewAction)
	{
		if (pPreviousAction == "InsideComputer" && pNewAction != "InsideComputer")
		{
			base.EndState();
		}
	}

	// Token: 0x0600015B RID: 347 RVA: 0x000090BC File Offset: 0x000072BC
	private void MakeShellsAppearInRoom()
	{
		base.controls.roomChanger.onRoomHasChanged("Internet");
	}

	// Token: 0x0600015C RID: 348 RVA: 0x000090D8 File Offset: 0x000072D8
	private void CreateNetNodes()
	{
		foreach (Ting ting in base.controls.world.tingRunner.GetTings())
		{
			if (ting is Computer || ting is Character || ting is FuseBox)
			{
				NetNode netNode = this.CreateNetNode(ting);
				if (ting.name == this._nameOfStartTing)
				{
					this._currentTargetedNetNode = netNode;
				}
			}
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x0600015D RID: 349 RVA: 0x0000918C File Offset: 0x0000738C
	private static GameObject netNodePrefab
	{
		get
		{
			if (InsideComputerState.s_netNodePrefab == null)
			{
				InsideComputerState.s_netNodePrefab = Resources.Load("NetNodePrefab") as GameObject;
			}
			return InsideComputerState.s_netNodePrefab;
		}
	}

	// Token: 0x0600015E RID: 350 RVA: 0x000091B8 File Offset: 0x000073B8
	private NetNode CreateNetNode(Ting pTing)
	{
		GameObject gameObject = GameObject.Find("NetNode_" + pTing.name);
		if (gameObject != null)
		{
			NetNode component = gameObject.GetComponent<NetNode>();
			Debug.Log(string.Concat(new object[] { "There's already a net node for ting ", pTing.name, ": ", component }));
			component.netNodes = this._netNodes;
			component.ting = pTing as MimanTing;
			this._netNodes.Add(component);
			return component;
		}
		GameObject gameObject2 = global::UnityEngine.Object.Instantiate(InsideComputerState.netNodePrefab, new Vector3(global::UnityEngine.Random.Range(-500f, 500f), global::UnityEngine.Random.Range(-500f, 500f), global::UnityEngine.Random.Range(-500f, 500f)), Quaternion.identity) as GameObject;
		NetNode component2 = gameObject2.GetComponent<NetNode>();
		component2.netNodes = this._netNodes;
		component2.ting = pTing as MimanTing;
		this._netNodes.Add(component2);
		return component2;
	}

	// Token: 0x0600015F RID: 351 RVA: 0x000092B4 File Offset: 0x000074B4
	public override void OnGUI()
	{
		GUI.color = Color.white;
		GUI.skin = this._skin;
		NetNode netNodeForTing = this.GetNetNodeForTing(this.jackedInAvatar.actionOtherObject as MimanTing);
		if (netNodeForTing != null)
		{
			GUI.Label(new Rect(40f, (float)(Screen.height - 500), 800f, 500f), this.NavigationText());
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00009324 File Offset: 0x00007524
	private string NavigationText()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (this._caught)
		{
			GUI.color = Color.red;
		}
		stringBuilder.Append(this.TransmissionText());
		stringBuilder.Append("\n====================\n\n");
		if (this._inTransmission)
		{
			if (this._caught)
			{
				for (int i = 0; i < 4 + (int)(Mathf.Abs(Mathf.Sin(Time.time * 2f)) * 5f); i++)
				{
					for (int j = 0; j < global::UnityEngine.Random.Range(5, 10); j++)
					{
						stringBuilder.Append(Randomizer.RandNth<string>(InsideComputerState.dataText));
					}
					stringBuilder.Append("\n");
				}
			}
			else
			{
				for (int k = 0; k < global::UnityEngine.Random.Range(2, 5); k++)
				{
					string name = Randomizer.RandNth<NetNode>(this._netNodes).ting.name;
					float num = global::UnityEngine.Random.Range(0f, 1f);
					if (num < 0.3f)
					{
						stringBuilder.Append("[");
						stringBuilder.Append(name.ToLower());
						stringBuilder.Append("]");
					}
					else if (num < 0.6f)
					{
						stringBuilder.Append(name.ToUpper());
					}
					else
					{
						stringBuilder.Append(name);
					}
					stringBuilder.Append("\n");
				}
			}
		}
		else
		{
			stringBuilder.Append("Available connections:\n");
			int num2 = 1;
			foreach (MimanTing mimanTing in (this.jackedInAvatar.actionOtherObject as MimanTing).connectedTings)
			{
				if (mimanTing is Character)
				{
					stringBuilder.Append("\n" + num2 + ": INVALID TARGET");
				}
				else
				{
					stringBuilder.Append(string.Concat(new object[] { "\n", num2, ": move to ", mimanTing.name }));
				}
				num2++;
			}
			stringBuilder.Append("\n\nq: exit");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00009558 File Offset: 0x00007758
	private string TransmissionText()
	{
		if (this._caught)
		{
			return "ATTACKED";
		}
		if (this._inTransmission)
		{
			return string.Concat(new object[]
			{
				"[LOADING ",
				this._targetTing.name,
				" ",
				(int)(this._transmissionPercentage * 100f),
				"%]"
			});
		}
		return "[" + this.jackedInAvatar.actionOtherObject.name + "]";
	}

	// Token: 0x06000162 RID: 354 RVA: 0x000095E8 File Offset: 0x000077E8
	public override void OnUpdate()
	{
		if (!this._delayedInstantiationHasTriggered)
		{
			this.DelayedInstantation();
		}
		PlayerRoamingState.CycleGlowEffectColor(this._glowEffect);
		if (this._dataVehicle != null)
		{
			this.MoveDataVehicle();
			this.SentriesDetectDataVehicle();
		}
		else
		{
			Debug.Log("No data vehicle in internet state");
		}
		PlayerRoamingState.ControlCamera(base.controls.camera, true, 0f);
		if (this.currentNode != null && !this._inTransmission && Input.GetKeyDown(KeyCode.Q))
		{
			this.JackOut();
		}
		this.CheckNumericInput();
		if (this._inTransmission)
		{
			float num = Vector3.Distance(this.currentNode.transform.position, this.targetNode.transform.position);
			float num2 = 70f;
			float num3 = num / num2;
			float num4 = Time.deltaTime * (1f / num3);
			if (this._caught)
			{
				this._transmissionPercentage -= num4 * 0.5f;
				if (this._transmissionPercentage <= 0f)
				{
					this._targetTing = null;
					this._inTransmission = false;
					this.SentriesStopAttackingYou();
				}
			}
			else
			{
				this._transmissionPercentage += num4;
				if (this._transmissionPercentage >= 1f)
				{
					this.jackedInAvatar.actionOtherObject = this._targetTing;
					this._targetTing = null;
					this._inTransmission = false;
					this._audio.Stop();
				}
			}
		}
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00009760 File Offset: 0x00007960
	private void SentriesDetectDataVehicle()
	{
		if (!this._inTransmission)
		{
			return;
		}
		if (this.targetNode == null)
		{
			return;
		}
		string name = this.targetNode.transform.name;
		foreach (InternetSentry internetSentry in this._sentries)
		{
			if ((name == internetSentry.guardTargetName || name == "NetNode_" + internetSentry.guardTargetName) && this._transmissionPercentage > 0.1f && (Vector3.Distance(this.targetNode.transform.position, this._dataVehicle.transform.position) < 70f || this._transmissionPercentage > 0.8f))
			{
				Debug.Log("Caught by sentry " + internetSentry.name + "! It's guard target = " + ((!(internetSentry.guardTarget == null)) ? internetSentry.guardTarget.name : "null"));
				internetSentry.SetAttackTarget(this._dataVehicle.transform);
				this._caught = true;
				this._audio.loop = false;
				SoundDictionary.PlaySound("InternetTravelStopSound", this._audio);
			}
		}
	}

	// Token: 0x06000164 RID: 356 RVA: 0x000098AC File Offset: 0x00007AAC
	private void SentriesStopAttackingYou()
	{
		this._caught = false;
		foreach (InternetSentry internetSentry in this._sentries)
		{
			internetSentry.SetNoAttackTarget();
		}
	}

	// Token: 0x06000165 RID: 357 RVA: 0x000098E8 File Offset: 0x00007AE8
	public override void OnLatestUpdate()
	{
		if (this._dataVehicle != null)
		{
			base.controls.depthOfField.focalTransform = this._dataVehicle.transform;
			this._ear.transform.position = this._dataVehicle.transform.position;
			this._ear.transform.eulerAngles = Camera.main.transform.eulerAngles;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000166 RID: 358 RVA: 0x00009960 File Offset: 0x00007B60
	private NetNode currentNode
	{
		get
		{
			return this.GetNetNodeForTing(this.jackedInAvatar.actionOtherObject as MimanTing);
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000167 RID: 359 RVA: 0x00009978 File Offset: 0x00007B78
	private NetNode targetNode
	{
		get
		{
			return this.GetNetNodeForTing(this._targetTing);
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00009988 File Offset: 0x00007B88
	private void MoveDataVehicle()
	{
		if (this._inTransmission)
		{
			this._dataVehicle.transform.position = Vector3.Lerp(this.currentNode.transform.position, this.targetNode.transform.position, this._transmissionPercentage);
		}
		else
		{
			this._dataVehicle.transform.position = this.currentNode.transform.position;
		}
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00009A00 File Offset: 0x00007C00
	public void StartTransmission(int pConnectionNr)
	{
		if (this._inTransmission)
		{
			Debug.Log("Already in transmission");
			return;
		}
		this._targetTing = this.GetConnection(pConnectionNr);
		if (this._targetTing == null)
		{
			return;
		}
		NetNode netNodeForTing = this.GetNetNodeForTing(this._targetTing);
		if (netNodeForTing == null)
		{
			NetNode netNode = this.CreateNetNode(this._targetTing);
			if (netNode == null)
			{
				Debug.Log("Failed to create new net node on the fly");
				return;
			}
		}
		if (this._targetTing is Character)
		{
			Debug.Log("Can't transfer to human");
			return;
		}
		if (this._targetTing != null)
		{
			this._inTransmission = true;
			this._caught = false;
			this._transmissionPercentage = 0f;
			SoundDictionary.PlaySound("InternetTravellingSound", this._audio);
			this._audio.loop = true;
		}
		else
		{
			Debug.Log("Failed to start transmission, couldn't get connection at nr " + pConnectionNr);
		}
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00009AF0 File Offset: 0x00007CF0
	public MimanTing GetConnection(int pConnectionNr)
	{
		MimanTing mimanTing = this.jackedInAvatar.actionOtherObject as MimanTing;
		if (mimanTing != null)
		{
			if (mimanTing.connectedTings.Length > 0 && pConnectionNr < mimanTing.connectedTings.Length)
			{
				return mimanTing.connectedTings[pConnectionNr];
			}
			Debug.Log("Can't get a connection at channel " + pConnectionNr);
		}
		else
		{
			Debug.Log("Not connected to a computer");
		}
		return null;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00009B60 File Offset: 0x00007D60
	private void JackOut()
	{
		MimanTing mimanTing = this.jackedInAvatar.actionOtherObject as MimanTing;
		WorldCoordinate position = mimanTing.position;
		PointTileNode pointTileNode = null;
		foreach (IntPoint intPoint in mimanTing.interactionPoints)
		{
			PointTileNode tile = mimanTing.room.GetTile(intPoint);
			if (tile != null && tile.links.Count > 0)
			{
				pointTileNode = tile;
				break;
			}
		}
		if (pointTileNode == null)
		{
			base.controls.world.settings.onNotification(base.controls.world.settings.avatarName, "Reality blocked, can't jack out!");
			mimanTing.AddConnectionToTing(base.controls.world.tingRunner.GetTing("Plaza_ParkTrashCan_1"));
		}
		else
		{
			base.controls.world.settings.avatarName = this.jackedInAvatar.name;
			WorldCoordinate worldCoordinate = new WorldCoordinate(position.roomName, pointTileNode.position.localPosition);
			bool flag = pointTileNode.GetOccupantOfType<Seat>() != null;
			this.jackedInAvatar.position = worldCoordinate;
			this.jackedInAvatar.sitting = flag;
			this.jackedInAvatar.StopAction();
		}
	}

	// Token: 0x0600016C RID: 364 RVA: 0x00009CB8 File Offset: 0x00007EB8
	private NetNode GetNetNodeForTing(MimanTing pTing)
	{
		foreach (NetNode netNode in this._netNodes)
		{
			if (netNode.ting == pTing)
			{
				return netNode;
			}
		}
		return null;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00009D30 File Offset: 0x00007F30
	private void CheckNumericInput()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.inputString.Contains("1"))
		{
			this.StartTransmission(0);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.inputString.Contains("2"))
		{
			this.StartTransmission(1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.inputString.Contains("3"))
		{
			this.StartTransmission(2);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.inputString.Contains("4"))
		{
			this.StartTransmission(3);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.inputString.Contains("5"))
		{
			this.StartTransmission(4);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.inputString.Contains("6"))
		{
			this.StartTransmission(5);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.inputString.Contains("7"))
		{
			this.StartTransmission(6);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.inputString.Contains("8"))
		{
			this.StartTransmission(7);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.inputString.Contains("9"))
		{
			this.StartTransmission(8);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			this.StartTransmission(9);
		}
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00009EE0 File Offset: 0x000080E0
	public override void OnRenderObject()
	{
		InsideComputerState.CreateLineMaterial();
		GL.PushMatrix();
		InsideComputerState.lineMaterial.SetPass(0);
		GL.Begin(1);
		if (this._currentTargetedNetNode != null)
		{
			GL.Color(new Color(1f, 1f, 1f, 1f));
			GL.Vertex(base.controls.camera.transform.position);
			GL.Vertex(this._currentTargetedNetNode.transform.position);
			GL.Vertex(base.controls.camera.transform.position);
			GL.Vertex(this._currentTargetedNetNode.transform.position);
		}
		foreach (NetNode netNode in this._netNodes)
		{
			if (!netNode.disposed)
			{
				netNode.DrawConnections();
			}
		}
		GL.End();
		GL.PopMatrix();
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000A004 File Offset: 0x00008204
	private static void CreateLineMaterial()
	{
		if (!InsideComputerState.lineMaterial)
		{
			InsideComputerState.lineMaterial = new Material("Shader \"Lines/Colored Blended\" {SubShader { Pass {     Blend SrcAlpha OneMinusSrcAlpha     ZWrite Off Cull Off Fog { Mode Off }     BindChannels {      Bind \"vertex\", vertex Bind \"color\", color }} } }");
			InsideComputerState.lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			InsideComputerState.lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	// Token: 0x040000D1 RID: 209
	private bool _delayedInstantiationHasTriggered;

	// Token: 0x040000D2 RID: 210
	private GlowEffect _glowEffect;

	// Token: 0x040000D3 RID: 211
	private GUISkin _skin;

	// Token: 0x040000D4 RID: 212
	private ActionMenu _actionMenu;

	// Token: 0x040000D5 RID: 213
	private List<NetNode> _netNodes = new List<NetNode>();

	// Token: 0x040000D6 RID: 214
	private NetNode _currentTargetedNetNode;

	// Token: 0x040000D7 RID: 215
	private InternetSentry[] _sentries = new InternetSentry[0];

	// Token: 0x040000D8 RID: 216
	private Vector3 _lookatPosition;

	// Token: 0x040000D9 RID: 217
	private PidVector3 _lookatPositionPID = new PidVector3();

	// Token: 0x040000DA RID: 218
	private string _nameOfStartTing;

	// Token: 0x040000DB RID: 219
	private MimanTing _targetTing;

	// Token: 0x040000DC RID: 220
	private bool _inTransmission;

	// Token: 0x040000DD RID: 221
	private float _transmissionPercentage;

	// Token: 0x040000DE RID: 222
	private GameObject _dataVehicle;

	// Token: 0x040000DF RID: 223
	private bool _caught;

	// Token: 0x040000E0 RID: 224
	private Character jackedInAvatar;

	// Token: 0x040000E1 RID: 225
	private Transform _ear;

	// Token: 0x040000E2 RID: 226
	private AudioListener _audioListener;

	// Token: 0x040000E3 RID: 227
	private AudioSource _audio;

	// Token: 0x040000E4 RID: 228
	private static GameObject s_netNodePrefab;

	// Token: 0x040000E5 RID: 229
	private static string[] dataText = new string[]
	{
		"#", "*", "@", "overflow", "corrupt", "memory", "0", "00", "000", "INVALID",
		"void", "null", "pointer", "EXCEPTION", "buffer", "ptr", "0x01", "x", "X", ";",
		"break"
	};

	// Token: 0x040000E6 RID: 230
	private static Material lineMaterial;
}
