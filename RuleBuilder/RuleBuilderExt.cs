using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Security;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace RuleBuilder {
	public class RuleBuilderExt : Plugin {
		public override bool Initialize(IPluginHost host) {
			this.host = host;
			ToolStripMenuItem menuItem = new ToolStripMenuItem("Change Password");
			menuItem.Click += (object sender, EventArgs args) => this.ShowChangePassword();
			this.host.MainWindow.EntryContextMenu.Items.Add(menuItem);
			this.host.MainWindow.EntryContextMenu.Opening +=
				(object sender, CancelEventArgs args) =>
					menuItem.Visible = this.host.MainWindow.GetSelectedEntriesCount() == 1;
			return base.Initialize(host);
		}
		private void ShowChangePassword() {
			PwEntry entry = host.MainWindow.GetSelectedEntry(true);
			if (entry == null) {
				return;
			}
			string oldPassword = entry.Strings.Get(PwDefs.PasswordField).ReadString();
			Forms.ChangePassword form = new Forms.ChangePassword(oldPassword);
			_ = form.ShowDialog();
			if (oldPassword != form.Password) {
				entry.CreateBackup(this.host.MainWindow.ActiveDatabase);
				entry.Strings.Set(PwDefs.PasswordField, new ProtectedString(true, form.Password));
				entry.Touch(true);
				this.host.MainWindow.UpdateUI(false, null, false, null, false, null, true);
			}
		}

		private IPluginHost host;
	}
}
