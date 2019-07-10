using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RuleBuilder.Rule {
	internal class PasswordRule : IPasswordGenerator {
		public string NewPassword() {
			List<char> password = new List<char>();
			foreach (Component component in this.Components) {
				HashSet<char> chars = new HashSet<char>(component.CharacterSet.Characters);
				foreach (char c in this.Exclude) {
					_ = chars.Remove(c);
				}
				if (chars.Count > 0) {
					for (int count = 0; count < component.MinCount; count++) {
						password.Add(Random.RandomItem(chars));
					}
				}
			}
			this.FillToLength(password);
			Random.Shuffle(password);
			return string.Join(string.Empty, password);
		}
		private void FillToLength(List<char> password) {
			if (password.Count >= this.Length) {
				return;
			}
			HashSet<char> allCharacters = this.AllCharacters();
			if (allCharacters.Count > 0) {
				while (password.Count < this.Length) {
					password.Add(Random.RandomItem(allCharacters));
				}
			}
			return;
		}
		public int Length { get; set; } = 0;
		public HashSet<char> Exclude { get; set; } = new HashSet<char>();
		public BindingList<Component> Components { get; set; } = new BindingList<Component>();
		private HashSet<char> AllCharacters() {
			HashSet<char> chars = new HashSet<char>();
			foreach (Component component in this.Components) {
				chars.UnionWith(component.CharacterSet.Characters);
			}
			foreach (char c in this.Exclude) {
				_ = chars.Remove(c);
			}
			return chars;
		}
	}
}
