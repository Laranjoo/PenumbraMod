using Microsoft.Xna.Framework;
using PenumbraMod.Content;
using PenumbraMod.Content.Rarities;
using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class AerogelBall : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Aerogel Ball"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("Shoots an bouncy ball" +
				"\nInflicts slimed on enemies" +
				"\n''Time to do some basketball guys!''"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 18;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 2500;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<AerogelBallProjectile>();
			Item.shootSpeed = 10f;
			Item.noUseGraphic = true;

		}
        
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AerogelBar>(), 9);
			recipe.AddIngredient(ItemID.Gel, 30);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}