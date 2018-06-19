using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Branch
{
	internal sealed class SudokuSolveBranch : IEnumerable<SudokuSolver>
	{
		public SudokuSolveBranch(Definition.Sudoku sourceSudoku)
		{
			if (sourceSudoku == null)
				throw new ArgumentNullException("sourceSudoku");
			this.sourceSudoku = sourceSudoku;
		}

		private Definition.Sudoku sourceSudoku;

		/*
		9 means 362880
		8 means 40320
		7 means 5040
		6 means 720
		5 means 120
		4 means 24
		3 means 6
		*/
		private const int SEAT_UPPER_BOUND = 9;

		private bool tryFindBranchBasedElements(Definition.Sudoku sudoku, int seatCount, out IEnumerable<Definition.Element> branchBasedElements)
		{
			if (seatCount < 2 || seatCount > 9)
				throw new ArgumentOutOfRangeException("seatCount. Between 2 and 9");

			branchBasedElements = new SudokuBlockEnumerable(sudoku)
				.FirstOrDefault(item => item.SeatCount() == seatCount);
			return branchBasedElements != null;
		}

		private IEnumerable<SudokuSolver> BranchSolverIterator(Definition.Sudoku sudoku)
		{
			int seatCount = -1;
			IEnumerable<Definition.Element> branchBasedElements = null;
			for (int i = 2; i <= SEAT_UPPER_BOUND; i++)
			{
				if (tryFindBranchBasedElements(sudoku, i, out branchBasedElements))
				{
					seatCount = i;
					break;
				}
			}

			if (seatCount < 0)
				yield break;

			var branchBasedEmptyElements = branchBasedElements.NotValued();

			var remainderValues = branchBasedElements
				.Values()
				.SudokuExcept();

			foreach (var values in new HeapFullPermutationEnumerable<int>(remainderValues))
			{
				var branchSudoku = sudoku.Copy();
				var branchElements = branchBasedEmptyElements
					.Select(item => branchSudoku.Grids[item.GridIndex].Elements[item.Index]);

				using (var validator = new SudokuValidator(branchSudoku))
				{
					using (var valueIterator = values.GetEnumerator())
					using (var elementIterator = branchElements.GetEnumerator())
					{
						while (valueIterator.MoveNext() && elementIterator.MoveNext())
						{
							var currentElement = elementIterator.Current;
							var currentValue = valueIterator.Current;

							//var fillingLog = string.Format("{0, -30}: ({1}, {2}) - {3}",
							//	"new branch",
							//	currentElement.GridIndex + 1,
							//	currentElement.Index + 1,
							//	currentValue);
							
							currentElement.SetValue(currentValue);

							if (validator.HasFailed)
								break;
							//else
							//	Console.WriteLine(fillingLog);
						}
					}

					if (!validator.HasFailed)
						yield return new SudokuSolver(branchSudoku);
				}
			}
		}

		#region IEnumerable

		public IEnumerator<SudokuSolver> GetEnumerator()
		{
			return this.BranchSolverIterator(this.sourceSudoku).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion IEnumerable


		public Definition.Grid branchBasedCluster { get; set; }
	}
}
