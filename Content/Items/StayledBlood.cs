using Microsoft.Xna.Framework;
using PenumbraMod.Content;
using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class StayledBlood : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("StayledBlood"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("This bloody blade was made by your strong effort and bloodshed, fires a Blood Ball");
			
		}

		public override void SetDefaults()
		{
			Item.damage = 115;
			Item.DamageType = DamageClass.Melee;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 40;
			Item.useAnimation = 26;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 1000;
			Item.rare = ModContent.RarityType<StayledBloodRarity>();
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<BloodBall>();
			Item.shootSpeed = 8f;
			
		}
       
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BloodystoneBar>(), 12);
			recipe.AddIngredient(ItemID.BloodButcherer, 1);
			recipe.AddIngredient(ItemID.Bone, 20);
			recipe.AddIngredient(ItemID.Vertebrae, 10);
			recipe.AddIngredient(ItemID.SoulofNight, 4);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}