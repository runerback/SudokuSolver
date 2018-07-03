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
			if (steps == null || !steps.Any())
				throw new ArgumentNullException("steps");

			this.steps = steps.ToArray();
			this.hasSteps = true;
			this.state = SudokuSolveState.Solved;
		}

		private SudokuSolveResult(SudokuSolveState state)
		{
			this.state = state;
		}

		private readonly ISolveStep[] steps = null;
		private readonly bool hasSteps = false;

		private readonly SudokuSolveState state;
		public SudokuSolveState State
		{
			get { return this.state; }
		}

		#region Instance

		/// <summary>
		/// the sudoku is invalid before solve
		/// </summary>
		internal static readonly SudokuSolveResult Invalid =
			new SudokuSolveResult(SudokuSolveState.Invalid);

		/// <summary>
		/// the sodoku already be solved before solve
		/// </summary>
		internal static readonly SudokuSolveResult Completed =
			new SudokuSolveResult(SudokuSolveState.Completed);

		/// <summary>
		/// cannot solve this sudoku
		/// </summary>
		internal static readonly SudokuSolveResult Unsolved =
			new SudokuSolveResult(SudokuSolveState.Unsolved);

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

		public bool HasNextStep()
		{
			if (!this.hasSteps)
				return false;
			if (this.currentStepIndex == this.steps.Length - 1)
				return false;
			return true;
		}

		public bool NextStep()
		{
			//if (!this.hasSteps)
			//	throw new InvalidOperationException("no step");

			if (!HasNextStep())
				return false;

			var stepIndex = this.currentStepIndex;
			var steps = this.steps;

			//if (stepIndex == steps.Length - 1)
			//	return false;

			stepIndex++;
			var step = steps[stepIndex];
			step.Apply();
			this.currentStepIndex = stepIndex;
			return true;
		}

		public void LastStep()
		{
			if (!HasNextStep())
				return;

			var steps = this.steps;
			int total = steps.Length - 1;
			for (int stepIndex = this.currentStepIndex + 1; stepIndex <= total; stepIndex++)
				steps[stepIndex].Apply();
			this.currentStepIndex = total;
		}

		public bool HasPreviousStep()
		{
			if (!this.hasSteps)
				return false;
			if (this.currentStepIndex < 0)
				return false;
			return true;
		}

		public bool PreviousStep()
		{
			//if (!this.hasSteps)
			//	throw new InvalidOperationException("no step");

			if (!HasPreviousStep())
				return false;

			var stepIndex = this.currentStepIndex;
			var steps = this.steps;

			//if (stepIndex < 0)
			//	return false;

			//if (stepIndex == 0)
			//{
			//	this.currentStepIndex = -1;
			//}
			//else
			//{
			//	steps[stepIndex].Revert();
			//	this.currentStepIndex--;
			//}
			if (stepIndex > 0)
				steps[stepIndex].Revert();
			this.currentStepIndex--;
			return true;
		}

		public void FirstStep()
		{
			if (!HasPreviousStep())
				return;

			var steps = this.steps;
			for (int stepIndex = this.currentStepIndex; stepIndex > 0; stepIndex--)
				steps[stepIndex].Revert();
			this.currentStepIndex = -1;
		}

		#endregion Operations

	}
}
