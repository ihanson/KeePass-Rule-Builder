using System.Security.Cryptography;

namespace RuleBuilder.Rule {
	internal class CharacterSet {
		public string Label { get; private set; }
		public char[] Characters { get; set; }
		public char RandomCharacter(RNGCryptoServiceProvider csp) => Random.RandomItem(csp, this.Characters);

		public static readonly CharacterSet AllCharacters = new CharacterSet() {
			Label = "All characters",
			Characters = (strUpperLetters + strLowerLetters + strDigits + strPunctuation).ToCharArray()
		};
		public static readonly CharacterSet Letters = new CharacterSet() {
			Label = "Letters (A–Z, a–z)",
			Characters = (strUpperLetters + strLowerLetters).ToCharArray()
		};
		public static readonly CharacterSet Digits = new CharacterSet() {
			Label = "Digits (0–9)",
			Characters = strDigits.ToCharArray()
		};
		public static readonly CharacterSet Punctuation = new CharacterSet() {
			Label = "Punctuation",
			Characters = strPunctuation.ToCharArray()
		};
		public static readonly CharacterSet UppercaseLetters = new CharacterSet() {
			Label = "Uppercase letters (A–Z)",
			Characters = strUpperLetters.ToCharArray()
		};
		public static readonly CharacterSet LowercaseLetters = new CharacterSet() {
			Label = "Lowercase letters (a–z)",
			Characters = strLowerLetters.ToCharArray()
		};
		private const string strUpperLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string strLowerLetters = "abcdefghijklmnopqrstuvwxyz";
		private const string strDigits = "0123456789";
		private const string strPunctuation = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ";
	}
}
