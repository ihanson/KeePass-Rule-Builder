using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Serialization.Tests {
	[TestClass]
	public class ComponentContractTests {
		[TestMethod]
		public void ComponentTest() {
			ComponentContract component = new ComponentContract(new Component(CharacterClass.Letters, true));
			Assert.AreEqual(CharacterClassEnum.Letters, component.CharacterSet.CharacterClass);
			Assert.IsTrue(component.Required);
		}
		[TestMethod]
		public void ComponentObjectTest() {
			Component component = new ComponentContract(new Component(CharacterClass.Letters, true)).Object();
			Assert.AreEqual(CharacterClassEnum.Letters, component.CharacterClass.Enumeration);
			Assert.IsTrue(component.Required);
		}
	}
}