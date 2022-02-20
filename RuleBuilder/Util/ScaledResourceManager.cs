using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using KeePass.UI;
using KeePassLib;
using KeePassLib.Utility;
using RuleBuilder.Properties;

namespace RuleBuilder.Util {
	class ScaledResourceManager : ResourceManager {
		private Dictionary<string, Image> Icons { get; } = new Dictionary<string, Image>();

		private ResourceManager LowRM { get; }

		private ScaledResourceManager(ResourceManager lowRM) {
			this.LowRM = lowRM;
		}

		public static void Initialize() {
			ResourceManager currentRM = Images.ResourceManager;
			if (DpiUtil.ScalingRequired && !(currentRM is ScaledResourceManager)) {
				PropertyInfo prop = typeof(Images).GetProperty(nameof(Images.ResourceManager), BindingFlags.Static | BindingFlags.NonPublic);
				prop.GetValue(null, null);

				FieldInfo field = typeof(Images).GetField("resourceMan", BindingFlags.Static | BindingFlags.NonPublic);
				field.SetValue(null, new ScaledResourceManager(currentRM));
			}
		}

		public override object GetObject(string name) => this.GetObject(name, null);

		public override object GetObject(string name, CultureInfo _) {
			if (!this.Icons.ContainsKey(name)) {
				this.Icons[name] = ScaledImage(name);
			}
			return this.Icons[name];
		}

		private Image ScaledImage(string name) {
			Image low = (Image)this.LowRM.GetObject(name);
			Image high = (Image)ImagesHigh.ResourceManager.GetObject(name);
			return GfxUtil.ScaleImage(high, DpiUtil.ScaleIntX(low.Width), DpiUtil.ScaleIntY(low.Height), ScaleTransformFlags.UIIcon);
		}
	}
}
