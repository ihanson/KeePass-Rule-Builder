using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Tests {
	[TestClass]
	public class ComponentTests {
		[TestMethod]
		public void ComponentTest() {
			Component component = new Component(CharacterClass.Letters, true);
			Assert.AreEqual(CharacterClassEnum.Letters, component.CharacterClass.Enumeration);
			Assert.IsTrue(component.Required);
		}
	}
}