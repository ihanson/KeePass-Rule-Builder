using System;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using KeePass.App;
using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Security;
using RuleBuilder.Rule;
using RuleBuilder.Util;

namespace RuleBuilder.Forms {
	delegate void SetKeyCombination(KeyCombination combo);
	public partial class ChangePassword : Window {
		private const string KeyCombinationsConfigKey = "KeePassRuleBuilder.KeyCombinations";
		private const int HotKeyMessage = 0x312;
		private const short ShiftKey = 0x10;

		private ChangePassword(IPluginHost host, KeePass.Forms.MainForm mainForm, PwDatabase database, PwEntry entry) {
			this.InitializeComponent();
			this.Host = host;
			this.MainForm = mainForm;
			this.Database = database;
			this.Entry = entry;
			this.LoadSettings();
			new WindowInteropHelper(this).Owner = mainForm.Handle;
			this.Title = $"{Properties.Resources.ChangePassword}: {entry.Strings.Get(PwDefs.TitleField)?.ReadString() ?? string.Empty}";
			this.Configuration = Rule.Serialization.Entry.EntryDefaultConfiguration(entry);
			this.txtOldPassword.Text = entry.Strings.Get(PwDefs.PasswordField)?.ReadString() ?? string.Empty;
			this.txtNewPassword.Text = this.Configuration.Generator.NewPassword();
			this.SetExpiration();
		}

		private IPluginHost Host { get; }

		private KeePass.Forms.MainForm MainForm { get; }

		private PwDatabase Database { get; }

		private PwEntry Entry { get; }

		private Hotkey OldPasswordHotkey { get; set; }

		private Hotkey NewPasswordHotkey { get; set; }

		private KeyCombination OldPasswordHotkeyCombo { get; set; }

		private KeyCombination NewPasswordHotkeyCombo { get; set; }

		private bool EntryChanged { get; set; }

		private Configuration Configuration { get; set; }

		private bool RuleChanged { get; set; }

		private bool SettingsChanged { get; set; }

		private HwndSource Source { get; set; }

		public static bool ShowChangePasswordDialog(KeePass.Plugins.IPluginHost host, KeePass.Forms.MainForm mainForm, PwEntry entry) {
			if (host == null) {
				throw new ArgumentNullException(nameof(host));
			}
			if (mainForm == null) {
				throw new ArgumentNullException(nameof(mainForm));
			}
			if (entry == null) {
				throw new ArgumentNullException(nameof(entry));
			}
			ChangePassword window = new ChangePassword(host, mainForm, mainForm.ActiveDatabase, entry);
			_ = window.ShowDialog();
			return window.EntryChanged;
		}

