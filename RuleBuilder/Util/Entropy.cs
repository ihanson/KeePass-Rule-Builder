using System;
using System.Collections.Generic;
using System.Linq;
using RuleBuilder.Rule;

namespace RuleBuilder.Util {
	public static class Entropy {
		private const int MaxRequired = 5;

		public static double EntropyBits(PasswordRule rule) {
			if (rule == null) {
				throw new ArgumentNullException(nameof(rule));
			}
			HashSet<string> allCharacters = rule.AllCharacters();
			double numTotalPasswords = Math.Pow(allCharacters.Count, rule.Length);
			List<CharacterSetCount> requiredSets = RequiredCharacterSets(rule);
			if (requiredSets.Count > MaxRequired) {
				throw new ArgumentOutOfRangeException(nameof(rule), "Too many required character sets to calculate the entropy");
			}
			double numInvalidPasswords = 0.0;
			for (int n = 1; n <= requiredSets.Count; n++) {
				foreach (List<CharacterSetCount> required in SubsetsOfLength(requiredSets, n)) {
					numInvalidPasswords += CountPasswordsNotContaining(
						allCharacters,
						rule.Length,
						required
					) * (n % 2 == 1 ? 1.0 : -1.0);
				}
			}
			double result = Math.Log(numTotalPasswords - numInvalidPasswords, 2.0);
			if (double.IsInfinity(result) || double.IsNaN(result)) {
				throw new ArgumentOutOfRangeException(nameof(rule), "Entropy is too large to be calculated");
			}
			return result;
		}

		private static List<CharacterSetCount> RequiredCharacterSets(PasswordRule rule) {
			HashSet<string> allCharacters = rule.AllCharacters();
			var sets = new List<CharacterSetCount>();
			foreach (Component component in rule.Components.Where((Component component) => component.Required)) {
				HashSet<string> chars = CharacterClass.SplitString(component.CharacterClass?.Characters ?? string.Empty);
				chars.IntersectWith(allCharacters);
				if (chars.Count > 0) {
					AppendOrUpdate(sets, chars);
				}
			}
			return sets;
		}

		private static void AppendOrUpdate(List<CharacterSetCount> list, HashSet<string> chars) {
			foreach (CharacterSetCount count in list) {
				if (count.Characters.SetEquals(chars)) {
					count.Increment();
					return;
				}
			}
			list.Add(new CharacterSetCount(chars));
		}

		private static IEnumerable<List<T>> SubsetsOfLength<T>(List<T> list, int length, int start = 0) {
			if (length == 0) {
				yield return new List<T>();
			} else {
				for (int i = start; i + length - 1 < list.Count; i++) {
					foreach (List<T> set in SubsetsOfLength(list, length - 1, i + 1)) {
						yield return set.Prepend(list[i]).ToList();
					}
				}
			}
		}

		private static double CountPasswordsNotContaining(HashSet<string> allCharacters, int passwordLength, List<CharacterSetCount> charSets) {
			int otherCharCount = allCharacters.Where((string character) => !charSets.Any((CharacterSetCount charSet) => charSet.Characters.Contains(character))).Count();
			if (otherCharCount == 0) {
				return 0.0;
			}
			double total = 0.0;
			foreach (List<CharacterSetCount> shortCount in ShortCounts(charSets)) {
				int remainingLength = passwordLength;
				double passwordCount = 1.0;
				foreach (CharacterSetCount charSet in shortCount) {
					passwordCount *= Combinations(remainingLength, charSet.Count) * Math.Pow(charSet.Characters.Count, charSet.Count);
					remainingLength -= charSet.Count;
				}
				passwordCount *= Math.Pow(otherCharCount, remainingLength);
				total += passwordCount;
			}
			return total;
		}

		private static IEnumerable<List<CharacterSetCount>> ShortCounts(List<CharacterSetCount> charSets, int start = 0) {
			if (start >= charSets.Count) {
				yield return new List<CharacterSetCount>();
			} else {
				for (int n = 0; n < charSets[start].Count; n++) {
					foreach (List<CharacterSetCount> list in ShortCounts(charSets, start + 1)) {
						yield return list.Prepend(charSets[0].WithCount(n)).ToList();
					}
				}
			}
		}

		private static double Combinations(int supersetSize, int subsetSize) {
			double numerator = 1.0;
			for (int n = supersetSize; n > (supersetSize - subsetSize); n--) {
				numerator *= n;
			}
			double denominator = 1.0;
			for (int n = 2; n <= subsetSize; n++) {
				denominator *= n;
			}
			return numerator / denominator;
		}
	}

	class CharacterSetCount {
		public CharacterSetCount(HashSet<string> characters) {
			this.Characters = characters;
		}

		public HashSet<string> Characters { get; }

		public int Count { get; private set; } = 1;

		public CharacterSetCount WithCount(int count) => new CharacterSetCount(this.Characters) {
			Count = count
		};

		public void Increment() {
			this.Count++;
		}
	}
}
