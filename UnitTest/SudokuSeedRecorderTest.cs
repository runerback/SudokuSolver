using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SudokuSolver.UnitTest
{
	[TestClass]
	public class SudokuSeedRecorderTest
	{
		private readonly CompletedSudokuBuilderSeedRecorder recorder = 
			CompletedSudokuBuilderSeedRecorder.Instance;

		[TestInitialize]
		public void Initialize()
		{
			recorder.Reset();
		}

		[TestCleanup]
		public void CleanUp()
		{
			recorder.Dispose();
		}

		[TestMethod]
		public void AddRecords()
		{
			for (int i = 0, j = int.MinValue; i < 1000; i++, j++)
			{
				recorder.Add(j);
			}
		}

		[TestMethod]
		public void GetLastLine()
		{
			AddRecords();
			int lastRecord = recorder.GetLastRecord();
		}
	}
}
