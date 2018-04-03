using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public sealed class HeapFullPermutationEnumerable<TSource> : IEnumerable<IEnumerable<TSource>>
	{
		public HeapFullPermutationEnumerable(IEnumerable<TSource> source)
		{
			if (source == null || !source.Any())
				throw new ArgumentNullException("source");
			this.sourceArray = source.ToArray();
		}

		private TSource[] sourceArray;

		private IEnumerable<IEnumerable<TSource>> Iterator(TSource[] source)
		{
			yield return source;

			int arrayLen = source.Length;
			int[] indexes = Enumerable.Repeat(0, arrayLen).ToArray();

			int index = 0;
			while (index < arrayLen)
			{
				if (indexes[index] < index)
				{
					if ((index & 1) == 1)//even
						Swap(ref source[0], ref source[index]);
					else //odd
						Swap(ref source[indexes[index]], ref source[index]);

					yield return source;

					indexes[index]++;
					index = 0;
				}
				else
				{
					indexes[index] = 0;
					index++;
				}
			}
		}

		private void Swap(ref TSource item1, ref TSource item2)
		{
			TSource temp = item2;
			item2 = item1;
			item1 = temp;
		}

		private TSource[] GetSourceArray()
		{
			return this.sourceArray.ToArray();
		}

		#region IEnumerable

		public IEnumerator<IEnumerable<TSource>> GetEnumerator()
		{
			return this.Iterator(GetSourceArray()).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion IEnumerable

	}
}
