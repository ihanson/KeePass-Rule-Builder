using System.Security.Cryptography;

namespace RuleBuilder.Rule {
	internal interface ICharacterSet {
		char[] Characters { get; }
	}
}
