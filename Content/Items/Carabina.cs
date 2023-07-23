using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content;
using Terraria.Audio;

namespace PenumbraMod.Content.Items
{
	public class Carabina : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Carabina"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("A very slow and powerful rifle that can easily destroy enemies"
				+ "\n''Boom headshot!''"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 355;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 124;
			Item.height = 44;
			Item.useTime = 78;
			Item.useAnimation = 78;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 50000;
			Item.rare = ItemRarityID.Purple;
            Item.UseSound = new SoundStyle("PenumbraMod/Assets/Sounds/Items/SniperShot")
            {
                Volume = 1.6f,
                PitchVariance = 0.2f,
                MaxInstances = 1,
            };
            Item.autoReuse = true;
			Item.shoot = ProjectileID.Bullet;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 28f;
			Item.noMelee = true;
			Item.crit = 32;
		}

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-22f, 1f);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 82f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HallowedBar, 15);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.SniperRifle, 1);
			recipe.AddIngredient(ModContent.ItemType<MeltedEmber>(), 7);      
		    recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }



    }
}