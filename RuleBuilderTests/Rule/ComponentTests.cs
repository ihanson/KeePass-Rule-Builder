using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Tests {
	[TestClass()]
	public class ComponentTests {
		[TestMethod()]
		public void ComponentTest() {
			Component component = new Component(CharacterClass.Letters, 4);
			Assert.AreEqual(CharacterClassEnum.Letters, component.CharacterClass.Enumeration);
			Assert.AreEqual(4, component.MinCount);
		}
	}
}