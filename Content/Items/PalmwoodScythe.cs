using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class PalmwoodScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Palmwood Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			
		}

		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 50;
			Item.height = 38;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 1000;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.PalmWood, 15)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}