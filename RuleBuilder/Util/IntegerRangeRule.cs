using System;
using System.Globalization;
using System.Windows.Controls;

namespace RuleBuilder.Util {
	class IntegerRangeRule : ValidationRule {
		public int? Min { get; set; }
		public int? Max { get; set; }
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
			try {
				int valInt = int.Parse((string)value);
				if (!this.ValueInRange(valInt)) {
					return new ValidationResult(false, null);
				}
			} catch (FormatException) {
				return new ValidationResult(false, null);
			}
			return ValidationResult.ValidResult;
		}
		private bool ValueInRange(int value) {
			if (this.Min != null && value < this.Min) {
				return false;
			}
			if (this.Max != null && value > this.Max) {
				return false;
			}
			return true;
		}
	}
}
