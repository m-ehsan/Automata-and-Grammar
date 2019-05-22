using System;

namespace Grammars
{
	public class Instruction : IEquatable<Instruction>
	{
		public char Left { get; set; }
		public string Right { get; set; }
		public bool IsLambda
		{
			get
			{
				return Right[0] == 0;
			}
		}

		public Instruction(char left, string right)
		{
			Left = left;
			Right = right;
		}

		public bool Equals(Instruction other)
		{
			if (other == null)
			{
				return false;
			}
			if (Left == other.Left && Right == other.Right && IsLambda == other.IsLambda)
			{
				return true;
			}
			return false;
		}
	}
}
