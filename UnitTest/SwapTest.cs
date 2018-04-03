using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SudokuSolver.UnitTest
{
	[TestClass]
	public class SwapTest
	{
		[TestMethod]
		public void Swap()
		{
			int[] source = new int[] { 1, 2 };

			Assert.AreEqual(1, source[0]);
			Assert.AreEqual(2, source[1]);

			swapValueType(ref source[0], ref source[1]);

			Assert.AreEqual(2, source[0]);
			Assert.AreEqual(1, source[1]);

			int temp = source[1];
			source[1] = source[0];
			source[0] = temp;

			Assert.AreEqual(1, source[0]);
			Assert.AreEqual(2, source[1]);

			source.Swap(0, 1);

			Assert.AreEqual(2, source[0]);
			Assert.AreEqual(1, source[1]);
		}

		private void swapValueType<T>(ref T t1, ref T t2)
		{
			T temp = t1;
			t1 = t2;
			t2 = temp;
		}

		[TestMethod] //win
		public void SwapWithRef()
		{
			int[] source = new int[] { 1, 2 };
			for (int i = 0; i < 10000; i++)
			{
				swapValueType(ref source[0], ref source[1]);
			}
		}

		[TestMethod]
		public void SwapDirectly()
		{
			int[] source = new int[] { 1, 2 };
			for (int i = 0; i < 10000; i++)
			{
				int temp = source[1];
				source[1] = source[0];
				source[0] = temp;
			}
		}

		[TestMethod]
		public void SwapInArray()
		{
			int[] source = new int[] { 1, 2 };
			for (int i = 0; i < 10000; i++)
			{
				source.Swap(0, 1);
			}
		}

		[TestMethod]
		public void SwapInArrayWithoutValdiation()
		{
			int[] source = new int[] { 1, 2 };
			for (int i = 0; i < 10000; i++)
			{
				source.SwapWithoutValidation(0, 1);
			}
		}
	}

	public static class ArrayExtension
	{
		public static void Swap(this Array source, int index1, int index2)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			int sourceLen = source.Length;
			if (index1 < 0 || index1 >= sourceLen)
				throw new ArgumentOutOfRangeException("index1");
			if (index2 < 0 || index2 >= sourceLen)
				throw new ArgumentOutOfRangeException("index2");

			var temp = source.GetValue(index1);
			source.SetValue(source.GetValue(index2), index1);
			source.SetValue(temp, index2);
		}

		public static void SwapWithoutValidation(this Array source, int index1, int index2)
		{
			var temp = source.GetValue(index1);
			source.SetValue(source.GetValue(index2), index1);
			source.SetValue(temp, index2);
		}
	}
}
