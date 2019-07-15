using System.Runtime.Serialization;

namespace RuleBuilder.Rule.Serialization {
	[DataContract]
	internal class ConfigurationContract {
		public ConfigurationContract(IPasswordGenerator generator) {
			if (generator is PasswordRule) {
				this.Rule = new RuleContract((PasswordRule)generator);
			} else if (generator is PasswordProfile) {
				this.Profile = new ProfileContract((PasswordProfile)generator);
			}
		}
		public IPasswordGenerator Object() => (IPasswordGenerator)this.Rule?.Object() ?? this.Profile.Object();
		[DataMember(EmitDefaultValue = false)]
		public RuleContract Rule { get; private set; }
		[DataMember(EmitDefaultValue = false)]
		public ProfileContract Profile { get; private set; }
	}
}
