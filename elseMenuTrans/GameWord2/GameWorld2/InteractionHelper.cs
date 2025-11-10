using System;
using System.Collections.Generic;
using System.Linq;
using GameTypes;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000042 RID: 66
	public class InteractionHelper
	{
		// Token: 0x06000488 RID: 1160 RVA: 0x00016FF0 File Offset: 0x000151F0
		public static TingType GetRandomTing<TingType>(MimanTingRunner pTingRunner, string pRoomName, int pTileGroup) where TingType : Ting
		{
			TingType[] tingsOfTypeInRoom = pTingRunner.GetTingsOfTypeInRoom<TingType>(pRoomName);
			int i = tingsOfTypeInRoom.Length;
			int num = Randomizer.GetIntValue(0, tingsOfTypeInRoom.Length);
			while (i > 0)
			{
				TingType tingType = tingsOfTypeInRoom[num];
				if (!tingType.isBeingHeld && (pTileGroup == -1 || tingType.tile.group == pTileGroup))
				{
					return tingType;
				}
				num++;
				if (num == tingsOfTypeInRoom.Length)
				{
					num = 0;
				}
				i--;
			}
			return (TingType)((object)null);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00017074 File Offset: 0x00015274
		public static TingType GetAnyRandomTing<TingType>(MimanTingRunner pTingRunner, string pRoomName, int pTileGroup) where TingType : Ting
		{
			TingType[] tingsOfTypeInRoom = pTingRunner.GetTingsOfTypeInRoom<TingType>(pRoomName);
			int i = tingsOfTypeInRoom.Length;
			int num = Randomizer.GetIntValue(0, tingsOfTypeInRoom.Length);
			while (i > 0)
			{
				TingType tingType = tingsOfTypeInRoom[num];
				if (!tingType.isBeingHeld && tingType.tile.group == pTileGroup)
				{
					return tingType;
				}
				num++;
				if (num == tingsOfTypeInRoom.Length)
				{
					num = 0;
				}
				i--;
			}
			return (TingType)((object)null);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000170F0 File Offset: 0x000152F0
		public static TingType GetRandomTingWhere<TingType>(RoomRunner pRoomRunner, string pRoomName, Predicate<TingType> pPredicate) where TingType : Ting
		{
			Room room = pRoomRunner.GetRoom(pRoomName);
			List<TingType> list = room.GetTingsOfType<TingType>().FindAll(pPredicate);
			if (list.Count > 0)
			{
				return Randomizer.RandNth<TingType>(list);
			}
			return (TingType)((object)null);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001712C File Offset: 0x0001532C
		public static Drink GetClosestDrink(Ting pCharacter, Room pRoom, string pLiquidType)
		{
			List<Drink> list = (from d in pRoom.GetTingsOfType<Drink>()
				where d.liquidType == pLiquidType && d.amount >= 100f
				select d).ToList<Drink>();
			list.Sort(delegate(Drink a, Drink b)
			{
				float num = pCharacter.localPoint.DistanceTo(a.localPoint);
				float num2 = pCharacter.localPoint.DistanceTo(b.localPoint);
				return (int)(num - num2);
			});
			if (list.Count == 0)
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00017190 File Offset: 0x00015390
		public static void GoToRoom(Character pCharacter, RoomRunner pRoomRunner, string pRoomName, MimanTingRunner pTingRunner)
		{
			if (pCharacter.finalTargetPosition.roomName == pRoomName)
			{
				return;
			}
			Ting randomTing = InteractionHelper.GetRandomTing<Point>(pTingRunner, pRoomName, -1);
			if (randomTing == null)
			{
				pCharacter.logger.Log("No target Point found in " + pRoomName);
				return;
			}
			pCharacter.WalkToTingAndInteract(randomTing);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000171E4 File Offset: 0x000153E4
		public void Reset()
		{
		}
	}
}
