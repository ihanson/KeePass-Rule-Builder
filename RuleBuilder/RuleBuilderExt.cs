using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Security;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
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
			Rule.IPasswordGenerator generator;
			try {
				ProtectedString generatorStr = entry.Strings.Get(PasswordRule);
				generator = generatorStr != null ? DeserializedGenerator(generatorStr.ReadString()) : Rule.PasswordProfile.DefaultProfile;
			} catch (Exception e) {
				_ = MessageBox.Show(e.Message, "Error loading password rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
				generator = Rule.PasswordProfile.DefaultProfile;
			}
			Forms.ChangePassword form = new Forms.ChangePassword(oldPassword, generator);
			_ = form.ShowDialog();
			if (form.SavedChange) {
				if (form.Generator is Rule.PasswordProfile && ((Rule.PasswordProfile)(form.Generator)).IsDefaultProfile) {
					entry.Strings.Remove(PasswordRule);
				} else {
					entry.Strings.Set(PasswordRule, new ProtectedString(false, SerializedGenerator(form.Generator)));
				}
				if (oldPassword != form.Password) {
					entry.CreateBackup(this.host.MainWindow.ActiveDatabase);
					entry.Strings.Set(PwDefs.PasswordField, new ProtectedString(true, form.Password));
					entry.Touch(true);
				}
			}
			this.host.MainWindow.UpdateUI(false, null, false, null, false, null, true);
		}

		private static Rule.IPasswordGenerator DeserializedGenerator(string generatorStr) {
			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(false)) {
				AutoFlush = true
			}) {
				writer.Write(generatorStr);
				stream.Position = 0;
				return ((Rule.Serialization.ConfigurationContract)new DataContractJsonSerializer(typeof(Rule.Serialization.ConfigurationContract)).ReadObject(stream)).Object();
			}
		}
		private static string SerializedGenerator(Rule.IPasswordGenerator generator) {
			using (MemoryStream stream = new MemoryStream())
			using (StreamReader reader = new StreamReader(stream, Encoding.UTF8)) {
				new DataContractJsonSerializer(typeof(Rule.Serialization.ConfigurationContract)).WriteObject(stream, new Rule.Serialization.ConfigurationContract(generator));
				stream.Position = 0;
				return reader.ReadToEnd();
			}
		}

		private IPluginHost host;
		private const string PasswordRule = "Password Rule";
	}
}
