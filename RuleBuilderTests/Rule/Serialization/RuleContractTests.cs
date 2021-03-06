﻿using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RuleBuilder.Rule.Serialization.Tests {
	[TestClass]
	public class RuleContractTests {
		[TestMethod]
		public void RuleContractTest() {
			RuleContract rule = new RuleContract(new PasswordRule(
				32,
				new ObservableCollection<Component>() {
					new Component(CharacterClass.Letters, false)
				},
				"x"
			));
			Assert.AreEqual(32, rule.Length);
			Assert.AreEqual(1, rule.Components.Count);
			Assert.AreEqual("x", rule.Exclude);
		}
		[TestMethod]
		public void RuleContractObjectTest() {
			PasswordRule rule = new RuleContract(new PasswordRule(
				32,
				new ObservableCollection<Component>() {
					new Component(CharacterClass.Letters, false)
				},
				"x"
			)).DeserializedObject();
			Assert.AreEqual(32, rule.Length);
			Assert.AreEqual(1, rule.Components.Count);
			Assert.AreEqual("x", rule.ExcludeCharacters);
		}
		[TestMethod]
		public void NegativeLengthTest() {
			_ = Assert.ThrowsException<SerializationException>(() => {
				_ = new RuleContract(new PasswordRule() {
					Length = -1
				}).DeserializedObject();
			});
		}
	}
}