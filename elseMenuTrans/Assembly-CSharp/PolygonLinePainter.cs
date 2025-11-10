using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007D RID: 125
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
internal class PolygonLinePainter : MonoBehaviour
{
	// Token: 0x060003B4 RID: 948 RVA: 0x0001AB60 File Offset: 0x00018D60
	private void Start()
	{
		this._meshfilter = base.GetComponent<MeshFilter>();
		if (this.polyLine != null && this.polyLine.Length > 2)
		{
			this.DrawPolyLine(this.polyLineDefaultThickness, this.polyLine);
		}
		if (this.quad != null && this.quad.Length == 4)
		{
			this.AddQuad(this.quad[0], this.quad[1], this.quad[2], this.quad[3]);
			this.Set();
		}
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0001AC24 File Offset: 0x00018E24
	public void Clear()
	{
		this._vertices.Clear();
		this._uvs.Clear();
		this._triangles.Clear();
		this._meshfilter.mesh.Clear();
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x0001AC64 File Offset: 0x00018E64
	private void Set()
	{
		this._meshfilter.mesh.vertices = this._vertices.ToArray();
		this._meshfilter.mesh.uv = this._uvs.ToArray();
		this._meshfilter.mesh.triangles = this._triangles.ToArray();
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x0001ACC4 File Offset: 0x00018EC4
	public void AddLine(Vector2 pStart, Vector2 pEnd, float pThickness)
	{
		pThickness *= 0.5f;
		Vector3 vector = new Vector3(pEnd.x, 0f, pEnd.y);
		Vector3 vector2 = new Vector3(pStart.x, 0f, pStart.y);
		Vector3 vector3 = vector - vector2;
		vector3.Normalize();
		Vector3 up = Vector3.up;
		Vector3 vector4 = Vector3.Cross(up, vector3);
		vector4.Normalize();
		this.AddQuad(vector2 - vector4 * pThickness, vector - vector4 * pThickness, vector2 + vector4 * pThickness, vector + vector4 * pThickness);
		this.Set();
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0001AD78 File Offset: 0x00018F78
	private void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		this._vertices.Add(a);
		this._uvs.Add(new Vector2(0f, 0f));
		this._triangles.Add(this._vertices.Count - 1);
		this._vertices.Add(b);
		this._uvs.Add(new Vector2(1f, 0f));
		this._triangles.Add(this._vertices.Count - 1);
		this._vertices.Add(d);
		this._uvs.Add(new Vector2(1f, 1f));
		this._triangles.Add(this._vertices.Count - 1);
		this._vertices.Add(a);
		this._uvs.Add(new Vector2(0f, 0f));
		this._triangles.Add(this._vertices.Count - 1);
		this._vertices.Add(d);
		this._uvs.Add(new Vector2(1f, 1f));
		this._triangles.Add(this._vertices.Count - 1);
		this._vertices.Add(c);
		this._uvs.Add(new Vector2(0f, 1f));
		this._triangles.Add(this._vertices.Count - 1);
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x0001AEFC File Offset: 0x000190FC
	public void DrawPolyLine(float pThickness, Vector2[] pPoints)
	{
		pThickness *= 0.5f;
		for (int i = 0; i < pPoints.Length - 1; i++)
		{
			Vector2 vector = pPoints[i];
			Vector2 vector2 = pPoints[i + 1];
			Vector2 vector3 = pPoints[i + 1] - pPoints[i];
			Vector2 vector4 = this.Perp(vector3);
			Line2D lineEdgePlane = this.GetLineEdgePlane(pPoints, i);
			Line2D lineEdgePlane2 = this.GetLineEdgePlane(pPoints, i + 1);
			Vector3 vector5 = this.ToVector3(lineEdgePlane.GetIntersectionPoint(vector2 - vector4 * pThickness, -vector3));
			Vector3 vector6 = this.ToVector3(lineEdgePlane2.GetIntersectionPoint(vector - vector4 * pThickness, -vector3));
			Vector3 vector7 = this.ToVector3(lineEdgePlane.GetIntersectionPoint(vector2 + vector4 * pThickness, -vector3));
			Vector3 vector8 = this.ToVector3(lineEdgePlane2.GetIntersectionPoint(vector + vector4 * pThickness, -vector3));
			this.AddQuad(vector5, vector6, vector7, vector8);
		}
		this.Set();
	}

	// Token: 0x060003BA RID: 954 RVA: 0x0001B024 File Offset: 0x00019224
	private Line2D GetLineEdgePlane(Vector2[] pPoints, int pIndex)
	{
		Vector2 vector;
		if (pIndex > 0)
		{
			vector = pPoints[pIndex] - pPoints[pIndex - 1];
		}
		else if (pPoints[pPoints.Length - 1] == pPoints[0])
		{
			vector = pPoints[pIndex] - pPoints[pPoints.Length - 2];
		}
		else
		{
			vector = pPoints[pIndex + 1] - pPoints[pIndex];
		}
		Vector2 vector2;
		if (pIndex < pPoints.Length - 1)
		{
			vector2 = pPoints[pIndex + 1] - pPoints[pIndex];
		}
		else if (pPoints[pPoints.Length - 1] == pPoints[0])
		{
			vector2 = pPoints[1] - pPoints[pIndex];
		}
		else
		{
			vector2 = vector;
		}
		vector2.Normalize();
		vector.Normalize();
		Vector2 vector3 = vector + vector2;
		vector3.Normalize();
		return new Line2D(pPoints[pIndex], vector3);
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0001B174 File Offset: 0x00019374
	private Vector2 Perp(Vector2 pSouce)
	{
		Vector2 vector = new Vector2(pSouce.y, -pSouce.x);
		vector.Normalize();
		return vector;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0001B1A0 File Offset: 0x000193A0
	private Vector3 ToVector3(Vector2 pSouce)
	{
		return new Vector3(pSouce.x, 0f, pSouce.y);
	}

	// Token: 0x040002CE RID: 718
	public Vector2[] quad;

	// Token: 0x040002CF RID: 719
	public float polyLineDefaultThickness = 0.1f;

	// Token: 0x040002D0 RID: 720
	public Vector2[] polyLine;

	// Token: 0x040002D1 RID: 721
	private List<Vector3> _vertices = new List<Vector3>();

	// Token: 0x040002D2 RID: 722
	private List<Vector2> _uvs = new List<Vector2>();

	// Token: 0x040002D3 RID: 723
	private List<int> _triangles = new List<int>();

	// Token: 0x040002D4 RID: 724
	private MeshFilter _meshfilter;
}
