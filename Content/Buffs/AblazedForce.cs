using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace PenumbraMod.Content.Buffs
{
    public class AblazedForce : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetKnockback(ModContent.GetInstance<ReaperClass>()) *= 0.07f;
            player.GetAttackSpeed(ModContent.GetInstance<ReaperClass>()) *= 0.10f;
        }    
    }
    class ApplyAblazedDebuff : ModPlayer
    {
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<AblazedForce>()))
                target.AddBuff(BuffID.OnFire, 180);
        }
    }
}
