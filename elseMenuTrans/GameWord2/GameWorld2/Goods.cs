using System;
using System.Text;
using GameTypes;
using RelayLib;
using TingTing;

namespace GameWorld2
{
	// Token: 0x02000066 RID: 102
	public class Goods : MimanTing
	{
		// Token: 0x0600061C RID: 1564 RVA: 0x0001CB2C File Offset: 0x0001AD2C
		protected override void SetupCells()
		{
			base.SetupCells();
			this.CELL_minerals = base.EnsureCell<char[]>("minerals", "zzzzzzzzzzzzzzzz".ToCharArray());
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001CB5C File Offset: 0x0001AD5C
		private string GetRandomMineralString()
		{
			string text = "abcdefghijklmnopqrstuvwxyz";
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 16; i++)
			{
				stringBuilder.Append(text[Randomizer.GetIntValue(0, text.Length)]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001CBA8 File Offset: 0x0001ADA8
		public void RandomizeMinerals()
		{
			this.minerals = this.GetRandomMineralString().ToCharArray();
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001CBBC File Offset: 0x0001ADBC
		public override void FixBeforeSaving()
		{
			this.RandomizeMinerals();
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001CBC4 File Offset: 0x0001ADC4
		public override bool DoesMasterProgramExist()
		{
			return false;
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x0001CBC8 File Offset: 0x0001ADC8
		public override IntPoint[] interactionPoints
		{
			get
			{
				return new IntPoint[]
				{
					base.localPoint + IntPoint.Up * 1,
					base.localPoint + IntPoint.Right * 1,
					base.localPoint + IntPoint.Left * 1,
					base.localPoint + IntPoint.Down * 1
				};
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0001CC64 File Offset: 0x0001AE64
		public override bool canBePickedUp
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000623 RID: 1571
		public override string verbDescription
		{
			get
			{
				return "inspect检查";
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000624 RID: 1572
		public override string tooltipName
		{
			get
			{
				return "goods货物";
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001CC78 File Offset: 0x0001AE78
		public override bool CanInteractWith(Ting pTingToInteractWith)
		{
			return pTingToInteractWith is SendPipe || pTingToInteractWith is Stove;
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x0001CC94 File Offset: 0x0001AE94
		[ShowInEditor]
		public string mineralsDisplayString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (char c in this.CELL_minerals.data)
				{
					stringBuilder.Append(c);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0001CCDC File Offset: 0x0001AEDC
		// (set) Token: 0x06000628 RID: 1576 RVA: 0x0001CCEC File Offset: 0x0001AEEC
		public char[] minerals
		{
			get
			{
				return this.CELL_minerals.data;
			}
			set
			{
				this.CELL_minerals.data = value;
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001CCFC File Offset: 0x0001AEFC
		public float GetPureness()
		{
			float num = 0f;
			foreach (char c in this.minerals)
			{
				if (c == 'a')
				{
					num += 1f;
				}
				else if (c == 'b')
				{
					num += 0.75f;
				}
				else if (c == 'c')
				{
					num += 0.5f;
				}
				else if (c == 'd')
				{
					num += 0.4f;
				}
				else if (c == 'e')
				{
					num += 0.3f;
				}
				else if (c == 'f')
				{
					num += 0.2f;
				}
				else if (c == 'g')
				{
					num += 0.1f;
				}
			}
			return num / (float)this.minerals.Length;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x0001CDCC File Offset: 0x0001AFCC
		public override Program masterProgram
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000198 RID: 408
		public new static string TABLE_NAME = "Tings_Goods";

		// Token: 0x04000199 RID: 409
		private ValueEntry<char[]> CELL_minerals;
	}
}
