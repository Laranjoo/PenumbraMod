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
	public class MarshmellowProjectile : ModProjectile
	{
        public override void SetDefaults()
		{
			Projectile.damage = 6;
			Projectile.width = 12;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft= 600;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 600;
			Projectile.aiStyle = 0;          
        }     
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.10f);
            Projectile.rotation += 0.4f;
        }
        public override void OnKill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }

    }
}