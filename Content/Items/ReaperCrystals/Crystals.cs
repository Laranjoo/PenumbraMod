using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.ReaperCrystals
{
    /// <summary>
    /// All the Reaper Crystals of the mod!
    /// </summary>
    public class Crystals
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
}