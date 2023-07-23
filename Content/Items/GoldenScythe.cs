using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class GoldenScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Golden Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("It material makes it heavy but beaultiful..." +
                "\n[c/bc9d14:Special ability:] When used, the player throws 4 golden scythes."); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 23;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 60;
			Item.height = 48;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 5570;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 16f;
			Item.noUseGraphic = false;
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
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[ModContent.ProjectileType<GoldScytheProj>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                const int NumProjectiles = 4;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    SoundEngine.PlaySound(SoundID.Item71, player.position);
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(60));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 1f - Main.rand.NextFloat(0f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<GoldScytheProj>(), damage, knockback, player.whoAmI);

                }

            }

            return true;
        }

        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.GoldBar, 17)
			.AddIngredient(ItemID.Wood, 12)
			.AddTile(TileID.Anvils)
			.Register();
		}
	}
}