using System.Linq;

namespace Grammars
{
	public class Grammar
	{
		public Set<Instruction> instructions;

		public Grammar()
		{
			instructions = new Set<Instruction>();
		}

		public void AddInstruction(Instruction ins)
		{
			instructions.Add(ins);
		}

		public void AddInstruction(char left, string right)
		{
			instructions.Add(new Instruction(left, right));
		}

		public void RemoveInstruction(Instruction ins)
		{
			instructions.Remove(ins);
		}

		public void RemoveInstruction(char left, string right)
		{
			instructions.Remove(new Instruction(left, right));
		}

		/// <summary>
		/// Removes all instructions from the grammar
		/// </summary>
		public void Clear()
		{
			instructions.Items.Clear();
		}

		private Set<char> First(string expression)
		{
			expression = expression.Trim();
			var processedNonTerminals = new Set<char>();
			return FirstSubRoutine(expression, ref processedNonTerminals);
		}

		private Set<char> FirstSubRoutine(string expression, ref Set<char> processedNonTerminals)
		{
			Set<char> result = new Set<char>();
			// expression is empty
			if (string.IsNullOrWhiteSpace(expression))
			{
				return result;
			}
			// expression is terminal
			if (IsTerminal(expression.First()))
			{
				result.Add(expression.First());
				return result;
			}
			// expression is non-terminal
			processedNonTerminals.Add(expression.First());
			foreach (var ins in instructions.Items.Where(u => u.Left == expression.First()))
			{
				if (ins.IsLambda)
				{
					result.Add((char)0);
				}
				else if (IsTerminal(ins.Right.First()))
				{
					result.Add(ins.Right.First());
				}
				else
				{
					if (!processedNonTerminals.Contains(ins.Right.First()))
					{
						processedNonTerminals.Add(ins.Right.First());
						result.AddToSet(FirstSubRoutine(ins.Right, ref processedNonTerminals));
					}
				}
			}
			if (expression.Length > 1 && SetContainsLambda(result))
			{
				result.Remove((char)0);
				result.AddToSet(FirstSubRoutine(expression.Substring(1), ref processedNonTerminals));
			}
			return result;
		}

		private Set<char> Follow(char nonTerminal)
		{
			Set<char> processedNonTerminals = new Set<char>();
			return FollowSubRoutine(nonTerminal, ref processedNonTerminals);
		}

		private Set<char> FollowSubRoutine(char nonTerminal, ref Set<char> processedNonTerminals)
		{
			Set<char> result = new Set<char>();
			if (IsTerminal(nonTerminal))
			{
				return result;
			}
			processedNonTerminals.Add(nonTerminal);
			foreach (var ins in instructions.Items)
			{
				if (ins.Right.Contains(nonTerminal))
				{
					if (ins.Right.IndexOf(nonTerminal) < ins.Right.Length - 1)
					{
						string expression = ins.Right.Substring(ins.Right.IndexOf(nonTerminal) + 1);
						Set<char> tempResult = new Set<char>();
						tempResult.AddToSet(First(expression));
						result.AddToSet(tempResult);
						if (SetContainsLambda(tempResult))
						{
							if (!processedNonTerminals.Contains(ins.Left))
							{
								processedNonTerminals.Add(ins.Left);
								result.AddToSet(FollowSubRoutine(ins.Left, ref processedNonTerminals));
							}
						}
					}
					if (ins.Right.Last() == nonTerminal)
					{
						if (!processedNonTerminals.Contains(ins.Left))
						{
							processedNonTerminals.Add(ins.Left);
							result.AddToSet(FollowSubRoutine(ins.Left, ref processedNonTerminals));
						}
					}
				}
			}
			if (nonTerminal == 'S')
			{
				result.Add('$');
			}

			// Remove possible lambda
			result.Remove((char)0);
			return result;
		}

		public bool IsLL1()
		{
			foreach (char nonTerminal in NonTerminals().Items)
			{
				Set<char> tempSet = new Set<char>();
				foreach (var ins in instructions.Items.Where(u => u.Left == nonTerminal))
				{
					if (tempSet.IsSharedWith(First(ins.Right)))
					{
						return false;
					}
					else
					{
						tempSet.AddToSet(First(ins.Right));
						tempSet.Remove((char)0);
					}
					if (ins.IsLambda)
					{
						if (First(nonTerminal.ToString()).IsSharedWith(Follow(nonTerminal)))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		#region helpers
		public Set<char> Terminals()
		{
			Set<char> terminals = new Set<char>();
			foreach (var ins in instructions.GetItems())
			{
				if (!char.IsUpper(ins.Left))
				{
					terminals.Add(ins.Left);
				}
				foreach (char ch in ins.Right)
				{
					if (!char.IsUpper(ch))
					{
						terminals.Add(ch);
					}
				}
			}
			return terminals;
		}

		public Set<char> NonTerminals()
		{
			Set<char> nonTerminals = new Set<char>();
			foreach (var ins in instructions.GetItems())
			{
				if (char.IsUpper(ins.Left))
				{
					nonTerminals.Add(ins.Left);
				}
				foreach (char ch in ins.Right)
				{
					if (char.IsUpper(ch))
					{
						nonTerminals.Add(ch);
					}
				}
			}
			return nonTerminals;
		}

		public Set<char> NonTerminalsWithInstruction()
		{
			Set<char> nonTerminals = new Set<char>();
			foreach (var ins in instructions.GetItems())
			{
				if (char.IsUpper(ins.Left))
				{
					nonTerminals.Add(ins.Left);
				}
			}
			return nonTerminals;
		}

		private bool IsTerminal(char c)
		{
			if (!char.IsUpper(c))
			{
				return true;
			}
			return false;
		}

		private bool SetContainsLambda(Set<char> set)
		{
			foreach (char item in set.Items)
			{
				if (item == 0)
				{
					return true;
				}
			}
			return false;
		} 
		#endregion
	}
}
