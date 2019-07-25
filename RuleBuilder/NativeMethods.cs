using System;
using System.Runtime.InteropServices;

namespace RuleBuilder {
	internal class NativeMethods {
		[DllImport("user32")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
		[DllImport("user32")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		[DllImport("user32")]
		public static extern short GetKeyState(int nVertKey);
	}
}