		private IntPtr HwndHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
			if (msg == HotKeyMessage) {
				WaitForKeyRelease();
				int hotKeyID = wParam.ToInt32();
				if (this.OldPasswordHotkey?.MatchesID(hotKeyID) ?? false) {
					_ = KeePass.Util.AutoType.PerformIntoCurrentWindow(this.Entry, this.Database, EscapeAutoType(this.txtOldPassword.Text));
					handled = true;
				} else if (this.NewPasswordHotkey?.MatchesID(hotKeyID) ?? false) {
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

		private void SaveClicked(object sender, RoutedEventArgs e) {
			string oldPassword = this.Entry.Strings.Get(PwDefs.PasswordField).ReadString();
			string newPassword = this.txtNewPassword.Text;
			bool passwordChanged = oldPassword != newPassword;
			if (passwordChanged || this.RuleChanged) {
				if (passwordChanged) {
					this.Entry.CreateBackup(this.Database);
					this.Entry.Strings.Set(PwDefs.PasswordField, new ProtectedString(true, newPassword));
					if (this.chkExpiration.IsChecked == true && this.dateExpiration.SelectedDate != null) {
						this.Entry.Expires = true;
						this.Entry.ExpiryTime = TimeZoneInfo.ConvertTimeToUtc(this.dateExpiration.SelectedDate.Value);
					} else {
						this.Entry.Expires = false;
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

		private (KeyCombination, KeyCombination) ReadSettings() {
			string config = this.Host.CustomConfig.GetString(KeyCombinationsConfigKey);
			if (config == null) {
				return (null, null);
			}
			string[] pieces = config.Split(',');
			try {
				return (
					new KeyCombination(
						(ModifierKeys)int.Parse(pieces[0]),
						(Key)int.Parse(pieces[1])
					),
					new KeyCombination(
						(ModifierKeys)int.Parse(pieces[2]),
						(Key)int.Parse(pieces[3])
					)
				);
			} catch (Exception) {
				return (null, null);
			}
		}

		private void LoadSettings() {
			(KeyCombination oldCombo, KeyCombination newCombo) = ReadSettings();
			this.OldPasswordHotkeyCombo = oldCombo ?? new KeyCombination(
				ModifierKeys.Control | ModifierKeys.Shift,
				Key.Z
			);
			this.NewPasswordHotkeyCombo = newCombo ?? new KeyCombination(
				ModifierKeys.Control | ModifierKeys.Shift,
				Key.X
			);
		}

		private void SaveSettings() {
			if (this.SettingsChanged) {
				this.Host.CustomConfig.SetString(
					KeyCombinationsConfigKey,
					string.Join(",", new[]{
						(int)this.OldPasswordHotkeyCombo.Modifiers,
						(int)this.OldPasswordHotkeyCombo.Key,
						(int)this.NewPasswordHotkeyCombo.Modifiers,
						(int)this.NewPasswordHotkeyCombo.Key
					})
				);
			}
		}

		private void WindowLoaded(object sender, RoutedEventArgs e) {
			if (!(
				AppPolicy.Current.AutoType
				&& AppPolicy.Current.AutoTypeWithoutContext
			)) {
				lblAutoTypeDisabled.Text = Properties.Resources.AutoTypeDisabledInPolicy;
				pnlAutoTypeOld.Visibility = Visibility.Collapsed;
				pnlAutoTypeNew.Visibility = Visibility.Collapsed;
			} else if (!this.Entry.GetAutoTypeEnabled()) {
				lblAutoTypeDisabled.Text = Properties.Resources.AutoTypeDisabledInEntry;
				pnlAutoTypeOld.Visibility = Visibility.Collapsed;
				pnlAutoTypeNew.Visibility = Visibility.Collapsed;
			} else {
				this.Source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
				this.Source.AddHook(this.HwndHook);
				this.TryRegisterHotkeys();
			}
			this.MinHeight = this.Height;
			this.MaxHeight = this.Height;
		}

		private void TryRegisterHotkeys() {
			this.UnregisterHotkeys();
			try {
				this.OldPasswordHotkey = Hotkey.RegisterHotKey(this, this.OldPasswordHotkeyCombo.ToKeys());
			} catch (HotKeyException) { }
			try {
				this.NewPasswordHotkey = Hotkey.RegisterHotKey(this, this.NewPasswordHotkeyCombo.ToKeys());
			} catch (HotKeyException) { }
			this.lblAutoTypeOld.Text = this.OldPasswordHotkeyCombo.ToString();
			this.lblAutoTypeNew.Text = this.NewPasswordHotkeyCombo.ToString();
			SetStrikethrough(this.lblAutoTypeOld, this.OldPasswordHotkey == null);
			SetStrikethrough(this.lblAutoTypeNew, this.NewPasswordHotkey == null);
		}

		private void UnregisterHotkeys() {
			this.OldPasswordHotkey?.Unregister();
			this.NewPasswordHotkey?.Unregister();
			this.OldPasswordHotkey = null;
			this.NewPasswordHotkey = null;
		}

		private void WindowClosing(object sender, EventArgs e) {
			this.UnregisterHotkeys();
			this.SaveSettings();
		}

		private void SetExpiration() {
			if (this.Configuration.Expiration != null) {
				this.chkExpiration.IsChecked = true;
				this.dateExpiration.IsEnabled = true;
				this.dateExpiration.SelectedDate = this.Configuration.Expiration.DateFrom(DateTime.Today);
			} else if (this.Entry.Expires) {
				this.chkExpiration.IsChecked = true;
				this.dateExpiration.IsEnabled = true;
				this.dateExpiration.SelectedDate = this.Entry.ExpiryTime.ToLocalTime();
			} else {
				this.chkExpiration.IsChecked = false;
				this.dateExpiration.IsEnabled = false;
				this.dateExpiration.SelectedDate = DateTime.Today;
			}
		}

		private void ExpirationClicked(object sender, RoutedEventArgs e) {
			this.dateExpiration.IsEnabled = this.chkExpiration.IsChecked ?? false;
		}

		private void ExpirationDateChanged(object sender, SelectionChangedEventArgs e) {
			if (this.dateExpiration.SelectedDate == null) {
				this.dateExpiration.SelectedDate = DateTime.Today;
			}
		}

		private void StartHotkeyEdit(TextBlock hotkeyLabel, Button configButton, SetKeyCombination setter) {
			Brush oldBack = hotkeyLabel.Background;
			Brush oldFore = hotkeyLabel.Foreground;
			Brush neutralBack = new SolidColorBrush(Color.FromRgb(0xff, 0xff, 0x00));
			Brush neutralFore = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
			Brush errorBack = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
			Brush errorFore = new SolidColorBrush(Color.FromRgb(0xff, 0xff, 0xff));
			KeyCombination combo = null;
			void lostFocusHandler(object sender, EventArgs e) {
				restore(true);
			}
			void keyPressHandler(object sender, System.Windows.Input.KeyEventArgs e) {
				if (e.Key == Key.Tab) {
					return;
				}
				e.Handled = true;
				if (e.Key == Key.Enter || e.Key == Key.Return) {
					restore(true);
				} else if (e.Key == Key.Escape) {
					restore(false);
				} else {
					combo = new KeyCombination(e.KeyboardDevice.Modifiers, e.Key);
					hotkeyLabel.Text = combo.ToString();
					(hotkeyLabel.Background, hotkeyLabel.Foreground) = combo.IsValidHotkey()
						? (neutralBack, neutralFore)
						: (errorBack, errorFore);
				}
			}
			void restore(bool changeCombo) {
				if (changeCombo && combo != null && combo.IsValidHotkey()) {
					setter(combo);
					this.SettingsChanged = true;
				}
				configButton.Visibility = Visibility.Visible;
				hotkeyLabel.Background = oldBack;
				hotkeyLabel.Foreground = oldFore;
				hotkeyLabel.Focusable = false;
				hotkeyLabel.KeyDown -= keyPressHandler;
				hotkeyLabel.LostFocus -= lostFocusHandler;
				this.Deactivated -= lostFocusHandler;
				configButton.Focus();
				this.TryRegisterHotkeys();
			}
			this.UnregisterHotkeys();
			SetStrikethrough(hotkeyLabel, false);
			configButton.Visibility = Visibility.Collapsed;
			hotkeyLabel.Background = neutralBack;
			hotkeyLabel.Foreground = neutralFore;
			hotkeyLabel.Focusable = true;
			hotkeyLabel.KeyDown += keyPressHandler;
			this.Deactivated += lostFocusHandler;
			hotkeyLabel.LostFocus += lostFocusHandler;
			hotkeyLabel.Focus();
		}

		private void ConfOldHotkeyClicked(object sender, RoutedEventArgs e) =>
			this.StartHotkeyEdit(
				this.lblAutoTypeOld,
				(Button)sender,
				(combo) => this.OldPasswordHotkeyCombo = combo
			);

		private void ConfNewHotkeyClicked(object sender, RoutedEventArgs e) =>
			this.StartHotkeyEdit(
				this.lblAutoTypeNew,
				(Button)sender,
				(combo) => this.NewPasswordHotkeyCombo = combo
			);

		private static void SetStrikethrough(TextBlock text, bool strike) {
			text.TextDecorations.Clear();
			if (strike) {
				text.TextDecorations.Add(new TextDecoration() {
					Location = TextDecorationLocation.Strikethrough
				});
			}
		}
	}
}
