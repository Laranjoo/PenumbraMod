using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Items;
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
            if (player.HeldItem.type == ModContent.ItemType<CorrosiveScythe>())
                player.GetDamage(ModContent.GetInstance<ReaperClass>()) += 0.10f;
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
