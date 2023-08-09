using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{
    public class BloodstainedForce : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(ModContent.GetInstance<ReaperClass>()).Flat += 2;
            player.GetArmorPenetration(ModContent.GetInstance<ReaperClass>()) += 4;
        }    
    }

    
}
