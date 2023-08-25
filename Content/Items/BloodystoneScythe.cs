using Microsoft.Xna.Framework;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class BloodystoneScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 109;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 64;
            Item.height = 54;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = 1;
            Item.knockBack = 5;
            Item.value = 20170;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 8f;
            Item.autoReuse = true;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CrimsonScythe>(), 1);
            recipe.AddIngredient(ModContent.ItemType<BloodystoneBar>(), 17);
            recipe.AddIngredient(ItemID.TissueSample, 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}