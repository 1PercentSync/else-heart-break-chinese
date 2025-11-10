using System;
using System.Threading;
using GameWorld2;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class GameWorldLoadingOnUpdate : MonoBehaviour
{
	// Token: 0x06000074 RID: 116 RVA: 0x0000476C File Offset: 0x0000296C
	public void Update()
	{
		if (this.world == null && this.timer-- < 0)
		{
			this.world = WorldOwner.instance.world;
			Debug.LogError("MAKING WORLD CALL");
		}
		else if (this.world != null)
		{
			this.world.Update(Time.deltaTime);
			Thread.Sleep(100);
		}
	}

	// Token: 0x0400005A RID: 90
	private World world;

	// Token: 0x0400005B RID: 91
	private int timer = 10;
}
