using System;
using System.Collections.Generic;
using KeePassLib.Cryptography;

namespace RuleBuilder.Rule {
	public static class Random {
		public static T RandomItem<T>(IEnumerable<T> set) {
			List<T> list = new List<T>(set);
			return list[RandomNumber(list.Count)];
		}
		public static void Shuffle<T>(List<T> list) {
			if (list == null) {
				throw new ArgumentNullException(nameof(list));
			}
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
			uint uiRange = (uint)range;
			uint max = uint.MaxValue - ((uint.MaxValue - uiRange + 1) % uiRange);
			uint number;
			do {
				number = BitConverter.ToUInt32(CryptoRandom.Instance.GetRandomBytes(sizeof(uint)), 0);
			} while (number > max);
			return (int)(number % uiRange);
		}
	}
}
