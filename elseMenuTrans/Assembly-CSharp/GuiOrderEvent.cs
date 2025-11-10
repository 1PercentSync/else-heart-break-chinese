using System;

// Token: 0x020000A2 RID: 162
public struct GuiOrderEvent : BaseEvent
{
	// Token: 0x060004A6 RID: 1190 RVA: 0x0002021C File Offset: 0x0001E41C
	public GuiOrderEvent(GuiOrderEvent.ShellEventType pEventType)
	{
		this.eventType = pEventType;
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00020228 File Offset: 0x0001E428
	public override string ToString()
	{
		return "GuiOrderEvent[ type: " + this.eventType.ToString() + " ]";
	}

	// Token: 0x04000383 RID: 899
	public GuiOrderEvent.ShellEventType eventType;

	// Token: 0x020000A3 RID: 163
	public enum ShellEventType
	{
		// Token: 0x04000385 RID: 901
		START_HACK_DEVICE,
		// Token: 0x04000386 RID: 902
		QUIT_HACK_DEVICE,
		// Token: 0x04000387 RID: 903
		GAME_LOADED,
		// Token: 0x04000388 RID: 904
		QUIT_COMPUTER,
		// Token: 0x04000389 RID: 905
		QUIT_PANEL
	}
}
