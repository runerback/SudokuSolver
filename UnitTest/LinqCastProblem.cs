using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SudokuSolver.UnitTest
{
	[TestClass]
	public class LinqCastProblem
	{
		[TestMethod]
		public void CastTest()
		{
			var items = Enumerable.Range(1, 9)
				.Select(item => (int2)item)
				.ToArray();
			try
			{
				var items1 = Enumerable.Range(1, 9)
					.Cast<int2>()
					.ToArray();
			}
			catch (InvalidCastException) { }

			var select = items.Select(item => (int)item);
			try
			{
				select.Any();
			}
			catch { throw; }

			var cast = items.Cast<int>();
			try
			{
				cast.Any();
			}
			catch (InvalidCastException) { }
		}

		public struct int2
		{
			private int value;

			public int2(int value)
			{
				this.value = value;
			}

			public static implicit operator int2(int value)
			{
				return new int2(value);
			}

			public static implicit operator int(int2 number)
			{
				return number.value;
			}
		}
	}
}
