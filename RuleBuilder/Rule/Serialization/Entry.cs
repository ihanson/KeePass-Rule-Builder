using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using KeePassLib;
using KeePassLib.Security;

namespace RuleBuilder.Rule.Serialization {
	public static class Entry {
		private const string PasswordRuleKey = "Password Rule";
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
				return ((ConfigurationContract)new DataContractJsonSerializer(typeof(ConfigurationContract)).ReadObject(writer.BaseStream)).Object();
			}
		}
	}
}
