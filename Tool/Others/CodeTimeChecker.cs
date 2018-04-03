using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public class CodeTimeChecker
	{
		private DateTime lastTime = DateTime.Now;

		public void Set()
		{
			this.lastTime = DateTime.Now;
		}

		public void Check()
		{
			string output = string.Format(@"cost time: {0:hh\:mm\:ss\.fff}", DateTime.Now - this.lastTime);
			if (this.Log != null)
			{
				this.Log(output);
			}
			else
			{
				Console.WriteLine(@"cost time: {0:hh\:mm\:ss\.fff}", output);
			}
		}

		public void Check(string desc)
		{
			string output = string.Format(@"{0} cost time: {1:hh\:mm\:ss\.fff}", desc, DateTime.Now - this.lastTime);
			if (this.Log != null)
				this.Log(output);
			else
				Console.WriteLine(output);
		}

		public Action<string> Log;
	}
}
