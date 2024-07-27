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
    public class AquamarineBolt : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.damage = 12;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 0;
            Projectile.alpha = 255;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            int dust2 = Dust.NewDust(Projectile.position, 0, 0, ModContent.DustType<AquaBolt>(), 0, 0f, 0);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 0.6f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.012f;

            Lighting.AddLight(Projectile.Center, Color.Aqua.ToVector3() * 2f);
        }
        public override void OnKill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int i = 0; i < 7; i++)
            {
                int dust2 = Dust.NewDust(Projectile.position, 0, 0, ModContent.DustType<AquaBolt>(), 0, 0f, 0);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].scale = (float)Main.rand.Next(100, 135) * 0.010f;
            }
          
        }
    }
}