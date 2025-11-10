using System;
using System.Collections.Generic;
using GameTypes;
using ProgrammingLanguageNr1;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000060 RID: 96
	public class Pawn : MimanTing
	{
		// Token: 0x060005B0 RID: 1456 RVA: 0x0001B564 File Offset: 0x00019764
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_programName = base.EnsureCell<string>("masterProgramName", "Pawn");
			this.CELL_startPosition = base.EnsureCell<WorldCoordinate>("startPosition", WorldCoordinate.NONE);
			this.CELL_startRotation = base.EnsureCell<Direction>("startRotation", Direction.DOWN);
			this.CELL_dead = base.EnsureCell<bool>("dead", false);
			this.CELL_moveNr = base.EnsureCell<int>("moveNr", 0);
			this.CELL_team = base.EnsureCell<int>("team", 0);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001B5EC File Offset: 0x000197EC
		public override void Update(float dt)
		{
			base.UpdateBubbleTimer();
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001B5F4 File Offset: 0x000197F4
		public override bool DoesMasterProgramExist()
		{
			return this._program != null;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001B604 File Offset: 0x00019804
		public override void FixBeforeSaving()
		{
			base.FixBeforeSaving();
			this.startPosition = base.position;
			this.startRotation = base.direction;
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0001B630 File Offset: 0x00019830
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x0001B634 File Offset: 0x00019834
		public override string tooltipName
		{
			get
			{
				return base.name;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0001B63C File Offset: 0x0001983C
		public override string verbDescription
		{
			get
			{
				return "";
			}
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001B644 File Offset: 0x00019844
		[SprakAPI(new string[] { "Resurrect if dead and move back to start position" })]
		public void API_Reset()
		{
			D.Log("Reset was called on pawn " + base.name);
			base.position = this.startPosition;
			base.direction = this.startRotation;
			this.dead = false;
			this.moveNr = 0;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001B68C File Offset: 0x0001988C
		[SprakAPI(new string[] { "Turn left" })]
		public void API_TurnLeft()
		{
			if (this.dead)
			{
				return;
			}
			base.TurnDegrees(90);
			this.moveNr++;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001B6BC File Offset: 0x000198BC
		[SprakAPI(new string[] { "Turn right" })]
		public void API_TurnRight()
		{
			if (this.dead)
			{
				return;
			}
			base.TurnDegrees(-90);
			this.moveNr++;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001B6EC File Offset: 0x000198EC
		[SprakAPI(new string[] { "How many moves has the pawn made?" })]
		public int API_GetMoveNr()
		{
			return this.moveNr;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001B6F4 File Offset: 0x000198F4
		[SprakAPI(new string[] { "Modulus" })]
		public int API_Modulus(float a, float b)
		{
			return (int)a % (int)b;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001B6FC File Offset: 0x000198FC
		[SprakAPI(new string[] { "Log" })]
		public void API_Log(string pText)
		{
			D.Log(pText);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001B704 File Offset: 0x00019904
		[SprakAPI(new string[] { "Print text" })]
		public void API_Print(string text)
		{
			this.Say(text, "");
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001B714 File Offset: 0x00019914
		[SprakAPI(new string[] { "Can move one step forward?" })]
		public bool API_CanMove()
		{
			WorldCoordinate worldCoordinate = new WorldCoordinate(base.room.name, base.localPoint + IntPoint.DirectionToIntPoint(base.direction));
			PointTileNode tile = this._roomRunner.GetRoom(base.room.name).GetTile(worldCoordinate.localPosition);
			if (tile == null)
			{
				return false;
			}
			if (tile.GetOccupants().Length > 0)
			{
				foreach (Ting ting in tile.GetOccupants())
				{
					if (ting is Pawn)
					{
						Pawn pawn = ting as Pawn;
						if (pawn.team == this.team)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001B7CC File Offset: 0x000199CC
		[SprakAPI(new string[] { "Get a random number between 0.0 and 1.0" })]
		public float API_Random()
		{
			return Randomizer.GetValue(0f, 1f);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001B7E0 File Offset: 0x000199E0
		[SprakAPI(new string[] { "Pause the master program", "number of seconds to pause for" })]
		public void API_Sleep(float seconds)
		{
			this.masterProgram.sleepTimer = seconds;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001B7F0 File Offset: 0x000199F0
		[SprakAPI(new string[] { "Move forward one step" })]
		public void API_Move()
		{
			if (this.dead)
			{
				return;
			}
			WorldCoordinate worldCoordinate = new WorldCoordinate(base.room.name, base.localPoint + IntPoint.DirectionToIntPoint(base.direction));
			PointTileNode tile = this._roomRunner.GetRoom(base.room.name).GetTile(worldCoordinate.localPosition);
			if (tile != null)
			{
				if (tile.GetOccupants().Length > 0)
				{
					foreach (Ting ting in tile.GetOccupants())
					{
						if (ting is Pawn)
						{
							Pawn pawn = ting as Pawn;
							if (pawn.team != this.team && !pawn.dead)
							{
								pawn.GetHit();
								base.PlaySound("FishAttack");
							}
						}
					}
				}
				else
				{
					base.position = worldCoordinate;
				}
			}
			this.moveNr++;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001B8EC File Offset: 0x00019AEC
		private void GetHit()
		{
			this.dead = true;
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0001B8F8 File Offset: 0x00019AF8
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x0001B908 File Offset: 0x00019B08
		[EditableInEditor]
		public int moveNr
		{
			get
			{
				return this.CELL_moveNr.data;
			}
			set
			{
				this.CELL_moveNr.data = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0001B918 File Offset: 0x00019B18
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x0001B928 File Offset: 0x00019B28
		[EditableInEditor]
		public bool dead
		{
			get
			{
				return this.CELL_dead.data;
			}
			set
			{
				this.CELL_dead.data = value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0001B938 File Offset: 0x00019B38
		// (set) Token: 0x060005C8 RID: 1480 RVA: 0x0001B948 File Offset: 0x00019B48
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

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0001B958 File Offset: 0x00019B58
		// (set) Token: 0x060005CA RID: 1482 RVA: 0x0001B968 File Offset: 0x00019B68
		[EditableInEditor]
		public int team
		{
			get
			{
				return this.CELL_team.data;
			}
			set
			{
				this.CELL_team.data = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0001B978 File Offset: 0x00019B78
		public override Program masterProgram
		{
			get
			{
				if (this._program == null)
				{
					this._program = base.EnsureProgram("MasterProgram", this.masterProgramName);
					List<FunctionDefinition> list = new List<FunctionDefinition>(FunctionDefinitionCreator.CreateDefinitions(this, typeof(Pawn)));
					list.AddRange(FunctionDefinitionCreator.CreateDefinitions(new ConnectionAPI(this, this._tingRunner, this.masterProgram), typeof(ConnectionAPI)));
					this._program.FunctionDefinitions = list;
				}
				this._program.executionsPerFrame = 10;
				return this._program;
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001BA04 File Offset: 0x00019C04
		public override void PrepareForBeingHacked()
		{
			if (this.masterProgram == null)
			{
				this.logger.Log("There was a problem generating the master program");
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0001BA24 File Offset: 0x00019C24
		// (set) Token: 0x060005CE RID: 1486 RVA: 0x0001BA34 File Offset: 0x00019C34
		[ShowInEditor]
		public WorldCoordinate startPosition
		{
			get
			{
				return this.CELL_startPosition.data;
			}
			set
			{
				this.CELL_startPosition.data = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0001BA44 File Offset: 0x00019C44
		// (set) Token: 0x060005D0 RID: 1488 RVA: 0x0001BA54 File Offset: 0x00019C54
		[ShowInEditor]
		public Direction startRotation
		{
			get
			{
				return this.CELL_startRotation.data;
			}
			set
			{
				this.CELL_startRotation.data = value;
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001BA64 File Offset: 0x00019C64
		public override void OnPutDown()
		{
			D.Log(base.name + " will start program when put down!");
			this.masterProgram.Start();
		}

		// Token: 0x0400017D RID: 381
		public new static string TABLE_NAME = "Ting_Pawns";

		// Token: 0x0400017E RID: 382
		private ValueEntry<string> CELL_programName;

		// Token: 0x0400017F RID: 383
		private ValueEntry<WorldCoordinate> CELL_startPosition;

		// Token: 0x04000180 RID: 384
		private ValueEntry<Direction> CELL_startRotation;

		// Token: 0x04000181 RID: 385
		private ValueEntry<bool> CELL_dead;

		// Token: 0x04000182 RID: 386
		private ValueEntry<int> CELL_moveNr;

		// Token: 0x04000183 RID: 387
		private ValueEntry<int> CELL_team;

		// Token: 0x04000184 RID: 388
		private Program _program;
	}
}
