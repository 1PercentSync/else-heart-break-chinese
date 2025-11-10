using System;
using ProgrammingLanguageNr1;

namespace GameWorld2
{
	// Token: 0x0200005A RID: 90
	public class ArcadeMachineAPI
	{
		// Token: 0x0600057A RID: 1402 RVA: 0x0001B07C File Offset: 0x0001927C
		public ArcadeMachineAPI(Computer pComputer)
		{
			this._computer = pComputer;
		}

		// Token: 0x0600057B RID: 1403
		[SprakAPI(new string[] { "Is a key pressed? (left/right/down/up/space)是否有按键按下？ (左/右/下/上/空格)" })]
		public bool API_IsKeyPressed(string key)
		{
			if (this._computer.isKeyPressed == null)
			{
				return false;
			}
			bool flag = this._computer.isKeyPressed(key.ToLower());
			if (flag)
			{
				this._computer.activeProgram.executionTime = 0f;
			}
			return flag;
		}

		// Token: 0x04000171 RID: 369
		private Computer _computer;
	}
}
