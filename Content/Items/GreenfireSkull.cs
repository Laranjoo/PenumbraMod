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
    public class GreenfireSkull : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Greenfire Skull"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            Main.projFrames[Projectile.type] = 7;
        
        }

        public override void SetDefaults()
        {
            Projectile.damage = 25;
            Projectile.width = 26;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.75f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.scale = 1.4f;
            Projectile.alpha = 255;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 180);
        }
       
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 25; k++)
            {
                int dust =  Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.GreenTorch, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f, Scale: 2.2f);
                Main.dust[dust].velocity *= 6.0f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.position);
        }
        public override void AI()
        {
            Projectile.ai[0] += 1f;

            FadeInAndOut();

            Projectile.velocity *= 1.04f;
            Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.CursedTorch, 0f, 0f, 0);
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity *= 3f;
            Main.dust[dust].scale = (float) Main.rand.Next(100, 170) * 0.006f;

            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.78f);
            Lighting.AddLight(Projectile.Center, Color.LightGreen.ToVector3() * 0.25f);   

        }
        public void FadeInAndOut()
        {
            // If last less than 50 ticks — fade in, than more — fade out
            if (Projectile.ai[0] <= 180f)
            {
                // Fade in
                Projectile.alpha -= 90;
                // Cap alpha before timer reaches 50 ticks
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;

                return;
            }

            // Fade out
            Projectile.alpha += 90;
            // Cal alpha to the maximum 255(complete transparent)
            if (Projectile.alpha > 255)
                Projectile.alpha = 255;
        }

      
    } 
}