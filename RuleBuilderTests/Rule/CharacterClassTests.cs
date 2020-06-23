using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Tests {
	[TestClass]
	public class CharacterClassTests {
		[TestMethod]
		public void DefaultCharacterClassTest() {
			CharacterClass charClass = new CharacterClass();
			Assert.AreEqual(string.Empty, charClass.Characters);
			Assert.AreEqual(CharacterClassEnum.Custom, charClass.Enumeration);
		}

		[TestMethod]
		public void CustomCharacterClassTest() {
			CharacterClass charClass = new CharacterClass("abc");
			Assert.AreEqual("abc", charClass.Characters);
			Assert.AreEqual(CharacterClassEnum.Custom, charClass.Enumeration);
		}

		[TestMethod]
		public void EnumeratedCharacterClassTest() {
			Assert.AreSame(CharacterClass.AllCharacters, CharacterClass.EnumeratedCharacterClass(CharacterClassEnum.AllCharacters));
			Assert.AreSame(CharacterClass.Letters, CharacterClass.EnumeratedCharacterClass(CharacterClassEnum.Letters));
			Assert.AreSame(CharacterClass.Digits, CharacterClass.EnumeratedCharacterClass(CharacterClassEnum.Digits));
			Assert.AreSame(CharacterClass.Punctuation, CharacterClass.EnumeratedCharacterClass(CharacterClassEnum.Punctuation));
			Assert.AreSame(CharacterClass.UppercaseLetters, CharacterClass.EnumeratedCharacterClass(CharacterClassEnum.UppercaseLetters));
			Assert.AreSame(CharacterClass.LowercaseLetters, CharacterClass.EnumeratedCharacterClass(CharacterClassEnum.LowercaseLetters));
			_ = Assert.ThrowsException<ArgumentException>(() => {
				_ = CharacterClass.EnumeratedCharacterClass((CharacterClassEnum)99);
			});
		}

		[TestMethod]
		public void SplitStringTest() => Assert.IsTrue(new HashSet<string> { "a", "b", "c", "🧸", "🐱", "\x200d", "👤" }.SetEquals(CharacterClass.SplitString("abc🧸🐱‍👤")));
	}
}