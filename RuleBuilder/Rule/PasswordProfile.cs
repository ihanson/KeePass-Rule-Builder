using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;

namespace RuleBuilder.Rule {
	internal class PasswordProfile : IPasswordGenerator {
		public PwProfile Profile { get; }
		public PasswordProfile(PwProfile profile) {
			this.Profile = profile;
		}
		public string NewPassword() {
			_ = PwGenerator.Generate(out ProtectedString result, this.Profile, null, KeePass.Program.PwGeneratorPool);
			return result.ReadString();
		}
	}
}
