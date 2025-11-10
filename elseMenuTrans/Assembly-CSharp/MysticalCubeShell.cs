using System;
using GameTypes;
using GameWorld2;
using RelayLib;
using UnityEngine;

// Token: 0x020000C4 RID: 196
public class MysticalCubeShell : Shell
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00026CF0 File Offset: 0x00024EF0
	public MysticalCube cube
	{
		get
		{
			return base.ting as MysticalCube;
		}
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x00026D00 File Offset: 0x00024F00
	public override void CreateTing()
	{
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x00026D04 File Offset: 0x00024F04
	protected override void Setup()
	{
		base.Setup();
		this._light = base.GetComponentInChildren<Light>();
		if (this._light == null)
		{
			D.LogError("Can't find light");
		}
		SoundDictionary.LoadSingleSound("Bird", "Seagull sound 8");
		SoundDictionary.LoadSingleSound("Boop", "Blip 0");
		SoundDictionary.LoadSingleSound("FiveBlips", "Blip 1");
		this.RefreshMaterialColor();
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00026D74 File Offset: 0x00024F74
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		this.cube.AddDataListener<Float3>("color", new ValueEntry<Float3>.DataChangeHandler(this.OnColorChange));
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00026DA4 File Offset: 0x00024FA4
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		this.cube.RemoveDataListener<Float3>("color", new ValueEntry<Float3>.DataChangeHandler(this.OnColorChange));
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x00026DD4 File Offset: 0x00024FD4
	private void OnColorChange(Float3 pPreviousColor, Float3 pNewColor)
	{
		this.RefreshMaterialColor();
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00026DDC File Offset: 0x00024FDC
	private void RefreshMaterialColor()
	{
		Float3 color = this.cube.color;
		Color color2 = new Color(color.x, color.y, color.z);
		base.GetComponentInChildren<Renderer>().material.color = color2;
		this._light.color = color2;
	}

	// Token: 0x040003E9 RID: 1001
	private Light _light;
}
