using System;
using UnityEngine;

namespace UnityStandardAssets.Cameras
{
	// Token: 0x02000047 RID: 71
	public abstract class AbstractTargetFollower : MonoBehaviour
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x0001322C File Offset: 0x0001142C
		protected virtual void Start()
		{
			if (this.m_AutoTargetPlayer)
			{
				this.FindAndTargetPlayer();
			}
			if (this.m_Target == null)
			{
				return;
			}
			this.targetRigidbody = this.m_Target.GetComponent<Rigidbody>();
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00013270 File Offset: 0x00011470
		private void FixedUpdate()
		{
			if (this.m_AutoTargetPlayer && (this.m_Target == null || !this.m_Target.gameObject.activeSelf))
			{
				this.FindAndTargetPlayer();
			}
			if (this.m_UpdateType == AbstractTargetFollower.UpdateType.FixedUpdate)
			{
				this.FollowTarget(Time.deltaTime);
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000132CC File Offset: 0x000114CC
		private void LateUpdate()
		{
			if (this.m_AutoTargetPlayer && (this.m_Target == null || !this.m_Target.gameObject.activeSelf))
			{
				this.FindAndTargetPlayer();
			}
			if (this.m_UpdateType == AbstractTargetFollower.UpdateType.LateUpdate)
			{
				this.FollowTarget(Time.deltaTime);
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00013328 File Offset: 0x00011528
		public void ManualUpdate()
		{
			if (this.m_AutoTargetPlayer && (this.m_Target == null || !this.m_Target.gameObject.activeSelf))
			{
				this.FindAndTargetPlayer();
			}
			if (this.m_UpdateType == AbstractTargetFollower.UpdateType.ManualUpdate)
			{
				this.FollowTarget(Time.deltaTime);
			}
		}

		// Token: 0x060002CB RID: 715
		protected abstract void FollowTarget(float deltaTime);

		// Token: 0x060002CC RID: 716 RVA: 0x00013384 File Offset: 0x00011584
		public void FindAndTargetPlayer()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject)
			{
				this.SetTarget(gameObject.transform);
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000133B4 File Offset: 0x000115B4
		public virtual void SetTarget(Transform newTransform)
		{
			this.m_Target = newTransform;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060002CE RID: 718 RVA: 0x000133C0 File Offset: 0x000115C0
		public Transform Target
		{
			get
			{
				return this.m_Target;
			}
		}

		// Token: 0x040001A9 RID: 425
		[SerializeField]
		protected Transform m_Target;

		// Token: 0x040001AA RID: 426
		[SerializeField]
		private bool m_AutoTargetPlayer = true;

		// Token: 0x040001AB RID: 427
		[SerializeField]
		private AbstractTargetFollower.UpdateType m_UpdateType;

		// Token: 0x040001AC RID: 428
		protected Rigidbody targetRigidbody;

		// Token: 0x02000048 RID: 72
		public enum UpdateType
		{
			// Token: 0x040001AE RID: 430
			FixedUpdate,
			// Token: 0x040001AF RID: 431
			LateUpdate,
			// Token: 0x040001B0 RID: 432
			ManualUpdate
		}
	}
}
