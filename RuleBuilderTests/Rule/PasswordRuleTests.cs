using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Tests {
	[TestClass]
	public class PasswordRuleTests {
		[TestMethod]
		public  void NoComponentsTest() {
			Assert.AreEqual(16, new PasswordRule() {
				Length = 16
			}.NewPassword().Length);
		}
		[TestMethod]
		public void ComponentsTest() {
			string password = new PasswordRule() {
				Length = 32,
				Components = new List<Component>() {
					new Component(new CharacterClass("a"), false),
					new Component(new CharacterClass("b"), true),
					new Component(new CharacterClass("cd"), true)
				}
			}.NewPassword();
			Assert.IsTrue(password.Contains("b"));
			Assert.IsTrue(password.Contains("c") || password.Contains("d"));
			Assert.AreEqual(32, password.Length);
		}
		[TestMethod]
		public void ExtraCharactersTest() {
			string password = new PasswordRule() {
				Length = 5,
				Components = new List<Component>() {
					new Component(new CharacterClass("a"), true),
					new Component(new CharacterClass("bc"), true),
					new Component(new CharacterClass("d"), true),
					new Component(new CharacterClass("e"), true),
					new Component(new CharacterClass("f"), true),
					new Component(new CharacterClass("g"), true),
					new Component(new CharacterClass("h"), true),
					new Component(new CharacterClass("i"), false)
				}
			}.NewPassword();
			Assert.IsTrue(password.Contains("a"));
			Assert.IsTrue(password.Contains("b") || password.Contains("c"));
			Assert.IsTrue(password.Contains("d"));
			Assert.IsTrue(password.Contains("e"));
			Assert.IsTrue(password.Contains("f"));
			Assert.IsTrue(password.Contains("g"));
			Assert.IsTrue(password.Contains("h"));
			Assert.IsFalse(password.Contains("i"));
			Assert.AreEqual(7, password.Length);
		}
		[TestMethod]
		public void ExcludeTest() {
			string password = new PasswordRule() {
				Length = 32,
				Components = new List<Component>() {
					new Component(new CharacterClass("abc"), false)
				},
				Exclude = "ab"
			}.NewPassword();
			Assert.AreEqual(0, CharCount(password, 'a'));
			Assert.AreEqual(0, CharCount(password, 'b'));
			Assert.AreEqual(32, CharCount(password, 'c'));
		}
		[TestMethod]
		public void ExcludeCharsTest() {
			Assert.IsTrue(new HashSet<string> { "a", "b", "c" }.SetEquals(new PasswordRule() {
				Exclude = "abca"
			}.ExcludeChars));
		}
		private static int CharCount(string str, char character) {
			int count = 0;
			foreach (char strChar in str) {
				if (strChar == character) {
					++count;
				}
			}
			return count;
		}
	}
}