using System;
using System.Windows.Input;

namespace SudokuSolver.GUI.Business
{
	internal sealed class Command : ICommand
	{
		public Command(Action<object> action)
		{
			if (action == null)
				throw new ArgumentNullException("action");
			this.action = action;
		}
			
		private Action<object> action;

		bool ICommand.CanExecute(object parameter)
		{
			return true;
		}

		event EventHandler ICommand.CanExecuteChanged
		{
			add { }
			remove { }
		}

		public void Execute(object parameter)
		{
			throw new NotImplementedException();
		}
	}
}
