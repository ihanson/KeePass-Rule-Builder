using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using KeePass.Util;
using KeePassLib.Cryptography.PasswordGenerator;
using RuleBuilder.Rule;

namespace RuleBuilder.Forms {
	public class EditRuleModel : RuleProperty {
		public EditRuleModel(Configuration config) {
			this.Configuration = config ?? throw new ArgumentNullException(nameof(config));
			this.RuleType = config.Generator is Rule.PasswordRule ? RuleType.Rule : RuleType.Profile;
			this.PasswordRule = (config.Generator as Rule.PasswordRule)?.Clone() ?? new Rule.PasswordRule();
			this.SelectedProfileIndex = this.ProfileIndex((config.Generator as Rule.PasswordProfile)?.Clone() ?? Rule.PasswordProfile.DefaultProfile);
			this.PasswordRule.RuleChanged += () => this.NotifyRuleChanged();
		}

		public List<Rule.PasswordProfile> Profiles { get; } = new Rule.PasswordProfile[] {
			Rule.PasswordProfile.DefaultProfile
		}.Concat(
			PwGeneratorUtil.GetAllProfiles(false).Select((PwProfile profile) => new Rule.PasswordProfile(profile))
		).ToList();

		private int _selectedProfileIndex;
		public int SelectedProfileIndex {
			get => this._selectedProfileIndex;
			set {
				if (this._selectedProfileIndex != value) {
					this._selectedProfileIndex = value;
					this.NotifyPropertyChanged(nameof(this.SelectedProfileIndex));
				}
			}
		}

		private RuleType _ruleType;
		public RuleType RuleType {
			get => this._ruleType;
			set {
				if (this._ruleType != value) {
					this._ruleType = value;
					this.NotifyPropertyChanged(nameof(this.RuleType));
				}
			}
		}

		public Rule.PasswordRule PasswordRule { get; }

		private bool _passwordExpires;
		public bool PasswordExpires {
			get => this._passwordExpires;
			set {
				if (this._passwordExpires != value) {
					this._passwordExpires = value;
					this.NotifyPropertyChanged(nameof(this.PasswordExpires), false);
				}
			}
		}

		private int _expirationLength;
		public int ExpirationLength {
			get => this._expirationLength;
			set {
				if (this._expirationLength != value) {
					this._expirationLength = value;
					this.NotifyPropertyChanged(nameof(this.ExpirationLength), false);
				}
			}
		}

		private ExpirationUnit _expirationUnit;
		public int ExpirationUnit {
			get => (int)this._expirationUnit;
			set {
				ExpirationUnit unit = (ExpirationUnit)value;
				if (this._expirationUnit != unit) {
					this._expirationUnit = unit;
					this.NotifyPropertyChanged(nameof(this.ExpirationUnit), false);
				}
			}
		}

		private Configuration _configuration;
		public Configuration Configuration {
			get {
				this._configuration.Generator = this.RuleType == RuleType.Rule
					? (IPasswordGenerator)this.PasswordRule
					: this.Profiles[this.SelectedProfileIndex];
				this._configuration.Expiration = this.PasswordExpires
					? new Expiration((ExpirationUnit)this.ExpirationUnit, this.ExpirationLength)
					: null;
				return this._configuration;
			}
			set {
				this._configuration = value;
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
