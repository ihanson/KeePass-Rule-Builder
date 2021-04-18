namespace RuleBuilder.Rule {
	public class Component : RuleProperty {
		public Component(CharacterClass characterClass, bool required) {
			this.ChangeDelegate = () => this.NotifyRuleChanged();
			this.CharacterClass = characterClass;
			this.Required = required;
		}

		private RuleChangedDelegate ChangeDelegate { get; }

		private CharacterClass _characterClass;
		public CharacterClass CharacterClass {
			get => this._characterClass;
			set {
				if (value != this._characterClass) {
					if (this._characterClass != null) {
						this._characterClass.RuleChanged -= this.ChangeDelegate;
					}
					if (value != null) {
						value.RuleChanged += this.ChangeDelegate;
					}
					this._characterClass = value;
					this.NotifyPropertyChanged(nameof(this.CharacterClass));
				}
			}
		}

		private bool _required;
		public bool Required {
			get => this._required;
			set {
				if (value != this._required) {
					this._required = value;
					this.NotifyPropertyChanged(nameof(this.Required));
				}
			}
		}

		public Component Clone() => new Component(this.CharacterClass, this.Required);
	}
}