using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuFiller;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.UnitTest
{
	[TestClass]
	public class SudokuFillerTests
	{
		[TestMethod]
		public void DuplicatedElementIndexTest()
		{
			var validator = TestInstances.SudokuInputValidator();

			int?[] values1 = new int?[]
			{
				1, 2, 3, 4, 5, 6, 7, 8, 9
			};
			Assert.AreEqual(-1, validator.GetDuplicatedElementIndex(values1));

			int?[] values2 = new int?[]
			{
				1, 2, 3, null, 5, null, null, null, 9
			};
			Assert.AreEqual(-1, validator.GetDuplicatedElementIndex(values2));

			int?[] values3 = new int?[]
			{
				1, 2, 3, 3, 3, 6, 7, 8, 9
			};
			Assert.AreEqual(3, validator.GetDuplicatedElementIndex(values3));

			int?[] values4 = new int?[]
			{
				null, 2, 3, null, 3, 6, 7, 8, 9
			};
			Assert.AreEqual(4, validator.GetDuplicatedElementIndex(values4));
		}

		[TestMethod]
		public void ElementLineIndexTest()
		{
			var validator = TestInstances.SudokuInputValidator();

			Assert.IsTrue(Enumerable.SequenceEqual(
				new Tuple<int, int>[]
				{
					Tuple.Create(6, 3), Tuple.Create(6, 4), Tuple.Create(6, 5), 
					Tuple.Create(7, 3), Tuple.Create(7, 4), Tuple.Create(7, 5), 
					Tuple.Create(8, 3), Tuple.Create(8, 4), Tuple.Create(8, 5), 
				},
				validator.GetElementsInRow(6, 3)), "Row");

			Assert.IsTrue(Enumerable.SequenceEqual(
				new Tuple<int, int>[]
				{
					Tuple.Create(2, 1), Tuple.Create(2, 4), Tuple.Create(2, 7), 
					Tuple.Create(5, 1), Tuple.Create(5, 4), Tuple.Create(5, 7), 
					Tuple.Create(8, 1), Tuple.Create(8, 4), Tuple.Create(8, 7), 
				},
				validator.GetElementsInColumn(5, 7)), "Column");
		}

		[TestMethod]
		public void StepsTest()
		{
			var basicSteps = new Steps(Enumerable.Range(1, 3));

			Assert.IsTrue(basicSteps.NextStep());
			Assert.AreEqual(1, basicSteps.CurrentStepValue);

			Assert.IsTrue(basicSteps.NextStep());
			Assert.AreEqual(2, basicSteps.CurrentStepValue);

			Assert.IsTrue(basicSteps.NextStep());
			Assert.AreEqual(3, basicSteps.CurrentStepValue);

			Assert.IsFalse(basicSteps.NextStep());
			Assert.AreEqual(3, basicSteps.CurrentStepValue);

			Assert.IsTrue(basicSteps.PreviousStep());
			Assert.AreEqual(2, basicSteps.CurrentStepValue);

			Assert.IsTrue(basicSteps.PreviousStep());
			Assert.AreEqual(1, basicSteps.CurrentStepValue);

			Assert.IsTrue(basicSteps.PreviousStep());

			var advancedSteps = new Steps(Enumerable.Range(1, 3));

			Assert.IsFalse(advancedSteps.PreviousStep());

			Assert.IsTrue(advancedSteps.NextStep());
			Assert.AreEqual(1, advancedSteps.CurrentStepValue);

			Assert.IsTrue(advancedSteps.PreviousStep());

			Assert.IsTrue(advancedSteps.NextStep());
			Assert.AreEqual(1, advancedSteps.CurrentStepValue);

			Assert.IsTrue(advancedSteps.NextStep());
			Assert.AreEqual(2, advancedSteps.CurrentStepValue);

			Assert.IsTrue(advancedSteps.PreviousStep());
			Assert.AreEqual(1, advancedSteps.CurrentStepValue);

			Assert.IsTrue(advancedSteps.NextStep());
			Assert.AreEqual(2, advancedSteps.CurrentStepValue);

			Assert.IsTrue(advancedSteps.NextStep());
			Assert.AreEqual(3, advancedSteps.CurrentStepValue);

			Assert.IsFalse(advancedSteps.NextStep());
			Assert.AreEqual(3, advancedSteps.CurrentStepValue);

			Assert.IsTrue(advancedSteps.PreviousStep());
			Assert.AreEqual(2, advancedSteps.CurrentStepValue);

			Assert.IsTrue(advancedSteps.PreviousStep());
			Assert.AreEqual(1, advancedSteps.CurrentStepValue);

			Assert.IsTrue(advancedSteps.PreviousStep());
			Assert.IsFalse(advancedSteps.PreviousStep());
		}

		class Step : ISolveStep
		{
			public Step(int value)
			{
				this.value = value;
			}

			private readonly int value;
			private const int DEFAULT_VALUE = 0;

			private int currentValue = DEFAULT_VALUE;
			public int Value
			{
				get { return this.currentValue; }
			}

			public void Apply()
			{
				this.currentValue = value;
			}

			public void Revert()
			{
				this.currentValue = DEFAULT_VALUE;
			}
		}

		class Steps
		{
			public Steps(IEnumerable<int> values)
			{
				if (values == null || !values.Any())
					throw new ArgumentNullException("values");
				this.steps = values.Select(item => new Step(item)).ToArray();
			}

			private readonly Step[] steps;
			private int currentStepIndex = -1;

			private int currentStepValue = -1;
			public int CurrentStepValue
			{
				get { return currentStepValue; }
			}

			public bool NextStep()
			{
				var stepIndex = this.currentStepIndex;
				var steps = this.steps;

				if (stepIndex == steps.Length - 1)
					return false;

				stepIndex++;
				var step = steps[stepIndex];
				step.Apply();
				this.currentStepValue = step.Value;
				this.currentStepIndex = stepIndex;
				return true;
			}

			public bool PreviousStep()
			{
				var stepIndex = this.currentStepIndex;
				var steps = this.steps;

				if (stepIndex < 0)
					return false;

				if (stepIndex == 0)
				{
					this.currentStepIndex = -1;
					this.currentStepValue = -1;
				}
				else
				{
					var step = steps[stepIndex];
					step.Revert();
					stepIndex--;
					var previousStep = steps[stepIndex];
					//previousStep.Apply();
					this.currentStepValue = previousStep.Value;
					this.currentStepIndex = stepIndex;
				}
				return true;
			}
		}
	}
}
