using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using KeePass.App;
using KeePass.Util;
using KeePassLib;

namespace RuleBuilder.Forms {
	/// <summary>
	/// Interaction logic for DiscardedPasswords.xaml
	/// </summary>
	public partial class DiscardedPasswords : Window {
		private DiscardedPasswords(KeePass.Forms.MainForm mainForm, IEnumerable<GeneratedPassword> passwords) {
			InitializeComponent();
			new WindowInteropHelper(this).Owner = mainForm.Handle;
			this.dgPasswords.ItemsSource = passwords;
			this.CopyPassword = new CopyPasswordCommand(this);
			if (!AppPolicy.Current.CopyToClipboard) {
				DataGridColumn copyColumn = this.dgPasswords.Columns[this.dgPasswords.Columns.Count - 1];
				copyColumn.Visibility = Visibility.Collapsed;
			}
		}

		public static void ShowDiscardedPasswordDialog(KeePass.Forms.MainForm mainForm, IEnumerable<GeneratedPassword> passwords) {
			new DiscardedPasswords(mainForm, passwords).ShowDialog();
		}

		public ICommand CopyPassword { get; }

		private void CloseClicked(object sender, RoutedEventArgs e) {
			this.Close();
		}
	}

	public class TimeZoneConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			((DateTime)value).ToLocalTime();
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			((DateTime)value).ToUniversalTime();
	}

	public class EntryConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			(value as PwEntry)?.Strings.Get(PwDefs.TitleField).ReadString() ?? string.Empty;
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			null;
	}

	public class PasswordConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			string password = value as string ?? string.Empty;
			return AppPolicy.Current.UnhidePasswords
				? password
				: new string('\u25CF', password.Length);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			null;
	}

	class CopyPasswordCommand : ICommand {
		public CopyPasswordCommand(DiscardedPasswords window) {
			this.Window = window;
		}
		public event EventHandler CanExecuteChanged { add { } remove { } }

		public bool CanExecute(object parameter) => true;

		private readonly DiscardedPasswords Window;

		public void Execute(object parameter) {
			GeneratedPassword password = parameter as GeneratedPassword;
			if (!string.IsNullOrEmpty(password?.Password)) {
				ClipboardUtil.Copy(password.Password, false, true, password.Entry, null, new WindowInteropHelper(this.Window).Handle);
			}
		}
	}

	public class GeneratedPassword {
		public GeneratedPassword(PwEntry entry, string password, DateTime generatedTime, PwDatabase sourceDatabase) {
			this.Password = password;
			this.Entry = entry;
			this.GeneratedTime = generatedTime;
			this.SourceDatabase = sourceDatabase;
		}
		public PwDatabase SourceDatabase { get; }
		public PwEntry Entry{ get; }
		public string Password { get; }
		public DateTime GeneratedTime { get; }
	}
}
