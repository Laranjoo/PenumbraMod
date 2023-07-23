using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Common;
using PenumbraMod.Content.Buffs;
using ReLogic.Utilities;
using System.Collections.Generic;
using System.Security.Policy;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.NPCs.Bosses.Eyestorm
{
    // The minions spawned when the body spawns
    // Please read MinionBossBody.cs first for important comments, they won't be explained here again
    public class EnergyConductorMinion : ModNPC
    {
        // This is a neat trick that uses the fact that NPCs have all NPC.ai[] values set to 0f on spawn (if not otherwise changed).
        // We set ParentIndex to a number in the body after spawning it. If we set ParentIndex to 3, NPC.ai[0] will be 4. If NPC.ai[0] is 0, ParentIndex will be -1.
        // Now combine both facts, and the conclusion is that if this NPC spawns by other means (not from the body), ParentIndex will be -1, allowing us to distinguish
        // between a proper spawn and an invalid/"cheated" spawn
        public int ParentIndex {
            get => (int)NPC.ai[0] - 1;
            set => NPC.ai[0] = value + 1;
        }

        public bool HasParent => ParentIndex > -1;

        public int PositionIndex {
            get => (int)NPC.ai[1] - 1;
            set => NPC.ai[1] = value + 1;
        }

        public bool HasPosition => PositionIndex > -1;

        // Helper method to determine the body type
        public static int BodyType() {
            return ModContent.NPCType<Eyeofthestorm>();
        }

        public override void SetStaticDefaults() {
            // DisplayName.SetDefault("Energy Conductor Protector");
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.TrailCacheLength[NPC.type] = 12; //How many copies of shadow/trail
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            // By default enemies gain health and attack if hardmode is reached. this NPC should not be affected by that
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            // Enemies can pick up coins, let's prevent it for this NPC
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Specify the debuffs it is immune to
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Poisoned,
                    ModContent.BuffType<StunnedNPC>(),
                    ModContent.BuffType<LowVoltage>(),
                    BuffID.Confused // Most NPCs have this
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

            // Optional: If you don't want this NPC to show on the bestiary (if there is no reason to show a boss minion separately)
            // Make sure to remove SetBestiary code aswell
            // NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
            //	Hide = true // Hides this NPC from the bestiary
            // };
            // NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
        }

        public override void SetDefaults() {
            NPC.width = 30;
            NPC.height = 30;
            NPC.defense = 10;
            NPC.lifeMax = 1000;
            NPC.HitSound = SoundID.NPCHit34;
            NPC.DeathSound = new SoundStyle("PenumbraMod/Assets/Sounds/SFX/ShieldBreakOut")
            {
                Volume = 3.1f,
                PitchVariance = 0.3f,
            };
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0f;
			NPC.alpha = 255; // This makes it transparent upon spawning, we have to manually fade it in in AI()
			NPC.aiStyle = -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
			int associatedNPCType = BodyType();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);

			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert, // Plain black background
				new FlavorTextBestiaryInfoElement("Protective shield of the stormy eye, their only objective is protect his owner at every cost.")
			});
		}
        public override void FindFrame(int frameHeight)
        {
            // This NPC animates with a simple "go from start frame to final frame, and loop back to start frame" rule
            // In this case: First stage: 0-1-2-0-1-2, Second stage: 3-4-5-3-4-5, 5 being "total frame count - 1"
            int startFrame = 0;
            int finalFrame = 4;
            int frameSpeed = 5;
            NPC.frameCounter += 0.7f;
            if (NPC.frameCounter > frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= finalFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }
        }
        public override Color? GetAlpha(Color drawColor) {
			if (NPC.IsABestiaryIconDummy) {
				// This is required because we have NPC.alpha = 255, in the bestiary it would look transparent
				return NPC.GetBestiaryEntryColor();
			}
			return Color.White * NPC.Opacity;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
			return true;
		}

		public override void HitEffect(NPC.HitInfo hit) {
			if (NPC.life <= 0) {
				for (int i = 0; i < 40; i++) {
					Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
					Dust dust = Dust.NewDustPerfect(NPC.Center, DustID.BlueTorch, velocity, 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));
					dust.noGravity = true;
				}
			}
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor) //PreDraw for trails
        {
            if (!NPC.IsABestiaryIconDummy)
            {
                Main.instance.LoadProjectile(NPC.type);
                Texture2D texture = TextureAssets.Npc[NPC.type].Value;

                // Redraw the projectile with the color not influenced by light
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, NPC.gfxOffY);
                    Color color = NPC.GetAlpha(lightColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    color.A = 0;
                    Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.oldRot[k], drawOrigin, NPC.scale, SpriteEffects.None, 0);
                }
            }

            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glowMask = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;
            var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                if (NPC.alpha < 50)
                {
                    spriteBatch.Draw(glowMask, NPC.Center - screenPos, NPC.frame, PenumbraMod.Eyestorm, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
                }

            }
        }
        public override void AI() {
			if (Despawn()) {
				return;
			}
            NPC.alpha -= 10;
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }
            
            MoveInFormation();
		}
		private bool Despawn() {
			if (Main.netMode != NetmodeID.MultiplayerClient &&
				(!HasPosition || !HasParent || !Main.npc[ParentIndex].active || Main.npc[ParentIndex].type != BodyType())) {
				// * Not spawned by the boss body (didn't assign a position and parent) or
				// * Parent isn't active or
				// * Parent isn't the body
				// => invalid, kill itself without dropping any items
				NPC.active = false;
				NPC.life = 0;
				NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
				return true;
			}
			return false;
		}
        public bool DrawShield => Main.npc[(int)NPC.ai[0]].life >= 15;
       
        private void MoveInFormation() {
			NPC parentNPC = Main.npc[ParentIndex];
            var entitySource = NPC.GetSource_FromAI();
            NPC.ai[2]++;
			NPC.ai[3]++;
            if (NPC.ai[2] == 1)
            {
                Projectile.NewProjectile(entitySource, NPC.Center, Vector2.Zero, ModContent.ProjectileType<LoopSoundProj>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                if (Main.npc[PenumbraGlobalNPC.eyeStorm].ai[2] > 200)
                    SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/SFX/ShieldBreakIn"));
            }
			NPC.Center = parentNPC.Center + new Vector2(200, 0).RotatedBy(NPC.ai[3] / 16);
           
            int type = ModContent.ProjectileType<EyeprojEmpty>();
            Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(parentNPC.Center) * 12f, type, 0, 0f, Main.myPlayer);
            int radius1 = 80;
            const int Repeats = 20;
            for (int i = 0; i < Repeats; ++i)
            {
                Vector2 position = parentNPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                int d = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 4.9f;
            }
            parentNPC.dontTakeDamage = true;
            if (parentNPC.life < 30)
            {
                NPC.life = 0;
                NPC.active = false;
            }
            if (parentNPC.ModNPC is Eyeofthestorm m)
            {
                if (DrawShield)
                {
                    m.doodo = true;
                }
                else
                {
                    m.doodo = false;
                }
            }
        }
        
	}
    public class EnergyConductorMinion2 : ModNPC
    {
        // This is a neat trick that uses the fact that NPCs have all NPC.ai[] values set to 0f on spawn (if not otherwise changed).
        // We set ParentIndex to a number in the body after spawning it. If we set ParentIndex to 3, NPC.ai[0] will be 4. If NPC.ai[0] is 0, ParentIndex will be -1.
        // Now combine both facts, and the conclusion is that if this NPC spawns by other means (not from the body), ParentIndex will be -1, allowing us to distinguish
        // between a proper spawn and an invalid/"cheated" spawn
        public int ParentIndex
        {
            get => (int)NPC.ai[0] - 1;
            set => NPC.ai[0] = value + 1;
        }

        public bool HasParent => ParentIndex > -1;

        public int PositionIndex
        {
            get => (int)NPC.ai[1] - 1;
            set => NPC.ai[1] = value + 1;
        }

        public bool HasPosition => PositionIndex > -1;

        public const float RotationTimerMax = 360;
        public ref float RotationTimer => ref NPC.ai[2];

        // Helper method to determine the body type
        public static int BodyType()
        {
            return ModContent.NPCType<Eyeofthestorm>();
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Energy Conductor Shooter");
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.TrailCacheLength[NPC.type] = 12; //How many copies of shadow/trail
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            // By default enemies gain health and attack if hardmode is reached. this NPC should not be affected by that
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            // Enemies can pick up coins, let's prevent it for this NPC
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Specify the debuffs it is immune to
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Poisoned,
                    ModContent.BuffType<StunnedNPC>(),
                    ModContent.BuffType<LowVoltage>(),
                    BuffID.Confused // Most NPCs have this
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
        }

        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 30;
            NPC.defense = 10;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit34;
            NPC.DeathSound = SoundID.Item14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.alpha = 255; // This makes it transparent upon spawning, we have to manually fade it in in AI()
            NPC.aiStyle = -1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
            int associatedNPCType = BodyType();
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);

            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert, // Plain black background
				new FlavorTextBestiaryInfoElement("Protective shooter of the stormy eye, their only objective is protect his owner at every cost.")
            });
        }

        public override Color? GetAlpha(Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                // This is required because we have NPC.alpha = 255, in the bestiary it would look transparent
                return NPC.GetBestiaryEntryColor();
            }
            return Color.White * NPC.Opacity;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor) //PreDraw for trails
        {
            if (!NPC.IsABestiaryIconDummy)
            {
                Main.instance.LoadProjectile(NPC.type);
                Texture2D texture = TextureAssets.Npc[NPC.type].Value;

                // Redraw the projectile with the color not influenced by light
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, NPC.gfxOffY);
                    Color color = NPC.GetAlpha(lightColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    color.A = 0;
                    Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.oldRot[k], drawOrigin, NPC.scale, SpriteEffects.None, 0);
                }
            }

            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glowMask = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;
            var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                if (NPC.alpha < 50)
                {
                    spriteBatch.Draw(glowMask, NPC.Center - screenPos, NPC.frame, PenumbraMod.Eyestorm, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
                }

            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                // If this NPC dies, spawn some visuals

                int dustType = 59; // Some blue dust, read the dust guide on the wiki for how to find the perfect dust

                for (int i = 0; i < 20; i++)
                {
                    Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                    Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));

                    dust.noLight = true;
                    dust.noGravity = true;
                    dust.fadeIn = Main.rand.NextFloat(0.3f, 0.8f);
                }
            }
        }

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            if (Despawn())
            {
                return;
            }
            NPC.alpha -= 10;
            if (NPC.alpha < 0)
            {
                NPC.alpha = 0;
            }
            Player player = Main.player[NPC.target];
            MoveInFormation(player);
        }

        private bool Despawn()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient &&
                (!HasPosition || !HasParent || !Main.npc[ParentIndex].active || Main.npc[ParentIndex].type != BodyType()))
            {
                // * Not spawned by the boss body (didn't assign a position and parent) or
                // * Parent isn't active or
                // * Parent isn't the body
                // => invalid, kill itself without dropping any items
                NPC.active = false;
                NPC.life = 0;
                NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                return true;
            }
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            // This NPC animates with a simple "go from start frame to final frame, and loop back to start frame" rule
            // In this case: First stage: 0-1-2-0-1-2, Second stage: 3-4-5-3-4-5, 5 being "total frame count - 1"
            int startFrame = 0;
            int finalFrame = 4;
            int frameSpeed = 5;
            NPC.frameCounter += 0.7f;
            if (NPC.frameCounter > frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= finalFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }
        }
        private void MoveInFormation(Player player)
        {
            NPC parentNPC = Main.npc[ParentIndex];
            if (parentNPC.life < 30)
            {
                NPC.life = 0;
                NPC.active = false;
            }
            // This basically turns the NPCs PositionIndex into a number between 0f and TwoPi to determine where around
            // the main body it is positioned at
            float rad = (float)PositionIndex / Eyeofthestorm.Minion() * MathHelper.TwoPi;

            // Add some slight uniform rotation to make the eyes move, giving a chance to touch the player and thus helping melee players
            RotationTimer += 0.5f;
            if (RotationTimer > RotationTimerMax)
            {
                RotationTimer = 0;
            }
            NPC.rotation = NPC.velocity.X / 16;
            // Since RotationTimer is in degrees (0..360) we can convert it to radians (0..TwoPi) easily
            float continuousRotation = MathHelper.ToRadians(RotationTimer);
            rad += continuousRotation;
            if (rad > MathHelper.TwoPi)
            {
                rad -= MathHelper.TwoPi;
            }
            else if (rad < 0)
            {
                rad += MathHelper.TwoPi;
            }

            float distanceFromBody = parentNPC.width + NPC.width;

            // offset is now a vector that will determine the position of the NPC based on its index
            Vector2 offset = Vector2.One.RotatedBy(rad) * distanceFromBody;

            Vector2 destination = parentNPC.Center + offset;
            Vector2 toDestination = destination - NPC.Center;
            Vector2 toDestinationNormalized = toDestination.SafeNormalize(Vector2.Zero);

            float speed = 12f;
            float inertia = 20;

            Vector2 moveTo = toDestinationNormalized * speed;
            NPC.velocity = (NPC.velocity * (inertia - 1) + moveTo) / inertia;
            NPC.ai[3]++;
            var entitySource = NPC.GetSource_FromAI();
            int type = ModContent.ProjectileType<ConductorProj>();
            int typ2e = ModContent.ProjectileType<Brightness10>();
            if (NPC.ai[3] == 100)
            {
                SoundEngine.PlaySound(SoundID.Item72, NPC.Center);
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 10f, type, 5, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 0f, typ2e, 5, 0f, Main.myPlayer);
                NPC.ai[3] = 0;
            }
        }
    }
}
