using System;
using System.Collections.Generic;
using System.Linq;
using Grammars;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Grammar_WinPhone
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private Grammar grammar = new Grammar();

		private double FontSize1 = 26;
		private double FontSize2 = 24;
		private double Margin1 = 4;

		InputPane inputPane = InputPane.GetForCurrentView();
		

		public MainPage()
		{
			InitializeComponent();
			Init();
			NavigationCacheMode = NavigationCacheMode.Required;

			inputPane.Showing += OnInputPaneShowing;
			inputPane.Hiding += OnInputPaneHiding;
		}

		private void OnInputPaneHiding(InputPane sender, InputPaneVisibilityEventArgs args)
		{
			CommandBar.Visibility = Visibility.Visible;
		}

		private void OnInputPaneShowing(InputPane sender, InputPaneVisibilityEventArgs args)
		{
			CommandBar.Visibility = Visibility.Collapsed;
		}

		private void Init()
		{
			// handle back key press
			HardwareButtons.BackPressed += HardwareButtons_BackPressed;

			// init status bar
			var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
			statusBar.BackgroundColor = Color.FromArgb(255, 60, 124, 188);
			statusBar.BackgroundOpacity = 1;

			// init status area
			StatusGrid.Background = new SolidColorBrush(Color.FromArgb(255, 228, 188, 32));

			// init TextBoxes
			LeftTextBox.FontSize = FontSize1;
			RightTextBox.FontSize = FontSize1;
			LeftTextBox.Text = "S";
			RightTextBox.Text = "";
			UpdateTextBoxes();

			UpdateInstructionsView();
		}

		private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
		{
			e.Handled = true;
			var msg = new MessageDialog("Do you want to exit?", "Exit");
			msg.Commands.Add(new UICommand("Exit", exitMessageDialogHandler));
			msg.Commands.Add(new UICommand("Cancel", exitMessageDialogHandler));
			await msg.ShowAsync();
		}

		private void exitMessageDialogHandler(IUICommand command)
		{
			if (command.Label == "Exit")
			{
				Application.Current.Exit();
			}
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			// TODO: Prepare page for display here.

			// TODO: If your application contains multiple pages, ensure that you are
			// handling the hardware Back button by registering for the
			// Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
			// If you are using the NavigationHelper provided by some templates,
			// this event is handled for you.
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			// Trim TextBoxes
			LeftTextBox.Text = LeftTextBox.Text.Trim();
			RightTextBox.Text = RightTextBox.Text.Trim();

			if (LeftTextBox.Text.Length == 0)
			{
				LeftTextBox.Focus(FocusState.Keyboard);
				return;
			}
			if (RightTextBox.Text.Length == 0)
			{
				RightTextBox.Focus(FocusState.Keyboard);
				return;
			}

			if (!string.IsNullOrEmpty(LeftTextBox.Text) && !string.IsNullOrEmpty(RightTextBox.Text))
			{
				AddInstruction(LeftTextBox.Text[0], RightTextBox.Text);
			}
		}

		private void AddInstruction(char left, string right)
		{
			// Validate inputs
			if (ValidateNonTerminal(left) && ValidateRightSide(right))
			{
				// Add instruction to grammar
				if (right == "λ")
				{
					grammar.instructions.Add(new Instruction(left, ((char)0).ToString()));
				}
				else
				{
					grammar.instructions.Add(new Instruction(left, right));
				}
				UpdateInstructionsView();
			}
		}

		private void RemoveInstruction(char left, string right)
		{
			// Remove from grammar
			if (right == "λ")
			{
				grammar.RemoveInstruction(left, ((char)0).ToString());
			}
			else
			{
				grammar.RemoveInstruction(left, right);
			}
			UpdateInstructionsView();
		}

		private bool ValidateNonTerminal(char nonTerminal)
		{
			if (char.IsUpper(nonTerminal))
			{
				return true;
			}
			return false;
		}

		private bool ValidateRightSide(string text)
		{
			if (text.Contains('$'))
			{
				return false;
			}
			if (text.Length > 1 && text.Contains('λ'))
			{
				return false;
			}
			return true;
		}

		private void UpdateInstructionsView()
		{
			// Refresh grammar view
			InstructionsStackPanel.Children.Clear();
			foreach (char nonTerminal in grammar.NonTerminalsWithInstruction().GetItems())
			{
				StackPanel sp = new StackPanel();
				sp.Orientation = Orientation.Horizontal;
				sp.HorizontalAlignment = HorizontalAlignment.Center;

				TextBlock tb = new TextBlock()
				{
					Foreground = new SolidColorBrush(Colors.Black),
					FontSize = FontSize2,
					FontWeight = FontWeights.SemiBold,
					Margin = new Thickness(Margin1),
					Text = nonTerminal.ToString()
				};
				sp.Children.Add(tb);
				tb = new TextBlock()
				{
					Foreground = new SolidColorBrush(Colors.Black),
					FontSize = FontSize2,
					FontWeight = FontWeights.SemiBold,
					Margin = new Thickness(Margin1),
					Text = "⟶"
				};
				sp.Children.Add(tb);

				List<Instruction> instructions = grammar.instructions.GetItems().Where(i => i.Left == nonTerminal).ToList();
				for (int i = 0; i < instructions.Count; i++)
				{
					tb = new TextBlock()
					{
						Foreground = new SolidColorBrush(Colors.Black),
						FontSize = FontSize2,
						FontWeight = FontWeights.SemiBold,
						Margin = new Thickness(Margin1),
						Text = instructions[i].IsLambda ? "λ" : instructions[i].Right
					};
					tb.Tapped += Instruction_Tapped;
					sp.Children.Add(tb);
					if (i < instructions.Count - 1)
					{
						tb = new TextBlock()
						{
							Foreground = new SolidColorBrush(Colors.Black),
							FontSize = FontSize2,
							Margin = new Thickness(Margin1),
							Text = "|"
						};
						sp.Children.Add(tb);
					}
				}
				InstructionsStackPanel.Children.Add(sp);
			}
			// Display result
			if (grammar.instructions.Count == 0)
			{
				StatusTextBlock.Text = "provide a grammar";
				StatusGrid.Background = new SolidColorBrush(Color.FromArgb(255, 255, 212, 42));
			}
			else
			{
				if (grammar.IsLL1())
				{
					StatusTextBlock.Text = "grammar is LL1";
					StatusGrid.Background = new SolidColorBrush(Color.FromArgb(255, 22, 145, 58));
				}
				else
				{
					StatusTextBlock.Text = "grammar is not LL1";
					StatusGrid.Background = new SolidColorBrush(Color.FromArgb(255, 140, 53, 53));
				}
			}
		}

		private void Instruction_Tapped(object sender, TappedRoutedEventArgs e)
		{
			string right = (sender as TextBlock).Text.ToString();
			char left = (((sender as TextBlock).Parent as StackPanel).Children[0] as TextBlock).Text.ToString()[0];
			RemoveInstruction(left, right);
		}

		private void LambdaButton_Click(object sender, RoutedEventArgs e)
		{
			RightTextBox.Text = "λ";
		}

		private void TextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			var textBox = sender as TextBox;
			textBox.Select(0, 0);
			textBox.Select(textBox.Text.Length, 0);
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = sender as TextBox;
			UpdateTextBox(textBox);
		}

		private void UpdateTextBox(TextBox tb)
		{
			if (tb.Text.Length == 0)
			{
				tb.BorderThickness = new Thickness(2.5);
				tb.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 140, 53, 53));
			}
			else
			{
				tb.BorderThickness = new Thickness(1);
				tb.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 60, 124, 188));
				if (tb.Name == "LeftTextBox")
				{
					if (char.IsLower(tb.Text[0]))
					{
						tb.Text = tb.Text.ToUpper();
						tb.Select(tb.Text.Length, 0);
					}
				}
			}
			
		}

		private void UpdateTextBoxes()
		{
			UpdateTextBox(LeftTextBox);
			UpdateTextBox(RightTextBox);
		}

		private void ClearAppBarButton_Click(object sender, RoutedEventArgs e)
		{
			grammar.Clear();
			UpdateInstructionsView();
		}

		private void ExitAppBarButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Exit();
		}
	}
}
