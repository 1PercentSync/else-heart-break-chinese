using System;
using System.Threading;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class IsInPlaymode
{
	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000383 RID: 899 RVA: 0x00019D04 File Offset: 0x00017F04
	public static bool on
	{
		get
		{
			if (Thread.CurrentThread.IsBackground)
			{
				Debug.Log("Accessing Application.isPlaying from background thread!");
				return false;
			}
			bool flag;
			try
			{
				flag = Application.isPlaying;
			}
			catch (Exception ex)
			{
				Debug.Log("Exception when calling Application.isPlaying: " + ex.ToString());
				flag = false;
			}
			return flag;
		}
	}
}
