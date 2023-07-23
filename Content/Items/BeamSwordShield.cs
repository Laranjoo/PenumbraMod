using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Dusts;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class BeamSwordShield : ModProjectile
	{
		
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Beam Sword Shield");
        }

		public override void SetDefaults() {
            Projectile.damage = 100;
            Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.width = 20;
			Projectile.height = 38;
			Projectile.timeLeft = 19;
		}
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.alpha += 10;
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.YellowStarDust, 0f, 0f, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 4f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.014f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.78f);
            if (player.HasBuff(ModContent.BuffType<ShieldCooldown>()))
            {
                Projectile.Kill();
                Projectile.timeLeft = 0;
                Projectile.active = false;
                Projectile.friendly = false;
            }

        }
        public override Color? GetAlpha(Color l)
        {
            return new Color(255, 218, 0, 0) * Projectile.Opacity;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (target.type != NPCID.TargetDummy)
                player.AddBuff(BuffID.ShadowDodge, 600);
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            player.AddBuff(ModContent.BuffType<ShieldCooldown>(), 600);
        }
    }
}
