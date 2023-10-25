using Microsoft.Xna.Framework;
using PenumbraMod.Content;
using PenumbraMod.Content.Rarities;
using Terraria;
using Terraria.DataStructures;
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
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.mana = 8;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 23000;
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CorrodedCrystal>();
			Item.shootSpeed = 16f;
		}
        int radius1 = 10;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 pos = player.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-50, 50));
			Projectile.NewProjectile(source, pos, velocity, type, damage, knockback, player.whoAmI);
            const int Repeats = 50;
            for (int i = 0; i < Repeats; ++i)
            {
                Vector2 position2 = pos + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                int r = Dust.NewDust(position2, 1, 1, DustID.GreenTorch);
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity *= 5f;
                Main.dust[r].rotation += 1.1f;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CorrosiveShard>(), 14)
                 .AddIngredient(ModContent.ItemType<CorrodedPlating>(), 12)

               .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}