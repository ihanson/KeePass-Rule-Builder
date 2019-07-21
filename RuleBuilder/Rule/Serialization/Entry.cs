using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using KeePassLib;
using KeePassLib.Security;

namespace RuleBuilder.Rule.Serialization {
	public static class Entry {
		public static IPasswordGenerator EntryGenerator(PwEntry entry) {
			try {
				ProtectedString generatorStr = entry.Strings.Get(PasswordRuleKey);
				return generatorStr != null ? DeserializedGenerator(generatorStr.ReadString()) : PasswordProfile.DefaultProfile;
			} catch {
				return PasswordProfile.DefaultProfile;
			}
		}
		public static void SetEntryGenerator(PwEntry entry, IPasswordGenerator generator) {
			if (generator is PasswordProfile && ((PasswordProfile)generator).IsDefaultProfile) {
				entry.Strings.Remove(PasswordRuleKey);
			} else {
				entry.Strings.Set(PasswordRuleKey, new ProtectedString(false, SerializedGenerator(generator)));
			}
		}
		private static string SerializedGenerator(IPasswordGenerator generator) {
			using (MemoryStream stream = new MemoryStream())
			using (StreamReader reader = new StreamReader(stream, Encoding.UTF8)) {
				new DataContractJsonSerializer(typeof(ConfigurationContract)).WriteObject(stream, new ConfigurationContract(generator));
				stream.Position = 0;
				return reader.ReadToEnd();
			}
		}
		private static IPasswordGenerator DeserializedGenerator(string generatorStr) {
			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(false)) {
				AutoFlush = true
			}) {
				writer.Write(generatorStr);
				stream.Position = 0;
				return ((ConfigurationContract)new DataContractJsonSerializer(typeof(ConfigurationContract)).ReadObject(stream)).Object();
			}
		}
		private const string PasswordRuleKey = "Password Rule";
	}
}
