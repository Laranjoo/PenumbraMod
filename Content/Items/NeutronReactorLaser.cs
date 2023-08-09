using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
namespace PenumbraMod.Content.Items
{
    public class NeutronReactorRay : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 11;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 270;
            Projectile.width = 600;
            Projectile.height = 10;
            //Projectile.aiStyle = 1;
            // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
            Projectile.friendly = true;
            //projectile.magic = true;
            //projectile.extraUpdates = 100;
            Projectile.timeLeft = 99999; // lowered from 300
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 0.5f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
            Projectile.netImportant = true;
        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        int t;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(t);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            t = reader.ReadInt32();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation; // The rotation of the Jousting Lance           
            float scaleFactor = -300f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
            float widthMultiplier = 16f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
            float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.
            if (Projectile.ai[2] >= 60)
            {
                scaleFactor = -600f;
                Projectile.scale = 2f;
            }
            if (Projectile.ai[2] >= 120)
            {
                scaleFactor = -900f;
                Projectile.scale = 3f;
            }
            // This Rectangle is the width and height of the Jousting Lance's hitbox which is used for the first step of collision.
            // You will need to modify the last two numbers if you have a bigger or smaller Jousting Lance.
            // Vanilla uses (0, 0, 300, 300) which that is quite large for the size of the Jousting Lance.
            // The size doesn't matter too much because this rectangle is only a basic check for the collision (the hit-line is much more important).
            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 300, 300);

            // Set the position of the large rectangle.
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;

            hlende = hitLineEnd;
            // First check that our large rectangle intersects with the target hitbox.
            // Then we check to see if a line from the tip of the Jousting Lance to the "end" of the lance intersects with the target hitbox.
            if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
            {
                return true;
            }
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.timeLeft = 2;
            t++;
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                // This code must only be ran on the client of the projectile owner
                if (player.channel)
                {
                    float holdoutDistance = player.HeldItem.shootSpeed;
                    // Calculate a normalized vector from player to mouse and multiply by holdoutDistance to determine resulting holdoutOffset
                    Vector2 holdoutOffset = holdoutDistance * Vector2.Normalize(Main.MouseWorld - playerCenter);
                    if (holdoutOffset.X != Projectile.velocity.X || holdoutOffset.Y != Projectile.velocity.Y)
                    {
                        // This will sync the projectile, most importantly, the velocity.
                        Projectile.netUpdate = true;
                    }

                    // Projectile.velocity acts as a holdoutOffset for held projectiles.
                    Projectile.velocity = holdoutOffset;
                }
                else
                {
                    Projectile.Kill();
                    t = 0;
                    Projectile.ai[1] = 0;
                }
            }
            if (Main.rand.NextBool(10))
            {
                int dust = Dust.NewDust(Projectile.Center, 10, 10, DustID.PinkTorch, 0f, 0f, 0);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 7.3f;
                Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 0.012f;
            }
            if (t == 1 || t == 40)
            {
                SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
            }
            if (t == 60 || t == 120)
            {
                SoundEngine.PlaySound(SoundID.Item60, Projectile.position);
                PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 5f, 16f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
                player.statMana -= 12;
            }
            if (t == 140)
            {
                player.statMana -= 12;
                SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
            }
            if (t == 170)
                t = 125;
            if (player.statMana <= 0)
            {
                Projectile.Kill();
                Projectile.timeLeft = 0;
            }
            if (Projectile.velocity.X > 0f)
            {
                player.ChangeDir(1);
            }
            else if (Projectile.velocity.X < 0f)
            {
                player.ChangeDir(-1);
            }
            const float rotation = 0.06f;
            player.ChangeDir(Projectile.direction); // Change the player's direction based on the projectile's own
            player.heldProj = Projectile.whoAmI; // We tell the player that the drill is the held projectile, so it will draw in their hand
            player.SetDummyItemTime(2); // Make sure the player's item time does not change while the projectile is out
            if (player.direction == 1)
            Projectile.Center = playerCenter + new Vector2(-5, -4); 
            else
                Projectile.Center = playerCenter + new Vector2(5, 4); // Centers the projectile on the player. Projectile.velocity will be added to this in later Terraria code causing the projectile to be held away from the player at a set distance.
            Projectile.rotation = Projectile.rotation.AngleLerp((new Vector2(Projectile.ai[0], Projectile.ai[1]) - Projectile.Center).ToRotation(), rotation);
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.ai[0] = Main.MouseWorld.X;
                Projectile.ai[1] = Main.MouseWorld.Y;
            }
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            Projectile.ai[2]++;

        }

    }
    public class NeutronReactorHeld : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            //Projectile.aiStyle = 1;
            // AIType = ProjectileID.Bullet; // Act exactly like default Bullet
            Projectile.friendly = true;
            //projectile.magic = true;
            //projectile.extraUpdates = 100;
            Projectile.timeLeft = 99999; // lowered from 300
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 0.5f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
            Projectile.netImportant = true;
        }
        Vector2 dir = Vector2.Zero;
        Vector2 hlende = Vector2.Zero;
        int t;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(t);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
           t = reader.ReadInt32();
        }      
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.timeLeft = 2;
            t++;
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                // This code must only be ran on the client of the projectile owner
                if (player.channel)
                {
                    float holdoutDistance = player.HeldItem.shootSpeed;
                    // Calculate a normalized vector from player to mouse and multiply by holdoutDistance to determine resulting holdoutOffset
                    Vector2 holdoutOffset = holdoutDistance * Vector2.Normalize(Main.MouseWorld - playerCenter);
                    if (holdoutOffset.X != Projectile.velocity.X || holdoutOffset.Y != Projectile.velocity.Y)
                    {
                        // This will sync the projectile, most importantly, the velocity.
                        Projectile.netUpdate = true;
                    }

                    // Projectile.velocity acts as a holdoutOffset for held projectiles.
                    Projectile.velocity = holdoutOffset;
                }
                else
                {
                    Projectile.Kill();
                    t = 0;
                    Projectile.ai[1] = 0;
                }
            }
            if (player.statMana <= 0)
            {
                Projectile.Kill();
                Projectile.timeLeft = 0;
            }
            if (Projectile.velocity.X > 0f)
            {
                player.ChangeDir(1);
            }
            else if (Projectile.velocity.X < 0f)
            {
                player.ChangeDir(-1);
            }
            const float rotation = 0.06f;
            player.ChangeDir(Projectile.direction); // Change the player's direction based on the projectile's own
            player.heldProj = Projectile.whoAmI; // We tell the player that the drill is the held projectile, so it will draw in their hand
            Projectile.Center = playerCenter;
            Projectile.rotation = Projectile.rotation.AngleLerp((new Vector2(Projectile.ai[0], Projectile.ai[1]) - Projectile.Center).ToRotation(), rotation);
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.ai[0] = Main.MouseWorld.X;
                Projectile.ai[1] = Main.MouseWorld.Y;
            }
            if (Projectile.ai[2] >= 0 && Projectile.ai[2] <= 59)
            {
                if (++Projectile.frameCounter >= 7)
                {
                    Projectile.frameCounter = 0;
                    // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                    if (++Projectile.frame >= Main.projFrames[Projectile.type])
                        Projectile.frame = 0;
                }
            }
            if (Projectile.ai[2] >= 60 && Projectile.ai[2] <= 119)
            {
                if (++Projectile.frameCounter >= 5)
                {
                    Projectile.frameCounter = 0;
                    // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                    if (++Projectile.frame >= Main.projFrames[Projectile.type])
                        Projectile.frame = 0;
                }
            }
            if (Projectile.ai[2] >= 120)
            {
                if (++Projectile.frameCounter >= 3)
                {
                    Projectile.frameCounter = 0;
                    // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                    if (++Projectile.frame >= Main.projFrames[Projectile.type])
                        Projectile.frame = 0;
                }
            }
            Projectile.ai[2]++;
        }

    }
}