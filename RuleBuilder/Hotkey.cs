using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RuleBuilder {
	internal static class HotKey {
		[DllImport("user32")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
		[DllImport("user32")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		private static int ID { get; set; } = 0;
		public static int RegisterHotKey(Form form, Keys keys) {
			if (!RegisterHotKey(form.Handle, ++ID, Modifiers(keys), (uint)(keys & Keys.KeyCode))) {
				throw new HotKeyException("Unable to register hotkey.");
			}
			return ID;
		}
		public static void UnregisterHotKey(Form form, int id) {
			_ = UnregisterHotKey(form.Handle, id);
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
		private enum Modifier {
			Alt = 0x0001,
			Ctrl = 0x0002,
			Shift = 0x0004,
			Windows = 0x0005,
			NoRepeat = 0x4000
		}
	}
	internal class HotKeyException: Exception {
		public HotKeyException(string message) : base(message) { }
	}
}
