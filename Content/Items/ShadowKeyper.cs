using PenumbraMod.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class ShadowKeyper : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shadow Keyper"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("All the 5 elemental swords were combinated..."
				+ "\nYou now have the power of the shadows"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 180;
			Item.DamageType = DamageClass.Melee;
			Item.width = 80;
			Item.height = 80;
            Item.useTime = 36;
            Item.useAnimation = 36;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 1000;
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<NightBeam>();
			Item.shootSpeed = 18f;
			Item.scale = 1.2f;
		}
       
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<HellFire>(), 1);
            recipe.AddIngredient(ModContent.ItemType<AbsoluteZero>(), 1);
            recipe.AddIngredient(ModContent.ItemType<NatureGrass>(), 1);
            recipe.AddIngredient(ModContent.ItemType<StayledBlood>(), 1);
            recipe.AddIngredient(ItemID.Keybrand, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}