using System;
using GameTypes;
using ProgrammingLanguageNr1;

namespace GameWorld2
{
	// Token: 0x02000050 RID: 80
	public class WeatherAPI
	{
		// Token: 0x0600053C RID: 1340 RVA: 0x00019778 File Offset: 0x00017978
		public WeatherAPI(Computer pComputer, WorldSettings pWorldSettings)
		{
			this._computer = pComputer;
			this._worldSettings = pWorldSettings;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00019790 File Offset: 0x00017990
		[SprakAPI(new string[] { "Set the amount of rain (0 - 250)" })]
		public void API_SetRain(float amount)
		{
			if (amount < 0f)
			{
				amount = 0f;
			}
			else if (amount > 250f)
			{
				amount = 250f;
			}
			this._worldSettings.rainTargetValue = amount;
			this._computer.API_Print("Rain level was set to " + amount);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x000197F0 File Offset: 0x000179F0
		[SprakAPI(new string[] { "Get the amount of rain (0 - 250)" })]
		public float API_GetRain()
		{
			float num = this._worldSettings.rain + Randomizer.GetValue(0.1f, 10f);
			if (num < 0f)
			{
				num = 0f;
			}
			if (num > 250f)
			{
				num = 250f;
			}
			return num;
		}

		// Token: 0x04000157 RID: 343
		private Computer _computer;

		// Token: 0x04000158 RID: 344
		private WorldSettings _worldSettings;
	}
}
