using KeePass.Plugins;
using KeePassLib;
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
			PwEntry entry = this.host.MainWindow.GetSelectedEntry(true);
			if (entry != null) {
				if (Forms.ChangePassword.ShowChangePasswordDialog(this.host.MainWindow.ActiveDatabase, entry)) {
					this.host.MainWindow.UpdateUI(false, null, false, null, false, null, true);
				}
			}
		}

		private IPluginHost host;
	}
}
