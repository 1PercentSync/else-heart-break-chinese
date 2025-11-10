using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class Bubble : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000002 RID: 2 RVA: 0x000020F4 File Offset: 0x000002F4
	public RectTransform rectTransform
	{
		get
		{
			return this._rectTransform;
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000020FC File Offset: 0x000002FC
	private void Start()
	{
		this._animator = base.GetComponent<Animator>();
		this._rectTransform = base.GetComponent<RectTransform>();
		this._arrowTransform = base.transform.Find("Arrow").GetComponent<RectTransform>();
		SoundDictionary.LoadMultiSound("BubbleAppear", "TextBubble Sound", 5);
		SoundDictionary.LoadMultiSound("BubbleClick", "MuseClick sound", 4);
		this._audioSource = base.GetComponent<AudioSource>();
		if (this._audioSource == null)
		{
			Debug.Log("The bubble " + base.name + " has no audio source");
		}
		if (this.bubbleType == BubbleType.TALK)
		{
			this.Talk();
		}
		else if (this.bubbleType == BubbleType.THINK)
		{
			this._animator.Play("HiddenThinkingBubble");
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000021C8 File Offset: 0x000003C8
	public void FollowTransform(Transform pFollowThis)
	{
		this._followThis = pFollowThis;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000021D4 File Offset: 0x000003D4
	private void DecideBubbleOrientation(BubbleOrientation pBubbleOrientation)
	{
		if (this.bubbleOrientation == BubbleOrientation.UNDECIDED)
		{
			this.bubbleOrientation = pBubbleOrientation;
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000021F0 File Offset: 0x000003F0
	private void Update()
	{
		if (this._followThis != null)
		{
			Vector3 vector = Camera.main.WorldToScreenPoint(this._followThis.position);
			float num = (float)Screen.width / 2f;
			float num2 = (float)Screen.height / 2f;
			float num3 = vector.x - num;
			float num4 = vector.y - num2;
			float num5 = 200f;
			float num6 = 100f;
			float num7 = Mathf.Clamp(num4, -num2 + num6, num2 - num5);
			float num8 = 300f;
			float num9 = num3;
			if (num3 < -num + num8)
			{
				this.DecideBubbleOrientation(BubbleOrientation.TOP_RIGHT);
			}
			else if (num3 > num - num8)
			{
				this.DecideBubbleOrientation(BubbleOrientation.TOP_LEFT);
			}
			else
			{
				this.DecideBubbleOrientation((num3 >= 0f) ? BubbleOrientation.TOP_RIGHT : BubbleOrientation.TOP_LEFT);
			}
			if (num3 < -num + num8)
			{
				num9 = -num + num8;
			}
			else if (num3 > num - num8)
			{
				num9 = num - num8;
			}
			float num10 = ((this.bubbleOrientation != BubbleOrientation.TOP_RIGHT) ? (-120f) : 120f);
			float num11 = 120f;
			this._rectTransform.anchoredPosition = new Vector2(num9 + num10, num7 + num11);
			this._arrowTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, (this.bubbleOrientation != BubbleOrientation.TOP_RIGHT) ? 225f : 135f));
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002364 File Offset: 0x00000564
	public void OnHoverBringForward()
	{
		base.transform.SetAsLastSibling();
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002374 File Offset: 0x00000574
	public void DestroyIn(float pLifeTime)
	{
		base.StartCoroutine("DelayedDestroy", pLifeTime);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002388 File Offset: 0x00000588
	private IEnumerator DelayedDestroy(float pLifeTime)
	{
		yield return new WaitForSeconds(pLifeTime);
		global::UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000023B4 File Offset: 0x000005B4
	private void PlayClickSound()
	{
		SoundDictionary.PlaySound("BubbleClick", this._audioSource);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000023C8 File Offset: 0x000005C8
	private void PlayTalkSound()
	{
		SoundDictionary.PlaySound("BubbleAppear", this._audioSource);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000023DC File Offset: 0x000005DC
	public void Think()
	{
		this._animator.SetTrigger("Think");
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000023F0 File Offset: 0x000005F0
	public void Talk()
	{
		this._animator.SetTrigger("Talk");
		this.PlayTalkSound();
	}

	// Token: 0x04000005 RID: 5
	public BubbleType bubbleType;

	// Token: 0x04000006 RID: 6
	public BubbleOrientation bubbleOrientation;

	// Token: 0x04000007 RID: 7
	private Animator _animator;

	// Token: 0x04000008 RID: 8
	private Transform _followThis;

	// Token: 0x04000009 RID: 9
	private RectTransform _rectTransform;

	// Token: 0x0400000A RID: 10
	private RectTransform _arrowTransform;

	// Token: 0x0400000B RID: 11
	private AudioSource _audioSource;
}
