using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class JungleScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jungle Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("If you hit an enemy with that scythe, vines attaches to them dealing damage over time." +
				"\n[c/e99131:Special ability:] When used, the scythe fires spores that follows enemies"); */

        }
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 52;
            Item.height = 46;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 2460;
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 6f;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noUseGraphic = true;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = false;
            }
            return player.ownedProjectileCounts[ModContent.ProjectileType<SporeProj>()] < 3;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noUseGraphic = true;
                const int NumProjectiles = 6;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(120));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 1f - Main.rand.NextFloat(0.2f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<SporeProj>(), damage, knockback, player.whoAmI);
                }
            }



            return false; // return false to stop vanilla from calling Projectile.NewProjectile.
        }


        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {

            target.AddBuff(ModContent.BuffType<SpikeStunned>(), 300);


        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.RichMahogany, 25)
            .AddIngredient(ItemID.Stinger, 15)
            .AddIngredient(ItemID.JungleSpores, 7)
            .AddIngredient(ItemID.RichMahogany, 15)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }

}



