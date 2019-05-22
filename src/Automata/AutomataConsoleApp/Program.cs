using Automata;
using System;
using System.Linq;
using System.Text;

namespace AutomataConsoleApp
{
	class Program
	{
		private static NFA_Machine NFA;

		static void Main(string[] args)
		{
			Console.OutputEncoding = Encoding.UTF8;
			NFA = new NFA_Machine();
			bool loop = true;
			while (loop)
			{
				DisplayMenu();
				Console.Write("> ");
				ConsoleKeyInfo key;
				do
				{
					key = Console.ReadKey(true);
				} while (!ValidateKey(key));

				switch (key.Key)
				{
					case ConsoleKey.D0:
						loop = false;
						break;
					case ConsoleKey.D1:
						EditAlphabets();
						break;
					case ConsoleKey.D2:
						EditStates();
						break;
					case ConsoleKey.D3:
						EditInitialStates();
						break;
					case ConsoleKey.D4:
						EditFinalStates();
						break;
					case ConsoleKey.D5:
						EditInstructions();
						break;
					case ConsoleKey.D6:
						VerifyString();
						break;
					case ConsoleKey.D7:
						DFA_Equivalent();
						break;
					case ConsoleKey.D8:
						Console.Clear();
						Console.Write("Do you want to clear the machine? (y/n) ");
						ConsoleKeyInfo choice;
						do
						{
							choice = Console.ReadKey(true);
						} while (choice.Key != ConsoleKey.Y && choice.Key != ConsoleKey.N && choice.Key != ConsoleKey.Escape);
						if (choice.Key == ConsoleKey.Y)
						{
							NFA = new NFA_Machine();
						}
						break;
					default:
						break;
				}
			}
		}

		private static void DFA_Equivalent()
		{
			Console.Clear();
			DFA_Machine DFA = NFA.DFA_Equivalent();
			Console.WriteLine("DFA equivalent:\n");
			Console.Write("Alphabets:  ");
			foreach (var item in DFA.GetAlphabets())
			{
				Console.Write(item + "  ");
			}
			Console.WriteLine();
			Console.Write("States:  ");
			foreach (var item in DFA.GetStates())
			{
				Console.Write(item + "  ");
			}
			Console.WriteLine();
			Console.Write("Initial state:  " + DFA.GetInitialState());
			Console.WriteLine();
			Console.Write("Final states:  ");
			foreach (var item in DFA.GetFinalStates())
			{
				Console.Write(item + "  ");
			}
			Console.WriteLine();
			Console.Write("Instructions:\t");
			foreach (var item in DFA.GetInstructions())
			{
				Console.Write(item + "\n\t\t");
			}
			Console.ReadKey(true);
		}

		private static void VerifyString()
		{
			Console.Clear();
			while (true)
			{
				Console.Write("Enter string to verify or type \"Back\" to end: ");
				string input = Console.ReadLine();
				if (input.Trim().ToLower() == "back")
				{
					break;
				}
				var paths = NFA.AllPathsFor(input);
				Console.WriteLine();
				Console.WriteLine("All possible paths:");
				foreach (var path in paths)
				{
					string pathString = "";
					for (int i = 0; i < path.Nodes.Count; i++)
					{
						if (i > 0)
						{
							pathString += " \u2192 ";
						}
						pathString += "[" + path.Nodes[i] + "," + (input.Substring(i, input.Length - i).Length > 0 ? input.Substring(i, input.Length - i) : "\u03BB") + "]";
					}
					Console.WriteLine(pathString);
				}
				Console.WriteLine();
				if (NFA.VerifyString(input))
				{
					Console.WriteLine("{0} has verified successfully!", input);
				}
				else
				{
					Console.WriteLine("{0} has not verified.", input);
				}
				Console.WriteLine();
			}
		}

		private static void EditAlphabets()
		{
			Console.Clear();
			Console.WriteLine("1) Add alphabet");
			Console.WriteLine("2) Remove alphabet");
			Console.WriteLine("Escape) Back");
			Console.WriteLine();
			Console.Write("> ");
			ConsoleKeyInfo key;
			do
			{
				key = Console.ReadKey(true);
			} while (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.Escape);
			switch (key.Key)
			{
				case ConsoleKey.D1:
					Console.Clear();
					while (true)
					{
						Console.Write("Enter alphabet to add or type \"Back\" to end: ");
						string input = Console.ReadLine();
						if (input.Trim().ToLower() != "back" && input.Trim().Length != 1)
						{
							Console.Write("Invalid input.\n\n");
							continue;
						}
						if (input.Trim().ToLower() == "back")
						{
							break;
						}
						char a = input.Trim()[0];
						if (NFA.AddAlphabet(a))
						{
							Console.WriteLine("{0} has been added to alphabets.", a);
						}
						else
						{
							Console.WriteLine("{0} is already in the alphabets.", a);
						}
						Console.WriteLine();
					}
					break;
				case ConsoleKey.D2:
					Console.Clear();
					while (true)
					{
						Console.Write("Enter alphabet to remove or type \"Back\" to end: ");
						string input = Console.ReadLine();
						if (input.Trim().ToLower() != "back" && input.Trim().Length != 1)
						{
							Console.Write("Invalid input.\n\n");
							continue;
						}
						if (input.Trim().ToLower() == "back")
						{
							break;
						}
						char a = input.Trim()[0];
						if (NFA.RemoveAlphabet(a))
						{
							Console.WriteLine("{0} has been removed from alphabets.", a);
						}
						else
						{
							Console.WriteLine("{0} does not exists in the alphabets.", a);
						}
						Console.WriteLine();
					}
					break;
				default:
					break;
			}
		}

