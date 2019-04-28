using System.Security.Cryptography;
using System.Windows.Forms;

namespace RuleBuilder.Forms {
	internal partial class EditRule : Form {
		public Rule.PasswordRule PasswordRule { get; set; } = new Rule.PasswordRule();
		public EditRule() {
			this.InitializeComponent();
			this.ruleSource.DataSource = this.PasswordRule;
			this.SetUpComboBox();
		}
		private void SetUpComboBox() {
			this.cboAdd.DisplayMember = nameof(Rule.CharacterSet.Label);
			this.cboAdd.Items.AddRange(new Rule.CharacterSet[] {
				Rule.CharacterSet.AllCharacters,
				Rule.CharacterSet.Letters,
				Rule.CharacterSet.Digits,
				Rule.CharacterSet.Punctuation,
				Rule.CharacterSet.UppercaseLetters,
				Rule.CharacterSet.LowercaseLetters
			});
		}
		private void ShowExample() {
			using (RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider()) {
				txtExample.Text = this.PasswordRule.NewPassword(csp);
			}
		}
		private void AddRow(object sender, System.EventArgs e) {
			ComboBox comboBox = sender as ComboBox;
			if (comboBox.SelectedItem == null) {
				return;
			}
			this.PasswordRule.Components.Add(new Rule.Component() {
				CharacterSet = comboBox.SelectedItem as Rule.CharacterSet
			});
			comboBox.SelectedIndex = -1;
		}
		private void AddCharacterSet(object sender, DataGridViewRowsAddedEventArgs e) {
			DataGridView dataGridView = sender as DataGridView;
			this.PasswordRule.Components[e.RowIndex].CreateNameCell(dataGridView[this.colCharacterSet.Index, e.RowIndex]);
		}
		private void HandleError(object sender, DataGridViewDataErrorEventArgs e) {
			DataGridView grid = sender as DataGridView;
			if (grid.Columns[e.ColumnIndex].ValueType == typeof(uint)) {
				_ = MessageBox.Show("Enter a valid number.", "Entry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void OnGridUpdate(object sender, DataGridViewCellEventArgs e) => this.ShowExample();
		private void OnLengthUpdate(object sender, System.EventArgs e) => this.ShowExample();
		private void RefreshClick(object sender, System.EventArgs e) => this.ShowExample();

		private void AddCustom(object sender, System.EventArgs e) {
			this.PasswordRule.Components.Add(new Rule.CustomComponent());
		}
	}
}