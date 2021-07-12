using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ovning5.Misc
{

	public class ConIO
	{
		// TODO: redo to singleton
		public ConIO(){ }

		public void ClearScrn() { Console.Clear(); }

		public void Show(string msg) {
			if (msg == null) return;
			Console.Write(msg);
		}

		public void ShowXY(int x, int y, string msg) {
			if ((x < 0) || (y < 0) || (x >= Console.WindowWidth) || (y >= Console.WindowHeight)) return;
			Console.SetCursorPosition(x,y);
			if ((msg == null) || (msg.Length == 0)) return;
			Console.Write(msg);
		}

		public string GetString(string prompt = null) {
			if (!(prompt == null) && prompt.Length > 0) this.Show(prompt);
			return Console.ReadLine();
		}

		// return a positive integer if successful or -1 if error
		public int GetNumber(string prompt = null) {
			int i1 = 0;
			if (!(prompt == null) && prompt.Length > 0) this.Show(prompt);
			if (Int32.TryParse(Console.ReadLine().Trim(), out i1)) return i1;
			return -1;
		}

	}
}
