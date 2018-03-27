using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SudokuSolver.GUI
{
	internal sealed class App : Application
	{
		public App(SudokuPlayer player)
		{
			if (player == null)
				throw new ArgumentNullException("player");
			this.player = player;

			InitializeComponent();
		}

		#region InitializeComponent

		private bool contentLoaded;

		public void InitializeComponent()
		{
			if (contentLoaded)
				return;
			contentLoaded = true;

			var controlTemplatesLocater = new Uri("/SudokuSolver.GUI;component/Resource/ControlTemplates.xaml", UriKind.Relative);
			this.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(controlTemplatesLocater));
		}

		#endregion InitializeComponent

		private SudokuPlayer player;

		protected override void OnStartup(StartupEventArgs e)
		{
			this.player.Show();
		}
	}
}
