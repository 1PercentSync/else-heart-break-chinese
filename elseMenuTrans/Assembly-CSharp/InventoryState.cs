using System;
using GameWorld2;
using TingTing;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class InventoryState : GameViewState
{
	// Token: 0x06000179 RID: 377 RVA: 0x0000A304 File Offset: 0x00008504
	public override void OnEnterBegin()
	{
		base.controls.fade.FadeToColor(new Color(0.2f, 0.2f, 0.2f, 0.6f));
		this._guiRoot = new Container(new Vector2(0f, 0f));
		this._inventoryList = new Container(new Vector2(400f, 200f));
		this._guiRoot.AddChild(this._inventoryList);
		this.BuildInventoryList();
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000A388 File Offset: 0x00008588
	public override GameViewState.RETURN OnEnterLoop()
	{
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000A38C File Offset: 0x0000858C
	public override void OnExitBegin()
	{
		base.controls.fade.FadeToTransparent();
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000A3A0 File Offset: 0x000085A0
	public override GameViewState.RETURN OnExitLoop()
	{
		return GameViewState.RETURN.FINISHED;
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000A3A4 File Offset: 0x000085A4
	public override void OnUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			base.EndState();
		}
		Container containerAtPosition = this._guiRoot.GetContainerAtPosition(Scaled.MousePos);
		if (containerAtPosition != null)
		{
			containerAtPosition.selected = true;
			if (Input.GetMouseButtonDown(0))
			{
				containerAtPosition.Pressed();
				return;
			}
		}
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000A3F4 File Offset: 0x000085F4
	public override void OnGUI()
	{
		GUI.color = Color.white;
		if (this._guiRoot != null)
		{
			this._guiRoot.Draw();
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000A424 File Offset: 0x00008624
	private void BuildInventoryList()
	{
		if (this.avatar == null)
		{
			Debug.LogError("No avatar!");
			return;
		}
		this._inventoryList.RemoveChildren();
		float num = 0f;
		float num2 = 0f;
		if (this.avatar.inventoryItems.Length == 0)
		{
			return;
		}
		this._inventoryList.AddChild(new Label("ActionOptionsBackground_NOSCALE", "Inventory:", new Vector2(num, num2)));
		Ting[] inventoryItems = this.avatar.inventoryItems;
		for (int i = 0; i < inventoryItems.Length; i++)
		{
			Ting ting = inventoryItems[i];
			num2 += 35f;
			Label label = new Label("ActionOptionsBackground_NOSCALE", ting.tooltipName, new Vector2(num, num2));
			string tingName = ting.name;
			label.SetOnContainerPressedDelegate(delegate(Container pContainer)
			{
				if (this.avatar.handItem != null)
				{
					this.avatar.MoveHandItemToInventory();
				}
				Ting ting2 = WorldOwner.instance.world.tingRunner.GetTing(tingName);
				this.avatar.TakeOutInventoryItem(ting2);
				this.EndState();
			});
			this._inventoryList.AddChild(label);
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000180 RID: 384 RVA: 0x0000A51C File Offset: 0x0000871C
	private Character avatar
	{
		get
		{
			return base.controls.avatar as Character;
		}
	}

	// Token: 0x040000ED RID: 237
	private Container _guiRoot;

	// Token: 0x040000EE RID: 238
	private Container _inventoryList;

	// Token: 0x040000EF RID: 239
	private Container _interactionList;
}
