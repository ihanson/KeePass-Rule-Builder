using System.Collections.Generic;
using System.Linq;
using KeePass.Util;
using KeePassLib.Cryptography.PasswordGenerator;
using RuleBuilder.Rule;

namespace RuleBuilder.Forms {
	public class EditRuleModel : RuleProperty {
		public EditRuleModel(Rule.IPasswordGenerator generator) {
			this.SelectedGenerator = generator;
			this.RuleType = generator is Rule.PasswordRule ? RuleType.Rule : RuleType.Profile;
			this.PasswordRule = (generator as Rule.PasswordRule)?.Clone() ?? new Rule.PasswordRule();
			this.SelectedProfileIndex = this.ProfileIndex((generator as Rule.PasswordProfile)?.Clone() ?? Rule.PasswordProfile.DefaultProfile);
			this.PasswordRule.RuleChanged += () => this.NotifyRuleChanged();
		}

		public List<Rule.PasswordProfile> Profiles { get; } = new Rule.PasswordProfile[] {
			Rule.PasswordProfile.DefaultProfile
		}.Concat(
			PwGeneratorUtil.GetAllProfiles(false).Select((PwProfile profile) => new Rule.PasswordProfile(profile))
		).ToList();

		private int __selectedProfileIndex;
		public int SelectedProfileIndex {
			get => this.__selectedProfileIndex;
			set {
				if (this.__selectedProfileIndex != value) {
					this.__selectedProfileIndex = value;
					this.NotifyPropertyChanged(nameof(this.SelectedProfileIndex));
				}
			}
		}

		private RuleType __ruleType;
		public RuleType RuleType {
			get => this.__ruleType;
			set {
				if (this.__ruleType != value) {
					this.__ruleType = value;
					this.NotifyPropertyChanged(nameof(this.RuleType));
				}
			}
		}

		public Rule.PasswordRule PasswordRule { get; }

		public Rule.IPasswordGenerator SelectedGenerator { get; set; }

		public Rule.IPasswordGenerator Generator() {
			if (this.RuleType == RuleType.Rule) {
				return this.PasswordRule;
			} else {
				return this.Profiles[this.SelectedProfileIndex];
			}
		}

		private int ProfileIndex(Rule.PasswordProfile profile) {
			if (profile.IsDefaultProfile) {
				return 0;
			} else {
				int index = this.Profiles.FindIndex((Rule.PasswordProfile p) => p.Name == profile.Name);
				return index != -1 ? index : 0;
			}
		}
	}

	public enum RuleType {
		Rule,
		Profile
	}
}
