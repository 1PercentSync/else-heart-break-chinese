using System;
using UnityEngine;

// Token: 0x02000084 RID: 132
public class Rope : MonoBehaviour
{
	// Token: 0x060003D7 RID: 983 RVA: 0x0001BF04 File Offset: 0x0001A104
	private void Start()
	{
		GameObject gameObject = null;
		for (int i = 0; i < this.nrOfPieces; i++)
		{
			GameObject gameObject2 = global::UnityEngine.Object.Instantiate(this.ropePiecePrefab, base.transform.position + new Vector3((float)i * this.pieceLength, 0f, 0f), base.transform.rotation) as GameObject;
			gameObject2.name = "RopePiece" + i;
			if (gameObject != null)
			{
				gameObject2.hingeJoint.connectedBody = gameObject.rigidbody;
			}
			else
			{
				gameObject2.transform.parent = base.transform;
			}
			gameObject = gameObject2;
		}
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0001BFBC File Offset: 0x0001A1BC
	private void Update()
	{
	}

	// Token: 0x040002F9 RID: 761
	public GameObject ropePiecePrefab;

	// Token: 0x040002FA RID: 762
	public int nrOfPieces = 10;

	// Token: 0x040002FB RID: 763
	public float pieceLength = 1f;

	// Token: 0x040002FC RID: 764
	public float pieceOverlap = 0.1f;
}
