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
					new Component(new CharacterClass("a"), 1),
					new Component(new CharacterClass("b"), 24),
					new Component(new CharacterClass("cd"), 4)
				}
			}.NewPassword();
			Assert.IsTrue(CharCount(password, 'a') >= 1);
			Assert.IsTrue(CharCount(password, 'b') >= 24);
			Assert.IsTrue(CharCount(password, 'c') + CharCount(password, 'd') >= 4);
			Assert.AreEqual(32, password.Length);
		}
		[TestMethod]
		public void ExtraCharactersTest() {
			string password = new PasswordRule() {
				Length = 32,
				Components = new List<Component>() {
					new Component(new CharacterClass("a"), 18),
					new Component(new CharacterClass("bc"), 18)
				}
			}.NewPassword();
			Assert.AreEqual(18, CharCount(password, 'a'));
			Assert.AreEqual(18, CharCount(password, 'b') + CharCount(password, 'c'));
			Assert.AreEqual(36, password.Length);
		}
		[TestMethod]
		public void ExcludeTest() {
			string password = new PasswordRule() {
				Length = 32,
				Components = new List<Component>() {
					new Component(new CharacterClass("abc"), 0)
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