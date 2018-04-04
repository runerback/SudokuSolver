using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Core.Observers
{
	internal class ElementClusterUpdatedEventArgs : EventArgs
	{
		public ElementClusterUpdatedEventArgs(int index, Definition.IElementCluster elementCluster)
		{
			if (elementCluster == null)
				throw new ArgumentNullException("elementCluster");
			this.elementCluster = elementCluster;
			this.index = index;
		}

		private int index;
		public int Index
		{
			get { return this.index; }
		}

		private Definition.IElementCluster elementCluster;
		public Definition.IElementCluster ElementCluster
		{
			get { return this.elementCluster; }
		}
	}
}
