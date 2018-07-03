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
			Assert.AreEqual(2, validator.GetDuplicatedElementIndex(values3));

			int?[] values4 = new int?[]
			{
				null, 2, 3, null, 3, 6, 7, 8, 9
			};
			Assert.AreEqual(2, validator.GetDuplicatedElementIndex(values4));

			int?[] values5 = new int?[]
			{
				null, 9, 8, null, 3, 3, 5, 7, 6
			};
			Assert.AreEqual(4, validator.GetDuplicatedElementIndex(values5));

			int?[] values6 = new int?[]
			{
				1, 1, null, null, null, null, null, null, null
			};
			Assert.AreEqual(0, validator.GetDuplicatedElementIndex(values6));
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

		[TestMethod]
		public void DecreTest()
		{
			int total = 6;
			List<int> list1 = new List<int>(total);
			
			int current = total;
			while (current >= 0)
			{
				list1.Add(--current);
			}

			List<int> list2 = new List<int>(total);
			current = total;
			for (int i = current; i >= 0; )
			{
				current = --i;
				list2.Add(current);
			}

			Assert.IsTrue(list1.SequenceEqual(list2), "First step not match previous step");
		}

		[TestMethod]
		public void IncreTest()
		{
			int total = 6;
			int[] source = Enumerable.Range(10, total).ToArray();
			int current = 1;

			int current1 = current;
			List<int> list1 = new List<int>(total);
			Func<bool> method1 = () =>
			{
				int index = current1;
				if (index == source.Length - 1)
					return false;
				index++;
				list1.Add(source[index]);
				current1 = index;
				return true;
			};
			while (method1()) ;

			int current2 = current;
			List<int> list2 = new List<int>();
			Action method2 = () =>
			{
				for (int index = current2 + 1; index <= source.Length - 1; index++)
				{
					list2.Add(source[index]);
				}
				current2 = source.Length - 1;
			};
			method2();

			Assert.IsTrue(list1.SequenceEqual(list2), "Last step not match next step");
			Assert.AreEqual(current1, current2, "Current index mismatch");
		}

		[TestMethod]
		public void EnumValueTest()
		{
			var value = ThreeDay.Day1;

			Action day2 = () =>
			{
				var current = value;
				current = ThreeDay.Day2;
			};
			day2();
			Assert.AreEqual(value, ThreeDay.Day2);

			Action day3 = () =>
			{
				var current = value;
				current = ThreeDay.Day3;
			};
			day3();
			Assert.AreEqual(value, ThreeDay.Day3);
		}

		enum ThreeDay
		{
			Day1,
			Day2,
			Day3
		}
	}
}
