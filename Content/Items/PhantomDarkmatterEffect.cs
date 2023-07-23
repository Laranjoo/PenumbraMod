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
	public class PhantomDarkmatterEffect : ModProjectile
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft= 15;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
		}
		public override void AI()
		{
			int dust2 = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<DarkMatter2>(), 0f, 0f, 0);
            Main.dust[dust2].noGravity = true;
            Main.dust[dust2].velocity *= 0.2f;
            Main.dust[dust2].scale = (float)Main.rand.Next(100, 200) * 0.010f;
        }
    }
}