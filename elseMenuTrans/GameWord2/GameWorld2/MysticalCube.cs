using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000012 RID: 18
	public class MysticalCube : MimanTing, TingWithButton
	{
		// Token: 0x06000197 RID: 407 RVA: 0x00009640 File Offset: 0x00007840
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00009650 File Offset: 0x00007850
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_mysteryLevel = base.EnsureCell<int>("mysteryLevel", 0);
			this.CELL_onInteractionProgramName = base.EnsureCell<string>("onInteractionProgramName", "TheCube");
			this.CELL_color = base.EnsureCell<Float3>("color", new Float3(0f, 0f, 0f));
		}

		// Token: 0x06000199 RID: 409
		[SprakAPI(new string[] { "Increase mystery level by any amount提升任意数量的神秘等级", "The amount" })]
		public void API_IncreaseMysteryLevel(float amount)
		{
			this.logger.Log("Called API_IncreaseMysteryLevel with argument " + amount.ToString());
			this.mysteryLevel += (int)amount;
		}

		// Token: 0x0600019A RID: 410
		[SprakAPI(new string[] { "Set the color of the cube设置立方体的颜色", "Amount of red (0 - 1)红色值 (0 - 1)", "Amount of green (0 - 1)绿色值 (0 - 1)", "Amount of blue (0 - 1)蓝色值 (0 - 1)" })]
		public void API_SetColor(float r, float g, float b)
		{
			this.logger.Log(string.Concat(new object[] { "Called API_SetColor with arguments ", r, ", ", g, ", ", b }));
			this.color = new Float3(r, g, b);
		}

		// Token: 0x0600019B RID: 411
		[SprakAPI(new string[] { "Get a random value between 0 and 1获取 0 到 1 之间的随机值", "" })]
		public float API_Random()
		{
			return (float)MysticalCube.s_random.NextDouble();
		}

		// Token: 0x0600019C RID: 412
		[SprakAPI(new string[] { "Play a sound播放一段音效", "name" })]
		public void API_PlaySound(string name)
		{
			base.PlaySound(name);
			base.audioLoop = false;
		}

		// Token: 0x0600019D RID: 413
		[SprakAPI(new string[] { "The sinus function正弦函数", "x" })]
		public float API_Sin(float x)
		{
			return (float)Math.Sin((double)x);
		}

		// Token: 0x0600019E RID: 414
		[SprakAPI(new string[] { "The cosinus function余弦函数", "x" })]
		public float API_Cos(float x)
		{
			return (float)Math.Cos((double)x);
		}

		// Token: 0x0600019F RID: 415
		[SprakAPI(new string[] { "Get the total time as a float获取总时间-浮点数格式" })]
		public float API_Time()
		{
			return this._worldSettings.totalWorldTime;
		}

		// Token: 0x060001A0 RID: 416
		[SprakAPI(new string[] { "Pause the master program暂停主程序", "number of seconds to pause for暂停的秒数" })]
		public void API_Sleep(float seconds)
		{
			this.masterProgram.sleepTimer = seconds;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000097B0 File Offset: 0x000079B0
		public void PushButton(Ting pUser)
		{
			this.logger.Log("PushMysteriousButton()");
			this.masterProgram.Start();
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x000097D0 File Offset: 0x000079D0
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001A3 RID: 419
		public override string verbDescription
		{
			get
			{
				return "push button按下按钮";
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001A4 RID: 420
		public override string tooltipName
		{
			get
			{
				return "mystical Cube神秘立方体";
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000097E4 File Offset: 0x000079E4
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x000097F4 File Offset: 0x000079F4
		[ShowInEditor]
		public int mysteryLevel
		{
			get
			{
				return this.CELL_mysteryLevel.data;
			}
			set
			{
				this.CELL_mysteryLevel.data = value;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00009804 File Offset: 0x00007A04
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00009814 File Offset: 0x00007A14
		[ShowInEditor]
		public Float3 color
		{
			get
			{
				return this.CELL_color.data;
			}
			set
			{
				this.CELL_color.data = value;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00009824 File Offset: 0x00007A24
		// (set) Token: 0x060001AA RID: 426 RVA: 0x00009834 File Offset: 0x00007A34
		[EditableInEditor]
		public string onInteractionSourceCodeName
		{
			get
			{
				return this.CELL_onInteractionProgramName.data;
			}
			set
			{
				this.CELL_onInteractionProgramName.data = value;
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009844 File Offset: 0x00007A44
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating an onInteractionProgram");
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00009864 File Offset: 0x00007A64
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is Locker;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00009870 File Offset: 0x00007A70
		public override Program masterProgram
		{
			get
			{
				this._program = base.EnsureProgram("MasterProgram", this.onInteractionSourceCodeName);
				List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(MysticalCube)));
				this._program.FunctionDefinitions = list;
				return this._program;
			}
		}

		// Token: 0x04000077 RID: 119
		public new static string TABLE_NAME = "Ting_MysicalCubes";

		// Token: 0x04000078 RID: 120
		private ValueEntry<int> CELL_mysteryLevel;

		// Token: 0x04000079 RID: 121
		private ValueEntry<string> CELL_onInteractionProgramName;

		// Token: 0x0400007A RID: 122
		private ValueEntry<Float3> CELL_color;

		// Token: 0x0400007B RID: 123
		private Program _program;

		// Token: 0x0400007C RID: 124
		private static Random s_random = new Random(DateTime.Today.Millisecond * DateTime.Today.Second * DateTime.Today.Minute * DateTime.Today.Hour);
	}
}
