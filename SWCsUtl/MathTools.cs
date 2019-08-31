using System;
using System.Collections.Generic;
using System.Text;

namespace SWCsUtl {
	public static class MathTools {

		public static T Clamp<T>(T x, T min, T max) where T : IComparable<T> {
			var c = Comparer<T>.Default;
			if (c.Compare(min, x) > 0) { return min; }
			if (c.Compare(x, max) > 0) { return max; }
			return x;
		}

	}
}
