using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class GreatCameraTester : MonoBehaviour
{
	// Token: 0x0600034A RID: 842 RVA: 0x000189B4 File Offset: 0x00016BB4
	private void Start()
	{
		this._camera = base.GetComponent<GreatCamera>();
		if (this._camera == null)
		{
			Debug.LogError("No great camera");
		}
	}

	// Token: 0x0600034B RID: 843 RVA: 0x000189E0 File Offset: 0x00016BE0
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			GreatCamera.invertCamera = !GreatCamera.invertCamera;
			Debug.Log((!GreatCamera.invertCamera) ? "Don't invert" : "Invert");
		}
		if (Input.GetMouseButtonDown(1))
		{
			this._camera.Input_StartDrag();
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			this._camera.Input_StartDrag();
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetMouseButtonUp(1))
		{
			this._camera.Input_EndDrag();
		}
		if (Input.GetMouseButton(1))
		{
			this._camera.Input_Drag(Input.GetAxis("Horizontal") * Time.deltaTime * this.mouseSensitivity, Input.GetAxis("Vertical") * Time.deltaTime * this.mouseSensitivity);
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			this._camera.Input_Drag(Time.deltaTime * -this.keyRotationSensitivity, 0f);
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			this._camera.Input_Drag(Time.deltaTime * this.keyRotationSensitivity, 0f);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this._camera.Input_ZoomDiscrete(1);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this._camera.Input_ZoomDiscrete(-1);
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			this._camera.Input_SetTilt(80f);
		}
		else if (Input.GetKeyDown(KeyCode.X))
		{
			this._camera.Input_SetTilt(20f);
		}
		float num = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * this.scrollSensitivity;
		this._camera.Input_ZoomDiscrete((int)num);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (this._camera.isInFixedMode)
			{
				this._camera.ExitFixedCamera();
			}
			else
			{
				this._camera.EnterFixedCamera(this.cameraPoint, this.targetPoint);
			}
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			this._camera.Shake(0.5f, 1f, true);
		}
	}

	// Token: 0x04000263 RID: 611
	private GreatCamera _camera;

	// Token: 0x04000264 RID: 612
	public float mouseSensitivity = 10f;

	// Token: 0x04000265 RID: 613
	public float keyRotationSensitivity = 300f;

	// Token: 0x04000266 RID: 614
	public float keyZoomSensitivity = 50f;

	// Token: 0x04000267 RID: 615
	public float scrollSensitivity = 100f;

	// Token: 0x04000268 RID: 616
	public Transform cameraPoint;

	// Token: 0x04000269 RID: 617
	public Transform targetPoint;

	// Token: 0x0400026A RID: 618
	private bool _scrolling;
}
