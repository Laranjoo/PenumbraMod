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
            player.GetDamage(ModContent.GetInstance<ReaperClass>()) += 0.2f;
            player.GetArmorPenetration(ModContent.GetInstance<ReaperClass>()) += 5;
            player.GetAttackSpeed(ModContent.GetInstance<ReaperClass>()) += 0.15f;
            player.GetKnockback(ModContent.GetInstance<ReaperClass>()) += 0.15f;
            player.GetCritChance(ModContent.GetInstance<ReaperClass>()) += 0.15f;
            if (player.HeldItem.DamageType == ModContent.GetInstance<ReaperClass>())
                player.HeldItem.scale = 1.2f;
        }    
    }

    
}
