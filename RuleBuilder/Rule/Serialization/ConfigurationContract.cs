using System;
using System.Runtime.Serialization;

namespace RuleBuilder.Rule.Serialization {
	[DataContract]
	public class ConfigurationContract {
		public ConfigurationContract(Configuration config) {
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			if (config.Generator is PasswordRule) {
				this.Rule = new RuleContract((PasswordRule)config.Generator);
			} else if (config.Generator is PasswordProfile) {
				this.Profile = new ProfileContract((PasswordProfile)config.Generator);
			}
			this.Expiration = config.Expiration != null ? new ExpirationContract(config.Expiration) : null;
		}

		[DataMember(EmitDefaultValue = false)]
		public RuleContract Rule { get; private set; }

		[DataMember(EmitDefaultValue = false)]
		public ProfileContract Profile { get; private set; }

		[DataMember(EmitDefaultValue = false)]
		public ExpirationContract Expiration { get; private set; }

		public Configuration DeserializedObject() => new Configuration(
			(IPasswordGenerator)this.Rule?.DeserializedObject() ?? this.Profile.DeserializedObject(),
			this.Expiration?.DeserializedObject()
		);
	}
}
