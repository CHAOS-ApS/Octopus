using System;
using System.Threading;

namespace Geckon.Octopus.TestUtilities
{
	public static class Timing
	{
		public const int DEFAULT_FUNCTION_TEST_INTERVAL = 50;

		/// <summary>
		/// Calls an action and throws an exception if it times out.
		/// </summary>
		/// <param name="action">The action to perform</param>
		/// <param name="timeout">The timeout in milliseconds</param>
		public static void Call(Action action, int timeout)
		{
			Call(action, timeout);
		}
		
		/// <summary>
		/// Calls an action and throws an exception if it times out.
		/// </summary>
		/// <param name="action">The action to perform</param>
		/// <param name="timeout">The timeout in milliseconds</param>
		/// <param name="message">A message to be used in case of timeout</param>
		public static void Call(Action action, int timeout, string message)
		{
			Thread threadToKill = null;

			Action wrappedAction = () =>
			                       	{
			                       		threadToKill = Thread.CurrentThread;
			                       		action();
			                       	};

			IAsyncResult result = wrappedAction.BeginInvoke(null, null);

			if (result.AsyncWaitHandle.WaitOne(timeout))
			{
				wrappedAction.EndInvoke(result);
			}
			else
			{	
				threadToKill.Abort();

				if(string.IsNullOrEmpty(message))
					throw new TimeoutException();

				throw new TimeoutException(message);
			}
		}

		/// <summary>
		/// Calls an function until it returns false and throws an exception if it times out.
		/// Uses the default interval between each test of function.
		/// </summary>
		/// <param name="function">The action to perform.</param>
		/// <param name="timeout">The timeout in milliseconds.</param>
		/// <param name="message">>A message to be used in case of timeout</param>
		public static void WaitWhile(Func<bool> function, int timeout, string message)
		{
			WaitWhile(function, timeout, DEFAULT_FUNCTION_TEST_INTERVAL, message);
		}

		/// <summary>
		/// Calls an function until it returns false and throws an exception if it times out.
		/// Uses the default interval between each test of function.
		/// </summary>
		/// <param name="function">The action to perform.</param>
		/// <param name="timeout">The timeout in milliseconds.</param>
		public static void WaitWhile(Func<bool> function, int timeout)
		{
			WaitWhile(function, timeout, DEFAULT_FUNCTION_TEST_INTERVAL);
		}

		/// <summary>
		/// Calls an function until it returns false and throws an exception if it times out.
		/// </summary>
		/// <param name="function">The action to perform.</param>
		/// <param name="timeout">The timeout in milliseconds.</param>
		/// <param name="testInterval">The time in milliseconds between each test of function.</param>
		public static void WaitWhile(Func<bool> function, int timeout, int testInterval)
		{
			WaitWhile(function, timeout, testInterval, null);
		}

		/// <summary>
		/// Calls an function until it returns false and throws an exception if it times out.
		/// </summary>
		/// <param name="function">The action to perform.</param>
		/// <param name="timeout">The timeout in milliseconds.</param>
		/// <param name="testInterval">The time in milliseconds between each test of function.</param>
		/// <param name="message">A message to be used in case of timeout</param>
		public static void WaitWhile(Func<bool> function, int timeout, int testInterval, string message)
		{
			WaitWhileFunction(function, timeout, testInterval, message, true);
		}

		/// <summary>
		/// Calls an function until it returns true and throws an exception if it times out.
		/// Uses the default interval between each test of function.
		/// </summary>
		/// <param name="function">The action to perform.</param>
		/// <param name="timeout">The timeout in milliseconds.</param>
		public static void WaitUntil(Func<bool> function, int timeout)
		{
			WaitUntil(function, timeout, DEFAULT_FUNCTION_TEST_INTERVAL);
		}

		/// <summary>
		/// Calls an function until it returns true and throws an exception if it times out.
		/// Uses the default interval between each test of function.
		/// </summary>
		/// <param name="function">The action to perform.</param>
		/// <param name="timeout">The timeout in milliseconds.</param>
		/// <param name="message">A message to be used in case of timeout</param>
		public static void WaitUntil(Func<bool> function, int timeout, string message)
		{
			WaitUntil(function, timeout, DEFAULT_FUNCTION_TEST_INTERVAL, message);
		}

		/// <summary>
		/// Calls an function until it returns true and throws an exception if it times out.
		/// </summary>
		/// <param name="function">The action to perform.</param>
		/// <param name="timeout">The timeout in milliseconds.</param>
		/// <param name="testInterval">The time in milliseconds between each test of function.</param>
		public static void WaitUntil(Func<bool> function, int timeout, int testInterval)
		{
			WaitUntil(function, timeout, testInterval, null);
		}

		/// <summary>
		/// Calls an function until it returns true and throws an exception if it times out.
		/// </summary>
		/// <param name="function">The action to perform.</param>
		/// <param name="timeout">The timeout in milliseconds.</param>
		/// <param name="testInterval">The time in milliseconds between each test of function.</param>
		/// <param name="message">A message to be used in case of timeout</param>
		public static void WaitUntil(Func<bool> function, int timeout, int testInterval, string message)
		{
			WaitWhileFunction(function, timeout, testInterval, message, false);
		}

		#region Private Methods
			
		private static void WaitWhileFunction(Func<bool> function, int timeout, int testInterval, string message, bool returnsTrue)
		{
			var endTime = DateTime.Now.AddMilliseconds(timeout);

			while (function() ^ !returnsTrue)
			{
				if(endTime.CompareTo(DateTime.Now) < 0)
				{
					if (string.IsNullOrEmpty(message))
						throw new TimeoutException();

					throw new TimeoutException(message);
				}

				Thread.Sleep(testInterval);
			}
		}

		#endregion
	}
}