using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KC = RuleBuilder.Util.KeyCombination;
using MK = System.Windows.Input.ModifierKeys;

namespace RuleBuilderTests.Util {
	[TestClass]
	public class KeyCombinationTests {
		[TestMethod]
		public void IsValidHotkeyTest() {
			Assert.IsTrue(new KC(MK.None, Key.F11).IsValidHotkey());
			Assert.IsFalse(new KC(MK.None, Key.Down).IsValidHotkey());
			Assert.IsTrue(new KC(MK.Control, Key.Down).IsValidHotkey());
			Assert.IsTrue(new KC(MK.Alt, Key.D4).IsValidHotkey());
			Assert.IsFalse(new KC(MK.Shift, Key.D4).IsValidHotkey());
			Assert.IsTrue(new KC(MK.Alt | MK.Control, Key.G).IsValidHotkey());
			Assert.IsTrue(new KC(MK.Alt | MK.Shift, Key.NumPad0).IsValidHotkey());
			Assert.IsTrue(new KC(MK.Control | MK.Shift, Key.Add).IsValidHotkey());
			Assert.IsTrue(new KC(MK.Alt | MK.Control | MK.Shift, Key.OemComma).IsValidHotkey());
			Assert.IsTrue(new KC(MK.Control, Key.Oem7).IsValidHotkey());
		}

		[TestMethod]
		public void ToKeysTest() {
			Assert.AreEqual(new KC(MK.None, Key.F11).ToKeys(), Keys.F11);
			Assert.AreEqual(new KC(MK.Control, Key.Down).ToKeys(), Keys.Control | Keys.Down);
			Assert.AreEqual(new KC(MK.Alt, Key.D4).ToKeys(), Keys.Alt | Keys.D4);
			Assert.AreEqual(new KC(MK.Alt | MK.Control, Key.G).ToKeys(), Keys.Alt | Keys.Control | Keys.G);
			Assert.AreEqual(new KC(MK.Alt | MK.Shift, Key.NumPad0).ToKeys(), Keys.Alt | Keys.Shift | Keys.NumPad0);
			Assert.AreEqual(new KC(MK.Control | MK.Shift, Key.Add).ToKeys(), Keys.Control | Keys.Shift | Keys.Add);
			Assert.AreEqual(new KC(MK.Alt | MK.Control | MK.Shift, Key.OemComma).ToKeys(), Keys.Alt | Keys.Control | Keys.Shift | Keys.Oemcomma);
			Assert.AreEqual(new KC(MK.Control, Key.Oem7).ToKeys(), Keys.Control | Keys.Oem7);
		}

		[TestMethod]
		public void ToStringTest() {
			Assert.AreEqual(new KC(MK.None, Key.F11).ToString(), "F11");
			Assert.AreEqual(new KC(MK.Control, Key.Down).ToString(), "Ctrl+Down");
			Assert.AreEqual(new KC(MK.Alt, Key.D4).ToString(), "Alt+4");
			Assert.AreEqual(new KC(MK.Alt | MK.Control, Key.G).ToString(), "Ctrl+Alt+G");
			Assert.AreEqual(new KC(MK.Alt | MK.Shift, Key.NumPad0).ToString(), "Alt+Shift+NumPad0");
			Assert.AreEqual(new KC(MK.Control | MK.Shift, Key.Add).ToString(), "Ctrl+Shift+Add");
			Assert.AreEqual(new KC(MK.Alt | MK.Control | MK.Shift, Key.OemComma).ToString(), "Ctrl+Alt+Shift+Comma");
			Assert.AreEqual(new KC(MK.Control, Key.Oem7).ToString(), "Ctrl+OEM7");
		}
	}
}