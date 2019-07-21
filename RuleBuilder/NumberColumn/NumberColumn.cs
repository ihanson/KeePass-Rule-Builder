using System;
using System.Windows.Forms;

namespace RuleBuilder.NumberColumn {
	internal class NumberColumn : DataGridViewColumn {
		public NumberColumn() : base(new NumberCell()) { }
		public override DataGridViewCell CellTemplate {
			get => base.CellTemplate;
			set {
				if (value != null && !value.GetType().IsAssignableFrom(typeof(NumberCell))) {
					throw new InvalidCastException("Must be a NumberCell");
				}
				base.CellTemplate = value;
			}
		}
	}
}
