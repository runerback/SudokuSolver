using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuSolver;

namespace SudokuFiller
{
	sealed class SudokuSolverWithSteps
	{
		public SudokuSolverWithSteps(SudokuData sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");
			this.source = sudoku;
		}

		private readonly SudokuData source;

		public SudokuSolveResult Solve()
		{
			using (var solver = new Solver(this.source.Copy()))
			{
				IEnumerable<ISolveStep> steps;
				if (solver.Solve(out steps))
					return SudokuSolveResult.Completed;
				return SudokuSolveResult.Unsolved;
			}
		}

		sealed class SolveStep : ISolveStep
		{
			public SolveStep(ElementData element, int newValue)
			{
				if (element == null)
					throw new ArgumentNullException("element");
				this.element = element;
				this.newValue = newValue;
			}

			private readonly ElementData element;
			private readonly int newValue;

			void ISolveStep.Apply()
			{
				this.element.Value = this.newValue;
			}

			void ISolveStep.Revert()
			{
				this.element.Value = null;
			}
		}

		sealed class Solver : IDisposable
		{
			public Solver(SudokuData sudokuData)
			{
				if (sudokuData == null)
					throw new ArgumentNullException("sudokuData");
				this.sudokuData = sudokuData;

				var sudoku = new SudokuSolver.Definition.Sudoku();
				foreach (var grid in sudokuData.Grids)
				{
					var sudokuGrid = sudoku.Grids[grid.Index];
					foreach (var element in grid.Elements.Where(item => item.Value.HasValue))
						sudokuGrid.Elements[element.Index].SetValue(element.Value.Value);
				}

				foreach (var element in new SudokuSolver.SudokuElementEnumerable(sudoku))
					element.ValueChanged += onElementValueChanged;

				this.sudoku = sudoku;
			}

			private readonly SudokuData sudokuData;
			private readonly SudokuSolver.Definition.Sudoku sudoku;
			private readonly List<ISolveStep> steps = new List<ISolveStep>();
			private readonly object solveLock = new object();

			private void onElementValueChanged(object sender, EventArgs e)
			{
				var element = (SudokuSolver.Definition.Element)sender;
				this.steps.Add(new SolveStep(
					this.sudokuData.Grids[element.GridIndex].Elements[element.Index], 
					element.Value));
			}

			public bool Solve(out IEnumerable<ISolveStep> steps)
			{
				if (disposed)
					throw new ObjectDisposedException("solver");

				try
				{
					lock (solveLock)
					{
						var sudoku = this.sudoku;
						using (var solver = new SudokuSolver.Core.SudokuSolver(sudoku))
						{
							if (solver.TrySolve() && sudoku.Validate())
							{
								steps = this.steps.ToArray();
								return true;
							}
						}
						steps = Enumerable.Empty<ISolveStep>();
						return false;
					}
				}
				finally
				{
					this.steps.Clear();
				}
			}

			private bool disposed;
			private void Dispose(bool disposing)
			{
				if (disposed)
					return;

				if (disposing)
				{
					foreach (var element in new SudokuSolver.SudokuElementEnumerable(this.sudoku))
						element.ValueChanged -= onElementValueChanged;
				}
				disposed = true;
			}

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}
		}
	}

	public interface ISolveStep
	{
		void Apply();
		void Revert();
	}
}
