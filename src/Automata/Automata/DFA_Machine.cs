using System.Linq;

namespace Automata
{
	public class DFA_Machine : BaseMachine
	{
		private string InitialState { get; set; }

		public DFA_Machine()
		{
			InitialState = "";
		}

		public string GetInitialState()
		{
			return InitialState;
		}

		public bool SetInitialState(string stateName)
		{
			if (States.Contains(stateName))
			{
				InitialState = stateName;
				return true;
			}
			return false;
		}

		public void RemoveInitialState()
		{
			InitialState = "";
		}

		public override bool AddInstruction(Instruction instruction)
		{
			if (States.Contains(instruction.CurrentState) && States.Contains(instruction.NextState) && Alphabets.Contains(instruction.Input))
			{
				if (ValidateInstruction(instruction))
				{
					Instructions.Add(instruction);
					return true;
				}
				return false;
			}
			return false;
		}

		private bool ValidateInstruction(Instruction instruction)
		{
			foreach (Instruction item in Instructions)
			{
				if (item.CurrentState == instruction.CurrentState && item.Input == instruction.Input)
				{
					return false;
				}
			}
			return true;
		}

		protected override void RemoveInitialStateInvolvingState(string stateName)
		{
			if (InitialState == stateName)
			{
				InitialState = "";
			}
		}

		public override bool VerifyString(string input)
		{
			return SubVerifyString(input, InitialState);
		}

		public Path PathFor(string input)
		{
			Path resultPath = new Path();
			if (VerifyString(input))
			{
				ExtractPath(input, InitialState, ref resultPath);
			}
			return resultPath;
		}

		private bool ExtractPath(string input, string state, ref Path path)
		{
			if (input.Length == 0 && FinalStates.Contains(state))
			{
				path.Nodes.Add(state);
				return true;
			}
			if (input.Length != 0)
			{
				var candidateInstructions = from ins in Instructions where ins.CurrentState == state && ins.Input == input[0] select ins;
				foreach (var instruction in candidateInstructions)
				{
					if (ExtractPath(input.Substring(1, input.Length - 1), instruction.NextState, ref path))
					{
						path.Nodes.Insert(0, state);
						return true;
					}
				}
			}
			return false;
		}
	}
}
