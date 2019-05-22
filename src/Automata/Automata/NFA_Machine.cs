using System.Collections.Generic;
using System.Linq;

namespace Automata
{
	public class NFA_Machine : BaseMachine
	{
		private List<string> InitialStates { get; set; }

		public NFA_Machine()
		{
			InitialStates = new List<string>();
		}

		public string[] GetInitialStates()
		{
			return InitialStates.ToArray();
		}

		public bool AddInitialState(string stateName)
		{
			if (States.Contains(stateName))
			{
				if (!InitialStates.Contains(stateName))
				{
					InitialStates.Add(stateName);
					return true;
				}
			}
			return false;
		}

		public bool RemoveInitialState(string stateName)
		{
			if (InitialStates.Contains(stateName))
			{
				InitialStates.Remove(stateName);
				return true;
			}
			return false;
		}

		public override bool AddInstruction(Instruction instruction)
		{
			if (States.Contains(instruction.CurrentState) && States.Contains(instruction.NextState) && Alphabets.Contains(instruction.Input))
			{
				if (!Instructions.Contains(instruction))
				{
					Instructions.Add(instruction);
					return true;
				}
				return false;
			}
			return false;
		}

		protected override void RemoveInitialStateInvolvingState(string stateName)
		{
			if (InitialStates.Contains(stateName))
			{
				InitialStates.Remove(stateName);
			}
		}

		public override bool VerifyString(string input)
		{
			foreach (string initialState in InitialStates)
			{
				if (SubVerifyString(input, initialState))
				{
					return true;
				}
			}
			return false;
		}

		public List<Path> AllPathsFor(string input)
		{
			List<Path> resultPaths = new List<Path>();
			Stack<string> tempStack = new Stack<string>();
			foreach (string initialState in InitialStates)
			{
				ExtractPaths(input, initialState, ref resultPaths, ref tempStack);
			}
			return resultPaths;
		}

		private void ExtractPaths(string input, string state, ref List<Path> paths, ref Stack<string> tempStack)
		{
			if (input.Length == 0)
			{
				tempStack.Push(state);
				paths.Add(new Path() { Nodes = tempStack.Reverse().ToList() });
				tempStack.Pop();
			}
			else
			{
				tempStack.Push(state);
				var candidateInstructions = Instructions.Where(i => i.CurrentState == state && i.Input == input[0]);
				if (candidateInstructions.Count() != 0)
				{
					foreach (var instruction in candidateInstructions)
					{
						ExtractPaths(input.Substring(1, input.Length - 1), instruction.NextState, ref paths, ref tempStack);
					}
				}
				else
				{
					paths.Add(new Path() { Nodes = tempStack.Reverse().ToList() });
				}
				tempStack.Pop();
			}
		}

		public DFA_Machine DFA_Equivalent()
		{
			DFA_Machine DFA = new DFA_Machine();

			// Copy alphabets
			foreach (char alphabet in Alphabets)
			{
				DFA.AddAlphabet(alphabet);
			}

			// Define states and instructions
			Set<Set<string>> statesSet = new Set<Set<string>>();
			Set<string> initialStatesSet = new Set<string>();
			foreach (string initialState in InitialStates)
			{
				initialStatesSet.AddItem(initialState);
			}
			statesSet.AddItem(initialStatesSet);
			DFA.AddState(GetStateName(initialStatesSet));
			DFA.SetInitialState(GetStateName(initialStatesSet));
			foreach (string finalState in FinalStates)
			{
				if (initialStatesSet.Items.Contains(finalState))
				{
					DFA.AddFinalState(GetStateName(initialStatesSet));
					break;
				}
			}

			for (int i = 0; i < statesSet.Items.Count; i++)
			{
				foreach (char alphabet in Alphabets)
				{
					Set<string> adjacentStates = AdjacentStates(statesSet.Items[i], alphabet);
					if (adjacentStates.Items.Count != 0)
					{
						if (!statesSet.Items.Contains(adjacentStates))
						{
							statesSet.AddItem(adjacentStates);
							DFA.AddState(GetStateName(adjacentStates));
							foreach (string finalState in FinalStates)
							{
								if (adjacentStates.Items.Contains(finalState))
								{
									DFA.AddFinalState(GetStateName(adjacentStates));
									break;
								}
							}
						}
						DFA.AddInstruction(new Instruction(GetStateName(statesSet.Items[i]), alphabet, GetStateName(adjacentStates)));
					}
					else
					{
						if (!DFA.HasState("TRAP"))
						{
							DFA.AddState("TRAP");
							foreach (char item in Alphabets)
							{
								DFA.AddInstruction(new Instruction("TRAP", item, "TRAP"));
							}
						}
						DFA.AddInstruction(new Instruction(GetStateName(statesSet.Items[i]), alphabet, "TRAP"));
					}
				}
			}
			return DFA;
		}

		private Set<string> AdjacentStates(Set<string> states, char alphabet)
		{
			Set<string> result = new Set<string>();
			foreach (string state in states.Items)
			{
				var matchingInstructions = Instructions.Where(i => i.CurrentState == state && i.Input == alphabet);
				foreach (var ins in matchingInstructions)
				{
					result.AddItem(ins.NextState);
				}
			}
			return result;
		}

		private string GetStateName(Set<string> state)
		{
			state.Items.Sort();
			string result = "";
			for (int i = 0; i < state.Items.Count; i++)
			{
				result += state.Items[i] + ((i < state.Items.Count - 1) ? "_" : "");
			}
			return result;
		}
	}
}
