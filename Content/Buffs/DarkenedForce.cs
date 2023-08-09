using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{

    public class DarkenedForce : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(ModContent.GetInstance<ReaperClass>()) += 0.05f;
            player.GetAttackSpeed(ModContent.GetInstance<ReaperClass>()) += 0.15f;
        }

    }
}
