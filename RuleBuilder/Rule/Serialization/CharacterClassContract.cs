using System;
using System.Runtime.Serialization;

namespace RuleBuilder.Rule.Serialization {
	[DataContract]
	public class CharacterClassContract {
		public CharacterClassContract(CharacterClass namedSet) {
			if (namedSet == null) {
				throw new ArgumentNullException(nameof(namedSet));
			}
			this.CharacterClass = namedSet.Enumeration;
			if (this.CharacterClass == CharacterClassEnum.Custom) {
				this.Characters = namedSet.Characters;
			}
		}

		[DataMember(EmitDefaultValue = false)]
		public string Characters { get; private set; }

		[DataMember]
		public CharacterClassEnum CharacterClass { get; private set; }

		public CharacterClass DeserializedObject() => this.CharacterClass == CharacterClassEnum.Custom ?
			new CharacterClass(this.Characters)
			: Rule.CharacterClass.EnumeratedCharacterClass(this.CharacterClass);
	}
}