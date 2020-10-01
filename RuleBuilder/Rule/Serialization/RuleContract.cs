using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using RuleBuilder.Properties;

namespace RuleBuilder.Rule.Serialization {
	[DataContract]
	public class RuleContract {
		public RuleContract(PasswordRule rule) {
			if (rule == null) {
				throw new ArgumentNullException(nameof(rule));
			}
			this.Length = rule.Length;
			this.Components = rule.Components
				.Where((Component component) => component.CharacterClass != null)
				.Select((Component component) => new ComponentContract(component))
				.ToList();
			this.Exclude = rule.ExcludeCharacters;
		}
		[DataMember]
		public int Length { get; private set; }
		[DataMember]
		public List<ComponentContract> Components { get; private set; }
		[DataMember]
		public string Exclude { get; private set; }
		public PasswordRule DeserializedObject() {
			if (this.Length < 0) {
				throw new SerializationException(Resources.PasswordLengthMustNotBeNegative);
			}
			return new PasswordRule() {
				Length = this.Length,
				Components = new ObservableCollection<Component>(this.Components.ConvertAll((ComponentContract component) => component.DeserializedObject())),
				ExcludeCharacters = this.Exclude
			};
		}
	}
}
