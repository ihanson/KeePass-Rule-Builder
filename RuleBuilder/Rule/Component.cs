using System;
using System.Windows.Forms;

namespace RuleBuilder.Rule {
	internal class Component {
		public Component(NamedCharacterSet characterSet, int minCount) {
			this.CharacterSet = characterSet;
			this.MinCount = minCount;
		}
		public NamedCharacterSet CharacterSet { get; set; }
		public int MinCount { get; set; } = 0;
	}
}
