using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Definition
{
	public sealed class Element : IComparable<Element>, IEquatable<Element>, IEquatable<int>
	{
		public Element(int gridIndex, int elementIndex)
		{
			this.value = null;
			this.gridIndex = gridIndex;
			this.elementIndex = elementIndex;
		}

		private int gridIndex;
		public int GridIndex
		{
			get { return this.gridIndex; }
		}

		private int elementIndex;
		public int Index
		{
			get { return this.elementIndex; }
		}

		private int? check(int value)
		{
			if (value >= 1 && value <= 9)
				return value;
			return null;
		}

		private int? value;
		public int? Value
		{
			get { return this.value; }
		}

		public bool HasValue
		{
			get { return this.value.HasValue; }
		}

		public void SetValue(int value)
		{
			var checkedValue = check(value);
			if (checkedValue != this.value)
			{
				this.value = checkedValue;

				if (ValueChanged != null)
					ValueChanged(this, EventArgs.Empty);
			}
		}

		public void ClearValue()
		{
			this.value = null;

			if (ValueChanged != null)
				ValueChanged(this, EventArgs.Empty);
		}

		public event EventHandler ValueChanged;

		public static bool operator ==(Element left, Element right)
		{
			if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
				return true;
			return left.Equals((object)right);
		}

		public static bool operator !=(Element left, Element right)
		{
			return !(left == right);
		}

		public static bool operator ==(Element left, int right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Element left, int right)
		{
			return !(left == right);
		}

		//public static implicit operator Element(int value)
		//{
		//	return new Element(value);
		//}

		public static implicit operator int(Element number)
		{
			return number.value.GetValueOrDefault();
		}

		public int CompareTo(Element other)
		{
			if (HasValue)
			{
				if (!other.HasValue)
				{
					return 1;
				}
				return value.Value.CompareTo(other.value.Value);
			}
			else
			{
				if (other.HasValue)
					return -1;
				return 0;
			}
		}

		public bool Equals(Element other)
		{
			if (other == null)
				return false;
			return value.Equals(other.value);
		}

		public bool Equals(int other)
		{
			return this.value == other;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			return Equals(obj as Element);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}

		public override string ToString()
		{
			return value.HasValue ? value.ToString() : "_";
		}
	}
}
