using Microsoft.Xna.Framework;
using PenumbraMod.Common.Systems;
using PenumbraMod.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace PenumbraMod.Content.DamageClasses
{
    public class ReaperClass : DamageClass
    {
        // This is an example damage class designed to demonstrate all the current functionality of the feature and explain how to create one of your own, should you need one.
        // For information about how to apply stat bonuses to specific damage classes, please instead refer to ExampleMod/Content/Items/Accessories/ExampleStatBonusAccessory.
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            // This method lets you make your damage class benefit from other classes' stat bonuses by default, as well as universal stat bonuses.
            // To briefly summarize the two nonstandard damage class names used by DamageClass:
            // Default is, you guessed it, the default damage class. It doesn't scale off of any class-specific stat bonuses or universal stat bonuses.
            // There are a number of items and projectiles that use this, such as thrown waters and the Bone Glove's bones.
            // Generic, on the other hand, scales off of all universal stat bonuses and nothing else; it's the base damage class upon which all others that aren't Default are built.
            if (damageClass == Generic)
                return StatInheritanceData.Full;

            return new StatInheritanceData(
                damageInheritance: 0f,
                critChanceInheritance: 0f,
                attackSpeedInheritance: 0f,
                armorPenInheritance: 0f,
                knockbackInheritance: 0f
            );

        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            // This method allows you to make your damage class benefit from and be able to activate other classes' effects (e.g. Spectre bolts, Magma Stone) based on what returns true.
            // Note that unlike our stat inheritance methods up above, you do not need to account for universal bonuses in this method.
            // For this example, we'll make our class able to activate melee- and magic-specifically effects.
            if (damageClass == Melee)
                return true;

            return false;
        }

        public override void SetDefaultStats(Player player)
        {
            player.GetArmorPenetration<ReaperClass>() += 1;
        }
        // This property lets you decide whether or not your damage class can use standard critical strike calculations.
        // Note that setting it to false will also prevent the critical strike chance tooltip line from being shown.
        // This prevention will overrule anything set by ShowStatTooltipLine, so be careful!
        public override bool UseStandardCritCalcs => true;

    }
    public class ReaperClassDPlayer : ModPlayer
    {
        public int ReaperEnergy;
        public int ReaperEnergyMax;
        public float ReaperEnergyMult;
        public bool FirstSlotActivate = false;
        public bool SecondSlotActivate = false;
        private ReaperUI bar;
        #region Crystals
        // 1st slot
        public bool amycryst = false;
        public bool emecryst = false;
        public bool magcryst = false;
        public bool rubycryst = false;
        public bool saphcryst = false;
        public bool diamcryst = false;
        public bool topcryst = false;
        public bool azucryst = false;
        public bool pricryst = false;
        public bool blood = false;
        public bool abla = false;
        public bool terra = false;
        public bool spec = false;
        public bool darke = false;
        public bool slim = false;
        public bool corr = false;
        public bool ony = false;
        public bool aqua = false;
        public bool peri = false;
        public bool roz = false;

        //2nd slot
        public bool amycryst2 = false;
        public bool emecryst2 = false;
        public bool magcryst2 = false;
        public bool rubycryst2 = false;
        public bool saphcryst2 = false;
        public bool diamcryst2 = false;
        public bool topcryst2 = false;
        public bool azucryst2 = false;
        public bool pricryst2 = false;
        public bool blood2 = false;
        public bool abla2 = false;
        public bool terra2 = false;
        public bool spec2 = false;
        public bool darke2 = false;
        public bool slim2 = false;
        public bool corr2 = false;
        public bool ony2 = false;
        public bool aqua2 = false;
        public bool peri2 = false;
        public bool roz2 = false;

        #endregion
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (ReaperEnergy > 9980)
            {
                if (ReaperClassSystem.ReaperClassKeybind.JustPressed && Player.active)
                {
                    SoundEngine.PlaySound(SoundID.Item113, Player.position);
                    Player.controlUseItem = true;
                    ReaperEnergy = 0;
                }
            }
        }
        public override void Initialize()
        {
            bar = new ReaperUI();
        }
        public override void PreUpdate()
        {
            if (ReaperEnergy >= ReaperEnergyMax)
            {
                ReaperEnergy = ReaperEnergyMax;

            }
            if (ReaperEnergy > 9980)
            {
                if (ReaperClassSystem.ReaperClassKeybind.JustPressed && Player.active)
                {
                    SoundEngine.PlaySound(SoundID.Item113, Player.position);
                    Player.controlUseItem = true;
                    ReaperEnergy = 0;
                    Player.AddBuff(ModContent.BuffType<ReaperControl>(), 5);
                }

            }
            if (ReaperEnergy >= 3000)
            {
                FirstSlotActivate = true;
            }
            else
                FirstSlotActivate = false;

            if (ReaperEnergy >= 7300)
            {
                SecondSlotActivate = true;
            }
            else
                SecondSlotActivate = false;

            #region Crystal check
            // ------------

            if (FirstSlotActivate)
            {
                if (ReaperEnergy <= 3005)
                {
                    Player.AddBuff(ModContent.BuffType<ReaperControlDust>(), 10);
                    SoundEngine.PlaySound(SoundID.Item73, Player.position);
                }
                if (bar.amycryst)
                    amycryst = true;
                //
                if (bar.magcryst)
                    magcryst = true;
                //
                if (bar.azucryst)
                    azucryst = true;
                //
                if (bar.pricryst)
                    pricryst = true;
                //
                if (bar.emecryst)
                    emecryst = true;
                //
                if (bar.diamcryst)
                    diamcryst = true;
                //
                if (bar.topcryst)
                    topcryst = true;
                //
                if (bar.saphcryst)
                    saphcryst = true;
                //
                if (bar.rubycryst)
                    rubycryst = true;
                //
                if (bar.abla)
                    abla = true;
                //
                if (bar.terra)
                    terra = true;
                //
                if (bar.slim)
                    slim = true;
                //
                if (bar.blood)
                    blood = true;
                //
                if (bar.darke)
                    darke = true;
                //
                if (bar.spec)
                    spec = true;
                //
                if (bar.corr)
                    corr = true;
                //
                if (bar.ony)
                    ony = true;
                //
                if (bar.roz)
                    roz = true;
                //
                if (bar.aqua)
                    aqua = true;
                //
                if (bar.peri)
                    peri = true;
                //
            }
            else
            {
                amycryst = false;
                emecryst = false;
                magcryst = false;
                rubycryst = false;
                saphcryst = false;
                diamcryst = false;
                topcryst = false;
                azucryst = false;
                pricryst = false;
                blood = false;
                abla = false;
                terra = false;
                spec = false;
                darke = false;
                slim = false;
                corr = false;
                ony = false;
                peri = false;
                aqua = false;
                roz = false;
            }

            // ------------

            if (SecondSlotActivate)
            {
                if (ReaperEnergy <= 7305)
                {
                    Player.AddBuff(ModContent.BuffType<ReaperControlDust>(), 10);
                    SoundEngine.PlaySound(SoundID.Item73, Player.position);
                }

                if (bar.amycryst2)
                    amycryst2 = true;
                //
                if (bar.magcryst2)
                    magcryst2 = true;
                //
                if (bar.azucryst2)
                    azucryst2 = true;
                //
                if (bar.pricryst2)
                    pricryst2 = true;
                //
                if (bar.emecryst2)
                    emecryst2 = true;
                //
                if (bar.diamcryst2)
                    diamcryst2 = true;
                //
                if (bar.topcryst2)
                    topcryst2 = true;
                //
                if (bar.saphcryst2)
                    saphcryst2 = true;
                //
                if (bar.rubycryst2)
                    rubycryst2 = true;
                //
                if (bar.abla2)
                    abla2 = true;
                //
                if (bar.terra2)
                    terra2 = true;
                //
                if (bar.slim2)
                    slim2 = true;
                //
                if (bar.blood2)
                    blood2 = true;
                //
                if (bar.darke2)
                    darke2 = true;
                //
                if (bar.spec2)
                    spec2 = true;
                //
                if (bar.corr2)
                    corr2 = true;
                //
                if (bar.ony2)
                    ony2 = true;
                //
                if (bar.roz2)
                    roz2 = true;
                //
                if (bar.aqua2)
                    aqua2 = true;
                //
                if (bar.peri2)
                    peri2 = true;
                //
            }
            else
            {
                amycryst2 = false;
                emecryst2 = false;
                magcryst2 = false;
                rubycryst2 = false;
                saphcryst2 = false;
                diamcryst2 = false;
                topcryst2 = false;
                azucryst2 = false;
                pricryst2 = false;
                blood2 = false;
                abla2 = false;
                terra2 = false;
                spec2 = false;
                darke2 = false;
                slim2 = false;
                corr2 = false;
                ony2 = false;
                peri2 = false;
                ony2 = false;
                aqua2 = false;
            }


            // ------------

            #endregion

            #region Crystal effects

            // ------------

            if (amycryst)
            {
                Player.AddBuff(BuffType<AmethystForce>(), 10);
            }

            if (amycryst2)
            {
                Player.AddBuff(BuffType<AmethystForce>(), 10);
            }

            // ------------

            if (magcryst)
            {
                Player.AddBuff(BuffType<MagicForce>(), 10);
            }

            if (magcryst2)
            {
                Player.AddBuff(BuffType<MagicForce>(), 10);
            }

            // ------------

            if (emecryst)
            {
                Player.AddBuff(BuffType<EmeraldForce>(), 10);
            }


            if (emecryst2)
            {
                Player.AddBuff(BuffType<EmeraldForce>(), 10);
            }

            // ------------

            if (pricryst)
            {
                Player.AddBuff(BuffType<PrimeyeForce>(), 10);
            }


            if (pricryst2)
            {
                Player.AddBuff(BuffType<PrimeyeForce>(), 10);
            }

            // ------------

            if (diamcryst)
            {
                Player.AddBuff(BuffType<DiamondForce>(), 10);
            }

            if (diamcryst2)
            {
                Player.AddBuff(BuffType<DiamondForce>(), 10);
            }

            // ------------

            if (saphcryst)
            {
                Player.AddBuff(BuffType<SapphireForce>(), 10);
            }

            if (saphcryst2)
            {
                Player.AddBuff(BuffType<SapphireForce>(), 10);
            }

            // ------------

            if (topcryst)
            {
                Player.AddBuff(BuffType<TopazForce>(), 10);
            }

            if (topcryst2)
            {
                Player.AddBuff(BuffType<TopazForce>(), 10);
            }

            // ------------

            if (rubycryst)
            {
                Player.AddBuff(BuffType<RubyForce>(), 10);
            }

            if (rubycryst2)
            {
                Player.AddBuff(BuffType<RubyForce>(), 10);
            }

            // ------------

            if (blood)
            {
                Player.AddBuff(BuffType<BloodstainedForce>(), 10);
            }

            if (blood2)
            {
                Player.AddBuff(BuffType<BloodstainedForce>(), 10);
            }

            // ------------

            if (darke)
            {
                Player.AddBuff(BuffType<DarkenedForce>(), 10);
            }

            if (darke2)
            {
                Player.AddBuff(BuffType<DarkenedForce>(), 10);
            }

            // ------------

            if (terra)
            {
                Player.AddBuff(BuffType<TerraForce>(), 10);
            }

            if (terra2)
            {
                Player.AddBuff(BuffType<TerraForce>(), 10);
            }

            // ------------

            if (slim)
            {
                Player.AddBuff(BuffType<SlimyForce>(), 10);
            }

            if (slim2)
            {
                Player.AddBuff(BuffType<SlimyForce>(), 10);
            }

            // ------------


            if (abla)
            {
                Player.AddBuff(BuffType<AblazedForce>(), 10);
            }

            if (abla2)
            {
                Player.AddBuff(BuffType<AblazedForce>(), 10);
            }

            // ------------


            if (corr)
            {
                Player.AddBuff(BuffType<CorrosiveForce>(), 10);
            }

            if (corr2)
            {
                Player.AddBuff(BuffType<CorrosiveForce>(), 10);
            }

            // ------------

            if (spec)
            {
                Player.AddBuff(BuffType<SpectreForce>(), 10);
            }

            if (spec2)
            {
                Player.AddBuff(BuffType<SpectreForce>(), 10);
            }

            // ------------

            if (ony)
            {
                Player.AddBuff(BuffType<OnyxForce>(), 10);
            }

            if (ony2)
            {
                Player.AddBuff(BuffType<OnyxForce>(), 10);
            }

            // ------------


            if (roz)
            {
                Player.AddBuff(BuffType<RozeQuartzForce>(), 10);
            }

            if (roz2)
            {
                Player.AddBuff(BuffType<RozeQuartzForce>(), 10);
            }

            // ------------


            if (aqua)
            {
                Player.AddBuff(BuffType<AquamarineForce>(), 10);
            }

            if (aqua2)
            {
                Player.AddBuff(BuffType<AquamarineForce>(), 10);
            }

            // ------------


            if (peri)
            {
                Player.AddBuff(BuffType<PeridotForce>(), 10);
            }

            if (peri2)
            {
                Player.AddBuff(BuffType<PeridotForce>(), 10);
            }

            // ------------
            #endregion

            if (ReaperEnergy > 0 && ReaperEnergy < (int)(ReaperEnergyMax * 0.95f))
                ReaperEnergy -= 1;

        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            #region CrystalsEffects

            if (Player.HasBuff(BuffType<MagicForce>()) && item.DamageType == GetInstance<ReaperClass>())
            {
                if (Main.rand.NextBool(4))
                    Projectile.NewProjectileDirect(source, position, Player.DirectionTo(Main.MouseWorld) * 8f, ProjectileID.BookOfSkullsSkull, damage, knockback, Player.whoAmI);
            }

            // ----------------
            #endregion
            return true;
        }
        public override void ResetEffects()
        {
            ReaperEnergyMax = 10000;
            ReaperEnergyMult = 0f;
        }
        public override void UpdateDead()
        {
            ReaperEnergy = 0;
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type != NPCID.TargetDummy)
            {
                if (ReaperEnergy < ReaperEnergyMax)
                {
                    if (item.DamageType.CountsAsClass(ModContent.GetInstance<ReaperClass>()))
                    {
                        ReaperEnergy += 50;
                    }

                }
            }
            // no idea why here
            if (ReaperEnergy > 9980)
            {
                if (ReaperClassSystem.ReaperClassKeybind.JustPressed && Player.active)
                {
                    Player.controlUseItem = true;
                    ReaperEnergy = 0;
                }
            }
            if (ReaperEnergy > ReaperEnergyMax)
            {
                ReaperEnergy = 10000;
            }

        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type != NPCID.TargetDummy)
            {
                if (ReaperEnergy < ReaperEnergyMax)
                {
                    if (proj.DamageType.CountsAsClass(ModContent.GetInstance<ReaperClass>()))
                    {
                        ReaperEnergy += 50;
                    }
                }

            }
            // no idea 
            if (ReaperEnergy > 9980)
            {
                if (ReaperClassSystem.ReaperClassKeybind.JustPressed && Player.active)
                {
                    Player.controlUseItem = true;
                    ReaperEnergy = 0;
                }
            }
            if (ReaperEnergy > ReaperEnergyMax)
            {
                ReaperEnergy = 10000;

            }

        }
    }
}