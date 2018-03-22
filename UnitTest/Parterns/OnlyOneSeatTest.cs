using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Parterns.UnitTest
{
	[TestClass]
	public class OnlyOneSeatTest
	{
		[TestMethod]
		public void FillElements()
		{
			int[] elements = new int[8];
			for (int i = 1; i <= 9; i++)
			{
				for (int k = 1, l = 0; k <= 9; k++)
				{
					if (i == k)
						continue;
					elements[l++] = k;
				}

				Assert.AreEqual(i, getTheValue(elements));
			}
		}

		private int getTheValue(IEnumerable<int> values)
		{
			int result = 1;
			foreach (var value in values
				.OrderBy(item => item))
			{
				if (value != result)
					break;
				result++;
			}
			return result;
		}
	}
}
