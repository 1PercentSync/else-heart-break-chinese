using System;
using System.Collections.Generic;
using GameTypes;
using GameWorld2;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class NetNode : MonoBehaviour
{
	// Token: 0x060003A1 RID: 929 RVA: 0x0001A4F4 File Offset: 0x000186F4
	private void Start()
	{
		base.transform.name = "NetNode_" + this.ting.name;
		global::UnityEngine.Object @object = null;
		this._shell = ShellManager.GetShellWithName(this.ting.name);
		if (this._shell != null)
		{
			base.transform.parent = this._shell.transform;
			this.hasRepresentationInSceneInternet = true;
		}
		else if (this.ting is Computer)
		{
			@object = Resources.Load("InternetComputerAvatar");
		}
		else if (this.ting is Character)
		{
			@object = Resources.Load("InternetPerson");
		}
		else
		{
			@object = Resources.Load("InternetDot");
		}
		if (@object != null)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate(@object) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0001A5F0 File Offset: 0x000187F0
	private void OnDestroy()
	{
		this.disposed = true;
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0001A5FC File Offset: 0x000187FC
	private void Update()
	{
		if (this.hasRepresentationInSceneInternet)
		{
			base.transform.position = this._shell.transform.position;
		}
		else
		{
			this.CalculatePosition();
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0001A63C File Offset: 0x0001883C
	private void CalculatePosition()
	{
		IntPoint worldPoint = this.ting.worldPoint;
		float num = (float)(worldPoint.x % 600 - 300);
		float num2 = (float)(worldPoint.y % 600 - 300);
		float num3 = ((!(this.ting.actionName == string.Empty)) ? 50f : 30f) + 10f * Mathf.Sin((float)(worldPoint.x + worldPoint.y) + Time.time * 0.2f);
		Vector3 vector = new Vector3(num, num3, num2);
		Vector3 vector2 = vector - base.transform.position;
		base.transform.Translate(vector2 * Time.deltaTime * 1f);
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0001A710 File Offset: 0x00018910
	public void DrawConnections()
	{
		foreach (MimanTing mimanTing in this.ting.connectedTings)
		{
			Vector3 position = base.transform.position;
			Vector3 position2 = new Vector3(0f, 0f, 0f);
			foreach (NetNode netNode in this.netNodes)
			{
				if (netNode.ting == mimanTing)
				{
					position2 = netNode.transform.position;
					break;
				}
			}
			GL.Color(new Color(0f, 1f, 1f, 1f));
			GL.Vertex3(position.x, position.y, position.z);
			GL.Vertex3(position2.x, position2.y, position2.z);
		}
	}

	// Token: 0x040002BA RID: 698
	public bool disposed;

	// Token: 0x040002BB RID: 699
	public List<NetNode> netNodes;

	// Token: 0x040002BC RID: 700
	public MimanTing ting;

	// Token: 0x040002BD RID: 701
	public bool hasRepresentationInSceneInternet;

	// Token: 0x040002BE RID: 702
	private Shell _shell;

	// Token: 0x040002BF RID: 703
	private int _fold;
}
