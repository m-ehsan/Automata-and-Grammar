using System;
using System.Collections.Generic;
using System.Linq;

namespace Automata
{
	public abstract class BaseMachine
	{
		protected List<char> Alphabets { get; set; }
		protected List<string> States { get; set; }
		protected List<string> FinalStates { get; set; }
		protected List<Instruction> Instructions { get; set; }

		public BaseMachine()
		{
			Alphabets = new List<char>();
			States = new List<string>();
			FinalStates = new List<string>();
			Instructions = new List<Instruction>();
		}

		public Array GetAlphabets()
		{
			return Alphabets.ToArray();
		}

		public Array GetStates()
		{
			return States.ToArray();
		}

		public Array GetFinalStates()
		{
			return FinalStates.ToArray();
		}

		public Array GetInstructions()
		{
			return Instructions.ToArray();
		}

		public bool HasState(string stateName)
		{
			return States.Contains(stateName);
		}

		public bool AddAlphabet(char alphabet)
		{
			if (!Alphabets.Contains(alphabet))
			{
				Alphabets.Add(alphabet);
				return true;
			}
			return false;
		}

		public bool RemoveAlphabet(char alphabet)
		{
			if (Alphabets.Contains(alphabet))
			{
				RemoveInstructionsInvolvingAlphabet(alphabet);
				Alphabets.Remove(alphabet);
				return true;
			}
			return false;
		}

		public bool AddState(string stateName)
		{
			if (!States.Contains(stateName))
			{
				States.Add(stateName);
				return true;
			}
			return false;
		}

		public bool RemoveState(string stateName)
		{
			if (States.Contains(stateName))
			{
				RemoveObjectsInvolvingState(stateName);
				States.Remove(stateName);
				return true;
			}
			return false;
		}

		public bool AddFinalState(string stateName)
		{
			if (States.Contains(stateName) && !FinalStates.Contains(stateName))
			{
				FinalStates.Add(stateName);
				return true;
			}
			return false;
		}

		public bool RemoveFinalState(string stateName)
		{
			if (FinalStates.Contains(stateName))
			{
				FinalStates.Remove(stateName);
				return true;
			}
			return false;
		}

		public abstract bool AddInstruction(Instruction instruction);

		public bool RemoveInstruction(Instruction instruction)
		{
			if (States.Contains(instruction.CurrentState) && States.Contains(instruction.NextState) && Alphabets.Contains(instruction.Input))
			{
				if (Instructions.Contains(instruction))
				{
					Instructions.Remove(instruction);
					return true;
				}
				return false;
			}
			return false;
		}

		private void RemoveObjectsInvolvingState(string stateName)
		{
			List<Instruction> temp = new List<Instruction>();

			// Remove related instructions
			foreach (Instruction instruction in Instructions)
			{
				if (instruction.CurrentState == stateName || instruction.NextState == stateName)
				{
					temp.Add(instruction);
				}
			}

			foreach (Instruction instruction in temp)
			{
				Instructions.Remove(instruction);
			}

			// Remove related final states
			if (FinalStates.Contains(stateName))
			{
				FinalStates.Remove(stateName);
			}

			// Remove related initial state
			RemoveInitialStateInvolvingState(stateName);
		}

		protected abstract void RemoveInitialStateInvolvingState(string stateName);

		private void RemoveInstructionsInvolvingAlphabet(char alphabet)
		{
			List<Instruction> temp = new List<Instruction>();

			// Remove related instructions
			foreach (Instruction instruction in Instructions)
			{
				if (instruction.Input == alphabet)
				{
					temp.Add(instruction);
				}
			}

			foreach (Instruction instruction in temp)
			{
				Instructions.Remove(instruction);
			}
		}

		public abstract bool VerifyString(string input);

		protected bool SubVerifyString(string input, string state)
		{
			if (input.Length == 0 && FinalStates.Contains(state))
			{
				return true;
			}
			if (input.Length != 0)
			{
				var candidateInstructions = Instructions.Where(i => i.CurrentState == state && i.Input == input[0]);
				foreach (var instruction in candidateInstructions)
				{
					if (SubVerifyString(input.Substring(1, input.Length - 1), instruction.NextState))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
