using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PenumbraMod.Content
{
    public class GoreProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0.99f;
            Projectile.rotation = 0f;
            return false;
        }
        public override void AI()
        {
            Projectile.rotation += 0.1f;
            Projectile.alpha += 2;
            if (Projectile.alpha > 255)
                Projectile.Kill();
        }
    }
}