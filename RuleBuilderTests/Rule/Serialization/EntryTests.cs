using System.Collections.Generic;
using System.Collections.ObjectModel;
using KeePass.Resources;
using KeePassLib;
using KeePassLib.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Serialization.Tests {
	[TestClass]
	public class EntryTests {
		private const string PasswordRuleKey = "Password Rule";
		private static readonly string ProfileName = $"{KPRes.RandomMacAddress} ({KPRes.BuiltIn})";
		[TestMethod]
		public void DecodeDefaultGeneratorTest() {
			PasswordProfile profile = (PasswordProfile)GeneratorFromJSON(null);
			Assert.IsTrue(profile.IsDefaultProfile);
		}
		[TestMethod]
		public void DecodeInvalidJSONTest() {
			PasswordProfile profile = (PasswordProfile)GeneratorFromJSON(null);
			Assert.IsTrue(profile.IsDefaultProfile);
		}
		[TestMethod]
		public void DecodeDefaultProfileTest() {
			PasswordProfile profile = (PasswordProfile)GeneratorFromJSON("{Profile:{IsDefault:true}");
			Assert.IsTrue(profile.IsDefaultProfile);
		}
		[TestMethod]
		public void EncodeDefaultProfileTest() {
			PasswordProfile profile = (PasswordProfile)EncodeDecodeGenerator(PasswordProfile.DefaultProfile);
			Assert.IsTrue(profile.IsDefaultProfile);
		}
		[TestMethod]
		public void DecodeNamedProfileTest() {
			PasswordProfile profile = (PasswordProfile)GeneratorFromJSON($"{{\"Profile\":{{\"Name\":{EncodeJSONString(ProfileName)}}}}}");
			Assert.IsFalse(profile.IsDefaultProfile);
			Assert.AreEqual(ProfileName, profile.Name);
			Assert.AreEqual(ProfileName, profile.Profile.Name);
		}
		[TestMethod]
		public void EncodeNamedProfileTest() {
			PasswordProfile profile = (PasswordProfile)EncodeDecodeGenerator(new PasswordProfile(PasswordProfile.NamedProfile(ProfileName).Profile));
			Assert.IsFalse(profile.IsDefaultProfile);
			Assert.AreEqual(ProfileName, profile.Name);
			Assert.AreEqual(ProfileName, profile.Profile.Name);
		}
		[TestMethod]
		public void DecodeRuleTest() {
			PasswordRule rule = (PasswordRule)GeneratorFromJSON("{\"Rule\":{\"Length\":32,\"Components\":[{\"CharacterSet\":{\"CharacterClass\":2},\"Required\":true},{\"CharacterSet\":{\"Characters\":\"xyz\"}}],\"Exclude\":\"abc\"}}");
			Assert.AreEqual(32, rule.Length);
			Assert.AreEqual(2, rule.Components.Count);
			Assert.AreEqual(CharacterClassEnum.Letters, rule.Components[0].CharacterClass.Enumeration);
			Assert.IsTrue(rule.Components[0].Required);
			Assert.AreEqual(CharacterClassEnum.Custom, rule.Components[1].CharacterClass.Enumeration);
			Assert.AreEqual("xyz", rule.Components[1].CharacterClass.Characters);
			Assert.IsFalse(rule.Components[1].Required);
			Assert.AreEqual("abc", rule.ExcludeCharacters);
		}
		[TestMethod]
		public void EncodeRuleTest() {
			PasswordRule rule = (PasswordRule)EncodeDecodeGenerator(new PasswordRule() {
				Length = 32,
				Components = new ObservableCollection<Component>() {
					new Component(CharacterClass.Letters, true),
					new Component(new CharacterClass("xyz"), false)
				},
				ExcludeCharacters = "abc"
			});
			Assert.AreEqual(32, rule.Length);
			Assert.AreEqual(2, rule.Components.Count);
			Assert.AreEqual(CharacterClassEnum.Letters, rule.Components[0].CharacterClass.Enumeration);
			Assert.IsTrue(rule.Components[0].Required);
			Assert.AreEqual(CharacterClassEnum.Custom, rule.Components[1].CharacterClass.Enumeration);
			Assert.AreEqual("xyz", rule.Components[1].CharacterClass.Characters);
			Assert.IsFalse(rule.Components[1].Required);
			Assert.AreEqual("abc", rule.ExcludeCharacters);
		}
		private static string EncodeJSONString(string str) => $"\"{str.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
		private static IPasswordGenerator GeneratorFromJSON(string json) {
			PwEntry entry = new PwEntry(true, true);
			if (json != null) {
				entry.Strings.Set(PasswordRuleKey, new ProtectedString(false, json));
			}
			return Entry.EntryGenerator(entry);
		}
		private static IPasswordGenerator EncodeDecodeGenerator(IPasswordGenerator generator) {
			PwEntry entry = new PwEntry(true, true);
			Entry.SetEntryGenerator(entry, generator);
			return Entry.EntryGenerator(entry);
		}
	}
}