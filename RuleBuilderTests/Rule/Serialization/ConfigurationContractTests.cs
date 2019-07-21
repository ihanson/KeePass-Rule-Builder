using KeePassLib.Cryptography.PasswordGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Serialization.Tests {
	[TestClass]
	public class ConfigurationContractTests {
		[TestMethod]
		public void RuleTest() {
			ConfigurationContract configuration = new ConfigurationContract(new PasswordRule());
			Assert.IsNotNull(configuration.Rule);
			Assert.IsNull(configuration.Profile);
		}

		[TestMethod]
		public void RuleObjectTest() {
			IPasswordGenerator generator = new ConfigurationContract(new PasswordRule()).Object();
			Assert.IsInstanceOfType(generator, typeof(PasswordRule));
		}
		[TestMethod]
		public void ProfileTest() {
			ConfigurationContract configuration = new ConfigurationContract(new PasswordProfile(new PwProfile()));
			Assert.IsNull(configuration.Rule);
			Assert.IsNotNull(configuration.Profile);
		}

		[TestMethod]
		public void ProfileObjectTest() {
			IPasswordGenerator generator = new ConfigurationContract(new PasswordProfile(new PwProfile())).Object();
			Assert.IsInstanceOfType(generator, typeof(PasswordProfile));
		}
	}
}