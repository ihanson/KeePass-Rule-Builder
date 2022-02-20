using System.Collections.ObjectModel;
using KeePass.Resources;
using KeePassLib;
using KeePassLib.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Serialization.Tests {
	[TestClass]
	public class EntryTests {
		private const string PasswordRuleKey = "Password Rule";
		private static readonly string ProfileName = $"{KPRes.MacAddress} ({KPRes.BuiltIn})";

		[TestMethod]
		public void DecodeDefaultGeneratorTest() {
			PasswordProfile profile = (PasswordProfile)ConfigurationFromJSON(null).Generator;
			Assert.IsTrue(profile.IsDefaultProfile);
		}

		[TestMethod]
		public void DecodeInvalidJSONTest() {
			PasswordProfile profile = (PasswordProfile)ConfigurationFromJSON("{").Generator;
			Assert.IsTrue(profile.IsDefaultProfile);
		}

		[TestMethod]
		public void DecodeDefaultProfileTest() {
			PasswordProfile profile = (PasswordProfile)ConfigurationFromJSON("{Profile:{IsDefault:true}").Generator;
			Assert.IsTrue(profile.IsDefaultProfile);
		}

		[TestMethod]
		public void EncodeDefaultProfileTest() {
			PasswordProfile profile = (PasswordProfile)EncodeDecodeConfiguration(new Configuration()).Generator;
			Assert.IsTrue(profile.IsDefaultProfile);
		}

		[TestMethod]
		public void DecodeNamedProfileTest() {
			PasswordProfile profile = (PasswordProfile)ConfigurationFromJSON($"{{\"Profile\":{{\"Name\":{EncodeJSONString(ProfileName)}}}}}").Generator;
			Assert.IsFalse(profile.IsDefaultProfile);
			Assert.AreEqual(ProfileName, profile.Name);
			Assert.AreEqual(ProfileName, profile.Profile.Name);
		}

		[TestMethod]
		public void EncodeNamedProfileTest() {
			PasswordProfile profile = (PasswordProfile)EncodeDecodeConfiguration(new Configuration(
				new PasswordProfile(PasswordProfile.NamedProfile(ProfileName).Profile),
				new Expiration(ExpirationUnit.Months, 3)
			)).Generator;
			Assert.IsFalse(profile.IsDefaultProfile);
			Assert.AreEqual(ProfileName, profile.Name);
			Assert.AreEqual(ProfileName, profile.Profile.Name);
		}

		[TestMethod]
		public void DecodeRuleTest() {
			Configuration config = ConfigurationFromJSON(
				"{\"Rule\":{\"Length\":32,\"Components\":[{\"CharacterSet\":{\"CharacterClass\":2},\"Required\":true},{\"CharacterSet\":{\"Characters\":\"xyz\"}}],\"Exclude\":\"abc\"},\"Expiration\":{\"Unit\":2,\"Length\":3}"
			);
			PasswordRule rule = (PasswordRule)config.Generator;
			Assert.AreEqual(32, rule.Length);
			Assert.AreEqual(2, rule.Components.Count);
			Assert.AreEqual(CharacterClassEnum.Letters, rule.Components[0].CharacterClass.Enumeration);
			Assert.IsTrue(rule.Components[0].Required);
			Assert.AreEqual(CharacterClassEnum.Custom, rule.Components[1].CharacterClass.Enumeration);
			Assert.AreEqual("xyz", rule.Components[1].CharacterClass.Characters);
			Assert.IsFalse(rule.Components[1].Required);
			Assert.AreEqual("abc", rule.ExcludeCharacters);
			Assert.AreEqual(ExpirationUnit.Months, config.Expiration.Unit);
			Assert.AreEqual(3, config.Expiration.Length);
		}

		[TestMethod]
		public void EncodeRuleTest() {
			Configuration config = EncodeDecodeConfiguration(new Configuration(
				new PasswordRule(
					32,
					new ObservableCollection<Component>() {
						new Component(CharacterClass.Letters, true),
						new Component(new CharacterClass("xyz"), false)
					},
					"abc"
				),
				new Expiration(ExpirationUnit.Months, 3)
			));
			PasswordRule rule = (PasswordRule)config.Generator;
			Assert.AreEqual(32, rule.Length);
			Assert.AreEqual(2, rule.Components.Count);
			Assert.AreEqual(CharacterClassEnum.Letters, rule.Components[0].CharacterClass.Enumeration);
			Assert.IsTrue(rule.Components[0].Required);
			Assert.AreEqual(CharacterClassEnum.Custom, rule.Components[1].CharacterClass.Enumeration);
			Assert.AreEqual("xyz", rule.Components[1].CharacterClass.Characters);
			Assert.IsFalse(rule.Components[1].Required);
			Assert.AreEqual("abc", rule.ExcludeCharacters);
			Assert.AreEqual(ExpirationUnit.Months, config.Expiration.Unit);
			Assert.AreEqual(3, config.Expiration.Length);
		}

		[TestMethod]
		public void GroupRuleTest() {
			PwGroup group1 = new PwGroup();
			PwGroup group2 = new PwGroup();
			PwGroup group3 = new PwGroup();
			PwEntry entry = new PwEntry(true, true);

			group1.AddGroup(group2, true);
			group2.AddGroup(group3, true);
			group3.AddEntry(entry, true);
			Entry.SetGroupConfiguration(
				group1,
				new Configuration(
					new PasswordRule(
						4,
						new Component[] { new Component(new CharacterClass("a"), false) },
						string.Empty
					)
				)
			);

			PasswordRule rule = (PasswordRule)Entry.EntryDefaultConfiguration(entry).Generator;
			Assert.AreEqual("a", rule.Components[0].CharacterClass.Characters);
		}

		[TestMethod]
		public void GroupRuleOverrideTest() {
			PwGroup group1 = new PwGroup();
			PwGroup group2 = new PwGroup();
			PwGroup group3 = new PwGroup();
			PwEntry entry = new PwEntry(true, true);

			group1.AddGroup(group2, true);
			group2.AddGroup(group3, true);
			group3.AddEntry(entry, true);
			Entry.SetGroupConfiguration(
				group1,
				new Configuration(
					new PasswordRule(
						4,
						new Component[] { new Component(new CharacterClass("a"), false) },
						string.Empty
					)
				)
			);
			Entry.SetEntryConfiguration(
				entry,
				new Configuration(
					new PasswordRule(
						4,
						new Component[] { new Component(new CharacterClass("b"), false) },
						string.Empty
					)
				)
			);

			PasswordRule rule = (PasswordRule)Entry.EntryDefaultConfiguration(entry).Generator;
			Assert.AreEqual("b", rule.Components[0].CharacterClass.Characters);
		}

		[TestMethod]
		public void GroupRuleDefaultTest() {
			PwGroup group1 = new PwGroup();
			PwGroup group2 = new PwGroup();
			PwGroup group3 = new PwGroup();
			PwEntry entry = new PwEntry(true, true);

			group1.AddGroup(group2, true);
			group2.AddGroup(group3, true);
			group3.AddEntry(entry, true);

			PasswordProfile profile = (PasswordProfile)Entry.EntryDefaultConfiguration(entry).Generator;
			Assert.IsTrue(profile.IsDefaultProfile);
		}

		private static string EncodeJSONString(string str) => $"\"{str.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";

		private static Configuration ConfigurationFromJSON(string json) {
			PwEntry entry = new PwEntry(true, true);
			if (json != null) {
				entry.Strings.Set(PasswordRuleKey, new ProtectedString(false, json));
			}
			return Entry.EntryDefaultConfiguration(entry);
		}

		private static Configuration EncodeDecodeConfiguration(Configuration config) {
			PwEntry entry = new PwEntry(true, true);
			Entry.SetEntryConfiguration(entry, config);
			return Entry.EntryDefaultConfiguration(entry);
		}
	}
}