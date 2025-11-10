using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000006 RID: 6
public class BubbleCanvasController : MonoBehaviour
{
	// Token: 0x06000010 RID: 16 RVA: 0x00002450 File Offset: 0x00000650
	private void Start()
	{
		this._thoughtBubbles = new List<Bubble>();
		this.r = global::UnityEngine.Random.Range(1f, 2f);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002480 File Offset: 0x00000680
	public void ClearThoughtBubbles()
	{
		foreach (Bubble bubble in this._thoughtBubbles)
		{
			global::UnityEngine.Object.Destroy(bubble.gameObject);
		}
		this._thoughtBubbles.Clear();
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000024F8 File Offset: 0x000006F8
	public void RemoveAllBubbles()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			global::UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002538 File Offset: 0x00000738
	public Bubble CreateBubble(bool pDigital, Transform pTransform, string pText, BubbleType pBubbleType, UnityAction pAction, float pLifeTime, string pCharacterName)
	{
		GameObject gameObject;
		if (pDigital)
		{
			gameObject = global::UnityEngine.Object.Instantiate(this.digitalBubblePrefab) as GameObject;
		}
		else
		{
			gameObject = global::UnityEngine.Object.Instantiate(this.bubblePrefab) as GameObject;
		}
		gameObject.transform.SetParent(base.transform, false);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.localPosition = new Vector2(-9999f, -9999f);
		Transform transform = gameObject.transform.Find("Text");
		Text component2 = transform.GetComponent<Text>();
		component2.text = pText;
		if (!pDigital)
		{
			component2.color = this.GetTextColorForCharacter(pCharacterName);
		}
		Transform transform2 = gameObject.transform.Find("Button");
		Button component3 = transform2.GetComponent<Button>();
		float num = (float)pText.Length * 7f;
		float num2 = 0f;
		RectTransform component4 = gameObject.GetComponent<RectTransform>();
		component4.sizeDelta += new Vector2(num, num2);
		Bubble component5 = gameObject.GetComponent<Bubble>();
		component5.bubbleType = pBubbleType;
		if (pBubbleType == BubbleType.TALK)
		{
			component5.DestroyIn(pLifeTime);
			component5.FollowTransform(pTransform);
		}
		else if (pBubbleType == BubbleType.THINK)
		{
			this._thoughtBubbles.Add(component5);
		}
		if (pAction != null)
		{
			component3.onClick.AddListener(pAction);
		}
		return component5;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002688 File Offset: 0x00000888
	private Color GetTextColorForCharacter(string pCharacterName)
	{
		if (pCharacterName == "Option")
		{
			return new Color(0.25f, 0.25f, 0.25f);
		}
		foreach (BubbleTextColorForCharacter bubbleTextColorForCharacter in this.bubbleColorsForCharacters)
		{
			if (bubbleTextColorForCharacter.characterName == pCharacterName)
			{
				return bubbleTextColorForCharacter.bubbleTextColor;
			}
		}
		return Color.black;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000026F8 File Offset: 0x000008F8
	private void Update()
	{
		float num = (float)this._thoughtBubbles.Count;
		float num2 = (float)Screen.width;
		float num3 = num2 / 2f;
		float num4 = num2 / (num + 1f);
		float num5 = (float)Screen.height;
		float num6 = num5 / 2f;
		float num7 = 90f;
		float num8 = 1f;
		foreach (Bubble bubble in this._thoughtBubbles)
		{
			float num9 = -num3 + num4 * num8 + Mathf.Cos(num8 + Time.time * 0.1f) * 15f;
			float num10 = -num6 + num7 + Mathf.Sin(num8 + Time.time * 0.25f * this.r) * 20f;
			bubble.rectTransform.anchoredPosition = new Vector2(num9, num10);
			num8 += 1f;
		}
	}

	// Token: 0x04000011 RID: 17
	public GameObject bubblePrefab;

	// Token: 0x04000012 RID: 18
	public GameObject digitalBubblePrefab;

	// Token: 0x04000013 RID: 19
	private List<Bubble> _thoughtBubbles;

	// Token: 0x04000014 RID: 20
	private float r;

	// Token: 0x04000015 RID: 21
	public BubbleTextColorForCharacter[] bubbleColorsForCharacters;
}
