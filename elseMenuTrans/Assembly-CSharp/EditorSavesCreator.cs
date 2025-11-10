using System;

// Token: 0x020000F4 RID: 244
public class EditorSavesCreator
{
	// Token: 0x06000728 RID: 1832 RVA: 0x0002E8EC File Offset: 0x0002CAEC
	public static string GetBaseName(string p)
	{
		int num = p.LastIndexOf("/");
		int num2 = p.LastIndexOf("\\");
		num = ((num2 <= num) ? num : num2);
		p = p.Substring(num + 1);
		int num3 = p.LastIndexOf(".");
		if (num3 == -1)
		{
			return string.Empty;
		}
		return p.Substring(0, num3);
	}
}
