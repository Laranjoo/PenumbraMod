using Microsoft.Xna.Framework;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class InfectedScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 91;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 56;
            Item.height = 50;
            Item.useTime = 18;
            Item.useAnimation = 16;
            Item.useStyle = 1;
            Item.knockBack = 5;
            Item.value = 17800;
            Item.rare = ItemRarityID.LightRed;
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
            recipe.AddIngredient(ModContent.ItemType<DemoniteScythe>(), 1);
            recipe.AddIngredient(ModContent.ItemType<InfectedBar>(), 13);
            recipe.AddIngredient(ItemID.ShadowScale, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}