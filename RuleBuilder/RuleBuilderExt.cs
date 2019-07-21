using KeePass.Plugins;
using KeePass.UI;
using KeePassLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RuleBuilder {
	public class RuleBuilderExt : Plugin {
		public override bool Initialize(IPluginHost host) {
			this.host = host;
			foreach (ToolStripItem item in new ToolStripItem[] {
				new ToolStripSeparator(),
				MenuItem("Generate New Password", this.ShowChangePassword, Properties.Resources.NewPassword),
				MenuItem("Edit Password Rule", this.ShowChangeRule, null)
			}) {
				this.host.MainWindow.EntryContextMenu.Items.Add(item);
				this.host.MainWindow.EntryContextMenu.Opening += (object sender, CancelEventArgs args) => {
					item.Visible = this.host.MainWindow.GetSelectedEntriesCount() == 1;
				};
			}
			return base.Initialize(host);
		}
		private delegate void Action();
		private static ToolStripMenuItem MenuItem(string text, Action action, Image image) {
			ToolStripMenuItem item = new ToolStripMenuItem(text) {
				Image = image
			};
			item.Click += (object sender, EventArgs args) => action();
			return item;
		}
		private void ShowChangePassword() {
			PwEntry entry = this.host.MainWindow.GetSelectedEntry(true);
			if (entry != null) {
				if (Forms.ChangePassword.ShowChangePasswordDialog(this.host.MainWindow.ActiveDatabase, entry)) {
					this.host.MainWindow.UpdateUI(false, null, false, null, false, null, true);
				}
			}
		}
		private void ShowChangeRule() {
			PwEntry entry = this.host.MainWindow.GetSelectedEntry(true);
			if (entry != null) {
				Rule.IPasswordGenerator generator = Rule.Serialization.Entry.EntryGenerator(entry);
				if (Forms.EditRule.ShowRuleDialog(ref generator)) {
					Rule.Serialization.Entry.SetEntryGenerator(entry, generator);
					entry.Touch(true);
					this.host.MainWindow.UpdateUI(false, null, false, null, false, null, true);
				}
			}
		}

		private IPluginHost host;
	}
}
