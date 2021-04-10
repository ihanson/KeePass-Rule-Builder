using KeePass.App;
using KeePassLib;
using KeePassLib.Security;
using RuleBuilder.Util;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace RuleBuilder.Forms {
	public partial class ChangePassword : Window {
		private const int HotKeyMessage = 0x312;
		private const short ShiftKey = 0x10;

		private ChangePassword(KeePass.Forms.MainForm mainForm, PwDatabase database, PwEntry entry) {
			this.InitializeComponent();
			this.MainForm = mainForm;
			this.Database = database;
			this.Entry = entry;
			new WindowInteropHelper(this).Owner = mainForm.Handle;
			this.Title = $"{Properties.Resources.ChangePassword}: {entry.Strings.Get(PwDefs.TitleField)?.ReadString() ?? string.Empty}";
			this.Generator = Rule.Serialization.Entry.EntryDefaultGenerator(entry);
			this.txtOldPassword.Text = entry.Strings.Get(PwDefs.PasswordField)?.ReadString() ?? string.Empty;
			this.txtNewPassword.Text = this.Generator.NewPassword();
		}

		private KeePass.Forms.MainForm MainForm { get; }

		private PwDatabase Database { get; }

		private PwEntry Entry { get; }

		private int OldPasswordHotKeyID { get; set; }

		private int NewPasswordHotKeyID { get; set; }

		private bool EntryChanged { get; set; }

		private Rule.IPasswordGenerator Generator { get; set; }

		private bool RuleChanged { get; set; }

		private HwndSource Source { get; set; }

		public static bool ShowChangePasswordDialog(KeePass.Forms.MainForm mainForm, PwEntry entry) {
			if (mainForm == null) {
				throw new ArgumentNullException(nameof(mainForm));
			}
			if (entry == null) {
				throw new ArgumentNullException(nameof(entry));
			}
			ChangePassword window = new ChangePassword(mainForm, mainForm.ActiveDatabase, entry);
			_ = window.ShowDialog();
			return window.EntryChanged;
		}

		private IntPtr HwndHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
			if (msg == HotKeyMessage) {
				WaitForKeyRelease();
				int hotKeyID = wParam.ToInt32();
				if (hotKeyID == this.OldPasswordHotKeyID) {
					_ = KeePass.Util.AutoType.PerformIntoCurrentWindow(this.Entry, this.Database, EscapeAutoType(this.txtOldPassword.Text));
					handled = true;
				} else if (hotKeyID == this.NewPasswordHotKeyID) {
					_ = KeePass.Util.AutoType.PerformIntoCurrentWindow(this.Entry, this.Database, EscapeAutoType(this.txtNewPassword.Text));
					handled = true;
				}
			}
			return IntPtr.Zero;
		}

		private static void WaitForKeyRelease() {
			DateTime start = DateTime.UtcNow;
			while (ShiftKeyDown()) {
				if (DateTime.UtcNow.Ticks - start.Ticks > 2e7) {
					break;
				}
			}
		}

		private static string EscapeAutoType(string text) => Regex.Replace(text, @"[+%^~()[\]{}]", (Match match) => $"{{{match.Value}}}");

		private static bool ShiftKeyDown() => (NativeMethods.GetKeyState(ShiftKey) & 0x80) != 0;

		private void AcceptClicked(object sender, RoutedEventArgs e) {
			string oldPassword = this.Entry.Strings.Get(PwDefs.PasswordField).ReadString();
			string newPassword = this.txtNewPassword.Text;
			bool passwordChanged = oldPassword != newPassword;
			if (passwordChanged || this.RuleChanged) {
				if (passwordChanged) {
					this.Entry.CreateBackup(this.Database);
				}
				this.Entry.Strings.Set(PwDefs.PasswordField, new ProtectedString(true, newPassword));
				if (this.RuleChanged) {
					Rule.Serialization.Entry.SetEntryGenerator(this.Entry, this.Generator);
				}
				this.Entry.Touch(true);
				this.EntryChanged = true;
			}
			this.DialogResult = true;
		}

		private void EditRuleClicked(object sender, RoutedEventArgs e) {
			Rule.IPasswordGenerator generator = this.Generator;
			if (EditRule.ShowRuleDialog(this.MainForm, ref generator)) {
				this.RuleChanged = true;
				this.Generator = generator;
				this.txtNewPassword.Text = this.Generator.NewPassword();
			}
		}

		private void RefreshClicked(object sender, RoutedEventArgs e) {
			this.txtNewPassword.Text = this.Generator.NewPassword();
		}

		private void WindowLoaded(object sender, RoutedEventArgs e) {
			this.Source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
			this.Source.AddHook(this.HwndHook);
			if (this.Entry.GetAutoTypeEnabled() && AppPolicy.Try(AppPolicyId.AutoTypeWithoutContext)) {
				try {
					this.OldPasswordHotKeyID = HotKey.RegisterHotKey(this, Keys.Z | Keys.Control | Keys.Shift);
					this.lblAutoTypeOld.Text = $"{Properties.Resources.AutoType}: Ctrl+Shift+Z";
				} catch (HotKeyException ex) {
					_ = ex;
				}
				try {
					this.NewPasswordHotKeyID = HotKey.RegisterHotKey(this, Keys.X | Keys.Control | Keys.Shift);
					this.lblAutoTypeNew.Text = $"{Properties.Resources.AutoType}: Ctrl+Shift+X";
				} catch (HotKeyException ex) {
					_ = ex;
				}
			}
			this.MinHeight = this.Height;
			this.MaxHeight = this.Height;
		}

		private void WindowClosed(object sender, EventArgs e) {
			HotKey.UnregisterHotKey(this, this.OldPasswordHotKeyID);
			HotKey.UnregisterHotKey(this, this.NewPasswordHotKeyID);
		}
	}
}
