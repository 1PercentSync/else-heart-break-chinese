using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class GameViewStateHandler
{
	// Token: 0x060000FD RID: 253 RVA: 0x00006294 File Offset: 0x00004494
	public GameViewStateHandler(GameViewControls pControls)
	{
		this._controls = pControls;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x000062B0 File Offset: 0x000044B0
	public void DrawGUI()
	{
		foreach (GameViewState gameViewState in this._stateList)
		{
			if (gameViewState.state != GameViewState.STATE.NOT_LOADED)
			{
				gameViewState.UpdateGUIAnimations(Time.deltaTime);
				gameViewState.OnGUI();
			}
		}
	}

	// Token: 0x060000FF RID: 255 RVA: 0x0000632C File Offset: 0x0000452C
	public void OnGizmos()
	{
		foreach (GameViewState gameViewState in this._stateList)
		{
			if (gameViewState.state != GameViewState.STATE.NOT_LOADED)
			{
				gameViewState.OnGizmos();
			}
		}
	}

	// Token: 0x06000100 RID: 256 RVA: 0x000063A0 File Offset: 0x000045A0
	public void OnRenderObject()
	{
		foreach (GameViewState gameViewState in this._stateList)
		{
			if (gameViewState.state != GameViewState.STATE.NOT_LOADED)
			{
				gameViewState.OnRenderObject();
			}
		}
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00006414 File Offset: 0x00004614
	public void Update()
	{
		if (this.currentState != null && !this.currentState.enumerator.MoveNext())
		{
			this._stateList.RemoveLast();
			if (this.currentState != null && this.currentState.state == GameViewState.STATE.PAUSED)
			{
				this.currentState.Resume();
			}
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00006474 File Offset: 0x00004674
	public void LatestUpdate()
	{
		foreach (GameViewState gameViewState in this._stateList)
		{
			if (gameViewState.state != GameViewState.STATE.NOT_LOADED)
			{
				gameViewState.OnLatestUpdate();
			}
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000103 RID: 259 RVA: 0x000064E8 File Offset: 0x000046E8
	public GameViewState currentState
	{
		get
		{
			return (this._stateList.Count <= 0) ? null : this._stateList.Last.Value;
		}
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00006514 File Offset: 0x00004714
	public void PushState(GameViewState pNextState)
	{
		if (this.currentState != null)
		{
			this.currentState.OnPaused();
		}
		this.Init(pNextState);
		this._stateList.AddLast(pNextState);
	}

	// Token: 0x06000105 RID: 261 RVA: 0x0000654C File Offset: 0x0000474C
	public void ChangeState(GameViewState pNewState)
	{
		foreach (GameViewState gameViewState in this._stateList)
		{
			gameViewState.EndState();
		}
		this.Init(pNewState);
		this._stateList.AddFirst(pNewState);
	}

	// Token: 0x06000106 RID: 262 RVA: 0x000065C8 File Offset: 0x000047C8
	private void Init(GameViewState pState)
	{
		pState.Init(this, this._controls);
	}

	// Token: 0x0400009A RID: 154
	private GameViewControls _controls;

	// Token: 0x0400009B RID: 155
	private LinkedList<GameViewState> _stateList = new LinkedList<GameViewState>();
}
