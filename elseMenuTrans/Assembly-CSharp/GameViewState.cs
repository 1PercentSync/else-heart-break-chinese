using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class GameViewState
{
	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060000DF RID: 223 RVA: 0x00006138 File Offset: 0x00004338
	// (set) Token: 0x060000E0 RID: 224 RVA: 0x00006140 File Offset: 0x00004340
	public bool initiated { get; private set; }

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000614C File Offset: 0x0000434C
	// (set) Token: 0x060000E2 RID: 226 RVA: 0x00006154 File Offset: 0x00004354
	public GameViewState.STATE state { get; private set; }

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060000E3 RID: 227 RVA: 0x00006160 File Offset: 0x00004360
	// (set) Token: 0x060000E4 RID: 228 RVA: 0x00006168 File Offset: 0x00004368
	private protected GameViewControls controls { protected get; private set; }

	// Token: 0x060000E5 RID: 229 RVA: 0x00006174 File Offset: 0x00004374
	public void Init(GameViewStateHandler pHandler, GameViewControls pControls)
	{
		this.state = GameViewState.STATE.NOT_LOADED;
		this.controls = pControls;
		this._handler = pHandler;
		this.initiated = true;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00006194 File Offset: 0x00004394
	public virtual void OnEnterBegin()
	{
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00006198 File Offset: 0x00004398
	public virtual GameViewState.RETURN OnEnterLoop()
	{
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000619C File Offset: 0x0000439C
	public virtual void OnExitBegin()
	{
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x000061A0 File Offset: 0x000043A0
	public virtual GameViewState.RETURN OnExitLoop()
	{
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000061A4 File Offset: 0x000043A4
	public virtual void OnGUI()
	{
	}

	// Token: 0x060000EB RID: 235 RVA: 0x000061A8 File Offset: 0x000043A8
	public virtual void OnPostRender()
	{
	}

	// Token: 0x060000EC RID: 236 RVA: 0x000061AC File Offset: 0x000043AC
	public virtual void OnRenderObject()
	{
	}

	// Token: 0x060000ED RID: 237 RVA: 0x000061B0 File Offset: 0x000043B0
	public virtual void OnUpdate()
	{
	}

	// Token: 0x060000EE RID: 238 RVA: 0x000061B4 File Offset: 0x000043B4
	public virtual void OnLatestUpdate()
	{
	}

	// Token: 0x060000EF RID: 239 RVA: 0x000061B8 File Offset: 0x000043B8
	public virtual void OnGizmos()
	{
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000061BC File Offset: 0x000043BC
	protected void ChangeState(GameViewState pNextState)
	{
		this._handler.ChangeState(pNextState);
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x000061CC File Offset: 0x000043CC
	protected void PushState(GameViewState pNewState)
	{
		this._handler.PushState(pNewState);
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000061DC File Offset: 0x000043DC
	private IEnumerator CreateEnumerator()
	{
		this.state = GameViewState.STATE.ENTERING;
		this.OnEnterBegin();
		while (this.state != GameViewState.STATE.NOT_LOADED)
		{
			this.UpdateAnimations(Time.deltaTime);
			switch (this.state)
			{
			case GameViewState.STATE.ENTERING:
				if (this.OnEnterLoop() == GameViewState.RETURN.FINISHED)
				{
					this.state = GameViewState.STATE.RUNNING;
				}
				break;
			case GameViewState.STATE.EXITING:
				if (this.OnExitLoop() == GameViewState.RETURN.FINISHED)
				{
					this.state = GameViewState.STATE.NOT_LOADED;
				}
				break;
			case GameViewState.STATE.RUNNING:
				this.OnUpdate();
				break;
			}
			IL_00CB:
			yield return null;
			continue;
			goto IL_00CB;
		}
		yield break;
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060000F3 RID: 243 RVA: 0x000061F8 File Offset: 0x000043F8
	public IEnumerator enumerator
	{
		get
		{
			if (this._enumerator == null)
			{
				this._enumerator = this.CreateEnumerator();
			}
			return this._enumerator;
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00006218 File Offset: 0x00004418
	public void EndState()
	{
		this.state = GameViewState.STATE.EXITING;
		this.OnExitBegin();
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00006228 File Offset: 0x00004428
	internal void PauseState()
	{
		this.state = GameViewState.STATE.PAUSED;
		this.OnPaused();
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00006238 File Offset: 0x00004438
	public virtual void OnPaused()
	{
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x0000623C File Offset: 0x0000443C
	internal void Resume()
	{
		this.state = GameViewState.STATE.RUNNING;
		this.OnResumed();
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x0000624C File Offset: 0x0000444C
	public virtual void OnResumed()
	{
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00006250 File Offset: 0x00004450
	internal void UpdateGUIAnimations(float pDeltaTime)
	{
		this._guiAnimate.Update(pDeltaTime);
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00006260 File Offset: 0x00004460
	internal void UpdateAnimations(float pDeltaTime)
	{
		this._easyAnimate.Update(pDeltaTime);
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00006270 File Offset: 0x00004470
	public bool IsTopState()
	{
		return this._handler.currentState == this;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00006280 File Offset: 0x00004480
	protected void EndTopState()
	{
		this._handler.currentState.EndState();
	}

	// Token: 0x0400008A RID: 138
	private GameViewStateHandler _handler;

	// Token: 0x0400008B RID: 139
	protected EasyAnimateTwo _guiAnimate = new EasyAnimateTwo();

	// Token: 0x0400008C RID: 140
	protected EasyAnimateTwo _easyAnimate = new EasyAnimateTwo();

	// Token: 0x0400008D RID: 141
	private IEnumerator _enumerator;

	// Token: 0x0200001D RID: 29
	public enum RETURN
	{
		// Token: 0x04000092 RID: 146
		FINISHED,
		// Token: 0x04000093 RID: 147
		RUN_AGAIN
	}

	// Token: 0x0200001E RID: 30
	public enum STATE
	{
		// Token: 0x04000095 RID: 149
		ENTERING,
		// Token: 0x04000096 RID: 150
		EXITING,
		// Token: 0x04000097 RID: 151
		RUNNING,
		// Token: 0x04000098 RID: 152
		PAUSED,
		// Token: 0x04000099 RID: 153
		NOT_LOADED
	}
}
