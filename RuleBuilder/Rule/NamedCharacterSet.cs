namespace RuleBuilder.Rule {
	internal class NamedCharacterSet : ICharacterSet {
		private NamedCharacterSet(string name, char[] chars, NamedCharacterSetEnum enumeration) {
			this.Name = name;
			this.Characters = chars;
			this.Enumeration = enumeration;
		}
		public string Name { get; }
		public char[] Characters { get; private set; }
		public NamedCharacterSetEnum Enumeration { get; }
		public static NamedCharacterSet CharacterSetFromEnumeration(NamedCharacterSetEnum enumeration) {
			switch (enumeration) {
				case NamedCharacterSetEnum.AllCharacters:
					return AllCharacters;
				case NamedCharacterSetEnum.Letters:
					return Letters;
				case NamedCharacterSetEnum.Digits:
					return Digits;
				case NamedCharacterSetEnum.Punctuation:
					return Punctuation;
				case NamedCharacterSetEnum.UppercaseLetters:
					return UppercaseLetters;
				case NamedCharacterSetEnum.LowercaseLetters:
					return LowercaseLetters;
				default:
					throw new System.ArgumentException("Invalid character set enumeration", nameof(enumeration));
			}
		}
		public static readonly NamedCharacterSet AllCharacters = new NamedCharacterSet("All characters", (strUpperLetters + strLowerLetters + strDigits + strPunctuation).ToCharArray(), NamedCharacterSetEnum.AllCharacters);
		public static readonly NamedCharacterSet Letters = new NamedCharacterSet("Letters (A–Z, a–z)", (strUpperLetters + strLowerLetters).ToCharArray(), NamedCharacterSetEnum.Letters);
		public static readonly NamedCharacterSet Digits = new NamedCharacterSet("Digits (0–9)", strDigits.ToCharArray(), NamedCharacterSetEnum.Digits);
		public static readonly NamedCharacterSet Punctuation = new NamedCharacterSet("Punctuation", strPunctuation.ToCharArray(), NamedCharacterSetEnum.Punctuation);
		public static readonly NamedCharacterSet UppercaseLetters = new NamedCharacterSet("Uppercase letters (A–Z)", strUpperLetters.ToCharArray(), NamedCharacterSetEnum.UppercaseLetters);
		public static readonly NamedCharacterSet LowercaseLetters = new NamedCharacterSet("Lowercase letters (a–z)", strLowerLetters.ToCharArray(), NamedCharacterSetEnum.LowercaseLetters);
		private const string strUpperLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string strLowerLetters = "abcdefghijklmnopqrstuvwxyz";
		private const string strDigits = "0123456789";
		private const string strPunctuation = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ";
	}
	internal enum NamedCharacterSetEnum {
		AllCharacters = 0,
		Letters = 1,
		Digits = 2,
		Punctuation = 3,
		UppercaseLetters = 4,
		LowercaseLetters = 5
	}
}
