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

		public ExpirationUnit Unit { get; }

		public int Length { get; }

		public DateTime DateFrom(DateTime origin) {
			try {
				switch (this.Unit) {
					case ExpirationUnit.Days:
						return origin.AddDays(this.Length);
					case ExpirationUnit.Weeks:
						return origin.AddDays(this.Length * 7);
					case ExpirationUnit.Months:
						return origin.AddMonths(this.Length);
					case ExpirationUnit.Years:
						return origin.AddYears(this.Length);
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
