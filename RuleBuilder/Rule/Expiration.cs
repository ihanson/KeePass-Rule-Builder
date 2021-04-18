using System;

namespace RuleBuilder.Rule {
	public class Expiration {
		public Expiration(ExpirationUnit unit, int length) {
			if (!Enum.IsDefined(typeof(ExpirationUnit), unit)) {
				throw new ArgumentOutOfRangeException(nameof(unit));
			}
			if (length < 1 || length > 999) {
				throw new ArgumentOutOfRangeException(nameof(length));
			}
			this.Unit = unit;
			this.Length = length;
		}

		public ExpirationUnit Unit { get; set; }

		public int Length { get; set; }

		public DateTime DateFromToday() {
			try {
				switch (this.Unit) {
					case ExpirationUnit.Days:
						return DateTime.Today.AddDays(this.Length);
					case ExpirationUnit.Weeks:
						return DateTime.Today.AddDays(this.Length * 7);
					case ExpirationUnit.Months:
						return DateTime.Today.AddMonths(this.Length);
					case ExpirationUnit.Years:
						return DateTime.Today.AddYears(this.Length);
					default:
						throw new ArgumentException("Invalid unit");
				}
			} catch (ArgumentOutOfRangeException) {
				return DateTime.MaxValue;
			}
		}
	}

	public enum ExpirationUnit {
		Days = 0,
		Weeks = 1,
		Months = 2,
		Years = 3
	}
}
