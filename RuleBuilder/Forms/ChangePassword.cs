using System.Windows.Forms;

namespace RuleBuilder.Forms {
	internal partial class ChangePassword : Form {
		public ChangePassword(string oldPassword, Rule.IPasswordGenerator generator) {
			this.InitializeComponent();
			this.Password = oldPassword;
			this.Generator = generator;
			this.txtOldPassword.Text = oldPassword;
			this.txtNewPassword.Text = this.Generator.NewPassword();
		}
		private void SaveNewPassword(object sender, System.EventArgs e) {
			this.Password = this.txtNewPassword.Text;
			this.SavedChange = true;
		}
		public string Password { get; set; }
		public bool SavedChange { get; private set; } = false;
		public Rule.IPasswordGenerator Generator { get; private set; }
		private void EditRule(object sender, System.EventArgs e) {
			this.Generator = Forms.EditRule.ShowRuleDialog(this.Generator);
			this.txtNewPassword.Text = this.Generator.NewPassword();
		}
	}
}
