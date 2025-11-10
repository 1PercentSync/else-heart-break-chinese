using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000065 RID: 101
	public class Machine : MimanTing
	{
		// Token: 0x060005FF RID: 1535
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "MachineA机器A");
			this.CELL_goodsName = base.EnsureCell<string>("goodsName", "");
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x0001C40C File Offset: 0x0001A60C
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[] { base.localPoint + IntPoint.DirectionToIntPoint(base.direction) * 2 };
			}
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001C448 File Offset: 0x0001A648
		public override void MaybeFixGroupIfOutsideIslandOfTiles()
		{
			base.FixGroupIfOutsideIslandOfTiles();
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001C450 File Offset: 0x0001A650
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x0001C460 File Offset: 0x0001A660
		public IntPoint goodsPoint
		{
			get
			{
				return IntPoint.DirectionToIntPoint(base.direction) * 0;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x0001C474 File Offset: 0x0001A674
		public override bool canBePickedUp
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000605 RID: 1541
		public override string verbDescription
		{
			get
			{
				return "inspect检查";
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000606 RID: 1542
		public override string tooltipName
		{
			get
			{
				return "machine机器";
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x0001C488 File Offset: 0x0001A688
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x0001C498 File Offset: 0x0001A698
		[ShowInEditor]
		public string goodsName
		{
			get
			{
				return this.CELL_goodsName.data;
			}
			set
			{
				this.CELL_goodsName.data = value;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0001C4A8 File Offset: 0x0001A6A8
		// (set) Token: 0x0600060A RID: 1546 RVA: 0x0001C4E0 File Offset: 0x0001A6E0
		public Ting currentGoods
		{
			get
			{
				if (this.goodsName == "")
				{
					return null;
				}
				return this._tingRunner.GetTing(this.goodsName);
			}
			set
			{
				if (value == null)
				{
					this.goodsName = "";
				}
				else
				{
					this.goodsName = value.name;
				}
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001C510 File Offset: 0x0001A710
		public void ProcessGoods(Goods pGoods)
		{
			this.currentGoods = pGoods;
			this.currentGoods.isBeingHeld = false;
			this.currentGoods.position = this.goodsPointInWorld;
			this.masterProgram.executionsPerFrame = 5;
			this.masterProgram.Start();
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001C558 File Offset: 0x0001A758
		public override void Update(float dt)
		{
			if (this.currentGoods == null)
			{
				MimanTing mimanTing = null;
				Ting[] occupants = base.room.GetTile(base.localPoint).GetOccupants();
				foreach (Ting ting in occupants)
				{
					if (!(ting is Machine))
					{
						mimanTing = ting as MimanTing;
						break;
					}
				}
				if (mimanTing != null && !mimanTing.isBeingHeld && !mimanTing.isDeleted)
				{
					this.currentGoods = mimanTing;
					this.masterProgram.executionsPerFrame = 10;
					this.masterProgram.Start();
				}
			}
			else if (this.currentGoods.isBeingHeld)
			{
				D.Log("is being held");
				this.RemovedGoods();
			}
			else if (this.currentGoods.position != this.goodsPointInWorld)
			{
				D.Log(string.Concat(new object[]
				{
					"different pos: ",
					this.currentGoods.position,
					" goods point: ",
					this.goodsPointInWorld
				}));
				this.RemovedGoods();
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x0001C690 File Offset: 0x0001A890
		public override bool autoUnregisterFromUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0001C694 File Offset: 0x0001A894
		public WorldCoordinate goodsPointInWorld
		{
			get
			{
				return new WorldCoordinate(base.room.name, base.localPoint + this.goodsPoint);
			}
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001C6C4 File Offset: 0x0001A8C4
		private void RemovedGoods()
		{
			D.Log(this.currentGoods.name + " was removed from " + base.name);
			this._program.StopAndReset();
			this.currentGoods = null;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001C704 File Offset: 0x0001A904
		private char Improve(char c)
		{
			char c2 = (char)((int)c - Randomizer.GetIntValue(1, 6));
			if (c2 <= 'a')
			{
				return 'a';
			}
			return c2;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0001C728 File Offset: 0x0001A928
		private void RunBlock()
		{
			if (this.onRunBlock != null)
			{
				this.onRunBlock();
			}
		}

		// Token: 0x06000612 RID: 1554
		[SprakAPI(new string[] { "Refine the mineral at a specific position, can accidentally mess up other parts of the mineral chain在特殊的位置提炼矿物，可能意外破坏矿物链中其他部分" })]
		public void API_Refine(float pos)
		{
			if (this.currentGoods == null)
			{
				D.Log("No goods to process in " + base.name);
				return;
			}
			Goods goods = this.currentGoods as Goods;
			if (goods == null)
			{
				return;
			}
			this.RunBlock();
			char c = goods.minerals[(int)pos];
			char c2 = this.Improve(c);
			goods.minerals[(int)pos] = c2;
			if (Randomizer.OneIn(7))
			{
				goods.minerals[(int)pos] = Randomizer.RandNth<char>(Machine.badChars);
			}
			this.masterProgram.sleepTimer = 2f;
		}

		// Token: 0x06000613 RID: 1555
		[SprakAPI(new string[] { "Get an overview of the minerals inside the goods获得货物里的矿物概览" })]
		public string API_Analyze()
		{
			if (this.currentGoods == null)
			{
				return "";
			}
			Goods goods = this.currentGoods as Goods;
			if (goods == null)
			{
				return "Can only analyze goods只能统计货物";
			}
			return goods.mineralsDisplayString;
		}

		// Token: 0x06000614 RID: 1556
		[SprakAPI(new string[] { "Get an estimate of the purity of the goods估计货物的纯度" })]
		public float API_Purity()
		{
			if (this.currentGoods == null)
			{
				return 0f;
			}
			Goods goods = this.currentGoods as Goods;
			if (goods == null)
			{
				return 0f;
			}
			return goods.GetPureness();
		}

		// Token: 0x06000615 RID: 1557
		[SprakAPI(new string[] { "Convert the goods into an object将货物转化为物品" })]
		public string API_Convert()
		{
			if (this.currentGoods == null)
			{
				return "No goods to process没有货物来处理";
			}
			Goods goods = this.currentGoods as Goods;
			if (goods == null)
			{
				return "Can only convert goods只能转化货物";
			}
			this.RunBlock();
			float pureness = goods.GetPureness();
			if (this.onGoodsConverted != null)
			{
				this.onGoodsConverted();
			}
			this._tingRunner.RemoveTingAfterUpdate(this.currentGoods.name);
			this.currentGoods.isDeleted = true;
			this.currentGoods = null;
			this.masterProgram.sleepTimer = 2f;
			if (pureness > 0.9f)
			{
				Hackdev hackdev = this._tingRunner.CreateTing<Hackdev>("Modifier" + this._worldSettings.tickNr.ToString(), base.position, base.direction, "SmallHackdev");
				this.currentGoods = hackdev;
				return hackdev.name;
			}
			if (pureness > 0.7f)
			{
				MysticalCube mysticalCube = this._tingRunner.CreateTing<MysticalCube>("MysticalCube" + this._worldSettings.tickNr.ToString(), base.position, base.direction, "MysticalCube");
				this.currentGoods = mysticalCube;
				return mysticalCube.name;
			}
			if (pureness > 0.5f)
			{
				Key key = this._tingRunner.CreateTing<Key>("Key" + this._worldSettings.tickNr.ToString(), base.position, base.direction, "Old_Key");
				this.currentGoods = key;
				return key.name;
			}
			if (pureness > 0.25f)
			{
				Floppy floppy = this._tingRunner.CreateTing<Floppy>("Floppy" + this._worldSettings.tickNr.ToString(), base.position, base.direction, "Diskette_Diskette" + Randomizer.GetIntValue(1, 10).ToString());
				this.currentGoods = floppy;
				return floppy.name;
			}
			Screwdriver screwdriver = this._tingRunner.CreateTing<Screwdriver>("Screwdriver" + this._worldSettings.tickNr.ToString(), base.position, base.direction, "Screwdriver_Screwdriver");
			this.currentGoods = screwdriver;
			return screwdriver.name;
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0001CA80 File Offset: 0x0001AC80
		// (set) Token: 0x06000617 RID: 1559 RVA: 0x0001CA90 File Offset: 0x0001AC90
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

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x0001CAA0 File Offset: 0x0001ACA0
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					this._program.FunctionDefinitions = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Machine)));
				}
				return this._program;
			}
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0001CAF8 File Offset: 0x0001ACF8
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x04000192 RID: 402
		private ValueEntry<string> CELL_programName;

		// Token: 0x04000193 RID: 403
		private ValueEntry<string> CELL_goodsName;

		// Token: 0x04000194 RID: 404
		private Program _program;

		// Token: 0x04000195 RID: 405
		public Action onRunBlock;

		// Token: 0x04000196 RID: 406
		public Action onGoodsConverted;

		// Token: 0x04000197 RID: 407
		private static char[] badChars = new char[] { 'u', 'v', 'w', 'x', 'y', 'z' };
	}
}
