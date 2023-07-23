using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class ShadewoodScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shadewood Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 44;
			Item.height = 36;
			Item.useTime = 34;
			Item.useAnimation = 34;
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
			.AddIngredient(ItemID.Shadewood, 15)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
}