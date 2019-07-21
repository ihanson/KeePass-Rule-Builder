using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Tests {
	[TestClass]
	public class RandomTests {
		[TestMethod]
		public void RandomItemTest() {
			HashSet<int> numbers = new HashSet<int> { 1, 2, 3, 4, 5 };
			Assert.IsTrue(numbers.Contains(Random.RandomItem(numbers)));
		}
		[TestMethod]
		public void RandomItemFailTest() {
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
				_ = Random.RandomItem(new HashSet<int> { });
			});
		}

		[TestMethod]
		public void ShuffleTest() {
			List<int> numbers = new List<int>(100);
			for (int i = 0; i < 100; i++) {
				numbers.Add(i);
			}
			List<int> shuffled = new List<int>(numbers);
			Random.Shuffle(shuffled);
			Assert.AreEqual(numbers.Count, shuffled.Count);
			for (int i = 0; i < numbers.Count; i++) {
				Assert.IsTrue(shuffled.Contains(numbers[i]));
			}
		}
	}
}