using System.Drawing;
using System.Windows.Forms;

namespace RuleBuilder.Rule {
	internal class CustomComponent : Component {
		private DataGridViewCell cell;
		public new CharacterSet CharacterSet => new CharacterSet() { Characters = ((string)this.cell.Value).ToCharArray() };
		public override void CreateNameCell(DataGridViewCell cell) {
			cell.ReadOnly = false;
			cell.Style = CellStyle;
			this.cell = cell;
		}
		private static readonly DataGridViewCellStyle CellStyle = new DataGridViewCellStyle() {
			Font = new Font("Consolas", 10f)
		};
	}
}
