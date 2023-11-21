using Terraria.Localization;
using Terraria.ModLoader;

namespace PenumbraMod
{
    public class PenumbraLocalization : ModSystem
    {
        // BESTIARY ENTRIES
        public static LocalizedText MeltedSkeleton { get; private set; }
        public static LocalizedText MarshSlime { get; private set; }
        public static LocalizedText EyeStormProtector { get; private set; }
        public static LocalizedText EyeStormShooter { get; private set; }
        public static LocalizedText EyeStorm { get; private set; }

        // ARMOR SET BONUSES
        public static LocalizedText PrismaArmor { get; private set; }
        public static LocalizedText SandHunterArmor { get; private set; }
        public static LocalizedText PerishedArmor { get; private set; }
        public static LocalizedText OldBloodystoneArmor { get; private set; }
        public static LocalizedText MeltedArmorNoKeybind { get; private set; }
        public static LocalizedText MarshArmorNoKeybind { get; private set; }
        public static LocalizedText BloodystoneArmorMelee { get; private set; }
        public static LocalizedText BloodystoneArmorMage { get; private set; }
        public static LocalizedText BloodystoneArmorRanged { get; private set; }
        public static LocalizedText BloodystoneArmorSummonner { get; private set; }
        public static LocalizedText RobeAqua { get; private set; }
        public static LocalizedText RobePeridot { get; private set; }
        public static LocalizedText RobeRoze { get; private set; }

        // ITEM TOOLTIPS
        public static LocalizedText DeathSickle { get; private set; }
        public static LocalizedText IceSickle { get; private set; }
        public static LocalizedText BeamSword { get; private set; }
        public static LocalizedText MagebladeShards { get; private set; }

        // COMBAT TEXT
        public static LocalizedText SoulBoosted { get; private set; }
        public static LocalizedText MeltedBlasterGunMode { get; private set; }
        public static LocalizedText MeltedBlasterChargeMode { get; private set; }
        public static LocalizedText MeltedBlasterOvercharging { get; private set; }

        // STRUCTURES ON WORLD GEN
        public static LocalizedText Marble { get; private set; }
        public static LocalizedText LostSword { get; private set; }
        public static LocalizedText ShimmerStructure { get; private set; }
        public override void Load()
        {
            string category = "PenumbraLocalization";
            
            // BESTIARY ENTRIES
            MeltedSkeleton ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Bestiary.MeltedSkeleton"));
            MarshSlime ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Bestiary.MarshSlime"));
            EyeStormProtector ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Bestiary.EyeStormProtector"));
            EyeStormShooter ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Bestiary.EyeStormShooter"));
            EyeStorm ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Bestiary.EyeStorm"));

            // ARMOR SET BONUSES
            PrismaArmor ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.PrismaArmor"));
            SandHunterArmor ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.SandHunter"));
            PerishedArmor ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.PerishedArmor"));
            OldBloodystoneArmor ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.OldBloodystoneArmor"));
            MeltedArmorNoKeybind ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.MeltedArmorWithoutKeybind"));
            MarshArmorNoKeybind ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.MarshArmorWithoutKeybind"));
            BloodystoneArmorMelee ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.BloodystoneArmorMelee"));
            BloodystoneArmorMage ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.BloodystoneArmorMage"));
            BloodystoneArmorRanged ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.BloodystoneArmorRanged"));
            BloodystoneArmorSummonner ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.BloodystoneArmorSummonner"));
            RobeRoze ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.RobeRoze"));
            RobePeridot ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.Peridot"));
            RobeAqua ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.SetBonuses.RobeAqua"));

            // ITEM TOOLTIPS
            DeathSickle ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ItemTooltips.DeathSickle"));
            IceSickle ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ItemTooltips.IceSickle"));
            BeamSword ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ItemTooltips.BeamSword"));
            MagebladeShards ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ItemTooltips.MagebladeShards"));

            // COMBAT TEXTS
            SoulBoosted ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.CombatTexts.SoulBoosted"));
            MeltedBlasterChargeMode ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.CombatTexts.MeltedBlasterChargeMode"));
            MeltedBlasterGunMode ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.CombatTexts.MeltedBlasterGunMode"));
            MeltedBlasterOvercharging ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.CombatTexts.MeltedBlasterOvercharge"));

            // STRUCTURES FOR WORLD GEN
            Marble ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Structures.Marble"));
            LostSword ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Structures.LostSword"));
            ShimmerStructure ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Structures.ShimmerStructure"));
        }
    }
}
