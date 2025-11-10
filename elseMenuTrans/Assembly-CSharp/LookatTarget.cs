using System;
using UnityEngine;

namespace UnityStandardAssets.Cameras
{
	// Token: 0x02000073 RID: 115
	public class LookatTarget : AbstractTargetFollower
	{
		// Token: 0x06000392 RID: 914 RVA: 0x00019FE8 File Offset: 0x000181E8
		protected override void Start()
		{
			base.Start();
			this.m_OriginalRotation = base.transform.localRotation;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0001A004 File Offset: 0x00018204
		protected override void FollowTarget(float deltaTime)
		{
			base.transform.localRotation = this.m_OriginalRotation;
			Vector3 vector = base.transform.InverseTransformPoint(this.m_Target.position);
			float num = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
			num = Mathf.Clamp(num, -this.m_RotationRange.y * 0.5f, this.m_RotationRange.y * 0.5f);
			base.transform.localRotation = this.m_OriginalRotation * Quaternion.Euler(0f, num, 0f);
			vector = base.transform.InverseTransformPoint(this.m_Target.position);
			float num2 = Mathf.Atan2(vector.y, vector.z) * 57.29578f;
			num2 = Mathf.Clamp(num2, -this.m_RotationRange.x * 0.5f, this.m_RotationRange.x * 0.5f);
			Vector3 vector2 = new Vector3(this.m_FollowAngles.x + Mathf.DeltaAngle(this.m_FollowAngles.x, num2), this.m_FollowAngles.y + Mathf.DeltaAngle(this.m_FollowAngles.y, num));
			this.m_FollowAngles = Vector3.SmoothDamp(this.m_FollowAngles, vector2, ref this.m_FollowVelocity, this.m_FollowSpeed);
			base.transform.localRotation = this.m_OriginalRotation * Quaternion.Euler(-this.m_FollowAngles.x, this.m_FollowAngles.y, 0f);
		}

		// Token: 0x040002A2 RID: 674
		[SerializeField]
		private Vector2 m_RotationRange;

		// Token: 0x040002A3 RID: 675
		[SerializeField]
		private float m_FollowSpeed = 1f;

		// Token: 0x040002A4 RID: 676
		private Vector3 m_FollowAngles;

		// Token: 0x040002A5 RID: 677
		private Quaternion m_OriginalRotation;

		// Token: 0x040002A6 RID: 678
		protected Vector3 m_FollowVelocity;
	}
}
