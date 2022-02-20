using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KeePass;
using KeePass.App.Configuration;
using KeePass.Forms;
using KeePass.Plugins;
using KeePass.UI;
using KeePass.Util;
using KeePassLib;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;
using RuleBuilder.Properties;
using RuleBuilder.Rule;
using RuleBuilder.Rule.Serialization;

namespace RuleBuilder.Forms {
	delegate void ChangeExpiration(DateTime newDate);

	static class EntryFormMod {
		public static void RegisterEntryForm(IPluginHost pluginHost) {
			GlobalWindowManager.WindowAdded += new EventHandler<GwmWindowEventArgs>((object _, GwmWindowEventArgs args) => {
				if (args.Form is PwEntryForm entryForm) {
					ModifyEntryForm(entryForm, pluginHost.MainWindow);
				}
			});
		}

		private static void ModifyEntryForm(PwEntryForm entryForm, MainForm mainWindow) {
			try {
				TabControl tabs = entryForm.Controls["m_tabMain"] as TabControl;
				TabPage page = tabs.TabPages[tabs.TabPages.IndexOfKey("m_tabEntry")];
				Button genPwButton = page.Controls["m_btnGenPw"] as Button;
				CheckBox expires = page.Controls["m_cbExpires"] as CheckBox;
				DateTimePicker expiresDate = page.Controls["m_dtExpireDateTime"] as DateTimePicker;
				if (expires == null || expiresDate == null) {
					throw new NullReferenceException();
				}
				Button changePassword = new Button() {
					Left = genPwButton.Left,
					Top = genPwButton.Top,
					Size = genPwButton.Size,
					TabIndex = genPwButton.TabIndex,
					UseVisualStyleBackColor = true,
				};
				_ = UIUtil.SetButtonImage(changePassword, UIUtil.CreateDropDownImage(Images.NewPassword), true);
				new ToolTip().SetToolTip(changePassword, Resources.GenerateAPassword);
				void changeExpiration(DateTime newDate) {
					expires.Checked = true;
					expiresDate.Value = newDate;
				}
				changePassword.Click += (object _1, EventArgs _2) => ShowMenu(entryForm, mainWindow, changePassword, changeExpiration);
				genPwButton.Visible = false;
				page.Controls.Add(changePassword);

			} catch (NullReferenceException) { }
		}

		private static void ShowMenu(PwEntryForm entryForm, MainForm mainWindow, Button button, ChangeExpiration changeExpiration) {
			CustomContextMenuStripEx menu = new CustomContextMenuStripEx();
			menu.Items.AddRange(MenuItems(entryForm, mainWindow, changeExpiration));
			menu.ShowEx(button);
		}

		private static ToolStripItem[] MenuItems(PwEntryForm entryForm, MainForm mainWindow, ChangeExpiration changeExpiration) {
			List<ToolStripItem> items = new List<ToolStripItem>();
			items.Add(RuleBuilderExt.MenuItem(Resources.GenerateFromRule, () => GenerateFromRule(entryForm, changeExpiration), Images.Dice));
			items.Add(RuleBuilderExt.MenuItem(Resources.EditPasswordRule, () => EditRule(entryForm, mainWindow), null));

			List<PwProfile> profiles = PwGeneratorUtil.GetAllProfiles(true);
			if (profiles.Count > 0) {
				items.Add(new ToolStripSeparator());
				bool hideBuiltIn = (Program.Config.UI.UIFlags &
			(ulong)AceUIFlags.HideBuiltInPwGenPrfInEntryDlg) != 0;
				foreach (PwProfile profile in profiles) {
					if (!hideBuiltIn || !PwGeneratorUtil.IsBuiltInProfile(profile.Name)) {
						items.Add(RuleBuilderExt.MenuItem(profile.Name, () => GenerateFromProfile(entryForm, profile), Images.Organizer));
					}
				}
			}

			items.Add(new ToolStripSeparator());
			items.Add(RuleBuilderExt.MenuItem(Resources.OpenBuiltInPasswordGenerator, () => GenerateFromProfileEditor(entryForm), Images.NewPassword));

			return items.ToArray();
		}

		private static void GenerateFromRule(PwEntryForm entryForm, ChangeExpiration changeExpiration) {
			entryForm.UpdateEntryStrings(true, false);
			Configuration config = Entry.DeserializedDefaultConfiguration(
				entryForm.EntryStrings.Get(Entry.PasswordRuleKey)?.ReadString(), entryForm.EntryRef.ParentGroup);
			entryForm.EntryStrings.Set(PwDefs.PasswordField, new ProtectedString(true, config.Generator.NewPassword()));
			if (config.Expiration != null) {
				changeExpiration(config.Expiration.DateFrom(DateTime.Today));
			}
			entryForm.UpdateEntryStrings(false, true, true);
		}

		private static void EditRule(PwEntryForm entryForm, MainForm mainWindow) {
			entryForm.UpdateEntryStrings(true, false);
			Configuration config = entryForm.EntryStrings.Exists(Entry.PasswordRuleKey)
				? Entry.DeserializedConfiguration(entryForm.EntryStrings.Get(Entry.PasswordRuleKey).ReadString())
				: null;
			if (Forms.EditRule.ShowRuleDialog(mainWindow, ref config)) {
				if (config == null || config.IsDefaultConfiguration()) {
					entryForm.EntryStrings.Remove(Entry.PasswordRuleKey);
				} else {
					entryForm.EntryStrings.Set(Entry.PasswordRuleKey, new ProtectedString(false, Entry.SerializedConfiguration(config)));
				}
				entryForm.UpdateEntryStrings(false, true, true);
			}
		}

		private static void GenerateFromProfileEditor(PwEntryForm entryForm) {
			entryForm.UpdateEntryStrings(true, false);
			PwProfile derived = PwProfile.DeriveFromPassword(entryForm.EntryStrings.GetSafe(PwDefs.PasswordField));
			PwGeneratorForm generator = new PwGeneratorForm();
			try {
				generator.InitEx(derived, true, false);
				if (generator.ShowDialog() == DialogResult.OK) {
					GenerateFromProfile(entryForm, generator.SelectedProfile);
				}
			} finally {
				UIUtil.DestroyForm(generator);
			}
		}

		private static void GenerateFromProfile(PwEntryForm entryForm, PwProfile profile) {
			byte[] entropy = EntropyForm.CollectEntropyIfEnabled(profile);
			_ = PwGenerator.Generate(out ProtectedString password, profile, entropy, Program.PwGeneratorPool);
			entryForm.EntryStrings.Set(PwDefs.PasswordField, password);
			entryForm.UpdateEntryStrings(false, true, true);
		}
	}
}
