using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SudokuSolver
{
	internal sealed class CompletedSudokuBuilderSeedRecorder : IDisposable
	{
		private ConcurrentBag<int> cache = new ConcurrentBag<int>();
		private static readonly string filename = "Completed Sudoku Builder Seeds.log";

		private static CompletedSudokuBuilderSeedRecorder instance = 
			new CompletedSudokuBuilderSeedRecorder();
		public static CompletedSudokuBuilderSeedRecorder Instance
		{
			get { return instance; }
		}

		private CompletedSudokuBuilderSeedRecorder()
		{
			if (File.Exists(filename))
			{
				Console.WriteLine("old record file will be remove");
				File.Delete(filename);
			}

			this.recordThreadBlocker = new AutoResetEvent(false);

			this.recordThread = new Thread(recordThreadLoop)
			{
				IsBackground = true,
				Name = "CompletedSudokuBuilderSeedRecorder"
			};
			this.recordThread.Start();

			this.recordStreamDisposeTimer = new Timer(
				onDisposeTimerTicked,
				this,
				recordStreamTimeout,
				-1);
		}

		private readonly object recordStreamLocker = new object();
		private FileStream recordStream = null;

		private FileStream getRecordStream()
		{
			lock (recordStreamLocker)
			{
				if (this.recordStream == null)
				{
					this.recordStream = new FileStream(filename, FileMode.Append, FileAccess.Write);
					//Console.WriteLine("stream created");
				}
				return this.recordStream;
			}
		}

		private void releaseRecordStream()
		{
			lock (recordStreamLocker)
			{
				if (this.recordStream != null)
				{
					this.recordStream.Dispose();
					this.recordStream = null;
					//Console.WriteLine("stream destroyed");
				}
			}
		}

		private void onDisposeTimerTicked(object state)
		{
			releaseRecordStream();
		}

		private Timer recordStreamDisposeTimer;
		private const int recordStreamTimeout = 6000;

		private Thread recordThread;
		private AutoResetEvent recordThreadBlocker;

		private static readonly byte[] CoreNewLineBuffer = new byte[] { (byte)'\r', (byte)'\n' };
		private static Encoding encoding = Encoding.UTF8;

		private void recordMethod(int value)
		{
			var stream = getRecordStream();

			var valueBuffer = encoding.GetBytes(value.ToString());
			stream.Write(valueBuffer, 0, valueBuffer.Length);
			stream.Write(CoreNewLineBuffer, 0, 2);
		}

		private void recordThreadLoop()
		{
			while (!disposing)
			{
				int nextValue;
				while (this.cache.TryTake(out nextValue))
				{
					recordMethod(nextValue);
				}

				if (!disposing)
				{
					this.recordThreadBlocker.WaitOne();
				}
			}
		}

		public void Add(int value)
		{
			this.cache.Add(value);
			this.recordStreamDisposeTimer.Change(recordStreamTimeout, -1);
			this.recordThreadBlocker.Set();
		}

		private bool disposing = false;
		private bool disposed = false;

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			this.disposing = disposing;
			if (disposing)
			{
				this.recordThreadBlocker.Set();
				this.recordThreadBlocker.Dispose();

				this.recordThread.Join();

				if (this.recordStream != null)
				{
					this.recordStream.Dispose();
				}

				this.recordStreamDisposeTimer.Change(-1, -1);
				this.recordStreamDisposeTimer.Dispose();
			}

			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
