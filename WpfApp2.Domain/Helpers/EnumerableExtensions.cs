using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.Helpers
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Distinct<T, TKey>(
			this IEnumerable<T> source,
			Func<T, TKey> keySelector)
			=> source.Distinct(new DelegateComparer<T, TKey>(keySelector));
	}

	public class DelegateComparer<T, TKey> : IEqualityComparer<T>
	{
		private Func<T, TKey> selector;
		public DelegateComparer(Func<T, TKey> keySelector)
		{
			selector = keySelector;
		}

		public bool Equals(T x, T y)
			=> selector(x).Equals(selector(y));
		public int GetHashCode(T obj)
			=> selector(obj).GetHashCode();
	}
}
