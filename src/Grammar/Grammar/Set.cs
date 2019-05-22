using System;
using System.Collections.Generic;

namespace Grammars
{
	public class Set<T> : IEquatable<Set<T>>
	{
		internal List<T> Items { get; set; }

		public int Count { get { return Items.Count; } }

		public Set()
		{
			Items = new List<T>();
		}

		public Set(List<T> value)
		{
			Items = value;
		}

		public bool Add(T item)
		{
			if (!Items.Contains(item))
			{
				Items.Add(item);
				return true;
			}
			return false;
		}

		public bool Remove(T item)
		{
			if (Items.Contains(item))
			{
				Items.Remove(item);
				return true;
			}
			return false;
		}

		public void AddToSet(Set<T> set)
		{
			foreach (T item in set.Items)
			{
				Items.Add(item);
			}
		}

		public T[] GetItems()
		{
			return Items.ToArray();
		}

		public bool IsSharedWith(Set<T> other)
		{
			foreach (var item in Items)
			{
				if (other.Items.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		public bool Contains(T item)
		{
			if (Items.Contains(item))
			{
				return true;
			}
			return false;
		}

		public bool Equals(Set<T> other)
		{
			if (other.Items.Count != Items.Count)
			{
				return false;
			}
			foreach (T item in other.Items)
			{
				if (!Items.Contains(item))
				{
					return false;
				}
			}
			return true;
		}
	}
}
