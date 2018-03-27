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

		public bool TryGetCurrentStep(out SudokuSolveStep currentStep)
		{
			currentStep = null;

			var steps = this.steps;
			var currentStepIndex = this.currentStepIndex;

			if (currentStepIndex == 0)
				return false; //no step

			if (currentStepIndex >= steps.Count)
				return false; //no more step

			currentStep = steps[currentStepIndex];
			return true;
		}

		public bool TryGetNextStep(out SudokuSolveStep nextStep)
		{
			nextStep = null;

			var steps = this.steps;
			var currentStepIndex  = this.currentStepIndex;
			if (currentStepIndex < 0)
			{
				if (steps.Count == 0)
					return false; //no step
			}
			else
			{
				if (steps.Count <= currentStepIndex + 1)
					return false; //no more step
			}
			this.currentStepIndex = ++currentStepIndex;
			nextStep = steps[currentStepIndex];
			return true;
		}

		public bool TryGetPreviousStep(out SudokuSolveStep previousStep)
		{
			previousStep = null;

			var steps = this.steps;
			var currentStepIndex = this.currentStepIndex;

			if (currentStepIndex <= 0)
				return false; //no more step

			this.currentStepIndex = --currentStepIndex;
			previousStep = steps[currentStepIndex];
			return true;
		}
	}
}
