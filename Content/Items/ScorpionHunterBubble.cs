using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class ScorpionHunterBubble : ModProjectile
	{
        public override void SetDefaults()
		{
			Projectile.damage = 50;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft= 180;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.aiStyle = 0;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            Projectile.alpha = 80;
        }
       
        public override void AI()
        {
            Projectile.rotation += 0.05f;
            Projectile.velocity *= 0.96f;
            Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3() * 0.10f);
          
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Venom, 120);
        }
        int radius1 = 30;
        public override void OnKill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.

            const int Repeats = 30;
            for (int i = 0; i < Repeats; ++i)
            {
                Vector2 position2 = Projectile.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                int r = Dust.NewDust(position2, 1, 1, DustID.BubbleBurst_Purple, 0f, 0f, 0, default(Color), 1f);
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity *= 2.9f;
                Main.dust[r].rotation += 1.1f;
            }
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item54, Projectile.position);
        }

    }
}