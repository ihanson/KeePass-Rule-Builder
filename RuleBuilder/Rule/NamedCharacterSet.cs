using System.Collections.Generic;

namespace RuleBuilder.Rule {
	internal class NamedCharacterSet {
		public NamedCharacterSet(string name, HashSet<char> chars, NamedCharacterSetEnum enumeration) {
			this.Name = name;
			this.Characters = new HashSet<char>(chars);
			this.Enumeration = enumeration;
		}
		public NamedCharacterSet() {
			this.Name = string.Empty;
			this.Characters = new HashSet<char>();
			this.Enumeration = NamedCharacterSetEnum.Custom;
		}
		public string Name { get; }
		public HashSet<char> Characters { get; set; }
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
		public static readonly NamedCharacterSet AllCharacters = new NamedCharacterSet("All characters", new HashSet<char>((strUpperLetters + strLowerLetters + strDigits + strPunctuation).ToCharArray()), NamedCharacterSetEnum.AllCharacters);
		public static readonly NamedCharacterSet Letters = new NamedCharacterSet("Letters (A–Z, a–z)", new HashSet<char>((strUpperLetters + strLowerLetters).ToCharArray()), NamedCharacterSetEnum.Letters);
		public static readonly NamedCharacterSet Digits = new NamedCharacterSet("Digits (0–9)", new HashSet<char>(strDigits.ToCharArray()), NamedCharacterSetEnum.Digits);
		public static readonly NamedCharacterSet Punctuation = new NamedCharacterSet("Punctuation", new HashSet<char>(strPunctuation.ToCharArray()), NamedCharacterSetEnum.Punctuation);
		public static readonly NamedCharacterSet UppercaseLetters = new NamedCharacterSet("Uppercase letters (A–Z)", new HashSet<char>(strUpperLetters.ToCharArray()), NamedCharacterSetEnum.UppercaseLetters);
		public static readonly NamedCharacterSet LowercaseLetters = new NamedCharacterSet("Lowercase letters (a–z)", new HashSet<char>(strLowerLetters.ToCharArray()), NamedCharacterSetEnum.LowercaseLetters);
		private const string strUpperLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string strLowerLetters = "abcdefghijklmnopqrstuvwxyz";
		private const string strDigits = "0123456789";
		private const string strPunctuation = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ";
	}
	internal enum NamedCharacterSetEnum {
		Custom = 0,
		AllCharacters = 1,
		Letters = 2,
		Digits = 3,
		Punctuation = 4,
		UppercaseLetters = 5,
		LowercaseLetters = 6
	}
}
