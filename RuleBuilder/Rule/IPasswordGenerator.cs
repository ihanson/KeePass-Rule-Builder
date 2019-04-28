using System.Security.Cryptography;

namespace RuleBuilder.Rule {
	internal interface IPasswordGenerator {
		string NewPassword(RNGCryptoServiceProvider csp);
	}
}
