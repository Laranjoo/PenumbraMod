using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{

    public class CorrosiveForce : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetArmorPenetration<ReaperClass>() += 5;
        }

    }
    class ApplyCorrosiveDebuff : ModPlayer
    {
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<CorrosiveForce>()))
            {
                target.AddBuff(ModContent.BuffType<Corrosion>(), 180);
            }
        }
    }
}
