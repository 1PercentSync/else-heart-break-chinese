using System;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class HideGroup : MonoBehaviour
{
	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000357 RID: 855 RVA: 0x00018F8C File Offset: 0x0001718C
	public HideGroup.State currentState
	{
		get
		{
			return this._currentState;
		}
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00018F94 File Offset: 0x00017194
	private void Start()
	{
		this._camera = Camera.main;
		this._currentState = ((!this.ShouldShow) ? HideGroup.State.HIDING : HideGroup.State.SHOWING);
		HideGroup.EnsureDefaultShader();
		HideGroup.EnsureOnlyShadowShader();
		if (this._currentState == HideGroup.State.SHOWING)
		{
			this.ShowWithNoFade();
		}
		else
		{
			this.HideWithNoFade();
		}
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00018FEC File Offset: 0x000171EC
	private static void EnsureDefaultShader()
	{
		if (HideGroup._defaultShader == null)
		{
			HideGroup._defaultShader = Shader.Find("Diffuse");
		}
		if (HideGroup._defaultShader == null)
		{
			throw new Exception("Failed to load diffuse shader");
		}
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00019034 File Offset: 0x00017234
	private static void EnsureOnlyShadowShader()
	{
		if (HideGroup._onlyShadowShader == null)
		{
			HideGroup._onlyShadowShader = (Shader)Resources.Load("OnlyShadow");
		}
		if (HideGroup._onlyShadowShader == null)
		{
			throw new Exception("Failed to load onlyshadow shader");
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600035B RID: 859 RVA: 0x00019080 File Offset: 0x00017280
	private bool ShouldShow
	{
		get
		{
			return !(this._camera == null) && Vector3.Dot(this._camera.transform.forward, this.hideDirection) >= -0.1f;
		}
	}

	// Token: 0x0600035C RID: 860 RVA: 0x000190C8 File Offset: 0x000172C8
	private void Update()
	{
		if (this._camera == null)
		{
			return;
		}
		HideGroup.State currentState = this._currentState;
		if (currentState != HideGroup.State.SHOWING)
		{
			if (currentState == HideGroup.State.HIDING)
			{
				if (this.ShouldShow)
				{
					this.ShowWithNoFade();
				}
			}
		}
		else if (Vector3.Dot(this._camera.transform.forward, this.hideDirection) < -0.1f)
		{
			this.HideWithNoFade();
		}
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00019148 File Offset: 0x00017348
	private void PrintEnabled(Renderer r)
	{
		Debug.Log("Set renderer of " + r.transform.name + " to " + ((!r.enabled) ? "disabled" : "enabled"));
	}

	// Token: 0x0600035E RID: 862 RVA: 0x00019190 File Offset: 0x00017390
	public void HideWithNoFade()
	{
		foreach (Projector projector in base.transform.GetComponentsInChildren<Projector>())
		{
			projector.enabled = false;
		}
		foreach (Renderer renderer in base.transform.GetComponentsInChildren<Renderer>())
		{
			if (renderer.transform.tag == "DoorClickThing")
			{
				renderer.enabled = true;
			}
			else if (renderer.material.shader == HideGroup._defaultShader)
			{
				renderer.material.shader = HideGroup._onlyShadowShader;
			}
			else
			{
				renderer.enabled = false;
			}
		}
		this._currentState = HideGroup.State.HIDING;
	}

	// Token: 0x0600035F RID: 863 RVA: 0x00019258 File Offset: 0x00017458
	public void ShowWithNoFade()
	{
		foreach (Projector projector in base.transform.GetComponentsInChildren<Projector>())
		{
			projector.enabled = true;
		}
		foreach (Renderer renderer in base.transform.GetComponentsInChildren<Renderer>())
		{
			if (renderer.transform.tag == "DoorClickThing")
			{
				renderer.enabled = false;
			}
			else if (renderer.material.shader == HideGroup._onlyShadowShader)
			{
				renderer.material.shader = HideGroup._defaultShader;
			}
			else
			{
				renderer.enabled = true;
			}
		}
		this._currentState = HideGroup.State.SHOWING;
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00019320 File Offset: 0x00017520
	private void OnDrawGizmos()
	{
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			Debug.DrawLine(transform.transform.position, transform.transform.position + this.hideDirection * 0.5f, this.debugColor);
		}
	}

	// Token: 0x04000274 RID: 628
	public const float HIDE_THRESHOLD = -0.1f;

	// Token: 0x04000275 RID: 629
	public const float SHOW_THRESHOLD = -0.1f;

	// Token: 0x04000276 RID: 630
	public Vector3 hideDirection;

	// Token: 0x04000277 RID: 631
	public Color debugColor = Color.magenta;

	// Token: 0x04000278 RID: 632
	private Camera _camera;

	// Token: 0x04000279 RID: 633
	private HideGroup.State _currentState;

	// Token: 0x0400027A RID: 634
	private static Shader _defaultShader;

	// Token: 0x0400027B RID: 635
	private static Shader _onlyShadowShader;

	// Token: 0x02000068 RID: 104
	public enum State
	{
		// Token: 0x0400027D RID: 637
		SHOWING,
		// Token: 0x0400027E RID: 638
		HIDING
	}
}
