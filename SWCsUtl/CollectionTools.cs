using System;
using System.Collections.Generic;
using System.Linq;

namespace SWCsUtl {
	public static class CollectionTools {
		public static IReadOnlyList<T> EnsureIsReadOnlyList<T>(this IEnumerable<T> source) {
			if (source is null) {
				throw new ArgumentNullException(nameof(source));
			}
			if (source is IReadOnlyList<T> list) {
				return list;
			} else {
				return source.ToArray();
			}
		}

		public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate) {
			int n = 0;
			foreach (var item in source) {
				if (predicate(item)) { return n; }
				++n;
			}
			return -1;
		}

	}
}
