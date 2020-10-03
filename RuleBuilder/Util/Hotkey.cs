using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using RuleBuilder.Properties;

namespace RuleBuilder.Util {
	internal static class HotKey {
		private enum Modifier {
			Alt = 0x0001,
			Ctrl = 0x0002,
			Shift = 0x0004,
			Windows = 0x0005,
			NoRepeat = 0x4000
		}
		private static int ID { get; set; }
		public static int RegisterHotKey(Window window, Keys keys) {
			if (!NativeMethods.RegisterHotKey(new WindowInteropHelper(window).Handle, ++ID, Modifiers(keys), (uint)(keys & Keys.KeyCode))) {
				throw new HotKeyException(Resources.UnableToRegisterHotkey);
			}
			return ID;
		}
		public static void UnregisterHotKey(Window window, int id) {
			NativeMethods.UnregisterHotKey(new WindowInteropHelper(window).Handle, id);
		}
		private static uint Modifiers(Keys keys) {
			uint result = 0;
			if ((keys & Keys.Alt) != Keys.None) {
				result |= (uint)Modifier.Alt;
			}
			if ((keys & Keys.Control) != Keys.None) {
				result |= (uint)Modifier.Ctrl;
			}
			if ((keys & Keys.Shift) != Keys.None) {
				result |= (uint)Modifier.Shift;
			}
			return result;
		}
	}
	[Serializable]
	public class HotKeyException : Exception {
		public HotKeyException() : base() { }

		public HotKeyException(string message) : base(message) { }

		public HotKeyException(string message, Exception innerException) : base(message, innerException) { }

		protected HotKeyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
