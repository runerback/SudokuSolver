using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SudokuSolver.UnitTest
{
	[TestClass]
	public class CommonTest
	{
		[TestMethod]
		public void IntToDouble()
		{
			try
			{
				int a = (int)(double)0; //force convert
			}
			catch
			{
				throw;
			}

			try
			{
				int b = (int)(object)(double)0; //unbox
				Assert.Fail();
			}
			catch { }
		}
	}
}
