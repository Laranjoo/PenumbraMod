using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Common;
using PenumbraMod.Common.Systems;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Placeable.Furniture;
using PenumbraMod.Content.Items.Consumables;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.NPCs.Bosses.Eyestorm
{
    [AutoloadBossHead]
    public class Eyeofthestorm : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[NPC.type] = 12; //How many copies of shadow/trail
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            Main.npcFrameCount[Type] = 10;


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
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public bool SpawnedMinions;
        public bool SpawnedMinions2;
        public override void SetDefaults()
        {
            NPC.width = 128;
            NPC.height = 136;
            NPC.damage = 20;
            NPC.defense = 15;
            NPC.lifeMax = 3450;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.Roar;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(gold: 5);
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 200f;
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/EyestormSong1");
            SceneEffectPriority = SceneEffectPriority.BossLow;
        }
        int radius1 = 120;
        public bool to = false;
        public override void AI()
        {
            PenumbraGlobalNPC.eyeStorm = NPC.whoAmI;
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

            if (player.dead)
            {
                // If the targeted player is dead, flee
                NPC.velocity.Y -= 0.04f;
                NPC.alpha += 5;
                // This method makes it so when the boss is in "despawn range" (outside of the screen), it despawns in 10 ticks
                NPC.EncourageDespawn(10);
                return;
            }
            // PHASE 2
            if (NPC.life < NPC.lifeMax / 2)
            {
                DoSecondPhase(player);
            }
            else if (NPC.life <= 10)
            {
                Death(player);
                NPC.life = 10;
                NPC.netUpdate = true;
                NPC.ai[0] = 0;
                NPC.ai[2] = 0;
                to = true;
                NPC.dontTakeDamage = true;
            }
            else
            {
                FirstPhase(player);
            }
        }
        public bool w = false;
        public bool c = false;
        public bool o = false;
        public bool d = false;
        public int a;
        public int b;
        int cam;
        int idontcare;
        Vector2 QWERTY = Vector2.Zero;

        private void FirstPhase(Player player)
        {
            NPC.ai[0]++;
            NPC.ai[1]++;
            NPC.localAI[0]++;
            NPC.localAI[1]++;
            cam++;
            idontcare++;
            Main.windSpeedCurrent = 1;
            if (NPC.life < NPC.lifeMax / 2)
            {
                DoSecondPhase(player);
                NPC.ai[0] = 0;
                NPC.ai[1] = 0;
                NPC.netUpdate = true;
            }
            float speed = 2f;
            float speed2 = 8f;
            float speed3 = 0f;
            if (NPC.ai[0] == 1)
            {
                NPC.alpha = 255;
                NPC.netUpdate = true;
            }
            //APPEAR AT PLAYER POS AND MAKE BOOM EFFECT
            if (idontcare == 51)
            {
                for (int k = 0; k < 50; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 2.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
            }
            if (NPC.ai[0] == 49)
            {
                PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 16f, 60, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
                NPC.netUpdate = true;
            }
            if (cam == 191)
            {
                NPC.netUpdate = true;
                PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 12f, 12f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
            }
            // CONTROL CAMERA
            if (cam >= 52 && cam <= 160)
            {
                player.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = true;
                player.GetModPlayer<PenumbraGlobalPlayer>().absolutepos = NPC.Center;
            }
            // POSITION
            if (cam >= 50 && cam <= 160)
            {
                float range2 = 2500f * 16f;
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.alpha = 0;
                    NPC.position = player.Center + new Vector2(-60, -250);
                }
            }
            if (cam >= 161 && cam <= 190)
            {
                player.GetModPlayer<PenumbraGlobalPlayer>().absolutepos = player.Center;
            }
            // START FOLLOWING
            if (NPC.ai[1] >= 190 && NPC.ai[1] <= 389)
            {
                NPC.velocity = NPC.DirectionTo(player.Center) * speed;
            }
            // SHOOT PROJECTILES
            if (NPC.ai[0] == 190 || NPC.ai[0] == 240 || NPC.ai[0] == 290 || NPC.ai[0] == 340)
            {
                player.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = false;
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                NPC.position += NPC.velocity;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                int velocity = 1;
                int type = ModContent.ProjectileType<Eyeproj>();

                int damage = NPC.damage / 4;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 14f * velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            // GLOW EFFECT
            if (NPC.ai[1] == 160 || NPC.ai[1] == 161 || NPC.ai[1] == 210 || NPC.ai[1] == 211 || NPC.ai[1] == 260 || NPC.ai[1] == 261 || NPC.ai[1] == 310 || NPC.ai[1] == 311)
            {
                NPC.alpha = 0;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            // EFFECT
            if (NPC.ai[0] == 160 || NPC.ai[0] == 210 || NPC.ai[0] == 260 || NPC.ai[0] == 310)
            {
                NPC.friendly = false;
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness3>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;
            }
            // SOUND
            if (NPC.ai[0] == 389)
            {
                NPC.dontTakeDamage = true;
                SoundEngine.PlaySound(SoundID.Roar);
                for (int k = 0; k < 30; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 2.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                NPC.netUpdate = true;
            }
            // DASH INTO PLAYER
            if (NPC.ai[1] == 390)
            {
                float k = 12f;
                NPC.velocity = NPC.DirectionTo(player.Center) * k;
                NPC.dontTakeDamage = true;

            }
            if (NPC.ai[1] >= 391 && NPC.ai[1] <= 428)
            {
                NPC.velocity *= 0.97f;
            }
            // FADE
            if (NPC.ai[0] >= 393 && NPC.ai[0] <= 427)
            {
                NPC.alpha += 20;
                if (NPC.alpha > 255)
                {
                    NPC.alpha = 255;
                }
                NPC.netUpdate = true;
            }
            // MAKE IT DONT HURT PLAYER WHEN STATIONARY
            if (NPC.ai[1] == 429)
            {
                NPC.friendly = true;
                NPC.netUpdate = true;
            }
            // STATIONARY
            if (NPC.ai[1] == 430)
            {
                NPC.velocity = NPC.DirectionTo(player.Center) * 0f;
                NPC.netUpdate = true;
            }
            // APPEAR AT PLAYER POS AGAIN
            if (NPC.ai[0] >= 480 && NPC.ai[0] <= 539)
            {
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-60, -300);
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                NPC.alpha -= 20;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
            }
            // GLOW
            if (NPC.ai[0] == 508)
            {
                NPC.friendly = false;
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;
            }
            if (NPC.ai[1] >= 510 && NPC.ai[1] <= 540)
            {
                NPC.alpha += 20;
            }
            // EFFECT
            if (NPC.ai[0] == 510)
            {
                NPC.friendly = false;
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<BrightnessDeath>();
                if (Main.expertMode || Main.masterMode)
                {
                    Attackers(player);
                }
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;
            }
            if (NPC.ai[0] == 540)
            {
                NPC.friendly = false;
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness8>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;
            }
            // SPIN MOVEMEMNT AND ALSO SHOOTING PROJECTILES
            if (NPC.ai[0] >= 540 && NPC.ai[0] <= 640)
            {
                NPC.alpha -= 20;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
                QWERTY = player.Center;
                NPC.Center = QWERTY + new Vector2(300, 0).RotatedBy(NPC.ai[0] / 14);

                NPC.dontTakeDamage = false;
            }
            // PROJECTILES
            if (NPC.ai[1] == 560 || NPC.ai[1] == 580 || NPC.ai[1] == 600 || NPC.ai[1] == 620 || NPC.ai[1] == 640)
            {

                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                NPC.position += NPC.velocity;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                int velocity = 1;
                int type = ModContent.ProjectileType<Eyeproj2>();

                int damage = NPC.damage / 4;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
            }
            // GLOW EFFECT
            if (NPC.ai[0] == 560 || NPC.ai[0] == 580 || NPC.ai[0] == 600 || NPC.ai[0] == 620 || NPC.ai[0] == 640)
            {

                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness2Death>();

                Vector2 velocity2 = speed2 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            // DASH INTO PLAYER
            if (NPC.ai[0] == 641)
            {
                float k = 16f;
                NPC.velocity = NPC.DirectionTo(player.Center) * k;
                NPC.dontTakeDamage = true;
                NPC.netUpdate = true;
                var entitySource = NPC.GetSource_FromAI();
                Vector2 launchVelocity = new Vector2(-4, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<Eyeproj2>(), NPC.damage / 3, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                }
                const int Repeats = 80;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int d = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 1.9f;
                }
                radius1 += 2;
            }
            if (NPC.ai[0] >= 642 && NPC.ai[0] <= 699)
            {
                NPC.alpha += 10;
                if (NPC.alpha > 255)
                {
                    NPC.alpha = 255;
                }
                NPC.velocity *= 0.98f;
                NPC.netUpdate = true;
            }
            if (NPC.ai[1] == 670)
            {
                NPC.friendly = false;
                NPC.position += NPC.velocity;
                NPC.netUpdate = true;
                NPC.velocity = NPC.DirectionTo(player.Center) * 0f;
            }

            // FOLLOW AGAIN AND FADE
            if (NPC.ai[1] >= 720 && NPC.ai[1] <= 930)
            {
                NPC.alpha -= 20;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
                NPC.netUpdate = true;
                NPC.dontTakeDamage = false;
                NPC.friendly = false;
                NPC.velocity = NPC.DirectionTo(player.Center) * speed;
            }
            // BEAM PROJECTILES
            if (NPC.ai[1] == 760 || NPC.ai[1] == 820 || NPC.ai[1] == 880)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.friendly = false;
                NPC.position += NPC.velocity;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                Vector2 velocity2 = player.Center + player.velocity * 30;
                Vector2 papa = Vector2.Zero;
                int type = ModContent.ProjectileType<LazerBeam>();
                int type2 = ModContent.ProjectileType<Line5>();

                int damage = 20;
                Projectile.NewProjectile(entitySource, velocity2, papa, type, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, velocity2, papa, type2, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
            }
            // EFFECT
            if (NPC.ai[0] == 760 || NPC.ai[0] == 820 || NPC.ai[0] == 880)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.friendly = false;
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness3>();

                Vector2 velocity2 = speed3 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;
            }
            //DASH AND FADE
            if (NPC.ai[1] == 932)
            {
                NPC.friendly = false;
                float d = 12f;
                NPC.velocity = NPC.DirectionTo(player.Center) * d;
                NPC.dontTakeDamage = true;
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

            }
            if (NPC.ai[1] >= 933 && NPC.ai[1] <= 979)
            {
                NPC.alpha += 10;
                if (NPC.alpha > 255)
                {
                    NPC.alpha = 255;
                }
                NPC.friendly = false;
                NPC.velocity *= 0.97f;
            }
            if (NPC.ai[1] >= 981 && NPC.ai[1] <= 1271)
            {
                NPC.alpha -= 10;
                if (NPC.alpha < 180)
                {
                    NPC.alpha = 180;
                }
                NPC.friendly = false;
                float d = 2f;
                NPC.velocity = NPC.DirectionTo(player.Center) * d;
            }
            // EFFECT
            if (NPC.ai[0] == 950)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.friendly = false;
                NPC.position += NPC.velocity;

                int type2 = ModContent.ProjectileType<Brightness6>();
                int type3 = ModContent.ProjectileType<Brightness7>();
                int type4 = ModContent.ProjectileType<Brightness8>();
                Vector2 offset = player.Center + new Vector2(-90, -300);
                Vector2 velocity2 = speed3 * new Vector2(0, 0);
                if (Main.expertMode || Main.masterMode)
                {
                    Attackers(player);
                }
                int damage2 = 0;

                Projectile.NewProjectile(entitySource, offset, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type3, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type4, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            // APPEAR AT PLAYER POS AGAIN
            if (NPC.ai[1] == 980)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-90, -300);
                    // Teleport
                }
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                NPC.alpha = 0;
                NPC.dontTakeDamage = false;

                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness2Death>();

                Vector2 velocity2 = speed2 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
            }
            // EFFECT
            if (NPC.ai[0] == 992)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.friendly = false;
                NPC.position += NPC.velocity;

                int type2 = ModContent.ProjectileType<Brightness6>();
                int type3 = ModContent.ProjectileType<Brightness7>();
                int type4 = ModContent.ProjectileType<Brightness8>();
                Vector2 offset = player.Center + new Vector2(240, -275);
                Vector2 velocity2 = speed3 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, offset, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type3, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type4, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            // APPEAR AT RANDOM POSITIONS
            // FIRST TELEPORT
            if (NPC.ai[1] == 1022)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(270, -375);
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();

                Vector2 velocity2 = new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness6>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness7>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness8>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
            }
            // SECOND TELEPORT
            if (NPC.ai[1] == 1064)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-270, 375);
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();

                Vector2 velocity2 = new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness6>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness7>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness8>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
            }
            // THIRD TELEPORT
            if (NPC.ai[1] == 1095)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-350, 50);
                    // Teleport
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();

                Vector2 velocity2 = new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness6>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness7>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness8>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
            }
            // FOURTH TELEPORT
            if (NPC.ai[1] == 1143)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-210, -250);
                    // Teleport
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();

                Vector2 velocity2 = new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness6>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness7>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness8>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
            }
            // FIFTH TELEPORT
            if (NPC.ai[1] == 1193)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(310, -50);
                    // Teleport
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();

                Vector2 velocity2 = new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness6>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness7>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness8>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
            }
            // SIXTH TELEPORT
            if (NPC.ai[1] == 1213)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-330, 30);
                    // Teleport
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();

                Vector2 velocity2 = new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness6>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness7>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness8>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
            }
            // LAST TELEPORT
            if (NPC.ai[1] == 1253)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(200, -80);
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();

                Vector2 velocity2 = new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness6>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness7>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, ModContent.ProjectileType<Brightness8>(), 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
            }
            // EXPLOSION
            if (NPC.ai[1] == 1052 || NPC.ai[1] == 1114 || NPC.ai[1] == 1167 || NPC.ai[1] == 1213 || NPC.ai[1] == 1252)
            {
                NPC.friendly = false;
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                NPC.position += NPC.velocity;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                int velocity = 1;
                int type = ModContent.ProjectileType<EXBEAM>();

                int damage = NPC.damage / 4;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 16f * velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                const int Repeats = 80;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int d = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 1.9f;
                }
                radius1 -= 4;
                NPC.netUpdate = true;
            }
            if (Main.masterMode)
            {
                if (NPC.ai[0] == 1050 || NPC.ai[0] == 1120)
                {
                    NPC.friendly = false;
                    NPC.dontTakeDamage = false;
                    var entitySource = NPC.GetSource_FromAI();
                    NPC.position += NPC.velocity;

                    Vector2 velocity = new(0, 0);
                    int type2 = ModContent.ProjectileType<LightningA>();
                    int type3 = ModContent.ProjectileType<LightningB>();
                    int type4 = ModContent.ProjectileType<LightningC>();
                    int type5 = ModContent.ProjectileType<LightningD>();
                    int type6 = ModContent.ProjectileType<LightningE>();

                    Projectile.NewProjectile(entitySource, player.Center, velocity, type2, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    Projectile.NewProjectile(entitySource, player.Center, velocity, type3, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    Projectile.NewProjectile(entitySource, player.Center, velocity, type4, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    Projectile.NewProjectile(entitySource, player.Center, velocity, type5, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    Projectile.NewProjectile(entitySource, player.Center, velocity, type6, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    NPC.netUpdate = true;
                }
            }
            // EFFECT
            if (NPC.ai[1] == 1270)
            {

                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<BrightnessDeath>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, player.Center, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;
            }
            // LAST ATTACK, SPINS AND SHOOTS A LOT OF PROJECTILES
            if (NPC.ai[0] >= 1280 && NPC.ai[0] <= 1560)
            {
                NPC.alpha = 0;
                QWERTY = player.Center;
                NPC.Center = QWERTY + new Vector2(300, 0).RotatedBy(NPC.ai[0] / 10);
                NPC.dontTakeDamage = false;
            }
            // SHOOT PROJECTILES
            if (NPC.ai[0] == 1290 || NPC.ai[0] == 1310 || NPC.ai[0] == 1340 || NPC.ai[0] == 1370 || NPC.ai[0] == 1390 || NPC.ai[0] == 1410 || NPC.ai[0] == 1450 || NPC.ai[0] == 1470 || NPC.ai[0] == 1500 || NPC.ai[0] == 1510 || NPC.ai[0] == 1520 || NPC.ai[0] == 1530 || NPC.ai[0] == 1540)
            {

                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                NPC.position += NPC.velocity;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                int velocity = 1;
                int type = ModContent.ProjectileType<Eyeproj3>();

                int damage = NPC.damage / 4;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 1f * velocity, type, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            // EFFECT
            if (NPC.ai[1] == 1600)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();

                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness3>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 1f * velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;
            }
            if (NPC.ai[1] >= 1541 && NPC.ai[1] <= 1619)
            {
                NPC.dontTakeDamage = false;
                NPC.position += NPC.velocity;
                NPC.velocity *= 0.96f;
                NPC.alpha += 20;
                if (NPC.alpha > 255)
                {
                    NPC.alpha = 255;
                    NPC.velocity = NPC.DirectionTo(player.Center) * 0f;
                }

                NPC.netUpdate = true;
            }
            // RESET AND SHOOT PROJECTILES IN CIRCLE
            if (NPC.ai[0] == 1620)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<BrightnessDeath>();
                NPC.alpha = 0;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
                NPC.velocity = NPC.DirectionTo(player.Center) * 0f;
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                Vector2 launchVelocity = new Vector2(-14, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<Eyeproj>(), NPC.damage / 3, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                }
                NPC.ai[0] = 150;
                NPC.ai[1] = 150;
                NPC.localAI[0] = 150;
                NPC.localAI[1] = 150;
                NPC.netUpdate = true;
                PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
                const int Repeats = 80;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int d = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 4.9f;
                }
                radius1 += 5;
            }
        }
        public void DoSecondPhase(Player player)
        {
            NPC.ai[2]++;
            NPC.ai[3]++;
            NPC.localAI[2]++;
            NPC.localAI[3]++;
            Main.windSpeedCurrent = 2;
            Main.UseStormEffects = true;
            // BOSS DEATH ANIMATION
            if (NPC.life <= 10)
            {
                Death(player);
                NPC.life = 1;
                NPC.netUpdate = true;
                NPC.ai[2] = 0;
                NPC.ai[3] = 0;
                to = true;
            }
            float speed2 = 8f;
            float speed3 = 0f;
            NPC.defense = 28;

            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/EyestormMusic");
            if (NPC.ai[2] == 1)
            {
                NPC.life = NPC.lifeMax / 2 - 2;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<LazerBeam2>();
                int type2 = ModContent.ProjectileType<Line6>();

                int damage = 10;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type2, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                NPC.netUpdate = true;
                NPC.dontTakeDamage = true;
                PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 12f, 30, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
                player.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = true;
                player.GetModPlayer<PenumbraGlobalPlayer>().absolutepos = NPC.Center;
            }
            if (NPC.ai[2] == 5)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                SoundEngine.PlaySound(SoundID.Item89, NPC.Center);
                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<BrightnessDeath>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            // STOP AND FADE
            if (NPC.ai[2] > 10)
            {
                NPC.alpha += 30;
                if (NPC.alpha > 250)
                {
                    NPC.alpha = 250;
                }

                float d = 0f;
                NPC.velocity = NPC.DirectionTo(player.Center) * d;
                NPC.position += NPC.velocity;

                NPC.netUpdate = true;


            }
            // EFFECTS
            if (NPC.ai[3] == 5 || NPC.ai[3] == 30 || NPC.ai[3] == 40 || NPC.ai[3] == 50 || NPC.ai[3] == 60 || NPC.ai[3] == 70)
            {
                NPC.dontTakeDamage = true;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                SoundEngine.PlaySound(SoundID.Item89, NPC.Center);
                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness6>();
                int type2 = ModContent.ProjectileType<EXBEAMHIT>();
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type2, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;


            }
            // EFFECT
            if (NPC.ai[2] == 50)
            {
                NPC.dontTakeDamage = true;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness7>();
                int type2 = ModContent.ProjectileType<EXBEAMHIT>();
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type2, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;


            }
            if (NPC.ai[3] == 121)
            {
                SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/SFX/ShieldBuild"));
            }
            if (NPC.ai[3] == 120 || NPC.ai[3] == 150)
            {    
                NPC.dontTakeDamage = true;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                SoundEngine.PlaySound(SoundID.Item100, NPC.Center);
                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness8>();
                int type2 = ModContent.ProjectileType<Brightness7>();
                int type3 = ModContent.ProjectileType<Brightness4>();
                int type4 = ModContent.ProjectileType<EXBEAMHIT>();
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type2, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type3, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type4, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                if (Main.masterMode)
                {
                    Conductors(player);
                }

                const int Repeats = 80;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int d = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 4.9f;
                }
                radius1 -= 20;
                NPC.netUpdate = true;


            }
            // FADE A BIT
            if (NPC.ai[3] == 120)
            {
                NPC.dontTakeDamage = true;
                NPC.position += NPC.velocity;
                NPC.alpha -= 20;
                NPC.netUpdate = true;
            }
            // EFFECT
            if (NPC.ai[3] == 170)
            {
                NPC.dontTakeDamage = true;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<BrightnessP2>();
                int type2 = ModContent.ProjectileType<BrightnessP22>();
                int type3 = ModContent.ProjectileType<Brightness9>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type2, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type3, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;


            }
            if (NPC.ai[3] == 240)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.friendly = false;
                NPC.position += NPC.velocity;
                Vector2 velocity2 = NPC.Center + new Vector2(0, 0);
                Vector2 papa = Vector2.Zero;
                int type = ModContent.ProjectileType<LazerBeam>();
                int type2 = ModContent.ProjectileType<Line5>();

                int damage = 10;
                Projectile.NewProjectile(entitySource, velocity2, papa, type, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, velocity2, papa, type2, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                player.GetModPlayer<PenumbraGlobalPlayer>().absolutepos = player.Center;

                NPC.netUpdate = true;


            }
            //EFFECT AND START PHASE 2
            if (NPC.ai[3] == 270)
            {
                NPC.dontTakeDamage = true;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<BrightnessDeath>();
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                NPC.dontTakeDamage = false;
                int damage = 0;
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                Vector2 launchVelocity = new Vector2(-5, 1); // Create a velocity moving the left.
                for (int i = 0; i < 10; i++)
                {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<EyeprojGlow2>(), NPC.damage / 3, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                }
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
                PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 12f, 30, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
                const int Repeats = 80;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int d = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 4.9f;
                    Main.dust[d].scale *= (float)Main.rand.Next(100, 170) * 0.014f;
                }
                if (Main.expertMode || Main.masterMode)
                {
                    Attackers(player);
                    SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/SFX/ShieldBreakIn"));
                }
                radius1 += 10;
                player.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = false;
            }
            // FADE AND LETS START PHASE 2!
            if (NPC.ai[2] > 270 && NPC.ai[2] <= 554)
            {
                NPC.alpha -= 50;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
                NPC.netUpdate = true;
                float d = 2f;
                NPC.velocity = NPC.DirectionTo(player.Center) * d;
                NPC.position += NPC.velocity;
            }
            // PROJECTILES
            if (NPC.ai[3] == 300 || NPC.ai[3] == 310 || NPC.ai[3] == 350 || NPC.ai[3] == 360 || NPC.ai[3] == 400 || NPC.ai[3] == 410)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                int velocity = 1;
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness4>();
                int type2 = ModContent.ProjectileType<EyeprojGlow>();
                for (int k = 0; k < 10; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 14f * velocity, type2, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;


            }
            // STOP
            if (NPC.ai[2] > 470)
            {
                NPC.netUpdate = true;
                float d = 0f;
                NPC.velocity = NPC.DirectionTo(player.Center) * d;
                NPC.position += NPC.velocity;
            }
            // APPEAR AT PLAYER POS AGAIN
            if (NPC.ai[2] == 480)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-90, -300);
                    // Teleport
                }
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                NPC.alpha = 0;
                NPC.dontTakeDamage = false;


                NPC.netUpdate = true;


                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness2Death>();

                Vector2 velocity2 = speed2 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
            }
            // EFFECT
            if (NPC.ai[3] == 485)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.friendly = false;
                NPC.position += NPC.velocity;

                int type2 = ModContent.ProjectileType<Brightness6>();
                int type3 = ModContent.ProjectileType<Brightness7>();
                int type4 = ModContent.ProjectileType<Brightness8>();
                Vector2 offset = NPC.Center;
                Vector2 velocity2 = speed3 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, offset, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type3, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type4, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;


            }
            // APPEAR AT RANDOM POSITIONS
            // FIRST TELEPORT
            if (NPC.ai[2] == 485)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(240, -275);
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();

                Vector2 velocity2 = speed2 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;
                int radius1 = 120;
                const int Repeats = 80;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int d = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 4.9f;
                }

            }
            // EFFECT
            if (NPC.ai[2] == 501)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.friendly = false;
                NPC.position += NPC.velocity;

                int type2 = ModContent.ProjectileType<Brightness6>();
                int type3 = ModContent.ProjectileType<Brightness7>();
                int type4 = ModContent.ProjectileType<Brightness8>();
                Vector2 offset = NPC.Center;
                Vector2 velocity2 = speed3 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, offset, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type3, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type4, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;


            }
            // SECOND TELEPORT
            if (NPC.ai[3] == 500)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-270, 275);
                    // Teleport
                }
                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();

                Vector2 velocity2 = speed2 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;
                int radius1 = 120;
                const int Repeats = 80;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int d = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 4.9f;
                }

            }
            // EFFECT
            if (NPC.ai[2] == 517)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.friendly = false;
                NPC.position += NPC.velocity;

                int type2 = ModContent.ProjectileType<Brightness6>();
                int type3 = ModContent.ProjectileType<Brightness7>();
                int type4 = ModContent.ProjectileType<Brightness8>();
                Vector2 offset = NPC.Center;
                Vector2 velocity2 = speed3 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, offset, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type3, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type4, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;


            }
            // THIRD TELEPORT
            if (NPC.ai[3] == 516)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(-310, 50);
                    // Teleport
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();

                Vector2 velocity2 = speed2 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }

                NPC.netUpdate = true;
                int radius1 = 120;
                const int Repeats = 80;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int d = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 4.9f;
                }

            }
            // EFFECT
            if (NPC.ai[2] == 531)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.friendly = false;
                NPC.position += NPC.velocity;

                int type2 = ModContent.ProjectileType<Brightness6>();
                int type3 = ModContent.ProjectileType<Brightness7>();
                int type4 = ModContent.ProjectileType<Brightness8>();
                Vector2 offset = NPC.Center;
                Vector2 velocity2 = speed3 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, offset, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type3, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, offset, velocity2, type4, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;


            }
            // FOURTH TELEPORT
            if (NPC.ai[3] == 530)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(210, -450);
                    // Teleport
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();
                int type3 = ModContent.ProjectileType<BrightnessDeath>();

                Vector2 velocity2 = speed2 * new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type3, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                if (Main.masterMode)
                {
                    Vector2 launchVelocity = new Vector2(-5, 1); // Create a velocity moving the left.
                    for (int i = 0; i < 10; i++)
                    {
                        // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                        // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                        launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                        // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                        Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<EyeprojGlow2>(), NPC.damage / 3, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    }
                }
                int radius1 = 120;
                const int Repeats = 80;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int d = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 4.9f;
                }
                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }

                NPC.netUpdate = true;
                PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 12f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);

            }
            // MOVE
            if (NPC.ai[2] >= 555 && NPC.ai[2] <= 729)
            {
                NPC.netUpdate = true;
                NPC.alpha = 0;
                float d = 2f;
                NPC.velocity = NPC.DirectionTo(player.Center) * d;
                NPC.position += NPC.velocity;
            }
            // TELEPORT AND PROJECTILES
            if (NPC.ai[3] == 611)
            {
                SoundEngine.PlaySound(SoundID.Item88, NPC.Center);
                NPC.friendly = false;
                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = player.Center + new Vector2(410, -350);
                    // Teleport
                }
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Brightness5>();
                int type3 = ModContent.ProjectileType<BrightnessDeath>();
                int type4 = ModContent.ProjectileType<LazerBeam2>();
                int type5 = ModContent.ProjectileType<Line6>();

                Vector2 velocity2 = new Vector2(0, 0);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type3, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type4, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type5, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                if (Main.masterMode)
                {
                    Vector2 launchVelocity = new Vector2(-5, 1); // Create a velocity moving the left.
                    for (int i = 0; i < 10; i++)
                    {
                        // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                        // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                        launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                        // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                        Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<EyeprojGlow2>(), NPC.damage / 3, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    }
                }

                NPC.dontTakeDamage = false;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }


            }
            // BEAM PROJECTILE when master m,doedoedeeoreor (skill issue))))))))

            if (Main.masterMode)
            {
                if (NPC.ai[2] == 580)
                {
                    var entitySource = NPC.GetSource_FromAI();
                    NPC.friendly = false;
                    NPC.position += NPC.velocity;
                    Vector2 velocity2 = player.Center + player.velocity * 30;
                    Vector2 papa = Vector2.Zero;
                    int type = ModContent.ProjectileType<LazerBeam>();
                    int type2 = ModContent.ProjectileType<Line5>();


                    Projectile.NewProjectile(entitySource, velocity2, papa, type, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    Projectile.NewProjectile(entitySource, velocity2, papa, type2, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                }
            }
            if (NPC.ai[2] == 541)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness>();
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

            }

            // PROJECTILES
            if (NPC.ai[2] == 571 || NPC.ai[2] == 601)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                int velocity = 1;
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness10>();
                int type2 = ModContent.ProjectileType<EyeprojGlow>();
                for (int k = 0; k < 10; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type, damage, 0f, Main.myPlayer);
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 14f * velocity, type2, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                if (Main.masterMode)
                {
                    Attackers(player);
                }

            }
            // PROJECTILES
            if (NPC.ai[3] == 690 || NPC.ai[3] == 700)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                int velocity = 1;
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness10>();
                int type2 = ModContent.ProjectileType<EyeprojGlow>();
                for (int k = 0; k < 10; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 14f * velocity, type2, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;


            }
            // EFFECT
            if (NPC.ai[3] == 730)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness>();
                for (int k = 0; k < 10; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;


            }
            // STOP
            if (NPC.ai[2] >= 730 && NPC.ai[2] <= 759)
            {
                NPC.netUpdate = true;
                Vector2 d = new Vector2(0, 0);
                NPC.velocity = d;
                NPC.position += NPC.velocity;
            }
            // MAKE A LITTLE ROTATION
            if (NPC.ai[2] >= 760 && NPC.ai[2] <= 831)
            {
                NPC.alpha = 0;
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
                QWERTY = player.Center;
                NPC.Center = QWERTY + new Vector2(350, 0).RotatedBy(NPC.ai[2] / 16);

                NPC.dontTakeDamage = false;
            }
            // PROJECTILES
            if (NPC.ai[3] == 770 || NPC.ai[3] == 780 || NPC.ai[3] == 790 || NPC.ai[3] == 800 || NPC.ai[3] == 810 || NPC.ai[3] == 820 || NPC.ai[3] == 830)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                int type2 = ModContent.ProjectileType<EyeprojGlow2>();
                int type3 = ModContent.ProjectileType<Brightness4>();
                for (int k = 0; k < 10; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 2f, type2, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, new Vector2(0, 0), type3, 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            if (NPC.ai[3] >= 820 && NPC.ai[3] <= 839)
            {
                NPC.alpha += 30;
            }
            if (NPC.ai[3] == 840)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                SoundEngine.PlaySound(SoundID.Item72, NPC.Center);
                Vector2 velocity2 = new Vector2(0, 0);
                int type2 = ModContent.ProjectileType<BrightnessDeath>();
                for (int k = 0; k < 10; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                Vector2 velocity = new(0, 0);
                if (Main.expertMode || Main.masterMode)
                {
                    Attackers(player);
                }
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            // APPEAR AT PLAYER POS AGAIN
            if (NPC.ai[2] >= 840 && NPC.ai[2] <= 1250)
            {
                Vector2 qwer = player.Center + new Vector2(-60, -300);
                NPC.friendly = false;

                float range2 = 2500f * 16f; // 100 tiles
                if (NPC.DistanceSQ(player.Center) < range2 * range2)
                {
                    NPC.position = qwer;
                }
                NPC.alpha = 0;
                NPC.dontTakeDamage = false;

            }
            // SOME EFFECTS and explosions
            if (NPC.ai[2] == 851 || NPC.ai[2] == 919 || NPC.ai[2] == 966 || NPC.ai[2] == 1009)
            {
                NPC.friendly = false;
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new(0, 0);
                int type = ModContent.ProjectileType<Brightness7>();
                int type2 = ModContent.ProjectileType<Brightness8>();
                int type3 = ModContent.ProjectileType<EXBEAMHIT>();


                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type2, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type3, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                int velocity2 = 1;
                int type4 = ModContent.ProjectileType<EXBEAM>();
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 12f * velocity2, type4, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            if (Main.masterMode)
            {
                if (NPC.ai[2] == 850 || NPC.ai[2] == 910 || NPC.ai[2] == 980)
                {
                    NPC.friendly = false;
                    NPC.dontTakeDamage = false;
                    var entitySource = NPC.GetSource_FromAI();
                    NPC.position += NPC.velocity;

                    Vector2 velocity = new(0, 0);
                    int type2 = ModContent.ProjectileType<LightningA>();
                    int type3 = ModContent.ProjectileType<LightningB>();
                    int type4 = ModContent.ProjectileType<LightningC>();
                    int type5 = ModContent.ProjectileType<LightningD>();
                    int type6 = ModContent.ProjectileType<LightningE>();

                    Projectile.NewProjectile(entitySource, player.Center, velocity, type2, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    Projectile.NewProjectile(entitySource, player.Center, velocity, type3, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    Projectile.NewProjectile(entitySource, player.Center, velocity, type4, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    Projectile.NewProjectile(entitySource, player.Center, velocity, type5, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    Projectile.NewProjectile(entitySource, player.Center, velocity, type6, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    NPC.netUpdate = true;
                }
            }


            // ARROW EFFECT
            if (NPC.ai[2] == 963)
            {
                NPC.friendly = false;
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();

                NPC.position += NPC.velocity;
                int type2 = ModContent.ProjectileType<Arrows>();

                Vector2 velocity2 = speed2 * new Vector2(3, 5);

                int damage2 = 0;

                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage2, 0f, Main.myPlayer, 0f, NPC.whoAmI);
            }
            if (NPC.ai[3] == 1013 || NPC.ai[3] == 1091 || NPC.ai[3] == 1135)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                SoundEngine.PlaySound(SoundID.Item72, NPC.Center);
                Vector2 offset = player.Center + new Vector2(-700, 0);
                Vector2 vel = new Vector2(10, 0);
                int type2 = ModContent.ProjectileType<EyeprojGlow>();
                Projectile.NewProjectile(entitySource, offset, vel, type2, NPC.damage / 4, 6f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            if (NPC.ai[2] == 1055 || NPC.ai[2] == 1121 || NPC.ai[2] == 1175)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                SoundEngine.PlaySound(SoundID.Item72, NPC.Center);
                Vector2 offset = player.Center + new Vector2(700, 0);
                Vector2 vel = new Vector2(-10, 0);
                int type2 = ModContent.ProjectileType<EyeprojGlow>();
                Projectile.NewProjectile(entitySource, offset, vel, type2, NPC.damage / 4, 6f, Main.myPlayer);
                NPC.netUpdate = true;
            }
            // EXPLOSION
            if (NPC.ai[2] == 1052 || NPC.ai[2] == 1102 || NPC.ai[2] == 1170)
            {
                NPC.friendly = false;
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                NPC.position += NPC.velocity;
                for (int k = 0; k < 20; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                int velocity = 1;
                int type = ModContent.ProjectileType<EXBEAM>();

                int damage = NPC.damage / 4;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 16f * velocity, type, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            // LAZER BEAnS master
            if (Main.masterMode)
            {
                if (NPC.ai[2] == 1072 || NPC.ai[2] == 1132 || NPC.ai[2] == 1201)
                {
                    NPC.friendly = false;
                    NPC.dontTakeDamage = false;
                    var entitySource = NPC.GetSource_FromAI();
                    NPC.friendly = false;
                    NPC.position += NPC.velocity;
                    Vector2 velocity2 = player.Center + player.velocity * 30;
                    Vector2 papa = Vector2.Zero;
                    int type = ModContent.ProjectileType<LazerBeam>();
                    int type2 = ModContent.ProjectileType<Line5>();
                    Projectile.NewProjectile(entitySource, velocity2, papa, type, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    Projectile.NewProjectile(entitySource, velocity2, papa, type2, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                }
            }

            // SOME EFFECTS
            if (NPC.ai[2] == 1039 || NPC.ai[2] == 1089 || NPC.ai[2] == 1158 || NPC.ai[2] == 1216)
            {
                NPC.friendly = false;
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                NPC.position += NPC.velocity;

                Vector2 velocity = new(0, 0);
                int type = ModContent.ProjectileType<Brightness2Death>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            // STOP AND START LAST ATTACK
            if (NPC.ai[2] >= 1260 && NPC.ai[2] <= 1500)
            {
                NPC.netUpdate = true;
                Vector2 d = new Vector2(0, 0);
                NPC.velocity = d;
                NPC.alpha = 0;
            }
            if (NPC.ai[3] >= 1322 && NPC.ai[3] <= 1502)
            {
                const int Repeats = 180;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int r = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[r].noGravity = true;
                    Main.dust[r].velocity *= 0.9f;
                    Main.dust[r].rotation += 1.1f;
                }
                radius1 -= 15;
            }
            if (NPC.ai[3] >= 1321 && NPC.ai[3] <= 1501)
            {
                PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 12f, 6f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
                NPC.netUpdate = true;
                int radius1 = 140;
                int radius2 = 180;
                const int Repeats = 180;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int r = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[r].noGravity = true;
                    Main.dust[r].velocity *= 0.9f;
                    Main.dust[r].rotation += 1.1f;
                    Vector2 position2 = NPC.Center + new Vector2(radius2, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                    int t = Dust.NewDust(position2, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[t].noGravity = true;
                    Main.dust[t].velocity *= 0.9f;
                    Main.dust[t].rotation += 1.1f;
                }
            }
            // LAZOOOOOOOOOOOOOOOORZ
            if (NPC.ai[3] == 1260)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<LazerBeam3>();
                int type2 = ModContent.ProjectileType<LazerBeam4>();
                int type3 = ModContent.ProjectileType<Line3>();
                int type4 = ModContent.ProjectileType<Line4>();
                for (int k = 0; k < 10; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                Projectile.NewProjectile(entitySource, NPC.Center + new Vector2(0, -17), velocity2, type, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center + new Vector2(0, -17), velocity2, type2, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center + new Vector2(0, -17), velocity2, type3, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center + new Vector2(0, -17), velocity2, type4, NPC.damage / 4, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;
                if (Main.expertMode || Main.masterMode)
                {
                    Attackers(player);
                }
            }
            // PROJECTILES ROTATED BT RANDOM WHEN LASER AND WHEN EXPERT OR MASTER
            if (Main.expertMode || Main.masterMode)
            {
                if (NPC.ai[3] == 1330 || NPC.ai[3] == 1340 || NPC.ai[3] == 1350 || NPC.ai[3] == 1360 || NPC.ai[3] == 1370 || NPC.ai[3] == 1380 || NPC.ai[3] == 1390 || NPC.ai[3] == 1400 || NPC.ai[3] == 1410 || NPC.ai[3] == 1420 || NPC.ai[3] == 1440 || NPC.ai[3] == 1460 || NPC.ai[2] == 1500)
                {
                    NPC.dontTakeDamage = true;
                    var entitySource = NPC.GetSource_FromAI();
                    NPC.position += NPC.velocity;
                    SoundEngine.PlaySound(SoundID.Item94, NPC.Center);
                    Vector2 velocity = new Vector2(0, 0);
                    int type = ModContent.ProjectileType<Brightness2Death>();
                    for (int k = 0; k < 20; k++)
                    {
                        int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                        Main.dust[dust].velocity *= 6.0f;
                    }

                    NPC.dontTakeDamage = false;
                    int damage = 0;
                    Vector2 launchVelocity = new Vector2(-12, 1); // Create a velocity moving the left.
                    for (int i = 0; i < 1; i++)
                    {
                        // Every iteration, rotate the newly spawned projectile by random
                        // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                        launchVelocity = launchVelocity.RotatedByRandom(360);

                        // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                        Projectile.NewProjectile(entitySource, NPC.Center, launchVelocity, ModContent.ProjectileType<EyeprojGlow>(), NPC.damage / 3, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                    }
                    Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                    NPC.netUpdate = true;


                }
            }

            // EFFECT
            if (NPC.ai[3] == 1320)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<BrightnessDeath>();
                int type2 = ModContent.ProjectileType<Brightness8>();
                int type3 = ModContent.ProjectileType<Brightness2Death>();

                for (int k = 0; k < 90; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                    Main.dust[dust].velocity *= 9.0f;
                }
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type2, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type3, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
            }
            // EFFECT
            if (NPC.ai[3] == 1520)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness>();
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

                NPC.netUpdate = true;
            }
            // RESET (finnaly oof)................
            if (NPC.ai[3] == 1550)
            {
                NPC.dontTakeDamage = false;
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity2 = new Vector2(0, 0);
                int type = ModContent.ProjectileType<BrightnessDeath>();    
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity2, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                NPC.netUpdate = true;
                NPC.ai[2] = 270;
                NPC.ai[3] = 270;
                NPC.localAI[2] = 270;
                NPC.localAI[3] = 270;
                if (Main.masterMode)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<EnergyConductorMinion>()))
                    {
                        Conductors(player);
                    }     
                }
            }
        }
        int r;
        public void Death(Player player)
        {
            for (int k = 0; k < 1; k++)
            {
                int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 1.5f);
                Main.dust[dust].velocity *= 6.0f;
            }
            r++;
            const int Repeats = 80;
            for (int i = 0; i < Repeats; ++i)
            {
                Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats) * 4);
                int y = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                Main.dust[y].noGravity = true;
                Main.dust[y].velocity *= 4.9f;
            }
            if (r == 1)
            {
                radius1 = 1;
                player.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = true;
                player.GetModPlayer<PenumbraGlobalPlayer>().absolutepos = NPC.Center;
                NPC.alpha = 0;
            }
            if (r < 125)
                radius1 += 4;
            else
            {
                radius1 -= 15;
                if (radius1 < 0)
                {
                    radius1 = 0;
                }
            }

            NPC.ai[0] = 0;
            NPC.ai[1] = 0;
            NPC.ai[2] = 0;
            NPC.ai[3] = 0;
            NPC.localAI[0] = 0;
            NPC.localAI[1] = 0;
            NPC.localAI[2] = 0;
            NPC.localAI[3] = 0;
            NPC.velocity.X = 0f;
            NPC.velocity.Y = 0f;
            float d = 0f;
            NPC.velocity = NPC.DirectionTo(player.Center) * d;
            a++;
            b++;
            #region animation
            if (a == 10 || a == 20 || a == 40 || a == 60 || a == 100 || a == 140)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;
                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness2Death>();
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 8f, 12f, 30, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
            }
            if (b < 158)
            {
                NPC.life = 10;
                NPC.defense = 99999;
                NPC.dontTakeDamage = true;
            }
            if (b == 11 || b == 21 || b == 31 || b == 41 || b == 61 || b == 101 || b == 141)
            {
                var entitySource = NPC.GetSource_FromAI();
                Vector2 offset = NPC.Center + Utils.RandomVector2(Main.rand, 0, 30);
                Vector2 velocity = new Vector2(0, 0);
                int type2 = ModContent.ProjectileType<ExplosionDeath>();
                Projectile.NewProjectile(entitySource, offset, velocity, type2, 0, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                SoundEngine.PlaySound(SoundID.Item89, NPC.Center);
            }

            if (a == 120)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<BrightnessDeath>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

            }
            if (a == 129)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness3>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);

            }
            if (a == 130)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness>();

                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                SoundEngine.PlaySound(SoundID.Item89, NPC.Center);
            }
            if (a == 135)
            {
                var entitySource = NPC.GetSource_FromAI();
                NPC.position += NPC.velocity;

                Vector2 velocity = new Vector2(0, 0);
                int type = ModContent.ProjectileType<Brightness4>();
                NPC.defense = 1;
                NPC.life = 1;
                int damage = 0;
                Projectile.NewProjectile(entitySource, NPC.Center, velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                SoundEngine.PlaySound(SoundID.Item89, NPC.Center);
                to = false;
            }
            if (a == 159)
            {
                NPC.dontTakeDamage = false;
                NPC.defense = 0;
                var entitySource = NPC.GetSource_FromAI();
                int velocity = 1;
                int type = ModContent.ProjectileType<LazerBeam2>();
                int type2 = ModContent.ProjectileType<Line6>();
                NPC.life = 1;
                int damage = 999;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 0f, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 0f, type2, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                if (Main.netMode != NetmodeID.Server)
                {
                    int backGoreType = Mod.Find<ModGore>("Eyegore").Type;
                    int frontGoreType = Mod.Find<ModGore>("Eyegore2").Type;
                    int frontGoreType2 = Mod.Find<ModGore>("Eyegore3").Type;
                    int frontGoreType3 = Mod.Find<ModGore>("Eyegore4").Type;
                    int frontGoreType4 = Mod.Find<ModGore>("Eyegore5").Type;
                    int frontGoreType5 = Mod.Find<ModGore>("Eyegore6").Type;
                    int frontGoreType6 = Mod.Find<ModGore>("Eyegore7").Type;
                    var entitySource2 = NPC.GetSource_Death();
                    for (int i = 0; i < 1; i++)
                    {
                        Gore.NewGore(entitySource2, NPC.position, new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5)), backGoreType);
                        Gore.NewGore(entitySource2, NPC.position, new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5)), frontGoreType);
                        Gore.NewGore(entitySource2, NPC.position, new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5)), frontGoreType2);
                        Gore.NewGore(entitySource2, NPC.position, new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5)), frontGoreType3);
                        Gore.NewGore(entitySource2, NPC.position, new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5)), frontGoreType4);
                        Gore.NewGore(entitySource2, NPC.position, new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5)), frontGoreType5);
                        Gore.NewGore(entitySource2, NPC.position, new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5)), frontGoreType6);
                    }

                }
                player.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = false;
                Main.UseStormEffects = false;
            }
            if (a > 160)
            {
                NPC.dontTakeDamage = false;
                NPC.defense = 0;
                var entitySource = NPC.GetSource_FromAI();
                int velocity = 1;
                int type = ModContent.ProjectileType<EyeprojKill>();
                int damage = 99999;
                Projectile.NewProjectile(entitySource, NPC.Center, NPC.DirectionTo(player.Center) * 16f * velocity, type, damage, 0f, Main.myPlayer, 0f, NPC.whoAmI);
                PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 22f, 12f, 30, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
                const int Repeats2 = 80;
                for (int i = 0; i < Repeats; ++i)
                {
                    Vector2 position = NPC.Center + new Vector2(radius1, 0).RotatedBy((i * MathHelper.PiOver2 / Repeats2) * 4);
                    int r = Dust.NewDust(position, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[r].noGravity = true;
                    Main.dust[r].velocity *= 4.9f;
                }
                radius1 += 10;
            }
            NPC.netUpdate = true;
            #endregion
        }
       
        public static int Minion()
        {
            int count = 1;
            if (Main.getGoodWorld)
            {
                count += 1;
            }
            return count;
        }
        public bool doodo = false;
        public void Conductors(Player player)
        {
            if (SpawnedMinions)
            {
                SpawnedMinions = false;
                // No point executing the code in this method again
                return;
            }

            SpawnedMinions = true;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Because we want to spawn minions, and minions are NPCs, we have to do this on the server (or singleplayer, "!= NetmodeID.MultiplayerClient" covers both)
                // This means we also have to sync it after we spawned and set up the minion
                return;
            }
            var entitySource = NPC.GetSource_FromAI();
            int count = Minion();
            for (int i = 0; i < count; i++)
            {
                int index = NPC.NewNPC(entitySource, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<EnergyConductorMinion>(), NPC.whoAmI);
                NPC minionNPC = Main.npc[index];

                if (minionNPC.active && minionNPC.ModNPC is EnergyConductorMinion minion)
                {
                    // This checks if our spawned NPC is indeed the minion, and casts it so we can access its variables
                    minion.ParentIndex = NPC.whoAmI; // Let the minion know who the "parent" is
                    minion.PositionIndex = i; // Give it the iteration index so each minion has a separate one, used for movement
                }
                // Finally, syncing, only sync on server and if the NPC actually exists (Main.maxNPCs is the index of a dummy NPC, there is no point syncing it)
                if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: index);
                }
            }
        }
        public void Attackers(Player player)
        {
            if (SpawnedMinions2)
            {
                SpawnedMinions2 = false;
                // No point executing the code in this method again
                return;
            }

            SpawnedMinions2 = true;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Because we want to spawn minions, and minions are NPCs, we have to do this on the server (or singleplayer, "!= NetmodeID.MultiplayerClient" covers both)
                // This means we also have to sync it after we spawned and set up the minion
                return;
            }
            var entitySource = NPC.GetSource_FromAI();
            int count = Minion();
            for (int i = 0; i < count; i++)
            {
                int index = NPC.NewNPC(entitySource, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<EnergyConductorMinion2>(), NPC.whoAmI);
                NPC minionNPC = Main.npc[index];

                if (minionNPC.active && minionNPC.ModNPC is EnergyConductorMinion2 minion)
                {
                    // This checks if our spawned NPC is indeed the minion, and casts it so we can access its variables
                    minion.ParentIndex = NPC.whoAmI; // Let the minion know who the "parent" is
                    minion.PositionIndex = i; // Give it the iteration index so each minion has a separate one, used for movement
                }

                // Finally, syncing, only sync on server and if the NPC actually exists (Main.maxNPCs is the index of a dummy NPC, there is no point syncing it)
                if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: index);
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor) //PreDraw for trails
        {
            if (!NPC.IsABestiaryIconDummy)
            {
                Main.instance.LoadProjectile(NPC.type);
                Texture2D texture = TextureAssets.Npc[NPC.type].Value;
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, NPC.gfxOffY);
                    Color color = NPC.GetAlpha(lightColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                    color.A = 0;
                    if (NPC.alpha < 200)
                        Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.oldRot[k], drawOrigin, NPC.scale, SpriteEffects.None, 0);
                }
 
            }

            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glowMask = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;
            Texture2D shield = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Shield").Value;
            var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new Vector2(shield.Width * 0.5f, NPC.height * 0.5f);
            if (!NPC.IsABestiaryIconDummy)
            {
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    if (NPC.alpha < 50)
                    {
                        spriteBatch.Draw(glowMask, NPC.Center - screenPos, NPC.frame, PenumbraMod.Eyestorm, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
                    }
                    
                    if (NPC.AnyNPCs(ModContent.NPCType<EnergyConductorMinion>()))
                    {
                        spriteBatch.Draw(shield, NPC.Center - screenPos, null, PenumbraMod.Storm * 0.04f, NPC.rotation, drawOrigin, NPC.scale, effects, 0);
                    }
                }
            }

        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Finally, we can add additional drops. Many Zombie variants have their own unique drops: https://terraria.fandom.com/wiki/Zombie
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<EyestormRelic>()));
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<EyestormBag>()));
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            // Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            // Boss masks are spawned with 1/7 chance
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.SandBlock, 1, 15, 30));
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.GoldBar, 1, 5, 15));
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.SandBoots, 7, 1));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ShockWave>(), 3, 1));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SparkBow>(), 3, 1));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<StaffofEnergy>(), 3, 1));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ChargeGun>(), 3, 1));

            npcLoot.Add(notExpertRule);
            if (NPC.type == NPCID.MoonLordCore && NPC.downedMoonlord)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OmniStaff>(), 2, 1));
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<LowVoltage>(), 120);
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return true;
        }
        public override void OnKill()
        {
            // This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedEyestormBoss, -1);
            Main.NewText("[c/65b6ff:The storm rests...]");
            Main.windSpeedCurrent = 0;
            // Since this hook is only ran in singleplayer and serverside, we would have to sync it manually.
            // Thankfully, vanilla sends the MessageID.WorldData packet if a BOSS was killed automatically, shortly after this hook is ran

            // If your NPC is not a boss and you need to sync the world (which includes ModSystem, check DownedBossSystem), use this code:
            /*
			if (Main.netMode == NetmodeID.Server) {
				NetMessage.SendData(MessageID.WorldData);
			}
			*/
        }
        public override void FindFrame(int frameHeight)
        {
            // This NPC animates with a simple "go from start frame to final frame, and loop back to start frame" rule
            // In this case: First stage: 0-1-2-0-1-2, Second stage: 3-4-5-3-4-5, 5 being "total frame count - 1"
            int startFrame = 0;
            int finalFrame = 4;
            if (NPC.life < NPC.lifeMax / 2)
            {
                startFrame = 5;
                finalFrame = Main.npcFrameCount[NPC.type] - 1;
            }
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
        public override void BossLoot(ref string name, ref int potionType)
        {
            // Here you'd want to change the potion type that drops when the boss is defeated. Because this boss is early pre-hardmode, we keep it unchanged
            // (Lesser Healing Potion). If you wanted to change it, simply write "potionType = ItemID.HealingPotion;" or any other potion type
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement("Ancient relic used to contain the power of a fallen sorcerer that used his magic to ensure rain in the deserts")
            });
        }
        bool n = false;
        int j;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(a);
            writer.Write(b);
            writer.Write(j);
            writer.Write(n);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            a = reader.ReadInt32();
            b = reader.ReadInt32();
            j = reader.ReadInt32();
            n = reader.ReadBoolean();
        }
        public override void HitEffect(NPC.HitInfo hit)
        {

            if (n)
            {
                return;
            }
            if (NPC.life <= 0)
            {
                j++;
                if (j > 5)
                {
                    n = true;
                }
                NPC.life = 5;
                for (int k = 0; k < 15; k++)
                {
                    int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.BlueTorch, NPC.oldVelocity.X * 0f, NPC.oldVelocity.Y * 0f, Scale: 2.2f);
                    Main.dust[dust].velocity *= 6.0f;
                }
                Main.StopRain();
            }
        }
    }
}
