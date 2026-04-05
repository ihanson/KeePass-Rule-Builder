using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using KeePass.Util;
using KeePassLib;

namespace RuleBuilder.Forms {
	/// <summary>
	/// Interaction logic for DiscardedPasswords.xaml
	/// </summary>
	public partial class DiscardedPasswords : Window {
		private DiscardedPasswords(IEnumerable<GeneratedPassword> passwords) {
			InitializeComponent();
			this.dgPasswords.ItemsSource = passwords;
			this.CopyPassword = new CopyPasswordCommand(this);
		}

		public static void ShowDiscardedPasswordDialog(IEnumerable<GeneratedPassword> passwords) {
			new DiscardedPasswords(passwords).ShowDialog();
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
				ClipboardUtil.Copy(password.Password, false, false, null, null, new WindowInteropHelper(this.Window).Handle);
			}
		}
	}

	public class GeneratedPassword {
		public GeneratedPassword(string entryName, string password, DateTime generatedTime, PwDatabase sourceDatabase) {
			this.Password = password;
			this.EntryName = entryName;
			this.GeneratedTime = generatedTime;
			this.SourceDatabase = sourceDatabase;
		}
		public PwDatabase SourceDatabase { get; }
		public string EntryName { get; }
		public string Password { get; }
		public DateTime GeneratedTime { get; }
	}
}
