using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;

namespace RuleBuilder.Rule {
	internal class PasswordRule : IPasswordGenerator {
		public string NewPassword(RNGCryptoServiceProvider csp) {
			string password = string.Empty;
			foreach (Component component in this.Components) {
				if (component.CharacterSet.Characters.Length > 0) {
					for (int count = 0; count < component.MinCount; count++) {
						password += component.CharacterSet.RandomCharacter(csp).ToString();
					}
				}
			}
			password = this.FillToLength(csp, password);
			char[] chars = password.ToCharArray();
			Random.Shuffle(csp, chars);
			return new string(chars);
		}
		private string FillToLength(RNGCryptoServiceProvider csp, string password) {
			if (password.Length >= this.Length) {
				return password;
			}
			char[] allCharacters = this.AllCharacters();
			if (allCharacters.Length > 0) {
				while (password.Length < this.Length) {
					password += Random.RandomItem(csp, allCharacters);
				}
			}
			return password;
		}

		public uint Length { get; set; } = 32;
		public BindingList<Component> Components { get; set; } = new BindingList<Component>();
		private char[] AllCharacters() {
			List<char> chars = new List<char>();
			HashSet<char> used = new HashSet<char>();
			foreach (Component component in this.Components) {
				foreach (char c in component.CharacterSet.Characters) {
					if (!used.Contains(c)) {
						chars.Add(c);
						used.Add(c);
					}
				}
			}
			return chars.ToArray();
		}
	}
}
