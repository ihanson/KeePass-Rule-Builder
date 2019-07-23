using System.Runtime.Serialization;

namespace RuleBuilder.Rule.Serialization {
	[DataContract]
	public class ComponentContract {
		public ComponentContract(Component component) {
			this.CharacterSet = new CharacterClassContract(component.CharacterClass);
			this.Required = component.Required;
		}
		[DataMember]
		public CharacterClassContract CharacterSet { get; private set; }
		[DataMember(EmitDefaultValue = false)]
		public bool Required { get; private set; }
		public Component Object() => new Component(this.CharacterSet.Object(), this.Required);
	}
}
