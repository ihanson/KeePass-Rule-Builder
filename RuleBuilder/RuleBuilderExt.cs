using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KeePass.Plugins;
using KeePassLib;
using RuleBuilder.Properties;
using RuleBuilder.Rule;
using RuleBuilder.Rule.Serialization;

namespace RuleBuilder {
	public class RuleBuilderExt : Plugin {
		private IPluginHost host;
		public override bool Initialize(IPluginHost host) {
			this.host = host;
			Util.ScaledResourceManager.Initialize();
			foreach (ToolStripItem item in new ToolStripItem[] {
				new ToolStripSeparator(),
				MenuItem(Resources.GenerateNewPassword, this.ShowChangePassword, Images.Dice),
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

			Forms.EntryFormMod.RegisterEntryForm(host);

			return base.Initialize(host);
		}

		public override string UpdateUrl => "https://raw.githubusercontent.com/ihanson/KeePass-Rule-Builder/main/RuleBuilder/version.txt";

		internal static ToolStripMenuItem MenuItem(string text, Action action, Image image) => new ToolStripMenuItem(text, image, (object _1, EventArgs _2) => action());

		private void ShowChangePassword() {
			PwEntry entry = this.host.MainWindow.GetSelectedEntry(true);
			if (entry != null) {
				if (Forms.ChangePassword.ShowChangePasswordDialog(this.host.MainWindow, entry)) {
					this.host.MainWindow.UpdateUI(false, null, false, null, true, null, true);
				}
			}
		}

		private void ShowChangeRule() {
			PwEntry entry = this.host.MainWindow.GetSelectedEntry(true);
			if (entry != null) {
				Configuration config = Entry.EntryConfiguration(entry);
				if (Forms.EditRule.ShowRuleDialog(this.host.MainWindow, ref config)) {
					Entry.SetEntryConfiguration(entry, config);
					entry.Touch(true);
					this.host.MainWindow.UpdateUI(false, null, false, null, true, null, true);
				}
			}
		}

		private void ShowGroupChangeRule() {
			PwGroup group = this.host.MainWindow.GetSelectedGroup();
			if (group != null) {
				Configuration config = Entry.GroupConfiguration(group);
				if (Forms.EditRule.ShowRuleDialog(this.host.MainWindow, ref config)) {
					Entry.SetGroupConfiguration(group, config);
					group.Touch(true);
					this.host.MainWindow.UpdateUI(false, null, false, null, true, null, true);
				}
			}
		}
	}
}
