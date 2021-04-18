using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using KeePass.App;
using KeePassLib;
using KeePassLib.Security;
using RuleBuilder.Rule;
using RuleBuilder.Util;

namespace RuleBuilder.Forms {
	public partial class ChangePassword : Window {
		private const int HotKeyMessage = 0x312;
		private const short ShiftKey = 0x10;

		private ChangePassword(KeePass.Forms.MainForm mainForm, PwDatabase database, PwEntry entry, bool showExpiration) {
			this.InitializeComponent();
			this.MainForm = mainForm;
			this.Database = database;
			this.Entry = entry;
			this.ShowExpiration = showExpiration;
			new WindowInteropHelper(this).Owner = mainForm.Handle;
			this.Title = $"{Properties.Resources.ChangePassword}: {entry.Strings.Get(PwDefs.TitleField)?.ReadString() ?? string.Empty}";
			this.Configuration = Rule.Serialization.Entry.EntryDefaultConfiguration(entry);
			this.txtOldPassword.Text = entry.Strings.Get(PwDefs.PasswordField)?.ReadString() ?? string.Empty;
			this.txtNewPassword.Text = this.Configuration.Generator.NewPassword();
			this.pnlExpiration.Visibility = this.ShowExpiration ? Visibility.Visible : Visibility.Collapsed;
			this.SetExpiration();
		}

		private KeePass.Forms.MainForm MainForm { get; }

		private PwDatabase Database { get; }

		private PwEntry Entry { get; }

		private bool ShowExpiration { get; }

		private int OldPasswordHotKeyID { get; set; }

		private int NewPasswordHotKeyID { get; set; }

		private bool EntryChanged { get; set; }

		private Configuration Configuration { get; set; }

		private bool RuleChanged { get; set; }

		private HwndSource Source { get; set; }

		public static bool ShowChangePasswordDialog(KeePass.Forms.MainForm mainForm, PwEntry entry, bool showExpiration) {
			if (mainForm == null) {
				throw new ArgumentNullException(nameof(mainForm));
			}
			if (entry == null) {
				throw new ArgumentNullException(nameof(entry));
			}
			ChangePassword window = new ChangePassword(mainForm, mainForm.ActiveDatabase, entry, showExpiration);
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
					this.Entry.Strings.Set(PwDefs.PasswordField, new ProtectedString(true, newPassword));
					if (this.ShowExpiration) {
						if (this.chkExpiration.IsChecked == true && this.dateExpiration.SelectedDate != null) {
							this.Entry.Expires = true;
							this.Entry.ExpiryTime = this.dateExpiration.SelectedDate.Value;
						} else {
							this.Entry.Expires = false;
						}
					}
				}
				if (this.RuleChanged) {
					Rule.Serialization.Entry.SetEntryConfiguration(this.Entry, this.Configuration);
				}
				this.Entry.Touch(true);
				this.EntryChanged = true;
			}
			this.DialogResult = true;
		}

		private void EditRuleClicked(object sender, RoutedEventArgs e) {
			Configuration config = this.Configuration;
			if (EditRule.ShowRuleDialog(this.MainForm, ref config)) {
				this.RuleChanged = true;
				this.Configuration = config;
				this.txtNewPassword.Text = this.Configuration.Generator.NewPassword();
				this.SetExpiration();
			}
		}

		private void RefreshClicked(object sender, RoutedEventArgs e) {
			this.txtNewPassword.Text = this.Configuration.Generator.NewPassword();
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

		private void SetExpiration() {
			if (this.Configuration.Expiration != null) {
				this.chkExpiration.IsChecked = true;
				this.dateExpiration.IsEnabled = true;
				this.dateExpiration.SelectedDate = this.Configuration.Expiration.DateFrom(DateTime.Today);
			} else {
				this.chkExpiration.IsChecked = false;
				this.dateExpiration.IsEnabled = false;
				this.dateExpiration.SelectedDate = DateTime.Today;
			}
		}

		private void ExpirationClicked(object sender, RoutedEventArgs e) {
			this.dateExpiration.IsEnabled = this.chkExpiration.IsChecked ?? false;
		}

		private void ExpirationDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
			if (this.dateExpiration.SelectedDate == null) {
				this.dateExpiration.SelectedDate = DateTime.Today;
			}
		}
	}
}
