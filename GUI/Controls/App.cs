using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SudokuSolver.GUI
{
	internal sealed class App : Application
	{
		public App()
		{
			InitializeComponent();
		}

		#region InitializeComponent

		private bool contentLoaded;

		private void InitializeComponent()
		{
			if (contentLoaded)
				return;
			contentLoaded = true;

			var controlTemplatesLocater = new Uri("/SudokuSolver.GUI;component/Resource/Theme.xaml", UriKind.Relative);
			this.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(controlTemplatesLocater));
		}

		#endregion InitializeComponent

		public void InvokeRun(SudokuPlayer player)
		{
			if (player == null)
				throw new ArgumentNullException("player");

			Dispatcher.BeginInvoke((Func<Window, int>)Run, player);
		}
	}
}
