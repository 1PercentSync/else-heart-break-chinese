using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x0200000D RID: 13
	public class Key : MimanTing
	{
		// Token: 0x0600012D RID: 301
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "BlankSlate空白钥匙");
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00008448 File Offset: 0x00006648
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000844C File Offset: 0x0000664C
		public override string verbDescription
		{
			get
			{
				return "use使用";
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00008454 File Offset: 0x00006654
		public override string tooltipName
		{
			get
			{
				return this.masterProgramName;
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000845C File Offset: 0x0000665C
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x06000132 RID: 306
		public override string UseTingOnTingDescription(Ting pOtherTing)
		{
			if (pOtherTing is Door)
			{
				return ((!(pOtherTing as Door).isLocked) ? "lock锁上" : "unlock解锁") + " door门";
			}
			return base.UseTingOnTingDescription(pOtherTing);
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000133 RID: 307 RVA: 0x000084B8 File Offset: 0x000066B8
		// (set) Token: 0x06000134 RID: 308 RVA: 0x000084C8 File Offset: 0x000066C8
		[EditableInEditor]
		public string masterProgramName
		{
			get
			{
				return this.CELL_programName.data;
			}
			set
			{
				this.CELL_programName.data = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000084D8 File Offset: 0x000066D8
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Key)));
				}
				return this._program;
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00008530 File Offset: 0x00006730
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00008550 File Offset: 0x00006750
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Door || pTingToInteractWith is Locker || pTingToInteractWith is TrashCan;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00008580 File Offset: 0x00006780
		public override void InteractWith(Ting pTingToInteractWith)
		{
			Door door = pTingToInteractWith as Door;
			if (door == null)
			{
				D.Log("The ting key is interacting with is not a door.");
				return;
			}
			base.actionOtherObject = door;
			this.masterProgram.executionsPerFrame = 50;
			this.masterProgram.maxExecutionTime = -1f;
			this.masterProgram.Start();
		}

		// Token: 0x06000139 RID: 313
		[SprakAPI(new string[] { "Unlock, returns true on success解锁，成功则返回真" })]
		public bool API_Unlock(float code)
		{
			Door door = base.actionOtherObject as Door;
			if (door == null)
			{
				D.Log("actionOtherObject is not a Door");
				return false;
			}
			return door.Unlock(code);
		}

		// Token: 0x0600013A RID: 314
		[SprakAPI(new string[] { "Lock, returns true on success上锁，成功则返回真" })]
		public bool API_Lock(float code)
		{
			Door door = base.actionOtherObject as Door;
			if (door == null)
			{
				D.Log("actionOtherObject is not a Door");
				return false;
			}
			return door.Lock(code);
		}

		// Token: 0x0600013B RID: 315
		[SprakAPI(new string[] { "Lock or unlock depending on if the door is locked根据门的状态上锁或解锁" })]
		public bool API_Toggle(float code)
		{
			Door door = base.actionOtherObject as Door;
			if (door == null)
			{
				D.Log("actionOtherObject is not a Door");
				return false;
			}
			if (door.isLocked)
			{
				return door.Unlock(code);
			}
			return door.Lock(code);
		}

		// Token: 0x04000061 RID: 97
		public new static string TABLE_NAME = "Tings_Keys";

		// Token: 0x04000062 RID: 98
		private ValueEntry<string> CELL_programName;

		// Token: 0x04000063 RID: 99
		private Program _program;
	}
}
