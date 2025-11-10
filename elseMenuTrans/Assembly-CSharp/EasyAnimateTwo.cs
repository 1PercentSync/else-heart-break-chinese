using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000013 RID: 19
public class EasyAnimateTwo
{
	// Token: 0x0600006D RID: 109 RVA: 0x00004650 File Offset: 0x00002850
	public void Update(float pDeltaTime)
	{
		foreach (KeyValuePair<int, IEasyAnimState> keyValuePair in this._objects.ToArray<KeyValuePair<int, IEasyAnimState>>())
		{
			int key = keyValuePair.Key;
			IEasyAnimState value = keyValuePair.Value;
			if (!value.Interpolate(pDeltaTime))
			{
				if (value.nextState != null)
				{
					this._objects[keyValuePair.Key] = value.nextState;
				}
				else
				{
					this._objects.Remove(key);
				}
			}
		}
	}

	// Token: 0x0600006E RID: 110 RVA: 0x000046E0 File Offset: 0x000028E0
	private int GetHash(object o, string pChannel)
	{
		return o.GetHashCode() ^ pChannel.GetHashCode();
	}

	// Token: 0x0600006F RID: 111 RVA: 0x000046F0 File Offset: 0x000028F0
	public void Register(object o, string pChannel, IEasyAnimState a)
	{
		int hash = this.GetHash(o, pChannel);
		if (!this._objects.ContainsKey(hash))
		{
			this._objects.Add(hash, a);
		}
		else
		{
			this._objects[hash] = a;
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00004738 File Offset: 0x00002938
	public bool IsAnimating(object o, string pChannel)
	{
		int hash = this.GetHash(o, pChannel);
		return this._objects.ContainsKey(hash);
	}

	// Token: 0x04000059 RID: 89
	private Dictionary<int, IEasyAnimState> _objects = new Dictionary<int, IEasyAnimState>();

	// Token: 0x020000F8 RID: 248
	// (Invoke) Token: 0x0600073F RID: 1855
	public delegate void OnComplete();
}
