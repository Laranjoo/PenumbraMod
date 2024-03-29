using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class DemoniteScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 36;
            Item.height = 38;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = 1;
            Item.knockBack = 5;
            Item.value = 2000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 5f;
            Item.autoReuse = true;

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff<ReaperControl>())
                for (int i = 0; i < 3; i++)
                {
                    Vector2 vel = velocity.RotatedByRandom(10);
                    Projectile.NewProjectile(source, position, vel, ProjectileID.TinyEater, damage, 2);
                }
                    
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.CrimtaneBar, 12)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}