		private static void EditStates()
		{
			Console.Clear();
			Console.WriteLine("1) Add state");
			Console.WriteLine("2) Remove state");
			Console.WriteLine("Escape) Back");
			Console.WriteLine();
			Console.Write("> ");
			ConsoleKeyInfo key;
			do
			{
				key = Console.ReadKey(true);
			} while (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.Escape);
			switch (key.Key)
			{
				case ConsoleKey.D1:
					Console.Clear();
					while (true)
					{
						Console.Write("Enter state to add or type \"Back\" to end: ");
						string input = Console.ReadLine();
						if (input.Trim().Length < 1)
						{
							Console.Write("Invalid input.\n\n");
							continue;
						}
						if (input.Trim().ToLower() == "back")
						{
							break;
						}
						if (NFA.AddState(input))
						{
							Console.WriteLine("{0} has been added to states.", input);
						}
						else
						{
							Console.WriteLine("{0} is already in the states.", input);
						}
						Console.WriteLine();
					}
					break;
				case ConsoleKey.D2:
					Console.Clear();
					while (true)
					{
						Console.Write("Enter state to remove or type \"Back\" to end: ");
						string input = Console.ReadLine();
						if (input.Trim().Length < 1)
						{
							Console.Write("Invalid input.\n\n");
							continue;
						}
						if (input.Trim().ToLower() == "back")
						{
							break;
						}
						if (NFA.RemoveState(input))
						{
							Console.WriteLine("{0} has been removed from states.", input);
						}
						else
						{
							Console.WriteLine("{0} does not exists in the states.", input);
						}
						Console.WriteLine();
					}
					break;
				default:
					break;
			}
		}

		private static void EditInitialStates()
		{
			Console.Clear();
			Console.WriteLine("1) Add initial state");
			Console.WriteLine("2) Remove initial state");
			Console.WriteLine("Escape) Back");
			Console.WriteLine();
			Console.Write("> ");
			ConsoleKeyInfo key;
			do
			{
				key = Console.ReadKey(true);
			} while (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.Escape);
			switch (key.Key)
			{
				case ConsoleKey.D1:
					Console.Clear();
					while (true)
					{
						Console.Write("Enter initial state to add or type \"Back\" to end: ");
						string input = Console.ReadLine();
						if (input.Trim().Length < 1)
						{
							Console.Write("Invalid input.\n\n");
							continue;
						}
						if (input.Trim().ToLower() == "back")
						{
							break;
						}
						if (NFA.AddInitialState(input))
						{
							Console.WriteLine("{0} has been added to initial states.", input);
						}
						else
						{
							Console.WriteLine("Unable to add {0} to initial states.", input);
						}
						Console.WriteLine();
					}
					break;
				case ConsoleKey.D2:
					Console.Clear();
					while (true)
					{
						Console.Write("Enter initial state to remove or type \"Back\" to end: ");
						string input = Console.ReadLine();
						if (input.Trim().Length < 1)
						{
							Console.Write("Invalid input.\n\n");
							continue;
						}
						if (input.Trim().ToLower() == "back")
						{
							break;
						}
						if (NFA.RemoveInitialState(input))
						{
							Console.WriteLine("{0} has been removed from initial states.", input);
						}
						else
						{
							Console.WriteLine("{0} does not exists in the initial states.", input);
						}
						Console.WriteLine();
					}
					break;
				default:
					break;
			}
		}

		private static void EditFinalStates()
		{
			Console.Clear();
			Console.WriteLine("1) Add final state");
			Console.WriteLine("2) Remove final state");
			Console.WriteLine("Escape) Back");
			Console.WriteLine();
			Console.Write("> ");
			ConsoleKeyInfo key;
			do
			{
				key = Console.ReadKey(true);
			} while (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.Escape);
			switch (key.Key)
			{
				case ConsoleKey.D1:
					Console.Clear();
					while (true)
					{
						Console.Write("Enter final state to add or type \"Back\" to end: ");
						string input = Console.ReadLine();
						if (input.Trim().Length < 1)
						{
							Console.Write("Invalid input.\n\n");
							continue;
						}
						if (input.Trim().ToLower() == "back")
						{
							break;
						}
						if (NFA.AddFinalState(input))
						{
							Console.WriteLine("{0} has been added to final states.", input);
						}
						else
						{
							Console.WriteLine("Unable to add {0} to final states.", input);
						}
						Console.WriteLine();
					}
					break;
				case ConsoleKey.D2:
					Console.Clear();
					while (true)
					{
						Console.Write("Enter final state to remove or type \"Back\" to end: ");
						string input = Console.ReadLine();
						if (input.Trim().Length < 1)
						{
							Console.Write("Invalid input.\n\n");
							continue;
						}
						if (input.Trim().ToLower() == "back")
						{
							break;
						}
						if (NFA.RemoveFinalState(input))
						{
							Console.WriteLine("{0} has been removed from final states.", input);
						}
						else
						{
							Console.WriteLine("{0} does not exists in the final states.", input);
						}
						Console.WriteLine();
					}
					break;
				default:
					break;
			}
		}

