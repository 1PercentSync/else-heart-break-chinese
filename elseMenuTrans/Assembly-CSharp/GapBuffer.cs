using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x020000E3 RID: 227
public class GapBuffer
{
	// Token: 0x06000671 RID: 1649 RVA: 0x0002A8E8 File Offset: 0x00028AE8
	public GapBuffer(string pData)
	{
		this.right.Append(pData);
		this._currentUndoLevel = new GapBuffer.GapBufferUndoLevel();
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x0002A954 File Offset: 0x00028B54
	internal char PopLeft()
	{
		GapBuffer.GapBufferAction gapBufferAction = new GapBuffer.GapBufferAction(' ', GapBuffer.GapBufferAction.ActionType.POP_LEFT);
		char c = gapBufferAction.Do(this.left, this.right);
		this._currentUndoLevel.AddAction(gapBufferAction);
		return c;
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0002A98C File Offset: 0x00028B8C
	internal char PopRight()
	{
		GapBuffer.GapBufferAction gapBufferAction = new GapBuffer.GapBufferAction(' ', GapBuffer.GapBufferAction.ActionType.POP_RIGHT);
		char c = gapBufferAction.Do(this.left, this.right);
		this._currentUndoLevel.AddAction(gapBufferAction);
		return c;
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0002A9C4 File Offset: 0x00028BC4
	internal void PushRight(char c)
	{
		GapBuffer.GapBufferAction gapBufferAction = new GapBuffer.GapBufferAction(c, GapBuffer.GapBufferAction.ActionType.PUSH_RIGHT);
		gapBufferAction.Do(this.left, this.right);
		this._currentUndoLevel.AddAction(gapBufferAction);
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0002A9FC File Offset: 0x00028BFC
	internal void PushLeft(char c)
	{
		GapBuffer.GapBufferAction gapBufferAction = new GapBuffer.GapBufferAction(c, GapBuffer.GapBufferAction.ActionType.PUSH_LEFT);
		gapBufferAction.Do(this.left, this.right);
		this._currentUndoLevel.AddAction(gapBufferAction);
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0002AA34 File Offset: 0x00028C34
	internal void BeginRecord()
	{
		if (this._currentUndoLevel == null || this._currentUndoLevel.commandCount > 0)
		{
			this._currentUndoLevel = new GapBuffer.GapBufferUndoLevel();
		}
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0002AA60 File Offset: 0x00028C60
	internal bool EndRecord()
	{
		if (this._currentUndoLevel.commandCount > 0)
		{
			if (this._future.Count > 0)
			{
				this._future.Clear();
			}
			this._history.Push(this._currentUndoLevel);
			this._dirty = true;
			return true;
		}
		return false;
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x0002AAB8 File Offset: 0x00028CB8
	public void Redo()
	{
		if (this._future.Count > 0)
		{
			GapBuffer.GapBufferUndoLevel gapBufferUndoLevel = this._future.Pop();
			gapBufferUndoLevel.Redo(this.left, this.right);
			this._history.Push(gapBufferUndoLevel);
			this._dirty = true;
		}
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0002AB08 File Offset: 0x00028D08
	public void Undo()
	{
		if (this._history.Count > 0)
		{
			GapBuffer.GapBufferUndoLevel gapBufferUndoLevel = this._history.Pop();
			gapBufferUndoLevel.Undo(this.left, this.right);
			this._future.Push(gapBufferUndoLevel);
			this._dirty = true;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x0600067A RID: 1658 RVA: 0x0002AB58 File Offset: 0x00028D58
	public string data
	{
		get
		{
			if (this._dirty)
			{
				char[] array = this.left.ToString().ToCharArray();
				Array.Reverse(array);
				this._data = new string(array) + this.right.ToString();
				this._dirty = false;
			}
			return this._data;
		}
	}

	// Token: 0x0400043C RID: 1084
	private bool _dirty = true;

	// Token: 0x0400043D RID: 1085
	private string _data = string.Empty;

	// Token: 0x0400043E RID: 1086
	public StringBuilder left = new StringBuilder();

	// Token: 0x0400043F RID: 1087
	public StringBuilder right = new StringBuilder();

	// Token: 0x04000440 RID: 1088
	private Stack<GapBuffer.GapBufferUndoLevel> _history = new Stack<GapBuffer.GapBufferUndoLevel>();

	// Token: 0x04000441 RID: 1089
	private Stack<GapBuffer.GapBufferUndoLevel> _future = new Stack<GapBuffer.GapBufferUndoLevel>();

	// Token: 0x04000442 RID: 1090
	private GapBuffer.GapBufferUndoLevel _currentUndoLevel;

	// Token: 0x020000E4 RID: 228
	internal struct GapBufferAction
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x0002ABB0 File Offset: 0x00028DB0
		internal GapBufferAction(char pC, GapBuffer.GapBufferAction.ActionType pType)
		{
			this.actionType = pType;
			this.c = pC;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x0002ABC0 File Offset: 0x00028DC0
		internal GapBuffer.GapBufferAction.ActionType reverseActionType
		{
			get
			{
				switch (this.actionType)
				{
				case GapBuffer.GapBufferAction.ActionType.PUSH_LEFT:
					return GapBuffer.GapBufferAction.ActionType.POP_LEFT;
				case GapBuffer.GapBufferAction.ActionType.PUSH_RIGHT:
					return GapBuffer.GapBufferAction.ActionType.POP_RIGHT;
				case GapBuffer.GapBufferAction.ActionType.POP_LEFT:
					return GapBuffer.GapBufferAction.ActionType.PUSH_LEFT;
				case GapBuffer.GapBufferAction.ActionType.POP_RIGHT:
					return GapBuffer.GapBufferAction.ActionType.PUSH_RIGHT;
				default:
					throw new Exception("asdas");
				}
			}
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0002AC04 File Offset: 0x00028E04
		internal char Do(StringBuilder pLeft, StringBuilder pRight)
		{
			return this.Do(pLeft, pRight, this.actionType);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0002AC14 File Offset: 0x00028E14
		internal char Do(StringBuilder pLeft, StringBuilder pRight, GapBuffer.GapBufferAction.ActionType pType)
		{
			switch (pType)
			{
			case GapBuffer.GapBufferAction.ActionType.PUSH_LEFT:
				pLeft.Insert(0, this.c);
				return this.c;
			case GapBuffer.GapBufferAction.ActionType.PUSH_RIGHT:
				pRight.Insert(0, this.c);
				return this.c;
			case GapBuffer.GapBufferAction.ActionType.POP_LEFT:
				if (pLeft.Length <= 0)
				{
					return ' ';
				}
				this.c = pLeft[0];
				pLeft.Remove(0, 1);
				return this.c;
			case GapBuffer.GapBufferAction.ActionType.POP_RIGHT:
				if (pRight.Length <= 0)
				{
					return ' ';
				}
				this.c = pRight[0];
				pRight.Remove(0, 1);
				return this.c;
			default:
				throw new Exception("Gap buffer did wrong");
			}
		}

		// Token: 0x04000443 RID: 1091
		internal char c;

		// Token: 0x04000444 RID: 1092
		internal GapBuffer.GapBufferAction.ActionType actionType;

		// Token: 0x020000E5 RID: 229
		internal enum ActionType
		{
			// Token: 0x04000446 RID: 1094
			PUSH_LEFT,
			// Token: 0x04000447 RID: 1095
			PUSH_RIGHT,
			// Token: 0x04000448 RID: 1096
			POP_LEFT,
			// Token: 0x04000449 RID: 1097
			POP_RIGHT
		}
	}

	// Token: 0x020000E6 RID: 230
	internal class GapBufferUndoLevel
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0002ACE0 File Offset: 0x00028EE0
		internal int commandCount
		{
			get
			{
				return this._commands.Count;
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0002ACF0 File Offset: 0x00028EF0
		public void AddAction(GapBuffer.GapBufferAction pAction)
		{
			this._commands.Add(pAction);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0002AD00 File Offset: 0x00028F00
		public void Undo(StringBuilder pLeft, StringBuilder pRight)
		{
			for (int i = this._commands.Count - 1; i >= 0; i--)
			{
				this._commands[i].Do(pLeft, pRight, this._commands[i].reverseActionType);
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0002AD58 File Offset: 0x00028F58
		public void Redo(StringBuilder pLeft, StringBuilder pRight)
		{
			for (int i = 0; i < this._commands.Count; i++)
			{
				this._commands[i].Do(pLeft, pRight);
			}
		}

		// Token: 0x0400044A RID: 1098
		private List<GapBuffer.GapBufferAction> _commands = new List<GapBuffer.GapBufferAction>();
	}
}
