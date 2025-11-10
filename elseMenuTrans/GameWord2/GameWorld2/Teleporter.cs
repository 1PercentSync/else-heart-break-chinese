using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000024 RID: 36
	public class Teleporter : MimanTing, TingWithButton
	{
		// Token: 0x06000332 RID: 818 RVA: 0x00012544 File Offset: 0x00010744
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "TeleporterSoftware");
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00012564 File Offset: 0x00010764
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00012574 File Offset: 0x00010774
		public override void Update(float dt)
		{
			base.UpdateBubbleTimer();
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0001257C File Offset: 0x0001077C
		[SprakAPI(new string[] { "Print", "text" })]
		public void API_Print(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x06000336 RID: 822
		[SprakAPI(new string[] { "Get the coordinates of your current position获取你当前位置的坐标" })]
		public string API_Position()
		{
			if (this._user != null)
			{
				return string.Concat(new object[]
				{
					"x: ",
					this._user.localPoint.x,
					", y: ",
					this._user.localPoint.y
				});
			}
			throw new Exception("User is null使用者不存在");
		}

		// Token: 0x06000337 RID: 823
		[SprakAPI(new string[] { "Teleport to another position in the same room. Returns an error message as a string.传送到同一房间的另一位置。返回错误信息字符串", "x", "y" })]
		public string API_Teleport(float x, float y)
		{
			if (!this.IsAllowedToTeleport(this._user as Character))
			{
				D.Log("Not allowed to teleport");
				return "Not allowed不允许";
			}
			WorldCoordinate worldCoordinate = new WorldCoordinate(this._user.room.name, (int)x, (int)y);
			if (this._user.room.GetTile(worldCoordinate.localPosition) != null)
			{
				this._user.position = worldCoordinate;
				return "Success成功";
			}
			return "Can't move there不能移动到那里";
		}

		// Token: 0x06000338 RID: 824
		[SprakAPI(new string[] { "Teleport to another position anywhere in the world, returns status.传送到世界任意位置，返回状态", "room房间", "x", "y" })]
		public string API_SetWorldPosition(string room, float x, float y)
		{
			if (room.Contains("inventory") || room.Contains("locker"))
			{
				return "Can't teleport there不能传送到那里";
			}
			if (!this.IsAllowedToTeleport(this._user as Character))
			{
				D.Log("Not allowed to set world position");
				return "Not allowed不允许";
			}
			if (this._roomRunner.HasRoom(room))
			{
				WorldCoordinate worldCoordinate = new WorldCoordinate(room, (int)x, (int)y);
				this._user.position = worldCoordinate;
				return "Success成功";
			}
			return "Can't find room无法找到房间 '" + room + "'";
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0001271C File Offset: 0x0001091C
		private bool IsAllowedToTeleport(Character pUser)
		{
			return pUser != null && !pUser.talking && pUser.conversationTarget == null;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0001273C File Offset: 0x0001093C
		public void PushButton(Ting pUser)
		{
			this._user = pUser;
			D.Log(base.name + " was activated!");
			this.masterProgram.Start();
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00012768 File Offset: 0x00010968
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600033C RID: 828
		public override string verbDescription
		{
			get
			{
				return "press button按下按钮";
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600033D RID: 829
		public override string tooltipName
		{
			get
			{
				return "teleporting device传送装置";
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0001277C File Offset: 0x0001097C
		// (set) Token: 0x0600033F RID: 831 RVA: 0x0001278C File Offset: 0x0001098C
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

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0001279C File Offset: 0x0001099C
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Teleporter)));
				}
				return this._program;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x000127F4 File Offset: 0x000109F4
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
			this._user = null;
		}

		// Token: 0x040000CD RID: 205
		public new static string TABLE_NAME = "Ting_Teleporters";

		// Token: 0x040000CE RID: 206
		private ValueEntry<string> CELL_programName;

		// Token: 0x040000CF RID: 207
		private Program _program;

		// Token: 0x040000D0 RID: 208
		private Ting _user;
	}
}
