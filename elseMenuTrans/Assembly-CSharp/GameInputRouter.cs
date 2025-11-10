using System;
using System.Collections.Generic;

// Token: 0x02000036 RID: 54
public class GameInputRouter : IInputFilter
{
	// Token: 0x06000241 RID: 577 RVA: 0x00011248 File Offset: 0x0000F448
	public void RegisterInputHandler(IInputHandler pHandler)
	{
		if (!this._inputHandlers.Contains(pHandler))
		{
			this._inputHandlers.Add(pHandler);
		}
	}

	// Token: 0x06000242 RID: 578 RVA: 0x00011268 File Offset: 0x0000F468
	public void UnregisterInputHandler(IInputHandler pHandler)
	{
		this._inputHandlers.Remove(pHandler);
	}

	// Token: 0x06000243 RID: 579 RVA: 0x00011278 File Offset: 0x0000F478
	public void RegisterInputGenerator(IInputGenerator pGenerator)
	{
		pGenerator.InputSender += this.ProcessInput;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0001128C File Offset: 0x0000F48C
	public void UnRegisterInputGenerator(IInputGenerator pGenerator)
	{
		pGenerator.InputSender -= this.ProcessInput;
	}

	// Token: 0x06000245 RID: 581 RVA: 0x000112A0 File Offset: 0x0000F4A0
	public void ProcessInput(IInputMessage pMessage)
	{
		if (this.InputStatus(pMessage.name))
		{
			pMessage.Accept();
			foreach (IInputHandler inputHandler in this._inputHandlers)
			{
				inputHandler.OnGameInput(pMessage);
			}
		}
		else
		{
			pMessage.Decline();
		}
	}

	// Token: 0x06000246 RID: 582 RVA: 0x00011328 File Offset: 0x0000F528
	public void EnableInput(string pName)
	{
		this._inputFilter[pName] = true;
	}

	// Token: 0x06000247 RID: 583 RVA: 0x00011338 File Offset: 0x0000F538
	public void DisableInput(string pName)
	{
		this._inputFilter[pName] = false;
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00011348 File Offset: 0x0000F548
	public bool InputStatus(string pName)
	{
		bool flag;
		return !this._inputFilter.TryGetValue(pName, out flag) || flag;
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0001136C File Offset: 0x0000F56C
	public void DisableAll()
	{
		foreach (string text in this._inputFilter.Keys)
		{
			this._inputFilter[text] = false;
		}
	}

	// Token: 0x0600024A RID: 586 RVA: 0x000113E0 File Offset: 0x0000F5E0
	public string[] GetFilterKeys()
	{
		List<string> list = new List<string>();
		foreach (string text in this._inputFilter.Keys)
		{
			list.Add(text);
		}
		return list.ToArray();
	}

	// Token: 0x0400016A RID: 362
	private List<IInputHandler> _inputHandlers = new List<IInputHandler>();

	// Token: 0x0400016B RID: 363
	private Dictionary<string, bool> _inputFilter = new Dictionary<string, bool>();

	// Token: 0x020000FC RID: 252
	// (Invoke) Token: 0x0600074F RID: 1871
	public delegate void InputHandlerFunction(IInputMessage pMessage);
}
