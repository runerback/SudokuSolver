using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace SudokuSolver
{
	public abstract class Log : IDisposable
	{
		private Log()
		{
			this.logThreadLock = new AutoResetEvent(false);

			var logThread = new Thread(recordThreadLoop)
			{
				IsBackground = true,
				Name = "CompletedSudokuBuilderSeedRecorder"
			};
			logThread.Start();
			this.logThread = logThread;

			this.logStreamDisposeTimer = new Timer(
				onDisposeTimerTicked,
				this,
				logStreamTimeout,
				-1);
		}

		public Log(string filename)
			: this()
		{
			if (string.IsNullOrWhiteSpace(filename))
				throw new ArgumentNullException("filename");
			this.filename = filename;
		}

		private ConcurrentBag<string> cache = new ConcurrentBag<string>();

		private string filename;
		public string FileName
		{
			get { return this.filename; }
		}

		public void Add(string line)
		{
			this.cache.Add(line);
			this.logStreamDisposeTimer.Change(logStreamTimeout, -1);
			this.logThreadLock.Set();
		}

		public void Reset()
		{
			if (File.Exists(filename))
			{
				File.Delete(filename);
			}
		}

		#region log stream

		private readonly object logStreamLock = new object();

		protected bool TestLogStreamLock()
		{
			return Monitor.TryEnter(logStreamLock);
		}

		private FileStream logStream = null;

		private FileStream getLogStream()
		{
			lock (logStreamLock)
			{
				if (this.logStream == null)
				{
					this.logStream = new FileStream(filename, FileMode.Append, FileAccess.Write);
				}
				return this.logStream;
			}
		}

		private void releaseLogStream()
		{
			lock (logStreamLock)
			{
				if (this.logStream != null)
				{
					this.logStream.Dispose();
					this.logStream = null;
				}
			}
		}

		#endregion log stream

		#region stream dispose timer

		private Timer logStreamDisposeTimer;
		private const int logStreamTimeout = 6000;

		private void onDisposeTimerTicked(object state)
		{
			releaseLogStream();
		}

		#endregion stream dispose timer

		#region log thread

		private Thread logThread;
		private AutoResetEvent logThreadLock;

		private void recordThreadLoop()
		{
			while (!disposing)
			{
				string nextValue;
				while (this.cache.TryTake(out nextValue))
				{
					writeToFile(nextValue);
				}

				if (!disposing)
				{
					this.logThreadLock.WaitOne();
				}
			}
		}

		#endregion log thread

		#region write to file

		private static readonly byte[] NewLineBuffer = new byte[] { (byte)'\r', (byte)'\n' };

		private static readonly Encoding encoding = Encoding.UTF8;
		public Encoding Encoding
		{
			get { return encoding; }
		}

		private void writeToFile(string line)
		{
			var stream = getLogStream();

			var valueBuffer = encoding.GetBytes(line);
			stream.Write(valueBuffer, 0, valueBuffer.Length);
			stream.Write(NewLineBuffer, 0, 2);

			stream.Flush();
		}

		#endregion write to file

		#region IDisposable

		protected abstract void Disposing();

		private bool disposing = false;
		private bool disposed = false;

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;

			this.disposing = disposing;
			if (disposing)
			{
				Disposing();

				this.logThreadLock.Set();
				this.logThreadLock.Dispose();

				this.logThread.Join();

				if (this.logStream != null)
				{
					this.logStream.Dispose();
				}

				this.logStreamDisposeTimer.Change(-1, -1);
				this.logStreamDisposeTimer.Dispose();
			}

			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion IDisposable
	}
}
