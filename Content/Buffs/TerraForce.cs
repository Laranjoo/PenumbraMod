using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{
    public class TerraForce : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(ModContent.GetInstance<ReaperClass>()) += 0.09f;
            player.GetArmorPenetration(ModContent.GetInstance<ReaperClass>()) += 3;
            player.GetAttackSpeed(ModContent.GetInstance<ReaperClass>()) += 0.7f;
            player.GetKnockback(ModContent.GetInstance<ReaperClass>()) += 0.10f;
            player.GetCritChance(ModContent.GetInstance<ReaperClass>()) += 0.09f;
            player.GetModPlayer<BuffPlayer>().liferegen = true;
            if (player.HeldItem.DamageType == ModContent.GetInstance<ReaperClass>())
                player.HeldItem.scale = 1.2f;
        }    
    }

    
}
