using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SudokuFiller
{
	static class VisualTreeHelperExtension
	{
		/// <summary>
		/// find all children with specified type
		/// </summary>
		/// <typeparam name="T">child type</typeparam>
		/// <param name="source">source control</param>
		/// <param name="sameDepth">true means all children with type are in same container, stop further search if any found.</param>
		/// <returns>all children with specified type</returns>
		public static IEnumerable<T> Children<T>(this DependencyObject source, bool sameDepth = true) where T : DependencyObject
		{
			if (source == null)
				throw new ArgumentNullException("source");

			int childrenCount = VisualTreeHelper.GetChildrenCount(source);
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(source, i);
				if (child == null)
					continue;

				if (child is T)
				{
					yield return (T)child;
					if (sameDepth) continue;
				}

				foreach (T childOfChild in Children<T>(child))
					yield return childOfChild;
			}
		}

		public static Window ContainerWindow(this DependencyObject source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return Window.GetWindow(source);
		}
	}
}
