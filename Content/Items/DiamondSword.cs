using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class DiamondSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("DiamondSword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("''OMG, Mom! i found diamonds!!!!''");
			
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = 1000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.scale = 1.5f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Diamond, 15);
			recipe.AddIngredient(ItemID.Wood, 5);
			recipe.AddIngredient(ItemID.MeteoriteBar, 7);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}