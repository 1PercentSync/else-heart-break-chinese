using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class GhostProjectorBehaviour : MonoBehaviour
{
	// Token: 0x06000317 RID: 791 RVA: 0x00017E48 File Offset: 0x00016048
	private void Start()
	{
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00017E4C File Offset: 0x0001604C
	private void Update()
	{
		this._changeDirTimer -= Time.deltaTime;
		if (this._changeDirTimer <= 0f)
		{
			if (global::UnityEngine.Random.Range(0, 100) < 50)
			{
				this._dir = ((global::UnityEngine.Random.Range(0, 100) >= 50) ? 1f : (-1f));
			}
			else
			{
				base.transform.Rotate(Vector3.up, global::UnityEngine.Random.Range(0f, 180f));
			}
			this._changeDirTimer = global::UnityEngine.Random.Range(0.1f, 1.5f);
		}
		base.transform.Rotate(Vector3.up, Time.deltaTime * 3.1415927f * 2f * this.rotationSpeed * this._dir);
	}

	// Token: 0x04000237 RID: 567
	public float fovChangeSpeed = 10f;

	// Token: 0x04000238 RID: 568
	public float rotationSpeed = 10f;

	// Token: 0x04000239 RID: 569
	private float _dir = 1f;

	// Token: 0x0400023A RID: 570
	private float _changeDirTimer;
}
