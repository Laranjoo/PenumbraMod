using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Armors
{
    public class PrismAura : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Prism Aura");
            Main.projFrames[Projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.damage = 45;
            Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.timeLeft = 60;
            Projectile.penetrate = -1;
            Projectile.light = 1f;
            Projectile.hide = false;
            Projectile.alpha = 0;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        bool firstSpawn = true;
        double deg;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.NextBool(6))
                target.AddBuff(ModContent.BuffType<PrismLightning>(), 60);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(230, 87, 120, 0) * Projectile.Opacity;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            return true;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            projOwner.heldProj = Projectile.whoAmI;
            Projectile.damage = 45;
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            if (firstSpawn)
            {

                firstSpawn = false;
            }

            if (Projectile.spriteDirection == 1)
            {//Adjust when facing the other direction

                Projectile.ai[1] = 280;
            }
            else
            {

                Projectile.ai[1] = 280;
            }
            deg = Projectile.ai[1];
            double rad = deg * (Math.PI / 180);
            double dist = 10;
            Projectile.timeLeft = 10;
            Projectile.position.X = projOwner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = projOwner.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
            if (Projectile.spriteDirection == 1)
            {//Adjust when facing the other direction

                Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(180f);
            }
            else
            {

                Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(180f);
            }
            //Projectile.Center += projOwner.gfxOffY * Vector2.UnitY;//Prevent glitchy animation.

            Projectile.ai[0] += 1f;

            if (projOwner.dead && !projOwner.active && !Main.dayTime)
            {//Disappear when player dies
                Projectile.Kill();
            }
            if (!Main.dayTime)
            {
                Projectile.timeLeft = 0;
                Projectile.Kill();
            }
            //Orient projectile
            Projectile.direction = projOwner.direction;
            Projectile.spriteDirection = Projectile.direction;

            if (projOwner.armor[0].type != ModContent.ItemType<PrismHelm>() || projOwner.armor[1].type != ModContent.ItemType<PrismArmor>() || projOwner.armor[2].type != ModContent.ItemType<PrismLeggings>())
            {
                projOwner.ClearBuff(ModContent.BuffType<PrismAura2>());
                Projectile.timeLeft = 0;
                Projectile.Kill();
            }
            if (projOwner.ZoneDirtLayerHeight)
            {
                Projectile.timeLeft = 0;
                projOwner.ClearBuff(ModContent.BuffType<PrismAura2>());
                Projectile.Kill();
            }
            else
            {

            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 30; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PurpleTorch, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f, Scale: 1.4f);
                Main.dust[dust].velocity *= 6.0f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item70, Projectile.position);
        }
    }
}