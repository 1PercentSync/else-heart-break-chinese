using System;

// Token: 0x02000037 RID: 55
public interface IInputFilter
{
	// Token: 0x0600024B RID: 587
	void EnableInput(string pName);

	// Token: 0x0600024C RID: 588
	void DisableInput(string pName);

	// Token: 0x0600024D RID: 589
	bool InputStatus(string pName);

	// Token: 0x0600024E RID: 590
	void DisableAll();

	// Token: 0x0600024F RID: 591
	string[] GetFilterKeys();
}
