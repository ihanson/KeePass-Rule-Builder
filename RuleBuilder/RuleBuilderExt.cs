using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.Plugins;
using KeePass.UI;
using KeePassLib;
using KeePassLib.Security;
using RuleBuilder.Properties;
using RuleBuilder.Rule;
using RuleBuilder.Rule.Serialization;

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

			GlobalWindowManager.WindowAdded += new EventHandler<GwmWindowEventArgs>((object _, GwmWindowEventArgs args) => {
				if (args.Form is PwEntryForm entryForm) {
					this.ModifyEntryForm(entryForm);
				}
			});
			return base.Initialize(host);
		}

		public override string UpdateUrl => "https://raw.githubusercontent.com/ihanson/KeePass-Rule-Builder/master/RuleBuilder/version.txt";

		private static ToolStripMenuItem MenuItem(string text, Action action, Image image) => new ToolStripMenuItem(text, image, (object _1, EventArgs _2) => action());

		private void ShowChangePassword() {
			PwEntry entry = this.host.MainWindow.GetSelectedEntry(true);
			if (entry != null) {
				if (Forms.ChangePassword.ShowChangePasswordDialog(this.host.MainWindow, entry, true)) {
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
				Configuration config = Rule.Serialization.Entry.GroupConfiguration(group);
				if (Forms.EditRule.ShowRuleDialog(this.host.MainWindow, ref config)) {
					Entry.SetGroupConfiguration(group, config);
					group.Touch(true);
					this.host.MainWindow.UpdateUI(false, null, false, null, true, null, true);
				}
			}
		}

		private void ModifyEntryForm(PwEntryForm entryForm) {
			int index = 0;
			foreach (ToolStripItem item in new ToolStripItem[] {
				MenuItem(Resources.GenerateFromRule, () => this.GenerateInEntryForm(entryForm), Resources.NewPassword),
				MenuItem(Resources.EditPasswordRule, () => this.EditRuleInEntryForm(entryForm), null),
				new ToolStripSeparator()
			}) {
				entryForm.PasswordGeneratorContextMenu.Items.Insert(index++, item);
			}
		}

		private void GenerateInEntryForm(PwEntryForm entryForm) {
			entryForm.UpdateEntryStrings(true, false);
			PwEntry editEntry = entryForm.EntryRef.CloneDeep();
			editEntry.Strings.Set(PwDefs.PasswordField, entryForm.EntryStrings.GetSafe(PwDefs.PasswordField));
			if (entryForm.EntryStrings.Exists(Entry.PasswordRuleKey)) {
				editEntry.Strings.Set(Entry.PasswordRuleKey, entryForm.EntryStrings.GetSafe(Entry.PasswordRuleKey));
			} else {
				editEntry.Strings.Remove(Entry.PasswordRuleKey);
			}
			if (Forms.ChangePassword.ShowChangePasswordDialog(this.host.MainWindow, editEntry, false)) {
				entryForm.EntryStrings.Set(PwDefs.PasswordField, editEntry.Strings.GetSafe(PwDefs.PasswordField));
				if (editEntry.Strings.Exists(Entry.PasswordRuleKey)) {
					entryForm.EntryStrings.Set(Entry.PasswordRuleKey, editEntry.Strings.GetSafe(Entry.PasswordRuleKey));
				} else {
					entryForm.EntryStrings.Remove(Entry.PasswordRuleKey);
				}
				entryForm.UpdateEntryStrings(false, true, true);
			}
		}

		private void EditRuleInEntryForm(PwEntryForm entryForm) {
			entryForm.UpdateEntryStrings(true, false);
			Configuration config = entryForm.EntryStrings.Exists(Entry.PasswordRuleKey)
				? Entry.DeserializedConfiguration(entryForm.EntryStrings.Get(Entry.PasswordRuleKey).ReadString())
				: null;
			if (Forms.EditRule.ShowRuleDialog(this.host.MainWindow, ref config)) {
				if (config == null || config.IsDefaultConfiguration()) {
					entryForm.EntryStrings.Remove(Entry.PasswordRuleKey);
				} else {
					entryForm.EntryStrings.Set(Entry.PasswordRuleKey, new ProtectedString(false, Entry.SerializedConfiguration(config)));
				}
				entryForm.UpdateEntryStrings(false, true, true);
			}
		}
	}
}
