﻿using KeePass.Resources;
using KeePass.Util;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;

namespace RuleBuilder.Rule {
	public class PasswordProfile : IPasswordGenerator {
		public PasswordProfile(PwProfile profile) {
			this.Profile = profile;
		}

		public static PasswordProfile DefaultProfile => new PasswordProfile(KeePass.Program.Config.PasswordGenerator.AutoGeneratedPasswordsProfile) {
			IsDefaultProfile = true
		};
		public string Name => this.IsDefaultProfile ? KPRes.AutoGeneratedPasswordSettings : this.Profile.Name;
		public bool IsDefaultProfile { get; private set; }
		public PwProfile Profile { get; }
		public static PasswordProfile NamedProfile(string key) {
			foreach (PwProfile profile in PwGeneratorUtil.GetAllProfiles(false)) {
				if (profile.Name == key) {
					return new PasswordProfile(profile);
				}
			}
			return DefaultProfile;
		}
		public string NewPassword() {
			_ = PwGenerator.Generate(out ProtectedString result, this.Profile, null, KeePass.Program.PwGeneratorPool);
			return result.ReadString();
		}
	}
}