using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SudokuFiller
{
	static class BindingExtension
	{
		public static Binding UpdateSource(this Binding sourceBinding, object newSource)
		{
			if (sourceBinding == null)
				throw new ArgumentNullException("sourceBinding");

			if (newSource == null || newSource == sourceBinding.Source)
				return sourceBinding;

			return new Binding()
			{
				Path = sourceBinding.Path,
				Source = newSource,
				Mode = sourceBinding.Mode,
				UpdateSourceTrigger = sourceBinding.UpdateSourceTrigger,
				Converter = sourceBinding.Converter,
				ConverterCulture = sourceBinding.ConverterCulture,
				ConverterParameter = sourceBinding.ConverterParameter,
				AsyncState = sourceBinding.AsyncState,
				BindingGroupName = sourceBinding.BindingGroupName,
				BindsDirectlyToSource = sourceBinding.BindsDirectlyToSource,
				FallbackValue = sourceBinding.FallbackValue,
				IsAsync = sourceBinding.IsAsync,
				StringFormat = sourceBinding.StringFormat,
				TargetNullValue = sourceBinding.TargetNullValue
			};
		}
	}
}
