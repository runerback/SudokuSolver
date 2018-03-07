using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuSolver.Extension;

namespace SudokuSolver
{
	public class CompletedSudokuBuilder
	{
		private static readonly IEnumerable<int> elementsInOrder = Enumerable.Range(1, 9);

		public CompletedSudokuBuilder(int seed)
		{
			this.currentSeed = seed;
			this.rnd = new Random(seed);
		}

		private Random rnd;

		private int currentSeed;

		private IEnumerable<int> GetElementsInRandomOrder()
		{
			return elementsInOrder
				.OrderBy(item => this.rnd.Next());
		}

		private IEnumerable<int> GetElementsInRandomOrder(IEnumerable<int> exceptedElements)
		{
			return elementsInOrder
				.Except(exceptedElements)
				.OrderBy(item => this.rnd.Next());
		}

		private bool GetNextElement(IEnumerable<int> exceptedElements, out int value)
		{
			value = -1;

			var nextSeq = elementsInOrder
				.Except(exceptedElements.Distinct())
				.OrderBy(item => this.rnd.Next());
			if (!nextSeq.Any())
				return false;

			value = nextSeq.First();
			return true;
		}

		public bool Build(Definition.Sudoku sudoku)
		{
			if (sudoku == null)
				throw new ArgumentNullException("sudoku");

			//Console.WriteLine("Grid 1, 5, 9");
			fullRandomGrid(sudoku.Grid1, elementsInOrder.OrderBy(item => rnd.Next()));
			fullRandomGrid(sudoku.Grid5, elementsInOrder.OrderBy(item => rnd.Next()));
			fullRandomGrid(sudoku.Grid9, elementsInOrder.OrderBy(item => rnd.Next()));

			//Console.WriteLine("Grid 3");
			if (!randomGrid3(sudoku.Grid1, sudoku.Grid3, sudoku.Grid9))
				return false;

			//Console.WriteLine("Grid 7");
			if (!randomGrid7(sudoku.Grid1, sudoku.Grid7, sudoku.Grid9))
				return false;

			//Console.WriteLine("Grid 2");
			if (!randomGrid2(sudoku.Grid1, sudoku.Grid2, sudoku.Grid3, sudoku.Grid5))
				return false;

			//Console.WriteLine("Grid 4");
			if (!randomGrid4(sudoku.Grid1, sudoku.Grid4, sudoku.Grid5, sudoku.Grid7))
				return false;

			//Console.WriteLine("Grid 6");
			if (!randomGrid6(sudoku.Grid3, sudoku.Grid4, sudoku.Grid5, sudoku.Grid6, sudoku.Grid9))
				return false;

			//Console.WriteLine("Grid 8");
			if (!randomGrid8(sudoku.Grid2, sudoku.Grid5, sudoku.Grid7, sudoku.Grid8, sudoku.Grid9))
				return false;

			return true;
		}

		private void fullRandomGrid(Definition.Grid grid, IEnumerable<int> rndSeq)
		{
			if (grid == null)
				throw new ArgumentNullException("grid");
			if (rndSeq == null)
				throw new ArgumentNullException("rndSeq");

			using (var valueIterator = rndSeq.GetEnumerator())
			using (var elementIterator = grid.Elements.AsEnumerable().GetEnumerator())
			{
				while (elementIterator.MoveNext() && valueIterator.MoveNext())
				{
					elementIterator.Current.SetValue(valueIterator.Current);
				}
			}
		}

		private bool randomGrid3(Definition.Grid grid1, Definition.Grid grid3, Definition.Grid grid9)
		{
			int index = 0;
			foreach (var element in grid3.Elements)
			{
				int value;
				if (!GetNextElement(grid1.Rows[index / 3].Elements.Cast2()
						.Concat(grid9.Columns[index % 3].Elements.Cast2())
						.Concat(grid3.Elements.Take(index).Cast2()),
					out value))
				{
					//Console.WriteLine("Cannot get next value at {0}", index + 1);
					return false;
				}
				element.SetValue(value);

				index++;
			}

			return true;
		}

		private bool randomGrid7(Definition.Grid grid1, Definition.Grid grid7, Definition.Grid grid9)
		{
			int index = 0;
			foreach (var element in grid7.Elements)
			{
				int value;
				if (!GetNextElement(grid1.Columns[index % 3].Elements.Cast2()
						.Concat(grid9.Rows[index / 3].Elements.Cast2())
						.Concat(grid7.Elements.Take(index).Cast2()),
					out value))
				{
					//Console.WriteLine("Cannot get next value at {0}", index + 1);
					return false;
				}
				element.SetValue(value);

				index++;
			}

			return true;
		}

		private bool randomGrid2(Definition.Grid grid1, Definition.Grid grid2, Definition.Grid grid3, Definition.Grid grid5)
		{
			int index = 0;
			foreach (var element in grid2.Elements)
			{
				int value;
				if (!GetNextElement(grid1.Rows[index / 3].Elements.Cast2()
						.Concat(grid3.Rows[index / 3].Elements.Cast2())
						.Concat(grid5.Columns[index % 3].Elements.Cast2())
						.Concat(grid2.Elements.Take(index).Cast2()),
					out value))
				{
					//Console.WriteLine("Cannot get next value at {0}", index + 1);
					return false;
				}
				element.SetValue(value);

				index++;
			}

			return true;
		}

		private bool randomGrid4(Definition.Grid grid1, Definition.Grid grid4, Definition.Grid grid5, Definition.Grid grid7)
		{
			int index = 0;
			foreach (var element in grid4.Elements)
			{
				int value;
				if (!GetNextElement(grid1.Columns[index % 3].Elements.Cast2()
						.Concat(grid7.Columns[index % 3].Elements.Cast2())
						.Concat(grid5.Rows[index / 3].Elements.Cast2())
						.Concat(grid4.Elements.Take(index).Cast2()),
					out value))
				{
					//Console.WriteLine("Cannot get next value at {0}", index + 1);
					return false;
				}
				element.SetValue(value);

				index++;
			}

			return true;
		}

		private bool randomGrid6(Definition.Grid grid3, Definition.Grid grid4, Definition.Grid grid5, Definition.Grid grid6, Definition.Grid grid9)
		{
			int index = 0;
			foreach (var element in grid6.Elements)
			{
				int value;
				if (!GetNextElement(grid3.Columns[index % 3].Elements.Cast2()
						.Concat(grid9.Columns[index % 3].Elements.Cast2())
						.Concat(grid4.Rows[index / 3].Elements.Cast2())
						.Concat(grid5.Rows[index / 3].Elements.Cast2())
						.Concat(grid6.Elements.Take(index).Cast2()),
					out value))
				{
					//Console.WriteLine("Cannot get next value at {0}", index + 1);
					return false;
				}
				element.SetValue(value);

				index++;
			}

			return true;
		}

		private bool randomGrid8(Definition.Grid grid2, Definition.Grid grid5, Definition.Grid grid7, Definition.Grid grid8, Definition.Grid grid9)
		{
			int index = 0;
			foreach (var element in grid8.Elements)
			{
				int value;
				if (!GetNextElement(grid7.Rows[index / 3].Elements.Cast2()
						.Concat(grid9.Rows[index / 3].Elements.Cast2())
						.Concat(grid2.Columns[index % 3].Elements.Cast2())
						.Concat(grid5.Columns[index % 3].Elements.Cast2())
						.Concat(grid8.Elements.Take(index).Cast2()),
					out value))
				{
					//Console.WriteLine("Cannot get next value at {0}", index + 1);
					return false;
				}
				element.SetValue(value);

				index++;
			}

			return true;
		}

	}
}
