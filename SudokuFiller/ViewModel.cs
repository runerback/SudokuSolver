using Runerback.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SudokuFiller
{
    sealed class ViewModel : ViewModelBase
    {
		public ViewModel()
		{
			this.solveCommand = new RelayCommand(solve, canSolve);
			this.resetCommand = new RelayCommand(reset, canReset);
		}

		private SudokuData sudokuData = new SudokuData();
		public SudokuData SudokuData
		{
			get { return this.sudokuData; }
			private set
			{
				if (value != this.sudokuData)
				{
					this.sudokuData = value;
					NotifyPropertyChanged("SudokuData");
				}
			}
		}

		private bool isInputBoardFocused = true;
		public bool IsInputBoardFocused
		{
			get { return this.isInputBoardFocused; }
			private set
			{
				if (this.isInputBoardFocused != value)
				{
					this.isInputBoardFocused = value;
					NotifyPropertyChanged("IsInputBoardFocused");
				}
			}
		}

		private bool showSteps = false;
		public bool ShowSteps
		{
			get { return this.showSteps; }
			set
			{
				if (this.showSteps != value)
				{
					this.showSteps = value;
					NotifyPropertyChanged("ShowSteps");
				}
			}
		}

		#region FillCommand

		private readonly RelayCommand solveCommand;
		public ICommand SolveCommand
		{
			get { return this.solveCommand; }
		}

		private bool canSolve(object obj)
		{
			return true;
		}

		private void solve(object obj)
		{
			this.IsInputBoardFocused = false;
			var sudoku = this.sudokuData.Copy();
			var steps = sudoku.Solve();
			this.Steps = steps;
			if (steps.State == SudokuSolveState.Solved)
			{
				this.SudokuData = sudoku;
				this.ShowSteps = true;
			}
			else
			{
				MessageBox.Show(solveResult(steps.State));
				this.IsInputBoardFocused = true;
			}
		}

		private string solveResult(SudokuSolveState state)
		{
			switch (state)
			{
				case SudokuSolveState.Completed:
					return "sudoku already completed";
				case SudokuSolveState.Invalid:
					return "invalid sudoku";
				case SudokuSolveState.Unsolved:
					return "cannot solve this sudoku with current knowledge";
				case SudokuSolveState.UNKNOWN:
					return "unknown state";
				case SudokuSolveState.Solved:
					return "solved";
				default: throw new NotImplementedException(state.ToString());
			}
		}

		#endregion FillCommand

		#region ResetCommand

		private readonly RelayCommand resetCommand;
		public ICommand ResetCommand
		{
			get { return this.resetCommand; }
		}

		private bool canReset(object obj)
		{
			return this.steps != null;
		}

		private void reset(object obj)
		{
			this.Steps = null;
			this.SudokuData = new SudokuData();
			this.ShowSteps = false;
			this.IsInputBoardFocused = true;
		}
		
		#endregion ResetCommand

		#region Steps

		private SudokuSolveResult steps;
		public SudokuSolveResult Steps
		{
			get { return this.steps; }
			private set
			{
				if (value != steps)
				{
					this.steps = value;
					NotifyPropertyChanged("Steps");
				}
			}
		}

		#endregion Steps


    }
}
