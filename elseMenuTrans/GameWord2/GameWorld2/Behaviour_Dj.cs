using System;
using GameTypes;
using GrimmLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200003F RID: 63
	public class Behaviour_Dj : TimetableBehaviour
	{
		// Token: 0x06000476 RID: 1142 RVA: 0x000169A4 File Offset: 0x00014BA4
		public Behaviour_Dj(string pMixerName, string[] pSongNames)
		{
			this._mixerName = pMixerName;
			this._songNames = pSongNames;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x000169BC File Offset: 0x00014BBC
		private bool AtGoal(Character pCharacter)
		{
			return this._mixer.HasInteractionPointHere(pCharacter.position) || this._mixer.position == pCharacter.position;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000169F8 File Offset: 0x00014BF8
		public float Execute(Character pCharacter, MimanTingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner, WorldSettings pWorldSettings)
		{
			if (pCharacter.busy)
			{
				return 1f;
			}
			if (this._mixer == null)
			{
				this._mixer = pTingRunner.GetTing<MusicBox>(this._mixerName);
				if (!this._mixer.mixer)
				{
					D.Log("The music box " + this._mixer.name + " isn't a dj Mixer");
				}
			}
			bool flag = this.AtGoal(pCharacter);
			bool flag2 = this._mixer.position == pCharacter.finalTargetPosition || this._mixer.HasInteractionPointHere(pCharacter.finalTargetPosition);
			bool flag3 = pCharacter.actionName == "";
			if (flag && flag3)
			{
				this._mixer.soundName = this._songNames[Randomizer.GetIntValue(0, this._songNames.Length)];
				this._mixer.audioTime = 0f;
				this._mixer.isPlaying = false;
				pCharacter.InteractWith(this._mixer);
			}
			else if (!flag && !flag2)
			{
				pCharacter.WalkTo(new WorldCoordinate(this._mixer.room.name, this._mixer.interactionPoints[0]));
			}
			return 1f;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00016B4C File Offset: 0x00014D4C
		public void OnFinish(Character pCharacter, TingRunner pTingRunner, RoomRunner pRoomRunner, DialogueRunner pDialogueRunner)
		{
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00016B50 File Offset: 0x00014D50
		public bool IsAtFinalPartOfTask(Character pCharacter)
		{
			return this.AtGoal(pCharacter);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00016B5C File Offset: 0x00014D5C
		public void Reset()
		{
		}

		// Token: 0x0400011D RID: 285
		private string _mixerName;

		// Token: 0x0400011E RID: 286
		private MusicBox _mixer;

		// Token: 0x0400011F RID: 287
		private string[] _songNames;
	}
}
