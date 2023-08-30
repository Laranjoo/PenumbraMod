using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class BloodrootPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("BloodrootPickaxe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("");
			
		}

		public override void SetDefaults()
		{
			Item.damage = 17;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 8;
			Item.useAnimation = 8;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 23000;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.pick = 195;
			Item.tileBoost += 1;
			Item.crit = 9;

			
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.OrangeBloodroot, 1);
			recipe.AddIngredient(ItemID.DeathbringerPickaxe, 1);
			recipe.AddIngredient(ModContent.ItemType<BloodystoneBar>(), 15);
            recipe.AddIngredient(ItemID.TissueSample, 10);
			recipe.AddIngredient(ItemID.Ruby, 2);
			
			

			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}