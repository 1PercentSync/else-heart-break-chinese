using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class OnMouseOver : MonoBehaviour
{
	// Token: 0x06000307 RID: 775 RVA: 0x000177C4 File Offset: 0x000159C4
	public void OnEnable()
	{
		if ((this.what & OnMouseOver.Action.TOGGLE_CHILDREN) != OnMouseOver.Action.NONE)
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(!this.boolStateOnHover);
			}
		}
		if ((this.what & OnMouseOver.Action.TOGGLE_RENDERER) != OnMouseOver.Action.NONE && base.renderer != null)
		{
			base.renderer.enabled = !this.boolStateOnHover;
		}
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00017880 File Offset: 0x00015A80
	public void OnDisable()
	{
		if ((this.what & OnMouseOver.Action.TOGGLE_CHILDREN) != OnMouseOver.Action.NONE)
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(!this.boolStateOnHover);
			}
		}
		if ((this.what & OnMouseOver.Action.TOGGLE_RENDERER) != OnMouseOver.Action.NONE && base.renderer != null)
		{
			base.renderer.enabled = !this.boolStateOnHover;
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0001793C File Offset: 0x00015B3C
	public void OnMouseEnter()
	{
		if ((this.what & OnMouseOver.Action.TOGGLE_CHILDREN) != OnMouseOver.Action.NONE)
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(this.boolStateOnHover);
			}
		}
		if ((this.what & OnMouseOver.Action.TOGGLE_RENDERER) != OnMouseOver.Action.NONE && base.renderer != null)
		{
			base.renderer.enabled = this.boolStateOnHover;
		}
	}

	// Token: 0x0600030A RID: 778 RVA: 0x000179F4 File Offset: 0x00015BF4
	public void OnMouseExit()
	{
		if ((this.what & OnMouseOver.Action.TOGGLE_CHILDREN) != OnMouseOver.Action.NONE)
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(!this.boolStateOnHover);
			}
		}
		if ((this.what & OnMouseOver.Action.TOGGLE_RENDERER) != OnMouseOver.Action.NONE && base.renderer != null)
		{
			base.renderer.enabled = !this.boolStateOnHover;
		}
	}

	// Token: 0x04000223 RID: 547
	public OnMouseOver.Action what;

	// Token: 0x04000224 RID: 548
	public bool boolStateOnHover;

	// Token: 0x02000058 RID: 88
	[Flags]
	public enum Action
	{
		// Token: 0x04000226 RID: 550
		NONE = 0,
		// Token: 0x04000227 RID: 551
		TOGGLE_CHILDREN = 1,
		// Token: 0x04000228 RID: 552
		TOGGLE_RENDERER = 2
	}
}
