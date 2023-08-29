using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class ShadewoodScythe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shadewood Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = ModContent.GetInstance<ReaperClass>();
			Item.width = 44;
			Item.height = 36;
			Item.useTime = 34;
			Item.useAnimation = 34;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 1200;
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<EMPTY>();
			Item.shootSpeed = 8f;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
			{
				for (int i = 0; i < 8; i++)
				{
					Vector2 vel = velocity.RotatedByRandom(MathHelper.ToRadians(20));
					vel *= 1f - Main.rand.NextFloat(0.4f);
					Projectile.NewProjectile(source, position, vel, ModContent.ProjectileType<ShadewoodScytheProj>(), damage, knockback, player.whoAmI);
				}
			}
               
            return false;
        }
        public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.Shadewood, 15)
			.AddTile(TileID.WorkBenches)
			.Register();
		}
	}
    public class ShadewoodScytheProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 12;
            Projectile.width = 22;
            Projectile.height = 10;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
			Projectile.aiStyle = 1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}