using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class EditableMesh
{
	// Token: 0x0600020A RID: 522 RVA: 0x0000F750 File Offset: 0x0000D950
	public EditableMesh(Mesh pMesh, MeshCollider pMeshCollider)
	{
		this._mesh = pMesh;
		this._meshCollider = pMeshCollider;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000F7B4 File Offset: 0x0000D9B4
	private int GetBinary(params bool[] pBits)
	{
		int num = 0;
		for (int i = 0; i < pBits.Length; i++)
		{
			if (pBits[i])
			{
				num += 1 << i;
			}
		}
		return num;
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000F7EC File Offset: 0x0000D9EC
	public void AddCubeFirstPointCenter(Vector3 pPosition, Vector3 pSize)
	{
		Vector3 vector = pSize.z * Vector3.forward;
		Vector3 vector2 = pSize.x * Vector3.right;
		Vector3 vector3 = pSize.y * Vector3.up;
		Vector3 vector4 = pPosition + vector2;
		Vector3 vector5 = pPosition + vector;
		Vector3 vector6 = pPosition + vector + vector2;
		Vector3 vector7 = pPosition + vector3;
		Vector3 vector8 = pPosition + vector3 + vector2;
		Vector3 vector9 = pPosition + vector3 + vector;
		Vector3 vector10 = pPosition + vector3 + vector + vector2;
		this.AddHardEdgeQuad(vector7, vector8, pPosition, vector4);
		this.AddHardEdgeQuad(vector8, vector10, vector4, vector6);
		this.AddHardEdgeQuad(pPosition, vector4, vector5, vector6);
		this.AddHardEdgeQuad(vector7, pPosition, vector9, vector5);
		this.AddHardEdgeQuad(vector9, vector10, vector7, vector8);
		this.AddHardEdgeQuad(vector10, vector9, vector6, vector5);
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000F8E0 File Offset: 0x0000DAE0
	public void AddCube(Vector3 pPosition, Vector3 pSize)
	{
		Vector3 vector = pSize.z * Vector3.forward * 0.5f;
		Vector3 vector2 = pSize.x * Vector3.right * 0.5f;
		Vector3 vector3 = pSize.y * Vector3.up * 0.5f;
		Vector3 vector4 = pPosition - vector2 - vector3 - vector;
		Vector3 vector5 = pPosition + vector2 - vector3 - vector;
		Vector3 vector6 = pPosition - vector2 - vector3 + vector;
		Vector3 vector7 = pPosition + vector2 - vector3 + vector;
		Vector3 vector8 = pPosition - vector2 + vector3 - vector;
		Vector3 vector9 = pPosition + vector2 + vector3 - vector;
		Vector3 vector10 = pPosition - vector2 + vector3 + vector;
		Vector3 vector11 = pPosition + vector2 + vector3 + vector;
		this.AddQuad(vector8, vector9, vector4, vector5);
		this.AddQuad(vector9, vector11, vector5, vector7);
		this.AddQuad(vector4, vector5, vector6, vector7);
		this.AddQuad(vector8, vector4, vector10, vector6);
		this.AddQuad(vector10, vector11, vector8, vector9);
		this.AddQuad(vector11, vector10, vector7, vector6);
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000FA40 File Offset: 0x0000DC40
	public void AddPlane(Vector3 pPosition, Vector3 pSize)
	{
		Vector3 vector = pSize.z * Vector3.forward * 0.5f;
		Vector3 vector2 = pSize.x * Vector3.right * 0.5f;
		Vector3 vector3 = pSize.y * Vector3.up * 0.5f;
		Vector3 vector4 = pPosition - vector2 + vector3 - vector;
		Vector3 vector5 = pPosition + vector2 + vector3 - vector;
		Vector3 vector6 = pPosition - vector2 + vector3 + vector;
		Vector3 vector7 = pPosition + vector2 + vector3 + vector;
		this.AddQuad(vector6, vector7, vector4, vector5);
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000FB04 File Offset: 0x0000DD04
	public void ClearMesh()
	{
		this._vertices.Clear();
		this._uvs.Clear();
		this._verticesByPoint.Clear();
		this._triangles.Clear();
		this._colors.Clear();
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000FB48 File Offset: 0x0000DD48
	public void ApplyMesh()
	{
		this._mesh.vertices = this._vertices.ToArray();
		this._mesh.uv = this._uvs.ToArray();
		this._mesh.triangles = this._triangles.ToArray();
		this._mesh.colors = this._colors.ToArray();
		this._mesh.RecalculateBounds();
		this._mesh.RecalculateNormals();
		this._mesh.Optimize();
		if (this._meshCollider != null)
		{
			this._meshCollider = null;
			this._meshCollider.sharedMesh = this._mesh;
		}
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000FBF8 File Offset: 0x0000DDF8
	public int[] AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		int num = this.AddVertex(a, new Vector2(0f, 0f));
		int num2 = this.AddVertex(b, new Vector2(1f, 0f));
		int num3 = this.AddVertex(c, new Vector2(0f, 1f));
		int num4 = this.AddVertex(d, new Vector2(1f, 1f));
		this._triangles.Add(num);
		this._triangles.Add(num2);
		this._triangles.Add(num4);
		this._triangles.Add(num);
		this._triangles.Add(num4);
		this._triangles.Add(num3);
		return new int[] { num, num2, num3, num4 };
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000FCC0 File Offset: 0x0000DEC0
	private void AddHardEdgeQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		int num = this.AddVertexHardNormal(a, new Vector2(0f, 0f));
		int num2 = this.AddVertexHardNormal(b, new Vector2(1f, 0f));
		int num3 = this.AddVertexHardNormal(c, new Vector2(0f, 1f));
		int num4 = this.AddVertexHardNormal(d, new Vector2(1f, 1f));
		this._triangles.Add(num);
		this._triangles.Add(num2);
		this._triangles.Add(num4);
		this._triangles.Add(num);
		this._triangles.Add(num4);
		this._triangles.Add(num3);
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000FD74 File Offset: 0x0000DF74
	public void AddSubdivQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		Vector3 vector = a + (b - a) * 0.5f;
		Vector3 vector2 = c + (d - c) * 0.5f;
		Vector3 vector3 = a + (c - a) * 0.5f;
		Vector3 vector4 = b + (d - b) * 0.5f;
		Vector3 vector5 = vector + (vector2 - vector) * 0.5f;
		this.AddQuad(a, vector, vector3, vector5);
		this.AddQuad(vector, b, vector5, vector4);
		this.AddQuad(vector3, vector5, c, vector2);
		this.AddQuad(vector5, vector4, vector2, d);
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0000FE30 File Offset: 0x0000E030
	public void AddPentaQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		Vector3 vector = a + (d - a) * 0.5f;
		int num = this.AddVertex(a, new Vector2(0f, 0f));
		int num2 = this.AddVertex(b, new Vector2(1f, 0f));
		int num3 = this.AddVertex(c, new Vector2(0f, 1f));
		int num4 = this.AddVertex(d, new Vector2(1f, 1f));
		int num5 = this.AddVertex(vector, new Vector2(0.5f, 0.5f));
		this._triangles.Add(num);
		this._triangles.Add(num2);
		this._triangles.Add(num5);
		this._triangles.Add(num2);
		this._triangles.Add(num4);
		this._triangles.Add(num5);
		this._triangles.Add(num5);
		this._triangles.Add(num4);
		this._triangles.Add(num3);
		this._triangles.Add(num);
		this._triangles.Add(num5);
		this._triangles.Add(num3);
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000FF64 File Offset: 0x0000E164
	private int AddVertex(Vector3 pVector, Vector2 pUV)
	{
		int num;
		if (!this._verticesByPoint.TryGetValue(pVector, out num))
		{
			this._uvs.Add(pUV);
			this._vertices.Add(pVector);
			this._colors.Add(this.currentColor);
			this._verticesByPoint.Add(pVector, this._vertices.Count - 1);
			return this._vertices.Count - 1;
		}
		return num;
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000FFD8 File Offset: 0x0000E1D8
	private int AddVertexHardNormal(Vector3 pVector, Vector2 pUV)
	{
		this._uvs.Add(pUV);
		this._vertices.Add(pVector);
		this._colors.Add(this.currentColor);
		return this._vertices.Count - 1;
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0001001C File Offset: 0x0000E21C
	public void TriangleStripBegin()
	{
		this._stripStartingCount = this._vertices.Count;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x00010030 File Offset: 0x0000E230
	public int TriangleStripAddVertex(Vector3 pPosition, Vector2 pUv, Color pColor)
	{
		this._vertices.Add(pPosition);
		this._uvs.Add(pUv);
		this._colors.Add(pColor);
		if (this._vertices.Count - this._stripStartingCount > 2)
		{
			this._triangles.Add(this._vertices.Count - 1);
			this._triangles.Add(this._vertices.Count - 2);
			this._triangles.Add(this._vertices.Count - 3);
		}
		return this._vertices.Count - 1;
	}

	// Token: 0x06000219 RID: 537 RVA: 0x000100D0 File Offset: 0x0000E2D0
	public void TriangleStripCloseLoop()
	{
		this._triangles.Add(this._stripStartingCount);
		this._triangles.Add(this._vertices.Count - 1);
		this._triangles.Add(this._vertices.Count - 2);
		this._triangles.Add(this._stripStartingCount + 1);
		this._triangles.Add(this._stripStartingCount);
		this._triangles.Add(this._vertices.Count - 1);
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0001015C File Offset: 0x0000E35C
	private void AppendMesh(Mesh pMesh, Vector3 pPosition, Vector3 pUp, Vector3 pRight, Vector3 pForward)
	{
		for (int i = 0; i < pMesh.triangles.Length; i += 3)
		{
			Vector3 vector = this.ComputeTransformedPosition(pMesh.vertices[pMesh.triangles[i]], pPosition, pUp, pRight, pForward);
			Vector3 vector2 = this.ComputeTransformedPosition(pMesh.vertices[pMesh.triangles[i + 1]], pPosition, pUp, pRight, pForward);
			Vector3 vector3 = this.ComputeTransformedPosition(pMesh.vertices[pMesh.triangles[i + 2]], pPosition, pUp, pRight, pForward);
			Vector2 vector4 = pMesh.uv[pMesh.triangles[i]];
			Vector2 vector5 = pMesh.uv[pMesh.triangles[i + 1]];
			Vector2 vector6 = pMesh.uv[pMesh.triangles[i + 2]];
			int num = this.AddVertexHardNormal(vector, vector4);
			int num2 = this.AddVertexHardNormal(vector2, vector5);
			int num3 = this.AddVertexHardNormal(vector3, vector6);
			this._triangles.Add(num);
			this._triangles.Add(num2);
			this._triangles.Add(num3);
		}
	}

	// Token: 0x0600021B RID: 539 RVA: 0x00010290 File Offset: 0x0000E490
	private int GetTriangleCount(Mesh pMesh)
	{
		return pMesh.triangles.Length / 3;
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0001029C File Offset: 0x0000E49C
	private EditableMesh.Triangle GetTriangle(Mesh pMesh, int pIndex)
	{
		return new EditableMesh.Triangle(pMesh.vertices[pMesh.triangles[pIndex * 3 + 2]], pMesh.vertices[pMesh.triangles[pIndex * 3 + 1]], pMesh.vertices[pMesh.triangles[pIndex * 3]], pMesh.uv[pMesh.triangles[pIndex * 3 + 2]], pMesh.uv[pMesh.triangles[pIndex * 3 + 1]], pMesh.uv[pMesh.triangles[pIndex * 3]]);
	}

	// Token: 0x0600021D RID: 541 RVA: 0x00010354 File Offset: 0x0000E554
	private Vector3 ComputeTransformedPosition(Vector3 pSource, Vector3 pTranslate, Vector3 pUp, Vector3 pRight, Vector3 pForward)
	{
		return pRight * (pSource.x + pTranslate.x) + pUp * (pSource.y + pTranslate.y) + pForward * (pSource.z + pTranslate.z);
	}

	// Token: 0x0600021E RID: 542 RVA: 0x000103AC File Offset: 0x0000E5AC
	private EditableMesh.Triangle[] CropTriangles(EditableMesh.Triangle[] pTriangles, Plane pPlane)
	{
		List<EditableMesh.Triangle> list = new List<EditableMesh.Triangle>();
		for (int i = 0; i < pTriangles.Length; i++)
		{
			list.AddRange(this.CropTriangle(pTriangles[i], pPlane));
		}
		return list.ToArray();
	}

	// Token: 0x0600021F RID: 543 RVA: 0x000103F4 File Offset: 0x0000E5F4
	private EditableMesh.Triangle[] CropTriangle(EditableMesh.Triangle pSource, Plane pPlane)
	{
		bool[] array = new bool[]
		{
			pPlane.GetSide(pSource.vertices[0]),
			pPlane.GetSide(pSource.vertices[1]),
			pPlane.GetSide(pSource.vertices[2])
		};
		if (!array[0] && !array[1] && !array[2])
		{
			return new EditableMesh.Triangle[0];
		}
		if (array[0] && array[1] && array[2])
		{
			return new EditableMesh.Triangle[] { pSource };
		}
		if (array[0] && array[1])
		{
			return this.CreateTwoTriangles(0, 1, 2, ref pPlane, pSource.uvs, pSource.vertices);
		}
		if (array[0] && array[2])
		{
			return this.CreateTwoTriangles(2, 0, 1, ref pPlane, pSource.uvs, pSource.vertices);
		}
		if (array[1] && array[2])
		{
			return this.CreateTwoTriangles(1, 2, 0, ref pPlane, pSource.uvs, pSource.vertices);
		}
		if (array[0])
		{
			return this.CreateOneTriangle(0, 1, 2, ref pPlane, pSource.uvs, pSource.vertices);
		}
		if (array[1])
		{
			return this.CreateOneTriangle(1, 2, 0, ref pPlane, pSource.uvs, pSource.vertices);
		}
		if (array[2])
		{
			return this.CreateOneTriangle(2, 0, 1, ref pPlane, pSource.uvs, pSource.vertices);
		}
		throw new Exception("this can't happend!");
	}

	// Token: 0x06000220 RID: 544 RVA: 0x00010598 File Offset: 0x0000E798
	private EditableMesh.Triangle[] CreateTwoTriangles(int pInsideA, int pInsideB, int pOutsideC, ref Plane pPlane, Vector2[] pTriangleUvs, Vector3[] pVertices)
	{
		float num;
		pPlane.Raycast(new Ray(pVertices[pInsideA], Vector3.Normalize(pVertices[pOutsideC] - pVertices[pInsideA])), out num);
		float num2;
		pPlane.Raycast(new Ray(pVertices[pInsideB], Vector3.Normalize(pVertices[pOutsideC] - pVertices[pInsideB])), out num2);
		float num3 = this.NormalizedDistance(pVertices[pInsideA], pVertices[pOutsideC], num);
		float num4 = this.NormalizedDistance(pVertices[pInsideB], pVertices[pOutsideC], num2);
		return new EditableMesh.Triangle[]
		{
			new EditableMesh.Triangle(pVertices[pInsideA], pVertices[pInsideB], this.LerpVertice(pVertices[pInsideB], pVertices[pOutsideC], num4), pTriangleUvs[pInsideA], pTriangleUvs[pInsideB], this.LerpUV(pTriangleUvs[pInsideB], pTriangleUvs[pOutsideC], num4)),
			new EditableMesh.Triangle(pVertices[pInsideA], this.LerpVertice(pVertices[pInsideB], pVertices[pOutsideC], num4), this.LerpVertice(pVertices[pInsideA], pVertices[pOutsideC], num3), pTriangleUvs[pInsideA], this.LerpUV(pTriangleUvs[pInsideB], pTriangleUvs[pOutsideC], num4), this.LerpUV(pTriangleUvs[pInsideA], pTriangleUvs[pOutsideC], num3))
		};
	}

	// Token: 0x06000221 RID: 545 RVA: 0x000107AC File Offset: 0x0000E9AC
	private EditableMesh.Triangle[] CreateOneTriangle(int pInsideA, int pOutsideB, int pOutsideC, ref Plane pPlane, Vector2[] pTriangleUvs, Vector3[] pVertices)
	{
		float num;
		pPlane.Raycast(new Ray(pVertices[pInsideA], Vector3.Normalize(pVertices[pOutsideB] - pVertices[pInsideA])), out num);
		float num2;
		pPlane.Raycast(new Ray(pVertices[pInsideA], Vector3.Normalize(pVertices[pOutsideC] - pVertices[pInsideA])), out num2);
		float num3 = this.NormalizedDistance(pVertices[pOutsideB], pVertices[pInsideA], num);
		float num4 = this.NormalizedDistance(pVertices[pOutsideC], pVertices[pInsideA], num2);
		return new EditableMesh.Triangle[]
		{
			new EditableMesh.Triangle(pVertices[pInsideA], this.LerpVertice(pVertices[pInsideA], pVertices[pOutsideB], num3), this.LerpVertice(pVertices[pInsideA], pVertices[pOutsideC], num4), pTriangleUvs[pInsideA], this.LerpUV(pTriangleUvs[pInsideA], pTriangleUvs[pOutsideB], num3), this.LerpUV(pTriangleUvs[pInsideA], pTriangleUvs[pOutsideC], num4))
		};
	}

	// Token: 0x06000222 RID: 546 RVA: 0x00010938 File Offset: 0x0000EB38
	private float NormalizedDistance(Vector3 pA, Vector3 pB, float pDistance)
	{
		return pDistance / (pB - pA).magnitude;
	}

	// Token: 0x06000223 RID: 547 RVA: 0x00010958 File Offset: 0x0000EB58
	private Vector3 LerpVertice(Vector3 pA, Vector3 pB, float pNormalizedDistance)
	{
		return pA + (pB - pA) * pNormalizedDistance;
	}

	// Token: 0x06000224 RID: 548 RVA: 0x00010970 File Offset: 0x0000EB70
	private Vector2 LerpUV(Vector2 pUVa, Vector2 pUVb, float pNormalizedDistance)
	{
		return pUVa + (pUVb - pUVa) * pNormalizedDistance;
	}

	// Token: 0x04000157 RID: 343
	private Mesh _mesh;

	// Token: 0x04000158 RID: 344
	private MeshCollider _meshCollider;

	// Token: 0x04000159 RID: 345
	private Dictionary<Vector3, int> _verticesByPoint = new Dictionary<Vector3, int>();

	// Token: 0x0400015A RID: 346
	private List<Vector3> _vertices = new List<Vector3>();

	// Token: 0x0400015B RID: 347
	private List<Vector2> _uvs = new List<Vector2>();

	// Token: 0x0400015C RID: 348
	private List<Color> _colors = new List<Color>();

	// Token: 0x0400015D RID: 349
	private List<int> _triangles = new List<int>();

	// Token: 0x0400015E RID: 350
	public Color currentColor = Color.white;

	// Token: 0x0400015F RID: 351
	private int _stripStartingCount;

	// Token: 0x02000031 RID: 49
	public struct Triangle
	{
		// Token: 0x06000225 RID: 549 RVA: 0x00010988 File Offset: 0x0000EB88
		public Triangle(Vector3 pVa, Vector3 pVb, Vector3 pVc, Vector2 pUVa, Vector2 pUVb, Vector2 pUVc)
		{
			this.vertices = new Vector3[] { pVa, pVb, pVc };
			this.uvs = new Vector2[] { pUVa, pUVb, pUVc };
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00010A00 File Offset: 0x0000EC00
		public void TranslateVertices(Vector3 pRealtivePosition)
		{
			for (int i = 0; i < this.vertices.Length; i++)
			{
				this.vertices[i] = this.vertices[i] + pRealtivePosition;
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00010A50 File Offset: 0x0000EC50
		public void TransformFromVectors(Vector3 pRight, Vector3 pUp, Vector3 pForward)
		{
			for (int i = 0; i < this.vertices.Length; i++)
			{
				this.vertices[i] = this.vertices[i].x * pRight + this.vertices[i].y * pUp + this.vertices[i].z * pForward;
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00010AD8 File Offset: 0x0000ECD8
		public static void TransformMany(EditableMesh.Triangle[] pTris, Vector3 pRight, Vector3 pUp, Vector3 pForward)
		{
			for (int i = 0; i < pTris.Length; i++)
			{
				pTris[i].TransformFromVectors(pRight, pUp, pForward);
			}
		}

		// Token: 0x04000160 RID: 352
		public Vector3[] vertices;

		// Token: 0x04000161 RID: 353
		public Vector2[] uvs;
	}

	// Token: 0x020000FB RID: 251
	// (Invoke) Token: 0x0600074B RID: 1867
	private delegate void QuadBuilder(Vector3 a, Vector3 b, Vector3 c, Vector3 d);
}
