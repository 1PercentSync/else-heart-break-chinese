using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class RoomChanger
{
	// Token: 0x060003C6 RID: 966 RVA: 0x0001BA54 File Offset: 0x00019C54
	public RoomChanger(Fade pFade)
	{
		this._fade = pFade;
		this.busy = false;
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060003C7 RID: 967 RVA: 0x0001BA78 File Offset: 0x00019C78
	// (set) Token: 0x060003C8 RID: 968 RVA: 0x0001BA80 File Offset: 0x00019C80
	public bool busy { get; private set; }

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060003C9 RID: 969 RVA: 0x0001BA8C File Offset: 0x00019C8C
	public string currentRoom
	{
		get
		{
			return Application.loadedLevelName;
		}
	}

	// Token: 0x060003CA RID: 970 RVA: 0x0001BA94 File Offset: 0x00019C94
	public void LoadRoom(string pNewRoom)
	{
		Debug.Log("LoadRoom() with pNewRoom: " + pNewRoom);
		if (this.busy)
		{
			throw new Exception("Can't change room while busy!");
		}
		this.busy = true;
		this._nextLevel = pNewRoom;
		this._fade.speed = 0.9f;
		this._fade.FadeToColor(Color.black);
		this._fade.onFadedToOpaque = new Fade.FadeCompleteEvent(this.OnFadedToOpaqueCompleted);
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0001BB0C File Offset: 0x00019D0C
	public void LoadRoomImmediately(string pNewRoom)
	{
		Debug.Log("LoadRoomImmediately() with pNewRoom: " + pNewRoom);
		if (this.busy)
		{
			throw new Exception("Can't change room while busy!");
		}
		this.busy = true;
		this._nextLevel = pNewRoom;
		this._fade.speed = 5f;
		if (this._fade.mode == Fade.FadeMode.TRANSPARENT)
		{
			this._fade.FadeToColor(Color.black);
			this._fade.onFadedToOpaque = new Fade.FadeCompleteEvent(this.OnFadedToOpaqueCompleted);
		}
		else
		{
			Debug.Log("Fade is alreay opaque");
			this.OnFadedToOpaqueCompleted();
		}
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0001BBAC File Offset: 0x00019DAC
	private void OnFadedToOpaqueCompleted()
	{
		Debug.Log("OnFadedToOpaqueCompleted() with _nextLevel: " + this._nextLevel);
		if (this._nextLevel != string.Empty)
		{
			try
			{
				GC.Collect();
				Resources.UnloadUnusedAssets();
				Debug.Log("Ran GC.Collect() and Resources.UnloadUnusedAssets()! Will now load next level! (" + this._nextLevel + ")");
				Application.LoadLevel(this._nextLevel);
			}
			catch (Exception ex)
			{
				Debug.Log(ex.ToString());
			}
			finally
			{
				if (this.onRoomHasChanged != null)
				{
					Debug.Log("Calling onRoomHasChanged from RoomChanger, new room: " + this._nextLevel);
					this.onRoomHasChanged(this._nextLevel);
				}
				else
				{
					Debug.Log("onRoomHasChanged is null");
				}
				this._nextLevel = string.Empty;
				this._fade.speed = 1f;
				this._fade.FadeToTransparent();
				this.busy = false;
				Debug.Log(this._nextLevel + " successfully loaded");
			}
		}
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0001BCE0 File Offset: 0x00019EE0
	private void ShowCursor()
	{
		Screen.showCursor = true;
	}

	// Token: 0x040002EB RID: 747
	private Fade _fade;

	// Token: 0x040002EC RID: 748
	private string _nextLevel = string.Empty;

	// Token: 0x040002ED RID: 749
	public Action<string> onRoomHasChanged;

	// Token: 0x040002EE RID: 750
	public Action<string, Action> LoadLevelAsync;
}
