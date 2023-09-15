using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class TheDesertShiver : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 56;
			Item.DamageType = DamageClass.Melee;
			Item.width = 68;
			Item.height = 68;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = 23000;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Diamond, 15);
			recipe.AddIngredient(ItemID.DesertFossil, 15);
			recipe.AddIngredient(ItemID.MeteoriteBar, 7);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}