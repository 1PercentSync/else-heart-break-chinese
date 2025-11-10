using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000007 RID: 7
public class BubbleTester : MonoBehaviour
{
	// Token: 0x06000017 RID: 23 RVA: 0x00002814 File Offset: 0x00000A14
	private void Start()
	{
		MainMenu.SetVolume(1f);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002820 File Offset: 0x00000A20
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Bubble bubble = this.bubbleCanvasController.CreateBubble(false, this.a, "Och då sa hon att jag skulle följa med på efterfest men jag vet inte om jag hinner.", BubbleType.TALK, null, 300f, "Sebastian");
			bubble.bubbleOrientation = BubbleOrientation.TOP_LEFT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Bubble bubble2 = this.bubbleCanvasController.CreateBubble(false, this.a, "Vad gör du här i staden?", BubbleType.TALK, null, 300f, "Pixie");
			bubble2.bubbleOrientation = BubbleOrientation.TOP_LEFT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Bubble bubble3 = this.bubbleCanvasController.CreateBubble(false, this.a, "Du dansar bra förresten", BubbleType.TALK, null, 300f, "Gunnar");
			bubble3.bubbleOrientation = BubbleOrientation.TOP_LEFT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			Bubble bubble4 = this.bubbleCanvasController.CreateBubble(true, this.a, "YO!", BubbleType.TALK, null, 300f, "Yulian");
			bubble4.bubbleOrientation = BubbleOrientation.TOP_LEFT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			Bubble bubble5 = this.bubbleCanvasController.CreateBubble(true, this.a, "Och då sa hon att jag skulle följa med på efterfest men jag vet inte om jag hinner.", BubbleType.TALK, null, 300f, "Siri");
			bubble5.bubbleOrientation = BubbleOrientation.TOP_LEFT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			Bubble bubble6 = this.bubbleCanvasController.CreateBubble(false, this.a, "Jag är trädgårdsmästare", BubbleType.TALK, null, 300f, "Nini");
			bubble6.bubbleOrientation = BubbleOrientation.TOP_LEFT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			Bubble bubble7 = this.bubbleCanvasController.CreateBubble(false, this.a, "All-inclusive?", BubbleType.TALK, null, 300f, "Bernd");
			bubble7.bubbleOrientation = BubbleOrientation.TOP_LEFT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			Bubble bubble8 = this.bubbleCanvasController.CreateBubble(false, this.a, "All-inclusive?!", BubbleType.TALK, null, 300f, "Babcia");
			bubble8.bubbleOrientation = BubbleOrientation.TOP_LEFT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			Bubble bubble9 = this.bubbleCanvasController.CreateBubble(false, this.a, "Fantastic", BubbleType.TALK, null, 300f, "Fib");
			bubble9.bubbleOrientation = BubbleOrientation.TOP_LEFT;
		}
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			Bubble bubble10 = this.bubbleCanvasController.CreateBubble(false, this.b, "Tomorrow!", BubbleType.TALK, null, 300f, "Monad");
			bubble10.bubbleOrientation = BubbleOrientation.TOP_LEFT;
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			this.bubbleCanvasController.CreateBubble(false, this.b, "Vem är du? Jag heter kanske Sebastian!", BubbleType.TALK, delegate
			{
				Debug.Log("HAJ");
			}, 2f, "Panda1");
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			this.bubbleCanvasController.CreateBubble(true, this.c, "Blaha... ok? Näää, men öh. Vad sa du? Jo jo, så är det. Kanske.", BubbleType.TALK, null, 4f, "Sebastian");
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			this.bubbleCanvasController.CreateBubble(false, null, "Sebastian", BubbleType.THINK, new UnityAction(this.SayWhat), 0f, "Sebastian");
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002B14 File Offset: 0x00000D14
	private void SayWhat()
	{
		this.bubbleCanvasController.CreateBubble(false, this.a, "Och då sa hon att jag skulle följa med på efterfest men jag vet inte om jag hinner.", BubbleType.TALK, null, 3f, "Petra");
		this.bubbleCanvasController.ClearThoughtBubbles();
	}

	// Token: 0x04000016 RID: 22
	public BubbleCanvasController bubbleCanvasController;

	// Token: 0x04000017 RID: 23
	public Transform a;

	// Token: 0x04000018 RID: 24
	public Transform b;

	// Token: 0x04000019 RID: 25
	public Transform c;
}
