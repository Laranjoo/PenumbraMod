using PenumbraMod.Content;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Content.Buffs;
using System;
using PenumbraMod.Content.Items.Placeable;

namespace PenumbraMod.Content.Items
{
	public class AquamarineStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.crit = 4; 
			Item.mana = 12;
			Item.DamageType = DamageClass.Magic;
			Item.width = 42;
			Item.height = 40;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 8;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<AquamarineBolt>();
			Item.shootSpeed = 8f;
			Item.noMelee = true;			
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Coral, 12);
			recipe.AddIngredient(ItemID.Seashell, 6);
            recipe.AddIngredient(ItemID.Starfish, 3);
            recipe.AddIngredient(ModContent.ItemType<Aquamarine>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}