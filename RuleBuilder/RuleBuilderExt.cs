using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KeePass.Plugins;
using KeePassLib;
using RuleBuilder.Properties;
using RuleBuilder.Rule;

namespace RuleBuilder {
	public class RuleBuilderExt : Plugin {
		private IPluginHost host;
		public override bool Initialize(IPluginHost host) {
			this.host = host;
			foreach (ToolStripItem item in new ToolStripItem[] {
				new ToolStripSeparator(),
				MenuItem(Resources.GenerateNewPassword, this.ShowChangePassword, Resources.NewPassword),
				MenuItem(Resources.EditPasswordRule, this.ShowChangeRule, null)
			}) {
				_ = this.host.MainWindow.EntryContextMenu.Items.Add(item);
				this.host.MainWindow.EntryContextMenu.Opening += (object sender, CancelEventArgs args) => {
					item.Visible = this.host.MainWindow.GetSelectedEntriesCount() == 1;
				};
			}
			_ = this.host.MainWindow.GroupContextMenu.Items.Add(
				MenuItem(Resources.EditPasswordRule, this.ShowGroupChangeRule, null)
			);
			return base.Initialize(host);
		}
		public override string UpdateUrl => "https://raw.githubusercontent.com/ihanson/KeePass-Rule-Builder/master/RuleBuilder/version.txt";
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
				if (Forms.ChangePassword.ShowChangePasswordDialog(this.host.MainWindow, entry)) {
					this.host.MainWindow.UpdateUI(false, null, false, null, false, null, true);
				}
			}
		}
		private void ShowChangeRule() {
			PwEntry entry = this.host.MainWindow.GetSelectedEntry(true);
			if (entry != null) {
				Configuration config = Rule.Serialization.Entry.EntryConfiguration(entry);
				if (Forms.EditRule.ShowRuleDialog(this.host.MainWindow, ref config)) {
					Rule.Serialization.Entry.SetEntryConfiguration(entry, config);
					entry.Touch(true);
					this.host.MainWindow.UpdateUI(false, null, false, null, false, null, true);
				}
			}
		}

		private void ShowGroupChangeRule() {
			PwGroup group = this.host.MainWindow.GetSelectedGroup();
			if (group != null) {
				Configuration config = Rule.Serialization.Entry.GroupConfiguration(group);
				if (Forms.EditRule.ShowRuleDialog(this.host.MainWindow, ref config)) {
					Rule.Serialization.Entry.SetGroupConfiguration(group, config);
					group.Touch(true);
					this.host.MainWindow.UpdateUI(false, null, false, null, false, null, true);
				}
			}
		}
	}
}
