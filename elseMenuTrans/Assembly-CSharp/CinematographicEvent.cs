using System;

// Token: 0x020000A6 RID: 166
public struct CinematographicEvent : BaseEvent
{
	// Token: 0x060004AB RID: 1195 RVA: 0x000202AC File Offset: 0x0001E4AC
	public CinematographicEvent(CinematographicEvent.CinematographicEventType pEventType)
	{
		this._eventType = pEventType;
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060004AC RID: 1196 RVA: 0x000202B8 File Offset: 0x0001E4B8
	public CinematographicEvent.CinematographicEventType eventType
	{
		get
		{
			return this._eventType;
		}
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x000202C0 File Offset: 0x0001E4C0
	public override string ToString()
	{
		return "CinematographicEvent[ type: " + this.eventType.ToString() + " ]";
	}

	// Token: 0x0400038C RID: 908
	private CinematographicEvent.CinematographicEventType _eventType;

	// Token: 0x020000A7 RID: 167
	public enum CinematographicEventType
	{
		// Token: 0x0400038E RID: 910
		PLAYER_FELL_ASLEEP,
		// Token: 0x0400038F RID: 911
		PLAYER_WOKE_UP
	}
}
