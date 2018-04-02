using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SudokuSolver.UnitTest
{
	[TestClass]
	public class SkipTest
	{
		[TestMethod]
		public void SikpTakeAndFirst()
		{
			var source = Enumerable.Range(0, 9);

			var skipTake = source.Skip(1).Take(1).First();
			var skipFirst = source.Skip(1).First();

			Assert.AreEqual(skipTake, skipFirst);
		}
	}
}
