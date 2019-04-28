using System;
using System.Windows.Forms;

namespace RuleBuilder.Rule {
	internal class Component {
		public CharacterSet CharacterSet { get; set; }
		public uint MinCount { get; set; } = 0;
		public virtual void CreateNameCell(DataGridViewCell cell) {
			cell.ReadOnly = true;
			cell.Value = this.CharacterSet.Label;
		}
	}
}
