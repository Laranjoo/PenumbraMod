using Microsoft.Xna.Framework;
using PenumbraMod.Common.Systems;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.Items.ReaperCrystals;
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
            player.GetCritChance<ReaperClass>() += 1;
            player.GetArmorPenetration<ReaperClass>() += 2;
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

        //2nd slot
        public bool amycryst2 = false;
        public bool emecryst2 = false;
        public bool magcryst2 = false;
        public bool rubycryst2 = false;
        public bool saphcryst2 = false;
        public bool diamcryst2 = false;
        public bool topcryst2 = false;
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
                {
                    amycryst = true;
                    Main.NewText("Activated!");
                }
                   

                if (bar.magcryst)
                {
                    magcryst = true;
                    Main.NewText("Activated!");
                }
                         
            }
            else
            {
                amycryst = false;
                magcryst = false;
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
                {
                    amycryst2 = true;
                }


                if (bar.magcryst2)
                {
                    magcryst2 = true;
                }
                  
            }
            else
            {
                amycryst2 = false;
                magcryst2 = false;
            }


            // ------------

            #endregion

            #region Crystal effects

            // ------------

            if (amycryst)
            {
                Player.AddBuff(BuffType<AmethystForce>(), 10);
            }
            else
            {
            }


            if (amycryst2)
            {
                Player.AddBuff(BuffType<AmethystForce>(), 10);
            }
            else
            {
            }

            // ------------

            if (magcryst)
            {
                Player.AddBuff(BuffType<MagicForce>(), 10);
            }
            else
            {
            }

            if (magcryst2)
            {
                Player.AddBuff(BuffType<MagicForce>(), 10);
            }
            else
            {
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
                        if (Main.LocalPlayer.HeldItem.DamageType == ModContent.GetInstance<ReaperClass>())
                            ReaperEnergy += (int)(Main.LocalPlayer.HeldItem.useTime * (ReaperEnergyMult + 2));
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
                        if (Main.LocalPlayer.HeldItem.DamageType.CountsAsClass(ModContent.GetInstance<ReaperClass>()))
                            ReaperEnergy += (int)(Main.LocalPlayer.HeldItem.useTime * (ReaperEnergyMult + 2));
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