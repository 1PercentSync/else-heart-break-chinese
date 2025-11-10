using System;
using System.Collections.Generic;
using System.Linq;
using GameTypes;
using GameWorld2;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class PauseMenu : GameViewState
{
	// Token: 0x0600018C RID: 396 RVA: 0x0000A92C File Offset: 0x00008B2C
	public PauseMenu(PlayerRoamingState pRoamingState, ActionMenu pActionMenu)
	{
		this._skin = (GUISkin)Resources.Load("DefaultSkin");
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000A9C0 File Offset: 0x00008BC0
	public override void OnEnterBegin()
	{
		this.position = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		this._teleportationPoints.AddRange(base.controls.world.tingRunner.GetTingsOfType<Point>());
		this._characters.AddRange(from c in base.controls.world.tingRunner.GetTingsOfType<Character>()
			orderby c.name
			select c);
		base.controls.fade.FadeToColor(new Color(0.2f, 0.2f, 0.2f, 0.6f));
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000AA74 File Offset: 0x00008C74
	public override GameViewState.RETURN OnEnterLoop()
	{
		if (base.controls.fade.alpha <= 0.6f)
		{
			return GameViewState.RETURN.RUN_AGAIN;
		}
		Time.timeScale = 0f;
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000AAA8 File Offset: 0x00008CA8
	public override void OnExitBegin()
	{
		base.controls.fade.FadeToTransparent();
		Time.timeScale = 1f;
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000AAC4 File Offset: 0x00008CC4
	public override GameViewState.RETURN OnExitLoop()
	{
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000AAC8 File Offset: 0x00008CC8
	public override void OnUpdate()
	{
		MainMenu.SetVolume(1f - base.controls.fade.alpha);
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			base.EndState();
		}
		if (this._guiAction != null)
		{
			this._guiAction();
		}
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000AB18 File Offset: 0x00008D18
	public override void OnGUI()
	{
		GUI.skin = this._skin;
		GUI.color = Color.white;
		if (GUI.Button(new Rect(this.position.x - 50f, this.position.y - 50f, 100f, 50f), "Resume"))
		{
			base.EndState();
		}
		if (GUI.Button(new Rect(this.position.x + -50f, this.position.y + 50f, 100f, 50f), "Save"))
		{
			this._guiAction = delegate
			{
				GameTime gameTimeClock = base.controls.world.settings.gameTimeClock;
				string text2 = string.Concat(new object[]
				{
					base.controls.avatar.room.name.Replace("_", " "),
					" - Day ",
					gameTimeClock.days,
					" - ",
					gameTimeClock.ToStringWithoutDayAndSeconds().Replace(":", "#")
				});
				base.controls.world.Save(WorldOwner.QUICKSAVE_DATA_PATH + text2 + ".json");
				base.EndState();
			};
		}
		this.pointsScrollPos = GUILayout.BeginScrollView(this.pointsScrollPos, new GUILayoutOption[0]);
		foreach (Point point in this._teleportationPoints)
		{
			Point saveThisInLambda2 = point;
			if (GUILayout.Button(point.name, new GUILayoutOption[0]))
			{
				this._guiAction = delegate
				{
					if (this.avatar != null)
					{
						this.avatar.position = saveThisInLambda2.position;
						this.avatar.ClearState();
						this.EndState();
					}
				};
			}
		}
		GUILayout.EndScrollView();
		this.charactersScrollPos = GUILayout.BeginScrollView(this.charactersScrollPos, new GUILayoutOption[0]);
		foreach (Character character in this._characters)
		{
			Character saveThisInLambda = character;
			if (GUILayout.Button(character.name + " at " + character.position, new GUILayoutOption[0]))
			{
				this._guiAction = delegate
				{
					if (this.avatar != null)
					{
						this.avatar.position = saveThisInLambda.position;
						this.avatar.ClearState();
						this.EndState();
					}
				};
			}
		}
		GUILayout.EndScrollView();
		GUILayout.BeginArea(new Rect(500f, 0f, 300f, (float)Screen.height));
		this.eventLogScrollPos = GUILayout.BeginScrollView(this.eventLogScrollPos, new GUILayoutOption[0]);
		foreach (string text in base.controls.world.settings.storyEventLog)
		{
			GUILayout.Button(text, new GUILayoutOption[0]);
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000193 RID: 403 RVA: 0x0000ADC4 File Offset: 0x00008FC4
	private Character avatar
	{
		get
		{
			return base.controls.avatar as Character;
		}
	}

	// Token: 0x040000FA RID: 250
	private const float ANIM_TIME = 1f;

	// Token: 0x040000FB RID: 251
	private const float TARGET_ALPHA = 0.6f;

	// Token: 0x040000FC RID: 252
	private Vector2 position = new Vector2(-100f, -100f);

	// Token: 0x040000FD RID: 253
	private Function _guiAction;

	// Token: 0x040000FE RID: 254
	private List<Point> _teleportationPoints = new List<Point>();

	// Token: 0x040000FF RID: 255
	private List<Character> _characters = new List<Character>();

	// Token: 0x04000100 RID: 256
	private Vector2 pointsScrollPos = new Vector2(0f, 0f);

	// Token: 0x04000101 RID: 257
	private Vector2 charactersScrollPos = new Vector2(0f, 0f);

	// Token: 0x04000102 RID: 258
	private Vector2 eventLogScrollPos = new Vector2(0f, 0f);

	// Token: 0x04000103 RID: 259
	private GUISkin _skin;
}
