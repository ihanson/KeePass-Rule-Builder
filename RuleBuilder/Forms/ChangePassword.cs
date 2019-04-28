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
			this.Password = txtNewPassword.Text;
			this.Close();
		}
		public string Password { get; set; }
		private void Button1_Click(object sender, System.EventArgs e) {
			new EditRule().ShowDialog();
		}
	}
}
