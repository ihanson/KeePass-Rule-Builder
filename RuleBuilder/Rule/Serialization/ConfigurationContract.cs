using System.Runtime.Serialization;

namespace RuleBuilder.Rule.Serialization {
	[DataContract]
	public class ConfigurationContract {
		public ConfigurationContract(IPasswordGenerator generator) {
			if (generator is PasswordRule) {
				this.Rule = new RuleContract((PasswordRule)generator);
			} else if (generator is PasswordProfile) {
				this.Profile = new ProfileContract((PasswordProfile)generator);
			}
		}
		[DataMember(EmitDefaultValue = false)]
		public RuleContract Rule { get; private set; }
		[DataMember(EmitDefaultValue = false)]
		public ProfileContract Profile { get; private set; }
		public IPasswordGenerator DeserializedObject() => (IPasswordGenerator)this.Rule?.DeserializedObject() ?? this.Profile.DeserializedObject();
	}
}
