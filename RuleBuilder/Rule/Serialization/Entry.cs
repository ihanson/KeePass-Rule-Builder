using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using KeePassLib;
using KeePassLib.Security;

namespace RuleBuilder.Rule.Serialization {
	public static class Entry {
		private const string PasswordRuleKey = "Password Rule";

		public static IPasswordGenerator EntryDefaultGenerator(PwEntry entry) {
			if (entry == null) {
				throw new ArgumentNullException(nameof(entry));
			}
			IPasswordGenerator generator = EntryGenerator(entry);
			if (generator != null) {
				return generator;
			}
			PwGroup group = entry.ParentGroup;
			while (group != null) {
				generator = GroupGenerator(group);
				if (generator != null) {
					return generator;
				}
				group = group.ParentGroup;
			}
			return PasswordProfile.DefaultProfile;
		}

		public static IPasswordGenerator EntryGenerator(PwEntry entry) {
			if (entry == null) {
				throw new ArgumentNullException(nameof(entry));
			}
			try {
				ProtectedString generatorStr = entry.Strings.Get(PasswordRuleKey);
				return generatorStr != null ? DeserializedGenerator(generatorStr.ReadString()) : null;
#pragma warning disable CA1031 // Do not catch general exception types
			} catch {
#pragma warning restore CA1031 // Do not catch general exception types
				return null;
			}
		}

		public static void SetEntryGenerator(PwEntry entry, IPasswordGenerator generator) {
			if (entry == null) {
				throw new ArgumentNullException(nameof(entry));
			}
			if (generator is PasswordProfile && ((PasswordProfile)generator).IsDefaultProfile) {
				entry.Strings.Remove(PasswordRuleKey);
			} else {
				entry.Strings.Set(PasswordRuleKey, new ProtectedString(false, SerializedGenerator(generator)));
			}
		}

		public static IPasswordGenerator GroupGenerator(PwGroup group) {
			if (group == null) {
				throw new ArgumentNullException(nameof(group));
			}
			try {
				string generatorStr = group.CustomData.Get(PasswordRuleKey);
				return generatorStr != null ? DeserializedGenerator(generatorStr) : null;
#pragma warning disable CA1031 // Do not catch general exception types
			} catch {
#pragma warning restore CA1031 // Do not catch general exception types
				return null;
			}
		}

		public static void SetGroupGenerator(PwGroup group, IPasswordGenerator generator) {
			if (group == null) {
				throw new ArgumentNullException(nameof(group));
			}
			if (generator is PasswordProfile && ((PasswordProfile)generator).IsDefaultProfile) {
				group.CustomData.Remove(PasswordRuleKey);
			} else {
				group.CustomData.Set(PasswordRuleKey, SerializedGenerator(generator));
			}
		}

		private static string SerializedGenerator(IPasswordGenerator generator) {
			using (StreamReader reader = new StreamReader(new MemoryStream(), Encoding.UTF8)) {
				new DataContractJsonSerializer(typeof(ConfigurationContract)).WriteObject(reader.BaseStream, new ConfigurationContract(generator));
				reader.BaseStream.Position = 0;
				return reader.ReadToEnd();
			}
		}
		private static IPasswordGenerator DeserializedGenerator(string generatorStr) {
			using (StreamWriter writer = new StreamWriter(new MemoryStream(), new UTF8Encoding(false)) {
				AutoFlush = true
			}) {
				writer.Write(generatorStr);
				writer.BaseStream.Position = 0;
				return ((ConfigurationContract)new DataContractJsonSerializer(typeof(ConfigurationContract)).ReadObject(writer.BaseStream)).DeserializedObject();
			}
		}
	}
}
