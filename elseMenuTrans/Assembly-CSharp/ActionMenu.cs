using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004B RID: 75
public class ActionMenu : MonoBehaviour
{
	// Token: 0x060002D1 RID: 721 RVA: 0x0001346C File Offset: 0x0001166C
	private void Start()
	{
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00013470 File Offset: 0x00011670
	public void ClearButtons()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			global::UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x000134B0 File Offset: 0x000116B0
	public void CreateButton(string pText, float y)
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate(this.actionButtonPrefab) as GameObject;
		Transform transform = gameObject.transform.Find("Background");
		Transform transform2 = transform.transform.Find("Text");
		Text component = transform2.GetComponent<Text>();
		component.text = pText;
		gameObject.transform.parent = this.canvasTransform;
		RectTransform component2 = gameObject.GetComponent<RectTransform>();
		component2.anchoredPosition = new Vector2(0f, -y);
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x0001352C File Offset: 0x0001172C
	public void Update()
	{
		this._easyAnimate.Update(Time.deltaTime);
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00013540 File Offset: 0x00011740
	private void OnGUI()
	{
		GUI.skin = this.skin;
		GUI.color = Color.white * this.alpha;
		Rect rect = new Rect(this._drawerEffect, this.offsetFromTop, this.width, this.height);
		GUI.BeginGroup(rect);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		this._maxWidth = 0f;
		this._maxHeight = 0f;
		foreach (ActionMenuItem actionMenuItem in this.items)
		{
			Vector2 vector = Vector2.zero;
			if (actionMenuItem.type == ActionMenuItemType.SECTION_TITLE)
			{
				GUILayout.Label(actionMenuItem.text, new GUILayoutOption[0]);
				vector = this.skin.label.CalcSize(new GUIContent(actionMenuItem.text));
			}
			else if (actionMenuItem.type == ActionMenuItemType.ACTION)
			{
				if (GUILayout.Button(actionMenuItem.text, new GUILayoutOption[0]) && this.onActionMenuPressed != null)
				{
					this.onActionMenuPressed(actionMenuItem.identifier);
				}
				vector = this.skin.button.CalcSize(new GUIContent(actionMenuItem.text));
				this._maxHeight += 5f;
			}
			else if (actionMenuItem.type == ActionMenuItemType.VERTICAL_SPACE)
			{
				GUILayout.Space(10f);
				vector = new Vector2(0f, 10f);
			}
			this._maxWidth = Mathf.Max(this._maxWidth, vector.x);
			this._maxHeight += vector.y;
		}
		GUILayout.EndVertical();
		GUI.EndGroup();
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x000136E8 File Offset: 0x000118E8
	public Rect GetBounds()
	{
		Rect rect = new Rect(0f, this.offsetFromTop, this._maxWidth, this._maxHeight);
		return rect;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00013714 File Offset: 0x00011914
	private void Fade(float pStart, float pEnd)
	{
		this._easyAnimate.Register(this, "actionMenuFade", new EasyAnimState<float>(0.5f, pStart, pEnd, new EasyAnimState<float>.InterpolationSampler(iTween.easeInCubic), delegate(float o)
		{
			this.alpha = o;
		}));
		this._easyAnimate.Register(this, "actionMenuDrawer", new EasyAnimState<float>(0.5f, (pStart >= 0.1f) ? 0f : (-300f), (pEnd >= 0.1f) ? 0f : (-300f), new EasyAnimState<float>.InterpolationSampler(iTween.easeInCubic), delegate(float o)
		{
			this._drawerEffect = o;
		}));
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x000137C0 File Offset: 0x000119C0
	public void FadeOut()
	{
		Debug.Log("Fade out action menu");
		this.Fade(1f, 0f);
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x000137DC File Offset: 0x000119DC
	public void FadeIn()
	{
		Debug.Log("Fade in action menu");
		this.Fade(0f, 1f);
	}

	// Token: 0x060002DA RID: 730 RVA: 0x000137F8 File Offset: 0x000119F8
	public void Hide()
	{
		Debug.Log("Hide action menu");
		this.alpha = 0f;
		this._drawerEffect = -300f;
	}

	// Token: 0x040001B8 RID: 440
	public ActionMenuItem[] items = new ActionMenuItem[]
	{
		new ActionMenuItem
		{
			text = "Beer",
			type = ActionMenuItemType.SECTION_TITLE
		},
		new ActionMenuItem
		{
			text = "Use"
		},
		new ActionMenuItem
		{
			text = "Drop"
		}
	};

	// Token: 0x040001B9 RID: 441
	public GUISkin skin;

	// Token: 0x040001BA RID: 442
	public float offsetFromTop = 30f;

	// Token: 0x040001BB RID: 443
	public float width = 200f;

	// Token: 0x040001BC RID: 444
	public float height = 1000f;

	// Token: 0x040001BD RID: 445
	public float alpha = 1f;

	// Token: 0x040001BE RID: 446
	public Transform canvasTransform;

	// Token: 0x040001BF RID: 447
	public GameObject actionButtonPrefab;

	// Token: 0x040001C0 RID: 448
	private float _maxWidth;

	// Token: 0x040001C1 RID: 449
	private float _maxHeight;

	// Token: 0x040001C2 RID: 450
	private EasyAnimateTwo _easyAnimate = new EasyAnimateTwo();

	// Token: 0x040001C3 RID: 451
	private float _drawerEffect;

	// Token: 0x040001C4 RID: 452
	public ActionMenu.OnActionMenuButtonPressed onActionMenuPressed;

	// Token: 0x020000FF RID: 255
	// (Invoke) Token: 0x0600075B RID: 1883
	public delegate void OnActionMenuButtonPressed(string pIdentifier);
}
