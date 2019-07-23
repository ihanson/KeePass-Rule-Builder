namespace RuleBuilder.Rule {
	public class Component {
		public Component(CharacterClass characterClass, bool required) {
			this.CharacterClass = characterClass;
			this.Required = required;
		}
		public CharacterClass CharacterClass { get; set; }
		public bool Required { get; set; }
	}
}
