using System;

// Token: 0x02000035 RID: 53
[Serializable]
public class SerializableKeyValuePair
{
	// Token: 0x0600023E RID: 574 RVA: 0x00011208 File Offset: 0x0000F408
	public SerializableKeyValuePair()
	{
	}

	// Token: 0x0600023F RID: 575 RVA: 0x00011210 File Offset: 0x0000F410
	public SerializableKeyValuePair(string pKey, float pValue)
	{
		this.Key = pKey;
		this.Value = pValue;
	}

	// Token: 0x04000168 RID: 360
	public string Key;

	// Token: 0x04000169 RID: 361
	public float Value;
}
