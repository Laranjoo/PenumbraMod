using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Accessories;
using PenumbraMod.Content.Items.Consumables;
using PenumbraMod.Content.Items.ReaperJewels;
using PenumbraMod.Content.NPCs.Bosses.Eyestorm;
using PenumbraMod.Content.Prefixes;
using PenumbraMod.Content.Tiles;
using System.Collections.Generic;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace PenumbraMod.Common
{
    // This file contains ALL global changes from vanilla and stuff
    public class PenumbraGlobals
    {

    }
    public class PenumbraGlobalPlayer : ModPlayer
    {
        public bool sandhuntereff;
        public bool absolutecamera;
        public bool MagebladeCutscene;
        public Vector2 absolutepos;
        Vector2 scrcache;
        private CrystalSlots item = new(); // initialize a new item so that NullReferenceException doesn't occur when creating a new player / loading a player that doesn't have an item already saved


        public override void Load()
        {
            base.Load();
            if (item.Item == new Item())
            {
                item.Item.SetDefaults();
                item.Item.stack = 1;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);

            tag.Add("item", item.Item);
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);

            if (tag.TryGet("item", out Item savedItem))
            {
                item.Item = savedItem;
                var mo = Mod as PenumbraMod;
                mo.Logger.Info(item.Item);
            }
        }
        public float time;
        public bool KillFrags;
        public override void PostUpdate()
        {
            #region MagebladeCutscene
            if (MagebladeCutscene)
            {
                time++;
                if (time == 2)
                {
                    SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Music/MagebladeCutscene")
                    {
                        Volume = 1f,
                        MaxInstances = 1,
                    });
                    Main.soundVolume = 0;
                }

                if (time >= 3 && time <= 250)
                {
                    Main.musicVolume -= 0.01f;
                    if (Main.musicVolume <= 0)
                        Main.musicVolume = 0;

                    Main.ambientVolume -= 0.01f;
                    if (Main.ambientVolume <= 0)
                        Main.ambientVolume = 0;

                    Main.soundVolume += 0.003f;
                    if (Main.soundVolume >= 1)
                        Main.soundVolume = 1;
                }
                if (time == 120)
                {
                    SoundEngine.PlaySound(SoundID.Item117, Player.Center);
                    SoundEngine.PlaySound(SoundID.Item30, Player.Center);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<PieceCutscene>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<PieceGlow>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(-150, -150f), Vector2.Zero, ModContent.ProjectileType<Brightness10>(), 0, 0, Player.whoAmI);
                }
                if (time == 180)
                {
                    SoundEngine.PlaySound(SoundID.Item117, Player.Center);
                    SoundEngine.PlaySound(SoundID.Item30, Player.Center);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<PieceCutscene2>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<PieceGlow2>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(-150, 100f), Vector2.Zero, ModContent.ProjectileType<Brightness10>(), 0, 0, Player.whoAmI);
                }
                if (time == 240)
                {
                    SoundEngine.PlaySound(SoundID.Item117, Player.Center);
                    SoundEngine.PlaySound(SoundID.Item30, Player.Center);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<PieceCutscene3>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<PieceGlow3>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(150, -150f), Vector2.Zero, ModContent.ProjectileType<Brightness10>(), 0, 0, Player.whoAmI);
                }
                if (time == 300)
                {
                    SoundEngine.PlaySound(SoundID.Item117, Player.Center);
                    SoundEngine.PlaySound(SoundID.Item30, Player.Center);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<PieceCutscene4>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<PieceGlow4>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + new Vector2(150, 100f), Vector2.Zero, ModContent.ProjectileType<Brightness10>(), 0, 0, Player.whoAmI);
                }
                if (time == 540)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Top + new Vector2(0, -80), Vector2.Zero, ModContent.ProjectileType<PieceGlow5>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Top + new Vector2(0, -80), Vector2.Zero, ModContent.ProjectileType<PieceGlow5>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Top + new Vector2(0, -80), Vector2.Zero, ModContent.ProjectileType<PieceGlow7>(), 0, 0, Player.whoAmI);
                }
              
                if (time == 600)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Top + new Vector2(0, -80), Vector2.Zero, ModContent.ProjectileType<PieceGlow6>(), 0, 0, Player.whoAmI);
                    KillFrags = true;
                    Main.musicVolume = 1;
                    Main.soundVolume = 1;
                    Main.ambientVolume = 1;
                }
            }
            else
            {
                time = 0;
                KillFrags = false;
            }
            #endregion
        }
        public override void OnEnterWorld()
        {

        }
        public override void ResetEffects()
        {
            sandhuntereff = false;
            MagebladeCutscene = false;
            KillFrags = false;
        }
        public override void ModifyScreenPosition()
        {
            Vector2 scrctr = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            if (absolutecamera)
            {
                scrcache = Vector2.Lerp(scrcache, new Vector2(absolutepos.X, absolutepos.Y) - scrctr, 0.1f);
                Main.screenPosition = scrcache;
            }
            else
            {
                scrcache = Main.screenPosition;
            }
            base.ModifyScreenPosition();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (sandhuntereff)
                if (Player.HeldItem.DamageType == GetInstance<ReaperClass>())
                    target.AddBuff(BuffID.Venom, 120);
            if (Player.HasBuff(BuffType<BloodstainedForce>()))
            {
                if (Main.rand.NextBool(2))
                {
                    Player.Heal(1);
                }            
            }
            if (Player.HasBuff(BuffType<BloodHunter>()))
            {
                Player.Heal(1);
            }
            if (Player.HasBuff(BuffType<CorrosiveForce>()))
            {
                target.AddBuff(BuffType<Corrosion>(), 120);
            }
            if (Player.HasBuff(BuffType<PeridotForce>()))
            {
                target.AddBuff(BuffID.Poisoned, 180);
            }
            if (Player.HasBuff(BuffType<TerraForce>()))
            {
                target.AddBuff(BuffID.CursedInferno, 240);
                if (Player.HeldItem.DamageType == GetInstance<ReaperClass>())
                    Player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy += 100;
            }
            if (Player.HasBuff<SpectreForce>())
            {
                if (Main.rand.NextBool(3) && !Player.HasBuff<SoulBoost>())
                {
                    CombatText.NewText(Player.getRect(), Color.LightBlue, (string)PenumbraLocalization.SoulBoosted);
                    Player.AddBuff(BuffType<SoulBoost>(), 180);
                }     
            }
            if (Player.HasBuff<SoulBoost>())
            {
                if (Player.HeldItem.DamageType == GetInstance<ReaperClass>())
                    Player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy += 70;
            }
            if (Player.HasBuff<AquamarineForce>())
            {
                if (Player.HeldItem.DamageType == GetInstance<ReaperClass>())
                    Player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy += 50;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (sandhuntereff)
                if (proj.DamageType.CountsAsClass(GetInstance<ReaperClass>()))
                    target.AddBuff(BuffID.Venom, 120);
        }
    }

    public class PenumbraGlobalNPC : GlobalNPC
    {
        public static int eyeStorm = -1;
    }
    public class PenumbraGlobalTooltips : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return GetInstance<PenumbraConfig>().VanillaChanges;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.DeathSickle)
            {
                tooltips.Add(new(Mod, "", (string)PenumbraLocalization.DeathSickle));
            }
            if (item.type == ItemID.IceSickle)
            {
                tooltips.Add(new(Mod, "", (string)PenumbraLocalization.IceSickle));
            }
            if (item.type == ItemID.BeamSword)
            {
                tooltips.Add(new(Mod, "", (string)PenumbraLocalization.BeamSword));
            }
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<FirstShardOfTheMageblade>())
                   && Main.LocalPlayer.HasItem(ModContent.ItemType<SecondShardOfTheMageblade>())
                   && Main.LocalPlayer.HasItem(ModContent.ItemType<ThirdShardOfTheMageblade>())
                   && Main.LocalPlayer.HasItem(ModContent.ItemType<FourthShardOfTheMageblade>()))
            {
                if (item.type == ItemType<FourthShardOfTheMageblade>()
                   || item.type == ItemType<ThirdShardOfTheMageblade>()
                   || item.type == ItemType<SecondShardOfTheMageblade>()
                   || item.type == ItemType<FirstShardOfTheMageblade>())
                    tooltips.Add(new(Mod, "", (string)PenumbraLocalization.MagebladeShards));
            }
        }
    }

    public class PenumbraGlobalItem : GlobalItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return GetInstance<PenumbraConfig>().VanillaChanges;
        }
        public override bool InstancePerEntity => true;
        /// <summary>
        /// Useful to check if item hit NPC
        /// </summary>
        public bool ItemhitNPC;
        public override void SetDefaults(Item item)
        {
            ItemhitNPC = false;
            if (item.type == ItemID.DeathSickle)
            {
                item.DamageType = GetInstance<ReaperClass>();
                item.useAnimation = 22;
                item.useTime = 22;
                item.shootSpeed = 11f;
            }
            if (item.type == ItemID.BottledWater)
                ItemID.Sets.ShimmerTransformToItem[ItemID.BottledWater] = ItemType<BottledShimmer>(); 

            if (item.type == ItemType<ReaperEmblem>())
                ItemID.Sets.ShimmerTransformToItem[ItemType<ReaperEmblem>()] = ItemID.WarriorEmblem;

            if (item.type == ItemID.SummonerEmblem)
                ItemID.Sets.ShimmerTransformToItem[ItemID.SummonerEmblem] = ItemType<ReaperEmblem>();

            if (item.type == ItemID.BeamSword)
            {
                item.useAnimation = 21;
                item.useTime = 21;
                item.shootSpeed = 14f;
                item.shoot = ProjectileType<BeamSwordProj>();
                item.noUseGraphic = false;
                item.noMelee = false;
            }
            if (item.type == ItemID.IceSickle)
            {
                item.DamageType = GetInstance<ReaperClass>();
                item.shoot = ProjectileType<IceSickle>();
                item.shootSpeed = 14f;
                item.damage = 90;
                item.rare = ItemRarityID.Cyan;
            }
            if (item.type == ItemID.CursedFlames)
            {
                item.shoot = ProjectileType<ShadowFlameProj>();
                item.shootSpeed = 27f;
                item.useTime = 6;
                item.useAnimation = 20;
                item.reuseDelay = 10;
                item.mana = 19;
            }
            if (item.type == ItemID.Gladius)
            {
                item.shoot = ProjectileType<GladiusRework>();
                item.shootSpeed = 7f;
                item.useTime = 6;
                item.useAnimation = 14;
                item.reuseDelay = 10;
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.DeathSickle)
            {
                if (player.HasBuff(BuffType<ReaperControl>()))
                {
                    player.AddBuff(BuffType<DeathSpeed>(), 360);
                }
            }
            if (item.type == ItemType<LeadScythe>())
            {
                if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
                {
                    SoundEngine.PlaySound(SoundID.Item37, player.position);
                    player.AddBuff(ModContent.BuffType<LeadForce>(), 899999);
                }
            }
            if (item.type == ItemType<CrimsonScythe>())
            {
                if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
                {
                    player.AddBuff(ModContent.BuffType<BloodHunter>(), 600);
                }
            }

            if (player.HasBuff(BuffType<StunnedNPC>()))
                return false;
            
                return true;
        }       
        public override void HoldItem(Item item, Player player)
        {
            if (player.HasBuff(BuffID.ShadowDodge))
            {
                if (player.HeldItem?.type == ItemID.BeamSword)
                {
                    player.statDefense += 10;
                }
            }
        }
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ItemhitNPC = true;
            target.AddBuff(BuffType<hitef>(), 10);
            if (target.type != NPCID.TargetDummy)
            {
                if (hit.DamageType == GetInstance<ReaperClass>())
                {
                    if (player.HeldItem.prefix == PrefixType<DeathlyPrefix>())
                        player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy += 50;
                    if (player.HeldItem.prefix == PrefixType<DarkeningPrefix>())
                        player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy += 40;
                }
            }      
        }
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            ItemhitNPC = true;
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (item.type == ItemID.BeamSword)
            {
                return true;
            }
            return false;
        }
        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            if (player.HasBuff(BuffType<DeathSpeed>()))
            {
                if (item.type == ItemID.DeathSickle)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.PurpleTorch);
                    }
                }
            }

            if (item.type == ItemID.IceSickle)
            {
                if (Main.rand.NextBool(8))
                {
                    Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.BlueTorch);
                }
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ItemID.IceSickle)
            {
                item.shoot = ProjectileType<IceSickle>();
                if (player.HasBuff(BuffType<ReaperControl>()))
                {
                    item.shootSpeed = 18f;
                    const int NumProjectiles = 3;
                    for (int i = 0; i < NumProjectiles; i++)
                    {
                        Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(40));
                        // Decrease velocity randomly for nicer visuals.
                        newVelocity *= 1f - Main.rand.NextFloat(0.3f);
                        // Create a projectile.
                        Projectile.NewProjectileDirect(source, position, newVelocity, ProjectileType<IceSickleExplosion>(), damage, knockback, player.whoAmI);

                    }
                    return false;
                }
            }
            if (item.type == ItemID.Gladius)
            {
                item.shoot = ProjectileType<GladiusRework>();
            }
            if (item.type == ItemID.BeamSword)
            {
                if (player.altFunctionUse == 2)
                {
                    const int NumProjectiles = 1;
                    for (int i = 0; i < NumProjectiles; i++)
                        Projectile.NewProjectileDirect(source, position, player.DirectionTo(Main.MouseWorld) * 2f, ProjectileType<BeamSwordShield>(), 100, knockback, player.whoAmI);
                    item.noUseGraphic = true;
                    item.noMelee = true;
                    item.useStyle = ItemUseStyleID.Shoot;
                }
                else
                {
                    Projectile.NewProjectileDirect(source, position, player.DirectionTo(Main.MouseWorld) * 14f, ProjectileType<BeamSwordProj>(), 70, knockback, player.whoAmI);
                    item.noUseGraphic = false;
                    item.noMelee = false;
                    item.useStyle = ItemUseStyleID.Swing;
                }
                return false;
            }
            if (player.HasBuff(BuffType<DeathSpeed>()))
            {
                if (item.type == ItemID.DeathSickle)
                {
                    Projectile.NewProjectileDirect(source, position, player.DirectionTo(Main.MouseWorld).RotatedBy(10) * -16f, ProjectileID.DeathSickle, damage, knockback, player.whoAmI);
                    Projectile.NewProjectileDirect(source, position, player.DirectionTo(Main.MouseWorld) * 16f, ProjectileID.DeathSickle, damage, knockback, player.whoAmI);
                    Projectile.NewProjectileDirect(source, position, player.DirectionTo(Main.MouseWorld).RotatedBy(-10) * -16f, ProjectileID.DeathSickle, damage, knockback, player.whoAmI);
                    return false;
                }
            }
            return true;

        }
        public override void AddRecipes()
        {
            Recipe r = Recipe.Create(ItemID.IceSickle);
            r.AddIngredient<GlacialChunk>(20);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();
        }
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (item.type == ItemID.CursedFlames)
            {
                spriteBatch.Draw(Request<Texture2D>("PenumbraMod/Content/Items/CursedFlameInv").Value, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0);
                return false;
            }

            return true;
        }
        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (item.type == ItemID.CursedFlames)
            {
                spriteBatch.Draw(Request<Texture2D>("PenumbraMod/Content/Items/CursedFlameInv").Value, item.position - Main.screenPosition, null, lightColor, rotation, new Vector2(0, 0), scale, SpriteEffects.None, 0);
                return false;
            }
            return true;
        }
    }
    public class HitEffect : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return GetInstance<PenumbraConfig>().HitEffect;
        }
        /// <summary>
        /// Useful to check if item hit NPC
        /// </summary>
        public bool ItemhitNPC;
        public override void SetDefaults(Item item)
        {
            ItemhitNPC = false;
        }
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // this exists because for some reason, the code on OnHit2() here wasnt working, so i did this lmao.
            OnHit2(target, player);
            ItemhitNPC = true;
        }
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            ItemhitNPC = true;
        }
        public void OnHit2(NPC npc, Player player)
        {
            if (ItemhitNPC)
            {
                Vector2 velocity = GetInstance<PenumbraConfig>().HitEffectVelocity;
                int NumProjectiles = GetInstance<PenumbraConfig>().HitEffectCount;
                for (int i = 0; i < NumProjectiles; i++)
                {

                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(360));
                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 4f - Main.rand.NextFloat(0.2f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, newVelocity, ProjectileType<HitEffectProj>(), 0, 0, player.whoAmI);

                }
            }
        }
    }
    public class UseTurn : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return GetInstance<PenumbraConfig>().UseTurn;
        }
        // some weapons doenst look that good with useturn and some of the mod "whips" have some visual glitches with it, so, make them dont be affected with useturn config.
        public override void SetDefaults(Item item)
        {
            if (item.type != ItemType<ShockWave>() && item.type != ItemID.Gladius && item.type != ItemID.CopperShortsword && item.type != ItemID.GoldShortsword && item.type != ItemID.IronShortsword && item.type != ItemID.LeadShortsword
                && item.type != ItemID.PlatinumShortsword && item.type != ItemID.SilverShortsword && item.type != ItemID.TinShortsword && item.type != ItemID.TungstenShortsword && item.type != ItemType<CompositeSword>() &&
                item.type != ItemType<Kusarigama>() && item.type != ItemType<BloodReaper>() && item.type != ItemType<CorrosionCannon>() && item.type != ItemType<DeathstrandingScythe>() && item.type != ItemType<TheMageblade>())

            {
                item.useTurn = true;
            }

        }

    }
    /* Removed since vanilla already has that option
    public class AutoReuse : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return GetInstance<PenumbraConfig>().Autouse;
        }
        public override void SetDefaults(Item item)
        {
            if (item.type != ItemType<Glock17>() && item.type != ItemType<NeutronReactor>())
            {
                item.autoReuse = true;
            }

        }

    }*/
    public class PenumbraGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return GetInstance<PenumbraConfig>().VanillaChanges;
        }
        /// <summary>
        /// Useful to check if projectile hit NPC
        /// </summary>
        public bool ProjhitNPC;
        public override void SetDefaults(Projectile projectile)
        {
            ProjhitNPC = false;
            if (projectile.type == ProjectileID.DeathSickle)
            {
                projectile.DamageType = GetInstance<ReaperClass>();
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            if (target.type != NPCID.TargetDummy)
            {
                if (hit.DamageType == GetInstance<ReaperClass>())
                {
                    if (player.HeldItem.prefix == PrefixType<DeathlyPrefix>())
                        player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy += 50;
                    if (player.HeldItem.prefix == PrefixType<DarkeningPrefix>())
                        player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy += 40;
                }
            }
            if (player.HeldItem?.type == ItemType<OmniStaff>())
            {
                if (projectile.type == ProjectileID.CultistBossLightningOrbArc)
                {
                    target.AddBuff(BuffType<HighVoltage>(), 120);
                }
            }
            if (player.HeldItem?.type == ItemID.InfluxWaver)
            {
                if (projectile.type == ProjectileID.InfluxWaver)
                {
                    target.AddBuff(BuffType<MediumVoltage>(), 120);
                }
            }
        }
    }
    public class HitEffectProjectile : PenumbraGlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool IsLoadingEnabled(Mod mod)
        {
            return GetInstance<PenumbraConfig>().HitEffect;
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ProjhitNPC = true;
            target.AddBuff(BuffType<hitef>(), 10);
            Vector2 velocity = GetInstance<PenumbraConfig>().HitEffectVelocity;
            int NumProjectiles = GetInstance<PenumbraConfig>().HitEffectCount;
            Player player = Main.player[projectile.owner];
            for (int i = 0; i < NumProjectiles; i++)
            {

                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(360));
                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 4f - Main.rand.NextFloat(0.2f);
                // Create a projectile.
                Projectile.NewProjectileDirect(Projectile.InheritSource(projectile), target.Center, newVelocity, ProjectileType<HitEffectProj>(), 0, 0, player.whoAmI);

            }
        }

    }
}
