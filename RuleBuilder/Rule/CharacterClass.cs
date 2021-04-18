using System;
using System.Collections.Generic;
using System.Globalization;
using RuleBuilder.Properties;

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
	public class CharacterClass : RuleProperty {
		public static readonly CharacterClass AllCharacters = new CharacterClass(
			Resources.AllCharacters,
			UppercaseLettersChars + LowercaseLettersChars + DigitsChars + PunctuationChars,
			CharacterClassEnum.AllCharacters
		);

		public static readonly CharacterClass Letters = new CharacterClass(
			Resources.Letters,
			UppercaseLettersChars + LowercaseLettersChars,
			CharacterClassEnum.Letters
		);

		public static readonly CharacterClass Digits = new CharacterClass(
			Resources.Digits,
			DigitsChars,
			CharacterClassEnum.Digits
		);

		public static readonly CharacterClass Punctuation = new CharacterClass(
			Resources.Punctuation,
			PunctuationChars,
			CharacterClassEnum.Punctuation
		);

		public static readonly CharacterClass UppercaseLetters = new CharacterClass(
			Resources.UppercaseLetters,
			UppercaseLettersChars,
			CharacterClassEnum.UppercaseLetters
		);

		public static readonly CharacterClass LowercaseLetters = new CharacterClass(
			Resources.LowercaseLetters,
			LowercaseLettersChars,
			CharacterClassEnum.LowercaseLetters
		);

		private const string UppercaseLettersChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string LowercaseLettersChars = "abcdefghijklmnopqrstuvwxyz";
		private const string DigitsChars = "0123456789";
		private const string PunctuationChars = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ";

		public CharacterClass() : this(string.Empty) { }

		public CharacterClass(string characters) : this(Resources.Custom, characters, CharacterClassEnum.Custom) { }

		private CharacterClass(string name, string characters, CharacterClassEnum enumeration) {
			this.Name = name;
			this.Characters = characters;
			this.Enumeration = enumeration;
		}

		public string Name { get; }

		private string _characters = string.Empty;
		public string Characters {
			get => this._characters;
			set {
				if (this._characters != value) {
					this._characters = value;
					this.NotifyPropertyChanged(nameof(this.Characters));
				}
			}
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
					throw new ArgumentException(Resources.InvalidCharacterSet, nameof(enumeration));
			}
		}

		public static HashSet<string> SplitString(string str) {
			HashSet<string> result = new HashSet<string>();
			TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(str);
			while (enumerator.MoveNext()) {
				result.Add((string)enumerator.Current);
			}
			return result;
		}

		public CharacterClass Clone() => new CharacterClass(this.Name, this.Characters, this.Enumeration);
	}
}
