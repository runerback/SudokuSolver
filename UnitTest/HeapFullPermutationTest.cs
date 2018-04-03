using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SudokuSolver.UnitTest
{
	[TestClass]
	public class HeapFullPermutationTest
	{
		[TestMethod]
		public void NineFullPermutation()
		{
			permutationMethod(9);
		}

		[TestMethod]
		public void TwelveFullPermutation()
		{
			permutationMethod(12);
		}

		private void permutationMethod(int upperBound)
		{
			var source = Enumerable.Range(1, upperBound);
			var hfp = new HeapFullPermutationEnumerable<int>(source);
			Assert.AreEqual(
				Factorial(upperBound), 
				new HeapFullPermutationEnumerable<int>(source).Count());
		}

		private int Factorial(int upperBound)
		{
			int result = 1;
			for (int i = 2; i <= upperBound; i++)
			{
				result *= i;
			}
			return result;
		}
	}
}
