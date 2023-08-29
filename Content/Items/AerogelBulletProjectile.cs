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
	public class AerogelBulletProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
             // DisplayName.SetDefault("Aerogel Bullet"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			
        }

        public override void SetDefaults()
		{
			Projectile.damage = 10;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft= 600;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 600;
			Projectile.aiStyle = 1;
            Projectile.light = 0.5f;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 1;
        }
	}
}