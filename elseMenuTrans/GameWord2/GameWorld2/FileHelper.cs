using System;

namespace GameWorld2
{
	// Token: 0x02000047 RID: 71
	public class FileHelper
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x000175AC File Offset: 0x000157AC
		public static string GetNameFromFilepath(string pFilepath)
		{
			if (string.IsNullOrEmpty(pFilepath))
			{
				throw new Exception("Filepath is empty!");
			}
			int num = pFilepath.LastIndexOf("/");
			int num2 = pFilepath.LastIndexOf("\\");
			if (num2 > num)
			{
				num = num2;
			}
			string text = pFilepath.Substring(num + 1);
			string text2 = text;
			int num3 = text.LastIndexOf(".");
			if (num3 > -1)
			{
				text2 = text.Substring(0, num3);
			}
			return text2;
		}
	}
}
