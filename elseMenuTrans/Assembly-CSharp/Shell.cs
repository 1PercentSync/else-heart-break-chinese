using System;
using System.Collections.Generic;
using GameTypes;
using GameWorld2;
using GrimmLib;
using RelayLib;
using TingTing;
using UnityEngine;

// Token: 0x020000A0 RID: 160
public abstract class Shell : MonoBehaviour
{
	// Token: 0x17000069 RID: 105
	// (get) Token: 0x0600047A RID: 1146 RVA: 0x0001F004 File Offset: 0x0001D204
	// (set) Token: 0x06000479 RID: 1145 RVA: 0x0001EFF8 File Offset: 0x0001D1F8
	public MimanTing ting { get; protected set; }

	// Token: 0x0600047B RID: 1147 RVA: 0x0001F00C File Offset: 0x0001D20C
	private void Start()
	{
		this.ting = WorldOwner.instance.world.tingRunner.GetTingUnsafe(base.name) as MimanTing;
		if (this.ting != null)
		{
			if (this.ting.room.name != Application.loadedLevelName)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				this.Setup();
			}
			this._tooltipTexture1 = Resources.Load("ToolTipColor2_NOSCALE") as Texture;
			this._tooltipTexture2 = Resources.Load("ToolTipColor1_NOSCALE") as Texture;
			this._lmb = Resources.Load("RMouseIcon_NOSCALE") as Texture;
			this._rmb = Resources.Load("LMouseIcon_NOSCALE") as Texture;
			this._skin = Resources.Load("TooltipSkin") as GUISkin;
			this._moduluTickToCheckForSmoke = Randomizer.GetIntValue(1, 30);
		}
		else
		{
			Debug.Log("Ting is null for shell " + base.name + " it must have been deleted from the world? Remove shell too then.");
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0001F120 File Offset: 0x0001D320
	private void OnDestroy()
	{
		if (!WorldOwner.instance.worldIsLoaded)
		{
			return;
		}
		if (this.hasSetupTingRef && !this.ting.isDeleted)
		{
			this.RemoveDataListeners();
		}
		this.RemoveBubbles();
		this.ting = null;
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0001F16C File Offset: 0x0001D36C
	private void RemoveBubbles()
	{
		if (this._bubble != null)
		{
			this._bubble.DestroyIn(0f);
			this._bubble = null;
		}
		if (this._decliningBubble != null)
		{
			this._decliningBubble.DestroyIn(0f);
			this._decliningBubble = null;
		}
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0001F1CC File Offset: 0x0001D3CC
	private void OnDrawGizmos()
	{
		this.ShellDrawGizmos();
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0001F1D4 File Offset: 0x0001D3D4
	protected virtual void ShellDrawGizmos()
	{
		if (this.hasSetupTingRef)
		{
			Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
			bool flag = true;
			foreach (IntPoint intPoint in this.ting.interactionPoints)
			{
				Gizmos.DrawCube(MimanHelper.TilePositionToVector3(intPoint), new Vector3(0.4f, 0.4f, 0.4f));
				PointTileNode tile = this.ting.room.GetTile(intPoint);
				if (tile != null && !tile.HasOccupants(this.ting))
				{
					flag = false;
				}
			}
			float num = 0.4f;
			Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			if (this.ting is Computer)
			{
				PointTileNode tile2 = this.ting.room.GetTile(this.ting.interactionPoints[0]);
				if (tile2 != null && tile2.HasOccupants<Seat>(null))
				{
					Gizmos.color = new Color(1f, 0f, 1f, 0.5f);
				}
			}
			else if (flag)
			{
				num = 50f;
				Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
			}
			Gizmos.DrawCube(MimanHelper.TilePositionToVector3(this.ting.localPoint), new Vector3(1f, num, 1f));
		}
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0001F36C File Offset: 0x0001D56C
	protected virtual void Setup()
	{
		this.SetupDataListeners();
		if (this.ShouldSnapPosAndDir())
		{
			this.SnapShellToTingPosition();
			this.SnapShellToTingDirection();
		}
		this._animationComponent = base.GetComponentInChildren<Animation>();
		this._audioSource = base.GetComponent<AudioSource>();
		if (this.ting.containsBrokenPrograms && base.name != "Heart")
		{
			this.OnEmitsSmokeChanged(false, true);
			ParticleSystem[] componentsInChildren = this._compilerSmoke.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in componentsInChildren)
			{
				for (int j = 0; j < 30; j++)
				{
					particleSystem.Simulate(0.1f);
				}
				particleSystem.Play();
			}
		}
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x0001F42C File Offset: 0x0001D62C
	protected virtual void SetupDataListeners()
	{
		this.ting.AddDataListener<WorldCoordinate>("position", new ValueEntry<WorldCoordinate>.DataChangeHandler(this.OnPositionChanged));
		this.ting.AddDataListener<Direction>("direction", new ValueEntry<Direction>.DataChangeHandler(this.OnDirectionChanged));
		this.ting.AddDataListener<bool>("isBeingHeld", new ValueEntry<bool>.DataChangeHandler(this.OnIsBeingHeldChanged));
		this.ting.AddDataListener<string>("dialogueLine", new ValueEntry<string>.DataChangeHandler(this.OnDialogueLineChange));
		this.ting.AddDataListener<bool>("emitsSmoke", new ValueEntry<bool>.DataChangeHandler(this.OnEmitsSmokeChanged));
		this.ting.AddDataListener<bool>("isPlaying", new ValueEntry<bool>.DataChangeHandler(this.OnIsPlayingChanged));
		this.ting.AddDataListener<string>("soundName", new ValueEntry<string>.DataChangeHandler(this.OnSoundNameChanged));
		MimanTing ting = this.ting;
		ting.onNewAction = (Ting.OnNewAction)Delegate.Combine(ting.onNewAction, new Ting.OnNewAction(this.OnActionChanged));
		MimanTing ting2 = this.ting;
		ting2.onPlaySound = (MimanTing.OnPlaySound)Delegate.Combine(ting2.onPlaySound, new MimanTing.OnPlaySound(this.PlaySound));
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0001F54C File Offset: 0x0001D74C
	protected virtual void RemoveDataListeners()
	{
		this.ting.RemoveDataListener<WorldCoordinate>("position", new ValueEntry<WorldCoordinate>.DataChangeHandler(this.OnPositionChanged));
		this.ting.RemoveDataListener<Direction>("direction", new ValueEntry<Direction>.DataChangeHandler(this.OnDirectionChanged));
		this.ting.RemoveDataListener<bool>("isBeingHeld", new ValueEntry<bool>.DataChangeHandler(this.OnIsBeingHeldChanged));
		this.ting.RemoveDataListener<string>("dialogueLine", new ValueEntry<string>.DataChangeHandler(this.OnDialogueLineChange));
		this.ting.RemoveDataListener<bool>("emitsSmoke", new ValueEntry<bool>.DataChangeHandler(this.OnEmitsSmokeChanged));
		this.ting.RemoveDataListener<bool>("isPlaying", new ValueEntry<bool>.DataChangeHandler(this.OnIsPlayingChanged));
		this.ting.RemoveDataListener<string>("soundName", new ValueEntry<string>.DataChangeHandler(this.OnSoundNameChanged));
		MimanTing ting = this.ting;
		ting.onNewAction = (Ting.OnNewAction)Delegate.Remove(ting.onNewAction, new Ting.OnNewAction(this.OnActionChanged));
		MimanTing ting2 = this.ting;
		ting2.onPlaySound = (MimanTing.OnPlaySound)Delegate.Remove(ting2.onPlaySound, new MimanTing.OnPlaySound(this.PlaySound));
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0001F66C File Offset: 0x0001D86C
	private void OnIsPlayingChanged(bool pPrev, bool pNew)
	{
		this.PlaySoundFromRightPosition();
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0001F674 File Offset: 0x0001D874
	protected void PlaySoundFromRightPosition()
	{
		if (this.ting.isPlaying)
		{
			this.PlaySound(this.ting.soundName);
			base.audio.time = this.ting.audioTime;
		}
		else
		{
			this.StopSound();
		}
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x0001F6C4 File Offset: 0x0001D8C4
	private void OnSoundNameChanged(string pPrevName, string pNewName)
	{
		this.PlaySoundFromRightPosition();
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x0001F6CC File Offset: 0x0001D8CC
	protected virtual bool ShouldSnapPosAndDir()
	{
		return !this.ting.isBeingHeld;
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0001F6DC File Offset: 0x0001D8DC
	protected virtual void ShellUpdate()
	{
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0001F6E0 File Offset: 0x0001D8E0
	private void OnEmitsSmokeChanged(bool pPreviousValue, bool pNewValue)
	{
		if (base.name == "Heart")
		{
			return;
		}
		if (pNewValue)
		{
			if (this._compilerSmoke == null)
			{
				global::UnityEngine.Object @object = Resources.Load("CompilerSmoke");
				D.isNull(@object, "Smoke prefab is null");
				this._compilerSmoke = (global::UnityEngine.Object.Instantiate(@object, base.transform.position, base.transform.rotation) as GameObject).transform;
				this._compilerSmoke.parent = base.transform;
			}
		}
		else if (this._compilerSmoke != null)
		{
			global::UnityEngine.Object.Destroy(this._compilerSmoke.gameObject);
		}
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0001F794 File Offset: 0x0001D994
	private void OnActionChanged(string pPreviousAction, string pNewAction)
	{
		if (pNewAction != pPreviousAction)
		{
			this.SelectAnimationAndSoundToPlay();
		}
		if (pNewAction == "Explode")
		{
			RunGameWorld.instance.gameViewControls.camera.Shake(2f, 4f, true);
			GameObject gameObject = Resources.Load("EXPLOSION_FX") as GameObject;
			global::UnityEngine.Object.Instantiate(gameObject, base.transform.position + new Vector3(0f, 3f, 0f), base.transform.rotation);
		}
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x0001F828 File Offset: 0x0001DA28
	protected virtual void SelectAnimationAndSoundToPlay()
	{
		if (this._animationComponent == null)
		{
			return;
		}
		string text = ((!(this.ting.actionName == string.Empty)) ? this.ting.actionName : "Idle");
		string animationNameFromActionName = this.GetAnimationNameFromActionName(text);
		if (animationNameFromActionName == string.Empty)
		{
			this._animationComponent.Stop();
		}
		else
		{
			this.SnapShellToTingDirection();
			if (this._animationComponent.GetClip(animationNameFromActionName))
			{
				this._animationComponent.Play(animationNameFromActionName);
			}
			else
			{
				D.Log("Can't play animation " + animationNameFromActionName + " in " + base.name);
			}
			this.PlaySound(animationNameFromActionName);
		}
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
	protected string GetAnimationNameFromActionName(string pActionName)
	{
		if (this._animationsForActionNames.ContainsKey(pActionName))
		{
			string[] array = this._animationsForActionNames[pActionName];
			D.assert(array.Length > 0, "Empty array of animation names");
			int num = global::UnityEngine.Random.Range(0, array.Length);
			return array[num];
		}
		return string.Empty;
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x0001F940 File Offset: 0x0001DB40
	private void OnPositionChanged(WorldCoordinate pPreviousPosition, WorldCoordinate pNewPosition)
	{
		if (this.ShouldSnapPosAndDir())
		{
			this.SnapShellToTingPosition();
		}
		if (pNewPosition.roomName != Application.loadedLevelName)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x0001F980 File Offset: 0x0001DB80
	protected void SnapXZToTing()
	{
		base.transform.position = MimanHelper.TilePositionToVector3(this.ting.localPoint);
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x0001F9A8 File Offset: 0x0001DBA8
	public virtual void SnapShellToTingPosition()
	{
		this.SnapXZToTing();
		base.transform.position += new Vector3(0f, Shell.GetSurfaceHeight(base.transform.position), 0f);
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x0001F9F0 File Offset: 0x0001DBF0
	public void SnapTingToShellPosition()
	{
		this.ting.position = new WorldCoordinate(this.ting.room.name, MimanHelper.Vector3ToTilePoint(base.transform.position));
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x0001FA30 File Offset: 0x0001DC30
	public static float GetSurfaceHeight(Vector3 pPosition)
	{
		Vector3 vector = pPosition + Vector3.up * 500f;
		Ray ray = new Ray(vector, Vector3.down);
		RaycastHit[] array = Physics.RaycastAll(ray);
		float num = float.MinValue;
		bool flag = false;
		if (array != null)
		{
			foreach (RaycastHit raycastHit in array)
			{
				if (raycastHit.point.y > num && raycastHit.transform.tag != "Seat" && (raycastHit.transform.tag == "TableSurface" || raycastHit.transform.tag == "PhysicsFloor" || raycastHit.transform.tag == "Carpet"))
				{
					num = raycastHit.point.y;
					flag = true;
				}
			}
		}
		if (flag)
		{
			return num;
		}
		return 0f;
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x0001FB48 File Offset: 0x0001DD48
	private void OnDirectionChanged(Direction pPreviousDirection, Direction pNewDirection)
	{
		if (this.ShouldSnapPosAndDir())
		{
			this.SnapShellToTingDirection();
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x0001FB5C File Offset: 0x0001DD5C
	public void SnapShellToTingDirection()
	{
		IntPoint intPoint = IntPoint.DirectionToIntPoint(this.ting.direction);
		base.transform.eulerAngles = new Vector3(0f, intPoint.Degrees(), 0f);
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x0001FB9C File Offset: 0x0001DD9C
	private void OnIsBeingHeldChanged(bool pPreviousValue, bool pNewValue)
	{
		if (!pNewValue)
		{
			this.SnapShellToTingPosition();
			this.SnapShellToTingDirection();
		}
	}

	// Token: 0x06000494 RID: 1172
	public abstract void CreateTing();

	// Token: 0x06000495 RID: 1173 RVA: 0x0001FBB0 File Offset: 0x0001DDB0
	private void Update()
	{
		if (Mathf.Approximately(Time.deltaTime, 0f))
		{
			return;
		}
		if (this.hasSetupTingRef)
		{
			this.ShellUpdate();
			this.DetectIfDeleted();
			this.UpdateTooltip();
			this.RefreshSmoke();
		}
		if (this._compilerSmoke != null)
		{
			this._compilerSmoke.rotation = Quaternion.identity;
		}
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x0001FC18 File Offset: 0x0001DE18
	protected virtual void RefreshSmoke()
	{
		if (Time.frameCount % this._moduluTickToCheckForSmoke == 0)
		{
			this.ting.emitsSmoke = this.ting.containsBrokenPrograms;
		}
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0001FC4C File Offset: 0x0001DE4C
	protected virtual void UpdateTooltip()
	{
		if (this._showTooltip)
		{
			this._tooltipAlpha += Time.deltaTime * 5f;
			if (this._tooltipAlpha > 1f)
			{
				this._tooltipAlpha = 1f;
			}
			this._showTooltip = false;
		}
		else
		{
			this._tooltipAlpha -= Time.deltaTime * 5f;
			if (this._tooltipAlpha < 0f)
			{
				this._tooltipAlpha = 0f;
			}
		}
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x0001FCD8 File Offset: 0x0001DED8
	private void DetectIfDeleted()
	{
		if (this.ting.isDeleted)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x0001FCF8 File Offset: 0x0001DEF8
	public void ConnectTing(Ting pTing)
	{
		if (ShellManager.TestForAnotherShellWithSameName(this))
		{
			Debug.LogError("There is another Shell with the name '" + base.name + "' in the room, can't setup Ting ref");
			return;
		}
		this.ting = WorldOwner.instance.world.tingRunner.GetTingUnsafe(base.name) as MimanTing;
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001FD50 File Offset: 0x0001DF50
	public bool hasSetupTingRef
	{
		get
		{
			return this.ting != null;
		}
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x0001FD60 File Offset: 0x0001DF60
	public void ApplyTransformPositionToTing()
	{
		if (this.ting == null)
		{
			return;
		}
		if (this.ting.room == null)
		{
			return;
		}
		if (base.transform == null)
		{
			return;
		}
		this.ting.position = new WorldCoordinate(this.ting.room.name, MimanHelper.Vector3ToTilePoint(base.transform.position));
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0001FDCC File Offset: 0x0001DFCC
	public void ApplyTransformRotationToTing()
	{
		this.ting.direction = GridMath.DegreesToDirection((int)base.transform.eulerAngles.y);
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x0001FE00 File Offset: 0x0001E000
	private void OnDialogueLineChange(string pPreviousDialogueLine, string pNewDialogueLine)
	{
		if (pNewDialogueLine == null)
		{
			pNewDialogueLine = string.Empty;
		}
		if (pPreviousDialogueLine == null)
		{
			pPreviousDialogueLine = string.Empty;
		}
		if (this._decliningBubble != null)
		{
			this._decliningBubble.DestroyIn(0f);
		}
		this._decliningBubble = this._bubble;
		if (this._decliningBubble != null)
		{
			this._decliningBubble.DestroyIn(0f);
		}
		if (pNewDialogueLine == string.Empty)
		{
			this._bubble = null;
		}
		else
		{
			string text = WorldOwner.instance.world.translator.Get(pNewDialogueLine, this.ting.lastConversation);
			this._bubble = RunGameWorld.instance.gameViewControls.bubbleManager.CreateBubble(this.useDigitalBubbles, this.mouthPosition, text, BubbleType.TALK, null, 5f / TimedDialogueNode.speedScaling, this.ting.name);
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x0600049E RID: 1182 RVA: 0x0001FEF4 File Offset: 0x0001E0F4
	protected virtual bool useDigitalBubbles
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x0600049F RID: 1183 RVA: 0x0001FEF8 File Offset: 0x0001E0F8
	public virtual Transform mouthPosition
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0001FF00 File Offset: 0x0001E100
	protected void PlaySound(string pKey)
	{
		if (this._audioSource == null)
		{
			Debug.Log("The shell for " + base.name + " doesn't have an audio source");
			return;
		}
		SoundDictionary.PlaySound(pKey, this._audioSource);
		if (this._audioSource.clip == null)
		{
			Debug.Log("_audioSource.clip == null");
		}
		else if (this.ting != null)
		{
			this.ting.audioTotalLength = this._audioSource.clip.length;
			this._audioSource.time = this.ting.audioTime;
		}
		else
		{
			Debug.Log(string.Concat(new string[] { "ting was null for ", base.name, " in PlaySound(", pKey, ")" }));
		}
		this._audioSource.loop = this.ting.audioLoop;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0001FFF8 File Offset: 0x0001E1F8
	protected void StopSound()
	{
		if (this._audioSource == null)
		{
			Debug.LogError("The shell for " + base.name + " doesn't have an audio source");
		}
		this._audioSource.Stop();
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0002003C File Offset: 0x0001E23C
	private void OnGUI()
	{
		if (this._tooltipAlpha < 0.01f)
		{
			return;
		}
		if (this.ting == null)
		{
			return;
		}
		if (this._tooltipTexture1 == null)
		{
			return;
		}
		if (this._tooltipTexture2 == null)
		{
			return;
		}
		GUI.skin = this._skin;
		Vector2 vector = this.AnchorPointOnScreen();
		GUI.color = new Color(1f, 1f, 1f, this._tooltipAlpha);
		float num = ((!this._tooltipShowsSecondLine) ? 45f : 90f);
		Rect rect = new Rect(vector.x + 50f, vector.y, 350f, num);
		GUI.DrawTexture(rect, this._tooltipTexture1);
		GUILayout.BeginArea(rect);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(new GUIContent(this._interactionText, this._lmb), new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		if (this._tooltipShowsSecondLine)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label(new GUIContent(this._interactionText2, this._rmb), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x00020174 File Offset: 0x0001E374
	private Vector2 AnchorPointOnScreen()
	{
		Vector3 vector = Camera.main.WorldToScreenPoint(base.transform.position + new Vector3(0f, 3f, 0f));
		return new Vector2(vector.x, (float)Screen.height - vector.y);
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x000201CC File Offset: 0x0001E3CC
	public void ShowTooltip(string pInteractionText, string pInteractionText2)
	{
		this._interactionText = pInteractionText;
		this._interactionText2 = pInteractionText2;
		this._tooltipShowsSecondLine = !(pInteractionText2 == string.Empty);
		this._showTooltip = true;
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0002020C File Offset: 0x0001E40C
	public virtual Vector3 lookTargetPoint
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x0400036E RID: 878
	public const float TILE_WIDTH = 1f;

	// Token: 0x0400036F RID: 879
	public const float HALF_TILE_WIDTH = 0.5f;

	// Token: 0x04000370 RID: 880
	protected const float ANIMATION_CROSS_FADE_TIME = 0.5f;

	// Token: 0x04000371 RID: 881
	private Bubble _bubble;

	// Token: 0x04000372 RID: 882
	private Bubble _decliningBubble;

	// Token: 0x04000373 RID: 883
	protected AudioSource _audioSource;

	// Token: 0x04000374 RID: 884
	protected Animation _animationComponent;

	// Token: 0x04000375 RID: 885
	protected Dictionary<string, string[]> _animationsForActionNames = new Dictionary<string, string[]>();

	// Token: 0x04000376 RID: 886
	protected Transform _compilerSmoke;

	// Token: 0x04000377 RID: 887
	protected int _moduluTickToCheckForSmoke = 1;

	// Token: 0x04000378 RID: 888
	private GUISkin _skin;

	// Token: 0x04000379 RID: 889
	private float _tooltipAlpha;

	// Token: 0x0400037A RID: 890
	protected bool _showTooltip;

	// Token: 0x0400037B RID: 891
	private string _interactionText;

	// Token: 0x0400037C RID: 892
	private string _interactionText2;

	// Token: 0x0400037D RID: 893
	private Texture _tooltipTexture1;

	// Token: 0x0400037E RID: 894
	private Texture _tooltipTexture2;

	// Token: 0x0400037F RID: 895
	private Texture _lmb;

	// Token: 0x04000380 RID: 896
	private Texture _rmb;

	// Token: 0x04000381 RID: 897
	private bool _tooltipShowsSecondLine;
}
