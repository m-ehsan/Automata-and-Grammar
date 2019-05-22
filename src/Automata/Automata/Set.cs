using System;
using System.Collections.Generic;

namespace Automata
{
	public class Set<T> : IEquatable<Set<T>>
	{
		internal List<T> Items { get; set; }

		public Set()
		{
			Items = new List<T>();
		}

		public Set(List<T> value)
		{
			Items = value;
		}

		public bool AddItem(T item)
		{
			if (!Items.Contains(item))
			{
				Items.Add(item);
				return true;
			}
			return false;
		}

		public bool RemoveItem(T item)
		{
			if (Items.Contains(item))
			{
				Items.Remove(item);
				return true;
			}
			return false;
		}

		public T[] GetItems()
		{
			return Items.ToArray();
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
