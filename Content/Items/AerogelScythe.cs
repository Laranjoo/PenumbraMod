using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Social;

namespace PenumbraMod.Content.Items
{
	public class AerogelScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Aerogel Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("This sticky scythe fires three hot slimes" +
                "\nThose hot slimes inflicts the hot slime debuff" +
                "\n[c/0066ff:Special ability:] When used, the scythe fires a quick homing slime saw, dealing high damage on enemies"); */
			
		}

		public override void SetDefaults()
		{
			Item.damage = 18;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 58;
			Item.height = 46;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 4340;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<HotSlimeShot>();
			Item.shootSpeed = 6f;
		}
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {

                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.BlueTorch);

            }
        }
 
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
                Projectile.NewProjectile(source, position, velocity * 2f, ModContent.ProjectileType<SlimeHomingProj>(), damage, knockback, player.whoAmI);

            const int NumProjectiles = 2;

            for (int i = 0; i < NumProjectiles; i++)
            {
 
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.4f);

                // Create a projectile.
                Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<HotSlimeShot>(), damage, knockback, player.whoAmI);

            }
            return true;

        }

        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient<AerogelBar>(16)
            .AddIngredient(ItemID.Gel, 12)
            .AddTile(TileID.Anvils)
			.Register();
		}
	}
}