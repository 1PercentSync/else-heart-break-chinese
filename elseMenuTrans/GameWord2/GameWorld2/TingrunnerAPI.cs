using System;
using System.Collections.Generic;
using System.Linq;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000056 RID: 86
	public class TingrunnerAPI
	{
		// Token: 0x06000558 RID: 1368 RVA: 0x00019FD4 File Offset: 0x000181D4
		public TingrunnerAPI(Computer pComputer, TingRunner pTingRunner, RoomRunner pRoomRunner)
		{
			this._computer = pComputer;
			this._tingRunner = pTingRunner;
			this._roomRunner = pRoomRunner;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00019FF4 File Offset: 0x000181F4
		[SprakAPI(new string[] { "" })]
		public object[] API_GetPeople()
		{
			List<string> list = new List<string>();
			foreach (Character character in this._tingRunner.GetTingsOfType<Character>())
			{
				list.Add(character.name);
			}
			return list.ToArray();
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001A040 File Offset: 0x00018240
		[SprakAPI(new string[] { "" })]
		public object[] API_GetThingsOfType(string type)
		{
			string text = type.ToLower();
			Ting[] array;
			if (text == "bed")
			{
				array = this._tingRunner.GetTingsOfType<Bed>();
			}
			else if (text == "creditcard")
			{
				array = this._tingRunner.GetTingsOfType<CreditCard>();
			}
			else if (text == "door")
			{
				array = this._tingRunner.GetTingsOfType<Door>();
			}
			else if (text == "drink")
			{
				array = this._tingRunner.GetTingsOfType<Drink>();
			}
			else if (text == "extractor")
			{
				array = this._tingRunner.GetTingsOfType<Extractor>();
			}
			else if (text == "fence")
			{
				array = this._tingRunner.GetTingsOfType<Fence>();
			}
			else if (text == "floppy")
			{
				array = this._tingRunner.GetTingsOfType<Floppy>();
			}
			else if (text == "fountain")
			{
				array = this._tingRunner.GetTingsOfType<Fountain>();
			}
			else if (text == "fryingpan")
			{
				array = this._tingRunner.GetTingsOfType<FryingPan>();
			}
			else if (text == "fusebox")
			{
				array = this._tingRunner.GetTingsOfType<FuseBox>();
			}
			else if (text == "goods")
			{
				array = this._tingRunner.GetTingsOfType<Goods>();
			}
			else if (text == "modifier" || text == "hackdev")
			{
				array = this._tingRunner.GetTingsOfType<Hackdev>();
			}
			else if (text == "key")
			{
				array = this._tingRunner.GetTingsOfType<Key>();
			}
			else if (text == "lamp")
			{
				array = this._tingRunner.GetTingsOfType<Lamp>();
			}
			else if (text == "locker")
			{
				array = this._tingRunner.GetTingsOfType<Locker>();
			}
			else if (text == "machine")
			{
				array = this._tingRunner.GetTingsOfType<Machine>();
			}
			else if (text == "map")
			{
				array = this._tingRunner.GetTingsOfType<Map>();
			}
			else if (text == "memory")
			{
				array = this._tingRunner.GetTingsOfType<Memory>();
			}
			else if (text == "musicbox")
			{
				array = this._tingRunner.GetTingsOfType<MusicBox>();
			}
			else if (text == "mysticalcube")
			{
				array = this._tingRunner.GetTingsOfType<MysticalCube>();
			}
			else if (text == "pawn")
			{
				array = this._tingRunner.GetTingsOfType<Pawn>();
			}
			else if (text == "point")
			{
				array = this._tingRunner.GetTingsOfType<Point>();
			}
			else if (text == "portal")
			{
				array = this._tingRunner.GetTingsOfType<Portal>();
			}
			else if (text == "radio")
			{
				array = this._tingRunner.GetTingsOfType<Radio>();
			}
			else if (text == "screwdriver")
			{
				array = this._tingRunner.GetTingsOfType<Screwdriver>();
			}
			else if (text == "seat")
			{
				array = this._tingRunner.GetTingsOfType<Seat>();
			}
			else if (text == "sendpipe")
			{
				array = this._tingRunner.GetTingsOfType<SendPipe>();
			}
			else if (text == "sink")
			{
				array = this._tingRunner.GetTingsOfType<Sink>();
			}
			else if (text == "snus")
			{
				array = this._tingRunner.GetTingsOfType<Snus>();
			}
			else if (text == "suitcase")
			{
				array = this._tingRunner.GetTingsOfType<Suitcase>();
			}
			else if (text == "taser")
			{
				array = this._tingRunner.GetTingsOfType<Taser>();
			}
			else if (text == "teleporter")
			{
				array = this._tingRunner.GetTingsOfType<Teleporter>();
			}
			else if (text == "tram")
			{
				array = this._tingRunner.GetTingsOfType<Tram>();
			}
			else if (text == "trashcan")
			{
				array = this._tingRunner.GetTingsOfType<TrashCan>();
			}
			else if (text == "tv")
			{
				array = this._tingRunner.GetTingsOfType<Tv>();
			}
			else
			{
				if (!(text == "vendingmachine"))
				{
					this._computer.API_Print("Can't get tings of type " + type);
					return new object[0];
				}
				array = this._tingRunner.GetTingsOfType<VendingMachine>();
			}
			List<string> list = new List<string>();
			foreach (Ting ting in array)
			{
				list.Add(ting.name);
			}
			return list.ToArray();
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001A560 File Offset: 0x00018760
		[SprakAPI(new string[] { "" })]
		public string API_GetPosition(string name)
		{
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(name);
			if (tingUnsafe != null)
			{
				return string.Concat(new object[]
				{
					"Room: ",
					tingUnsafe.position.roomName,
					", coordinate: ",
					tingUnsafe.position.localPosition
				});
			}
			return "Thing not found";
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001A5C8 File Offset: 0x000187C8
		[SprakAPI(new string[] { "" })]
		public string API_GetAction(string name)
		{
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(name);
			if (tingUnsafe != null)
			{
				return tingUnsafe.actionName;
			}
			return "Thing not found";
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001A5F4 File Offset: 0x000187F4
		[SprakAPI(new string[] { "" })]
		public string API_GetRoom(string name)
		{
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(name);
			if (tingUnsafe != null)
			{
				return tingUnsafe.position.roomName;
			}
			return "";
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001A628 File Offset: 0x00018828
		[SprakAPI(new string[] { "" })]
		public void API_SetPosition(string name, string targetThing)
		{
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(name);
			Ting tingUnsafe2 = this._tingRunner.GetTingUnsafe(targetThing);
			if (tingUnsafe == null)
			{
				this._computer.API_Print(name + " not found");
				return;
			}
			if (tingUnsafe is Character && tingUnsafe.name != "PlayWife")
			{
				this._computer.API_Print("Impossible to move " + tingUnsafe.name);
				return;
			}
			if (tingUnsafe2 == null)
			{
				this._computer.API_Print(targetThing + " not found");
				return;
			}
			try
			{
				tingUnsafe.position = tingUnsafe2.position;
			}
			catch (Exception)
			{
				throw new Error("Can't move " + tingUnsafe.name + " to position of " + tingUnsafe2.name);
			}
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001A71C File Offset: 0x0001891C
		[SprakAPI(new string[] { "" })]
		public void API_InteractWith(string name, string target)
		{
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(name);
			Ting tingUnsafe2 = this._tingRunner.GetTingUnsafe(target);
			if (tingUnsafe == null)
			{
				this._computer.API_Print(name + " not found");
				return;
			}
			if (target == null)
			{
				this._computer.API_Print(target + " not found");
				return;
			}
			Character character = tingUnsafe as Character;
			if (character == null)
			{
				this._computer.API_Print(tingUnsafe2 + " is not a character");
				return;
			}
			character.WalkToTingAndInteract(tingUnsafe2);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001A7A8 File Offset: 0x000189A8
		[SprakAPI(new string[] { "" })]
		public object[] API_GetThingsInRoom(string roomName)
		{
			Room roomUnsafe = this._roomRunner.GetRoomUnsafe(roomName);
			if (roomUnsafe == null)
			{
				this._computer.API_Print(roomName + " not found");
				return new string[0];
			}
			return (from t in roomUnsafe.GetTings()
				select t.name).ToArray<string>();
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001A814 File Offset: 0x00018A14
		[SprakAPI(new string[] { "" })]
		public object[] API_GetAllRooms()
		{
			List<string> list = new List<string>();
			foreach (Room room in this._roomRunner.rooms)
			{
				list.Add(room.name);
			}
			return list.ToArray();
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001A890 File Offset: 0x00018A90
		[SprakAPI(new string[] { "Get the type of a thing" })]
		public string API_GetTypeOfThing(string name)
		{
			Ting tingUnsafe = this._tingRunner.GetTingUnsafe(name);
			if (tingUnsafe == null)
			{
				this._computer.Say("No such thing", "");
				return "";
			}
			string text = tingUnsafe.GetType().Name.ToLower();
			if (text == "hackdev")
			{
				text = "modifier";
			}
			return text;
		}

		// Token: 0x04000164 RID: 356
		private Computer _computer;

		// Token: 0x04000165 RID: 357
		private TingRunner _tingRunner;

		// Token: 0x04000166 RID: 358
		private RoomRunner _roomRunner;
	}
}
