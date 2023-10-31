using PenumbraMod.Content.Dusts;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{
    public class BuffPlayer : ModPlayer
    {
        public bool corrosion;
        public bool lifeRegenDebuff;
        public bool hotslime;
        public bool bleeding;
        public bool MarshmellowEffect;
        public bool Leadforce;
        public bool MeltedArmor;
        public bool aura;
        public bool aura2;
        public bool melting;
        public bool l;
        public bool l2;
        public bool l3;
        public bool reaperef;
        public bool heartbuff;
        public bool liferegen;
        public override void ResetEffects()
        {
            lifeRegenDebuff = false;
            MarshmellowEffect = false;
            Leadforce = false;
            MeltedArmor = false;
            aura = false;
            aura2 = false;
            melting = false;
            hotslime = false;
            l = false;
            l2 = false;
            l3 = false;
            reaperef = false;
            bleeding = false;
            corrosion = false;
            heartbuff = false;
            liferegen = false;
        }

        public override void UpdateLifeRegen()
        {
            if (liferegen)
            {
                Player.lifeRegen += 2;
            }
        }
        public override void UpdateBadLifeRegen()
        {
            if (lifeRegenDebuff)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;

                Player.lifeRegen -= 56;
            }
            if (corrosion)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;

                Player.lifeRegen -= 18;
            }
            if (melting)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;

                Player.lifeRegen -= 16;
            }
            if (bleeding)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;

                Player.lifeRegen -= 24;
            }
            if (l)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;

                Player.lifeRegen -= 10;
            }
            if (l2)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;

                Player.lifeRegen -= 20;
            }
            if (l3)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;

                Player.lifeRegen -= 30;
            }
            if (hotslime)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;

                Player.lifeRegen -= 8;
            }

        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (lifeRegenDebuff && drawInfo.shadow == 0f)
            {
                r = 0.5f;
                g = 0.5f;
                b = 0.5f;
            }
            if (corrosion && drawInfo.shadow == 0f)
            {
                g = 1f;

                if (Main.rand.NextBool(10))
                {
                    int dust = Dust.NewDust(drawInfo.Position, Player.width, Player.height, DustID.GreenTorch, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].rotation += 0.1f;
                    drawInfo.DustCache.Add(dust);
                }
            }
            if (MarshmellowEffect && drawInfo.shadow == 0f)
            {

                if (Main.rand.NextBool(8))
                {
                    int dust = Dust.NewDust(drawInfo.Position, Player.width, Player.height, ModContent.DustType<MarshmellowDust>(), Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].noGravity = false;
                    drawInfo.DustCache.Add(dust);
                }
            }
            if (bleeding && drawInfo.shadow == 0f)
            {

                if (Main.rand.NextBool(8))
                {
                    int dust = Dust.NewDust(drawInfo.Position, Player.width, Player.height, DustID.RedMoss, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 2f;
                    Main.dust[dust].noGravity = false;
                    drawInfo.DustCache.Add(dust);
                }
            }
            if (reaperef && drawInfo.shadow == 0f)
            {
                if (Main.rand.NextBool(2))
                {
                    int dust = Dust.NewDust(drawInfo.Position, Player.width, Player.height, DustID.LavaMoss, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 6f;
                    Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.014f;
                    Main.dust[dust].noGravity = true;
                    drawInfo.DustCache.Add(dust);
                }
            }
            if (Leadforce && drawInfo.shadow == 0f)
            {
                if (Main.rand.NextBool(10))
                {
                    int dust = Dust.NewDust(drawInfo.Position, Player.width, Player.height, DustID.Lead, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 0f;

                    Main.dust[dust].noGravity = false;
                    drawInfo.DustCache.Add(dust);
                }

            }
            if (MeltedArmor && drawInfo.shadow == 0f)
            {
                if (Main.rand.NextBool(2))
                {
                    int dust = Dust.NewDust(drawInfo.Position, Player.width, Player.height, DustID.Lava, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity.Y -= 8f;
                    Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.008f;
                    Main.dust[dust].noGravity = false;
                    drawInfo.DustCache.Add(dust);
                }

            }
            if (melting && drawInfo.shadow == 0f)
            {
                if (Main.rand.NextBool(2))
                {
                    int dust = Dust.NewDust(drawInfo.Position, Player.width, Player.height, DustID.Lava, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 0f;
                    Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.014f;
                    Main.dust[dust].noGravity = false;
                    drawInfo.DustCache.Add(dust);
                }

            }
            if (hotslime && drawInfo.shadow == 0f)
            {

                if (Main.rand.NextBool(9))
                {
                    int dust = Dust.NewDust(drawInfo.Position, Player.width, Player.height, DustID.BlueMoss, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.008f;
                    Main.dust[dust].noGravity = false;
                    drawInfo.DustCache.Add(dust);
                }

            }
            if (aura && drawInfo.shadow == 0f)
            {

                if (Main.rand.NextBool(3))
                {
                    int dust = Dust.NewDust(drawInfo.Position, Player.width, Player.height, DustID.PinkCrystalShard, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.008f;
                    Main.dust[dust].noGravity = false;
                    drawInfo.DustCache.Add(dust);
                }

            }
            if (l2 && drawInfo.shadow == 0f)
            {

                if (Main.rand.NextBool(3))
                {
                    int dust = Dust.NewDust(drawInfo.Position, Player.width, Player.height, DustID.BlueTorch, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 1f;
                    Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.008f;
                    Main.dust[dust].noGravity = false;
                    drawInfo.DustCache.Add(dust);
                }

            }
            if (l3 && drawInfo.shadow == 0f)
            {

                if (Main.rand.NextBool(3))
                {
                    int dust = Dust.NewDust(drawInfo.Position, Player.width, Player.height, DustID.BlueTorch, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 2f;
                    Main.dust[dust].scale = (float)Main.rand.Next(80, 140) * 0.012f;
                    Main.dust[dust].noGravity = false;
                    drawInfo.DustCache.Add(dust);
                }

            }
        }


        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (Leadforce)
            {
                SoundEngine.PlaySound(SoundID.NPCHit42, Player.position);
                Player.ClearBuff(ModContent.BuffType<LeadForce>());
                modifiers.FinalDamage = modifiers.FinalDamage * 0.50f;
                for (int k = 0; k < 30; k++)
                {
                    int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Lead, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 6.0f;
                }
            }
            if (MeltedArmor)
            {
                SoundEngine.PlaySound(SoundID.Item70, Player.position);
                for (int k = 0; k < 30; k++)
                {
                    int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.LavaMoss, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 6.0f;
                }
            }
            if (aura)
            {
                SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Player.position);
                for (int k = 0; k < 10; k++)
                {
                    int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.PinkCrystalShard, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 6.0f;
                }
            }
            if (aura2)
            {
                SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Player.position);
                for (int k = 0; k < 10; k++)
                {
                    int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.PinkCrystalShard, Player.velocity.X * 0f, Player.velocity.Y * 0f);
                    Main.dust[dust].velocity *= 2.0f;
                }
            }
        }

        public override void PostHurt(Player.HurtInfo info)
        {

            if (Leadforce)
            {
                SoundEngine.PlaySound(SoundID.NPCHit42, Player.position);
                Player.ClearBuff(ModContent.BuffType<LeadForce>());

            }
            if (MeltedArmor)
            {
                SoundEngine.PlaySound(SoundID.Item70, Player.position);
            }
            if (aura)
            {
                SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, Player.position);

            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (lifeRegenDebuff)
            {

                if (damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + " cough into death");

                return true;
            }
            if (melting)
            {

                if (damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + "melted until its bones turned into ashes");

                return true;
            }
            if (l)
            {

                if (damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + "got violentely shocked");

                return true;
            }
            if (l2)
            {

                if (damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + "got shocked and burned alive");

                return true;
            }
            if (l3)
            {

                if (damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + "felt like a power pole");

                return true;
            }
            if (bleeding)
            {

                if (damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
                    damageSource = PlayerDeathReason.ByCustomReason(Player.name + "bleed until it turned into a sack of skin");

                return true;
            }
            return true;
        }
    }

}
