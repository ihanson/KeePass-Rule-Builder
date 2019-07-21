using System;
using System.Collections.Generic;
using System.Globalization;

namespace RuleBuilder.Rule {
	public enum CharacterClassEnum {
		Custom = 0,
		AllCharacters = 1,
		Letters = 2,
		Digits = 3,
		Punctuation = 4,
		UppercaseLetters = 5,
		LowercaseLetters = 6
	}
	public class CharacterClass {
		public static readonly CharacterClass AllCharacters = new CharacterClass("All characters", SplitString(UppercaseLettersChars, LowercaseLettersChars, DigitsChars, PunctuationChars), CharacterClassEnum.AllCharacters);
		public static readonly CharacterClass Letters = new CharacterClass("Letters (A–Z, a–z)", SplitString(UppercaseLettersChars, LowercaseLettersChars), CharacterClassEnum.Letters);
		public static readonly CharacterClass Digits = new CharacterClass("Digits (0–9)", SplitString(DigitsChars), CharacterClassEnum.Digits);
		public static readonly CharacterClass Punctuation = new CharacterClass("Punctuation", SplitString(PunctuationChars), CharacterClassEnum.Punctuation);
		public static readonly CharacterClass UppercaseLetters = new CharacterClass("Uppercase letters (A–Z)", SplitString(UppercaseLettersChars), CharacterClassEnum.UppercaseLetters);
		public static readonly CharacterClass LowercaseLetters = new CharacterClass("Lowercase letters (a–z)", SplitString(LowercaseLettersChars), CharacterClassEnum.LowercaseLetters);
		private const string UppercaseLettersChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string LowercaseLettersChars = "abcdefghijklmnopqrstuvwxyz";
		private const string DigitsChars = "0123456789";
		private const string PunctuationChars = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ";
		public CharacterClass() : this(string.Empty) { }
		public CharacterClass(string characters) {
			this.Name = string.Empty;
			this.Characters = characters;
			this.Enumeration = CharacterClassEnum.Custom;
		}
		private CharacterClass(string name, HashSet<string> chars, CharacterClassEnum enumeration) {
			this.Name = name;
			this.CharacterSet = chars;
			this.Enumeration = enumeration;
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
					throw new ArgumentException("Invalid character set enumeration", nameof(enumeration));
			}
		}
		public static string SortedString(IEnumerable<string> charSet) {
			List<string> chars = new List<string>(charSet);
			chars.Sort();
			return string.Join(string.Empty, chars);
		}
		public static HashSet<string> SplitString(params string[] strings) {
			HashSet<string> result = new HashSet<string>();
			foreach (string str in strings) {
				TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(str);
				while (enumerator.MoveNext()) {
					result.Add((string)enumerator.Current);
				}
			}
			return result;
		}
	}
}
