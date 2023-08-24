using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items.Consumables;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class PlatinumScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Platinum Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("Pointy and shiny!" +
                "\n[c/f6d8eb:Special ability:] When used, the player throws a platinum scythe that sticks into ground/enemies, dealing damage in them"); */

        }

        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 52;
            Item.height = 52;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 7850;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 10f;
            Item.noUseGraphic = false;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noUseGraphic = true;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = false;
            }
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[ModContent.ProjectileType<PlatinumScytheProj>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                const int NumProjectiles = 1;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    SoundEngine.PlaySound(SoundID.Item71, player.position);
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(30));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 2f - Main.rand.NextFloat(0.4f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<PlatinumScytheProj>(), damage, knockback, player.whoAmI);

                }

            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.PlatinumBar, 20)
            .AddIngredient(ItemID.Wood, 15)
            .AddTile(TileID.Anvils)
            .Register();
        }
        public class PlatinumScytheProj : ModProjectile
        {
            public bool IsStickingToTarget
            {
                get => Projectile.ai[0] == 1f;
                set => Projectile.ai[0] = value ? 1f : 0f;
            }

            // Index of the current target
            public int TargetWhoAmI
            {
                get => (int)Projectile.ai[1];
                set => Projectile.ai[1] = value;
            }

            public int GravityDelayTimer
            {
                get => (int)Projectile.ai[2];
                set => Projectile.ai[2] = value;
            }

            public float StickTimer
            {
                get => Projectile.localAI[0];
                set => Projectile.localAI[0] = value;
            }
            public override void SetDefaults()
            {
                Projectile.penetrate = -1;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.damage = 25;
                Projectile.timeLeft = 180;
                Projectile.aiStyle = 0;
                Projectile.width = 42;
                Projectile.height = 66;
                Projectile.alpha = 0;
                Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            }
            private const int GravityDelay = 45;

            public override void AI()
            {
                if (IsStickingToTarget)
                {
                    StickyAI();
                }
                else
                {
                    NormalAI();
                }
            }

            private void NormalAI()
            {
                GravityDelayTimer++; // doesn't make sense.

                // For a little while, the javelin will travel with the same speed, but after this, the javelin drops velocity very quickly.
                if (GravityDelayTimer >= GravityDelay)
                {
                    GravityDelayTimer = GravityDelay;

                    // wind resistance
                    Projectile.velocity.X *= 0.98f;
                    // gravity
                    Projectile.velocity.Y += 0.35f;
                }

                // Offset the rotation by 90 degrees because the sprite is oriented vertiacally.
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            }

            private const int StickTime = 60 * 15; // 15 seconds
            private void StickyAI()
            {
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                StickTimer += 1f;

                // Every 30 ticks, the javelin will perform a hit effect
                bool hitEffect = StickTimer % 30f == 0f;
                int npcTarget = TargetWhoAmI;
                if (StickTimer >= StickTime || npcTarget < 0 || npcTarget >= 200)
                { // If the index is past its limits, kill it
                    Projectile.Kill();
                }
                else if (Main.npc[npcTarget].active && !Main.npc[npcTarget].dontTakeDamage)
                {
                    // If the target is active and can take damage
                    // Set the projectile's position relative to the target's center
                    Projectile.Center = Main.npc[npcTarget].Center - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.npc[npcTarget].gfxOffY;
                    if (hitEffect)
                    {
                        // Perform a hit effect here, causing the npc to react as if hit.
                        // Note that this does NOT damage the NPC, the damage is done through the debuff.
                        Main.npc[npcTarget].HitEffect(0, 1.0);
                    }
                }
                else
                { // Otherwise, kill the projectile
                    Projectile.Kill();
                }
            }

            public override void Kill(int timeLeft)
            {
                Vector2 usePos = Projectile.position; // Position to use for dusts

                // Offset the rotation by 90 degrees because the sprite is oriented vertiacally.
                Vector2 rotationVector = (Projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2(); // rotation vector to use for dust velocity
                usePos += rotationVector * 16f;
            }

            private const int MaxStickingJavelin = 6; // This is the max amount of javelins able to be attached to a single NPC
            private readonly Point[] stickingJavelins = new Point[MaxStickingJavelin]; // The point array holding for sticking javelins

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                IsStickingToTarget = true; // we are sticking to a target
                TargetWhoAmI = target.whoAmI; // Set the target whoAmI
                Projectile.velocity = (target.Center - Projectile.Center) *
                    0.75f; // Change velocity based on delta center of targets (difference between entity centers)
                Projectile.netUpdate = true; // netUpdate this javelin
                Projectile.KillOldestJavelin(Projectile.whoAmI, Type, target.whoAmI, stickingJavelins);
            }

            public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
            {
                // For going through platforms and such, javelins use a tad smaller size
                width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
                return true;
            }

            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                // By shrinking target hitboxes by a small amount, this projectile only hits if it more directly hits the target.
                // This helps the javelin stick in a visually appealing place within the target sprite.
                if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
                {
                    targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
                }
                // Return if the hitboxes intersects, which means the javelin collides or not
                return projHitbox.Intersects(targetHitbox);
            }
            public override bool OnTileCollide(Vector2 oldVelocity)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.rotation += 0f;
                return false;
            }
        }
    }
}