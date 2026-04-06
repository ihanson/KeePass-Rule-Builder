using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Keys = System.Windows.Forms.Keys;

namespace RuleBuilder.Util {
	public class KeyCombination {
		public readonly ModifierKeys Modifiers;
		public readonly Key Key;

		public KeyCombination(ModifierKeys modifiers, Key key) {
			this.Modifiers = modifiers;
			this.Key = key;
		}

		public bool IsValidHotkey() =>
			this.IsFunctionKey() || (
				(this.IsCtrlDown() || this.IsAltDown()) && (
					this.IsMoveKey()
					|| this.IsDigitKey()
					|| this.IsLetterKey()
					|| this.IsNumPadDigitKey()
					|| this.IsNumPadFunctionKey()
					|| this.IsOemGroup1Key()
					|| this.IsOemGroup2Key()
				)
			);

		public Keys ToKeys() => this.TranslateModifier() | this.TranslateKey();

		private Keys TranslateModifier() => Keys.None
				| (this.IsCtrlDown() ? Keys.Control : Keys.None)
				| (this.IsAltDown() ? Keys.Alt : Keys.None)
				| (this.IsShiftDown() ? Keys.Shift : Keys.None);

		private Keys TranslateKey() {
			if (this.IsMoveKey()) {
				return TranslateRange(Key.PageUp, Keys.PageUp, this.Key);
			}
			if (this.IsDigitKey()) {
				return TranslateRange(Key.D0, Keys.D0, this.Key);
			}
			if (this.IsLetterKey()) {
				return TranslateRange(Key.A, Keys.A, this.Key);
			}
			if (this.IsNumPadDigitKey()) {
				return TranslateRange(Key.NumPad0, Keys.NumPad0, this.Key);
			}
			if (this.IsNumPadFunctionKey()) {
				return TranslateRange(Key.Multiply, Keys.Multiply, this.Key);
			}
			if (this.IsFunctionKey()) {
				return TranslateRange(Key.F1, Keys.F1, this.Key);
			}
			if (this.IsOemGroup1Key()) {
				return TranslateRange(Key.Oem1, Keys.Oem1, this.Key);
			}
			if (this.IsOemGroup2Key()) {
				return TranslateRange(Key.Oem4, Keys.Oem4, this.Key);
			}
			throw new UnmappedKeyCombinationException(this.Key);
		}

		public override string ToString() =>
			string.Join(
				"+",
				new []{
					(ModifierKeys.Control, "Ctrl"),
					(ModifierKeys.Alt, "Alt"),
					(ModifierKeys.Shift, "Shift")
				}.Where((tuple) => (this.Modifiers & tuple.Item1) != 0)
				.Select((tuple) => tuple.Item2)
				.Append(this.KeyName())
			);

		private string KeyName() {
			if (this.IsDigitKey()) {
				return TranslateRange(Key.D0, 0, this.Key).ToString();
			}
			if (this.IsLetterKey()) {
				return TranslateRange(Key.A, 'A', this.Key).ToString();
			}
			if (this.IsNumPadDigitKey()) {
				return string.Format("NumPad{0}", TranslateRange(Key.NumPad0, 0, this.Key));
			}
			if (this.IsFunctionKey()) {
				return string.Format("F{0}", TranslateRange(Key.F1, 1, this.Key));
			}
			if (this.IsOemGroup2Key()) {
				return string.Format("OEM{0}", TranslateRange(Key.Oem4, 4, this.Key));
			}
			switch (this.Key) {
				case Key.PageUp: return "PageUp";
				case Key.PageDown: return "PageDown";
				case Key.End: return "End";
				case Key.Home: return "Home";
				case Key.Left: return "Left";
				case Key.Up: return "Up";
				case Key.Right: return "Right";
				case Key.Down: return "Down";
				case Key.Multiply: return "Multiply";
				case Key.Add: return "Add";
				case Key.Separator: return "Separator";
				case Key.Subtract: return "Subtract";
				case Key.Decimal: return "Decimal";
				case Key.Divide: return "Divide";
				case Key.Oem1: return "OEM1";
				case Key.OemPlus: return "Plus";
				case Key.OemComma: return "Comma";
				case Key.OemMinus: return "Minus";
				case Key.OemPeriod: return "Period";
				case Key.Oem2: return "OEM2";
				case Key.Oem3: return "OEM3";
				default: return string.Empty;
			}
		}

		private bool IsCtrlDown() => (this.Modifiers & ModifierKeys.Control) != 0;
		private bool IsAltDown() => (this.Modifiers & ModifierKeys.Alt) != 0;
		private bool IsShiftDown() => (this.Modifiers & ModifierKeys.Shift) != 0;
		private bool IsMoveKey() => IsInRange(Key.PageUp, Key.Down, this.Key);
		private bool IsDigitKey() => IsInRange(Key.D0, Key.D9, this.Key);
		private bool IsLetterKey() => IsInRange(Key.A, Key.Z, this.Key);
		private bool IsNumPadDigitKey() => IsInRange(Key.NumPad0, Key.NumPad9, this.Key);
		private bool IsNumPadFunctionKey() => IsInRange(Key.Multiply, Key.Divide, this.Key);
		private bool IsFunctionKey() => IsInRange(Key.F1, Key.F24, this.Key);
		private bool IsOemGroup1Key() => IsInRange(Key.Oem1, Key.Oem3, this.Key);
		private bool IsOemGroup2Key() => IsInRange(Key.Oem4, Key.Oem8, this.Key);

		private static bool IsInRange(Key rangeMin, Key rangeMax, Key key) =>
			rangeMin <= key && key <= rangeMax;

		private static Keys TranslateRange(Key keyRangeStart, Keys keysRangeStart, Key key) =>
			key - keyRangeStart + keysRangeStart;
		private static char TranslateRange(Key keyRangeStart, char charRangeStart, Key key) =>
			(char)(key - keyRangeStart + charRangeStart);
		private static int TranslateRange(Key keyRangeStart, int intRangeStart, Key key) =>
			key - keyRangeStart + intRangeStart;
	}

	internal class UnmappedKeyCombinationException : ApplicationException {
		public readonly Key Key;

		public UnmappedKeyCombinationException(Key key) {
			this.Key = key;
		}
	}
}
