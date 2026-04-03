using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using RuleBuilder.Properties;

namespace RuleBuilder.Util {
	internal class Hotkey {
		private Hotkey(Window window, int id) {
			this.Window = window;
			this.ID = id;
		}
		private enum Modifier {
			Alt = 0x0001,
			Ctrl = 0x0002,
			Shift = 0x0004,
			Windows = 0x0005,
			NoRepeat = 0x4000
		}
		private Window Window { get; }
		private int ID { get; }
		private static int LastID { get; set; }
		public static Hotkey RegisterHotKey(Window window, Keys keys) {
			int id = ++LastID;
			if (!NativeMethods.RegisterHotKey(new WindowInteropHelper(window).Handle, id, Modifiers(keys), (uint)(keys & Keys.KeyCode))) {
				throw new HotKeyException(Resources.UnableToRegisterHotkey);
			}
			return new Hotkey(window, id);
		}
		public bool MatchesID(int id) => this.ID == id;
		public void Unregister() {
			if (!NativeMethods.UnregisterHotKey(new WindowInteropHelper(this.Window).Handle, this.ID)) {
				throw new HotKeyException(Resources.UnableToUnregisterHotkey);
			}
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
