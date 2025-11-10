using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class ShellManager
{
	// Token: 0x060004AF RID: 1199 RVA: 0x000202EC File Offset: 0x0001E4EC
	public static Shell GetShellWithName(string pName)
	{
		return ShellManager.GetShellsInScene().Find((Shell o) => o.name == pName);
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0002031C File Offset: 0x0001E51C
	public static Shell[] GetShellsWithName(string pName)
	{
		List<Shell> list = new List<Shell>();
		global::UnityEngine.Object[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(Shell));
		foreach (global::UnityEngine.Object @object in array)
		{
			if (@object.name == pName)
			{
				list.Add(@object as Shell);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00020384 File Offset: 0x0001E584
	public static bool ShellWithNameExistsInRoom(string pName)
	{
		List<Shell> shellsInScene = ShellManager.GetShellsInScene();
		return shellsInScene.Find((Shell o) => o.name == pName) != null;
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x000203BC File Offset: 0x0001E5BC
	public static List<Shell> GetShellsInScene()
	{
		List<Shell> list = new List<Shell>();
		global::UnityEngine.Object[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(Shell));
		foreach (global::UnityEngine.Object @object in array)
		{
			list.Add(@object as Shell);
		}
		return list;
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x0002040C File Offset: 0x0001E60C
	public static List<Shell> GetShellsWithTingConnectionInScene()
	{
		List<Shell> list = new List<Shell>();
		global::UnityEngine.Object[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(Shell));
		foreach (global::UnityEngine.Object @object in array)
		{
			Shell shell = @object as Shell;
			if (shell.hasSetupTingRef)
			{
				list.Add(shell);
			}
		}
		return list;
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x0002046C File Offset: 0x0001E66C
	public static void DestroyShellsInScene()
	{
		foreach (Shell shell in ShellManager.GetShellsInScene())
		{
			global::UnityEngine.Object.Destroy(shell.gameObject);
		}
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x000204D8 File Offset: 0x0001E6D8
	public static bool TestForAnotherShellWithSameName(Shell pShell)
	{
		bool flag = false;
		foreach (Shell shell in ShellManager.GetShellsInScene())
		{
			if (shell != pShell && shell.transform.name == pShell.name)
			{
				flag = true;
				break;
			}
		}
		return flag;
	}
}
