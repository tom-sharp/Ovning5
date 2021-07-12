using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Misc
{
	public static class BugCheck
	{
		private static StreamWriter bugcheckfile;

		/// <summary>
		/// Will log Bugcheck info to file.
		/// if log filas an AplicationException is thrown
		/// </summary>
		/// <param name="sender">sender object</param>
		/// <param name="msg">message to be logged</param>
		public static void Log(object sender, string msg)
		{
			// log this message to file
			DateTime DT = DateTime.Now;
			try
			{
				bugcheckfile = File.AppendText("_BUGCHECK.log");
			}
			catch
			{
				throw new ApplicationException("BUGCHECK: Failed to log");
			}
			if (bugcheckfile == null) throw new ApplicationException("BUGCHECK: Failed to log");
			try
			{
				if (sender != null)
					bugcheckfile.WriteLine($"{DT:yyyy-MM-dd hh:mm:ss}  BugCheck: {sender.GetType()} :: {msg}");
				else
					bugcheckfile.WriteLine($"{DT:yyyy-MM-dd hh:mm:ss}  BugCheck: StaticClass :: {msg}");

			}
			catch
			{
				bugcheckfile.Close();
				throw new ApplicationException("BUGCHECK: Failed to log");
			}
			bugcheckfile.Close();
		}
		public static void Critical(object sender, string msg) {
			BugCheck.Log(sender, msg);
			throw new ApplicationException("BugChek:: {msg}");
		}
	}
}
