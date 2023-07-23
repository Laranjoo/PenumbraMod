using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using PenumbraMod.Content.DamageClasses;

namespace PenumbraMod.Content.Buffs
{

    public class MagicForce : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(GetInstance<ReaperClass>()) += 0.05f;
            player.GetAttackSpeed(GetInstance<ReaperClass>()) += 0.10f;
            player.GetCritChance(GetInstance<ReaperClass>()) -= 0.05f;
        }

    }

    
}
