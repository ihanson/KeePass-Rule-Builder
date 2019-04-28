using System.Security.Cryptography;

namespace RuleBuilder.Rule {
	internal static class Random {
		public static T RandomItem<T>(RNGCryptoServiceProvider csp, T[] array) => array[RandomNumber(csp, array.Length)];
		public static void Shuffle<T>(RNGCryptoServiceProvider csp, T[] array) {
			for (int i = 0; i < array.Length; i++) {
				int swapIndex = RandomNumber(csp, array.Length - i) + i;
				T temp = array[i];
				array[i] = array[swapIndex];
				array[swapIndex] = temp;
			}
		}
		private static int RandomNumber(RNGCryptoServiceProvider csp, int range) {
			uint uRange = (uint)range;
			uint max = uint.MaxValue - ((uint.MaxValue - uRange + 1) % uRange);
			byte[] bytes = new byte[sizeof(uint)];
			uint number;
			do {
				csp.GetBytes(bytes);
				number = System.BitConverter.ToUInt32(bytes, 0);
			} while (number > max);
			return (int)(number % uRange);
		}
	}
}
