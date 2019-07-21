namespace RuleBuilder.Rule {
	public class Component {
		public Component(CharacterClass characterClass, int minCount) {
			this.CharacterClass = characterClass;
			this.MinCount = minCount;
		}
		public CharacterClass CharacterClass { get; set; }
		public int MinCount { get; set; } = 0;
	}
}
