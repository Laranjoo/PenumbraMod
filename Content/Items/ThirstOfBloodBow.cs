using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items
{
	public class ThirstOfBloodBow : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("ThirstOfBloodBow"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("Converts Unholy Arrows into BloodThirsty Arrows"
				+ "\n''Caution! It bites...''"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 78;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 12400;
			Item.rare = 4;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = 1;
			Item.useAmmo = AmmoID.Arrow;
			Item.shootSpeed = 16f;
			Item.noMelee = true;
			Item.crit = 25;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.OrangeBloodroot, 1);
			recipe.AddIngredient(ItemID.TendonBow, 1);
			recipe.AddIngredient(ItemID.Bone, 15);
			recipe.AddIngredient(ItemID.Vertebrae, 10);
			recipe.AddIngredient(ItemID.SoulofNight, 5);
			recipe.AddIngredient(ModContent.ItemType<BloodystoneBar>(), 10);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.UnholyArrow)
				type = ModContent.ProjectileType<BloodThirstyArrow>();
           
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 60f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }


	}
}