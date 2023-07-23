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
    public class DarkExplosion : ModProjectile
	{
       
		public override void SetStaticDefaults()
		{
		 // DisplayName.SetDefault("Dark Matter"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

		public override void SetDefaults()
		{
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        
        public override void AI()
        {
            Projectile.ai[0] += 1f;
            Lighting.AddLight(Projectile.Center, Color.Black.ToVector3() * 0.93f);
            Projectile.scale *= 1.03f;
            Projectile.rotation += 0.1f;
            FadeInAndOut();
            
        }
        public void FadeInAndOut()
        {
            // If last less than 50 ticks — fade in, than more — fade out
            if (Projectile.ai[0] <= 30f)
            {
                // Fade in
                Projectile.alpha -= 90;
                // Cap alpha before timer reaches 50 ticks
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;

                return;
            }

            // Fade out
            Projectile.alpha += 30;
            // Cal alpha to the maximum 255(complete transparent)
            if (Projectile.alpha > 255)
                Projectile.alpha = 255;
        }
    }
}