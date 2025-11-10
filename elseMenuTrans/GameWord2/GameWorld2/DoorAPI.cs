using System;
using System.Linq;
using GameTypes;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000052 RID: 82
	public class DoorAPI
	{
		// Token: 0x06000544 RID: 1348 RVA: 0x000199B8 File Offset: 0x00017BB8
		public DoorAPI(Computer pComputer, TingRunner pTingRunner, RoomRunner pRoomRunner)
		{
			this._computer = pComputer;
			this._tingRunner = pTingRunner;
			this._roomRunner = pRoomRunner;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x000199D8 File Offset: 0x00017BD8
		[SprakAPI(new string[] { "Lock a door", "The name of the door to lock" })]
		public bool API_Lock(string doorName)
		{
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(doorName);
			if (tingUnsafe is Door)
			{
				(tingUnsafe as Door).isLocked = true;
				this._computer.API_Print("'" + doorName + "'");
				this._computer.API_Print("is now locked.");
				return true;
			}
			this._computer.API_Print("Can't find a door named '" + doorName + "'");
			return false;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00019A54 File Offset: 0x00017C54
		[SprakAPI(new string[] { "Unlock a door", "The name of the door to unlock" })]
		public bool API_Unlock(string doorName)
		{
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(doorName);
			if (tingUnsafe is Door)
			{
				(tingUnsafe as Door).isLocked = false;
				this._computer.API_Print("'" + doorName + "'");
				this._computer.API_Print("is now unlocked!");
				return true;
			}
			this._computer.API_Print("Can't find a door named '" + doorName + "'");
			return false;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00019AD0 File Offset: 0x00017CD0
		[SprakAPI(new string[] { "Find the path between two objects" })]
		public object[] API_FindPath(string start, string goal)
		{
			this._computer.API_Sleep(Randomizer.GetValue(1f, 4f));
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(start);
			Ting tingUnsafe2 = this._tingRunner.GetTingUnsafe(goal);
			if (tingUnsafe == null)
			{
				this._computer.API_Print("Can't find start object");
				return new object[0];
			}
			if (tingUnsafe2 == null)
			{
				this._computer.API_Print("Can't find goal object");
				return new object[0];
			}
			MimanPathfinder2 mimanPathfinder = new MimanPathfinder2(this._tingRunner, this._roomRunner);
			MimanPath mimanPath = mimanPathfinder.Search(tingUnsafe, tingUnsafe2);
			return mimanPath.tings.Select((Ting t) => t.room.name).ToArray<string>();
		}

		// Token: 0x0400015B RID: 347
		private Computer _computer;

		// Token: 0x0400015C RID: 348
		private TingRunner _tingRunner;

		// Token: 0x0400015D RID: 349
		private RoomRunner _roomRunner;
	}
}
