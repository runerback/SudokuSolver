using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.GUI.Business
{
	internal class SudokuSolveStepCollection
	{
		private List<SudokuSolveStep> steps = new List<SudokuSolveStep>();

		private readonly object stepsLock = new object();

		public void Add(SudokuSolveStep step)
		{
			lock (stepsLock)
			{
				this.steps.Add(step);
			}
		}

		public void Clear()
		{
			lock (stepsLock)
			{
				this.steps.Clear();
			}
		}

		private int currentStepIndex = -1;

		public bool Previous()
		{
			Business.SudokuSolveStep currentStep;
			if (!TryGetCurrentStep(out currentStep))
				return false;
			currentStep.Restore();
			return TrySeekPreviousStep();
		}

		public bool Next()
		{
			if (!TrySeekNextStep())
				return false;

			Business.SudokuSolveStep currentStep;
			if (!TryGetCurrentStep(out currentStep))
				return false;
			currentStep.Execute();
			return true;
		}

		private bool TryGetCurrentStep(out SudokuSolveStep currentStep)
		{
			currentStep = null;

			var steps = this.steps;

			if (steps.Count == 0)
				return false; //no step

			var currentStepIndex = this.currentStepIndex;

			if (currentStepIndex < 0 || currentStepIndex >= steps.Count)
				return false;

			currentStep = steps[currentStepIndex];
			return true;
		}

		private bool TrySeekPreviousStep()
		{
			var steps = this.steps;

			if (steps.Count == 0)
				return false; //no step

			var currentStepIndex = this.currentStepIndex;

			if (currentStepIndex < 0)
				return false; //no more step

			this.currentStepIndex = --currentStepIndex;
			return true;
		}

		private bool TrySeekNextStep()
		{
			var steps = this.steps;

			if (steps.Count == 0)
				return false; //no step

			var currentStepIndex = this.currentStepIndex;
			if (steps.Count <= currentStepIndex + 1)
				return false; //no more step

			this.currentStepIndex = ++currentStepIndex;
			return true;
		}
	}
}
