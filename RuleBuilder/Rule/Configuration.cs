namespace RuleBuilder.Rule {
	public class Configuration {
		public Configuration() {
			this.Generator = PasswordProfile.DefaultProfile;
		}

		public Configuration(IPasswordGenerator generator, Expiration expiration = null) {
			this.Expiration = expiration;
			this.Generator = generator;
		}

		public IPasswordGenerator Generator { get; set; }

		public Expiration Expiration { get; set; }

		public bool IsDefaultConfiguration() => this.Expiration == null && this.Generator is PasswordProfile profile && profile.IsDefaultProfile;
	}
}
