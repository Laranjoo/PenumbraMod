using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;
using Terraria.Audio;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;
using PenumbraMod.Content.Buffs;

namespace PenumbraMod.Content.Items
{
	public class CopperScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Copper Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("[c/964316:Special ability:] When used, the player throws 1 copper scythe");

        }

		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 46;
			Item.height = 38;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 1000;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<EMPTY>();
			Item.shootSpeed = 12f;
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
			return player.ownedProjectileCounts[ModContent.ProjectileType<CopperScytheProj>()] < 1;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
		     	const int NumProjectiles = 1;

               for (int i = 0; i < NumProjectiles; i++)
               {
                    SoundEngine.PlaySound(SoundID.Item71, player.position);
  
                   // Create a projectile.
                   Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<CopperScytheProj>(), damage, knockback, player.whoAmI);

               }

			}
			
            return true;
		}

        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.CopperBar, 14)
			.AddIngredient(ItemID.Wood, 9)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}