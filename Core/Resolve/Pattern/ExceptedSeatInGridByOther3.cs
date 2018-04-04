using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Pattern
{
	internal sealed class ExceptedSeatInGridByOther3 : SudokuSolverPartternBase
	{
		public ExceptedSeatInGridByOther3(Definition.Sudoku sudoku)
			: base(sudoku)
		{
		}

		protected override IEnumerable<Observers.ObserverBase> registerObservers(Definition.Sudoku sudoku)
		{
			//not observer in this parttern
			yield break;
		}

		private void searchAndFill(Definition.Grid grid)
		{
			var currentGridSeatsCount = grid.Elements.SeatCount();
			if (currentGridSeatsCount <= 1) //fill by OneInNine pattern
				return;

			Definition.Grid grid1InRow, grid2InRow, grid1InColumn, grid2InColumn;
			grid.GetOtherGrids(Definition.LineType.Row, out grid1InRow, out grid2InRow);
			grid.GetOtherGrids(Definition.LineType.Column, out grid1InColumn, out grid2InColumn);

			var currentGridValues = grid.Elements.Values().ToArray();
			var currentGridSeatIndexes = grid.Elements.NotValued().Select(item => item.Index).ToArray();
			int currentGridIndex = grid.Index;

			var allElementsInOtherGrid = grid1InRow.Elements
				.Concat(grid2InRow.Elements)
				.Concat(grid1InColumn.Elements)
				.Concat(grid2InColumn.Elements)
				.Valued()
				.ExceptWithoutDisdinct(item => item.Value, currentGridValues);

			foreach (var group in allElementsInOtherGrid
				.GroupBy(item => item.Value)
				.Where(item => item.Count() > 2))
			{
				var preFilledIndexes = group
					.SelectMany(item =>
						pretendFillSeatIndex(currentGridIndex, currentGridSeatIndexes, item.GridIndex, item.Index));
				var exceptedSeatIndexes = currentGridSeatIndexes.Except(preFilledIndexes);
				if (exceptedSeatIndexes.Count() == 1)
				{
					//means only one seat left
					int exceptedSeatIndex = exceptedSeatIndexes.First();
					grid.Elements[exceptedSeatIndex].SetValue(group.Key);
				}
			}
		}

		/// <returns>excepted not valued seats index. excepted by valuedElement1InOtherGrid</returns>
		private IEnumerable<int> pretendFillSeatIndex(int sourceGridIndex, IEnumerable<int> sourceGridSeatIndexes, int otherGridIndex, int otherElementIndex)
		{
			var lineType = GridHelper.GetLineType(sourceGridIndex, otherGridIndex);
			int otherElementLayer = GridHelper.GetLayer(otherElementIndex, lineType);

			var query = sourceGridSeatIndexes
				.Where(item => GridHelper.GetLayer(item, lineType) == otherElementLayer);
			return query;
		}

		public override void Fill()
		{
			//this could be a littel longer than other pattern
			foreach (var grid in this.sudoku.Grids)
			{
				searchAndFill(grid);

				if (HasFailed)
					break;
			}
		}

		private const int index = 4;
		public override int Index
		{
			get { return index; }
		}
	}
}
