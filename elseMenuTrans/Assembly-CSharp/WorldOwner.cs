using System;
using System.Diagnostics;
using GameTypes;
using GameWorld2;
using RelayLib;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class WorldOwner
{
	// Token: 0x06000729 RID: 1833 RVA: 0x0002E94C File Offset: 0x0002CB4C
	private WorldOwner()
	{
		this.status = "No world loaded";
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x0600072C RID: 1836 RVA: 0x0002E9A4 File Offset: 0x0002CBA4
	// (set) Token: 0x0600072B RID: 1835 RVA: 0x0002E998 File Offset: 0x0002CB98
	public string status { get; private set; }

	// Token: 0x0600072D RID: 1837 RVA: 0x0002E9AC File Offset: 0x0002CBAC
	private World NewEmptyWorld()
	{
		return new World(new InitialSaveFileCreator().CreateEmptyRelay());
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x0600072E RID: 1838 RVA: 0x0002E9C0 File Offset: 0x0002CBC0
	// (set) Token: 0x0600072F RID: 1839 RVA: 0x0002EA8C File Offset: 0x0002CC8C
	public World world
	{
		get
		{
			if (this._world == null && this._error == null)
			{
				try
				{
					if (IsInPlaymode.on)
					{
						global::UnityEngine.Debug.Log("WorldOwner is creating a new empty world");
						this._world = this.NewEmptyWorld();
					}
					else
					{
						global::UnityEngine.Debug.Log("WorldOwner is loading a world from init data");
						this._world = this.LoadWorldFromInitData();
					}
				}
				catch (Exception ex)
				{
					this.status = "Empty World Loaded";
					this._world = this.NewEmptyWorld();
					this._error = new Exception("Failed to load world", ex);
					throw this._error;
				}
			}
			if (IsInPlaymode.on)
			{
				this.EnsureRunGameWorldCamera();
			}
			return this._world;
		}
		set
		{
			this._world = value;
			if (this._world == null)
			{
				this.status = "No world loaded";
			}
			else
			{
				this.status = "World Is set";
			}
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000730 RID: 1840 RVA: 0x0002EABC File Offset: 0x0002CCBC
	public bool worldIsLoaded
	{
		get
		{
			return this._world != null;
		}
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x0002EACC File Offset: 0x0002CCCC
	public World LoadWorldFromInitData()
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		InitialSaveFileCreator initialSaveFileCreator = new InitialSaveFileCreator();
		initialSaveFileCreator.logger.AddListener(new D.LogHandler(global::UnityEngine.Debug.Log));
		bool flag = true;
		RelayTwo relayTwo = initialSaveFileCreator.CreateRelay(WorldOwner.INIT_DATA_PATH, flag);
		World world = new World(relayTwo);
		this.status = "World is loaded";
		initialSaveFileCreator.logger.RemoveListener(new D.LogHandler(global::UnityEngine.Debug.Log));
		stopwatch.Stop();
		global::UnityEngine.Debug.Log("WorldOwner.LoadWorldFromInitData took " + stopwatch.Elapsed.TotalSeconds + " s.");
		return world;
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x0002EB6C File Offset: 0x0002CD6C
	public void LoadWorldFromSave(string pFilePath)
	{
		this._world = new World(new RelayTwo(pFilePath));
		this.status = "World is loaded";
		global::UnityEngine.Debug.Log("Loaded new World from save");
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x0002EBA0 File Offset: 0x0002CDA0
	private void EnsureRunGameWorldCamera()
	{
		if (RunGameWorld.instance == null)
		{
			global::UnityEngine.Object.Instantiate(Resources.Load("RunGameWorldCamera"));
			if (RunGameWorld.instance == null)
			{
				D.LogError("Failed to instantiate RunGameWorldCamera");
			}
		}
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x0002EBE8 File Offset: 0x0002CDE8
	public void UnloadWorld()
	{
		global::UnityEngine.Debug.Log("Unloading world!");
		this._error = null;
		this._world = null;
		this.status = "No world loaded";
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000735 RID: 1845 RVA: 0x0002EC10 File Offset: 0x0002CE10
	public static WorldOwner instance
	{
		get
		{
			if (WorldOwner._instance == null)
			{
				WorldOwner._instance = new WorldOwner();
			}
			return WorldOwner._instance;
		}
	}

	// Token: 0x040004C6 RID: 1222
	public static string INIT_DATA_PATH = Application.dataPath + "/InitData/";

	// Token: 0x040004C7 RID: 1223
	public static string QUICKSAVE_DATA_PATH = Application.dataPath + "/Saves/";

	// Token: 0x040004C8 RID: 1224
	private static WorldOwner _instance;

	// Token: 0x040004C9 RID: 1225
	private World _world;

	// Token: 0x040004CA RID: 1226
	private Exception _error;
}
