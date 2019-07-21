using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Serialization.Tests {
	[TestClass]
	public class CharacterClassContractTests {
		[TestMethod]
		public void BuiltInCharacterSetTest() {
			CharacterClassContract charClass = new CharacterClassContract(CharacterClass.LowercaseLetters);
			Assert.AreEqual(CharacterClassEnum.LowercaseLetters, charClass.CharacterClass);
			Assert.IsNull(charClass.Characters);
		}
		[TestMethod]
		public void BuiltInCharacterSetObjectTest() {
			CharacterClass charClass = new CharacterClassContract(CharacterClass.LowercaseLetters).Object();
			Assert.AreEqual(CharacterClassEnum.LowercaseLetters, charClass.Enumeration);
			Assert.AreEqual(CharacterClass.LowercaseLetters.Characters, charClass.Characters);
		}
		[TestMethod]
		public void CustomCharacterSetTest() {
			CharacterClassContract charClass = new CharacterClassContract(new CharacterClass("abc"));
			Assert.AreEqual(CharacterClassEnum.Custom, charClass.CharacterClass);
			Assert.AreEqual("abc", charClass.Characters);
		}
		[TestMethod]
		public void CustomCharacterSetObjectTest() {
			CharacterClass charClass = new CharacterClassContract(new CharacterClass("abc")).Object();
			Assert.AreEqual(CharacterClassEnum.Custom, charClass.Enumeration);
			Assert.AreEqual("abc", charClass.Characters);
		}
	}
}