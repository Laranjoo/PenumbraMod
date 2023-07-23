using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Items.Consumables;
using Terraria.Audio;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items
{
	public class TrueSlimeGun : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("True Slime Gun"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("Now it hurts!" +
				"\nNow its useful!" +
				"\nConsumes gel as ammo"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 44;
			Item.height = 33;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = 5;
			Item.knockBack = 4;
			Item.value = 1050;
			Item.rare = 1;
			Item.UseSound = SoundID.Item13;
            Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SlimeSquirt>();
			Item.useAmmo = AmmoID.Gel;
			Item.shootSpeed = 14f;
			Item.noMelee = true;
			Item.crit = 12;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
           
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 45f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, 1f);
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			
			recipe.AddIngredient(ItemID.SlimeGun, 1);
			recipe.AddIngredient(ModContent.ItemType<AerogelBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }


	}
}