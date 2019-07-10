using KeePassLib.Cryptography;
using System;
using System.Collections.Generic;

namespace RuleBuilder.Rule {
	internal static class Random {
		public static T RandomItem<T>(HashSet<T> set) {
			List<T> list = new List<T>(set);
			return list[RandomNumber(list.Count)];
		}
		public static void Shuffle<T>(List<T> list) {
			for (int i = 0; i < list.Count; i++) {
				int swapIndex = RandomNumber(list.Count - i) + i;
				T temp = list[i];
				list[i] = list[swapIndex];
				list[swapIndex] = temp;
			}
		}
		private static int RandomNumber(int range) {
			if (range <= 0) {
				throw new ArgumentOutOfRangeException(nameof(range));
			}
			uint uRange = (uint)range;
			uint max = uint.MaxValue - ((uint.MaxValue - uRange + 1) % uRange);
			uint number;
			do {
				number = System.BitConverter.ToUInt32(CryptoRandom.Instance.GetRandomBytes(sizeof(uint)), 0);
			} while (number > max);
			return (int)(number % uRange);
		}
	}
}
