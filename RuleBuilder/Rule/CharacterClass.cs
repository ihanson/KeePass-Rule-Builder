using System.Collections.Generic;
using System.Globalization;

namespace RuleBuilder.Rule {
	internal class CharacterClass {
		private CharacterClass(string name, HashSet<string> chars, CharacterClassEnum enumeration) {
			this.Name = name;
			this.CharacterSet = chars;
			this.Enumeration = enumeration;
		}
		public CharacterClass() : this(string.Empty) { }
		public CharacterClass(string characters) {
			this.Name = string.Empty;
			this.Characters = characters;
			this.Enumeration = CharacterClassEnum.Custom;
		}
		public string Name { get; }
		public HashSet<string> CharacterSet { get; private set; } = new HashSet<string>();
		public string Characters {
			get => SortedString(this.CharacterSet);
			set => this.CharacterSet = SplitString(value);
		}
		public CharacterClassEnum Enumeration { get; }
		public static CharacterClass EnumeratedCharacterClass(CharacterClassEnum enumeration) {
			switch (enumeration) {
				case CharacterClassEnum.AllCharacters:
					return AllCharacters;
				case CharacterClassEnum.Letters:
					return Letters;
				case CharacterClassEnum.Digits:
					return Digits;
				case CharacterClassEnum.Punctuation:
					return Punctuation;
				case CharacterClassEnum.UppercaseLetters:
					return UppercaseLetters;
				case CharacterClassEnum.LowercaseLetters:
					return LowercaseLetters;
				default:
					throw new System.ArgumentException("Invalid character set enumeration", nameof(enumeration));
			}
		}
		public static string SortedString(IEnumerable<string> charSet) {
			List<string> chars = new List<string>(charSet);
			chars.Sort();
			return string.Join(string.Empty, chars);
		}
		public static HashSet<string> SplitString(string str) {
			HashSet<string> result = new HashSet<string>();
			TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(str);
			while (enumerator.MoveNext()) {
				result.Add((string)enumerator.Current);
			}
			return result;
		}

		public static readonly CharacterClass AllCharacters = new CharacterClass("All characters", SplitString(strUpperLetters + strLowerLetters + strDigits + strPunctuation), CharacterClassEnum.AllCharacters);
		public static readonly CharacterClass Letters = new CharacterClass("Letters (A–Z, a–z)", SplitString(strUpperLetters + strLowerLetters), CharacterClassEnum.Letters);
		public static readonly CharacterClass Digits = new CharacterClass("Digits (0–9)", SplitString(strDigits), CharacterClassEnum.Digits);
		public static readonly CharacterClass Punctuation = new CharacterClass("Punctuation", SplitString(strPunctuation), CharacterClassEnum.Punctuation);
		public static readonly CharacterClass UppercaseLetters = new CharacterClass("Uppercase letters (A–Z)", SplitString(strUpperLetters), CharacterClassEnum.UppercaseLetters);
		public static readonly CharacterClass LowercaseLetters = new CharacterClass("Lowercase letters (a–z)", SplitString(strLowerLetters), CharacterClassEnum.LowercaseLetters);
		private const string strUpperLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string strLowerLetters = "abcdefghijklmnopqrstuvwxyz";
		private const string strDigits = "0123456789";
		private const string strPunctuation = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ";
	}
	internal enum CharacterClassEnum {
		Custom = 0,
		AllCharacters = 1,
		Letters = 2,
		Digits = 3,
		Punctuation = 4,
		UppercaseLetters = 5,
		LowercaseLetters = 6
	}
}
