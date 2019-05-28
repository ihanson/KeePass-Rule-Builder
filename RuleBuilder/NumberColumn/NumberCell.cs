using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace RuleBuilder.NumberColumn {
	class NumberCell : DataGridViewTextBoxCell {
		public NumberCell() : this(0, 99) { }
		public NumberCell(int min, int max) : base() {
			this.Style.Format = "F0";
			this.Minimum = min;
			this.Maximum = max;
		}
		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle) {
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			NumberCellControl ctl = DataGridView.EditingControl as NumberCellControl;
			ctl.Value = (int)this.Value;
			ctl.Minimum = this.Minimum;
			ctl.Maximum = this.Maximum;
		}
		public override Type EditType => typeof(NumberCellControl);
		public override Type ValueType => typeof(int);
		public override object DefaultNewRowValue => 0;
		public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter) => formattedValue;
		public int Minimum { get; }
		public int Maximum { get; }
	}
}