using System;
using System.Linq;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200004F RID: 79
	public class InternetAPI
	{
		// Token: 0x06000538 RID: 1336 RVA: 0x000196D8 File Offset: 0x000178D8
		public InternetAPI(Computer pComputer, TingRunner pTingRunner)
		{
			this._computer = pComputer;
		}

		// Token: 0x06000539 RID: 1337
		[SprakAPI(new string[] { "Get a list of all connections (list of names)获取所有连接的列表的名字" })]
		public object[] API_GetConnections()
		{
			return this._computer.connectedTings.Select((MimanTing t) => t.name).ToArray<string>();
		}

		// Token: 0x0600053A RID: 1338
		[SprakAPI(new string[] { "Use with caution请谨慎使用" })]
		public void API_Slurp()
		{
			if (this._computer.user != null)
			{
				this._computer.user.SlurpIntoInternet(this._computer);
				return;
			}
			this._computer.API_Print("No one to slurp没有可吸入的目标");
		}

		// Token: 0x04000155 RID: 341
		private Computer _computer;
	}
}
