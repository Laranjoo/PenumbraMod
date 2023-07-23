using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class EnchantedShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
		  // DisplayName.SetDefault("EnchantedShot"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

		}

		public override void SetDefaults()
		{
			Projectile.damage = 11;
			Projectile.width = 25;
			Projectile.height = 13;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft= 600;
			Projectile.light = 0.25f;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.scale = 1.5f;


        }
		public override void AI()
		{
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.Enchanted_Gold, 1f, 0f, 0, Color.LightBlue, 1f);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 1.7f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.011f;



            int dust2 = Dust.NewDust(Projectile.Center, 1, 1, DustID.Enchanted_Pink, 1f, 0f, 0);
            Main.dust[dust2].noGravity = false;
            Main.dust[dust2].velocity *= 1.7f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.011f;

            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }

        }

        public override void Kill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}