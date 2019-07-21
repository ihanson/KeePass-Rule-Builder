using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;

namespace RuleBuilder.Rule.Serialization.Tests {
	[TestClass()]
	public class ComponentContractTests {
		[TestMethod()]
		public void ComponentTest() {
			ComponentContract component = new ComponentContract(new Component(CharacterClass.Letters, 32));
			Assert.AreEqual(CharacterClassEnum.Letters, component.CharacterSet.CharacterClass);
			Assert.AreEqual(32, component.MinCount);
		}
		[TestMethod()]
		public void ComponentObjectTest() {
			Component component = new ComponentContract(new Component(CharacterClass.Letters, 32)).Object();
			Assert.AreEqual(CharacterClassEnum.Letters, component.CharacterClass.Enumeration);
			Assert.AreEqual(32, component.MinCount);
		}
		[TestMethod()]
		public void NegativeLengthTest() {
			_ = Assert.ThrowsException<SerializationException>(() => _ = new ComponentContract(new Component(CharacterClass.Letters, -15)).Object());
		}
	}
}