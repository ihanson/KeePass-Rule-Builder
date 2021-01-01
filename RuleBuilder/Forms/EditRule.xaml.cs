using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace RuleBuilder.Forms {
	public partial class EditRule : Window {
		private EditRule(KeePass.Forms.MainForm mainForm, Rule.IPasswordGenerator generator) {
			this.Data = new EditRuleModel(generator);
			this.Data.PasswordRule.Components.Add(new Rule.Component(null, false));
			this.DataContext = this.Data;
			new WindowInteropHelper(this).Owner = mainForm.Handle;
			InitializeComponent();
			(this.Data.RuleType == RuleType.Rule ? this.rdoRule : this.rdoProfile).IsChecked = true;
			this.GenerateExamplePassword();
			this.Data.RuleChanged += () => this.GenerateExamplePassword();
			this.dgComponents.SelectedIndex = 0;
			this.BuildMenu();
		}

		public EditRuleModel Data { get; }

		private ContextMenu CharacterClassMenu { get; } = new ContextMenu() {
			Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom
		};

		private void BuildMenu() {
			this.CharacterClassMenu.Items.Clear();
			foreach (Rule.CharacterClass characterClass in new Rule.CharacterClass[] {
				Rule.CharacterClass.AllCharacters,
				Rule.CharacterClass.Letters,
				Rule.CharacterClass.Digits,
				Rule.CharacterClass.Punctuation,
				Rule.CharacterClass.UppercaseLetters,
				Rule.CharacterClass.LowercaseLetters
			}) {
				this.CharacterClassMenu.Items.Add(this.CharacterClassItem(characterClass.Name, characterClass));
			}
			this.CharacterClassMenu.Items.Add(new Separator());
			this.CharacterClassMenu.Items.Add(this.CharacterClassItem(Properties.Resources.Custom, new Rule.CharacterClass()));
		}

		private MenuItem CharacterClassItem(string caption, Rule.CharacterClass characterClass) {
			MenuItem item = new MenuItem() {
				Header = caption
			};
			item.Click += (_1, _2) => {
				IList<Rule.Component> components = this.Data.PasswordRule.Components;
				int index = components.Count - 1;
				components.Insert(index, new Rule.Component(characterClass.Clone(), false));
				this.dgComponents.SelectedIndex = index;
			};
			return item;
		}

		public static bool ShowRuleDialog(KeePass.Forms.MainForm mainForm, ref Rule.IPasswordGenerator generator) {
			if (mainForm == null) {
				throw new ArgumentNullException(nameof(mainForm));
			}
			EditRule window = new EditRule(mainForm, generator);
			_ = window.ShowDialog();
			generator = window.Data.SelectedGenerator;
			return window.DialogResult ?? false;
		}

		private void RuleOrProfileChecked(object sender, RoutedEventArgs e) {
			bool isRule = rdoRule.IsChecked ?? false;
			this.Data.RuleType = isRule ? RuleType.Rule : RuleType.Profile;
			this.gridRule.Visibility = isRule ? Visibility.Visible : Visibility.Hidden;
			this.lbProfiles.Visibility = isRule ? Visibility.Hidden : Visibility.Visible;
		}

		private void GeneratePasswordClicked(object sender, RoutedEventArgs e) {
			this.GenerateExamplePassword();
		}

		private void GenerateExamplePassword() {
			try {
				this.txtEntropy.Text = (this.Data.Generator() as Rule.PasswordRule)?.EntropyBits().ToString("N0", CultureInfo.CurrentCulture.NumberFormat) ?? string.Empty;
			} catch (ArgumentOutOfRangeException) {
				this.txtEntropy.Text = "Unknown";
			}
			this.txtExample.Text = this.Data.Generator().NewPassword();
		}

		private void AcceptClicked(object sender, RoutedEventArgs e) {
			this.Data.SelectedGenerator = this.Data.Generator();
			this.DialogResult = true;
		}

		private void DataGridSelected(object sender, RoutedEventArgs e) {
			_ = ((DataGrid)sender).BeginEdit(e);
		}

		private void AddRowClicked(object sender, RoutedEventArgs e) {
			this.CharacterClassMenu.PlacementTarget = (Button)sender;
			this.CharacterClassMenu.IsOpen = true;
		}

		private void DeleteRowClicked(object sender, RoutedEventArgs e) {
			int oldIndex = this.dgComponents.SelectedIndex;
			this.Data.PasswordRule.Components.Remove(dgComponents.SelectedItem as Rule.Component);
			this.dgComponents.SelectedIndex = oldIndex;
		}

		private void SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
			this.btnDeleteRow.IsEnabled = (((DataGrid)sender).SelectedItem as Rule.Component)?.CharacterClass != null;
		}

		private void TextBoxLoaded(object sender, RoutedEventArgs e) {
			TextBox textBox = (TextBox)sender;
			if (this.dgComponents.SelectedValue == textBox.DataContext) {
				_ = textBox.Focus();
			}
		}

		private void FormClosing(object sender, System.ComponentModel.CancelEventArgs e) {
			this.Data.PasswordRule.Components.RemoveAt(this.Data.PasswordRule.Components.Count - 1);
		}
	}
}
