using System;
using System.Windows.Forms;

namespace RuleBuilder.Rule {
	internal class Component {
		public Component(char[] characterSet, int minCount) {
			this.CharacterSet = characterSet;
			this.MinCount = minCount;
		}
		public char[] CharacterSet { get; set; }
		public int MinCount { get; set; } = 0;
	}
}
