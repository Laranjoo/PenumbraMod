using Microsoft.Xna.Framework;
using PenumbraMod.Content;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class AbsoluteZero : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Absolute Zero"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("This blade is so heavy and cold that makes your hands shake..." +
				"\n It fires an icy shot that explodes in 8 pieces"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 120;
			Item.DamageType = DamageClass.Melee;
			Item.width = 70;
			Item.height = 70;
			Item.useTime = 60;
			Item.useAnimation = 44;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 32000;
			Item.rare = ModContent.RarityType<AbsoluteZeroRarity>();
			Item.UseSound = SoundID.Item1;
            Item.UseSound = SoundID.Item28;
            Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<IcyShot>();
			Item.shootSpeed = 10f;
			Item.scale = 1.4f;
		}
        
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GlacialChunk>(), 22);
			recipe.AddIngredient(ItemID.IceBlade, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}