using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class NotificationsPane : MonoBehaviour
{
	// Token: 0x060003A7 RID: 935 RVA: 0x0001A884 File Offset: 0x00018A84
	private void OnGUI()
	{
		GUI.skin = this.skin;
		Rect rect = new Rect((float)Screen.width - this.width, this._y, this.width, this.height);
		GUI.BeginGroup(rect);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(new GUIContent(this._message, this.icon), new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUI.EndGroup();
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0001A8FC File Offset: 0x00018AFC
	private void Start()
	{
		this._easyAnimator = new EasyAnimateTwo();
		this._notificationSoundSource = base.transform.GetComponent<AudioSource>();
		if (this._notificationSoundSource == null)
		{
			Debug.Log("_notificationSoundSource is null in " + base.name);
		}
		SoundDictionary.LoadSingleSound("Notify", "NotificationBannerSound");
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0001A95C File Offset: 0x00018B5C
	private void Update()
	{
		this._easyAnimator.Update(Time.deltaTime);
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0001A970 File Offset: 0x00018B70
	public void ShowNotification(string pMessage)
	{
		this.ShowNotification(pMessage, null);
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0001A97C File Offset: 0x00018B7C
	public void ShowNotification(string pMessage, Function pOnShowAction)
	{
		this._message = " " + pMessage;
		EasyAnimState<float> easyAnimState = new EasyAnimState<float>(this.slideTime, this._y, this.offsetFromTop, new EasyAnimState<float>.InterpolationSampler(iTween.easeInQuad), delegate(float o)
		{
			this._y = o;
		}, pOnShowAction);
		EasyAnimState<float> easyAnimState2 = easyAnimState.Then(new EasyAnimState<float>(this.displayTime, this.offsetFromTop, this.offsetFromTop, new EasyAnimState<float>.InterpolationSampler(iTween.easeInQuad), delegate(float o)
		{
			this._y = o;
		}, null));
		easyAnimState2.Then(new EasyAnimState<float>(this.slideTime, this.offsetFromTop, -50f, new EasyAnimState<float>.InterpolationSampler(iTween.easeInQuad), delegate(float o)
		{
			this._y = o;
		}, null));
		this._easyAnimator.Register(this, "notificationsPane", easyAnimState);
		if (!this._notificationSoundSource.isPlaying)
		{
			SoundDictionary.PlaySound("Notify", this._notificationSoundSource);
		}
	}

	// Token: 0x040002C0 RID: 704
	public GUISkin skin;

	// Token: 0x040002C1 RID: 705
	public float offsetFromTop = 30f;

	// Token: 0x040002C2 RID: 706
	public float width = 200f;

	// Token: 0x040002C3 RID: 707
	public float height = 600f;

	// Token: 0x040002C4 RID: 708
	public Texture icon;

	// Token: 0x040002C5 RID: 709
	public float slideTime = 0.3f;

	// Token: 0x040002C6 RID: 710
	public float displayTime = 3f;

	// Token: 0x040002C7 RID: 711
	private float _y = -50f;

	// Token: 0x040002C8 RID: 712
	private string _message;

	// Token: 0x040002C9 RID: 713
	private EasyAnimateTwo _easyAnimator;

	// Token: 0x040002CA RID: 714
	private AudioSource _notificationSoundSource;
}
