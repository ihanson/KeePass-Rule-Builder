using System.ComponentModel;

namespace RuleBuilder.Rule {
	public delegate void RuleChangedDelegate();
	public abstract class RuleProperty : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged = (_1, _2) => { };
		public event RuleChangedDelegate RuleChanged = () => { };
		public void NotifyPropertyChanged(string property, bool affectsRule = true) {
			this.PropertyChanged(this, new PropertyChangedEventArgs(property));
			if (affectsRule) {
				this.RuleChanged();
			}
		}
		public void NotifyRuleChanged() {
			this.RuleChanged();
		}
	}
}
