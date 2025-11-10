using System;
using GameTypes;
using GameWorld2;
using RelayLib;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class LampShell : Shell
{
	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000574 RID: 1396 RVA: 0x00026260 File Offset: 0x00024460
	public Lamp lamp
	{
		get
		{
			return base.ting as Lamp;
		}
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x00026270 File Offset: 0x00024470
	public override void CreateTing()
	{
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x00026274 File Offset: 0x00024474
	protected override void Setup()
	{
		base.Setup();
		this._light = base.GetComponentInChildren<Light>();
		if (this._light == null)
		{
			D.LogError("Can't find light");
		}
		SoundDictionary.LoadSingleSound("Alarm", "FactoryWarningBellSound 0");
		this.lamp.masterProgram.Start();
		this.RefreshLightEnabled();
		this.RefreshMaterialColor();
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x000262DC File Offset: 0x000244DC
	protected override void SetupDataListeners()
	{
		base.SetupDataListeners();
		this.lamp.AddDataListener<Float3>("color", new ValueEntry<Float3>.DataChangeHandler(this.OnColorChange));
		this.lamp.AddDataListener<bool>("on", new ValueEntry<bool>.DataChangeHandler(this.OnOnChange));
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00026328 File Offset: 0x00024528
	protected override void RemoveDataListeners()
	{
		base.RemoveDataListeners();
		this.lamp.RemoveDataListener<Float3>("color", new ValueEntry<Float3>.DataChangeHandler(this.OnColorChange));
		this.lamp.RemoveDataListener<bool>("on", new ValueEntry<bool>.DataChangeHandler(this.OnOnChange));
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x00026374 File Offset: 0x00024574
	private void OnColorChange(Float3 pPreviousColor, Float3 pNewColor)
	{
		this.RefreshMaterialColor();
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0002637C File Offset: 0x0002457C
	private void OnOnChange(bool pPrevios, bool pNew)
	{
		this.RefreshLightEnabled();
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00026384 File Offset: 0x00024584
	private void RefreshLightEnabled()
	{
		this._light.enabled = this.lamp.on;
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0002639C File Offset: 0x0002459C
	private void RefreshMaterialColor()
	{
		Float3 color = this.lamp.color;
		Color color2 = new Color(color.x, color.y, color.z);
		this._light.color = color2;
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x000263E0 File Offset: 0x000245E0
	protected override bool ShouldSnapPosAndDir()
	{
		return false;
	}

	// Token: 0x040003E2 RID: 994
	private Light _light;
}
