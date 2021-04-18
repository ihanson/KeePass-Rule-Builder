using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuleBuilder.Rule;

namespace RuleBuilderTests.Rule {
	[TestClass]
	public class ExpirationTests {
		[TestMethod]
		public void ExpirationTest() {
			Expiration expiration = new Expiration(ExpirationUnit.Days, 20);
			Assert.AreEqual(ExpirationUnit.Days, expiration.Unit);
			Assert.AreEqual(20, expiration.Length);
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Expiration((ExpirationUnit)300, 300));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Expiration(ExpirationUnit.Months, 0));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Expiration(ExpirationUnit.Months, 1000));
		}

		[TestMethod]
		public void DateFromTest() {
			DateTime origin = new DateTime(2021, 5, 31);
			Assert.AreEqual(new DateTime(2021, 6, 12), new Expiration(ExpirationUnit.Days, 12).DateFrom(origin));
			Assert.AreEqual(new DateTime(2021, 10, 18), new Expiration(ExpirationUnit.Weeks, 20).DateFrom(origin));
			Assert.AreEqual(new DateTime(2021, 6, 30), new Expiration(ExpirationUnit.Months, 1).DateFrom(origin));
			Assert.AreEqual(new DateTime(2021, 7, 31), new Expiration(ExpirationUnit.Months, 2).DateFrom(origin));
			Assert.AreEqual(new DateTime(2022, 2, 28), new Expiration(ExpirationUnit.Months, 9).DateFrom(origin));
			Assert.AreEqual(new DateTime(2022, 5, 31), new Expiration(ExpirationUnit.Years, 1).DateFrom(origin));
			Assert.AreEqual(DateTime.MaxValue, new Expiration(ExpirationUnit.Years, 100).DateFrom(new DateTime(9990, 1, 1)));
		}
	}
}