		private static void EditInstructions()
		{
			Console.Clear();
			Console.WriteLine("1) Add instruction");
			Console.WriteLine("2) Remove instruction");
			Console.WriteLine("Escape) Back");
			Console.WriteLine();
			Console.Write("> ");
			ConsoleKeyInfo key;
			do
			{
				key = Console.ReadKey(true);
			} while (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.Escape);
			switch (key.Key)
			{
				case ConsoleKey.D1:
					Console.Clear();
					while (true)
					{
						Console.Write("Enter instruction to add or type \"Back\" to end\nexample: state1 a state2\n> ");
						string input = Console.ReadLine();
						if (input.Trim().ToLower() == "back")
						{
							break;
						}
						string[] s = input.Trim().Split(new char[] { ' ' });
						if (s.Count() < 3 || s.Count() > 3 || s[1].Length != 1)
						{
							Console.Write("Invalid input.\n\n");
							continue;
						}
						Instruction ins = new Instruction(s[0], s[1][0], s[2]);
						if (NFA.AddInstruction(ins))
						{
							Console.WriteLine("\"{0}\" has been added to instructions.", ins.ToString());
						}
						else
						{
							Console.WriteLine("Unable to add \"{0}\" to instructions.", ins.ToString());
						}
						Console.WriteLine();
					}
					break;
				case ConsoleKey.D2:
					Console.Clear();
					while (true)
					{
						Console.Write("Enter instruction to remove or type \"Back\" to end\nexample: state1 a state2\n> ");
						string input = Console.ReadLine();
						if (input.Trim().ToLower() == "back")
						{
							break;
						}
						string[] s = input.Trim().Split(new char[] { ' ' });
						if (s.Count() < 3 || s.Count() > 3 || s[1].Length != 1)
						{
							Console.Write("Invalid input.\n\n");
							continue;
						}
						Instruction ins = new Instruction(s[0], s[1][0], s[2]);
						if (NFA.RemoveInstruction(ins))
						{
							Console.WriteLine("\"{0}\" has been removed from instructions.", ins.ToString());
						}
						else
						{
							Console.WriteLine("Unable to remove \"{0}\" from instructions.", ins.ToString());
						}
						Console.WriteLine();
					}
					break;
				default:
					break;
			}
		}

		static void DisplayMenu()
		{
			Console.Clear();
			Console.WriteLine("NFA machine:\n");
			Console.Write("Alphabets:  ");
			foreach (var item in NFA.GetAlphabets())
			{
				Console.Write(item + "  ");
			}
			Console.WriteLine();
			Console.Write("States:  ");
			foreach (var item in NFA.GetStates())
			{
				Console.Write(item + "  ");
			}
			Console.WriteLine();
			Console.Write("Initial states:  ");
			foreach (var item in NFA.GetInitialStates())
			{
				Console.Write(item + "  ");
			}
			Console.WriteLine();
			Console.Write("Final states:  ");
			foreach (var item in NFA.GetFinalStates())
			{
				Console.Write(item + "  ");
			}
			Console.WriteLine();
			Console.Write("Instructions:\t");
			foreach (var item in NFA.GetInstructions())
			{
				Console.Write(item + "\n\t\t");
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("1) Edit alphabets");
			Console.WriteLine("2) Edit states");
			Console.WriteLine("3) Edit initial states");
			Console.WriteLine("4) Edit final states");
			Console.WriteLine("5) Edit instructions");
			Console.WriteLine("6) Validate string");
			Console.WriteLine("7) DFA equivalent");
			Console.WriteLine("8) Clear machine");
			Console.WriteLine("0) Exit");
			Console.WriteLine();
		}

		private static bool ValidateKey(ConsoleKeyInfo key)
		{
			if (key.Key == ConsoleKey.D0 ||
				key.Key == ConsoleKey.D1 ||
				key.Key == ConsoleKey.D2 ||
				key.Key == ConsoleKey.D3 ||
				key.Key == ConsoleKey.D4 ||
				key.Key == ConsoleKey.D5 ||
				key.Key == ConsoleKey.D6 ||
				key.Key == ConsoleKey.D7 ||
				key.Key == ConsoleKey.D8 ||
				key.Key == ConsoleKey.Escape
				)
			{
				return true;
			}
			return false;
		}
	}
}
