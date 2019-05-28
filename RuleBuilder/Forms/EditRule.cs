using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RuleBuilder.Forms {
	internal partial class EditRule : Form {
		public Rule.PasswordRule PasswordRule { get; set; } = new Rule.PasswordRule();
		public EditRule() {
			this.InitializeComponent();
			this.MinColIndex = this.dgvComponents.Columns.Add(new NumberColumn.NumberColumn() {
				HeaderText = "Minimum",
				SortMode = DataGridViewColumnSortMode.NotSortable
			});
			this.CharColIndex = this.dgvComponents.Columns[CharCol].Index;
			this.addButton = new DataGridViewButtonCell() {
				Value = "Add Character Set",
				ContextMenuStrip = this.mnuComponents
			};
			this.BuildMenu();
			int rowIndex = this.dgvComponents.Rows.Add();
			this.dgvComponents[this.CharColIndex, rowIndex] = this.addButton;
			this.dgvComponents[this.MinColIndex, rowIndex].ReadOnly = true;
			this.udPasswordLength.Select(udPasswordLength.Text.Length, 0);
		}
		private void BuildMenu() {
			for (int i = 0; i < NamedCharacterSets.Length; i++) {
				Rule.NamedCharacterSet charSet = NamedCharacterSets[i];
				ToolStripItem item = new ToolStripMenuItem(charSet.Name);
				item.Click += (object sender, EventArgs e) => this.AddCharacterSet(charSet);
				this.mnuComponents.Items.Insert(i, item);
			}
		}
		private void ShowExample() {
			using (RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider()) {
				txtExample.Text = this.PasswordRule.NewPassword(csp);
			}
		}
		private void AddCharacterSet(Rule.NamedCharacterSet charSet) {
			int rowIndex = this.dgvComponents.RowCount - 1;
			this.dgvComponents.Rows.Insert(rowIndex, new object[] { charSet.Name});
			this.dgvComponents[this.CharColIndex, rowIndex].ReadOnly = true;
			this.dgvComponents[this.MinColIndex, rowIndex].Value = 0;
			this.PasswordRule.Components.Insert(rowIndex, new Rule.Component(charSet.Characters, 0));
			this.ShowExample();
		}
		private void AddCustom() {
			int rowIndex = this.dgvComponents.RowCount - 1;
			DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell() {
				Style = new DataGridViewCellStyle() {
					Font = new Font("Consolas", this.Font.Size)
				}
			};
			this.dgvComponents.Rows.Insert(rowIndex);
			this.dgvComponents[this.CharColIndex, rowIndex] = cell;
			this.dgvComponents[this.MinColIndex, rowIndex].Value = 0;
			this.PasswordRule.Components.Insert(rowIndex, new Rule.Component(new char[] { }, 0));
			cell.ReadOnly = false;
			this.dgvComponents.CurrentCell = cell;
			this.ShowExample();
		}
		private void OnLengthUpdate(object sender, EventArgs e) {
			this.PasswordRule.Length = (uint)this.udPasswordLength.Value;
			this.ShowExample();
		}
		private void OnRefreshClick(object sender, EventArgs e) => this.ShowExample();
		private void OnCellClick(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex < 0 || e.RowIndex < 0) {
				return;
			}
			DataGridView grid = sender as DataGridView;
			DataGridViewCell cell = grid[e.ColumnIndex, e.RowIndex];
			if (cell == this.addButton) {
				Rectangle rect = grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
				this.mnuComponents.Show(grid, new Point(rect.Left, rect.Bottom));
			}
		}
		private readonly DataGridViewButtonCell addButton;
		private readonly int CharColIndex;
		private readonly int MinColIndex;
		private const string CharCol = "Characters";
		private void AddCustom(object sender, EventArgs e) => this.AddCustom();
		private void OnCellValueChange(object sender, DataGridViewCellEventArgs e) {
			if (e.RowIndex < 0) {
				return;
			}
			DataGridView grid = sender as DataGridView;
			DataGridViewCell cell = grid[e.ColumnIndex, e.RowIndex];
			if (!cell.IsInEditMode) {
				return;
			}
			if (e.ColumnIndex == this.CharColIndex) {
				this.PasswordRule.Components[e.RowIndex].CharacterSet = (cell.Value as string).ToCharArray();
			} else if (e.ColumnIndex == this.MinColIndex) {
				this.PasswordRule.Components[e.RowIndex].MinCount = (int)cell.Value;
			}
			this.ShowExample();
		}
		private void OnDirtyStateChange(object sender, EventArgs e) {
			DataGridView grid = sender as DataGridView;
			if (grid.IsCurrentCellDirty) {
				_ = grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
		private void OnDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			DataGridView grid = sender as DataGridView;
			if (e.Row.Index == grid.RowCount - 1) {
				e.Cancel = true;
			} else {
				this.PasswordRule.Components.RemoveAt(e.Row.Index);
				this.ShowExample();
			}
		}
		private void OnDeleteRowClick(object sender, EventArgs e) {
			int rowIndex = this.dgvComponents.SelectedRows[0].Index;
			this.dgvComponents.Rows.RemoveAt(rowIndex);
			this.PasswordRule.Components.RemoveAt(rowIndex);
			this.ShowExample();
		}
		private void OnSelectionChange(object sender, EventArgs e) {
			DataGridView grid = sender as DataGridView;
			this.btnDeleteRow.Enabled = grid.SelectedRows.Count > 0 && grid.SelectedRows[0].Index < grid.RowCount - 1;
		}
		private static readonly Rule.NamedCharacterSet[] NamedCharacterSets = new Rule.NamedCharacterSet[] {
			Rule.NamedCharacterSet.AllCharacters,
			Rule.NamedCharacterSet.Letters,
			Rule.NamedCharacterSet.Digits,
			Rule.NamedCharacterSet.Punctuation,
			Rule.NamedCharacterSet.UppercaseLetters,
			Rule.NamedCharacterSet.LowercaseLetters
		};
	}
}