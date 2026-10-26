using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ShapedGlassRebalance
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class ShapedGlassRebalancePlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Slendykray";
        public const string PluginName = "ShapedGlassRebalance";
        public const string PluginVersion = "1.0.0";

        public static ConfigEntry<float> hpFraction;

        public void Awake()
        {
            Log.Init(Logger);

            Asset.Init();

            InitConfig();

            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void InitConfig()
        {
            Sprite icon = Asset.mainBundle.LoadAsset<Sprite>("icon.png");
            ModSettingsManager.SetModIcon(icon);

            hpFraction = Config.Bind<float>("ShapedGlassRebalance", "hpFraction", 70f, "max hp threshold when one shot protection stops working");

            ModSettingsManager.AddOption(new SliderOption(hpFraction, new SliderConfig() { min = 0f, max = 100f }));
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            float num = 1f - 1f / self.cursePenalty;

            if (num < hpFraction.Value / 100f)
            {
                self.oneShotProtectionFraction = 0.1f;
            }
        
        }

    }
}
