using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misc
{
	public static class Extensions
	{
		public static bool IsEmpty(this string s) => s.Length == 0;
		public static bool IsOdd(this int i) => i % 2 != 0;
		public static bool IsEven(this int i) => i % 2 == 0;

	}
}
