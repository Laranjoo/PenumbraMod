using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class MarshmellowPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Marshmellow Pickaxe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("This fluffy pickaxe is somehow strong");
			
		}

		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Melee;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 1000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.pick = 40;
			Item.crit = 9;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 7);
			recipe.AddIngredient(ModContent.ItemType<Marshmellow>(), 12);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}