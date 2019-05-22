using System;

namespace Automata
{
	public class Instruction : IEquatable<Instruction>
    {
		public string CurrentState { get; set; }
		public char Input { get; set; }
		public string NextState { get; set; }

		public Instruction()
		{
		}

		public Instruction(string currentState, char input, string nextState)
		{
			CurrentState = currentState;
			Input = input;
			NextState = nextState;
		}

		public override string ToString()
		{
			return "\u03B4(" + CurrentState + "," + Input + ")=" + NextState;
		}

		public bool Equals(Instruction other)
		{
			if (other == null)
			{
				return false;
			}
			if (CurrentState == other.CurrentState && Input == other.Input && NextState == other.NextState)
			{
				return true;
			}
			return false;
		}
	}
}
