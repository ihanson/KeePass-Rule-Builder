using System;
using System.Runtime.Serialization;

namespace RuleBuilder.Rule.Serialization {
	[DataContract]
	public class ExpirationContract {
		public ExpirationContract(Expiration expiration) {
			if (expiration == null) {
				throw new ArgumentNullException(nameof(expiration));
			}
			this.Unit = expiration.Unit;
			this.Length = expiration.Length;
		}

		[DataMember]
		public ExpirationUnit Unit { get; private set; }

		[DataMember]
		public int Length { get; private set; }

		public Expiration DeserializedObject() => new Expiration(this.Unit, this.Length);
	}
}
