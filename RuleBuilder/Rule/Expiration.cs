namespace RuleBuilder.Rule {
	public class Expiration {
		public Expiration(ExpirationUnit unit, int length) {
			this.Unit = unit;
			this.Length = length;
		}

		public ExpirationUnit Unit { get; set; }

		public int Length { get; set; }
	}

	public enum ExpirationUnit {
		Days = 0,
		Weeks = 1,
		Months = 2,
		Years = 3
	}
}
