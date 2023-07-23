using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class AerogelHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Aerogel Hammer"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
		}

		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 1000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.hammer = 55;
			Item.crit = 14;

			
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Gel, 16);
			recipe.AddIngredient(ModContent.ItemType<AerogelBar>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}