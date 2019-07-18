using KeePassLib;
using KeePassLib.Security;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RuleBuilder.Forms {
	internal partial class ChangePassword : Form {
		public static bool ShowChangePasswordDialog(PwDatabase database, PwEntry entry) {
			ChangePassword form = new ChangePassword(database, entry);
			_ = form.ShowDialog();
			return form.EntryChanged;
		}
		private ChangePassword(PwDatabase database, PwEntry entry) {
			this.InitializeComponent();
			this.Database = database;
			this.Entry = entry;
			this.Text = $"Change Password: {entry.Strings.Get(PwDefs.TitleField).ReadString()}";
			this.Generator = Rule.Serialization.Entry.EntryGenerator(entry);
			this.txtOldPassword.Text = entry.Strings.Get(PwDefs.PasswordField).ReadString();
			this.txtNewPassword.Text = this.Generator.NewPassword();
			this.OldPasswordHotKeyID = HotKey.RegisterHotKey(this, Keys.Z | Keys.Control | Keys.Shift);
			this.NewPasswordHotKeyID = HotKey.RegisterHotKey(this, Keys.X | Keys.Control | Keys.Shift);
		}
		private void SaveNewPassword(object sender, System.EventArgs e) {
			string oldPassword = this.Entry.Strings.Get(PwDefs.PasswordField).ReadString();
			string newPassword = this.txtNewPassword.Text;
			bool pwChanged = oldPassword != newPassword;
			if (pwChanged || this.RuleChanged) {
				if (pwChanged) {
					this.Entry.CreateBackup(this.Database);
				}
				this.Entry.Strings.Set(PwDefs.PasswordField, new ProtectedString(true, newPassword));
				Rule.Serialization.Entry.SetEntryGenerator(this.Entry, this.Generator);
				this.Entry.Touch(true);
				this.EntryChanged = true;
			}
		}
		private PwDatabase Database { get; }
		private PwEntry Entry { get; }
		private int OldPasswordHotKeyID { get; }
		private int NewPasswordHotKeyID { get; }
		private bool EntryChanged { get; set; } = false;
		private bool RuleChanged { get; set; } = false;
		public Rule.IPasswordGenerator Generator { get; private set; }
		private void EditRule(object sender, System.EventArgs e) {
			Rule.IPasswordGenerator generator = this.Generator;
			if (Forms.EditRule.ShowRuleDialog(ref generator, this.Entry)) {
				this.RuleChanged = true;
				this.Generator = generator;
				this.txtNewPassword.Text = this.Generator.NewPassword();
			}
		}
		private void Refresh(object sender, System.EventArgs e) {
			this.txtNewPassword.Text = this.Generator.NewPassword();
		}
		private const int WM_HOTKEY = 0x312;
		protected override void WndProc(ref Message m) {
			if (m.Msg == WM_HOTKEY) {
				int hotKeyID = (int)m.WParam;
				if (hotKeyID == this.OldPasswordHotKeyID) {
					_ = KeePass.Util.AutoType.PerformIntoCurrentWindow(this.Entry, this.Database, EscapeAutoType(this.txtOldPassword.Text));
				} else if (hotKeyID == this.NewPasswordHotKeyID) {
					_ = KeePass.Util.AutoType.PerformIntoCurrentWindow(this.Entry, this.Database, EscapeAutoType(this.txtNewPassword.Text));
				}
			}
			base.WndProc(ref m);
		}
		private static string EscapeAutoType(string text) => Regex.Replace(text, @"[+%^~()[\]{}]", (Match match) => $"{{{match.Value}}}");
		private void OnFormClosed(object sender, FormClosedEventArgs e) {
			HotKey.UnregisterHotKey(this, this.OldPasswordHotKeyID);
			HotKey.UnregisterHotKey(this, this.NewPasswordHotKeyID);
		}
	}
}
