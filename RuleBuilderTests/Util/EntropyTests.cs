using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuleBuilder.Rule;
using RuleBuilder.Util;

namespace RuleBuilderTests.Util {
	[TestClass]
	public class EntropyTests {
		[TestMethod]
		public void SinglePasswordTest() {
			Assert.AreEqual(0.0, Entropy.EntropyBits(
					new PasswordRule(32, new Component[] {
						new Component(new CharacterClass("0"), false)
					}, string.Empty)
			));
		}

		[TestMethod]
		public void PowerOf2Test() {
			Assert.AreEqual(32.0, Entropy.EntropyBits(
				new PasswordRule(32, new Component[] {
					new Component(new CharacterClass("01"), false)
				}, string.Empty)
			));

			Assert.AreEqual(64.0, Entropy.EntropyBits(
				new PasswordRule(32, new Component[] {
					new Component(new CharacterClass("0123"), false)
				}, string.Empty)
			));
		}

		[TestMethod]
		public void TooManyRequiredTest() {
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => Entropy.EntropyBits(
				new PasswordRule(32, new Component[] {
					new Component(new CharacterClass("a"), true),
					new Component(new CharacterClass("b"), true),
					new Component(new CharacterClass("c"), true),
					new Component(new CharacterClass("d"), true),
					new Component(new CharacterClass("e"), true),
					new Component(new CharacterClass("f"), true),
					new Component(new CharacterClass("g"), true),
				}, string.Empty)
			));
		}

		[TestMethod]
		public void NoneRequiredTest() {
			Assert.AreEqual(8.0, Entropy.EntropyBits(
				new PasswordRule(4, new Component[] {
					new Component(new CharacterClass("0"), false),
					new Component(new CharacterClass("1"), false),
					new Component(new CharacterClass("2"), false),
					new Component(new CharacterClass("3"), false)
				}, string.Empty)
			));
		}

		[TestMethod]
		public void OneRequiredTest() {
			Assert.AreEqual(3.907, Entropy.EntropyBits(
				new PasswordRule(4, new Component[] {
					new Component(new CharacterClass("0"), false),
					new Component(new CharacterClass("1"), true)
				}, string.Empty)
			), 0.001);
		}

		[TestMethod]
		public void AllRequiredTest() {
			Assert.AreEqual(4.585, Entropy.EntropyBits(
				new PasswordRule(4, new Component[] {
					new Component(new CharacterClass("0"), true),
					new Component(new CharacterClass("1"), true),
					new Component(new CharacterClass("2"), true),
					new Component(new CharacterClass("3"), true)
				}, string.Empty)
			), 0.001);
		}

		[TestMethod]
		public void RandomPositionTest() {
			Assert.AreEqual(Entropy.EntropyBits(
				new PasswordRule(4, new Component[] {
					new Component(new CharacterClass("0"), true),
					new Component(new CharacterClass("1"), true),
					new Component(new CharacterClass("1"), true),
					new Component(new CharacterClass("1"), true)
				}, string.Empty)
			), 2.0);
		}

		[TestMethod]
		public void TooManyCharactersTest() {
			double power = Math.Floor(Math.Log10(double.MaxValue));

			Assert.AreEqual(power / Math.Log10(2.0), Entropy.EntropyBits(
				new PasswordRule((int)power, new Component[] {
					new Component(new CharacterClass("0123456789"), false)
				}, string.Empty)
			), 0.001);

			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => Entropy.EntropyBits(
				new PasswordRule((int)power + 1, new Component[] {
					new Component(new CharacterClass("0123456789"), false)
				}, string.Empty)
			));
		}

		[TestMethod]
		public void DuplicateSetTest() {
			_ = Entropy.EntropyBits(new PasswordRule(
				16,
				Enumerable.Repeat(new Component(new CharacterClass("0123"), true), 8)
					.Concat(Enumerable.Repeat(new Component(new CharacterClass("4567"), true), 8)),
				string.Empty
			));
		}

		[TestMethod]
		public void ExcludedRequirementTest() {
			Assert.AreEqual(0.0, Entropy.EntropyBits(
				new PasswordRule(4, new Component[] {
					new Component(new CharacterClass("0"), true),
					new Component(new CharacterClass("1"), true)
				}, "1")
			));
		}

		[TestMethod]
		public void DefaultComponentTest() {
			Assert.IsTrue(Entropy.EntropyBits(
				new PasswordRule(4, Array.Empty<Component>(), string.Empty)
			) > 0.0);
		}
	}
}
