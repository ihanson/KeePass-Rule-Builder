using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RuleBuilder.NumberColumn {
	public partial class NumberCellControl : NumericUpDown, IDataGridViewEditingControl {
		public NumberCellControl() {
			this.GotFocus += (object sender, EventArgs e) => this.Select(0, this.Text.Length);
		}
		public DataGridView EditingControlDataGridView { get; set; }
		public object EditingControlFormattedValue {
			get => this.Value;
			set {
				if (value is string) {
					try {
						this.Value = int.Parse((string)value);
					} catch {
						this.Value = 0;
					}
				}
			}
		}
		public int EditingControlRowIndex { get; set; }
		public bool EditingControlValueChanged { get; set; }
		public new int Value {
			get => (int)base.Value;
			set => base.Value = (decimal)value;
		}
		public Cursor EditingPanelCursor => base.Cursor;
		public bool RepositionEditingControlOnValueChange => false;
		public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle) {
			this.Font = dataGridViewCellStyle.Font;
			this.ForeColor = dataGridViewCellStyle.ForeColor;
			this.BackColor = dataGridViewCellStyle.BackColor;
		}
		public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey) => !dataGridViewWantsInputKey;
		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) => this.EditingControlFormattedValue;
		public void PrepareEditingControlForEdit(bool selectAll) { }
		protected override void OnValueChanged(EventArgs e) {
			this.EditingControlValueChanged = true;
			this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
			base.OnValueChanged(e);
		}
	}
}
