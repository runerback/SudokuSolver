using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuFiller
{
	public static class TestInstances
	{
		public static SudokuInputValidatorTestInterface SudokuInputValidator()
		{
			return new SudokuInputValidator(new SudokuData());
		}
	}
}
