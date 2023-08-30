using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items
{
	public class InfectedGun : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 46;
			Item.height = 22;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 12000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.Bullet;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 14f;
			Item.noMelee = true;
			Item.crit = 25;
		}
      
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Musket, 1);
            recipe.AddIngredient(ModContent.ItemType<InfectedBar>(), 12);
            recipe.AddIngredient(ItemID.ShadowScale, 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet)
				type = ModContent.ProjectileType<InfectedWorm>();
           
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 50f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }


	}
}