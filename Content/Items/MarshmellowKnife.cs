using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class MarshmellowKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
        }

		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 7;
			Item.height = 17;
			Item.useTime = 4;
			Item.useAnimation = 12;
			Item.reuseDelay = 14;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = 300;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MarshmellowKnifeProjectile>();
			Item.consumable = true;
			Item.maxStack = 999;
			Item.shootSpeed = 14f;
			Item.noMelee = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(15);
			recipe.AddIngredient(ModContent.ItemType<Marshmellow>(), 5);
			recipe.AddIngredient(ItemID.Wood, 3);
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}