using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KeePass.Plugins;
using KeePassLib;
using RuleBuilder.Forms;
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

			EntryFormMod.RegisterEntryForm(host);

			return base.Initialize(host);
		}

		public override string UpdateUrl => "https://raw.githubusercontent.com/ihanson/KeePass-Rule-Builder/main/RuleBuilder/version.txt";

		internal static ToolStripMenuItem MenuItem(string text, Action action, Image image) => new ToolStripMenuItem(text, image, (object _1, EventArgs _2) => action());

		private bool HasAddedDiscardedMenu { get; set; }

		private List<GeneratedPassword> DiscardedEntries { get; set; } = new List<GeneratedPassword>();

		private void ShowChangePassword() {
			KeePass.Forms.MainForm mainWindow = this.host.MainWindow;
			PwEntry entry = mainWindow.GetSelectedEntry(true);
			if (entry != null) {
				if (Forms.ChangePassword.ShowChangePasswordDialog(this.host, mainWindow, entry, out List<GeneratedPassword> discardedEntries)) {
					this.RefreshEntries();
				}
				this.DiscardedEntries.AddRange(discardedEntries);
				if (discardedEntries.Count > 0) {
					mainWindow.SetStatusEx(
						string.Format(
							Resources.DiscardedPasswordsInMenu,
							mainWindow.ToolsMenu.Text,
							Resources.DiscardedPasswords
						)
					);
				}
				if (!this.HasAddedDiscardedMenu && this.DiscardedEntries.Count > 0) {
					ToolStripMenuItem menuItem = new ToolStripMenuItem(
						Resources.DiscardedPasswords,
						Images.TrashCan,
						(_1, _2) => {
							if (!mainWindow.IsFileLocked(null)) {
								DiscardedPasswords.ShowDiscardedPasswordDialog(
									this.DiscardedEntries.Where(
										(e) => object.ReferenceEquals(e.SourceDatabase, mainWindow.ActiveDatabase)
									)
								);
							}
						}
					) {
						Enabled = !mainWindow.IsFileLocked(null)
					};
					mainWindow.ToolsMenu.DropDownItems.AddRange(new ToolStripItem[] {
						new ToolStripSeparator(),
						menuItem
					});
					mainWindow.DocumentManager.ActiveDocumentSelected += (_1, _2) => {
						menuItem.Enabled = !mainWindow.IsFileLocked(null);
					};
					mainWindow.FileOpened += (_, e) => {
						menuItem.Enabled = !mainWindow.IsFileLocked(null);
					};
					mainWindow.FileClosingPost += (_, e) => {
						menuItem.Enabled = false;
						if ((e.Flags & KeePass.Forms.FileEventFlags.Locking) == 0) {
							this.DiscardedEntries.RemoveAll(
								(password) => object.ReferenceEquals(password.SourceDatabase, e.Database)
							);
						}
					};
					this.HasAddedDiscardedMenu = true;
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
					this.RefreshEntries();
				}
			}
		}

		private void ShowGroupChangeRule() {
			PwGroup group = this.host.MainWindow.GetSelectedGroup();
			if (group != null) {
				Configuration config = Entry.GroupConfiguration(group);
				if (EditRule.ShowRuleDialog(this.host.MainWindow, ref config)) {
					Entry.SetGroupConfiguration(group, config);
					group.Touch(true);
					this.RefreshEntries();
				}
			}
		}

		private void RefreshEntries() {
			this.host.MainWindow.RefreshEntriesList();
			this.host.MainWindow.UpdateUI(false, null, false, null, false, null, true);
		}
	}
}
