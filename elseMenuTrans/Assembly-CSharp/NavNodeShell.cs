using System;
using GameWorld2;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class NavNodeShell : Shell
{
	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060005AF RID: 1455 RVA: 0x00026E38 File Offset: 0x00025038
	public NavNode navNode
	{
		get
		{
			return base.ting as NavNode;
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00026E48 File Offset: 0x00025048
	public override void CreateTing()
	{
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00026E4C File Offset: 0x0002504C
	protected override bool ShouldSnapPosAndDir()
	{
		return true;
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00026E50 File Offset: 0x00025050
	private void OnDrawGizmos()
	{
		if (this.targetNavNode != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(base.transform.position, this.targetNavNode.transform.position);
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00026E98 File Offset: 0x00025098
	private Shell targetNavNode
	{
		get
		{
			if (!WorldOwner.instance.worldIsLoaded || this.navNode == null)
			{
				return null;
			}
			if (this._targetNodeCache == null)
			{
				this._targetNodeCache = ShellManager.GetShellWithName(this.navNode.mainTrackName);
			}
			return this._targetNodeCache;
		}
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x00026EF0 File Offset: 0x000250F0
	public void InvalidateCache()
	{
		this._targetNodeCache = null;
	}

	// Token: 0x040003EA RID: 1002
	private Shell _targetNodeCache;
}
