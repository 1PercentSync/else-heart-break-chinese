using System;
using UnityEngine;

// Token: 0x02000075 RID: 117
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
	// Token: 0x06000398 RID: 920 RVA: 0x0001A21C File Offset: 0x0001841C
	private void Update()
	{
		if (this.axes == MouseLook.RotationAxes.MouseXAndY)
		{
			this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
			this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
			this.rotationX = MouseLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
			this.rotationY = MouseLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
			Quaternion quaternion = Quaternion.AngleAxis(this.rotationX, Vector3.up);
			Quaternion quaternion2 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
			base.transform.localRotation = this.originalRotation * quaternion * quaternion2;
		}
		else if (this.axes == MouseLook.RotationAxes.MouseX)
		{
			this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
			this.rotationX = MouseLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
			Quaternion quaternion3 = Quaternion.AngleAxis(this.rotationX, Vector3.up);
			base.transform.localRotation = this.originalRotation * quaternion3;
		}
		else
		{
			this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
			this.rotationY = MouseLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
			Quaternion quaternion4 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
			base.transform.localRotation = this.originalRotation * quaternion4;
		}
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0001A3C8 File Offset: 0x000185C8
	private void Start()
	{
		if (base.rigidbody)
		{
			base.rigidbody.freezeRotation = true;
		}
		this.originalRotation = base.transform.localRotation;
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0001A404 File Offset: 0x00018604
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x040002A8 RID: 680
	public MouseLook.RotationAxes axes;

	// Token: 0x040002A9 RID: 681
	public float sensitivityX = 15f;

	// Token: 0x040002AA RID: 682
	public float sensitivityY = 15f;

	// Token: 0x040002AB RID: 683
	public float minimumX = -360f;

	// Token: 0x040002AC RID: 684
	public float maximumX = 360f;

	// Token: 0x040002AD RID: 685
	public float minimumY = -60f;

	// Token: 0x040002AE RID: 686
	public float maximumY = 60f;

	// Token: 0x040002AF RID: 687
	private float rotationX;

	// Token: 0x040002B0 RID: 688
	private float rotationY;

	// Token: 0x040002B1 RID: 689
	private Quaternion originalRotation;

	// Token: 0x02000076 RID: 118
	public enum RotationAxes
	{
		// Token: 0x040002B3 RID: 691
		MouseXAndY,
		// Token: 0x040002B4 RID: 692
		MouseX,
		// Token: 0x040002B5 RID: 693
		MouseY
	}
}
