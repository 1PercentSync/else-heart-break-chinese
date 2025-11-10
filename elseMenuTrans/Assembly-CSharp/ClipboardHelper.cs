using System;
using System.Reflection;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public static class ClipboardHelper
{
	// Token: 0x0600064D RID: 1613 RVA: 0x00029608 File Offset: 0x00027808
	private static PropertyInfo GetSystemCopyBufferProperty()
	{
		if (ClipboardHelper.m_systemCopyBufferProperty == null)
		{
			Type typeFromHandle = typeof(GUIUtility);
			ClipboardHelper.m_systemCopyBufferProperty = typeFromHandle.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
			if (ClipboardHelper.m_systemCopyBufferProperty == null)
			{
				throw new Exception("Can't access internal member 'GUIUtility.systemCopyBuffer' it may have been removed / renamed");
			}
		}
		return ClipboardHelper.m_systemCopyBufferProperty;
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x0600064E RID: 1614 RVA: 0x00029658 File Offset: 0x00027858
	// (set) Token: 0x0600064F RID: 1615 RVA: 0x00029678 File Offset: 0x00027878
	public static string clipboard
	{
		get
		{
			PropertyInfo systemCopyBufferProperty = ClipboardHelper.GetSystemCopyBufferProperty();
			return (string)systemCopyBufferProperty.GetValue(null, null);
		}
		set
		{
			PropertyInfo systemCopyBufferProperty = ClipboardHelper.GetSystemCopyBufferProperty();
			systemCopyBufferProperty.SetValue(null, value, null);
		}
	}

	// Token: 0x0400041A RID: 1050
	private static PropertyInfo m_systemCopyBufferProperty;
}
