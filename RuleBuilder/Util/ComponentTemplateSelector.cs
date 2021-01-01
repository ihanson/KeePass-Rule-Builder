using System.Windows;
using System.Windows.Controls;

namespace RuleBuilder.Util {
	public class ComponentTemplateSelector : DataTemplateSelector {
		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			Rule.Component component = item as Rule.Component;
			if (component == null) {
				return null;
			}
			if (component.CharacterClass == null) {
				return this.BlankCell;
			}
			return component.CharacterClass?.Enumeration == Rule.CharacterClassEnum.Custom && this.CustomCharacterCell != null
				? this.CustomCharacterCell
				: this.CharacterClassCell;
		}
		public DataTemplate CharacterClassCell { get; set; }
		public DataTemplate CustomCharacterCell { get; set; }
		public DataTemplate BlankCell { get; set; }
	}
}
