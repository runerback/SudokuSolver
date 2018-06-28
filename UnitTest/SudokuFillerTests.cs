using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuFiller;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.UnitTest
{
	[TestClass]
	public class SudokuFillerTests
	{
		[TestMethod]
		public void DuplicatedElementIndexTest()
		{
			var validator = TestInstances.SudokuInputValidator();

			int?[] values1 = new int?[]
			{
				1, 2, 3, 4, 5, 6, 7, 8, 9
			};
			Assert.AreEqual(-1, validator.GetDuplicatedElementIndex(values1));

			int?[] values2 = new int?[]
			{
				1, 2, 3, null, 5, null, null, null, 9
			};
			Assert.AreEqual(-1, validator.GetDuplicatedElementIndex(values2));

			int?[] values3 = new int?[]
			{
				1, 2, 3, 3, 3, 6, 7, 8, 9
			};
			Assert.AreEqual(3, validator.GetDuplicatedElementIndex(values3));

			int?[] values4 = new int?[]
			{
				null, 2, 3, null, 3, 6, 7, 8, 9
			};
			Assert.AreEqual(4, validator.GetDuplicatedElementIndex(values4));
		}

		[TestMethod]
		public void ElementLineIndexTest()
		{
			var validator = TestInstances.SudokuInputValidator();

			Assert.IsTrue(Enumerable.SequenceEqual(
				new Tuple<int, int>[]
				{
					Tuple.Create(6, 3), Tuple.Create(6, 4), Tuple.Create(6, 5), 
					Tuple.Create(7, 3), Tuple.Create(7, 4), Tuple.Create(7, 5), 
					Tuple.Create(8, 3), Tuple.Create(8, 4), Tuple.Create(8, 5), 
				},
				validator.GetElementsInRow(6, 3)), "Row");

			Assert.IsTrue(Enumerable.SequenceEqual(
				new Tuple<int, int>[]
				{
					Tuple.Create(2, 1), Tuple.Create(2, 4), Tuple.Create(2, 7), 
					Tuple.Create(5, 1), Tuple.Create(5, 4), Tuple.Create(5, 7), 
					Tuple.Create(8, 1), Tuple.Create(8, 4), Tuple.Create(8, 7), 
				},
				validator.GetElementsInColumn(5, 7)), "Column");
		}
	}
}
