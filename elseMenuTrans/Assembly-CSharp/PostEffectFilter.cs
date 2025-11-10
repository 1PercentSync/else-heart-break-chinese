using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class PostEffectFilter : MonoBehaviour
{
	// Token: 0x0600030C RID: 780 RVA: 0x00017AB8 File Offset: 0x00015CB8
	private void Update()
	{
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00017ABC File Offset: 0x00015CBC
	private void OnPostRender()
	{
		if (this.material == null)
		{
			Debug.LogError("Please Assign a material on the inspector");
			return;
		}
		GL.PushMatrix();
		this.material.SetPass(0);
		GL.LoadPixelMatrix();
		GL.Viewport(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
		GL.Begin(7);
		GL.TexCoord(new Vector3(0f, 0f));
		GL.Vertex3(0f, 0f, 0f);
		GL.TexCoord(new Vector3(0f, 1f));
		GL.Vertex3(0f, (float)Screen.height, 0f);
		GL.TexCoord(new Vector3(1f, 1f));
		GL.Vertex3((float)Screen.width, (float)Screen.height, 0f);
		GL.TexCoord(new Vector3(1f, 0f));
		GL.Vertex3((float)Screen.width, 0f, 0f);
		GL.End();
		GL.PopMatrix();
	}

	// Token: 0x04000229 RID: 553
	public Material material;
}
