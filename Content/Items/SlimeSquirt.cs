using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class SlimeSquirt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Slime Squirt"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.

		}

		public override void SetDefaults()
		{
			Projectile.damage = 16;
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.aiStyle = 68;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft= 600;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			int dust2 = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<SlimeParticle12>(), 0f, 0f, 0);
            Main.dust[dust2].noGravity = false;
            Main.dust[dust2].velocity *= 0.2f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 200) * 0.008f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slimed, 300);
        }

        public override void OnKill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item85, Projectile.position);
        }
    }
}