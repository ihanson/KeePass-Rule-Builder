using KeePass.Resources;
using KeePassLib.Cryptography.PasswordGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RuleBuilder.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RuleBuilder.Rule.Tests {
	[TestClass()]
	public class PasswordProfileTests {
		[TestMethod()]
		public void PasswordProfileTest() {
			PwProfile profile = new PwProfile();
			Assert.AreSame(profile, new PasswordProfile(profile).Profile);
		}

		[TestMethod()]
		public void NamedProfileTest() {
			Assert.AreEqual(ProfileName, PasswordProfile.NamedProfile(ProfileName).Name);
		}
		[TestMethod()]
		public void NamedProfileNotFoundTest() {
			Assert.IsTrue(PasswordProfile.NamedProfile("xxxxxxxxxxxxx not a real profile xxxxxxxxxxxxx").IsDefaultProfile);
		}

		[TestMethod()]
		public void NewPasswordTest() {
			Assert.IsTrue(Regex.IsMatch(PasswordProfile.NamedProfile(ProfileName).NewPassword(), @"(?:[\dA-F]{2}-){5}[\dA-F]{2}"));
		}
		private static readonly string ProfileName = $"{KPRes.RandomMacAddress} ({KPRes.BuiltIn})";
	}
}