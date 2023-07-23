using Microsoft.Xna.Framework;
using PenumbraMod.Content;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class CorrodedStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.staff[Type] = true;		
		}

		public override void SetDefaults()
		{
			Item.damage = 72;
			Item.DamageType = DamageClass.Magic;
			Item.width = 64;
			Item.height = 68;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 23000;
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<IcyShot>();
			Item.shootSpeed = 12f;
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CorrosiveShard>(), 18)
               .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}