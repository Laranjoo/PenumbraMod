using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class EnergyConductorM : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Energy Conductor"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Main.projFrames[Projectile.type] = 12;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 30;
            Projectile.width = 18;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.knockBack = 3f;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage()
        {
            return true;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (++Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            if (!CheckActive(owner))
            {
                return;
            }

            Projectile.rotation = Projectile.velocity.X * 0.09f;
            GeneralBehavior(owner);
            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
        }
        private void GeneralBehavior(Player owner)
        {
            Projectile.ai[1]++;
            float dist = 40f;
            Projectile.rotation = Projectile.AngleTo(owner.Center);
            Projectile.position = owner.Center + Vector2.One.RotatedBy(Projectile.ai[1] / 12) * dist;
            Vector2 placeToGo = owner.Center;
            Vector2 goTo = Projectile.Center.DirectionTo(placeToGo) * 6f;
            if (Projectile.Center.DistanceSQ(placeToGo) < 2 * 2)
            {
                Projectile.Center = placeToGo;
                Projectile.velocity = Vector2.Zero;
            }
            else
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, goTo, 0.1f);

            // If your minion is flying, you want to do this independently of any conditions
            float overlapVelocity = 0.05f;

            // Fix overlap with other minions
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];

                if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
                {
                    if (Projectile.position.X < other.position.X)
                    {
                        Projectile.velocity.X -= overlapVelocity;
                    }
                    else
                    {
                        Projectile.velocity.X += overlapVelocity;
                    }

                    if (Projectile.position.Y < other.position.Y)
                    {
                        Projectile.velocity.Y -= overlapVelocity;
                    }
                    else
                    {
                        Projectile.velocity.Y += overlapVelocity;
                    }
                }
            }
        }
        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            // Starting search distance
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;

            // This code is required if your minion weapon has the targeting feature
            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 800f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }
            if (!foundTarget)
            {

                // This code is required either way, used for finding a target
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                        bool closeThroughWall = between < 100f;

                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }
            NPC closestNPC = FindClosestNPC(distanceFromTarget);
            if (closestNPC == null)
                return;
            if (foundTarget)
            {
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] == 1)
                {
                    int radius1 = 30;
                    const int Repeats = 40;
                    for (int i = 0; i < Repeats; ++i)
                    {
                        Vector2 position = Projectile.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                        int d = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 1.9f;
                    }
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Projectile.DirectionTo(closestNPC.Center) * 12f, ModContent.ProjectileType<EnergyConductorP>(), 30, 3f, Main.myPlayer);
                    SoundEngine.PlaySound(SoundID.Item72, Projectile.Center);
                }
                if (Projectile.ai[0] >= 24)
                {
                    Projectile.ai[0] = 0;
                }
            }
        }
        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
                if (target.CanBeChasedBy())
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }

        // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<EnergyConductor>());

                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<EnergyConductor>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }
    }
}