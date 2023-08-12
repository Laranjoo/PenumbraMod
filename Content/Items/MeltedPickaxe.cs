using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class MeltedPickaxe : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 13;
			Item.DamageType = DamageClass.Melee;
			Item.width = 42;
			Item.height = 36;
			Item.useTime = 12;
			Item.useAnimation = 16;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 1000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.pick = 105;
			Item.tileBoost += 1;
			Item.crit = 9;

			
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MeltedEmber>(), 13);
			recipe.AddIngredient(ItemID.Obsidian, 8);
            recipe.AddIngredient(ItemID.SoulofNight, 2);
            recipe.AddIngredient(ItemID.MoltenPickaxe, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}