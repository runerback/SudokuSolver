using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuFiller
{
	public sealed class SudokuSolveResult
	{
		private SudokuSolveResult(IEnumerable<ISolveStep> steps)
		{
			if (steps != null && steps.Any())
			{
				this.steps = steps.ToArray();
				this.hasSteps = true;
			}
		}

		private readonly ISolveStep[] steps = null;
		private readonly bool hasSteps = false;

		private readonly SudokuSolveState state = SudokuSolveState.UNKNOWN;
		public SudokuSolveState State
		{
			get
			{
				var state = this.state;
				if (state == SudokuSolveState.UNKNOWN)
				{
					if (this == Invalid)
						state = SudokuSolveState.Invalid;
					else if (this == Completed)
						state = SudokuSolveState.Completed;
					else if (this == Unsolved)
						state = SudokuSolveState.Unsolved;
					else
						state = SudokuSolveState.Solved;
				}
				return state;
			}
		}

		#region Instance

		/// <summary>
		/// the sudoku is invalid before solve
		/// </summary>
		internal static readonly SudokuSolveResult Invalid =
			new SudokuSolveResult(null);

		/// <summary>
		/// the sodoku already be solved before solve
		/// </summary>
		internal static readonly SudokuSolveResult Completed =
			new SudokuSolveResult(null);

		/// <summary>
		/// cannot solve this sudoku
		/// </summary>
		internal static readonly SudokuSolveResult Unsolved =
			new SudokuSolveResult(null);

		/// <summary>
		/// solved with steps
		/// </summary>
		internal static SudokuSolveResult Solved(IEnumerable<ISolveStep> steps)
		{
			if (steps == null || !steps.Any())
				throw new ArgumentNullException("steps");
			return new SudokuSolveResult(steps);
		}

		#endregion Instance
		
		#region Operations

		private int currentStepIndex = -1;

		public bool NextStep()
		{
			if (!this.hasSteps)
				throw new InvalidOperationException("no step");

			var stepIndex = this.currentStepIndex;
			var steps = this.steps;

			if (stepIndex == steps.Length - 1)
				return false;

			stepIndex++;
			var step = steps[stepIndex];
			step.Apply();
			this.currentStepIndex = stepIndex;
			return true;
		}

		public bool PreviousStep()
		{
			if (!this.hasSteps)
				throw new InvalidOperationException("no step");

			var stepIndex = this.currentStepIndex;
			var steps = this.steps;

			if (stepIndex < 0)
				return false;

			if (stepIndex == 0)
			{
				this.currentStepIndex = -1;
			}
			else
			{
				steps[stepIndex].Revert();
				this.currentStepIndex--;
			}
			return true;
		}

		#endregion Operations

	}
}
