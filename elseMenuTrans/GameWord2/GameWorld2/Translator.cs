using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using GameTypes;

namespace GameWorld2
{
	// Token: 0x02000088 RID: 136
	public class Translator
	{
		// Token: 0x060007A1 RID: 1953 RVA: 0x000215C0 File Offset: 0x0001F7C0
		public Translator(Translator.Language pLanguage)
		{
			this._language = pLanguage;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x000215F8 File Offset: 0x0001F7F8
		public void LoadTranslationFiles(string pRootDirectory)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			this._dict = new Dictionary<Translator.Language, Dictionary<string, Dictionary<string, string>>>();
			this._dict.Add(Translator.Language.ENGLISH, new Dictionary<string, Dictionary<string, string>>());
			this._dict.Add(Translator.Language.LATIN, new Dictionary<string, Dictionary<string, string>>());
			string[] filesRecursively = this.GetFilesRecursively(pRootDirectory);
			for (int i = 0; i < filesRecursively.Length; i++)
			{
				this.FoundFile(filesRecursively[i]);
			}
			stopwatch.Stop();
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0002166C File Offset: 0x0001F86C
		private string[] GetFilesRecursively(string pPath)
		{
			List<string> list = new List<string>();
			string[] directories = Directory.GetDirectories(pPath);
			string[] files = Directory.GetFiles(pPath);
			foreach (string text in files)
			{
				list.Add(text);
			}
			foreach (string text2 in directories)
			{
				list.AddRange(this.GetFilesRecursively(text2));
			}
			return list.ToArray();
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000216EC File Offset: 0x0001F8EC
		private void FoundFile(string pFilepath)
		{
			if (pFilepath.EndsWith(".mtf"))
			{
				Translator.Language language = Translator.Language.NOT_SET;
				if (pFilepath.Contains(".eng"))
				{
					language = Translator.Language.ENGLISH;
				}
				else if (pFilepath.Contains(".lat"))
				{
					language = Translator.Language.LATIN;
				}
				if (language == Translator.Language.NOT_SET)
				{
					throw new Exception("Can't handle file path " + pFilepath);
				}
				this.LoadTranslationsFile(pFilepath, language);
			}
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00021758 File Offset: 0x0001F958
		private void LoadTranslationsFile(string pFilepath, Translator.Language pLanguage)
		{
			StreamReader streamReader = File.OpenText(pFilepath);
			int num = 0;
			while (!streamReader.EndOfStream)
			{
				string text = streamReader.ReadLine();
				Match match = Translator.regex.Match(text);
				if (match.Success)
				{
					string value = match.Groups[1].Value;
					string value2 = match.Groups[2].Value;
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(pFilepath));
					Dictionary<string, Dictionary<string, string>> dictionary = this._dict[pLanguage];
					Dictionary<string, string> dictionary2 = null;
					if (!dictionary.TryGetValue(fileNameWithoutExtension, out dictionary2))
					{
						dictionary2 = new Dictionary<string, string>();
						dictionary[fileNameWithoutExtension] = dictionary2;
					}
					dictionary2[value] = value2;
				}
				num++;
			}
			streamReader.Close();
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0002181C File Offset: 0x0001FA1C
		internal void SetLanguage(Translator.Language pLanguage)
		{
			if (pLanguage != Translator.Language.SWEDISH && this._dict == null)
			{
				throw new Exception("Must load translation files before changing the language to something other than Swedish");
			}
			this._language = pLanguage;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00021850 File Offset: 0x0001FA50
		public string Get(string pSentenceToTranslate, string pDialogue)
		{
			D.isNull(pDialogue, "pSentenceToTranslate can't be null!");
			D.isNull(pDialogue, "pDialogue can't be null!");
			if (this._language == Translator.Language.SWEDISH)
			{
				return pSentenceToTranslate;
			}
			if (this._dict == null)
			{
				throw new Exception("Can't translate sentence, translations files are not loaded");
			}
			Dictionary<string, Dictionary<string, string>> dictionary = this._dict[this._language];
			Dictionary<string, string> dictionary2 = null;
			if (!dictionary.TryGetValue(pDialogue, out dictionary2))
			{
				D.Log(string.Concat(new object[] { "WARNING! Didn't find translation for '", pSentenceToTranslate, "' and dialogue '", pDialogue, "' in language '", this._language, "'." }));
				return pSentenceToTranslate;
			}
			string text;
			bool flag = dictionary2.TryGetValue(pSentenceToTranslate, out text);
			if (flag)
			{
				return text;
			}
			D.Log(string.Concat(new string[] { "Found no translation for ", pSentenceToTranslate, " in dia '", pDialogue, "'" }));
			return pSentenceToTranslate;
		}

		// Token: 0x04000200 RID: 512
		public Logger logger = new Logger();

		// Token: 0x04000201 RID: 513
		private Translator.Language _language = Translator.Language.SWEDISH;

		// Token: 0x04000202 RID: 514
		private Dictionary<Translator.Language, Dictionary<string, Dictionary<string, string>>> _dict;

		// Token: 0x04000203 RID: 515
		private static Regex regex = new Regex("\"(.+)\" => \"(.+)\"");

		// Token: 0x02000089 RID: 137
		public enum Language
		{
			// Token: 0x04000205 RID: 517
			NOT_SET,
			// Token: 0x04000206 RID: 518
			SWEDISH,
			// Token: 0x04000207 RID: 519
			ENGLISH,
			// Token: 0x04000208 RID: 520
			LATIN
		}
	}
}
