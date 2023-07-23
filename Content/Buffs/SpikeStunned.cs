using Microsoft.Xna.Framework;
using PenumbraMod.Common.Base;
using PenumbraMod.Content.Dusts;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static PenumbraMod.Common.PenumbraGlobalItem;

namespace PenumbraMod.Content.Buffs
{
    // This class serves as an example of a debuff that causes constant loss of life
    // See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
    public class SpikeStunned : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spike Stunned"); // Buff display name
            // Description.SetDefault("The spíkes hurt..."); // Buff description
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
        }

        // Allows you to make this buff give certain effects to the given player
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.PenumbraNPCBuff().Vines = true;
        }
    }
    public class hitef : ModBuff
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<PenumbraConfig>().HitEffect;
        }
        public override string Texture => "PenumbraMod/Content/Buffs/DeathSpeed";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
        }

        // Allows you to make this buff give certain effects to the given player
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.PenumbraNPCBuff().hiteff = true;
        }
    }

    public class BuffNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool stunned;
        public bool corrosion;
        public bool hiteff;
        public bool melting;
        public int meltingtime;
        public bool prismlight;
        public int HotSlimeTime;
        public int VinesTime;
        public bool Vines;
        public bool HotSlime;
        public bool DarkMatter;
        public int DarkMatterTime;
        public bool MeltedEx;
        public int prismtime;
        public bool l;
        public bool l2;
        public bool l3;
        public int ltime;
        public int ltime2;
        public int ltime3;
        public bool bleeding;

        #region Debuff Immunities
        public static List<int> Bosses = new()
        {
            NPCID.EyeofCthulhu,
            NPCID.SkeletronHead,
            NPCID.SkeletronHand,
            NPCID.SkeletronPrime,
            NPCID.Plantera,
            NPCID.PlanterasTentacle,
            NPCID.KingSlime,
            NPCID.PirateShip,
            NPCID.Golem,
            NPCID.GolemHead,
            NPCID.GolemFistRight,
            NPCID.GolemFistLeft,
            NPCID.GolemHeadFree,
            NPCID.Everscream,
            NPCID.DukeFishron,
            NPCID.MoonLordCore,
            NPCID.MoonLordHead,
            NPCID.MoonLordLeechBlob,
            NPCID.MoonLordFreeEye,
            NPCID.TheDestroyer,
            NPCID.TheDestroyerBody,
            NPCID.TheDestroyerTail,
            NPCID.Spazmatism,
            NPCID.Retinazer,
            NPCID.EaterofWorldsBody,
            NPCID.EaterofWorldsHead,
            NPCID.EaterofWorldsTail,
            NPCID.BrainofCthulhu,
            NPCID.QueenBee,
            NPCID.QueenSlimeBoss,
            NPCID.IceQueen,
            NPCID.WallofFlesh,
            NPCID.WallofFleshEye,
            NPCID.Deerclops,
            NPCID.EmpressButterfly,
            NPCID.CultistBoss,
            NPCID.MartianSaucer,
            NPCID.MourningWood,
            NPCID.Pumpking,
            NPCID.SantaNK1,
            NPCID.DD2Betsy,
            NPCID.BloodNautilus,
            636,


        };
        public static void AddDebuffImmunity(int npcType, int[] array)
        {
            if (!NPCID.Sets.DebuffImmunitySets.TryGetValue(npcType, out var entry) || entry?.SpecificallyImmuneTo is null)
                return;

            int[] array2 = NPCID.Sets.DebuffImmunitySets[npcType].SpecificallyImmuneTo;
            NPCID.Sets.DebuffImmunitySets[npcType] = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = array2.Concat(array).ToArray()
            };
        }
        public override void SetStaticDefaults()
        {
            for (int i = 0; i < NPCLoader.NPCCount; i++)
            {
                if (Bosses.Contains(i))
                {
                    AddDebuffImmunity(i, new int[] {
                    ModContent.BuffType<StunnedNPC>() });


                }
            }
        }
        #endregion
        public override void ResetEffects(NPC npc)
        {
            stunned = false;
            HotSlime = false;
            Vines = false;
            DarkMatter = false;
            MeltedEx = false;
            prismlight = false;
            melting = false;
            hiteff = false;
            l = false;
            l2 = false;
            l3 = false;
            bleeding = false;
            corrosion = false;
            if (!npc.HasBuff(ModContent.BuffType<SpikeStunned>()))
            {
                Vines = false;
                VinesTime = 0;
            }
            if (!npc.HasBuff(ModContent.BuffType<HotSlime1>()))
            {
                HotSlime = false;
                HotSlimeTime = 0;
            }
            if (!npc.HasBuff(ModContent.BuffType<PrismLightning>()))
            {
                prismlight = false;
                prismtime = 0;
            }
            if (!npc.HasBuff(ModContent.BuffType<Melting>()))
            {
                melting = false;
                meltingtime = 0;
            }
            if (!npc.HasBuff(ModContent.BuffType<LowVoltage>()))
            {
                l = false;
                ltime = 0;
            }
            if (!npc.HasBuff(ModContent.BuffType<MediumVoltage>()))
            {
                l2 = false;
                ltime2 = 0;
            }
            if (!npc.HasBuff(ModContent.BuffType<HighVoltage>()))
            {
                l3 = false;
                ltime3 = 0;
            }
        }


        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (Vines)
            {

                VinesTime++;
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 6;


            }
            if (corrosion)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 18;
            }
            if (bleeding)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 24;
            }
            if (DarkMatter)
            {

                VinesTime++;
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 50;


            }
            if (HotSlime)
            {

                VinesTime++;
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 8;


            }
            if (prismlight)
            {

                prismtime++;
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 15;


            }
            if (melting)
            {

                meltingtime++;
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 16;


            }
            if (l)
            {

                ltime++;
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 10;


            }
            if (l2)
            {

                ltime2++;
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 20;


            }
            if (l3)
            {

                ltime3++;
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 30;


            }
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Vines)
            {
                drawColor = new Color(121, 143, 43);

            }

            if (stunned)
            {
                drawColor = new Color(155, 135, 110);

            }

            if (hiteff)
            {
                drawColor = ModContent.GetInstance<PenumbraConfig>().HitEffectColor;

            }
            if (HotSlime)
            {
                drawColor = new Color(47, 75, 166);
                if (Main.rand.NextBool(6))
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.BlueTorch, 1f, 0f, 0);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].velocity *= 2f;
                    Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.010f;
                }

            }
            if (corrosion)
            {
                drawColor = new Color(50, 201, 65);
                if (Main.rand.NextBool(6))
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.GreenMoss, 0f, 0f, 0);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].rotation += 0.1f;
                    Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.010f;
                }

            }
            if (bleeding)
            {
                drawColor = new Color(255, 0, 0);
                if (Main.rand.NextBool(14))
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.RedMoss, 1f, 0f, 0);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].velocity *= 2f;
                    Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.010f;
                }

            }
            if (DarkMatter)
            {
                drawColor = new Color(28, 28, 28);
                if (Main.rand.NextBool(3))
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<DarkMatter2>(), 1f, 0f, 0);
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].velocity *= 4f;
                    Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.012f;
                }

            }
            if (MeltedEx)
            {

                for (int k = 0; k < 30; k++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.LavaMoss, npc.velocity.X * 0f, npc.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 6.0f;
                }

            }
            if (melting)
            {

                for (int k = 0; k < 10; k++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Lava, npc.velocity.X * 0f, npc.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 0f;
                }

            }
            if (prismlight)
            {

                for (int k = 0; k < 3; k++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.PurpleTorch, npc.velocity.X * 0f, npc.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 0f;
                    int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.BlueTorch, npc.velocity.X * 0f, npc.velocity.Y * 0f);
                    Main.dust[dust2].velocity *= 0f;
                }

            }
            if (l)
            {

                for (int k = 0; k < 2; k++)
                {
                    int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.BlueTorch, npc.velocity.X * 0f, npc.velocity.Y * 0f);
                    Main.dust[dust2].velocity *= 0f;
                }

            }
            if (l2)
            {

                for (int k = 0; k < 3; k++)
                {
                    int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.BlueTorch, npc.velocity.X * 0f, npc.velocity.Y * 0f);
                    Main.dust[dust2].velocity *= 0f;
                }

            }
            if (l3)
            {

                for (int k = 0; k < 4; k++)
                {
                    int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.BlueTorch, npc.velocity.X * 0f, npc.velocity.Y * 0f);
                    Main.dust[dust2].velocity *= 0f;
                }

            }
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if (MeltedEx)
            {
                for (int k = 0; k < 30; k++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.LavaMoss, npc.velocity.X * 0f, npc.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 6.0f;
                }

            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (MeltedEx)
            {
                for (int k = 0; k < 30; k++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.LavaMoss, npc.velocity.X * 0f, npc.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 6.0f;
                }
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (MeltedEx)
            {
                for (int k = 0; k < 30; k++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.LavaMoss, npc.velocity.X * 0f, npc.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 6.0f;
                }

            }
        }
        public override bool PreAI(NPC npc)
        {
            if (stunned)
            {
                if (npc.noGravity && !npc.noTileCollide)
                    npc.velocity.Y += 0.3f;
                npc.position.X = npc.oldPosition.X;
                if (npc.noTileCollide)
                {
                    npc.position.Y = npc.oldPosition.Y;
                    npc.velocity.Y = 0;
                }
                npc.velocity.X = 0;

                npc.frameCounter = 0;
                return false;
            }
            return true;
        }
    }
}
