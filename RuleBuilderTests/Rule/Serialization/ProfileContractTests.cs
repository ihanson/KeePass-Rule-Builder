﻿using KeePass.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Serialization.Tests {
	[TestClass()]
	public class ProfileContractTests {
		[TestMethod()]
		public void DefaultProfileTest() {
			ProfileContract profile = new ProfileContract(PasswordProfile.DefaultProfile);
			Assert.IsTrue(profile.IsDefault);
			Assert.IsNull(profile.Name);
		}
		[TestMethod()]
		public void DefaultProfileObjectTest() {
			PasswordProfile profile = new ProfileContract(PasswordProfile.DefaultProfile).Object();
			Assert.IsTrue(profile.IsDefaultProfile);
		}
		[TestMethod()]
		public void NamedProfileTest() {
			ProfileContract profile = new ProfileContract(new PasswordProfile(PasswordProfile.NamedProfile(ProfileName).Profile));
			Assert.IsFalse(profile.IsDefault);
			Assert.AreEqual(ProfileName, profile.Name);
		}
		[TestMethod()]
		public void NamedProfileObjectTest() {
			PasswordProfile profile = new ProfileContract(new PasswordProfile(PasswordProfile.NamedProfile(ProfileName).Profile)).Object();
			Assert.IsFalse(profile.IsDefaultProfile);
			Assert.AreEqual(ProfileName, profile.Name);
		}
		private static readonly string ProfileName = $"{KPRes.RandomMacAddress} ({KPRes.BuiltIn})";
	}
}