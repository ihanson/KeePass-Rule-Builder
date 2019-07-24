using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using KeePass.Util;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;

namespace RuleBuilder.Forms {
	internal partial class EditRule : Form {
		private const string CharCol = "Characters";
		private const string RequiredCol = "Required";
		private EditRule(Rule.IPasswordGenerator generator) {
			this.InitializeComponent();
			this.SelectedGenerator = generator;
			this.CharColIndex = this.dgvComponents.Columns[CharCol].Index;
			this.RequiredColIndex = this.dgvComponents.Columns[RequiredCol].Index;
			this.dgvComponents.Columns[RequiredColIndex].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			this.AddButton = new DataGridViewButtonCell() {
				Value = "Add Character Set",
				ContextMenuStrip = this.mnuComponents
			};
			this.BuildMenu();
			int rowIndex = this.dgvComponents.Rows.Add();
			this.dgvComponents[this.CharColIndex, rowIndex] = this.AddButton;
			this.dgvComponents[this.RequiredColIndex, rowIndex] = new DataGridViewTextBoxCell() {
				Value = ""
			};
			this.dgvComponents[this.RequiredColIndex, rowIndex].ReadOnly = true;
			this.udPasswordLength.Select(this.udPasswordLength.Text.Length, 0);
			this.Profiles.Insert(0, Rule.PasswordProfile.DefaultProfile);
			this.lbProfiles.DataSource = this.Profiles;
			this.lbProfiles.DisplayMember = nameof(Rule.PasswordProfile.Name);
			this.LoadGenerator(generator);
			this.ShowPanel();
			this.Loaded = true;
			this.ShowExample();
		}
		private static Rule.CharacterClass[] CharacterClasses => new Rule.CharacterClass[] {
			Rule.CharacterClass.AllCharacters,
			Rule.CharacterClass.Letters,
			Rule.CharacterClass.Digits,
			Rule.CharacterClass.Punctuation,
			Rule.CharacterClass.UppercaseLetters,
			Rule.CharacterClass.LowercaseLetters
		};
		private Rule.PasswordRule PasswordRule { get; set; }
		private List<Rule.PasswordProfile> Profiles { get; } = PwGeneratorUtil.GetAllProfiles(false).ConvertAll((PwProfile profile) => new Rule.PasswordProfile(profile));
		private Rule.IPasswordGenerator SelectedGenerator { get; set; }
		private DataGridViewButtonCell AddButton { get; }
		private bool Loaded { get; set; }
		private int CharColIndex { get; }
		private int RequiredColIndex { get; }
		public static bool ShowRuleDialog(ref Rule.IPasswordGenerator generator) {
			EditRule form = new EditRule(generator);
			_ = form.ShowDialog();
			generator = form.SelectedGenerator;
			return form.DialogResult == DialogResult.Yes;
		}
		private void BuildMenu() {
			for (int i = 0; i < CharacterClasses.Length; i++) {
				Rule.CharacterClass charClass = CharacterClasses[i];
				ToolStripItem item = new ToolStripMenuItem(charClass.Name);
				item.Click += (object sender, EventArgs e) => this.AddCharacterClass(charClass);
				this.mnuComponents.Items.Insert(i, item);
			}
		}
		private void LoadGenerator(Rule.IPasswordGenerator generator) {
			if (generator is Rule.PasswordRule) {
				this.LoadRule((Rule.PasswordRule)generator);
				this.LoadProfile(Rule.PasswordProfile.DefaultProfile);
				this.rdoRule.Checked = true;
			} else if (generator is Rule.PasswordProfile) {
				this.LoadRule(new Rule.PasswordRule());
				this.LoadProfile((Rule.PasswordProfile)generator);
				this.rdoProfile.Checked = true;
			}
		}
		private void LoadRule(Rule.PasswordRule rule) {
			this.PasswordRule = new Rule.PasswordRule() {
				Length = rule.Length
			};
			this.udPasswordLength.Value = rule.Length;
			this.txtExclude.Text = rule.Exclude;
			foreach (Rule.Component component in rule.Components) {
				this.AddComponent(component);
			}
			this.dgvComponents.Rows[0].Selected = true;
		}
		private void LoadProfile(Rule.PasswordProfile profile) {
			this.lbProfiles.SelectedIndex = 0;
			if (!profile.IsDefaultProfile) {
				string name = profile.Name;
				for (int i = 0; i < this.Profiles.Count; i++) {
					if (this.Profiles[i].Name == name) {
						this.lbProfiles.SelectedIndex = i;
						break;
					}
				}
			}
		}
		private Rule.IPasswordGenerator Generator() {
			if (this.rdoRule.Checked) {
				return this.PasswordRule;
			} else {
				return (Rule.PasswordProfile)this.lbProfiles.SelectedValue;
			}
		}
		private void ShowExample() {
			if (!this.Loaded) {
				return;
			}
			if (this.rdoRule.Checked) {
				this.txtExample.Text = this.PasswordRule.NewPassword();
			} else {
				_ = PwGenerator.Generate(out ProtectedString result, ((Rule.PasswordProfile)this.lbProfiles.SelectedValue).Profile, null, KeePass.Program.PwGeneratorPool);
				this.txtExample.Text = result.ReadString();
			}
		}
		private void AddCharacterClass(Rule.CharacterClass charClass) {
			this.AddComponent(new Rule.Component(charClass, false));
			this.ShowExample();
		}
		private void AddCustom() {
			this.AddComponent(new Rule.Component(new Rule.CharacterClass(), false));
			this.ShowExample();
		}
		private void AddComponent(Rule.Component component) {
			int index = this.PasswordRule.Components.Count;
			this.PasswordRule.Components.Insert(index, component);
			if (component.CharacterClass.Enumeration == Rule.CharacterClassEnum.Custom) {
				DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell() {
					Value = component.CharacterClass.Characters,
					Style = new DataGridViewCellStyle() {
						Font = new Font("Consolas", this.Font.Size)
					}
				};
				this.dgvComponents.Rows.Insert(index);
				this.dgvComponents[this.CharColIndex, index] = cell;
				this.dgvComponents[this.RequiredColIndex, index].Value = component.Required;
				cell.ReadOnly = false;
				this.dgvComponents.CurrentCell = cell;
			} else {
				this.dgvComponents.Rows.Insert(index, new object[] { component.CharacterClass.Name });
				this.dgvComponents[this.CharColIndex, index].ReadOnly = true;
				this.dgvComponents[this.RequiredColIndex, index].Value = component.Required;
			}
		}
		private void OnLengthUpdate(object sender, EventArgs e) {
			this.PasswordRule.Length = (int)this.udPasswordLength.Value;
			this.ShowExample();
		}
		private void OnExcludeUpdate(object sender, EventArgs e) {
			this.PasswordRule.Exclude = this.txtExclude.Text;
			this.ShowExample();
		}
		private void OnRefreshClick(object sender, EventArgs e) => this.ShowExample();
		private void OnCellClick(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex < 0 || e.RowIndex < 0) {
				return;
			}
			DataGridView grid = (DataGridView)sender;
			DataGridViewCell cell = grid[e.ColumnIndex, e.RowIndex];
			if (cell == this.AddButton) {
				Rectangle rect = grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
				this.mnuComponents.Show(grid, new Point(rect.Left, rect.Bottom));
			}
		}
		private void AddCustom(object sender, EventArgs e) => this.AddCustom();
		private void OnCellValueChange(object sender, DataGridViewCellEventArgs e) {
			if (e.RowIndex < 0) {
				return;
			}
			DataGridView grid = (DataGridView)sender;
			DataGridViewCell cell = grid[e.ColumnIndex, e.RowIndex];
			if (!cell.IsInEditMode) {
				return;
			}
			if (e.ColumnIndex == this.CharColIndex) {
				this.PasswordRule.Components[e.RowIndex].CharacterClass.Characters = (string)(cell.Value ?? string.Empty);
			} else if (e.ColumnIndex == this.RequiredColIndex) {
				this.PasswordRule.Components[e.RowIndex].Required = (bool)cell.Value;
			}
			this.ShowExample();
		}
		private void OnDirtyStateChange(object sender, EventArgs e) {
			DataGridView grid = (DataGridView)sender;
			if (grid.IsCurrentCellDirty) {
				_ = grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}
		private void OnDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			DataGridView grid = (DataGridView)sender;
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
			DataGridView grid = (DataGridView)sender;
			this.btnDeleteRow.Enabled = grid.SelectedRows.Count > 0 && grid.SelectedRows[0].Index < grid.RowCount - 1;
		}
		private void RuleTypeSelected(object sender, EventArgs e) => this.ShowPanel();
		private void ShowPanel() {
			this.pnlRule.Visible = this.rdoRule.Checked;
			this.pnlProfile.Visible = this.rdoProfile.Checked;
			this.ShowExample();
		}
		private void Save(object sender, EventArgs e) {
			this.SelectedGenerator = this.Generator();
			this.DialogResult = DialogResult.Yes;
		}
	}
}