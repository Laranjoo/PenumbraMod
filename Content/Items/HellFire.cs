using PenumbraMod.Content;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class HellFire : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("HellFire"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("Its too hot that is melting...");
			
		}

		public override void SetDefaults()
		{
			Item.damage = 145;
			Item.DamageType = DamageClass.Melee;
			Item.width = 58;
			Item.height = 58;
			Item.useTime = 42;
			Item.useAnimation = 26;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 1000;
			Item.rare = ModContent.RarityType<HellFireRarity>();
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<HellShot>();
			Item.shootSpeed = 8f;
			
		}
       
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 20);
			recipe.AddIngredient(ItemID.FieryGreatsword, 1);
			recipe.AddIngredient(ItemID.LivingFireBlock, 14);
			recipe.AddIngredient(ItemID.SoulofMight, 7);
			recipe.AddIngredient(ModContent.ItemType<MeltedEmber>(), 15);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}