using PenumbraMod.Content;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Buffs;
using System;
using PenumbraMod.Content.Items.Consumables;

namespace PenumbraMod.Content.Items
{
	public class MarshmellowStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Marshmellow Staff"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("Fires a heavy marshmellow that makes enemies slow" +
                "\n'Please, don't eat the cable...'"); */
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.crit = 10; 
			Item.mana = 5;
			Item.DamageType = DamageClass.Magic;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 8;
			Item.value = 1000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MarshmellowSlowProjectile>();
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			
		}

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 8);
			recipe.AddIngredient(ModContent.ItemType<Marshmellow>(), 9);
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
        }
	}
}