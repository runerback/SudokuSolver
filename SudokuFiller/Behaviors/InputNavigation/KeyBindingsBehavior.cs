using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SudokuFiller
{
	/// <summary>
	/// origin InputBindings will be swallowed by TextBox
	/// </summary>
	sealed class KeyBindingsBehavior : Behavior<FrameworkElement>
	{
		public KeyBindingsBehavior()
		{

		}
		
		#region KeyBindings

		private readonly KeyBindingCollection keyBindings = new KeyBindingCollection();
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public KeyBindingCollection KeyBindings
		{
			get { return this.keyBindings; }
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeKeyBindings()
		{
			return this.keyBindings.Count > 0;
		}
		
		#endregion KeyBindings

		protected override void OnAttached()
		{
			var target = this.AssociatedObject;

			target.PreviewKeyDown += OnPreviewKeyDown;

			target.Dispatcher.BeginInvoke((Action)delegate
			{
				initialize(target);
			}, System.Windows.Threading.DispatcherPriority.Loaded);
		}

		private void initialize(FrameworkElement target)
		{
			var context = target.DataContext;
			//redirect Command binding to AssociatedObject
			foreach (KeyBinding keyBinding in this.keyBindings)
			{
				var binding = BindingOperations.GetBinding(keyBinding, KeyBinding.CommandProperty);
				if (binding != null)
				{
					BindingOperations.ClearBinding(keyBinding, KeyBinding.CommandProperty);
					BindingOperations.SetBinding(
						keyBinding,
						KeyBinding.CommandProperty,
						new Binding()
						{
							Path = binding.Path,
							Source = context,
							Mode = binding.Mode,
							UpdateSourceTrigger = binding.UpdateSourceTrigger,
							Converter = binding.Converter,
							ConverterCulture = binding.ConverterCulture,
							ConverterParameter = binding.ConverterParameter,
							AsyncState = binding.AsyncState,
							BindingGroupName = binding.BindingGroupName,
							BindsDirectlyToSource = binding.BindsDirectlyToSource,
							FallbackValue = binding.FallbackValue,
							IsAsync = binding.IsAsync,
							StringFormat = binding.StringFormat,
							TargetNullValue = binding.TargetNullValue
						});
				}
			}
		}

		protected override void OnDetaching()
		{
			this.AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
			this.keyBindings.Clear();
		}

		private void OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			this.keyBindings.ExecuteCommands(e.Key);
		}
	}

	sealed class KeyBindingCollection : IList
	{
		public KeyBindingCollection() { }

		public KeyBindingCollection(IList source)
		{
			if (source != null && source.Count > 0)
				AddRange(source);
		}

		private readonly List<KeyBinding> innerCollection = new List<KeyBinding>();

		#region ICollection

		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
				throw new ArgumentNullException("array");

			if (this.innerCollection.Count == 0)
				return;
			if (index < 0 || index >= innerCollection.Count)
				throw new ArgumentOutOfRangeException("index");

			lock (syncRoot)
			{
				var source = this.innerCollection.ToArray();
				Array.Copy(source, index, array, 0, source.Length - index);
			}
		}

		#endregion ICollection

		#region IList
		
		int IList.Add(object value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			var keyBinding = value as KeyBinding;
			if (keyBinding == null)
				throw new NotSupportedException("value. KeyBinding only");

			lock (syncRoot)
			{
				this.innerCollection.Add(keyBinding);
				register(keyBinding);
				return this.innerCollection.Count - 1;
			}
		}

		bool IList.Contains(object value)
		{
			var keyBinding = value as KeyBinding;
			if (keyBinding == null)
				return false;

			lock (syncRoot)
				return this.innerCollection.Contains(keyBinding);
		}

		int IList.IndexOf(object value)
		{
			var keyBinding = value as KeyBinding;
			if (keyBinding == null)
				return -1;

			lock (syncRoot)
				return this.innerCollection.IndexOf(keyBinding);
		}

		void IList.Insert(int index, object value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			var keyBinding = value as KeyBinding;
			if (keyBinding == null)
				throw new NotSupportedException("value. KeyBinding only");

			lock (syncRoot)
			{
				var collection = this.innerCollection;

				int safeIndex = index;
				if (index < 0)
					safeIndex = 0;
				else if (index >= collection.Count)
					safeIndex = collection.Count - 1;

				collection.Insert(safeIndex, keyBinding);
				register(keyBinding);
			}
		}

		void IList.Remove(object value)
		{
			var keyBinding = value as KeyBinding;
			if (keyBinding == null)
				return;

			lock (syncRoot)
			{
				this.innerCollection.Remove(keyBinding);
				unregister(keyBinding);
			}
		}

		object IList.this[int index]
		{
			get
			{
				if (index < 0 || index >= this.innerCollection.Count)
					throw new ArgumentOutOfRangeException("index");

				lock (syncRoot)
					return this.innerCollection[index];
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				var keyBinding = value as KeyBinding;
				if (keyBinding == null)
					throw new NotSupportedException("value. KeyBinding only");

				if (index < 0 || index >= this.innerCollection.Count)
					throw new ArgumentOutOfRangeException("index");

				lock (syncRoot)
					this.innerCollection[index] = keyBinding;
			}
		}

		#endregion IList

		public IEnumerator GetEnumerator()
		{
			lock (syncRoot)
				return innerCollection.ToArray().GetEnumerator();
		}

		private readonly object syncRoot = new object();
		public object SyncRoot
		{
			get { return syncRoot; }
		}

		public int Count
		{
			get
			{
				lock (syncRoot)
					return this.innerCollection.Count;
			}
		}

		public bool IsSynchronized
		{
			get { return false; }
		}

		public bool IsFixedSize
		{
			get { return false; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.innerCollection.Count)
				throw new ArgumentOutOfRangeException("index");

			lock (syncRoot)
			{
				var keyBinding = this.innerCollection[index];
				this.innerCollection.RemoveAt(index);
				unregister(keyBinding);
			}
		}

		public void Clear()
		{
			lock (syncRoot)
			{
				this.innerCollection.Clear();
				this.keyBindingMap.Clear();
			}
		}

		public KeyBinding this[int index]
		{
			get
			{
				lock (syncRoot)
					return this.innerCollection[index];
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				lock (syncRoot)
					this.innerCollection[index] = value;
			}
		}

		public int Add(KeyBinding value)
		{
			return ((IList)this).Add(value);
		}

		public void AddRange(IEnumerable source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			lock (syncRoot)
			{
				var collection = this.innerCollection;

				foreach (var item in source)
				{
					var keyBinding = item as KeyBinding;
					if (keyBinding == null)
						throw new NotSupportedException("KeyBinding only");

					collection.Add(keyBinding);
					register(keyBinding);
				}
			}
		}

		public int IndexOf(KeyBinding value)
		{
			return ((IList)this).IndexOf(value);
		}

		public void Insert(int index, KeyBinding value)
		{
			((IList)this).Insert(index, value);
		}

		public void Remove(KeyBinding value)
		{
			((IList)this).Remove(value);
		}

		public bool Contains(KeyBinding key)
		{
			return ((IList)this).Contains(key);
		}

		public void CopyTo(KeyBinding[] keyBindings, int index)
		{
			lock (syncRoot)
				this.innerCollection.CopyTo(keyBindings, index);
		}
		
		#region Key Command Map

		private readonly Dictionary<Key, List<KeyBinding>> keyBindingMap =
			new Dictionary<Key, List<KeyBinding>>();

		private void register(KeyBinding keyBinding)
		{
			if (keyBinding == null)
				throw new ArgumentNullException("keyBinding");

			var map = this.keyBindingMap;
			var key = keyBinding.Key;

			List<KeyBinding> bindings;
			if (!map.TryGetValue(key, out bindings))
			{
				bindings = new List<KeyBinding>();
				map.Add(key, bindings);
			}
			bindings.Add(keyBinding);
		}

		private void unregister(KeyBinding keyBinding)
		{
			if (keyBinding == null)
				throw new ArgumentNullException("keyBinding");

			var map = this.keyBindingMap;
			var key = keyBinding.Key;

			List<KeyBinding> bindings;
			if (!map.TryGetValue(key, out bindings))
				return;

			if (bindings.Remove(keyBinding))
			{
				if (bindings.Count == 0)
					map.Remove(key);
			}
		}

		internal void ExecuteCommands(Key key)
		{
			IEnumerable<KeyBinding> matches;
			lock (syncRoot)
			{
				var map = this.keyBindingMap;

				List<KeyBinding> bindings;
				if (!this.keyBindingMap.TryGetValue(key, out bindings))
					return;
				matches = bindings.ToArray();
			}

			foreach (var binding in matches)
			{
				if (binding.Command != null)
					binding.Command.Execute(binding.CommandParameter);
			}
		}

		#endregion Key Command Map

	}
}
