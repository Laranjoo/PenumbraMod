using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.ReaperJewels
{
    /// <summary>
    /// All the Reaper Jewels of the mod!
    /// </summary>
    public class Jewels // Nothing there because ¯\_(ツ)_/¯
    {
    }
    public class AmythestCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 3000;
            Item.rare = 2;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Amethyst, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class RubyCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 10000;
            Item.rare = 2;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Ruby, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        
    }
    public class DiamondCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 15000;
            Item.rare = 2;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Diamond, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class TopazCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 4000;
            Item.rare = 2;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Topaz, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class AblazedCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 13000;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 12);
            recipe.AddIngredient(ItemID.Obsidian, 8);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
        }
    }
    public class DarkenedCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 12000;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemoniteBar, 10);
            recipe.AddIngredient(ItemID.ShadowScale, 15);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
    public class BloodstainedCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 14000;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrimtaneBar, 12);
            recipe.AddIngredient(ItemID.TissueSample, 15);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
    public class SlimyCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 5000;
            Item.rare = ItemRarityID.Blue;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();            
            recipe.AddIngredient(ModContent.ItemType<AerogelBar>(), 15);
            recipe.AddIngredient(ModContent.ItemType<RubyCrystal>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class SpectreCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 34000;
            Item.rare = ItemRarityID.Cyan;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddIngredient(ItemID.SpectreBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
    public class TerraCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 45000;
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TopazCrystal>(), 1);
            recipe.AddIngredient(ModContent.ItemType<AmythestCrystal>(), 1);
            recipe.AddIngredient(ModContent.ItemType<SapphireCrystal>(), 1);
            recipe.AddIngredient(ModContent.ItemType<EmeraldCrystal>(), 1);
            recipe.AddIngredient(ModContent.ItemType<RubyCrystal>(), 1);
            recipe.AddIngredient(ModContent.ItemType<DiamondCrystal>(), 1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
    public class SapphireCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 7000;
            Item.rare = 2;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Sapphire, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class EmeraldCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 8000;
            Item.rare = 2;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Emerald, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class MagicCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 12000;
            Item.rare = ItemRarityID.Pink;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }
    }
    public class AzuriteCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 10000;
            Item.rare = 3;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }
    }
    public class ThePrimeyeCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 33000;
            Item.rare = 3;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SoulofFright, 20);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
    public class CorrosiveJewel : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 14;
            Item.value = 43000;
            Item.rare = ItemRarityID.Lime;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<CorrosiveShard>(15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}