using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using KeePassLib;
using KeePassLib.Security;

namespace RuleBuilder.Rule.Serialization {
	public static class Entry {
		private const string PasswordRuleKey = "Password Rule";

		public static Configuration EntryDefaultConfiguration(PwEntry entry) {
			if (entry == null) {
				throw new ArgumentNullException(nameof(entry));
			}
			Configuration config = EntryConfiguration(entry);
			if (config != null) {
				return config;
			}
			PwGroup group = entry.ParentGroup;
			while (group != null) {
				config = GroupConfiguration(group);
				if (config != null) {
					return config;
				}
				group = group.ParentGroup;
			}
			return new Configuration();
		}

		public static Configuration EntryConfiguration(PwEntry entry) {
			if (entry == null) {
				throw new ArgumentNullException(nameof(entry));
			}
			try {
				ProtectedString configStr = entry.Strings.Get(PasswordRuleKey);
				return configStr != null ? DeserializedConfiguration(configStr.ReadString()) : null;
#pragma warning disable CA1031 // Do not catch general exception types
			} catch {
#pragma warning restore CA1031 // Do not catch general exception types
				return null;
			}
		}

		public static void SetEntryConfiguration(PwEntry entry, Configuration config) {
			if (entry == null) {
				throw new ArgumentNullException(nameof(entry));
			}
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			if (config.Expiration == null && config.Generator is PasswordProfile && ((PasswordProfile)config.Generator).IsDefaultProfile) {
				entry.Strings.Remove(PasswordRuleKey);
			} else {
				entry.Strings.Set(PasswordRuleKey, new ProtectedString(false, SerializedConfiguration(config)));
			}
		}

		public static Configuration GroupConfiguration(PwGroup group) {
			if (group == null) {
				throw new ArgumentNullException(nameof(group));
			}
			try {
				string configStr = group.CustomData.Get(PasswordRuleKey);
				return configStr != null ? DeserializedConfiguration(configStr) : null;
#pragma warning disable CA1031 // Do not catch general exception types
			} catch {
#pragma warning restore CA1031 // Do not catch general exception types
				return null;
			}
		}

		public static void SetGroupConfiguration(PwGroup group, Configuration config) {
			if (group == null) {
				throw new ArgumentNullException(nameof(group));
			}
			if (config == null) {
				throw new ArgumentNullException(nameof(config));
			}
			if (config.Expiration == null && config.Generator is PasswordProfile && ((PasswordProfile)config.Generator).IsDefaultProfile) {
				group.CustomData.Remove(PasswordRuleKey);
			} else {
				group.CustomData.Set(PasswordRuleKey, SerializedConfiguration(config));
			}
		}

		private static string SerializedConfiguration(Configuration config) {
			using (StreamReader reader = new StreamReader(new MemoryStream(), Encoding.UTF8)) {
				new DataContractJsonSerializer(typeof(ConfigurationContract)).WriteObject(reader.BaseStream, new ConfigurationContract(config));
				reader.BaseStream.Position = 0;
				return reader.ReadToEnd();
			}
		}

		private static Configuration DeserializedConfiguration(string generatorStr) {
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
