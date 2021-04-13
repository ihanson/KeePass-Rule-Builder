using KeePassLib.Cryptography.PasswordGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Serialization.Tests {
	[TestClass]
	public class ConfigurationContractTests {
		[TestMethod]
		public void RuleTest() {
			ConfigurationContract configuration = new ConfigurationContract(new Configuration(new PasswordRule()));
			Assert.IsNotNull(configuration.Rule);
			Assert.IsNull(configuration.Profile);
		}

		[TestMethod]
		public void RuleObjectTest() {
			Configuration config = new ConfigurationContract(new Configuration(new PasswordRule())).DeserializedObject();
			Assert.IsInstanceOfType(config.Generator, typeof(PasswordRule));
		}
		[TestMethod]
		public void ProfileTest() {
			ConfigurationContract configuration = new ConfigurationContract(new Configuration(new PasswordProfile(new PwProfile())));
			Assert.IsNull(configuration.Rule);
			Assert.IsNotNull(configuration.Profile);
		}

		[TestMethod]
		public void ProfileObjectTest() {
			Configuration config = new ConfigurationContract(new Configuration(new PasswordProfile(new PwProfile()))).DeserializedObject();
			Assert.IsInstanceOfType(config.Generator, typeof(PasswordProfile));
		}
	}
}