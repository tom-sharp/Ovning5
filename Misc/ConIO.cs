using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Misc.Interfaces;
namespace Misc
{
	public class ConIO 
	{
		int pushx = 0, pushy = 0;
		ConsoleColor pushbg = ConsoleColor.Black, pushfg = ConsoleColor.White;
		private static ConIO pinstance = null;

		public static ConIO PInstance { get { if (pinstance == null) pinstance = new ConIO(); return pinstance; } }

		private ConIO() { }

		public void ClearScrn() { Console.Clear(); }

		public void Show(string msg) {
			if (msg == null) return;
			Console.Write(msg);
		}

		public void ShowErr(string msg = null)
		{
			string str;
			if (msg == null) str = "".PadRight(Console.WindowWidth);
			else str = msg.PadRight(Console.WindowWidth);
			this.PushConsoleSettings();			
			Console.SetCursorPosition(0, 0);
			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;
			this.GetKey(str);
			this.PopConsoleColor();
			Console.SetCursorPosition(0, 0);
			this.Show("".PadRight(Console.WindowWidth));
			this.PopConsoleCursor();
		}

		public void ShowStatus(string msg = null)
		{
			string str;
			if (msg == null) str = "".PadRight(Console.WindowWidth);
			else str = msg.PadRight(Console.WindowWidth);
			this.PushConsoleSettings();
			Console.SetCursorPosition(0, 0);
			Console.BackgroundColor = ConsoleColor.Blue;
			Console.ForegroundColor = ConsoleColor.White;
			this.GetKey(str);
			this.PopConsoleColor();
			Console.SetCursorPosition(0, 0);
			this.Show("".PadRight(Console.WindowWidth));
			this.PopConsoleCursor();
		}

		public void ShowXY(int x, int y, string msg) {
			if ((x < 0) || (y < 0) || (x >= Console.WindowWidth) || (y >= Console.WindowHeight)) return;
			Console.SetCursorPosition(x,y);
			if ((msg == null) || (msg.Length == 0)) return;
			Console.Write(msg);
		}

		public string GetString(string prompt = null) {
			if ((prompt != null) && prompt.Length > 0) this.Show(prompt);
			return Console.ReadLine();
		}

		// return a positive integer if successful or -1 if error
		public int GetNumber(string prompt = null, bool required = false) {
			int i1 = 0;
			do {
				if ((prompt != null) && prompt.Length > 0) this.Show(prompt);
				if (Int32.TryParse(Console.ReadLine().Trim(), out i1)) return i1;
			} while (required);
			return -1;
		}

		// return an array of int representing the input OR null in nothing was entered or could not
		// parse to a positive integer
		public int[] GetMultiNumber(string prompt = null, bool required = false)
		{
			int i1, i2, i3;
			string[] input = null;
			int[] ret = null;
			do
			{
				if ((prompt != null) && prompt.Length > 0) this.Show(prompt);
				input = Console.ReadLine().Trim().Split();
				if (input.Length > 0) {
					i1 = 0; i2 = 0;
					while (i1 < input.Length) { if (input[i1].Length > 0) i2++; i1++; }
					if (i2 == 0) return null; 
					ret = new int[i2];
					i1 = 0;	i2 = 0;	i3 = 0;
					while (i1 < input.Length) {
						if (input[i1].Length > 0)
						{
							if (Int32.TryParse(input[i1], out i2)) { ret[i3] = i2; i3++; }
							else { ret = null; break; }
						}
						i1++;
					}
				}
				else ret = null;
			} while (required);
			return ret;
		}

		// return a positive integer if successful or -1 if error
		public bool GetYN(string prompt)
		{
			int ret;
			while (true) {
				ret = this.GetKey(prompt);
				this.Show("\n");
				switch (ret) {
					case 'y': return true;
					case 'Y': return true;
					case 'n': return false;
					case 'N': return false;
				}
			}
		}



		// wait for a key to be pressed and return the acsii code
		// if special key like PgUp or Dn keycode is returned (> 255)
		public int GetKey(string msg = null)
		{
			if ((msg != null) && (msg.Length > 0)) this.Show(msg);
			ConsoleKeyInfo key = Console.ReadKey(true);
			if ((int)key.KeyChar == 0) return (int)key.Key + 256;
			return (int)key.KeyChar;
		}


		private void PushConsoleSettings()
		{
			(pushx, pushy) = Console.GetCursorPosition();
			pushbg = Console.BackgroundColor;
			pushfg = Console.ForegroundColor;
			Console.CursorVisible = false;
		}
		private void PopConsoleSettings()
		{
			Console.SetCursorPosition(pushx, pushy);
			Console.BackgroundColor = pushbg;
			Console.ForegroundColor = pushfg;
			Console.CursorVisible = true;
		}


		private void PopConsoleColor()
		{
			Console.BackgroundColor = pushbg;
			Console.ForegroundColor = pushfg;
		}

		private void PopConsoleCursor()
		{
			Console.SetCursorPosition(pushx, pushy);
			Console.CursorVisible = true;
		}

	}
}
