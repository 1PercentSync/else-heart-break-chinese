using System;
using System.Linq;
using ProgrammingLanguageNr1;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000053 RID: 83
	public class MemoryAPI
	{
		// Token: 0x06000549 RID: 1353 RVA: 0x00019BA4 File Offset: 0x00017DA4
		public MemoryAPI(Computer pComputer, TingRunner pTingRunner)
		{
			this._computer = pComputer;
			this._tingRunner = pTingRunner;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00019BBC File Offset: 0x00017DBC
		[SprakAPI(new string[] { "Save something to the active memory unit", "key", "value to save" })]
		public object SaveMemory(object[] args)
		{
			object obj = args[1];
			if (obj.GetType() == typeof(string) || obj.GetType() == typeof(float) || obj.GetType() == typeof(bool))
			{
				this._computer.EnsureMemoryUnit();
				string text = ReturnValueConversions.SafeUnwrap<string>(args, 0);
				this._computer.memory[text] = obj;
				return VoidType.voidType;
			}
			throw new Error("Can't save value of type " + ReturnValueConversions.PrettyObjectType(obj.GetType()) + " in memory");
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00019C60 File Offset: 0x00017E60
		[SprakAPI(new string[] { "Retreive something from the active memory unit", "key" })]
		public object LoadMemory(object[] args)
		{
			this._computer.EnsureMemoryUnit();
			string text = ReturnValueConversions.SafeUnwrap<string>(args, 0);
			object obj;
			if (!this._computer.memory.data.TryGetValue(text, out obj))
			{
				this._computer.API_Print("Can't load memory with key '" + text + "'");
				return 0f;
			}
			Type type = obj.GetType();
			if (type == typeof(float))
			{
				return (float)obj;
			}
			if (type == typeof(double))
			{
				return (float)((double)obj);
			}
			if (type == typeof(int))
			{
				return (float)((int)obj);
			}
			if (type == typeof(string))
			{
				return obj;
			}
			if (type == typeof(bool))
			{
				return obj;
			}
			throw new Error("Can't load memory of type " + ReturnValueConversions.PrettyObjectType(type) + " from memory");
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00019D60 File Offset: 0x00017F60
		[SprakAPI(new string[] { "Connect to an external hard drive", "name" })]
		public void API_HD(string memoryUnitName)
		{
			if (memoryUnitName == "")
			{
				this._computer.memory = null;
				this._computer.API_Print("Disconnected from external memory unit");
				return;
			}
			Memory memory = this._tingRunner.GetTingUnsafe(memoryUnitName) as Memory;
			if (memory != null)
			{
				this._computer.memory = memory;
				this._computer.API_Print("Connected to " + memoryUnitName);
				return;
			}
			throw new Error("Can't connect to external memory unit '" + memoryUnitName + "'");
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00019DF0 File Offset: 0x00017FF0
		[SprakAPI(new string[] { "Get the keys of all entries in the memory unit" })]
		public object[] API_GetMemories()
		{
			this._computer.EnsureMemoryUnit();
			return this._computer.memory.data.Keys.ToArray<string>();
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00019E24 File Offset: 0x00018024
		[SprakAPI(new string[] { "Remove all memories from the memory unit" })]
		public void API_ClearMemories()
		{
			this._computer.EnsureMemoryUnit();
			this._computer.memory.data.Clear();
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00019E54 File Offset: 0x00018054
		[SprakAPI(new string[] { "Remove a specific memory", "The key" })]
		public void API_EraseMemory(string key)
		{
			this._computer.EnsureMemoryUnit();
			this._computer.memory.data.Remove(key);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00019E84 File Offset: 0x00018084
		[SprakAPI(new string[] { "Does the computer have the memory with a certain key?", "The key" })]
		public bool API_HasMemory(string key)
		{
			this._computer.EnsureMemoryUnit();
			return this._computer.memory.data.ContainsKey(key);
		}

		// Token: 0x0400015F RID: 351
		private Computer _computer;

		// Token: 0x04000160 RID: 352
		private TingRunner _tingRunner;
	}
}
