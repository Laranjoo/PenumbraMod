using PenumbraMod.Content.DamageClasses;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Prefixes
{
    // This class serves as an example for declaring item 'prefixes', or 'modifiers' in other words.
    public class DeathlyPrefix : ModPrefix
    {
        public virtual float power => 1f;

        // Change your category this way, defaults to PrefixCategory.Custom. Affects which items can get this prefix.
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        // See documentation for vanilla weights and more information.
        // In case of multiple prefixes with similar functions this can be used with a switch/case to provide different chances for different prefixes
        // Note: a weight of 0f might still be rolled. See CanRoll to exclude prefixes.
        // Note: if you use PrefixCategory.Custom, actually use ModItem.ChoosePrefix instead.
        public override float RollChance(Item item)
        {
            return 5f;
        }

        // Determines if it can roll at all.
        // Use this to control if a prefix can be rolled or not.
        public override bool CanRoll(Item item)
        {
            return item.DamageType.CountsAsClass(ModContent.GetInstance<ReaperClass>());
        }

        // Use this function to modify these stats for items which have this prefix:
        // Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= 1f + 0.12f * power;
            knockbackMult *= 1f + 0.08f * power;
            useTimeMult *= 1f - 0.10f * power;
            critBonus += 4;
        }

        // Modify the cost of items with this modifier with this function.
        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 1f + 0.09f * power;
        }

        // This is used to modify most other stats of items which have this modifier.
        public override void Apply(Item item) { }

        // This prefix doesn't affect any non-standard stats, so these additional tooltiplines aren't actually necessary, but this pattern can be followed for a prefix that does affect other stats.
        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            yield return new TooltipLine(Mod, "Deathly", (string)LocalizedDescriptions.Deathly)
            {
                IsModifier = true, // Sets the color to the positive modifier color.              
            };
        }
    }
    public class DarkeningPrefix : ModPrefix
    {
        public virtual float power => 1f;

        // Change your category this way, defaults to PrefixCategory.Custom. Affects which items can get this prefix.
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        // See documentation for vanilla weights and more information.
        // In case of multiple prefixes with similar functions this can be used with a switch/case to provide different chances for different prefixes
        // Note: a weight of 0f might still be rolled. See CanRoll to exclude prefixes.
        // Note: if you use PrefixCategory.Custom, actually use ModItem.ChoosePrefix instead.
        public override float RollChance(Item item)
        {
            return 4f;
        }

        // Determines if it can roll at all.
        // Use this to control if a prefix can be rolled or not.
        public override bool CanRoll(Item item)
        {
            return item.DamageType.CountsAsClass(ModContent.GetInstance<ReaperClass>());
        }

        // Use this function to modify these stats for items which have this prefix:
        // Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= 1f + 0.08f * power;
            knockbackMult *= 1f + 0.06f * power;
            useTimeMult *= 1f - 0.07f * power;
        }

        // Modify the cost of items with this modifier with this function.
        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 1f + 0.06f * power;
        }

        // This is used to modify most other stats of items which have this modifier.
        public override void Apply(Item item) { }

        // This prefix doesn't affect any non-standard stats, so these additional tooltiplines aren't actually necessary, but this pattern can be followed for a prefix that does affect other stats.
        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            yield return new TooltipLine(Mod, "Darkening", (string)LocalizedDescriptions.Darkening)
            {
                IsModifier = true, // Sets the color to the positive modifier color.              
            };
        }
    }
    class LocalizedDescriptions : ModSystem
    {
        public static LocalizedText Deathly { get; private set; }
        public static LocalizedText Darkening { get; private set; }
        public override void Load()
        {
            string category = "PrefixesDescriptions";
            Deathly ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Deathly"));
            Darkening ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Darkening"));
        }
    }
}
