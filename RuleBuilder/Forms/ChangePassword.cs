using KeePassLib.Security;
using System.Windows.Forms;

namespace RuleBuilder.Forms {
	public partial class ChangePassword : Form {
		public ChangePassword(string oldPassword) {
			this.InitializeComponent();
			this.Password = oldPassword;
			this.txtOldPassword.Text = oldPassword;
			this.txtNewPassword.Text = oldPassword;
		}
		private void SaveNewPassword(object sender, System.EventArgs e) {
			this.Password = this.txtNewPassword.Text;
		}
		public string Password { get; set; }
		private Rule.IPasswordGenerator Generator { get; set; }
		private void EditRule(object sender, System.EventArgs e) {
			this.Generator = Forms.EditRule.ShowRuleDialog(this.Generator);
			this.txtNewPassword.Text = this.Generator.NewPassword();
		}
	}
}
