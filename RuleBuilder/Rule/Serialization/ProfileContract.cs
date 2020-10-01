using System;
using System.Runtime.Serialization;

namespace RuleBuilder.Rule.Serialization {
	[DataContract]
	public class ProfileContract {
		public ProfileContract(PasswordProfile profile) {
			if (profile == null) {
				throw new ArgumentNullException(nameof(profile));
			}
			this.IsDefault = profile.IsDefaultProfile;
			if (!this.IsDefault) {
				this.Name = profile.Name;
			}
		}
		[DataMember(EmitDefaultValue = false)]
		public bool IsDefault { get; private set; }
		[DataMember(EmitDefaultValue = false)]
		public string Name { get; private set; }
		public PasswordProfile DeserializedObject() => this.IsDefault ? PasswordProfile.DefaultProfile : PasswordProfile.NamedProfile(this.Name);
	}
}
