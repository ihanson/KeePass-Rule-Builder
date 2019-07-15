using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RuleBuilder.Rule.Serialization {
	[DataContract]
	internal class RuleContract {
		public RuleContract(PasswordRule rule) {
			this.Length = rule.Length;
			this.Components = rule.Components.ConvertAll((Component component) => new ComponentContract(component));
			this.Exclude = rule.Exclude;
		}
		public PasswordRule Object() {
			if (this.Length < 0) {
				throw new SerializationException("Password length must not be negative.");
			}
			return new PasswordRule() {
				Length = this.Length,
				Components = this.Components.ConvertAll((ComponentContract component) => component.Object()),
				Exclude = this.Exclude
			};
		}
		[DataMember]
		public int Length { get; private set; }
		[DataMember]
		public List<ComponentContract> Components { get; private set; }
		[DataMember]
		public string Exclude { get; private set; }
	}
}
